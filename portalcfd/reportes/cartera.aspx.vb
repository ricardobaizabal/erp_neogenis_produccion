Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Public Class cartera
    Inherits System.Web.UI.Page
    Private ds As DataSet
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaCatalogos()
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pReporteCobranza @cmd=3, @clienteid='" & clienteid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "'")
        detalleGrid.DataSource = ds
        detalleGrid.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub CargaCatalogos()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(cmbCuentaConcentradora, "select id, nombre  from tblCuentasConcentradoras where isnull(borradorbit,0)=0 order by nombre", 0, True)
        ObjCat.Catalogo(clienteid, "exec pMisClientes @cmd=1, @userid='" & Session("userid").ToString & "'", 0, True)
        ObjCat.Catalogo(sucursalid, "select id, sucursal from tblSucursalCliente where clienteId='" & clienteid.SelectedValue & "' and isnull(borradobit,0)=0 order by clienteId", 0, True)
        If Session("perfilid") = 3 Then
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 and id='" & Session("userid") & "' order by nombre", Session("userid"), True)
            vendedorid.Enabled = False
        Else
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 and perfilid=3 or perfilid=5 order by nombre", 0, True)
        End If
        ObjCat = Nothing
    End Sub

    Private Sub detalleGrid_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles detalleGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    e.Item.Cells(10).Text = FormatCurrency(ds.Tables(0).Compute("sum(descuento)", ""), 2).ToString
                    e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(10).Font.Bold = True
                    '
                    e.Item.Cells(11).Text = FormatCurrency(ds.Tables(0).Compute("sum(descuento)", ""), 2).ToString
                    e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(11).Font.Bold = True
                    '
                    e.Item.Cells(12).Text = FormatCurrency(ds.Tables(0).Compute("sum(iva)", ""), 2).ToString
                    e.Item.Cells(12).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(12).Font.Bold = True
                    '
                    e.Item.Cells(13).Text = FormatCurrency(ds.Tables(0).Compute("sum(pagado)", ""), 2).ToString
                    e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(13).Font.Bold = True
                    '
                    e.Item.Cells(14).Text = FormatCurrency(ds.Tables(0).Compute("sum(saldo)", ""), 2).ToString
                    e.Item.Cells(14).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(14).Font.Bold = True
                    '
                    e.Item.Cells(15).Text = FormatCurrency(ds.Tables(0).Compute("sum(total)", ""), 2).ToString
                    e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(15).Font.Bold = True
                End If
        End Select
    End Sub

    Private Sub detalleGrid_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles detalleGrid.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pReporteCobranza @cmd=3, @clienteid='" & clienteid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "'")
        detalleGrid.DataSource = ds
        ObjData = Nothing
    End Sub

End Class