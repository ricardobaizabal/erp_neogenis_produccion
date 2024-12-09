Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI

Public Class carteraGraf
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaCatalogos()
            Call CargaAcumulado()
            Call MuestraGrafica()
        End If
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

    Private Sub CargaAcumulado()
        panelDetalle.Visible = False
        Dim ObjData As New DataControl
        reporteGrid.DataSource = ObjData.FillDataSet("exec pReporteCobranza @cmd=1, @clienteid='" & clienteid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "'")
        reporteGrid.DataBind()
        ObjData = Nothing
    End Sub

    Protected Sub reporteGrid_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles reporteGrid.ItemCommand
        Select Case e.CommandName
            Case "cmdR1"
                Call MuestraDetalle(1)
            Case "cmdR2"
                Call MuestraDetalle(2)
            Case "cmdR3"
                Call MuestraDetalle(3)
            Case "cmdR4"
                Call MuestraDetalle(4)
            Case "cmdR5"
                Call MuestraDetalle(5)
            Case "cmdR6"
                Call MuestraDetalle(6)
            Case "cmdR7"
                Call MuestraDetalle(7)
        End Select
    End Sub

    Private Sub MuestraDetalle(ByVal rangoId As Integer)
        Session("rangoId") = rangoId
        panelDetalle.Visible = True
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pReporteCobranza @cmd=2, @rangoid='" & rangoId.ToString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "'")
        detalleGrid.DataSource = ds
        detalleGrid.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub FiltraVendedor()
        Dim Objdata As New DataControl
        If Session("perfilid") = 3 Then
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where u.perfilid=3 and u.id='" & Session("userid") & "' and s.id= '" & sucursalid.SelectedValue & "' order by nombre", 0, True)
        Else
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where u.perfilid=3 and s.id='" & sucursalid.SelectedValue & "' and isnull(u.borradobit,0)=0 order by nombre", 0, True)
        End If
        Objdata = Nothing
    End Sub

    Private Sub FiltraVendedorCliente()
        Dim Objdata As New DataControl
        If clienteid.SelectedValue = 0 Then
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 order by nombre", 0, True)
        Else
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id inner join tblMisClientes c on c.id=s.clienteId where u.perfilid=3 and c.id = '" & clienteid.SelectedValue.ToString & "' and isnull(u.borradobit,0)=0 order by nombre", 0, True)
        End If
        Objdata = Nothing
    End Sub

    'Private Sub detalleGrid_ExportCellFormatting(ByVal sender As Object, ByVal e As Telerik.Web.UI.ExportCellFormattingEventArgs) Handles detalleGrid.ExportCellFormatting
    '    If (e.FormattedColumn.UniqueName) = "folio" Then
    '        e.Cell.Style("mso-number-format") = "0\.00"
    '    End If
    'End Sub

    Protected Sub detalleGrid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles detalleGrid.ItemDataBound
        'Select Case e.Item.ItemType
        '    Case Telerik.Web.UI.GridItemType.Footer
        '        If ds.Tables(0).Rows.Count > 0 Then
        '            e.Item.Cells(10).Text = FormatCurrency(ds.Tables(0).Compute("sum(importe)", ""), 2).ToString
        '            e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
        '            e.Item.Cells(10).Font.Bold = True
        '            '
        '            e.Item.Cells(11).Text = FormatCurrency(ds.Tables(0).Compute("sum(descuento)", ""), 2).ToString
        '            e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
        '            e.Item.Cells(11).Font.Bold = True
        '            '
        '            e.Item.Cells(12).Text = FormatCurrency(ds.Tables(0).Compute("sum(iva)", ""), 2).ToString
        '            e.Item.Cells(12).HorizontalAlign = HorizontalAlign.Right
        '            e.Item.Cells(12).Font.Bold = True
        '            '
        '            e.Item.Cells(13).Text = FormatCurrency(ds.Tables(0).Compute("sum(pagado)", ""), 2).ToString
        '            e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
        '            e.Item.Cells(13).Font.Bold = True
        '            '
        '            e.Item.Cells(14).Text = FormatCurrency(ds.Tables(0).Compute("sum(saldo)", ""), 2).ToString
        '            e.Item.Cells(14).HorizontalAlign = HorizontalAlign.Right
        '            e.Item.Cells(14).Font.Bold = True
        '            '
        '            e.Item.Cells(15).Text = FormatCurrency(ds.Tables(0).Compute("sum(total)", ""), 2).ToString
        '            e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Right
        '            e.Item.Cells(15).Font.Bold = True
        '        End If
        'End Select
    End Sub

    Private Sub MuestraGrafica()
        '
        RadChart1.Visible = True
        Dim ds As New DataSet
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pReporte306090 @clienteid='" & clienteid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @sucursalid='" & sucursalid.SelectedValue & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "'")
        ObjData = Nothing
        '
        RadChart1.SeriesOrientation = Telerik.Charting.ChartSeriesOrientation.Vertical
        Dim dv As New DataView
        dv = ds.Tables(0).DefaultView
        RadChart1.DataSource = dv

        RadChart1.DataBind()
        dv = Nothing
        ''
    End Sub

    Private Sub clienteid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles clienteid.SelectedIndexChanged
        Call MuestraSucursal()
        Call FiltraVendedorCliente()
        If Session("perfilid") = 3 Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 and id='" & Session("userid") & "' order by nombre", Session("userid"), True)
            vendedorid.Enabled = False
        End If
        Call CargaAcumulado()
        Call MuestraGrafica()
    End Sub

    Private Sub MuestraSucursal()
        Dim Objdata As New DataControl
        Objdata.Catalogo(sucursalid, "select id, sucursal from tblSucursalCliente where clienteId='" & clienteid.SelectedValue & "' and isnull(borradobit,0)=0 order by clienteId", 0, True)
        Objdata = Nothing
    End Sub

    Private Sub DownloadPDF(ByVal cfdid As Long)
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("exec pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", connF)
        Try

            connF.Open()

            Dim rs As SqlDataReader
            rs = cmdF.ExecuteReader()

            If rs.Read Then
                serie = rs("serie").ToString
                folio = rs("folio").ToString
            End If
        Catch ex As Exception
            '
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        Dim FilePath = Server.MapPath("~/portalcfd/pdf/") & "ng_" & serie.ToString & folio.ToString & ".pdf"
        '
        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        End If

    End Sub

    Protected Sub btnExportExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExportExcel.Click

        detalleGrid.MasterTableView.ExportToExcel()

    End Sub

    Protected Sub detalleGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles detalleGrid.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pReporteCobranza @cmd=2, @rangoid='" & Session("rangoId").ToString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "'")
        detalleGrid.DataSource = ds
        ObjData = Nothing
    End Sub

    Protected Sub detalleGrid_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles detalleGrid.ItemCommand
        Select Case e.CommandName
            Case "cmdPDF"
                Call DownloadPDF(e.CommandArgument)
            Case "cmdFolio"
                Response.Redirect("~/portalcfd/CFD_Detalle.aspx?id=" & e.CommandArgument.ToString)
        End Select
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            detalleGrid.MasterTableView.GetColumn("pdf").Visible = False
        End If
    End Sub

    Private Sub vendedorid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles vendedorid.SelectedIndexChanged
        Call CargaAcumulado()
        Call MuestraGrafica()
    End Sub

    Private Sub sucursalid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles sucursalid.SelectedIndexChanged

        If sucursalid.SelectedValue = 0 Then
            Call FiltraVendedorCliente()
        Else
            Call FiltraVendedor()
        End If
        If clienteid.SelectedValue = 0 And sucursalid.SelectedValue = 0 Then
            Dim Objdata As New DataControl
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 order by nombre", 0, True)
            Objdata = Nothing
        End If

        If Session("perfilid") = 3 Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 and id='" & Session("userid") & "' order by nombre", Session("userid"), True)
            vendedorid.Enabled = False
        End If

        Call CargaAcumulado()
        Call MuestraGrafica()

    End Sub

    Private Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Call CargaAcumulado()
        Call MuestraGrafica()
    End Sub

End Class