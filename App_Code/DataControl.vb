Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.VisualBasic
Public Class DataControl
    Private p_mensaje As String = ""
    Private p_conexion As String = ""
    Private parms As String = String.Empty
    Private parmsC As String = String.Empty

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
        cmd.CommandTimeout = 60
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

    Public Sub Catalogo(ByVal combo As Web.UI.WebControls.DropDownList, ByVal sql As String, ByVal sel As String, Optional ByVal todo As Boolean = False)
        '
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlDataAdapter(sql, conn)
        Dim ds As DataSet = New DataSet
        cmd.Fill(ds)
        combo.DataSource = ds
        combo.DataValueField = ds.Tables(0).Columns(0).ColumnName
        combo.DataTextField = ds.Tables(0).Columns(1).ColumnName
        combo.DataBind()
        '
        If todo Then
            combo.Items.Insert(0, New ListItem("--Todos--", "0"))
        Else
            combo.Items.Insert(0, New ListItem("--Seleccione--", "0"))
        End If
        If sel.Length > 0 Then
            combo.SelectedIndex = combo.Items.IndexOf(combo.Items.FindByValue(sel))
        End If
        conn.Close()
        conn.Dispose()
        conn = Nothing
        '
    End Sub

    Public Sub CatalogoStr(ByVal combo As Web.UI.WebControls.DropDownList, ByVal sql As String, ByVal sel As String, Optional ByVal todo As Boolean = False)
        '
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlDataAdapter(sql, conn)
        Dim ds As DataSet = New DataSet
        cmd.Fill(ds)
        combo.DataSource = ds
        combo.DataValueField = ds.Tables(0).Columns(0).ColumnName
        combo.DataTextField = ds.Tables(0).Columns(1).ColumnName
        combo.DataBind()
        '
        If todo Then
            combo.Items.Insert(0, New ListItem("--Todos--", "0"))
        Else
            combo.Items.Insert(0, New ListItem("--Seleccione--", "0"))
        End If
        If sel.Length > 0 Then
            combo.SelectedIndex = combo.Items.IndexOf(combo.Items.FindByValue(sel))
        End If
        conn.Close()
        conn.Dispose()
        conn = Nothing
        '
    End Sub

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

    Public Function FillDatatable(ByVal SQL As String) As DataTable
        Dim conn As New SqlConnection(p_conexion)
        conn.Open()
        Dim cmd As New SqlDataAdapter(SQL, conn)
        cmd.SelectCommand.CommandTimeout = 3600
        Dim dt As DataTable = New DataTable
        cmd.Fill(dt)
        conn.Close()
        conn.Dispose()
        conn = Nothing
        Return dt
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

    Public Sub ExecuteNonQueryWithParams(ByVal spName As String, ByVal params As ArrayList)
        Dim conn As New SqlConnection(p_conexion)
        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            Dim command As New SqlCommand
            command.CommandType = CommandType.StoredProcedure
            command.CommandText = spName
            command.CommandTimeout = 60
            command.Connection = conn

            parms = String.Empty
            parmsC = String.Empty

            For Each param As SqlParameter In params
                command.Parameters.Add(param)
                parms += param.ParameterName.ToString() + "=" + param.SqlValue.ToString() + ","
            Next

            parmsC = parms

            command.ExecuteNonQuery()
            command.Parameters.Clear()
            command.Dispose()

        Catch ex As Exception
            Throw New Exception(" - ExecuteNonQueryWithParams: " + ex.Message.ToString())
        Finally
            If conn.State <> ConnectionState.Closed Then
                conn.Close()
                conn.Dispose()
                conn = Nothing
            End If
        End Try
    End Sub
End Class