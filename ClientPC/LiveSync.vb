Imports System

Module LiveSync
    Public Event StudentsChanged As EventHandler

    Public Sub NotifyStudentsChanged()
        RaiseEvent StudentsChanged(Nothing, EventArgs.Empty)
    End Sub
End Module
