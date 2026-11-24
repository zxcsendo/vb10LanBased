Imports System.Drawing
Imports System.Windows.Forms

Module Theme
    Public ReadOnly Maroon As Color = Color.FromArgb(128, 0, 32)
    Public ReadOnly MaroonDark As Color = Color.FromArgb(90, 0, 22)
    Public ReadOnly MaroonSoft As Color = Color.FromArgb(255, 240, 243)
    Public ReadOnly PageBg As Color = Color.FromArgb(248, 246, 247)
    Public ReadOnly CardBg As Color = Color.White
    Public ReadOnly TextDark As Color = Color.FromArgb(28, 28, 30)
    Public ReadOnly TextMuted As Color = Color.FromArgb(110, 110, 118)
    Public ReadOnly Border As Color = Color.FromArgb(230, 224, 226)

    Public Function MakeLabel(text As String, Optional bold As Boolean = False, Optional size As Single = 10.0F) As Label
        Dim lbl As New Label()
        lbl.Text = text
        lbl.AutoSize = True
        lbl.ForeColor = TextDark
        lbl.Font = New Font("Segoe UI", size, If(bold, FontStyle.Bold, FontStyle.Regular))
        lbl.BackColor = Color.Transparent
        Return lbl
    End Function

    Public Function MakeMuted(text As String, Optional size As Single = 9.0F) As Label
        Dim lbl As Label = MakeLabel(text, False, size)
        lbl.ForeColor = TextMuted
        Return lbl
    End Function

    Public Function MakeTextBox() As TextBox
        Dim txt As New TextBox()
        txt.BorderStyle = BorderStyle.FixedSingle
        txt.Font = New Font("Segoe UI", 10.0F)
        txt.BackColor = Color.White
        txt.ForeColor = TextDark
        txt.Height = 28
        Return txt
    End Function

    Public Function MakePrimaryButton(text As String) As Button
        Dim btn As New Button()
        btn.Text = text
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderSize = 0
        btn.BackColor = Maroon
        btn.ForeColor = Color.White
        btn.Font = New Font("Segoe UI Semibold", 10.5F, FontStyle.Bold)
        btn.Cursor = Cursors.Hand
        btn.Height = 42
        AddHandler btn.MouseEnter, Sub() btn.BackColor = MaroonDark
        AddHandler btn.MouseLeave, Sub() btn.BackColor = Maroon
        Return btn
    End Function

    Public Function MakeGhostButton(text As String) As Button
        Dim btn As New Button()
        btn.Text = text
        btn.FlatStyle = FlatStyle.Flat
        btn.FlatAppearance.BorderColor = Border
        btn.FlatAppearance.BorderSize = 1
        btn.BackColor = Color.White
        btn.ForeColor = Maroon
        btn.Font = New Font("Segoe UI Semibold", 10.0F, FontStyle.Bold)
        btn.Cursor = Cursors.Hand
        btn.Height = 42
        Return btn
    End Function

    Public Sub StyleDataGrid(dgv As DataGridView)
        dgv.BackgroundColor = Color.White
        dgv.BorderStyle = BorderStyle.None
        dgv.EnableHeadersVisualStyles = False
        dgv.ColumnHeadersDefaultCellStyle.BackColor = Maroon
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White
        dgv.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI Semibold", 9.5F, FontStyle.Bold)
        dgv.ColumnHeadersDefaultCellStyle.Padding = New Padding(8, 6, 8, 6)
        dgv.ColumnHeadersHeight = 38
        dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        dgv.DefaultCellStyle.Font = New Font("Segoe UI", 9.5F)
        dgv.DefaultCellStyle.ForeColor = TextDark
        dgv.DefaultCellStyle.SelectionBackColor = MaroonSoft
        dgv.DefaultCellStyle.SelectionForeColor = MaroonDark
        dgv.DefaultCellStyle.Padding = New Padding(6, 4, 6, 4)
        dgv.RowTemplate.Height = 34
        dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(252, 250, 250)
        dgv.GridColor = Border
        dgv.RowHeadersVisible = False
        dgv.AllowUserToAddRows = False
        dgv.AllowUserToDeleteRows = False
        dgv.ReadOnly = True
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
    End Sub
End Module
