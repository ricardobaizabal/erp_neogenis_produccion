Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Threading
Imports System.Globalization
Partial Class portalcfd_almacen_InventarioCiclico
    Inherits System.Web.UI.Page
    Dim ds As DataSet = New DataSet

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        chkAll.Attributes.Add("onclick", "checkedAll(" & Me.Form.ClientID.ToString & ");")
        'If Not IsPostBack Then
        '    Dim idsPudenAgregar As New List(Of Integer) From {0, 17, 18, 32, 34}
        '    If idsPudenAgregar.Contains(Session("userid")) Then
        '        'PanelAgregarAjuste.Visible = True
        '    End If
        'End If
        If Not IsPostBack Then
            'fha_ini.SelectedDate = Now()
            'fha_fin.SelectedDate = Now()

            '
            '   Protege contra doble clic la creación de la factura
            '
            ' btnConfirmar.Attributes.Add("onclick", "javascript:" + btnConfirmar.ClientID + ".disabled=true;" + ClientScript.GetPostBackEventReference(btnConfirmar, ""))
            '

            CrearTablaTemp()

            Dim objCat As New DataControl
            objCat.Catalogo(cmbMarca, "select id, nombre from tblProyecto order by nombre", 0)
            objCat = Nothing

            If Session("perfilid") <> 1 Then
                btnConfirmar.Enabled = False
            End If

        End If
    End Sub

    Function MuestraUltimosMovimientos() As DataSet
        Dim ObjData As New DataControl
        Dim ds As DataSet = New DataSet
        ds = ObjData.FillDataSet("exec pInventario @cmd=11")
        productslist.DataSource = ds
        productslist.DataBind()
        ObjData = Nothing

        Return ds
    End Function

    Protected Sub productslist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productslist.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pInventario @cmd=11")
        productslist.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub productslist_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles productslist.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                DeleteItem(e.CommandArgument)
                ' CargaTotales()
                Call MuestraUltimosMovimientos()
        End Select
    End Sub
    Private Sub DeleteItem(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pInventario @cmd='16', @idCiclico ='" & id.ToString & "'", conn)

        conn.Open()

        cmd.ExecuteReader()

        conn.Close()

        'Call DisplayItems()

    End Sub

    Protected Sub productslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles productslist.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                'Dim lnkFolio As LinkButton = CType(e.Item.FindControl("lnkFolio"), LinkButton)
                'Dim imgNuevo As ImageButton = CType(e.Item.FindControl("imgNuevo"), ImageButton)
                'If Session("perfilid") = 3 Then
                '    lnkFolio.Enabled = False
                'End If
                'If e.Item.DataItem("estatus_cobranza") = "Pendiente" Then
                '    e.Item.Cells(13).ForeColor = Drawing.Color.DarkRed
                'Else
                '    e.Item.Cells(13).ForeColor = Drawing.Color.Green
                'End If
                'e.Item.Cells(13).Font.Bold = True
                'If e.Item.DataItem("UbiNew") = True Then
                '    imgNuevo.ImageUrl = "~/portalcfd/images/check.png"
                '    'imgNuevo.ToolTip = "Enviado el " & e.Item.DataItem("fechaenvio").ToString
                'Else
                '    imgNuevo.ImageUrl = "~/portalcfd/images/check_vacio.png"
                '    'imgNuevo.ToolTip = "Enviado el " & e.Item.DataItem("fechaenvio").ToString
                'End If

                'Case Telerik.Web.UI.GridItemType.Footer
                '    If ds.Tables(0).Rows.Count > 0 Then
                '        e.Item.Cells(6).Text = "Totales"
                '        e.Item.Cells(6).Font.Bold = True

                '        e.Item.Cells(10).Text = ds.Tables(0).Compute("sum(quantityTeorico)", "")
                '        e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Center
                '        e.Item.Cells(10).Font.Bold = True
                '        '
                '        e.Item.Cells(12).Text = ds.Tables(0).Compute("sum(quantityTeoricoApi)", "")
                '        e.Item.Cells(12).HorizontalAlign = HorizontalAlign.Center
                '        e.Item.Cells(12).Font.Bold = True
                '        '
                '        e.Item.Cells(13).Text = ds.Tables(0).Compute("sum(quantityFisico)", "")
                '        e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Center
                '        e.Item.Cells(13).Font.Bold = True
                '        '
                '        e.Item.Cells(15).Text = ds.Tables(0).Compute("sum(quantityFinal)", "")
                '        e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Left
                '        e.Item.Cells(15).Font.Bold = True
                '    End If
        End Select
    End Sub

    Private Sub DisplayItems()
        productslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
        Session("TmpDetalleInventariado") = MuestraUltimosMovimientos().Tables(0)
        productslist.DataSource = Session("TmpDetalleInventariado")
        productslist.DataBind()
    End Sub

    Private Sub CrearTablaTemp()
        Dim dt As New DataTable()
        dt.Columns.Add("id")
        dt.Columns.Add("idCarga")
        dt.Columns.Add("fecha")
        dt.Columns.Add("productId")
        dt.Columns.Add("barcode")
        dt.Columns.Add("marca")
        dt.Columns.Add("descripcion")
        dt.Columns.Add("barcodeLocationERP")
        dt.Columns.Add("quantityTeorico")
        'dt.Columns.Add("timbrado")
        dt.Columns.Add("barcodeLocationAPI")
        dt.Columns.Add("quantityTeoricoApi")
        dt.Columns.Add("quantityFisico")
        dt.Columns.Add("quantityDifferenceERP")
        'dt.Columns.Add("quantityDifference")
        dt.Columns.Add("quantityFinal")
        dt.Columns.Add("comentario")
        dt.Columns.Add("chkInventC")
        '    dt.Columns.Add("UbiNew")
        Session("TmpDetalleInventariado") = dt
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
        'RadWindow1.VisibleOnPageLoad = False
    End Sub

    Public Sub CargarSaldoPendiente()
        Dim i As Integer = 0

        Session("TmpDetalleInventariado").Rows.Clear()

        For Each dataItem As Telerik.Web.UI.GridDataItem In productslist.MasterTableView.Items
            Dim lblid As String = dataItem.GetDataKeyValue("id").ToString
            Dim lblidCarga As Label = DirectCast(dataItem.FindControl("lblidCarga"), Label)
            Dim lblFecha As Label = DirectCast(dataItem.FindControl("lblFecha"), Label)
            Dim lblProductId As Label = DirectCast(dataItem.FindControl("lblproductId"), Label)
            Dim lblBarcode As Label = DirectCast(dataItem.FindControl("lblbarcode"), Label)
            Dim lblMarca As Label = DirectCast(dataItem.FindControl("lblmarca"), Label)
            Dim lblDescripcion As Label = DirectCast(dataItem.FindControl("lbldescripcion"), Label)
            Dim lblbarcodeLocationERP As Label = DirectCast(dataItem.FindControl("lblbarcodeLocationERP"), Label)
            Dim lblQuantityTeoricoAPi As Label = DirectCast(dataItem.FindControl("lblquantityTeoricoApi"), Label)
            Dim lblQuantityTeorico As Label = DirectCast(dataItem.FindControl("lblquantityTeorico"), Label)
            Dim lblbarcodeLocationAPI As Label = DirectCast(dataItem.FindControl("lblbarcodeLocationAPI"), Label)
            Dim lblquantityFisico As Label = DirectCast(dataItem.FindControl("lblquantityFisico"), Label)
            'Dim lblQuantityDifference As Label = DirectCast(dataItem.FindControl("lblquantityDifference"), Label)
            Dim txtQuantityDifferenceERP As RadNumericTextBox = DirectCast(dataItem.FindControl("txtquantityDifferenceERP"), RadNumericTextBox)
            'Dim txtQuantityDifference As RadNumericTextBox = DirectCast(dataItem.FindControl("txtquantityDifference"), RadNumericTextBox)
            'Dim lblQuantityFinal As Label = DirectCast(dataItem.FindControl("txtCantidadFinal"), Label)
            Dim txtCantidadFinal As RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidadFinal"), RadNumericTextBox)
            Dim txtComentario As TextBox = DirectCast(dataItem.FindControl("txtComentario"), TextBox)
            'Dim lblUbiNew As Label = DirectCast(dataItem.FindControl("lblubiNew"), Label)
            Dim chkInventC As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkInventC"), System.Web.UI.WebControls.CheckBox)

            Dim dr As DataRow = Session("TmpDetalleInventariado").NewRow()
            dr.Item("id") = lblid.ToString
            dr.Item("idCarga") = lblidCarga.Text
            dr.Item("fecha") = lblFecha.Text
            dr.Item("productId") = lblProductId.Text
            dr.Item("barcode") = lblBarcode.Text
            dr.Item("marca") = lblMarca.Text
            dr.Item("descripcion") = lblDescripcion.Text
            dr.Item("barcodeLocationERP") = lblbarcodeLocationERP.Text
            dr.Item("quantityTeoricoApi") = lblQuantityTeoricoAPi.Text
            dr.Item("quantityTeorico") = lblQuantityTeorico.Text
            dr.Item("barcodeLocationAPI") = lblbarcodeLocationAPI.Text
            dr.Item("quantityFisico") = lblquantityFisico.Text
            'dr.Item("quantityDifference") = lblQuantityDifference.Text
            dr.Item("quantityDifferenceERP") = txtQuantityDifferenceERP.Text
            'dr.Item("quantityDifference") = txtQuantityDifference.Text
            'dr.Item("quantityFinal") = lblQuantityFinal.Text
            dr.Item("quantityFinal") = txtCantidadFinal.Text
            'dr.Item("UbiNew") = lblUbiNew.Text
            dr.Item("comentario") = txtComentario.Text
            dr.Item("chkInventC") = chkInventC.Checked
            Session("TmpDetalleInventariado").Rows.Add(dr)
        Next

        productslist.DataSource = Session("TmpDetalleInventariado")
        productslist.DataBind()

        'CargaTotalCFDI()
        'panelResume.Visible = True
    End Sub

    Protected Sub AgregaInventario()
        Dim cadena As String = ""
        Dim ObjData As New DataControl()
        For Each rows As DataRow In Session("TmpDetalleInventariado").Rows
            If CBool(rows("chkInventC")) = True Then
                ObjData.RunSQLQuery("EXEC pInventario @cmd=12, @seleccion='" & rows("id") & "', @cantidadFinal='" & rows("quantityFinal") & "', @cantidadERP='" & rows("quantityTeorico") & "', @cantidadDiferencia='" & rows("quantityDifferenceERP") & "', @barcodeLocationERP='" & rows("barcodeLocationERP") & "', @barcodeLocationAPI='" & rows("barcodeLocationAPI") & "', @comentario='" & rows("comentario") & "'")

                If cadena = "" Then
                    cadena = rows("id")
                Else
                    cadena = cadena + "," + rows("id")

                End If
            End If
        Next

        ObjData.RunSQLQuery("EXEC pInventario @cmd=13")

        'ObjData.RunSQLQuery("EXEC pInventario @cmd=18, @cadena='" & cadena & "'")

        cadena = Nothing
        ObjData = Nothing
        Call MuestraUltimosMovimientos()
        'lblMensajeConf.Text = "Ubicaciones y Cantidades actualizadas"

    End Sub

    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Call AgregaInventario()
    End Sub

    'Private Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAll.CheckedChanged
    '    'Dim valido As Boolean
    '    'valido = ValidarCFDI()

    '    'If valido = True Then
    '    '    CargarSaldoPendiente()
    '    '    RadWindow1.VisibleOnPageLoad = False
    '    'End If
    '    CargarSaldoPendiente()
    'End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim ObjData As New DataControl
        Dim filtroUbicacion As String = Trim(txtUbicacion.Text)
        Dim filtroSku As String = Trim(txtSku.Text)
        'lcng: Se usa cmd 11 por ajuste de corrección de filtros ubicación y sku
        ds = ObjData.FillDataSet("exec pInventario @cmd=11, @marca='" & cmbMarca.SelectedValue.ToString & "', @ubicacion='" & filtroUbicacion & "', @sku='" & filtroSku & "'")
        productslist.DataSource = ds
        productslist.DataBind()
        ObjData = Nothing
    End Sub

    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
    End Sub


End Class