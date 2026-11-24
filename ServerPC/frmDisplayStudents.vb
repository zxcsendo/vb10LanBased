Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Windows.Forms

Public Class frmDisplayStudents
    Inherits Form

    Private dgv As DataGridView
    Private lblCount As Label
    Private lblStatus As Label
    Private txtSearch As TextBox

    Public Sub New()
        Me.Text = "CampusSIS · Student Records"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.MinimumSize = New Size(1100, 700)
        Me.Size = New Size(1200, 760)
        Me.BackColor = Theme.PageBg
        Me.Font = New Font("Segoe UI", 10.0F)
        BuildUi()
        AddHandler Me.Shown, Sub() ViewStudents()
    End Sub

    Private Sub BuildUi()
        Dim nav As New Panel()
        nav.Dock = DockStyle.Top
        nav.Height = 64
        nav.BackColor = Theme.Maroon
        Me.Controls.Add(nav)

        Dim brand As New Label()
        brand.Text = "CampusSIS"
        brand.Font = New Font("Segoe UI Semibold", 16.0F, FontStyle.Bold)
        brand.ForeColor = Color.White
        brand.AutoSize = True
        brand.Location = New Point(28, 18)
        brand.BackColor = Color.Transparent
        nav.Controls.Add(brand)

        Dim role As New Label()
        role.Text = "SERVER · Live student directory"
        role.Font = New Font("Segoe UI", 9.5F)
        role.ForeColor = Color.FromArgb(255, 210, 218)
        role.AutoSize = True
        role.Location = New Point(160, 24)
        role.BackColor = Color.Transparent
        nav.Controls.Add(role)

        Dim body As New Panel()
        body.Dock = DockStyle.Fill
        body.Padding = New Padding(36, 24, 36, 28)
        Me.Controls.Add(body)
        body.BringToFront()
        nav.BringToFront()

        Dim heroTitle As Label = Theme.MakeLabel("Student records", True, 22.0F)
        heroTitle.Location = New Point(8, 8)
        body.Controls.Add(heroTitle)

        Dim heroSub As Label = Theme.MakeMuted("Showing students registered from Client PCs over the LAN.")
        heroSub.Location = New Point(10, 42)
        body.Controls.Add(heroSub)

        Dim statCard As New Panel()
        statCard.Location = New Point(8, 78)
        statCard.Size = New Size(220, 78)
        statCard.BackColor = Color.White
        AddHandler statCard.Paint, AddressOf PaintCardBorder
        body.Controls.Add(statCard)

        Dim statLabel As Label = Theme.MakeMuted("TOTAL STUDENTS")
        statLabel.Location = New Point(18, 14)
        statCard.Controls.Add(statLabel)

        lblCount = Theme.MakeLabel("0", True, 22.0F)
        lblCount.ForeColor = Theme.Maroon
        lblCount.Location = New Point(18, 36)
        statCard.Controls.Add(lblCount)

        txtSearch = Theme.MakeTextBox()
        txtSearch.Location = New Point(250, 104)
        txtSearch.Width = 320
        txtSearch.Text = ""
        body.Controls.Add(txtSearch)

        Dim searchCaption As Label = Theme.MakeMuted("Search name / course / number")
        searchCaption.Location = New Point(250, 82)
        body.Controls.Add(searchCaption)

        Dim btnSearch As Button = Theme.MakeGhostButton("Search")
        btnSearch.Location = New Point(580, 100)
        btnSearch.Size = New Size(100, 34)
        AddHandler btnSearch.Click, Sub() ViewStudents(txtSearch.Text.Trim())
        body.Controls.Add(btnSearch)

        Dim btnRefresh As Button = Theme.MakePrimaryButton("Refresh")
        btnRefresh.Location = New Point(690, 100)
        btnRefresh.Size = New Size(120, 34)
        AddHandler btnRefresh.Click, Sub()
                                         txtSearch.Clear()
                                         ViewStudents()
                                     End Sub
        body.Controls.Add(btnRefresh)

        lblStatus = Theme.MakeMuted("")
        lblStatus.Location = New Point(830, 108)
        body.Controls.Add(lblStatus)

        Dim card As New Panel()
        card.Location = New Point(8, 175)
        card.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        card.Size = New Size(body.ClientSize.Width - 16, body.ClientSize.Height - 190)
        card.BackColor = Color.White
        AddHandler card.Paint, AddressOf PaintCardBorder
        body.Controls.Add(card)

        dgv = New DataGridView()
        dgv.Location = New Point(12, 12)
        dgv.Size = New Size(card.Width - 24, card.Height - 24)
        dgv.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        Theme.StyleDataGrid(dgv)
        card.Controls.Add(dgv)

        AddHandler body.Resize, Sub()
                                    card.Size = New Size(body.ClientSize.Width - 16, Math.Max(200, body.ClientSize.Height - 190))
                                End Sub
    End Sub

    Private Sub PaintCardBorder(sender As Object, e As PaintEventArgs)
        Dim p As Panel = DirectCast(sender, Panel)
        Using pen As New Pen(Theme.Border)
            e.Graphics.DrawRectangle(pen, 0, 0, p.Width - 1, p.Height - 1)
        End Using
    End Sub

    Public Sub ViewStudents(Optional keyword As String = "")
        Try
            Using con As SqlConnection = Connection.OpenConnection()
                Dim sql As String =
                    "SELECT StudentID, StudentNumber, FirstName, LastName, MiddleName, Course, YearLevel, Section, Gender, ContactNumber, Email, Address, DateRegistered " &
                    "FROM Students"

                If Not String.IsNullOrWhiteSpace(keyword) Then
                    sql &= " WHERE StudentNumber LIKE @q OR FirstName LIKE @q OR LastName LIKE @q OR Course LIKE @q OR Section LIKE @q"
                End If
                sql &= " ORDER BY DateRegistered DESC, StudentID DESC"

                Using da As New SqlDataAdapter(sql, con)
                    If Not String.IsNullOrWhiteSpace(keyword) Then
                        da.SelectCommand.Parameters.AddWithValue("@q", "%" & keyword & "%")
                    End If

                    Dim dt As New DataTable()
                    da.Fill(dt)
                    dgv.DataSource = dt
                    ApplyHeaders()
                    lblCount.Text = dt.Rows.Count.ToString()
                    lblStatus.ForeColor = Theme.TextMuted
                    lblStatus.Text = "Updated " & DateTime.Now.ToString("h:mm:ss tt")
                End Using
            End Using
        Catch ex As Exception
            lblStatus.ForeColor = Theme.Maroon
            lblStatus.Text = "Load failed"
            MessageBox.Show("Could not load students." & Environment.NewLine & Environment.NewLine &
                            ex.Message & Environment.NewLine & Environment.NewLine &
                            "Check Connection.vb and that CreateDatabase.sql was run on the server.",
                            "Database error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ApplyHeaders()
        SetHeader("StudentID", "ID")
        SetHeader("StudentNumber", "Student No.")
        SetHeader("FirstName", "First Name")
        SetHeader("LastName", "Last Name")
        SetHeader("MiddleName", "Middle Name")
        SetHeader("Course", "Course")
        SetHeader("YearLevel", "Year")
        SetHeader("Section", "Section")
        SetHeader("Gender", "Gender")
        SetHeader("ContactNumber", "Contact")
        SetHeader("Email", "Email")
        SetHeader("Address", "Address")
        SetHeader("DateRegistered", "Registered")
    End Sub

    Private Sub SetHeader(name As String, text As String)
        If dgv.Columns.Contains(name) Then
            dgv.Columns(name).HeaderText = text
        End If
    End Sub
End Class
