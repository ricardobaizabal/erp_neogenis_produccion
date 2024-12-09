Imports System.Data.SqlClient
Imports System.Data

Public Class CuentaConcentradora
    Public id As String
    Public nombre As String

    Dim Obj As New DataControl
    Private Function GetParametersSql(ByVal cmd As String) As SqlParameter()
        Dim pcmd As New SqlParameter("@cmd", SqlDbType.Int) With {.Value = cmd}
        Dim pid As New SqlParameter("@id", SqlDbType.Int) With {.Value = id}
        Dim pnombre As New SqlParameter("@nombre", SqlDbType.VarChar) With {.Value = nombre}
        Dim parameters() As SqlParameter = {pcmd, pid, pnombre}
        Return parameters
    End Function

    Public Sub Save()
        Dim cmd As String
        If id = 0 Then
            cmd = 1
        Else
            cmd = 4
        End If
        Obj.ExecProcedureOneWay("pCuentasConcentradoras", 1, GetParametersSql(cmd))
    End Sub

    Public Sub Find(ByVal id As Integer)
        Dim result As DataSet
        result = Obj.FillDataSet("exec pCuentasConcentradoras @cmd=3, @id = " & id)
        For Each row As DataRow In result.Tables(0).Rows
            id = id
            nombre = row("nombre")
        Next
    End Sub

    Public Function GetAll() As DataSet
        Dim result As DataSet
        result = Obj.FillDataSet("exec pCuentasConcentradoras @cmd=2")
        Return result
    End Function

    Public Sub Remove(ByVal id As Integer)
        Obj.FillDataSet("exec pCuentasconcentradoras @cmd=5, @id = " & id)
    End Sub
End Class
