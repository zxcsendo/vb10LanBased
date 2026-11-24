Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

Public Class frmRegistrar
    Inherits Form

    Private web As WebBrowser
    Private pollTimer As Timer
    Private ready As Boolean = False
    Private loggedIn As Boolean = False
    Private loadingShell As Boolean = False

    Private filterYear As String = ""
    Private filterCourse As String = ""
    Private filterSection As String = ""
    Private filterSearch As String = ""
    Private chartRange As String = "weeks"

    Public Sub New()
        Me.Text = "Datamex College of Saint Adeline · Registrar"
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = True
        Me.StartPosition = FormStartPosition.Manual
        Me.ClientSize = New Size(1100, 700)
        Me.MaximumSize = Me.Size
        Me.MinimumSize = Me.Size
        Me.BackColor = Color.White

        web = New WebBrowser()
        web.Dock = DockStyle.Fill
        web.ScriptErrorsSuppressed = True
        web.AllowWebBrowserDrop = False
        web.IsWebBrowserContextMenuEnabled = False
        web.WebBrowserShortcutsEnabled = False
        web.ObjectForScripting = New RegistrarPageHost(Me)
        Me.Controls.Add(web)

        pollTimer = New Timer()
        pollTimer.Interval = 1500
        AddHandler pollTimer.Tick, Sub()
                                       If loggedIn Then RefreshData(True)
                                   End Sub

        AddHandler Me.Load, AddressOf OnFormLoad
        AddHandler Me.FormClosed, AddressOf HandleFormClosed
        AddHandler LiveSync.StudentsChanged, AddressOf OnLiveChange
        AddHandler web.DocumentCompleted, AddressOf OnDocReady
    End Sub

    Private Sub OnFormLoad(sender As Object, e As EventArgs)
        WebUi.EnableModernBrowser("ClientPC.exe")
        ShowLoginPage()
    End Sub

    Private Sub ShowLoginPage()
        loggedIn = False
        ready = False
        pollTimer.Stop()
        web.ObjectForScripting = New RegistrarPageHost(Me)
        web.DocumentText = WebUi.RegistrarLoginHtml()
    End Sub

    Private Sub ShowDashboard()
        loadingShell = True
        ready = False
        web.ObjectForScripting = New RegistrarPageHost(Me)
        web.DocumentText = WebUi.RegistrarShellHtml()
    End Sub

    Private Sub OnDocReady(sender As Object, e As WebBrowserDocumentCompletedEventArgs)
        web.ObjectForScripting = New RegistrarPageHost(Me)
        If loadingShell AndAlso loggedIn Then
            ready = True
            loadingShell = False
            RefreshData(False)
            pollTimer.Start()
        End If
    End Sub

    Public Function TryLogin(username As String, password As String) As String
        Dim u As String = If(username Is Nothing, "", username).Trim()
        Dim p As String = If(password Is Nothing, "", password)

        If String.Equals(u, "registrar", StringComparison.OrdinalIgnoreCase) AndAlso p = "admin" Then
            loggedIn = True
            ShowDashboard()
            Return "OK"
        End If

        Return "Invalid username or password."
    End Function

    Public Sub DoLogout()
        Dim ans As DialogResult = MessageBox.Show(
            "Are you sure you want to logout?",
            "Confirm logout",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question)
        If ans <> DialogResult.Yes Then Return

        pollTimer.Stop()
        ready = False
        loggedIn = False
        ShowLoginPage()
    End Sub

    Private Sub OnLiveChange(sender As Object, e As EventArgs)
        If loggedIn AndAlso Me.IsHandleCreated Then
            Me.BeginInvoke(New MethodInvoker(Sub() RefreshData(True)))
        End If
    End Sub

    Private Sub HandleFormClosed(sender As Object, e As FormClosedEventArgs)
        pollTimer.Stop()
        RemoveHandler LiveSync.StudentsChanged, AddressOf OnLiveChange
    End Sub

    Public Sub SetFilters(year As String, course As String, section As String, search As String)
        filterYear = If(year Is Nothing, "", year).Trim()
        filterCourse = If(course Is Nothing, "", course).Trim()
        filterSection = If(section Is Nothing, "", section).Trim()
        filterSearch = If(search Is Nothing, "", search).Trim()
    End Sub

    Public Sub SetChartRange(rangeName As String)
        Dim r As String = If(rangeName Is Nothing, "weeks", rangeName.Trim().ToLower())
        If r <> "weeks" AndAlso r <> "months" AndAlso r <> "years" Then r = "weeks"
        chartRange = r
        If ready AndAlso web.Document IsNot Nothing Then
            SetHtml("chartArea", BuildChartHtml())
            UpdateChartTitle()
        End If
    End Sub

    Private Sub UpdateChartTitle()
        Dim title As String = "Registrations"
        If chartRange = "weeks" Then
            title = "Registrations · last 8 weeks"
        ElseIf chartRange = "months" Then
            title = "Registrations · last 12 months"
        Else
            title = "Registrations · last 5 years"
        End If
        SetText("chartTitle", title)
    End Sub

    Public Sub RefreshData(Optional silent As Boolean = True)
        If Not loggedIn OrElse Not ready OrElse web.Document Is Nothing Then Return

        Dim total As Integer = 0
        Dim visible As Integer = 0
        Dim courses As Integer = 0
        Dim tableHtml As String = BuildTableHtml(visible, total, courses)

        SetHtml("grid", tableHtml)
        SetHtml("chartArea", BuildChartHtml())
        UpdateChartTitle()
        SetText("statTotal", total.ToString())
        SetText("statVisible", visible.ToString())
        SetText("statCourses", courses.ToString())
    End Sub

    Private Function BuildChartHtml() As String
        Try
            Dim labels As New List(Of String)
            Dim display As New List(Of String)
            Dim counts As New Dictionary(Of String, Integer)

            If chartRange = "months" Then
                For i As Integer = 11 To 0 Step -1
                    Dim d As Date = New Date(Date.Today.Year, Date.Today.Month, 1).AddMonths(-i)
                    Dim key As String = d.ToString("yyyy-MM")
                    labels.Add(key)
                    display.Add(d.ToString("MMM yy"))
                    counts(key) = 0
                Next
            ElseIf chartRange = "years" Then
                For i As Integer = 4 To 0 Step -1
                    Dim y As Integer = Date.Today.Year - i
                    Dim key As String = y.ToString()
                    labels.Add(key)
                    display.Add(key)
                    counts(key) = 0
                Next
            Else
                ' weeks: last 8 weeks (week start Monday)
                Dim startOfThisWeek As Date = Date.Today.AddDays(-(CInt(Date.Today.DayOfWeek) + 6) Mod 7)
                For i As Integer = 7 To 0 Step -1
                    Dim ws As Date = startOfThisWeek.AddDays(-7 * i)
                    Dim key As String = ws.ToString("yyyy-MM-dd")
                    labels.Add(key)
                    display.Add(ws.ToString("MMM d"))
                    counts(key) = 0
                Next
            End If

            Using con As SqlConnection = Connection.OpenConnection()
                If chartRange = "months" Then
                    Dim sql As String =
                        "SELECT CONVERT(varchar(7), DateRegistered, 120) AS K, COUNT(*) AS C FROM Students " &
                        "WHERE DateRegistered >= DATEADD(month, -11, DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0)) " &
                        "GROUP BY CONVERT(varchar(7), DateRegistered, 120)"
                    FillChartCounts(con, sql, counts)
                ElseIf chartRange = "years" Then
                    Dim sql As String =
                        "SELECT CAST(YEAR(DateRegistered) AS varchar(4)) AS K, COUNT(*) AS C FROM Students " &
                        "WHERE YEAR(DateRegistered) >= YEAR(GETDATE()) - 4 GROUP BY YEAR(DateRegistered)"
                    FillChartCounts(con, sql, counts)
                Else
                    Dim startOfThisWeek As Date = Date.Today.AddDays(-(CInt(Date.Today.DayOfWeek) + 6) Mod 7)
                    Dim oldest As Date = startOfThisWeek.AddDays(-7 * 7)
                    Using cmd As New SqlCommand("SELECT CONVERT(date, DateRegistered) AS D, COUNT(*) AS C FROM Students WHERE DateRegistered >= @oldest GROUP BY CONVERT(date, DateRegistered)", con)
                        cmd.Parameters.AddWithValue("@oldest", oldest)
                        Using rdr As SqlDataReader = cmd.ExecuteReader()
                            While rdr.Read()
                                Dim day As Date = Convert.ToDateTime(rdr("D"))
                                Dim offset As Integer = (CInt(day.DayOfWeek) + 6) Mod 7
                                Dim weekStart As String = day.AddDays(-offset).ToString("yyyy-MM-dd")
                                If counts.ContainsKey(weekStart) Then
                                    counts(weekStart) = counts(weekStart) + Convert.ToInt32(rdr("C"))
                                End If
                            End While
                        End Using
                    End Using
                End If
            End Using

            Dim maxVal As Integer = 1
            For Each k As String In labels
                If counts(k) > maxVal Then maxVal = counts(k)
            Next

            Dim w As Integer = 680
            Dim h As Integer = 190
            Dim padL As Integer = 34
            Dim padR As Integer = 14
            Dim padT As Integer = 18
            Dim padB As Integer = 34
            Dim plotW As Double = w - padL - padR
            Dim plotH As Double = h - padT - padB
            Dim n As Integer = labels.Count
            Dim gap As Double = 8
            Dim barW As Double = Math.Max(8, (plotW - (gap * (n + 1))) / n)

            Dim sb As New StringBuilder()
            sb.Append("<svg class='chart-svg' viewBox='0 0 ").Append(w).Append(" ").Append(h).Append("' preserveAspectRatio='xMidYMid meet' width='100%' height='190'>")
            sb.Append("<line x1='").Append(padL).Append("' y1='").Append(padT).Append("' x2='").Append(padL).Append("' y2='").Append(padT + plotH).Append("' stroke='#e3d6db' stroke-width='1'/>")
            sb.Append("<line x1='").Append(padL).Append("' y1='").Append(padT + plotH).Append("' x2='").Append(padL + plotW).Append("' y2='").Append(padT + plotH).Append("' stroke='#e3d6db' stroke-width='1'/>")

            For i As Integer = 0 To n - 1
                Dim val As Integer = counts(labels(i))
                Dim barH As Double = (val / CDbl(maxVal)) * plotH
                Dim x As Double = padL + gap + (i * (barW + gap))
                Dim y As Double = padT + plotH - barH
                sb.Append("<rect x='").Append(Math.Round(x, 1)).Append("' y='").Append(Math.Round(y, 1)).Append("' width='").Append(Math.Round(barW, 1)).Append("' height='").Append(Math.Round(Math.Max(barH, 0), 1)).Append("' rx='4' fill='#800020' opacity='0.9'/>")
                If val > 0 Then
                    sb.Append("<text x='").Append(Math.Round(x + barW / 2, 1)).Append("' y='").Append(Math.Round(y - 4, 1)).Append("' text-anchor='middle' font-size='10' fill='#800020' font-weight='bold'>").Append(val.ToString()).Append("</text>")
                End If
                sb.Append("<text x='").Append(Math.Round(x + barW / 2, 1)).Append("' y='").Append(h - 10).Append("' text-anchor='middle' font-size='9' fill='#8a7a80'>").Append(WebUi.Esc(display(i))).Append("</text>")
            Next

            sb.Append("</svg>")
            Return sb.ToString()
        Catch ex As Exception
            Return "<div class='empty'>Chart unavailable</div>"
        End Try
    End Function

    Private Sub FillChartCounts(con As SqlConnection, sql As String, counts As Dictionary(Of String, Integer))
        Using cmd As New SqlCommand(sql, con)
            Using rdr As SqlDataReader = cmd.ExecuteReader()
                While rdr.Read()
                    Dim key As String = Convert.ToString(rdr("K"))
                    If counts.ContainsKey(key) Then counts(key) = Convert.ToInt32(rdr("C"))
                End While
            End Using
        End Using
    End Sub

    Private Sub SetHtml(id As String, html As String)
        Dim el As HtmlElement = web.Document.GetElementById(id)
        If el IsNot Nothing Then el.InnerHtml = html
    End Sub

    Private Sub SetText(id As String, text As String)
        Dim el As HtmlElement = web.Document.GetElementById(id)
        If el IsNot Nothing Then el.InnerText = text
    End Sub

    Private Function BuildTableHtml(ByRef visible As Integer, ByRef total As Integer, ByRef courseCount As Integer) As String
        Try
            Using con As SqlConnection = Connection.OpenConnection()
                Using cmdTotal As New SqlCommand("SELECT COUNT(*) FROM Students", con)
                    total = Convert.ToInt32(cmdTotal.ExecuteScalar())
                End Using
                Using cmdCourses As New SqlCommand("SELECT COUNT(DISTINCT Course) FROM Students", con)
                    courseCount = Convert.ToInt32(cmdCourses.ExecuteScalar())
                End Using

                Dim sql As String =
                    "SELECT StudentID, StudentNumber, FirstName, LastName, MiddleName, Course, YearLevel, Section, Gender, ContactNumber, Email, Address, DateRegistered " &
                    "FROM Students WHERE 1=1"

                If filterYear <> "" Then sql &= " AND YearLevel = @year"
                If filterCourse <> "" Then sql &= " AND Course = @course"
                If filterSection <> "" Then sql &= " AND Section LIKE @section"
                If filterSearch <> "" Then
                    sql &= " AND (StudentNumber LIKE @q OR FirstName LIKE @q OR LastName LIKE @q OR Email LIKE @q)"
                End If
                sql &= " ORDER BY DateRegistered DESC, StudentID DESC"

                Using cmd As New SqlCommand(sql, con)
                    If filterYear <> "" Then cmd.Parameters.AddWithValue("@year", filterYear)
                    If filterCourse <> "" Then cmd.Parameters.AddWithValue("@course", filterCourse)
                    If filterSection <> "" Then cmd.Parameters.AddWithValue("@section", "%" & filterSection & "%")
                    If filterSearch <> "" Then cmd.Parameters.AddWithValue("@q", "%" & filterSearch & "%")

                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        Dim sb As New StringBuilder()
                        sb.Append("<table class='data'><thead><tr>")
                        sb.Append("<th>ID Number</th><th>Full Name</th><th>Course</th><th>Year</th><th>Section</th><th>Status</th><th>Actions</th>")
                        sb.Append("</tr></thead><tbody>")

                        Dim any As Boolean = False
                        While rdr.Read()
                            any = True
                            visible += 1
                            Dim id As String = Convert.ToString(rdr("StudentID"))
                            Dim fullName As String = (Convert.ToString(rdr("FirstName")) & " " & Convert.ToString(rdr("LastName"))).Trim()
                            sb.Append("<tr>")
                            sb.Append("<td>").Append(WebUi.Esc(Convert.ToString(rdr("StudentNumber")))).Append("</td>")
                            sb.Append("<td>").Append(WebUi.Esc(fullName)).Append("</td>")
                            sb.Append("<td>").Append(WebUi.Esc(Convert.ToString(rdr("Course")))).Append("</td>")
                            sb.Append("<td>").Append(WebUi.Esc(Convert.ToString(rdr("YearLevel")))).Append("</td>")
                            sb.Append("<td>").Append(WebUi.Esc(Convert.ToString(rdr("Section")))).Append("</td>")
                            sb.Append("<td><span class='badge'>Enrolled</span></td>")
                            sb.Append("<td><button class='btn btn-view' onclick='viewStudent(").Append(id).Append(")'>View all</button></td>")
                            sb.Append("</tr>")
                        End While

                        If Not any Then
                            sb.Append("<tr><td colspan='7'><div class='empty'>No matching student records.</div></td></tr>")
                        End If

                        sb.Append("</tbody></table>")
                        Return sb.ToString()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return "<div class='empty'>Could not load students.<br/>" & WebUi.Esc(ex.Message) & "</div>"
        End Try
    End Function

    Public Function BuildDetailHtml(id As String) As String
        Try
            Using con As SqlConnection = Connection.OpenConnection()
                Using cmd As New SqlCommand("SELECT * FROM Students WHERE StudentID = @id", con)
                    cmd.Parameters.AddWithValue("@id", Convert.ToInt32(id))
                    Using rdr As SqlDataReader = cmd.ExecuteReader()
                        If Not rdr.Read() Then
                            Return "<div class='empty'>Student not found.</div>"
                        End If

                        Dim sb As New StringBuilder()
                        sb.Append("<table class='kv'>")
                        AppendKv(sb, "Student No.", Convert.ToString(rdr("StudentNumber")))
                        AppendKv(sb, "Full Name", (Convert.ToString(rdr("FirstName")) & " " & Convert.ToString(rdr("MiddleName")) & " " & Convert.ToString(rdr("LastName"))).Replace("  ", " ").Trim())
                        AppendKv(sb, "Course", Convert.ToString(rdr("Course")))
                        AppendKv(sb, "Year Level", Convert.ToString(rdr("YearLevel")))
                        AppendKv(sb, "Section", Convert.ToString(rdr("Section")))
                        AppendKv(sb, "Gender", Convert.ToString(rdr("Gender")))
                        AppendKv(sb, "Contact", Convert.ToString(rdr("ContactNumber")))
                        AppendKv(sb, "Email", Convert.ToString(rdr("Email")))
                        AppendKv(sb, "Address", Convert.ToString(rdr("Address")))
                        AppendKv(sb, "Date Registered", Convert.ToDateTime(rdr("DateRegistered")).ToString("MMMM d, yyyy h:mm tt"))
                        sb.Append("</table>")

                        Dim full As String = (Convert.ToString(rdr("FirstName")) & " " & Convert.ToString(rdr("LastName"))).Trim()
                        Dim subText As String = Convert.ToString(rdr("StudentNumber")) & " · " & Convert.ToString(rdr("Course"))
                        Dim titleEl As HtmlElement = web.Document.GetElementById("mTitle")
                        If titleEl IsNot Nothing Then titleEl.InnerText = full
                        Dim subEl As HtmlElement = web.Document.GetElementById("mSub")
                        If subEl IsNot Nothing Then subEl.InnerText = subText

                        Return sb.ToString()
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Return "<div class='empty'>" & WebUi.Esc(ex.Message) & "</div>"
        End Try
    End Function

    Private Sub AppendKv(sb As StringBuilder, label As String, value As String)
        sb.Append("<tr><th>").Append(WebUi.Esc(label)).Append("</th><td>")
        sb.Append(WebUi.Esc(If(String.IsNullOrWhiteSpace(value), "—", value)))
        sb.Append("</td></tr>")
    End Sub
End Class
