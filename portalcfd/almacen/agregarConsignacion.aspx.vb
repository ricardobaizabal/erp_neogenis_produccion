Public Class agregarConsignacion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        If Not IsPostBack Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(almacenid, "select id, nombre from tblAlmacen where id <>4 order by nombre", 0)
            ObjCat.Catalogo(clienteid, "EXEC pMisClientes @cmd=1, @clienteUnionId='" & Session("clienteid") & "' ", 0)
            ObjCat.Catalogo(sucursalid, "EXEC pListarSucursales  @clienteid='" & clienteid.SelectedValue & "'", 0)
            ObjCat = Nothing
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim consignacionId As Long = 0
        Dim ObjData As New DataControl
        consignacionId = ObjData.RunSQLScalarQuery("exec pConsignaciones @cmd=1, @userid='" & Session("userid").ToString & "', @almacenid='" & almacenid.SelectedValue.ToString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @comentario='" & txtComentario.Text & "', @orden_compra='" & txtOrdenCompra.Text & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/almacen/editaconsignacion.aspx?id=" & consignacionId.ToString)
    End Sub

    Private Sub CargaSucursales()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(sucursalid, "EXEC pListarSucursales  @clienteid='" & clienteid.SelectedValue & "'", 0)
        ObjCat = Nothing
    End Sub

    Private Sub clienteid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles clienteid.SelectedIndexChanged
        CargaSucursales()
    End Sub

End Class