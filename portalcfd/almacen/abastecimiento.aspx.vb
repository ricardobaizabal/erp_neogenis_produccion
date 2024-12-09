
Partial Class portalcfd_almacen_abastecimiento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call MuestraReporte()
        End If
    End Sub
    Private Sub MuestraReporte()
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pInventario @cmd=1")
        productslist.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub productslist_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productslist.NeedDataSource
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pInventario @cmd=1")
        ObjData = Nothing
    End Sub
End Class
