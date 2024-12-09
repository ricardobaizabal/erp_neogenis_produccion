Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Public Class DataControl
    Private p_mensaje As String = ""
    Private p_conexion As String = ""

    Sub New()
        p_conexion = ConfigurationManager.ConnectionStrings("conn").ConnectionString
    End Sub

    ReadOnly Property mensaje() As String
        Get
            Return p_mensaje
        End Get
    End Property

    WriteOnly Property conexion() As String
        Set(ByVal value As String)
            p_conexion = value
        End Set
    End Property

    Public Sub RunSQLQuery(ByVal SQL As String)
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlCommand(SQL, conn)

        cmd.ExecuteNonQuery()
        conn.Close()
        conn.Dispose()
        conn = Nothing
    End Sub

    Public Function RunSQLScalarQuery(ByVal SQL As String) As Long
        Dim valor As Long = 0
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlCommand(SQL, conn)
        valor = cmd.ExecuteScalar
        conn.Close()
        conn.Dispose()
        conn = Nothing
        Return valor
    End Function

    Public Function RunSQLScalarQueryDecimal(ByVal SQL As String) As Decimal
        Dim valor As Decimal = 0
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlCommand(SQL, conn)
        valor = cmd.ExecuteScalar
        conn.Close()
        conn.Dispose()
        conn = Nothing
        Return valor
    End Function

    Public Function RunSQLScalarQueryString(ByVal SQL As String) As String
        Dim valor As String = ""
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlCommand(SQL, conn)
        valor = cmd.ExecuteScalar
        conn.Close()
        conn.Dispose()
        conn = Nothing
        Return valor
    End Function





    Public Function FillDataSet(ByVal SQL As String) As DataSet
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlDataAdapter(SQL, conn)
        cmd.SelectCommand.CommandTimeout = 3600
        Dim ds As DataSet = New DataSet
        cmd.Fill(ds)
        conn.Close()
        conn.Dispose()
        conn = Nothing
        Return ds
    End Function

    Public Sub Sentence(ByVal _sentence_ As String, ByVal type As Integer, ByVal param As ArrayList)
        Dim conn As New SqlConnection(p_conexion)
        Dim command As New SqlCommand
        Try
            conn.Open()
            command.Connection = conn
            If type = 0 Then
                command.CommandType = CommandType.Text
            Else
                command.CommandType = CommandType.StoredProcedure
            End If
            command.CommandText = _sentence_
            For cont As Integer = 0 To param.Count - 1
                command.Parameters.Add(param(cont))
            Next
            command.ExecuteNonQuery()
            command.Parameters.Clear()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Public Function SentenceScalarLong(ByVal _sentence_ As String, ByVal type As Integer, ByVal param As ArrayList) As Long
        Dim valor As Long = 0
        Dim conn As New SqlConnection(p_conexion)
        Dim command As New SqlCommand
        Try
            conn.Open()
            command.Connection = conn
            If type = 0 Then
                command.CommandType = CommandType.Text
            Else
                command.CommandType = CommandType.StoredProcedure
            End If
            command.CommandText = _sentence_
            For cont As Integer = 0 To param.Count - 1
                command.Parameters.Add(param(cont))
            Next
            valor = command.ExecuteScalar
            command.Parameters.Clear()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
        End Try
        Return valor
    End Function
    Public Function ExecProcedureOneWay(ByVal _name As String, ByVal type As Integer, ByVal param() As SqlParameter) As String
        Dim valor As String = ""
        Dim conn As New SqlConnection(p_conexion)
        Dim command As New SqlCommand
        Try
            conn.Open()
            command.Connection = conn
            If type = 0 Then
                command.CommandType = CommandType.Text
            Else
                command.CommandType = CommandType.StoredProcedure
            End If
            command.CommandText = _name
            command.Parameters.AddRange(param)
            command.ExecuteScalar()
            command.Parameters.Clear()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
        End Try
        Return valor
    End Function
    Public Function ExecProcedureTwoWay(ByVal _name As String, ByVal type As Integer, ByVal param() As SqlParameter) As String
        Dim valor As String = ""
        Dim conn As New SqlConnection(p_conexion)
        Dim command As New SqlCommand
        Try
            conn.Open()
            command.Connection = conn
            If type = 0 Then
                command.CommandType = CommandType.Text
            Else
                command.CommandType = CommandType.StoredProcedure
            End If
            command.CommandText = _name
            command.Parameters.AddRange(param)
            valor = command.ExecuteScalar()
            command.Parameters.Clear()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
        End Try
        Return valor
    End Function
End Class