Public Class consignaciones1
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(clienteid, "select id, isnull(razonsocial,'') as razon_social from tblMisClientes where id in (select distinct clienteid from tblConsignacion) order by razonsocial", 0, True)
            ObjCat = Nothing
        End If
    End Sub

    Private Sub reporteGrid_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles reporteGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    e.Item.Cells(6).Text = ds.Tables(0).Compute("sum(cantidad)", "").ToString
                    e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(6).Font.Bold = True
                    '
                    e.Item.Cells(7).Text = ds.Tables(0).Compute("sum(facturado)", "").ToString
                    e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(7).Font.Bold = True
                    '
                    e.Item.Cells(8).Text = ds.Tables(0).Compute("sum(regresado)", "").ToString
                    e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(8).Font.Bold = True
                    '
                    e.Item.Cells(9).Text = ds.Tables(0).Compute("sum(existencia)", "").ToString
                    e.Item.Cells(9).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(9).Font.Bold = True
                    '
                    e.Item.Cells(10).Text = FormatCurrency(ds.Tables(0).Compute("sum(importe)", ""), 2).ToString
                    e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(10).Font.Bold = True
                End If
        End Select
    End Sub

    Private Sub reporteGrid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles reporteGrid.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=26, @clienteid='" & clienteid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=26, @clienteid='" & clienteid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
        ObjData = Nothing
    End Sub

End Class