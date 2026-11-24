Option Strict On
Option Explicit On

Namespace My
    Partial Friend Class MyApplication
        <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
        Public Sub New()
            MyBase.New(Global.Microsoft.VisualBasic.ApplicationServices.AuthenticationMode.Windows)
            Me.IsSingleInstance = False
            Me.EnableVisualStyles = True
            Me.SaveMySettingsOnExit = True
            Me.ShutDownStyle = Global.Microsoft.VisualBasic.ApplicationServices.ShutdownMode.AfterAllFormsClose
        End Sub

        <Global.System.Diagnostics.DebuggerStepThroughAttribute()> _
        Protected Overrides Sub OnCreateMainForm()
            Dim studentForm As New Global.ClientPC.frmAddStudent()
            Dim registrarForm As New Global.ClientPC.frmRegistrar()

            Dim wa As System.Drawing.Rectangle = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea
            Dim gap As Integer = 16
            Dim totalW As Integer = studentForm.Width + registrarForm.Width + gap
            Dim left As Integer = wa.Left + Math.Max(gap, (wa.Width - totalW) \ 2)
            Dim top As Integer = wa.Top + Math.Max(gap, (wa.Height - studentForm.Height) \ 2)

            studentForm.Location = New System.Drawing.Point(left, top)
            registrarForm.Location = New System.Drawing.Point(left + studentForm.Width + gap, top)

            Me.MainForm = studentForm
            registrarForm.Show()
        End Sub
    End Class
End Namespace
