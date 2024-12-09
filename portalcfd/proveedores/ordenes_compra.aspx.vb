Public Class ordenes_compra
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call MuestraOrdenes()
        End If
    End Sub

    Private Sub btnAddOrder_Click(sender As Object, e As System.EventArgs) Handles btnAddOrder.Click
        Response.Redirect("~/portalcfd/proveedores/agregarorden.aspx")
    End Sub

    Private Sub MuestraOrdenes()
        Dim ObjData As New DataControl
        ordersList.DataSource = ObjData.FillDataSet("exec pOrdenCompra @cmd=1")
        ordersList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub ordersList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles ordersList.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                Response.Redirect("~/portalcfd/proveedores/editarorden.aspx?id=" & e.CommandArgument.ToString)
            Case "cmdDelete"
                Call EliminaOrden(e.CommandArgument)
                Call MuestraOrdenes()
            Case "cmdReceive"
                Response.Redirect("~/portalcfd/proveedores/recibirorden.aspx?id=" & e.CommandArgument.ToString)
            Case "cmdAddOrdenParcial"
                Response.Redirect("~/portalcfd/proveedores/agregaordenparcial.aspx?id=" & e.CommandArgument.ToString)
        End Select
    End Sub

    Private Sub ordersList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles ordersList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                Dim lnkRecibir As LinkButton = CType(e.Item.FindControl("lnkRecibir"), LinkButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a borrar una orden de compra. ¿Desea continuar?');")
                btnDelete.Enabled = False
                lnkRecibir.Visible = False

                If e.Item.DataItem("estatusid") = 1 Then
                    btnDelete.Enabled = True
                Else
                    btnDelete.ToolTip = "Operación no permitida"
                End If
                If e.Item.DataItem("estatusid") = 2 Then
                    lnkRecibir.Visible = True
                End If
                If e.Item.DataItem("estatusid") = 3 Or e.Item.DataItem("estatusid") = 4 Then
                    btnDelete.Visible = False
                End If
        End Select
    End Sub

    Private Sub ordersList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ordersList.NeedDataSource
        Dim ObjData As New DataControl
        ordersList.DataSource = ObjData.FillDataSet("exec pOrdenCompra @cmd=1")
        ObjData = Nothing
    End Sub

    Private Sub EliminaOrden(ByVal ordenid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=4, @ordenid='" & ordenid.ToString & "'")
        ObjData = Nothing
    End Sub

End Class