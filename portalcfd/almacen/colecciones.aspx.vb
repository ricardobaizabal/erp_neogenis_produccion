Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class colecciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call muestraColecciones()
            grdColecciones.DataBind()
        End If
    End Sub

    Private Sub grdColecciones_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles grdColecciones.ItemCommand
        Select Case e.CommandName

            Case "cmdEdit"
                EditaColeccion(e.CommandArgument)

            Case "cmdDelete"
                EliminaColeccion(e.CommandArgument)

        End Select
    End Sub

    Private Sub grdColecciones_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles grdColecciones.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Colecciones" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('¿Está seguro que desea borrar esta colección de la base de datos?');")

            End If

        End If
    End Sub

    Private Sub grdColecciones_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles grdColecciones.NeedDataSource
        Call muestraColecciones()
    End Sub

    Private Sub muestraColecciones()
        Dim ds As New DataSet()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("EXEC pColecciones @cmd=1")
        grdColecciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdColecciones.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub EditaColeccion(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pColecciones @cmd=2, @coleccionid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                '
                txtCodigo.Text = rs("codigo")
                txtNombre.Text = rs("nombre")
                '
                panelRegistroColeccion.Visible = True
                '
                InsertOrUpdate.Value = 1
                coleccionID.Value = rs("id")

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub EliminaColeccion(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pColecciones @cmd=3, @coleccionid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelRegistroColeccion.Visible = False

            Response.Redirect("colecciones.aspx", False)

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub btnAgregaColeccion_Click(sender As Object, e As EventArgs) Handles btnAgregaColeccion.Click
        InsertOrUpdate.Value = 0
        txtCodigo.Text = ""
        txtNombre.Text = ""
        panelRegistroColeccion.Visible = True
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        txtCodigo.Text = ""
        txtNombre.Text = ""
        panelRegistroColeccion.Visible = False
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim ObjData As New DataControl
        If InsertOrUpdate.Value = 0 Then
            ObjData.RunSQLQuery("EXEC pColecciones @cmd=4, @codigo='" & txtCodigo.Text.ToString & "', @nombre='" & txtNombre.Text.ToString.ToUpper & "'")
        Else
            ObjData.RunSQLQuery("EXEC pColecciones @cmd=5, @codigo='" & txtCodigo.Text.ToString & "', @nombre='" & txtNombre.Text.ToString.ToUpper & "', @coleccionid='" & coleccionID.Value.ToString & "'")
        End If

        ObjData = Nothing
        Response.Redirect("colecciones.aspx", False)
    End Sub
End Class