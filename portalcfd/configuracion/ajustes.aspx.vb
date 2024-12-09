Imports System.Data
Imports System.Data.SqlClient
Public Class ajustes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaDatos()
        End If
    End Sub
    Private Sub CargaDatos()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCliente @cmd=5", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                tipo_cambio.Text = rs("tipo_cambio")
            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try
    End Sub
    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pCliente @cmd=6, @tipo_cambio='" & tipo_cambio.Text & "'")
        ObjData = Nothing
        lblMensaje.Text = "La configuración ha sido guardada."
    End Sub
End Class