Imports System.Drawing
Imports System.Windows.Forms

Public Class frmAddStudent
    Inherits Form

    Private web As WebBrowser

    Public Sub New()
        Me.Text = "Datamex College of Saint Adeline · Student"
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = True
        Me.StartPosition = FormStartPosition.Manual
        Me.ClientSize = New Size(920, 700)
        Me.MaximumSize = Me.Size
        Me.MinimumSize = Me.Size
        Me.BackColor = Color.White

        web = New WebBrowser()
        web.Dock = DockStyle.Fill
        web.ScriptErrorsSuppressed = True
        web.AllowWebBrowserDrop = False
        web.IsWebBrowserContextMenuEnabled = False
        web.WebBrowserShortcutsEnabled = False
        web.ObjectForScripting = New StudentPageHost()
        Me.Controls.Add(web)

        AddHandler Me.Load, AddressOf OnFormLoad
    End Sub

    Private Sub OnFormLoad(sender As Object, e As EventArgs)
        WebUi.EnableModernBrowser("ClientPC.exe")
        web.DocumentText = WebUi.StudentPageHtml()
    End Sub
End Class
