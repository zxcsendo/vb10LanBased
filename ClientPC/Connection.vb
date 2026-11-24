Imports System.Data.SqlClient

Module Connection
    ' ============================================================
    ' LAN SETUP:
    '  - SERVER PC: keep Data Source as .\SQLEXPRESS (or your instance)
    '  - CLIENT PC: change SERVER_IP to the server static IP, e.g.
    '      "Data Source=192.168.1.10\SQLEXPRESS;..."
    '  - Use SQL Auth on LAN if Windows Auth fails across machines
    ' ============================================================

    Public Const SERVER_NAME As String = ".\SQLEXPRESS"
    Public Const DATABASE_NAME As String = "StudentInformationSystem"

    ' Set USE_SQL_AUTH = True for crossover / LAN with SQL login
    Public Const USE_SQL_AUTH As Boolean = False
    Public Const SQL_USER As String = "sis_user"
    Public Const SQL_PASS As String = "Sis@12345"

    Public Function GetConnectionString() As String
        If USE_SQL_AUTH Then
            Return "Data Source=" & SERVER_NAME &
                   ";Initial Catalog=" & DATABASE_NAME &
                   ";User ID=" & SQL_USER &
                   ";Password=" & SQL_PASS &
                   ";Connect Timeout=8;"
        End If

        Return "Data Source=" & SERVER_NAME &
               ";Initial Catalog=" & DATABASE_NAME &
               ";Integrated Security=True;Connect Timeout=8;"
    End Function

    Public Function OpenConnection() As SqlConnection
        Dim con As New SqlConnection(GetConnectionString())
        con.Open()
        Return con
    End Function
End Module
