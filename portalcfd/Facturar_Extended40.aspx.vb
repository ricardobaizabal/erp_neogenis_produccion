Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports Telerik.Reporting.Processing
Imports System.IO
Imports System.Xml
Imports System.Xml.Xsl
Imports ThoughtWorks.QRCode.Codec
Imports ThoughtWorks.QRCode.Codec.Util
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading
Imports System.Globalization
Imports Ionic.Zip
Imports System.Web.Services.Protocols
Imports Library_Class_Neogenis

Partial Class Facturar_Extended40
    Inherits System.Web.UI.Page

    Private subtotal As Decimal = 0
    Private descuento As Decimal = 0
    Private iva As Decimal = 0
    Private total As Decimal = 0
    Private totaldescuento As Decimal = 0


    Private tieneIvaTasaCero As Boolean = False
    Private tieneIva16 As Boolean = False
    Private archivoLlavePrivada As String = ""
    Private contrasenaLlavePrivada As String = ""
    Private archivoCertificado As String = ""
    Private serie As String = ""
    Private folio As Long = 0
    Private tipocontribuyenteid As Integer = 0
    'Private tipoid As Integer = 0
    Private tipoprecioid As Integer
    Private cadOrigComp As String
    Dim listMensajes As New List(Of String)

    Private m_xmlDOM As New XmlDocument
    Const URI_SAT = "http://www.sat.gob.mx/cfd/4"
    Const URI_SAT_EXT = "http://www.sat.gob.mx/ComercioExterior20"
    Const URI_SAT_COMPLEMENTO = "http://www.sat.gob.mx/detallista"
    Private listErrores As New List(Of String)
    Private Comprobante As XmlNode
    Private docXML As String = ""
    Dim UUID As String = ""
    Dim AplicarRetencion As Boolean = False

    Private qrBackColor As Integer = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb
    Private qrForeColor As Integer = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb
    Dim dsItemsList As DataSet
    Private data As Byte()
    'Creo mi datatable y columnas
    Public Shared miDataTable1 As New DataTable
    Private Renglon As DataRow = miDataTable1.NewRow()
    Private mi_variable As String
    Private urlcomplemento As Integer = 0
    Dim contador As Integer = 0

#Region "Load Initial Values"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then
            miDataTable1.Clear()
            miDataTable1.Dispose()
            Session("Columna1") = 0
            If miDataTable1.Columns.Contains("uuid") Then
                miDataTable1.Columns.Remove("uuid")
                miDataTable1.Columns.Add("uuid")
            Else

                miDataTable1.Columns.Add("uuid")
            End If
            '''''''''''''''''''
            'Fieldsets Legends'
            '''''''''''''''''''

            lblClientsSelectionLegend.Text = Resources.Resource.lblClientsSelectionLegend
            lblClientData.Text = Resources.Resource.lblClientData
            lblClientItems.Text = Resources.Resource.lblItems
            lblResume.Text = Resources.Resource.lblResume

            '''''''''''''''''''''''''''''''''
            'Combobox Values & Empty Message'
            '''''''''''''''''''''''''''''''''

            Dim ObjCat As New DataControl
            'ObjCat.Catalogo(cmbCliente, "EXEC pMisClientes @cmd=1, @clienteUnionId='" & Session("clienteid") & "' ", 0)
            ObjCat.Catalogo(cmbCliente, "EXEC pCatalogos @cmd=4 ", 0)
            ObjCat.Catalogo(cmbProyecto, "select id, nombre from tblProyecto order by nombre", 0)
            ObjCat.Catalogo(cmbTipoDocumento, "select distinct b.id, b.nombre as tipodocumento from tblMisFolios a inner join tblTipoDocumento b on a.tipoid=b.id where serie is not null and tipoid <> 15", 1)
            ObjCat.Catalogo(cmbMetodoPago, "select id, id + ' - ' + nombre from tblMetodoPago order by nombre", "PPD")
            ObjCat.Catalogo(cmbAlmacen, "select id, nombre from tblAlmacen where id<>4 order by nombre", 5)
            ObjCat.Catalogo(cmbMoneda, "select id, nombre from tblMoneda order by nombre", 1)
            ObjCat.Catalogo(cmbUsoCFD, "select codigo, codigo + ' - ' + descripcion as nombre from tblUsoCFDI order by descripcion", 1)
            ObjCat.Catalogo(cmbTipoRelacion, "select id, id + ' - ' + nombre as descripcion from tblTipoRelacion order by nombre asc", 0)
            ObjCat.Catalogo(cmbComplementoFactura, "select id, nombre from tblComplementosFactura where id in ( 1,6,7 ) order by id", 0)
            ObjCat.Catalogo(cmbExportacion, "select id, descripcion from tblCFDExportacion", "01")
            ObjCat.Catalogo(cmbPeriodicidad, "select id, descripcion from tblPeriodicidad", "04")
            ObjCat.Catalogo(cmbMes, "select id, descripcion from tblMeses", Format(DateTime.Now.Month, "0#"))
            Try
                ObjCat.Catalogo(cmbUUID, "select top  4800 uuid, convert(varchar(10), fecha_factura, 103) + ' | ' + isnull(serie,'') + convert(varchar(10), folio) + ' - ' + isnull(uuid,'') as folio from tblCFD where isnull(uuid,'')<>'' and (estatus=1 or estatus=3) and (serie='A' or serie='FD')  and clienteid='" & cmbCliente.SelectedValue.ToString & "' order by fecha_factura desc", 0)
            Catch ex As Exception

            End Try

            ObjCat = Nothing

            cmbCliente.Text = Resources.Resource.cmbEmptyMessage

            ''''''''''''''
            'Label Titles'
            ''''''''''''''

            lblSocialReason.Text = Resources.Resource.lblSocialReason
            lblContact.Text = Resources.Resource.lblContact
            lblContactPhone.Text = Resources.Resource.lblContactPhone
            lblRFC.Text = Resources.Resource.lblRFC
            lblNumCtaPago.Text = Resources.Resource.lblNumCtaPago
            lblNumCtaPago.ToolTip = Resources.Resource.lblNumCtaPagoTooltip
            lblSubTotal.Text = Resources.Resource.lblSubTotal
            lblImporteTasaCero.Text = Resources.Resource.lblImporteTasaCero
            lblIVA.Text = Resources.Resource.lblIVA
            lblTotal.Text = Resources.Resource.lblTotal
            txtAnio.Text = DateTime.Now.Year
            Call CargaSucursales()

            '''''''''''''''''''
            'Validators Titles'
            '''''''''''''''''''
            ''''''''''''''''
            'Buttons Titles'
            ''''''''''''''''

            btnCreateInvoice.Text = Resources.Resource.btnCreateInvoice
            btnCancelInvoice.Text = Resources.Resource.btnCancelInvoice
            '
            '   Protege contra doble clic la creación de la factura
            '
            btnCreateInvoice.Attributes.Add("onclick", "javascript:" + btnCreateInvoice.ClientID + ".disabled=true;" + ClientScript.GetPostBackEventReference(btnCreateInvoice, ""))

            ''''''''''''''''''''''''''
            'Set CFD Session Variable'
            ''''''''''''''''''''''''''

            If Not String.IsNullOrEmpty(Request("id")) Then

                Session("CFD") = Request("id")

                Call CargaCFD()

                panelItemsRegistration.Visible = True
                itemsList.Visible = True
                panelResume.Visible = True
                panelDescuento.Visible = True

                Call DisplayItems()
                Call CargaTotales()

            Else
                Session("CFD") = 0
            End If

            'If System.Configuration.ConfigurationManager.AppSettings("divisas") = 1 Then
            '    panelDivisas.Visible = True
            'Else
            '    panelDivisas.Visible = False
            'End If

        End If

    End Sub

    Private Sub CargaCFD()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCFD @cmd=10, @cfdid='" & Session("CFD").ToString & "'", conn)
        Dim clienteid As Long = 0
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            panelSpecificClient.Visible = True
            panelItemsRegistration.Visible = True

            If rs.Read() Then
                cmbTipoDocumento.SelectedValue = rs("tipodocumentoid")
                cmbCliente.SelectedValue = rs("clienteid")
                Call CargaSucursales()
                cmbSucursal.SelectedValue = rs("sucursalid")
                cmbAlmacen.SelectedValue = rs("almacenid")
                clienteid = rs("clienteid")
                cmbProyecto.SelectedValue = rs("proyectoid")
                cmbCliente.Enabled = False
                cmbSucursal.Enabled = False
                cmbAlmacen.Enabled = False
                cmbProyecto.Enabled = False
                If rs("rfc") = "XAXX010101000" Then
                    If cmbTipoDocumento.SelectedValue = 2 Then
                        panelFacturaGlobal.Visible = False
                    Else
                        panelFacturaGlobal.Visible = True
                        cmbPeriodicidad.SelectedValue = rs("periodicidad_id")
                        cmbMes.SelectedValue = rs("fac_global_mes")
                        txtAnio.Text = rs("fac_global_anio")
                    End If
                End If
            End If

            rs.Close()

            conn.Close()
            conn.Dispose()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try
        '
        Call CargaCliente(clienteid)
        ''
    End Sub

    Private Function CargaLugarExpedicion() As String
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCliente @cmd=3", conn)
        Dim LugarExpedicion As String = ""
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read() Then
                LugarExpedicion = rs("fac_cp")
            End If

            rs.Close()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return LugarExpedicion

    End Function

#End Region

#Region "Combobox Events"

    Private Sub CargaCliente(ByVal ClienteId As Long)
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pMisClientes @cmd=2, @clienteid='" & ClienteId.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            tipoprecioid = 0
            panelSpecificClient.Visible = True
            panelItemsRegistration.Visible = True

            If rs.Read() Then
                lblSocialReasonValue.Text = rs("razonsocial")
                lblContactValue.Text = rs("contacto")
                lblContactPhoneValue.Text = rs("telefono_contacto")
                lblRFCValue.Text = rs("rfc")
                lblTipoPrecioValue.Text = rs("tipoprecio")
                tipoprecioid = rs("tipoprecioid")
                Dim ObjCat As New DataControl
                ObjCat.Catalogo(cmbFormaPago, "select id, id + ' - ' + nombre from tblFormaPago order by nombre", rs("formapagoid"))
                ObjCat.Catalogo(cmbCondiciones, "select id, nombre from tblCondiciones", rs("condicionesid"))
                ObjCat.Catalogo(cmbUUID, "select top 4800 uuid, convert(varchar(10), fecha_factura, 103) + ' | ' + isnull(serie,'') + convert(varchar(10), folio) + ' - ' + isnull(uuid,'') as folio from tblCFD where isnull(uuid,'')<>'' and (estatus=1 or estatus=3) and (serie='A' or serie='FD')  and clienteid='" & ClienteId.ToString & "' order by fecha_factura desc", 0)
                ObjCat = Nothing
                txtNumCtaPago.Text = rs("numctapago")
                tipocontribuyenteid = rs("tipocontribuyenteid")
                If rs("rfc") = "XAXX010101000" Then
                    If cmbTipoDocumento.SelectedValue = 2 Then
                        panelFacturaGlobal.Visible = False
                    Else
                        panelFacturaGlobal.Visible = True
                    End If
                    cmbUsoCFD.SelectedValue = "S01"
                    Else
                        panelFacturaGlobal.Visible = False
                End If
            End If

            rs.Close()
            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            Response.End()
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub ClearItems()

        itemsList.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
        itemsList.DataSource = Nothing
        itemsList.DataBind()

        Session("CFD") = 0
        itemsList.Visible = False

        lblSubTotalValue.Text = ""
        lblIVAValue.Text = ""
        lblTotalValue.Text = ""
        panelResume.Visible = False
        panelDescuento.Visible = False

    End Sub

#End Region

#Region "Add Invoice Items"

    Protected Sub GetCFD()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCFD @cmd=1, @clienteid='" & cmbCliente.SelectedValue & "', @sucursalid='" & cmbSucursal.SelectedValue.ToString & "', @monedaid='" & cmbMoneda.SelectedValue.ToString & "', @orden_compra='" & txtOrdenCompra.Text & "', @almacenid='" & cmbAlmacen.SelectedValue.ToString & "', @proyectoid='" & cmbProyecto.SelectedValue.ToString & "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                Session("CFD") = rs("cfdid")

            End If

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            Response.End()
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub

    Protected Sub InsertItem(ByVal productoid As Integer, ByVal item As GridItem)
        '
        ' Instancía objetos del grid
        '
        Dim lblCodigo As Label = DirectCast(item.FindControl("lblCodigo"), Label)
        Dim txtDescripcion As System.Web.UI.WebControls.TextBox = DirectCast(item.FindControl("txtDescripcion"), System.Web.UI.WebControls.TextBox)
        Dim lblUnidad As Label = DirectCast(item.FindControl("lblUnidad"), Label)
        Dim lblDisponibles As Label = DirectCast(item.FindControl("lblDisponibles"), Label)
        Dim txtQuantity As RadNumericTextBox = DirectCast(item.FindControl("txtQuantity"), RadNumericTextBox)
        Dim txtUnitaryPrice As RadNumericTextBox = DirectCast(item.FindControl("txtUnitaryPrice"), RadNumericTextBox)
        Dim cantidad As Decimal = 0
        Dim talla As Double = 0
        Dim cajas As Double = 0
        Dim piezasporcaja As Double = 0

        Dim txtTalla As RadNumericTextBox = DirectCast(item.FindControl("txtTalla"), RadNumericTextBox)
        Dim txtCajas As RadNumericTextBox = DirectCast(item.FindControl("txtCajas"), RadNumericTextBox)
        Dim txtPiezasPorCaja As RadNumericTextBox = DirectCast(item.FindControl("txtPiezasPorCaja"), RadNumericTextBox)
        Dim txtNoIdentificacion As RadTextBox = DirectCast(item.FindControl("txtNoIdentificacion"), RadTextBox)
        Dim txtValorDolares As RadNumericTextBox = DirectCast(item.FindControl("txtValorDolares"), RadNumericTextBox)
        Try
            cantidad = Convert.ToDecimal(txtQuantity.Text.Trim())
        Catch ex As Exception
            cantidad = 0
        End Try

        Try
            talla = Convert.ToDecimal(txtTalla.Text)
        Catch ex As Exception
            talla = 0
        End Try

        Try
            cajas = Convert.ToDecimal(txtCajas.Text)
        Catch ex As Exception
            cajas = 0
        End Try

        Try
            piezasporcaja = Convert.ToDecimal(txtPiezasPorCaja.Text)
        Catch ex As Exception
            piezasporcaja = 0
        End Try

        If cantidad > 0 Then
            If lblDisponibles.Text = "N/A" Then
                '
                '   Agrega la partida
                '
                Dim objdata As New DataControl
                Dim porcentaje_descuento As String = ""
                Dim descuento As Decimal = 0

                If cmbTipoDocumento.SelectedValue <> 2 Then
                    porcentaje_descuento = objdata.RunSQLScalarQueryDecimal("EXEC pMisClientes @cmd=7, @clienteid='" & cmbCliente.SelectedValue.ToString & "'")
                    'descuento = ((Convert.ToDecimal(porcentaje_descuento) * (Convert.ToDecimal(cantidad) * Convert.ToDecimal(txtUnitaryPrice.Text))) / 100)
                    descuento = (Convert.ToDecimal(porcentaje_descuento) / 100) * (Convert.ToDecimal(cantidad) * Convert.ToDecimal(txtUnitaryPrice.Text))
                End If

                objdata.RunSQLQuery("EXEC pCFD @cmd=2, @cfdid='" & Session("CFD").ToString & "', @codigo='" & lblCodigo.Text & "', @descripcion='" & txtDescripcion.Text & "', @cantidad='" & cantidad.ToString & "', @unidad='" & lblUnidad.Text & "', @precio='" & txtUnitaryPrice.Text & "', @productoid='" & productoid.ToString & "', @importe_descuento='" & descuento.ToString & "', @talla='" & txtTalla.Text.ToString & "', @cajas='" & txtCajas.Text.ToString & "', @piezasporcaja='" & txtPiezasPorCaja.Text.ToString & "',@numero_identificacion='" & txtNoIdentificacion.Text.ToString & "',@valor_dolares='" & txtValorDolares.Text.ToString & "'")
                objdata = Nothing

                DisplayItems()
                Call CargaTotales()
                panelResume.Visible = True
                panelDescuento.Visible = True
                gridResults.Visible = False
                itemsList.Visible = True
                txtSearchItem.Text = ""
                txtSearchItem.Focus()
                btnCancelSearch.Visible = False
                btnAgregaConceptos.Visible = False
                lblMensaje.Text = ""
            Else
                If Convert.ToDecimal(lblDisponibles.Text) >= cantidad Or cmbTipoDocumento.SelectedValue = 2 Then
                    '
                    '   Agrega la partida
                    '
                    Dim objdata As New DataControl
                    Dim porcentaje_descuento As String = ""
                    Dim descuento As Decimal = 0

                    'If cmbTipoDocumento.SelectedValue <> 2 Then
                    '    porcentaje_descuento = objdata.RunSQLScalarQueryDecimal("EXEC pMisClientes @cmd=7, @clienteid='" & cmbCliente.SelectedValue.ToString & "'")
                    '    descuento = ((Convert.ToDecimal(porcentaje_descuento) * (Convert.ToDecimal(cantidad) * Convert.ToDecimal(txtUnitaryPrice.Text))) / 100)
                    'End If

                    porcentaje_descuento = objdata.RunSQLScalarQueryDecimal("EXEC pMisClientes @cmd=7, @clienteid='" & cmbCliente.SelectedValue.ToString & "'")
                    'descuento = ((Convert.ToDecimal(porcentaje_descuento) * (Convert.ToDecimal(cantidad) * Convert.ToDecimal(txtUnitaryPrice.Text))) / 100)
                    descuento = (Convert.ToDecimal(porcentaje_descuento) / 100) * (Convert.ToDecimal(cantidad) * Convert.ToDecimal(txtUnitaryPrice.Text))
                    objdata.RunSQLQuery("EXEC pCFD @cmd=2, @cfdid='" & Session("CFD").ToString & "', @codigo='" & lblCodigo.Text & "', @descripcion='" & txtDescripcion.Text & "', @cantidad='" & cantidad.ToString & "', @unidad='" & lblUnidad.Text & "', @precio='" & txtUnitaryPrice.Text & "', @productoid='" & productoid.ToString & "', @importe_descuento='" & descuento.ToString & "', @talla='" & txtTalla.Text.ToString & "', @cajas='" & txtCajas.Text.ToString & "', @piezasporcaja='" & txtPiezasPorCaja.Text.ToString & "',@numero_identificacion='" & txtNoIdentificacion.Text.ToString & "',@valor_dolares='" & txtValorDolares.Text.ToString & "'")
                    objdata = Nothing

                    DisplayItems()
                    Call CargaTotales()
                    panelResume.Visible = True
                    panelDescuento.Visible = True
                    gridResults.Visible = False
                    itemsList.Visible = True
                    txtSearchItem.Text = ""
                    txtSearchItem.Focus()
                    btnCancelSearch.Visible = False
                    btnAgregaConceptos.Visible = False
                    lblMensaje.Text = ""
                Else
                    lblMensaje.Text = "La cantidad solicitada es mayor a la existencia para este producto."
                End If
            End If
        Else
            lblMensaje.Text = "Debes proporcionar la cantidad a facturar"
        End If
    End Sub

    Private Sub DisplayItems()
        Dim ObjData As New DataControl
        itemsList.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
        dsItemsList = ObjData.FillDataSet("EXEC pCFD @cmd=3, @cfdid='" & Session("CFD").ToString & "'")
        itemsList.DataSource = dsItemsList
        itemsList.DataBind()
        ObjData = Nothing
    End Sub

    Protected Sub itemsList_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles itemsList.NeedDataSource
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCFD @cmd=3, @cfdid='" & Session("CFD").ToString & "'", conn)

        Try

            conn.Open()

            cmd.Fill(dsItemsList)

            itemsList.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
            itemsList.DataSource = dsItemsList
            itemsList.DataBind()

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            '
        Finally

            conn.Close()
            conn.Dispose()

        End Try
    End Sub

    Private Sub CargaTotales()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pCFD @cmd=16, @cfdid='" & Session("CFD").ToString & "', @tipocambio='" & txtTipoCambio.Text & "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue.ToString & "'", conn)
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                tieneIva16 = rs("tieneIva16")
                tieneIvaTasaCero = rs("tieneIvaTasaCero")
                subtotal = rs("importe")
                iva = rs("iva")
                'tipoid = rs("tipoid")
                descuento = rs("totaldescuento")
                total = rs("total")
                'importetasacero = rs("importetasacero")
                '
                lblSubTotalValue.Text = FormatCurrency(rs("importe_pesos"), 2).ToString
                lblImporteTasaCeroValue.Text = FormatCurrency(rs("importetasacero"), 2).ToString
                lblDescuentoValue.Text = FormatCurrency(rs("totaldescuento"), 2).ToString
                totaldescuento = rs("totaldescuento")
                lblIVAValue.Text = FormatCurrency(rs("iva_pesos"), 2).ToString
                'lblTotalValue.Text = FormatCurrency(rs("total_pesos"), 2).ToString
                lblTotalValue.Text = (Format(CDbl(rs("total_pesos")), "0.#0")).ToString

                '
                '
                'Select Case tipoid
                '    Case 3, 6
                '        '
                '        If tipocontribuyenteid <> 1 Then
                '            lblRetIVAValue.Text = FormatCurrency((iva / 3) * 2, 2).ToString
                '            lblRetISRValue.Text = FormatCurrency((importe * 0.1), 2).ToString
                '            lblTotalValue.Text = FormatCurrency((total - (importe * 0.1) - ((iva / 3) * 2)), 2).ToString
                '        Else
                '            lblRetIVAValue.Text = FormatCurrency(0, 2).ToString
                '            lblRetISRValue.Text = FormatCurrency(0, 2).ToString
                '        End If
                '        '
                '    Case 7
                '        '
                '        If tipocontribuyenteid <> 1 Then
                '            lblRetIVAValue.Text = FormatCurrency((iva * 0.1), 2).ToString
                '            lblRetISRValue.Text = FormatCurrency(0, 2).ToString
                '            lblTotalValue.Text = FormatCurrency((total - (iva * 0.1)), 2).ToString
                '        Else
                '            lblRetIVAValue.Text = FormatCurrency(0, 2).ToString
                '            lblRetISRValue.Text = FormatCurrency(0, 2).ToString
                '        End If
                '        '
                '    Case 11
                '        lblRet.Text = "Ret. 5 al millar="
                '        lblRetISRValue.Text = FormatCurrency(importesindescuento * 0.005, 2).ToString
                '        lblTotalValue.Text = FormatCurrency((total - (importesindescuento * 0.005)), 2).ToString
                '    Case 12
                '        lblRet.Text = "Ret. ="
                '        lblRetISRValue.Text = FormatCurrency(importesindescuento * 0.009, 2).ToString
                '        lblTotalValue.Text = FormatCurrency((total - (importesindescuento * 0.009)), 2).ToString

                'End Select
                If System.Configuration.ConfigurationManager.AppSettings("retencion4") = 1 And cmbTipoDocumento.SelectedValue = 5 Then
                    panelRetencion.Visible = True
                    lblRetValue.Text = FormatCurrency(rs("importe") * 0.04, 2).ToString
                    lblTotalValue.Text = FormatCurrency(rs("total") - (rs("importe") * 0.04) - rs("totaldescuento"), 2).ToString
                End If
                '

            End If

        Catch ex As Exception
            '
            lblTotal.Text = ex.ToString
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
    End Sub

#End Region

#Region "Telerik Grid Items Deleting Events"

    Protected Sub itemsList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles itemsList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim lnkdel As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('" & Resources.Resource.ItemsDeleteConfirmationMessage & "');")
                e.Item.Cells(1).Text = Replace(e.Item.DataItem("descripcion"), vbCrLf, "<br />").ToString
            Case Telerik.Web.UI.GridItemType.Footer
                If dsItemsList.Tables(0).Rows.Count > 0 Then
                    e.Item.Cells(5).Text = "Piezas:"
                    e.Item.Cells(5).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(5).Font.Bold = True
                    '
                    e.Item.Cells(6).Text = dsItemsList.Tables(0).Compute("sum(cantidad)", "").ToString
                    e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(6).Font.Bold = True
                End If
        End Select
        'If TypeOf e.Item Is GridDataItem Then
        '    Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)
        '    If e.Item.OwnerTableView.Name = "Items" Then
        '        Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
        '        lnkdel.Attributes.Add("onclick", "return confirm ('" & Resources.Resource.ItemsDeleteConfirmationMessage & "');")
        '        e.Item.Cells(1).Text = Replace(e.Item.DataItem("descripcion"), vbCrLf, "<br />").ToString
        '    End If
        'End If
    End Sub

    Protected Sub itemsList_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles itemsList.ItemCommand

        Select Case e.CommandName

            Case "cmdDelete"
                DeleteItem(e.CommandArgument)
                CargaTotales()

        End Select

    End Sub

    Private Sub DeleteItem(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCFD @cmd='4', @partidaId ='" & id.ToString & "'", conn)

        conn.Open()

        cmd.ExecuteReader()

        conn.Close()

        Call DisplayItems()

    End Sub

#End Region

#Region "Telerik Grid Items Column Names (From Resource File)"

    Protected Sub itemsList_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles itemsList.ItemCreated

        If TypeOf e.Item Is GridHeaderItem Then

            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)

            If e.Item.OwnerTableView.Name = "Items" Then

                header("codigo").Text = Resources.Resource.gridColumnNameCode
                header("descripcion").Text = Resources.Resource.gridColumnNameDescription
                header("cantidad").Text = Resources.Resource.gridColumnNameQuantity
                header("unidad").Text = Resources.Resource.gridColumnNameMeasureUnit
                'header("precio").Text = Resources.Resource.gridColumnNameUnitaryPrice
                header("importe").Text = Resources.Resource.gridColumnNameAmount

            End If

        End If

    End Sub

#End Region

#Region "Create Invoice"

    Protected Sub btnCreateInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateInvoice.Click
        If Page.IsValid Then
            If Session("uuids_relacionados") = 0 Then
                GetUUIDS_relacionados()
            End If
            Dim Timbrado As Boolean = False
            Dim MensageError As String = ""
            RadWindow1.VisibleOnPageLoad = False

            If cmbTipoDocumento.SelectedValue = 10 Then
                Call Remisionar()
            Else
                '
                '   Rutina de generación de XML CFDI Versión 3.3
                '
                Call CargaTotales()
                '
                '   Guadar Metodo de Pago
                '
                Call GuadarMetodoPago()
                '
                m_xmlDOM = CrearDOM()
                '
                '   Verifica que tipo de comprobante se va a emitir
                '
                Dim TipoDeComprobante As String = Nothing
                Select Case cmbTipoDocumento.SelectedValue
                    Case 1, 3, 4, 5, 6
                        '   Ingreso
                        TipoDeComprobante = "I"
                    Case 2, 8
                        '   Egreso (Nota de Crédito)
                        TipoDeComprobante = "E"
                    Case 24
                        TipoDeComprobante = "P"
                End Select
                '
                '   Asigna Serie y Folio
                '
                Call AsignaSerieFolio()
                '
                Comprobante = CrearNodoComprobante(TipoDeComprobante)
                '
                m_xmlDOM.AppendChild(Comprobante)
                IndentarNodo(Comprobante, 1)
                '
                '  Factura al publico en general
                '
                VerificaFacturaGlobal(Comprobante)
                '
                '   Agrega CfdiRelacionados
                '
                If cmbTipoRelacion.SelectedValue <> 0 And tblRelacionados.Items.Count > 0 Then
                    CrearNodoCfdiRelacionados(Comprobante)
                    IndentarNodo(Comprobante, 1)
                End If
                '
                '   Agrega los datos del emisor
                '
                Call ConfiguraEmisor()
                '
                '   Agrega los datos del receptor
                '
                Call ConfiguraReceptor()
                '
                '   Agrega los conceptos de la factura
                '
                CrearNodoConceptos(Comprobante)
                IndentarNodo(Comprobante, 1)
                '
                '   Agrega Impuestos
                '
                CrearNodoImpuestos(Comprobante)
                IndentarNodo(Comprobante, 1)
                '
                '   Agrega complemento comercio exterior
                '
                Dim replaceBit As Boolean = True
                If cmbComplementoFactura.SelectedValue = 1 Then
                    replaceBit = False
                    CrearNodoComplementoDetallista(Comprobante)
                End If
                '
                '   Agrega complemento comercio exterior
                '
                If cmbComplementoFactura.SelectedValue = 7 Then
                    CrearNodoComplemento(Comprobante)
                    IndentarNodo(Comprobante, 0)
                End If
                '
                '   Sellar Comprobante
                '
                SellarCFD(Comprobante)
                If replaceBit = True Then
                    m_xmlDOM.InnerXml = (Replace(m_xmlDOM.InnerXml, "schemaLocation", "xsi:schemaLocation", , , CompareMethod.Text))
                End If
                'm_xmlDOM.InnerXml = (Replace(m_xmlDOM.InnerXml, "schemaLocation", "xsi:schemaLocation", , , CompareMethod.Text))
                m_xmlDOM.Save(Server.MapPath("cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & ".xml")
                '
                '   Realiza Timbrado
                '
                If folio > 0 Then
                    Try
                        '
                        '   Timbrado SIFEI
                        '
                        Dim SIFEIUsuario As String = System.Configuration.ConfigurationManager.AppSettings("SIFEIUsuario")
                        Dim SIFEIContrasena As String = System.Configuration.ConfigurationManager.AppSettings("SIFEIContrasena")
                        Dim SIFEIIdEquipo As String = System.Configuration.ConfigurationManager.AppSettings("SIFEIIdEquipo")

                        System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType) Or DirectCast(768, System.Net.SecurityProtocolType) Or DirectCast(192, System.Net.SecurityProtocolType) Or DirectCast(48, System.Net.SecurityProtocolType)

                        'Pruebas
                        'Dim TimbreSifei As New SIFEIPruebas.SIFEIService()

                        'Producción
                        Dim TimbreSifei As New SIFEI33.SIFEIService()
                        Call Comprimir()

                        'Dim bytes() As Byte
                        'bytes = TimbreSifei.getCFDI(SIFEIUsuario, SIFEIContrasena, data, "", SIFEIIdEquipo)
                        'Descomprimir(bytes)
                        'Timbrado = True
                        'MensageError = ""

                        Try
                            Dim bytes() As Byte
                            bytes = TimbreSifei.getCFDISign(SIFEIUsuario, SIFEIContrasena, data, "", SIFEIIdEquipo)
                            Descomprimir(bytes)
                            Timbrado = True
                        Catch sx As SoapException

                            Call cfdnotimbrado()
                            Timbrado = False
                            MensageError = "Código: " & sx.Detail.FirstChild.SelectSingleNode("codigo").InnerText & vbCrLf & vbCrLf & "Error: " & sx.Message.ToString & vbCrLf & vbCrLf & "Detalle: " & sx.Detail.FirstChild.SelectSingleNode("error").InnerText

                            Dim xmlSoapEx As XmlDocument = New XmlDocument()
                            xmlSoapEx.LoadXml(sx.Detail.InnerXml)
                            Dim nsManager As XmlNamespaceManager = New XmlNamespaceManager(xmlSoapEx.NameTable)
                            nsManager.AddNamespace("ns2", "http://MApeados/")
                            If (xmlSoapEx.DocumentElement.SelectSingleNode("codigo", nsManager).InnerText.Trim() = "307") Then
                                Dim strXml As String = xmlSoapEx.DocumentElement.SelectSingleNode("xml", nsManager).InnerText
                                Dim xml307 As Byte() = Convert.FromBase64String(strXml)
                                Try
                                    Descomprimir(xml307)
                                    Timbrado = True
                                Catch ex As SoapException
                                    Call cfdnotimbrado()
                                    Timbrado = False
                                    MensageError = "Código: " & ex.Detail.FirstChild.SelectSingleNode("codigo").InnerText & vbCrLf & vbCrLf & "Detalle: " & ex.Detail.FirstChild.SelectSingleNode("error").InnerText
                                End Try
                            End If
                        Catch ex As Exception
                            Try
                                Dim bytes() As Byte
                                bytes = TimbreSifei.getCFDISign(SIFEIUsuario, SIFEIContrasena, data, "", SIFEIIdEquipo)
                                Descomprimir(bytes)
                                Timbrado = True
                            Catch sx As SoapException
                                Call cfdnotimbrado()
                                Timbrado = False
                                MensageError = "Código: " & sx.Detail.FirstChild.SelectSingleNode("codigo").InnerText & vbCrLf & vbCrLf & "Detalle: " & sx.Detail.FirstChild.SelectSingleNode("error").InnerText

                                Dim xmlSoapEx As XmlDocument = New XmlDocument()
                                xmlSoapEx.LoadXml(sx.Detail.InnerXml)
                                Dim nsManager As XmlNamespaceManager = New XmlNamespaceManager(xmlSoapEx.NameTable)
                                nsManager.AddNamespace("ns2", "http://MApeados/")
                                If (xmlSoapEx.DocumentElement.SelectSingleNode("codigo", nsManager).InnerText.Trim() = "307") Then
                                    Dim strXml As String = xmlSoapEx.DocumentElement.SelectSingleNode("xml", nsManager).InnerText
                                    Dim xml307 As Byte() = Convert.FromBase64String(strXml)
                                    Try
                                        Descomprimir(xml307)
                                        Timbrado = True
                                    Catch soex As SoapException
                                        Call cfdnotimbrado()
                                        Timbrado = False
                                        MensageError = "Código: " & soex.Detail.FirstChild.SelectSingleNode("codigo").InnerText & vbCrLf & "Detalle: " & soex.Detail.FirstChild.SelectSingleNode("error").InnerText
                                    End Try
                                End If
                            End Try
                        End Try



                        cadOrigComp = CadenaOriginalComplemento()

                        If Timbrado = True Then
                            '
                            '   Genera Código Bidimensional
                            '
                            Call generacbb()
                            '
                            '   Marca el cfd como timbrado
                            '
                            Call cfdtimbrado()
                            '
                            '   Obtiene el UUID
                            '
                            'Dim filePath As String = Server.MapPath("~/portalcfd/cfd_storage/ng_") & serie.ToString & folio.ToString & "_timbrado.xml"
                            'Dim UUID() As String
                            'ReDim UUID(0)
                            ''
                            'Dim FlujoReader As XmlTextReader = Nothing
                            'Dim j As Integer
                            'FlujoReader = New XmlTextReader(filePath)
                            'FlujoReader.WhitespaceHandling = WhitespaceHandling.None
                            'While FlujoReader.Read()
                            '    Select Case FlujoReader.NodeType
                            '        Case XmlNodeType.Element
                            '            If FlujoReader.Name = "tfd:TimbreFiscalDigital" Then
                            '                For j = 0 To FlujoReader.AttributeCount - 1
                            '                    FlujoReader.MoveToAttribute(j)
                            '                    If FlujoReader.Name = "UUID" Then
                            '                        UUID(0) = FlujoReader.Value.ToString
                            '                    End If
                            '                Next
                            '            End If
                            '    End Select
                            'End While
                            ''
                            ''   Marca el cfd como timbrado
                            ''
                            'Call cfdtimbrado(UUID(0))
                            '
                            '   Descarga Inventario si hay folio y fué timbrado el cfdi
                            '
                            Call DescargaInventario(Session("CFD"))
                            '
                            '   Verifica timbrado y rescate de folio
                            '
                            'Call VerificaTimbrado(Session("CFD"))
                            '
                            '   Agrega addendas
                            '   
                            Call AddendasDespuesDelTimbrado()
                            '
                            '   Genera PDF
                            '

                            If Not File.Exists(Server.MapPath("~/portalcfd/pdf") & "\ng_" & serie.ToString & folio.ToString & ".pdf") Then
                                GuardaPDF(GeneraPDF(Session("CFD")), Server.MapPath("~/portalcfd/pdf") & "\ng_" & serie.ToString & folio.ToString & ".pdf")
                            End If
                        End If

                    Catch ex As SoapException
                        Call cfdnotimbrado()
                        Timbrado = False
                        MensageError = ex.Detail.InnerText
                    End Try
                Else
                    Call cfdnotimbrado()
                    Timbrado = False
                    MensageError = "No se encontraron folios disponibles."
                End If

            End If

            Session("CFD") = 0

            If Timbrado = True Then
                Response.Redirect("~/portalcfd/cfd.aspx")
            Else
                txtErrores.Text = MensageError.ToString
                RadWindow1.VisibleOnPageLoad = True
            End If

        End If
    End Sub

    Private Function Comprimir()
        Dim zip As ZipFile = New ZipFile(serie.ToString & folio.ToString.ToString & ".zip")
        zip.AddFile(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & ".xml", "")
        Dim ms As New MemoryStream()
        zip.Save(ms)
        data = ms.ToArray
    End Function

    Private Function Descomprimir(ByVal data5 As Byte())
        Dim ms1 As New MemoryStream(data5)
        Dim zip1 As ZipFile = New ZipFile()
        zip1 = ZipFile.Read(ms1)

        Dim archivo As String = ""
        Dim DirectorioExtraccion As String = Server.MapPath("~/portalcfd/cfd_storage/").ToString
        Dim e As ZipEntry
        For Each e In zip1
            archivo = e.FileName
            e.Extract(DirectorioExtraccion, ExtractExistingFileAction.OverwriteSilently)
        Next

        Dim Path = Server.MapPath("~/portalcfd/cfd_storage/")
        If File.Exists(Path & archivo) Then
            System.IO.File.Copy(Path & archivo, Path & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
        End If

    End Function

    Private Sub GuadarMetodoPago()
        Dim Objdata As New DataControl
        Dim cmdFacturaGlobal As String = ""
        If panelFacturaGlobal.Visible Then
            cmdFacturaGlobal = "',@periodicidad_id='" & cmbPeriodicidad.SelectedValue &
                               "',@fac_global_mes='" & cmbMes.SelectedValue &
                               "',@fac_global_anio='" & txtAnio.Text
        End If
        Objdata.RunSQLQuery("exec pCFD @cmd=25, @metodopagoid='" & cmbMetodoPago.SelectedValue &
                                            "', @usocfdi='" & cmbUsoCFD.SelectedValue &
                                            "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue &
                                            "', @cfdid='" & Session("CFD").ToString & cmdFacturaGlobal & "'")
        Objdata = Nothing
    End Sub

    Private Function FileToMemory(ByVal Filename As String) As MemoryStream
        Dim FS As New System.IO.FileStream(Filename, FileMode.Open)
        Dim MS As New System.IO.MemoryStream
        Dim BA(FS.Length - 1) As Byte
        FS.Read(BA, 0, BA.Length)
        FS.Close()
        MS.Write(BA, 0, BA.Length)
        Return MS
    End Function

    Private Sub DescargaInventario(ByVal cfdid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pControlInventario @cmd=5, @cfdid='" & cfdid.ToString & "', @userid='" & Session("userid").ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub VerificaTimbrado(ByVal cfdid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pCFD @cmd=32, @cfdid='" & cfdid.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub AsignaCFDUsuario(ByVal cfdid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pUsuarios @cmd=7, @userid='" & Session("userid").ToString & "', @cfdid='" & cfdid.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub Remisionar()
        '
        Call CargaTotales()
        '
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        '   Obtiene folio y actualiza cfd
        '
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim aprobacion As String = ""
        Dim annioaprobacion As String = ""

        Dim SQLUpdate As String = ""

        If Not chkAduana.Checked Then
            SQLUpdate = "exec pCFD @cmd=17, @cfdid='" & Session("CFD").ToString & "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue.ToString & "', @usocfdi='" & cmbUsoCFD.SelectedValue & "', @instrucciones='" & txtObservaciones.Text & "', @aduana='" & nombreaduana.Text & "', @fecha_pedimento='', @numero_pedimento='" & numeropedimento.Text & "', @tipocambio='" & txtTipoCambio.Text & "', @metodopagoid='" & cmbMetodoPago.SelectedValue.ToString & "', @formapagoid='" & cmbFormaPago.SelectedValue.ToString & "', @numctapago='" & txtNumCtaPago.Text & "', @condicionesid='" & cmbCondiciones.SelectedValue.ToString & "', @sucursalid='" & cmbSucursal.SelectedValue.ToString & "'"
        Else
            SQLUpdate = "exec pCFD @cmd=17, @cfdid='" & Session("CFD").ToString & "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue.ToString & "', @usocfdi='" & cmbUsoCFD.SelectedValue & "', @instrucciones='" & txtObservaciones.Text & "', @aduana='" & nombreaduana.Text & "', @fecha_pedimento='" & fechapedimento.SelectedDate.Value.ToShortDateString & "', @numero_pedimento='" & numeropedimento.Text & "', @tipocambio='" & txtTipoCambio.Text & "', @metodopagoid='" & cmbMetodoPago.SelectedValue.ToString & "', @formapagoid='" & cmbFormaPago.SelectedValue.ToString & "', @numctapago='" & txtNumCtaPago.Text & "', @sucursalid='" & cmbSucursal.SelectedValue.ToString & "'"
        End If

        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand(SQLUpdate, connF)
        Try

            connF.Open()

            Dim rs As SqlDataReader
            rs = cmdF.ExecuteReader()

            If rs.Read Then
                serie = rs("serie").ToString
                folio = rs("folio").ToString
                aprobacion = rs("aprobacion").ToString
                annioaprobacion = rs("annio_solicitud").ToString
                'tipoid = rs("tipoid")
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            Response.End()
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        '   Marca el documento como formato
        '
        Dim ObjM As New DataControl
        ObjM.RunSQLQuery("exec pCFD @cmd=33, @cfdid='" & Session("CFD").ToString & "'")
        ObjM = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
        '   Genera PDF
        '
        If Not File.Exists(Server.MapPath("~/portalcfd/pdf") & "\ng_" & serie.ToString & folio.ToString & ".pdf") Then
            GuardaPDF(GeneraPDF_Documento(Session("CFD")), Server.MapPath("~/portalcfd/pdf") & "\ng_" & serie.ToString & folio.ToString & ".pdf")
        End If
        '
        '
        '   Descarga Inventario 
        '
        Call DescargaInventario(Session("CFD"))
        '
    End Sub

    Private Sub AsignaSerieFolio()
        '
        '   Obtiene serie y folio
        '
        Dim aprobacion As String = ""
        Dim annioaprobacion As String = ""

        Dim SQLUpdate As String = ""

        If Not chkAduana.Checked Then
            SQLUpdate = "exec pCFD @cmd=17, @cfdid='" & Session("CFD").ToString & "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue.ToString & "', @instrucciones='" & txtObservaciones.Text & "', @aduana='" & nombreaduana.Text & "', @fecha_pedimento='', @numero_pedimento='" & numeropedimento.Text & "', @monedaid='" & cmbMoneda.SelectedValue.ToString & "', @tipocambio='" & txtTipoCambio.Text & "', @metodopagoid='" & cmbMetodoPago.SelectedValue.ToString & "', @formapagoid='" & cmbFormaPago.SelectedValue.ToString & "', @numctapago='" & txtNumCtaPago.Text & "', @condicionesid='" & cmbCondiciones.SelectedValue.ToString & "', @sucursalid='" & cmbSucursal.SelectedValue.ToString & "', @proyectoid='" & cmbProyecto.SelectedValue.ToString & "', @tiporelacion='" & cmbTipoRelacion.SelectedValue.ToString & "', @uuid_relacionado='" & cmbUUID.SelectedValue.ToString & "'"
        Else
            SQLUpdate = "exec pCFD @cmd=17, @cfdid='" & Session("CFD").ToString & "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue.ToString & "', @instrucciones='" & txtObservaciones.Text & "', @aduana='" & nombreaduana.Text & "', @fecha_pedimento='" & fechapedimento.SelectedDate.Value.ToShortDateString & "', @numero_pedimento='" & numeropedimento.Text & "', @monedaid='" & cmbMoneda.SelectedValue.ToString & "', @tipocambio='" & txtTipoCambio.Text & "', @metodopagoid='" & cmbMetodoPago.SelectedValue.ToString & "', @formapagoid='" & cmbFormaPago.SelectedValue.ToString & "', @numctapago='" & txtNumCtaPago.Text & "', @condicionesid='" & cmbCondiciones.SelectedValue.ToString & "', @sucursalid='" & cmbSucursal.SelectedValue.ToString & "', @proyectoid='" & cmbProyecto.SelectedValue.ToString & "', @tiporelacion='" & cmbTipoRelacion.SelectedValue.ToString & "', @uuid_relacionado='" & cmbUUID.SelectedValue.ToString & "'"
        End If

        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand(SQLUpdate, connF)
        Try

            connF.Open()

            Dim rs As SqlDataReader
            rs = cmdF.ExecuteReader()

            If rs.Read Then
                serie = rs("serie").ToString
                folio = rs("folio").ToString
                aprobacion = rs("aprobacion").ToString
                annioaprobacion = rs("annio_solicitud").ToString
                'tipoid = rs("tipoid")
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            Response.End()
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
    End Sub

    Private Sub ConfiguraEmisor()
        '
        '   Obtiene datos del emisor
        '
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pCFD @cmd=11", conn)
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                CrearNodoEmisor(Comprobante, rs("razonsocial"), rs("fac_rfc"), rs("regimenid"))
                IndentarNodo(Comprobante, 1)
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
    End Sub

    Private Sub ConfiguraReceptor()
        '
        '   Obtiene datos del receptor
        '
        Dim connR As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdR As New SqlCommand("exec pCFD @cmd=12, @clienteId='" & cmbCliente.SelectedValue.ToString & "'", connR)
        Try

            connR.Open()

            Dim rs As SqlDataReader
            rs = cmdR.ExecuteReader()

            If rs.Read Then
                CrearNodoReceptor(Comprobante, rs("denominacion_razon_social"), rs("fac_rfc"), cmbUsoCFD.SelectedValue, rs("fac_cp"), "", "", rs("regimenfiscalid"), rs("identidad_tributaria"), rs("clave_pais"))
                IndentarNodo(Comprobante, 1)
            End If


        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            connR.Close()
            connR.Dispose()
            connR = Nothing
        End Try
    End Sub

    Private Sub CrearNodoConceptos(ByVal Nodo As XmlNode)
        Dim ObjData As New DataControl
        '
        '   Revisa y elimina registros previos de impuestos
        '
        ObjData.RunSQLQuery("exec pCFDTraslados @cmd=6, @cfdid=" & Session("CFD"))
        ObjData.RunSQLQuery("exec pCFDRetenciones @cmd=6, @cfdid=" & Session("CFD"))
        '
        '   Agrega Partidas
        '
        Dim Conceptos As XmlElement
        Dim Concepto As XmlElement
        Dim Impuestos As XmlElement
        Dim Traslados As XmlElement
        Dim Traslado As XmlElement

        Dim conceptoid As Integer
        Dim Base As Decimal
        Dim Impuesto As String
        Dim TipoFactor As String
        Dim TasaOCuota As String
        Dim Importe As Decimal

        Conceptos = CrearNodo("cfdi:Conceptos")
        IndentarNodo(Conceptos, 2)

        Dim connP As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdP As New SqlCommand("exec pCFD @cmd=13, @cfdId='" & Session("CFD").ToString & "'", connP)
        Try
            connP.Open()
            '
            Dim rs As SqlDataReader
            rs = cmdP.ExecuteReader()
            '
            While rs.Read
                conceptoid = rs("id")
                Concepto = CrearNodo("cfdi:Concepto")
                Concepto.SetAttribute("ClaveProdServ", rs("claveprodserv"))
                Concepto.SetAttribute("NoIdentificacion", rs("codigo"))
                Concepto.SetAttribute("Cantidad", rs("cantidad"))
                Concepto.SetAttribute("ClaveUnidad", rs("claveunidad"))
                Concepto.SetAttribute("Unidad", rs("unidad"))
                Concepto.SetAttribute("Descripcion", rs("descripcion"))
                Concepto.SetAttribute("ObjetoImp", "0" + rs("objeto_impuestoid").ToString)

                If rs("descuento") > 0 Then
                    Concepto.SetAttribute("Descuento", Math.Round(rs("descuento"), 6))
                End If

                Concepto.SetAttribute("ValorUnitario", Math.Round(rs("precio"), 6))
                Concepto.SetAttribute("Importe", Math.Round(rs("importe"), 6))

                Impuestos = CrearNodo("cfdi:Impuestos")
                'If iva > 0 Then
                Traslados = CrearNodo("cfdi:Traslados")
                    Traslado = CrearNodo("cfdi:Traslado")

                If rs("descuento") > 0 Then
                    Base = Math.Round(rs("importe"), 6) - Math.Round(rs("descuento"), 6)
                    Traslado.SetAttribute("Base", Base)
                Else
                    Base = Math.Round(rs("importe"), 6)
                    Traslado.SetAttribute("Base", Base)
                End If

                Impuesto = "002"
                TasaOCuota = rs("tasaocuota")
                If CBool(rs("exento")) = False Then
                    TipoFactor = "Tasa"
                    Traslado.SetAttribute("Impuesto", Impuesto)
                    Traslado.SetAttribute("TipoFactor", TipoFactor)
                    Traslado.SetAttribute("TasaOCuota", TasaOCuota)
                    Importe = Format(CDbl(rs("iva")), "#0.000000")
                    Traslado.SetAttribute("Importe", Importe)
                Else
                    TipoFactor = "Exento"
                    Traslado.SetAttribute("Impuesto", Impuesto)
                    Traslado.SetAttribute("TipoFactor", TipoFactor)
                    Importe = 0
                End If

                ObjData.RunSQLQuery("exec pCFDTraslados @cmd=1, " &
                                     "  @cfdid=" & Session("CFD") &
                                     ", @partidaid=" & conceptoid &
                                     ", @baseTraslado='" & Base &
                                     "',@impuesto ='" & Impuesto &
                                     "',@tipofactor='" & TipoFactor &
                                     "',@tasaOcuota='" & TasaOCuota &
                                     "',@importe=" & Importe)

                Traslados.AppendChild(Traslado)
                Impuestos.AppendChild(Traslados)
                'End If
                Concepto.AppendChild(Impuestos)
                Conceptos.AppendChild(Concepto)
                IndentarNodo(Conceptos, 2)
                Concepto = Nothing
            End While
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            connP.Close()
            connP.Dispose()
            connP = Nothing
        End Try

        Nodo.AppendChild(Conceptos)

    End Sub

    Private Sub cfdnotimbrado()
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pCFD @cmd=23, @cfdid='" & Session("CFD").ToString & "'")
        ObjData.RunSQLQuery("exec pCFD @cmd=31, @cfdid='" & Session("CFD").ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub cfdtimbrado()
        Dim uuid As String
        uuid = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        Dim Objdata As New DataControl
        Objdata.RunSQLQuery("exec pCFD @cmd=24, @uuid='" & uuid.ToString & "', @cfdid='" & Session("CFD").ToString & "', @subtotal='" & subtotal.ToString & "', @descuento='" & descuento.ToString & "', @iva='" & iva.ToString & "', @total='" & total.ToString & "'")
        Objdata = Nothing
    End Sub

    Private Sub obtienellave()
        Dim connX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("exec pCFD @cmd=19, @clienteid='" & Session("clienteid").ToString & "', @cfdid='" & Session("CFD").ToString & "'", connX)
        Try

            connX.Open()

            Dim rs As SqlDataReader
            rs = cmdX.ExecuteReader()

            If rs.Read Then
                archivoLlavePrivada = Server.MapPath("~/portalcfd/llave") & "\" & rs("archivo_llave_privada")
                contrasenaLlavePrivada = rs("contrasena_llave_privada")
                archivoCertificado = Server.MapPath("~/portalcfd/certificados") & "\" & rs("archivo_certificado")
            End If

        Catch ex As Exception
            '
        Finally

            connX.Close()
            connX.Dispose()
            connX = Nothing

        End Try
    End Sub

    Private Function Parametros(ByVal codigoUsuarioProveedor As String, ByVal codigoUsuario As String, ByVal idSucursal As Integer, ByVal textoXml As String) As String
        Dim root As XmlNode
        Dim xmlParametros As New XmlDocument()

        If xmlParametros.ChildNodes.Count = 0 Then
            Dim declarationNode As XmlNode = xmlParametros.CreateXmlDeclaration("1.0", "UTF-8", String.Empty)

            xmlParametros.AppendChild(declarationNode)

            root = xmlParametros.CreateElement("Parametros")
            xmlParametros.AppendChild(root)
        Else
            root = xmlParametros.DocumentElement
            root.RemoveAll()
        End If

        Dim attribute As XmlAttribute = root.OwnerDocument.CreateAttribute("Version")
        attribute.Value = "1.0"
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("CodigoUsuarioProveedor")
        attribute.Value = codigoUsuarioProveedor
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("CodigoUsuario")
        attribute.Value = codigoUsuario
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("IdSucursal")
        attribute.Value = idSucursal.ToString()
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("TextoXml")
        attribute.Value = textoXml
        root.Attributes.Append(attribute)

        Return xmlParametros.InnerXml
    End Function

    Private Sub generacbb()
        Dim CadenaCodigoBidimensional As String = ""
        Dim FinalSelloDigitalEmisor As String = ""
        UUID = ""
        Dim rfcE As String = ""
        Dim rfcR As String = ""
        Dim total As String = ""
        Dim sello As String = ""
        '
        '   Obtiene datos del cfdi para construir string del CBB
        '
        rfcE = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Emisor")
        rfcR = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
        total = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Total", "cfdi:Comprobante")
        UUID = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        sello = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloCFD", "tfd:TimbreFiscalDigital")
        FinalSelloDigitalEmisor = Mid(sello, (Len(sello) - 7))
        '
        Dim totalDec As Decimal = CType(total, Decimal)
        '
        CadenaCodigoBidimensional = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx" & "?id=" & UUID & "&re=" & rfcE & "&rr=" & rfcR & "&tt=" & totalDec.ToString & "&fe=" & FinalSelloDigitalEmisor
        '
        '   Genera gráfico
        '
        Dim qrCodeEncoder As QRCodeEncoder = New QRCodeEncoder
        qrCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE
        qrCodeEncoder.QRCodeScale = 6
        qrCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L
        'La versión "0" calcula automáticamente el tamaño
        qrCodeEncoder.QRCodeVersion = 0

        qrCodeEncoder.QRCodeBackgroundColor = System.Drawing.Color.FromArgb(qrBackColor)
        qrCodeEncoder.QRCodeForegroundColor = System.Drawing.Color.FromArgb(qrForeColor)

        Dim CBidimensional As Drawing.Image
        CBidimensional = qrCodeEncoder.Encode(CadenaCodigoBidimensional, System.Text.Encoding.UTF8)
        CBidimensional.Save(Server.MapPath("~/portalCFD/cbb/") & serie.ToString & folio.ToString & ".png", System.Drawing.Imaging.ImageFormat.Png)
    End Sub

    Public Function GetXmlAttribute(ByVal url As String, campo As String, nodo As String) As String
        '
        '   Obtiene datos del cfdi para construir string del CBB
        '
        Dim valor As String = ""
        Dim FlujoReader As XmlTextReader = Nothing
        Dim i As Integer
        '
        '   Leer del fichero e ignorar los nodos vacios
        '
        FlujoReader = New XmlTextReader(url)
        FlujoReader.WhitespaceHandling = WhitespaceHandling.None
        Try
            While FlujoReader.Read()
                Select Case FlujoReader.NodeType
                    Case XmlNodeType.Element
                        If FlujoReader.Name = nodo Then
                            For i = 0 To FlujoReader.AttributeCount - 1
                                FlujoReader.MoveToAttribute(i)
                                If FlujoReader.Name = campo Then
                                    valor = FlujoReader.Value.ToString
                                End If
                            Next
                        End If
                End Select
            End While
            FlujoReader.Close()
            FlujoReader = Nothing
        Catch ex As Exception
            valor = ""
        End Try
        Return valor
    End Function

    Private Function TotalPartidas(ByVal cfdId As Long) As Long
        Dim Total As Long = 0
        Dim connP As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdP As New SqlCommand("exec pCFD @cmd=15, @cfdid='" & cfdId.ToString & "'", connP)
        Try

            connP.Open()

            Dim rs As SqlDataReader
            rs = cmdP.ExecuteReader()

            If rs.Read Then
                Total = rs("total")
            End If

        Catch ex As Exception
            '
        Finally
            connP.Close()
            connP.Dispose()
            connP = Nothing
        End Try
        Return Total
    End Function

    Private Sub MessageBox(ByVal strMsg As String)
        Dim lbl As New Label
        lbl.Text = "<script language='javascript'>" & Environment.NewLine & "window.alert(" & "'" & strMsg & "'" & ")</script>"
        Page.Controls.Add(lbl)
    End Sub

    'Private Function CadenaOriginalComplemento() As String
    '    '
    '    '   Obtiene los valores del timbre de respuesta
    '    '
    '    Dim Version As String = ""
    '    Dim SelloSAT As String = ""
    '    Dim UUID As String = ""
    '    Dim NoCertificadoSAT As String = ""
    '    Dim SelloCFD As String = ""
    '    Dim FechaTimbrado As String = ""
    '    Dim RfcProvCertif As String = ""
    '    '
    '    Dim FlujoReader As XmlTextReader = Nothing
    '    Dim i As Integer
    '    FlujoReader = New XmlTextReader(Server.MapPath("~/portalcfd/cfd_storage/ng_") & serie.ToString & folio.ToString & "_timbrado.xml")
    '    FlujoReader.WhitespaceHandling = WhitespaceHandling.None
    '    While FlujoReader.Read()
    '        Select Case FlujoReader.NodeType
    '            Case XmlNodeType.Element
    '                If FlujoReader.Name = "tfd:TimbreFiscalDigital" Then
    '                    For i = 0 To FlujoReader.AttributeCount - 1
    '                        FlujoReader.MoveToAttribute(i)
    '                        If FlujoReader.Name = "FechaTimbrado" Then
    '                            FechaTimbrado = FlujoReader.Value
    '                        ElseIf FlujoReader.Name = "UUID" Then
    '                            UUID = FlujoReader.Value
    '                        ElseIf FlujoReader.Name = "NoCertificadoSAT" Then
    '                            NoCertificadoSAT = FlujoReader.Value
    '                        ElseIf FlujoReader.Name = "SelloCFD" Then
    '                            SelloCFD = FlujoReader.Value
    '                        ElseIf FlujoReader.Name = "SelloSAT" Then
    '                            SelloSAT = FlujoReader.Value
    '                        ElseIf FlujoReader.Name = "Version" Then
    '                            Version = FlujoReader.Value
    '                        ElseIf FlujoReader.Name = "RfcProvCertif" Then
    '                            RfcProvCertif = FlujoReader.Value
    '                        End If
    '                    Next
    '                End If
    '        End Select
    '    End While

    '    Dim cadena As String = ""
    '    cadena = "||" & Version & "|" & UUID & "|" & FechaTimbrado & "|" & RfcProvCertif & "|" & SelloCFD & "|" & NoCertificadoSAT & "||"
    '    Return cadena

    'End Function

    Private Function CadenaOriginalComplemento() As String

        Dim Version As String = ""
        Dim selloSAT As String = ""
        Dim UUID As String = ""
        Dim noCertificadoSAT As String = ""
        Dim selloCFD As String = ""
        Dim FechaTimbrado As String = ""
        Dim RfcProvCertif As String = ""

        Version = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Version", "tfd:TimbreFiscalDigital")
        UUID = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        FechaTimbrado = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "FechaTimbrado", "tfd:TimbreFiscalDigital")
        RfcProvCertif = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "RfcProvCertif", "tfd:TimbreFiscalDigital")
        selloCFD = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloCFD", "tfd:TimbreFiscalDigital")
        noCertificadoSAT = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificadoSAT", "tfd:TimbreFiscalDigital")

        Dim cadena As String = ""
        cadena = "||" & Version & "|" & UUID & "|" & FechaTimbrado & "|" & RfcProvCertif & "|" & selloCFD & "|" & noCertificadoSAT & "||"
        Return cadena
        '
    End Function

    Private Sub CrearNodoComplementoDetallista(ByVal Nodo As XmlNode)

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")

        Dim largo = Len(CStr(Format(CDbl(total), "#,###.00")))
        Dim decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
        Dim CantidadTexto As String = ""
        CantidadTexto = Num2Text(total - decimales).ToUpper & " PESOS " & Mid(decimales, Len(decimales) - 1) & "/100 M.N."

        Dim Complemento As XmlElement
        Complemento = CrearNodo("cfdi:Complemento")

        Dim doc As XmlDocument = New XmlDocument()
        Dim detallista As XmlElement
        Dim requestForPaymentIdentification As XmlElement
        Dim entityType As XmlElement
        Dim specialInstruction As XmlElement
        Dim specialInstructionText As XmlElement
        Dim orderIdentification As XmlElement
        Dim referenceIdentification1 As XmlElement
        Dim ReferenceDate1 As XmlElement
        Dim AdditionalInformation As XmlElement
        Dim referenceIdentification2 As XmlElement
        Dim DeliveryNote As XmlElement
        Dim referenceIdentification3 As XmlElement
        Dim ReferenceDate2 As XmlElement
        Dim buyer As XmlElement
        Dim buyergln As XmlElement
        Dim contactInformation As XmlElement
        Dim personOrDepartmentName As XmlElement
        Dim personOrDepartmentNameText As XmlElement
        Dim seller As XmlElement
        Dim sellergln As XmlElement
        Dim alternatePartyIdentification As XmlElement

        Dim allowanceCharge As XmlElement
        Dim specialServicesType1 As XmlElement
        Dim monetaryAmountOrPercentage As XmlElement
        Dim monetaryAmountOrPercentageRate As XmlElement
        Dim percentage As XmlElement

        Dim totalAmount As XmlElement
        Dim Amount1 As XmlElement

        Dim TotalAllowanceCharge As XmlElement
        Dim specialServicesType2 As XmlElement
        Dim Amount2 As XmlElement

        urlcomplemento = 1
        detallista = doc.CreateElement("detallista:detallista", URI_SAT_COMPLEMENTO)
        detallista.SetAttribute("xmlns:detallista", URI_SAT_COMPLEMENTO)
        detallista.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/detallista http://www.sat.gob.mx/sitio_internet/cfd/detallista/detallista.xsd")
        detallista.SetAttribute("type", "SimpleInvoiceType")
        detallista.SetAttribute("documentStructureVersion", "AMC8.1")
        detallista.SetAttribute("documentStatus", "ORIGINAL")
        detallista.SetAttribute("contentVersion", "1.3.1")

        requestForPaymentIdentification = doc.CreateElement("detallista:requestForPaymentIdentification", URI_SAT_COMPLEMENTO)
        entityType = doc.CreateElement("detallista:entityType", URI_SAT_COMPLEMENTO)
        entityType.AppendChild(doc.CreateTextNode("INVOICE"))
        requestForPaymentIdentification.AppendChild(entityType)

        specialInstruction = doc.CreateElement("detallista:specialInstruction", URI_SAT_COMPLEMENTO)
        specialInstruction.SetAttribute("code", "ZZZ")
        specialInstructionText = doc.CreateElement("detallista:text", URI_SAT_COMPLEMENTO)
        specialInstructionText.AppendChild(doc.CreateTextNode(CantidadTexto))
        specialInstruction.AppendChild(specialInstructionText)

        orderIdentification = doc.CreateElement("detallista:orderIdentification", URI_SAT_COMPLEMENTO)
        referenceIdentification1 = doc.CreateElement("detallista:referenceIdentification", URI_SAT_COMPLEMENTO)
        referenceIdentification1.SetAttribute("type", "ON")
        referenceIdentification1.AppendChild(doc.CreateTextNode(txtOrdenCompra.Text))
        orderIdentification.AppendChild(referenceIdentification1)

        If Not calFechaOrdenCompra.SelectedDate Is Nothing Then
            ReferenceDate1 = doc.CreateElement("detallista:ReferenceDate", URI_SAT_COMPLEMENTO)
            ReferenceDate1.AppendChild(doc.CreateTextNode(Format(calFechaOrdenCompra.SelectedDate, "yyyy-MM-dd")))
            orderIdentification.AppendChild(ReferenceDate1)
        End If

        AdditionalInformation = doc.CreateElement("detallista:AdditionalInformation", URI_SAT_COMPLEMENTO)
        referenceIdentification2 = doc.CreateElement("detallista:referenceIdentification", URI_SAT_COMPLEMENTO)
        referenceIdentification2.SetAttribute("type", "ATZ")
        referenceIdentification2.AppendChild(doc.CreateTextNode(txtNumeroDeReferenciaAdicional.Text))
        AdditionalInformation.AppendChild(referenceIdentification2)

        If txtNumeroDeContraRecibo.Text.ToString.Length > 0 Then
            DeliveryNote = doc.CreateElement("detallista:DeliveryNote", URI_SAT_COMPLEMENTO)
            referenceIdentification3 = doc.CreateElement("detallista:referenceIdentification", URI_SAT_COMPLEMENTO)
            referenceIdentification3.AppendChild(doc.CreateTextNode(txtNumeroDeContraRecibo.Text))
            DeliveryNote.AppendChild(referenceIdentification3)
            If Not calFechaContraRecibo.SelectedDate Is Nothing Then
                ReferenceDate2 = doc.CreateElement("detallista:ReferenceDate", URI_SAT_COMPLEMENTO)
                ReferenceDate2.AppendChild(doc.CreateTextNode(Format(calFechaContraRecibo.SelectedDate, "yyyy-MM-dd")))
                DeliveryNote.AppendChild(ReferenceDate2)
            End If
        End If

        buyer = doc.CreateElement("detallista:buyer", URI_SAT_COMPLEMENTO)
        buyergln = doc.CreateElement("detallista:gln", URI_SAT_COMPLEMENTO)
        buyergln.AppendChild(doc.CreateTextNode(txtGLNComprador.Text))
        contactInformation = doc.CreateElement("detallista:contactInformation", URI_SAT_COMPLEMENTO)
        personOrDepartmentName = doc.CreateElement("detallista:personOrDepartmentName", URI_SAT_COMPLEMENTO)
        personOrDepartmentNameText = doc.CreateElement("detallista:text", URI_SAT_COMPLEMENTO)
        personOrDepartmentNameText.AppendChild(doc.CreateTextNode(txtNumeroDepartamento.Text))

        personOrDepartmentName.AppendChild(personOrDepartmentNameText)
        contactInformation.AppendChild(personOrDepartmentName)
        buyer.AppendChild(buyergln)
        buyer.AppendChild(contactInformation)

        detallista.AppendChild(requestForPaymentIdentification)
        detallista.AppendChild(specialInstruction)
        detallista.AppendChild(orderIdentification)
        detallista.AppendChild(AdditionalInformation)
        If txtNumeroDeContraRecibo.Text.ToString.Length > 0 Then
            detallista.AppendChild(DeliveryNote)
        End If
        detallista.AppendChild(buyer)

        seller = doc.CreateElement("detallista:seller", URI_SAT_COMPLEMENTO)
        sellergln = doc.CreateElement("detallista:gln", URI_SAT_COMPLEMENTO)
        alternatePartyIdentification = doc.CreateElement("detallista:alternatePartyIdentification", URI_SAT_COMPLEMENTO)

        sellergln.AppendChild(doc.CreateTextNode(txtGLNVendedor.Text))
        alternatePartyIdentification.SetAttribute("type", "SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY")
        alternatePartyIdentification.AppendChild(doc.CreateTextNode(txtNumeroEmisor.Text))
        seller.AppendChild(sellergln)
        seller.AppendChild(alternatePartyIdentification)
        detallista.AppendChild(seller)

        allowanceCharge = doc.CreateElement("detallista:allowanceCharge", URI_SAT_COMPLEMENTO)
        allowanceCharge.SetAttribute("allowanceChargeType", "ALLOWANCE_GLOBAL")
        allowanceCharge.SetAttribute("settlementType", "OFF_INVOICE")

        specialServicesType1 = doc.CreateElement("detallista:specialServicesType", URI_SAT_COMPLEMENTO)
        specialServicesType1.AppendChild(doc.CreateTextNode("AJ"))

        monetaryAmountOrPercentage = doc.CreateElement("detallista:monetaryAmountOrPercentage", URI_SAT_COMPLEMENTO)

        monetaryAmountOrPercentageRate = doc.CreateElement("detallista:rate", URI_SAT_COMPLEMENTO)
        monetaryAmountOrPercentageRate.SetAttribute("base", "INVOICE_VALUE")

        percentage = doc.CreateElement("detallista:percentage", URI_SAT_COMPLEMENTO)
        percentage.AppendChild(doc.CreateTextNode("0.00"))

        monetaryAmountOrPercentageRate.AppendChild(percentage)
        monetaryAmountOrPercentage.AppendChild(monetaryAmountOrPercentageRate)
        allowanceCharge.AppendChild(specialServicesType1)
        allowanceCharge.AppendChild(monetaryAmountOrPercentage)
        detallista.AppendChild(allowanceCharge)
        '
        '   PARTIDAS
        '
        Dim connP As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdP As New SqlCommand("exec pCFD @cmd=13, @cfdId='" & Session("CFD").ToString & "'", connP)
        Try
            connP.Open()
            '
            Dim rs As SqlDataReader
            rs = cmdP.ExecuteReader()
            '
            Dim partida As Integer = 0
            While rs.Read
                partida = partida + 1
                Dim lineItem As XmlElement
                lineItem = doc.CreateElement("detallista:lineItem", URI_SAT_COMPLEMENTO)
                lineItem.SetAttribute("type", "SimpleInvoiceLineItemType")
                lineItem.SetAttribute("number", partida)

                Dim tradeItemIdentification As XmlElement
                Dim gtin As XmlElement

                tradeItemIdentification = doc.CreateElement("detallista:tradeItemIdentification", URI_SAT_COMPLEMENTO)
                gtin = doc.CreateElement("detallista:gtin", URI_SAT_COMPLEMENTO)
                gtin.AppendChild(doc.CreateTextNode(rs("codigo")))
                tradeItemIdentification.AppendChild(gtin)

                Dim alternateTradeItemIdentification As XmlElement
                alternateTradeItemIdentification = doc.CreateElement("detallista:alternateTradeItemIdentification", URI_SAT_COMPLEMENTO)
                alternateTradeItemIdentification.SetAttribute("type", "SUPPLIER_ASSIGNED")
                alternateTradeItemIdentification.AppendChild(doc.CreateTextNode(rs("codigo")))

                Dim tradeItemDescriptionInformation As XmlElement
                Dim longText As XmlElement
                tradeItemDescriptionInformation = doc.CreateElement("detallista:tradeItemDescriptionInformation", URI_SAT_COMPLEMENTO)
                tradeItemDescriptionInformation.SetAttribute("language", "ES")

                longText = doc.CreateElement("detallista:longText", URI_SAT_COMPLEMENTO)
                longText.AppendChild(doc.CreateTextNode(Truncate(rs("descripcion"), 35)))
                tradeItemDescriptionInformation.AppendChild(longText)

                Dim invoicedQuantity As XmlElement
                invoicedQuantity = doc.CreateElement("detallista:invoicedQuantity", URI_SAT_COMPLEMENTO)
                invoicedQuantity.SetAttribute("unitOfMeasure", "PCE")
                invoicedQuantity.AppendChild(doc.CreateTextNode(rs("cantidad")))

                Dim grossPrice As XmlElement
                Dim lineItemAmount1 As XmlElement
                grossPrice = doc.CreateElement("detallista:grossPrice", URI_SAT_COMPLEMENTO)
                lineItemAmount1 = doc.CreateElement("detallista:Amount", URI_SAT_COMPLEMENTO)
                lineItemAmount1.AppendChild(doc.CreateTextNode(Format(rs("precio"), "#0.00")))
                grossPrice.AppendChild(lineItemAmount1)

                Dim netPrice As XmlElement
                Dim lineItemAmount2 As XmlElement
                netPrice = doc.CreateElement("detallista:netPrice", URI_SAT_COMPLEMENTO)
                lineItemAmount2 = doc.CreateElement("detallista:Amount", URI_SAT_COMPLEMENTO)
                lineItemAmount2.AppendChild(doc.CreateTextNode(Format(rs("precio_bruto"), "#0.00")))
                netPrice.AppendChild(lineItemAmount2)

                Dim totalLineAmount As XmlElement
                Dim grossAmount As XmlElement
                Dim lineItemAmount3 As XmlElement
                Dim netAmount As XmlElement
                Dim lineItemAmount4 As XmlElement

                totalLineAmount = doc.CreateElement("detallista:totalLineAmount", URI_SAT_COMPLEMENTO)

                grossAmount = doc.CreateElement("detallista:grossAmount", URI_SAT_COMPLEMENTO)
                lineItemAmount3 = doc.CreateElement("detallista:Amount", URI_SAT_COMPLEMENTO)
                lineItemAmount3.AppendChild(doc.CreateTextNode(Format(rs("importe"), "#0.00")))
                grossAmount.AppendChild(lineItemAmount3)
                totalLineAmount.AppendChild(grossAmount)

                netAmount = doc.CreateElement("detallista:netAmount", URI_SAT_COMPLEMENTO)
                lineItemAmount4 = doc.CreateElement("detallista:Amount", URI_SAT_COMPLEMENTO)
                lineItemAmount4.AppendChild(doc.CreateTextNode(Format(rs("importe") - rs("descuento"), "#0.00")))
                netAmount.AppendChild(lineItemAmount4)
                totalLineAmount.AppendChild(netAmount)

                lineItem.AppendChild(tradeItemIdentification)
                lineItem.AppendChild(alternateTradeItemIdentification)
                lineItem.AppendChild(tradeItemDescriptionInformation)
                lineItem.AppendChild(invoicedQuantity)

                lineItem.AppendChild(grossPrice)
                lineItem.AppendChild(netPrice)
                lineItem.AppendChild(totalLineAmount)

                detallista.AppendChild(lineItem)

            End While
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            connP.Close()
            connP.Dispose()
            connP = Nothing
        End Try

        totalAmount = doc.CreateElement("detallista:totalAmount", URI_SAT_COMPLEMENTO)
        Amount1 = doc.CreateElement("detallista:Amount", URI_SAT_COMPLEMENTO)
        Amount1.AppendChild(doc.CreateTextNode(Format(total, "#0.00")))
        totalAmount.AppendChild(Amount1)
        detallista.AppendChild(totalAmount)

        TotalAllowanceCharge = doc.CreateElement("detallista:TotalAllowanceCharge", URI_SAT_COMPLEMENTO)
        TotalAllowanceCharge.SetAttribute("allowanceOrChargeType", "ALLOWANCE")
        specialServicesType2 = doc.CreateElement("detallista:specialServicesType", URI_SAT_COMPLEMENTO)
        specialServicesType2.AppendChild(doc.CreateTextNode("AJ"))
        Amount2 = doc.CreateElement("detallista:Amount", URI_SAT_COMPLEMENTO)
        Amount2.AppendChild(doc.CreateTextNode(Format(totaldescuento, "#0.00")))

        TotalAllowanceCharge.AppendChild(specialServicesType2)
        TotalAllowanceCharge.AppendChild(Amount2)
        detallista.AppendChild(TotalAllowanceCharge)

        doc.AppendChild(detallista)
        doc.Save(Server.MapPath("~/portalcfd/cfd_storage/") & "complemento_" & serie.ToString & folio.ToString & ".xml")

        IndentarNodo(Complemento, 1)
        Nodo.AppendChild(Complemento)
        urlcomplemento = 0

        m_xmlDOM.Save(Server.MapPath("~/portalcfd/cfd_storage/") & serie.ToString & folio.ToString & ".xml")

        Dim xmlCfdi As New System.Xml.XmlDocument
        xmlCfdi.Load(Server.MapPath("~/portalcfd/cfd_storage/") & serie.ToString & folio.ToString & ".xml")
        docXML = xmlCfdi.InnerXml.ToString
        docXML = (Replace(docXML, "<cfdi:Complemento></cfdi:Complemento>", "<cfdi:Complemento>" & doc.InnerXml.ToString & "</cfdi:Complemento>", , , CompareMethod.Text))
        docXML = (Replace(docXML, "schemaLocation", "xsi:schemaLocation", , , CompareMethod.Text))
        '
        Dim objWriter As New System.IO.StreamWriter(Server.MapPath("~/portalcfd/cfd_storage/") & serie.ToString & folio.ToString & ".xml")
        objWriter.Write(docXML.ToString)
        objWriter.Close()
        '
        m_xmlDOM = New System.Xml.XmlDocument
        m_xmlDOM.Load(Server.MapPath("~/portalcfd/cfd_storage/") & serie.ToString & folio.ToString & ".xml")
        '
        Dim elemList As XmlNodeList = m_xmlDOM.GetElementsByTagName("cfdi:Comprobante")
        '
        For i As Integer = 0 To elemList.Count - 1
            Comprobante = elemList(0)
        Next
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
    End Sub

    Public Function Truncate(value As String, length As Integer) As String
        If length > value.Length Then
            Return value
        Else
            Return value.Substring(0, length)
        End If
    End Function
#End Region

#Region "Manejo de PDF"

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub

    Private Function GeneraPDF(ByVal cfdid As Long) As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim numeroaprobacion As String = ""
        Dim anoAprobacion As String = ""
        Dim fechaHora As String = ""
        Dim noCertificado As String = ""
        Dim razonsocial As String = ""
        Dim callenum As String = ""
        Dim colonia As String = ""
        Dim ciudad As String = ""
        Dim rfc As String = ""
        Dim em_razonsocial As String = ""
        Dim em_callenum As String = ""
        Dim em_colonia As String = ""
        Dim em_ciudad As String = ""
        Dim em_rfc As String = ""
        Dim em_regimen As String = ""
        Dim importe As Decimal = 0
        Dim importetasacero As Decimal = 0
        Dim importe_descuento As Decimal = 0
        Dim iva As Decimal = 0
        Dim total As Decimal = 0
        Dim CantidadTexto As String = ""
        Dim condiciones As String = ""
        Dim enviara As String = ""
        Dim instrucciones As String = ""
        Dim pedimento As String = ""
        Dim retencion As Decimal = 0
        Dim monedaid As Integer = 1
        Dim expedicionLinea1 As String = ""
        Dim expedicionLinea2 As String = ""
        Dim expedicionLinea3 As String = ""
        Dim porcentaje As Decimal = 0
        Dim plantillaid As Integer = 1
        Dim metodopago As String = ""
        Dim formapago As String = ""
        Dim numctapago As String = ""
        Dim serie As String = ""
        Dim folio As Integer = 0
        Dim usoCFDI As String = ""
        Dim tipo_comprobante As String = ""
        Dim tiporelacion As String = ""
        Dim uuid_relacionado As String = ""
        Dim tipoid As Integer = 0

        Dim ds As DataSet = New DataSet

        Try
            Dim cmd As New SqlCommand("EXEC pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                serie = rs("serie")
                folio = rs("folio")
                tipoid = rs("tipoid")
                em_razonsocial = rs("em_razonsocial")
                em_callenum = rs("em_callenum")
                em_colonia = rs("em_colonia")
                em_ciudad = rs("em_ciudad")
                em_rfc = rs("em_rfc")
                em_regimen = rs("regimen")
                razonsocial = rs("razonsocial")
                callenum = rs("callenum")
                colonia = rs("colonia")
                ciudad = rs("ciudad")
                rfc = rs("rfc")
                importe = rs("importe")
                importetasacero = rs("importetasacero")
                importe_descuento = rs("importe_descuento")
                iva = rs("iva")
                total = rs("total")
                monedaid = rs("monedaid")
                fechaHora = rs("fecha_factura").ToString
                condiciones = rs("condiciones").ToString
                enviara = rs("enviara").ToString
                instrucciones = rs("instrucciones")
                If rs("aduana") = "" Or rs("numero_pedimento") = "" Then
                    pedimento = ""
                Else
                    pedimento = "Aduana: " & rs("aduana") & vbCrLf & "Fecha: " & rs("fecha_pedimento").ToString & vbCrLf & "Número: " & rs("numero_pedimento").ToString
                End If
                expedicionLinea1 = rs("expedicionLinea1")
                expedicionLinea2 = rs("expedicionLinea2")
                expedicionLinea3 = rs("expedicionLinea3")
                porcentaje = rs("porcentaje")
                plantillaid = rs("plantillaid")
                tipocontribuyenteid = rs("tipocontribuyenteid")
                metodopago = rs("metodopago")
                formapago = rs("formapago")
                numctapago = rs("numctapago")
                usoCFDI = rs("usocfdi")
                tiporelacion = rs("tiporelacion")
                uuid_relacionado = rs("uuid_relacionado")
            End If
            rs.Close()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try

        Dim largo = Len(CStr(Format(CDbl(total), "#,###.00")))
        Dim decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)

        If monedaid = 1 Then
            CantidadTexto = "Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N."
        Else
            CantidadTexto = "Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD"
        End If

        Select Case tipoid
            Case 3, 6, 7      ' honorarios y arrendamiento
                Dim reporte As New Formatos.formato_cfdi_honorarios33
                reporte.ReportParameters("plantillaId").Value = plantillaid
                reporte.ReportParameters("cfdiId").Value = cfdid
                Select Case tipoid
                    Case 3
                        reporte.ReportParameters("txtDocumento").Value = "Arrendamiento No.    " & serie.ToString & folio.ToString
                    Case 6
                        reporte.ReportParameters("txtDocumento").Value = "Honorarios No.    " & serie.ToString & folio.ToString
                    Case 7
                        reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
                End Select
                reporte.ReportParameters("txtCondicionesPago").Value = condiciones
                reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb/" & serie.ToString & folio.ToString & ".png")
                reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
                reporte.ReportParameters("txtFechaEmision").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Fecha", "cfdi:Comprobante")
                reporte.ReportParameters("txtFechaCertificacion").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "FechaTimbrado", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtUUID").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtPACCertifico").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "RfcProvCertif", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtSerieEmisor").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificado", "cfdi:Comprobante")
                reporte.ReportParameters("txtSerieCertificadoSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificadoSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtClienteRazonSocial").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Nombre", "cfdi:Receptor")
                reporte.ReportParameters("txtClienteCalleNum").Value = callenum
                reporte.ReportParameters("txtClienteColonia").Value = colonia
                reporte.ReportParameters("txtClienteCiudadEstado").Value = ciudad
                reporte.ReportParameters("txtClienteRFC").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
                reporte.ReportParameters("txtSelloDigitalCFDI").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Sello", "cfdi:Comprobante")
                reporte.ReportParameters("txtSelloDigitalSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtPedimento").Value = pedimento
                reporte.ReportParameters("txtEnviarA").Value = enviara

                If tipocontribuyenteid = 1 Then
                    reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
                    'reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                    reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                    reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe + iva, 2).ToString
                    reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
                    reporte.ReportParameters("txtRetIva").Value = FormatCurrency(0, 2).ToString
                    reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva), 2).ToString
                    '
                    '   Ajusta cantidad con texto
                    '
                    total = FormatNumber((importe + iva), 2)
                    largo = Len(CStr(Format(CDbl(total), "#,###.00")))
                    decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
                    CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
                    '
                Else
                    If tipoid = 7 Then
                        reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
                        'reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                        reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                        reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe + iva, 2).ToString
                        reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
                        reporte.ReportParameters("txtRetIva").Value = FormatCurrency((iva * 0.1), 2).ToString
                        reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva) - ((iva * 0.1)), 2).ToString
                        '
                        '   Ajusta cantidad con texto
                        '
                        total = FormatNumber((importe + iva) - ((iva * 0.1)), 2)
                        largo = Len(CStr(Format(CDbl(total), "#,###.00")))
                        decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
                        CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
                        '
                    Else
                        reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
                        'reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                        reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                        reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe + iva, 2).ToString
                        reporte.ReportParameters("txtRetISR").Value = FormatCurrency(importe * 0.1, 2).ToString
                        reporte.ReportParameters("txtRetIva").Value = FormatCurrency((iva / 3) * 2, 2).ToString
                        reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva) - (importe * 0.1) - ((iva / 3) * 2), 2).ToString
                        '
                        '   Ajusta cantidad con texto
                        '
                        total = FormatNumber((importe + iva) - (importe * 0.1) - ((iva / 3) * 2), 2)
                        largo = Len(CStr(Format(CDbl(total), "#,###.00")))
                        decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
                        If monedaid = 1 Then
                            CantidadTexto = "Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N."
                        Else
                            CantidadTexto = "Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD"
                        End If
                    End If
                End If
                reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto
                reporte.ReportParameters("txtCadenaOriginal").Value = cadOrigComp
                reporte.ReportParameters("txtEmisorRazonSocial").Value = em_razonsocial
                reporte.ReportParameters("txtLugarExpedicion").Value = expedicionLinea1 & vbCrLf & expedicionLinea2 & vbCrLf & expedicionLinea3
                If porcentaje > 0 Then
                    reporte.ReportParameters("txtInteres").Value = porcentaje.ToString
                End If
                reporte.ReportParameters("txtRegimen").Value = em_regimen.ToString
                reporte.ReportParameters("txtMetodoPago").Value = metodopago.ToString
                reporte.ReportParameters("txtFormaPago").Value = formapago.ToString
                reporte.ReportParameters("txtUsoCFDI").Value = usoCFDI.ToString
                reporte.ReportParameters("txtNumCtaPago").Value = "Núm. cuenta: " & numctapago.ToString
                reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString
                Return reporte
            Case Else
                Dim reporte As New formato_cfdi40_natural
                reporte.ReportParameters("plantillaId").Value = plantillaid
                reporte.ReportParameters("cfdiId").Value = cfdid
                Select Case tipoid
                    Case 1, 4, 7
                        reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
                    Case 2, 8
                        reporte.ReportParameters("txtDocumento").Value = "Nota de Crédito No. " & serie.ToString & folio.ToString
                    Case 5
                        reporte.ReportParameters("txtDocumento").Value = "Carta Porte No.    " & serie.ToString & folio.ToString
                        reporte.ReportParameters("txtLeyenda").Value = "IMPUESTO RETENIDO DE CONFORMIDAD CON LA LEY DEL IMPUESTO AL VALOR AGREGADO     EFECTOS FISCALES AL PAGO"
                    Case 6
                        reporte.ReportParameters("txtDocumento").Value = "Honorarios No.    " & serie.ToString & folio.ToString
                    Case Else
                        reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
                End Select
                reporte.ReportParameters("txtCondicionesPago").Value = condiciones
                reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb/" & serie.ToString & folio.ToString & ".png")
                reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
                reporte.ReportParameters("txtFechaEmision").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Fecha", "cfdi:Comprobante")
                reporte.ReportParameters("txtFechaCertificacion").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "FechaTimbrado", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtUUID").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtPACCertifico").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "RfcProvCertif", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtSerieEmisor").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificado", "cfdi:Comprobante")
                reporte.ReportParameters("txtSerieCertificadoSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificadoSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtClienteRazonSocial").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Nombre", "cfdi:Receptor")
                reporte.ReportParameters("txtClienteCalleNum").Value = callenum
                reporte.ReportParameters("txtClienteColonia").Value = colonia
                reporte.ReportParameters("txtClienteCiudadEstado").Value = ciudad
                reporte.ReportParameters("txtClienteRFC").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
                reporte.ReportParameters("txtSelloDigitalCFDI").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Sello", "cfdi:Comprobante")
                reporte.ReportParameters("txtSelloDigitalSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtInstrucciones").Value = instrucciones
                reporte.ReportParameters("txtPedimento").Value = pedimento
                reporte.ReportParameters("txtEnviarA").Value = enviara
                reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto
                reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe, 2).ToString
                reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                reporte.ReportParameters("txtEtiquetaIVA").Value = "IVA 16%"
                'reporte.ReportParameters("txtEtiquetaRetIVA").Value = "- RET IVA"
                reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                reporte.ReportParameters("txtDescuento").Value = FormatCurrency(importe_descuento, 2).ToString
                reporte.ReportParameters("txtRetIVA").Value = FormatCurrency(0, 2).ToString
                reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
                reporte.ReportParameters("txtTotal").Value = FormatCurrency(total, 2).ToString
                reporte.ReportParameters("txtCadenaOriginal").Value = cadOrigComp
                reporte.ReportParameters("txtEmisorRazonSocial").Value = em_razonsocial
                reporte.ReportParameters("txtLugarExpedicion").Value = expedicionLinea1 & vbCrLf & expedicionLinea2 & vbCrLf & expedicionLinea3
                tipo_comprobante = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "TipoDeComprobante", "cfdi:Comprobante")

                If tipo_comprobante = "I" Then
                    tipo_comprobante = "Ingreso"
                ElseIf tipo_comprobante = "E" Then
                    tipo_comprobante = "Egreso"
                ElseIf tipo_comprobante = "N" Then
                    tipo_comprobante = "Nómina"
                ElseIf tipo_comprobante = "P" Then
                    tipo_comprobante = "Pago"
                ElseIf tipo_comprobante = "T" Then
                    tipo_comprobante = "Traslado"
                End If

                reporte.ReportParameters("txtTipoComprobante").Value = tipo_comprobante

                If tblRelacionados.Items.Count > 0 Then

                    reporte.ReportParameters("txtTipoRelacion").Value = "Tipo Relación: " & cmbTipoRelacion.SelectedItem.Text
                    Dim OBJ As New DataControl
                    Dim dt As DataSet = OBJ.FillDataSet("exec pCFD @cmd=46, @cfdid='" & Session("CFD").ToString & "'")
                    Dim nodoc As Integer = 0
                    For Each row As DataRow In dt.Tables(0).Rows
                        nodoc += 1
                    Next
                    reporte.ReportParameters("txtUUIDRelacionado").Value = "UUID Relacionado(s): " & nodoc.ToString
                End If

                If tipoid = 5 Then
                    retencion = FormatNumber((importe * 0.04), 2)
                    reporte.ReportParameters("txtRetencion").Value = FormatCurrency(retencion, 2).ToString
                    reporte.ReportParameters("txtTotal").Value = FormatCurrency(total - retencion, 2).ToString
                    largo = Len(CStr(Format(CDbl(total - retencion), "#,###.00")))
                    decimales = Mid(CStr(Format(CDbl(total - retencion), "#,###.00")), largo - 2)
                    If monedaid = 1 Then
                        CantidadTexto = "Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N."
                    Else
                        CantidadTexto = "Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD"
                    End If
                    reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto.ToString
                End If

                reporte.ReportParameters("txtRegimen").Value = em_regimen.ToString
                reporte.ReportParameters("txtMetodoPago").Value = metodopago.ToString
                reporte.ReportParameters("txtFormaPago").Value = formapago.ToString
                If numctapago.Length > 0 Then
                    reporte.ReportParameters("txtNumCtaPago").Value = "Núm. cuenta: " & numctapago.ToString
                End If

                If panelFacturaGlobal.Visible Then
                    instrucciones = instrucciones + " | " + "Factura Global con Periodicidad " & cmbPeriodicidad.SelectedItem.Text & ", Correspondiente al Mes de " & cmbMes.SelectedItem.Text & " del Año " & txtAnio.Text & "."
                End If
                reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString
                reporte.ReportParameters("txtUsoCFDI").Value = "Uso de CFDI: " & usoCFDI

                Return reporte

        End Select

    End Function

    'Private Function GeneraPDF(ByVal cfdid As Long) As Telerik.Reporting.Report
    '    Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

    '    Dim numeroaprobacion As String = ""
    '    Dim anoAprobacion As String = ""
    '    Dim fechaHora As String = ""
    '    Dim noCertificado As String = ""
    '    Dim razonsocial As String = ""
    '    Dim callenum As String = ""
    '    Dim colonia As String = ""
    '    Dim ciudad As String = ""
    '    Dim rfc As String = ""
    '    Dim em_razonsocial As String = ""
    '    Dim em_callenum As String = ""
    '    Dim em_colonia As String = ""
    '    Dim em_ciudad As String = ""
    '    Dim em_rfc As String = ""
    '    Dim em_regimen As String = ""
    '    Dim importe As Decimal = 0
    '    Dim importesindescuento As Decimal = 0
    '    Dim importetasacero As Decimal = 0
    '    Dim importe_descuento As Decimal = 0
    '    Dim iva As Decimal = 0
    '    Dim total As Decimal = 0
    '    Dim CantidadTexto As String = ""
    '    Dim condiciones As String = ""
    '    Dim enviara As String = ""
    '    Dim instrucciones As String = ""
    '    Dim pedimento As String = ""
    '    Dim retencion As Decimal = 0
    '    Dim tipoid As Integer = 0
    '    Dim divisaid As Integer = 1
    '    Dim expedicionLinea1 As String = ""
    '    Dim expedicionLinea2 As String = ""
    '    Dim expedicionLinea3 As String = ""
    '    Dim porcentaje As Decimal = 0
    '    Dim plantillaid As Integer = 1
    '    Dim tipopago As String = ""
    '    Dim formapago As String = ""
    '    Dim numctapago As String = ""
    '    Dim orden_compra As String = ""

    '    Dim ds As DataSet = New DataSet

    '    Try

    '        Dim cmd As New SqlCommand("EXEC pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", conn)
    '        conn.Open()
    '        Dim rs As SqlDataReader
    '        rs = cmd.ExecuteReader()

    '        If rs.Read Then
    '            serie = rs("serie")
    '            folio = rs("folio")
    '            tipoid = rs("tipoid")
    '            em_razonsocial = rs("em_razonsocial")
    '            em_callenum = rs("em_callenum")
    '            em_colonia = rs("em_colonia")
    '            em_ciudad = rs("em_ciudad")
    '            em_rfc = rs("em_rfc")
    '            em_regimen = rs("regimen")
    '            razonsocial = rs("razonsocial")
    '            callenum = rs("callenum")
    '            colonia = rs("colonia")
    '            ciudad = rs("ciudad")
    '            rfc = rs("rfc")
    '            importe = rs("importe")
    '            importe_descuento = rs("importe_descuento")
    '            importetasacero = rs("importetasacero")
    '            iva = rs("iva")
    '            total = rs("total")
    '            divisaid = rs("divisaid")
    '            fechaHora = rs("fecha_factura").ToString
    '            condiciones = rs("condiciones").ToString
    '            enviara = rs("enviara").ToString
    '            instrucciones = rs("instrucciones")
    '            If rs("aduana") = "" Or rs("numero_pedimento") = "" Then
    '                pedimento = ""
    '            Else
    '                pedimento = "Aduana: " & rs("aduana") & vbCrLf & "Fecha: " & rs("fecha_pedimento").ToString & vbCrLf & "Número: " & rs("numero_pedimento").ToString
    '            End If
    '            expedicionLinea1 = rs("expedicionLinea1")
    '            expedicionLinea2 = rs("expedicionLinea2")
    '            expedicionLinea3 = rs("expedicionLinea3")
    '            porcentaje = rs("porcentaje")
    '            plantillaid = rs("plantillaid")
    '            tipocontribuyenteid = rs("tipocontribuyenteid")
    '            tipopago = rs("tipopago")
    '            formapago = rs("formapago")
    '            numctapago = rs("numctapago")
    '            orden_compra = rs("orden_compra")
    '        End If
    '        rs.Close()
    '    Catch ex As Exception
    '        Response.Write(ex.ToString)
    '    Finally

    '        conn.Close()
    '        conn.Dispose()
    '        conn = Nothing
    '    End Try

    '    Dim objData As New DataControl

    '    Dim largo = Len(CStr(Format(CDbl(total), "#,###.00")))
    '    Dim decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)

    '    If System.Configuration.ConfigurationManager.AppSettings("divisas") = 1 Then
    '        If divisaid = 1 Then
    '            CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
    '        Else
    '            CantidadTexto = "( Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD. )"
    '        End If
    '    Else
    '        CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
    '    End If

    '    Dim reporte As New Formatos.formato_cfdi_neogenis
    '    reporte.ReportParameters("plantillaId").Value = plantillaid
    '    reporte.ReportParameters("cfdiId").Value = cfdid
    '    Select Case tipoid
    '        Case 1, 4, 7, 11, 12
    '            reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
    '            reporte.ReportParameters("txtTipoDocumento").Value = "Ingreso"
    '        Case 2, 8
    '            reporte.ReportParameters("txtDocumento").Value = "Nota de Crédito No.    " & serie.ToString & folio.ToString
    '            reporte.ReportParameters("txtTipoDocumento").Value = "Egreso"
    '        Case 5
    '            reporte.ReportParameters("txtDocumento").Value = "Carta Porte No.    " & serie.ToString & folio.ToString
    '            reporte.ReportParameters("txtLeyenda").Value = "IMPUESTO RETENIDO DE CONFORMIDAD CON LA LEY DEL IMPUESTO AL VALOR AGREGADO     EFECTOS FISCALES AL PAGO"
    '            reporte.ReportParameters("txtTipoDocumento").Value = "Ingreso"
    '        Case 6
    '            reporte.ReportParameters("txtDocumento").Value = "Recibo de Honorarios No.    " & serie.ToString & folio.ToString
    '            reporte.ReportParameters("txtTipoDocumento").Value = "Ingreso"
    '        Case Else
    '            reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
    '            reporte.ReportParameters("txtTipoDocumento").Value = "Ingreso"
    '    End Select
    '    reporte.ReportParameters("txtCondicionesPago").Value = condiciones
    '    reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb/" & serie.ToString & folio.ToString & ".png")
    '    reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
    '    reporte.ReportParameters("txtFechaEmision").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "fecha", "cfdi:Comprobante")
    '    reporte.ReportParameters("txtFechaCertificacion").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "FechaTimbrado", "tfd:TimbreFiscalDigital")
    '    reporte.ReportParameters("txtUUID").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
    '    reporte.ReportParameters("txtSerieEmisor").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "noCertificado", "cfdi:Comprobante")
    '    reporte.ReportParameters("txtSerieCertificadoSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "noCertificadoSAT", "tfd:TimbreFiscalDigital")
    '    reporte.ReportParameters("txtClienteRazonSocial").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "nombre", "cfdi:Receptor")
    '    reporte.ReportParameters("txtClienteCalleNum").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "calle", "cfdi:Domicilio") & " " & GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "noExterior", "cfdi:Domicilio") & " " & GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "noInterior", "cfdi:Domicilio")
    '    reporte.ReportParameters("txtClienteColonia").Value = "COL. " & GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "colonia", "cfdi:Domicilio") & " CP. " & GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "codigoPostal", "cfdi:Domicilio")
    '    reporte.ReportParameters("txtClienteCiudadEstado").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "municipio", "cfdi:Domicilio") & " " & GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "estado", "cfdi:Domicilio") & " " & GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "pais", "cfdi:Domicilio")
    '    reporte.ReportParameters("txtClienteRFC").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "rfc", "cfdi:Receptor")
    '    '
    '    reporte.ReportParameters("txtSelloDigitalCFDI").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "sello", "cfdi:Comprobante")
    '    reporte.ReportParameters("txtSelloDigitalSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "selloSAT", "tfd:TimbreFiscalDigital")
    '    '
    '    reporte.ReportParameters("txtInstrucciones").Value = instrucciones
    '    reporte.ReportParameters("txtPedimento").Value = pedimento
    '    reporte.ReportParameters("txtEnviarA").Value = enviara
    '    reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto
    '    '
    '    reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe, 2).ToString
    '    reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
    '    reporte.ReportParameters("txtDescuento").Value = FormatCurrency(importe_descuento, 2).ToString
    '    reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
    '    reporte.ReportParameters("txtTotal").Value = FormatCurrency(total, 2).ToString

    '    reporte.ReportParameters("txtCadenaOriginal").Value = cadOrigComp
    '    reporte.ReportParameters("txtEmisorRazonSocial").Value = em_razonsocial
    '    reporte.ReportParameters("txtLugarExpedicion").Value = expedicionLinea1 & vbCrLf & expedicionLinea2 & vbCrLf & expedicionLinea3

    '    If porcentaje > 0 Then
    '        reporte.ReportParameters("txtInteres").Value = porcentaje.ToString
    '    End If

    '    Select Case tipoid
    '        Case 5
    '            retencion = FormatNumber((importe * 0.04), 2)
    '            reporte.ReportParameters("txtRetencion").Value = FormatCurrency(retencion, 2).ToString
    '            reporte.ReportParameters("txtTotal").Value = FormatCurrency(total - retencion, 2).ToString
    '            largo = Len(CStr(Format(CDbl(total - retencion), "#,###.00")))
    '            decimales = Mid(CStr(Format(CDbl(total - retencion), "#,###.00")), largo - 2)
    '            If divisaid = 1 Then
    '                CantidadTexto = "( Son " + Num2Text((total - retencion - decimales)) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
    '            Else
    '                CantidadTexto = "( Son " + Num2Text((total - retencion - decimales)) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD )"
    '            End If
    '            reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto.ToString
    '        Case 11
    '            reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
    '            reporte.ReportParameters("txtSubTotal").Value = FormatCurrency(importe + iva, 2).ToString
    '            retencion = FormatNumber((importesindescuento * 0.005), 2)
    '            reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(retencion, 2).ToString
    '            reporte.ReportParameters("txtTotal").Value = FormatCurrency(total - retencion, 2).ToString
    '            largo = Len(CStr(Format(CDbl(total - retencion), "#,###.00")))
    '            decimales = Mid(CStr(Format(CDbl(total - retencion), "#,###.00")), largo - 2)
    '            If divisaid = 1 Then
    '                CantidadTexto = "( Son " + Num2Text((total - retencion - decimales)) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
    '            Else
    '                CantidadTexto = "( Son " + Num2Text((total - retencion - decimales)) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD )"
    '            End If
    '            reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto.ToString
    '        Case 12
    '            reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
    '            reporte.ReportParameters("txtSubTotal").Value = FormatCurrency(importe + iva, 2).ToString
    '            retencion = FormatNumber((importesindescuento * 0.009), 2)
    '            reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(retencion, 2).ToString
    '            reporte.ReportParameters("txtTotal").Value = FormatCurrency(total - retencion, 2).ToString
    '            largo = Len(CStr(Format(CDbl(total - retencion), "#,###.00")))
    '            decimales = Mid(CStr(Format(CDbl(total - retencion), "#,###.00")), largo - 2)
    '            If divisaid = 1 Then
    '                CantidadTexto = "( Son " + Num2Text((total - retencion - decimales)) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
    '            Else
    '                CantidadTexto = "( Son " + Num2Text((total - retencion - decimales)) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD )"
    '            End If
    '            reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto.ToString
    '    End Select
    '    '
    '    reporte.ReportParameters("txtRegimen").Value = em_regimen.ToString
    '    reporte.ReportParameters("txtFormaPago").Value = tipopago.ToString
    '    reporte.ReportParameters("txtMetodoPago").Value = formapago.ToString
    '    reporte.ReportParameters("txtNumCtaPago").Value = "Núm. cuenta: " & numctapago.ToString
    '    reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString

    '    If txtOrdenCompra.Text <> "" Then
    '        reporte.ReportParameters("txtOrdenCompra").Value = "NO. ORDEN DE COMPRA: " & txtOrdenCompra.Text.ToString
    '    End If

    '    Dim totalPzas As String
    '    totalPzas = objData.RunSQLScalarQuery("exec pCFD @cmd=34, @cfdid='" & cfdid.ToString & "'")
    '    objData = Nothing

    '    reporte.ReportParameters("txtTotalPiezas").Value = totalPzas.ToString

    '    Return reporte

    'End Function

    Private Function GeneraPDF_Documento(ByVal cfdid As Long) As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim numeroaprobacion As String = ""
        Dim anoAprobacion As String = ""
        Dim fechaHora As String = ""
        Dim noCertificado As String = ""
        Dim razonsocial As String = ""
        Dim callenum As String = ""
        Dim colonia As String = ""
        Dim ciudad As String = ""
        Dim rfc As String = ""
        Dim em_razonsocial As String = ""
        Dim em_callenum As String = ""
        Dim em_colonia As String = ""
        Dim em_ciudad As String = ""
        Dim em_rfc As String = ""
        Dim em_regimen As String = ""
        Dim rec_razonsocial As String = ""
        Dim rec_callenum As String = ""
        Dim rec_colonia As String = ""
        Dim rec_ciudad As String = ""
        Dim rec_rfc As String = ""

        Dim folio_aprobacion As String = ""
        Dim folio_emision As String = ""
        Dim folio_vigencia As String = ""
        Dim folio_rango As String = ""

        Dim importe As Decimal = 0
        Dim importetasacero As Decimal = 0
        Dim iva As Decimal = 0
        Dim total As Decimal = 0
        Dim CantidadTexto As String = ""
        Dim condiciones As String = ""
        Dim enviara As String = ""
        Dim instrucciones As String = ""
        Dim pedimento As String = ""
        Dim retencion As Decimal = 0
        Dim tipoid As Integer = 0
        Dim divisaid As Integer = 1
        Dim expedicionLinea1 As String = ""
        Dim expedicionLinea2 As String = ""
        Dim expedicionLinea3 As String = ""
        Dim porcentaje As Decimal = 0
        Dim plantillaid As Integer = 1
        Dim codigo_cbb As String = ""
        Dim tipopago As String = ""
        Dim formapago As String = ""
        Dim numctapago As String = ""

        Dim ds As DataSet = New DataSet

        Dim cmd As New SqlCommand("EXEC pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", conn)
        conn.Open()
        Dim rs As SqlDataReader
        rs = cmd.ExecuteReader()

        If rs.Read Then
            serie = rs("serie")
            folio = rs("folio")
            'tipoid = rs("tipoid")
            tipoid = cmbTipoDocumento.SelectedValue
            em_razonsocial = rs("em_razonsocial")
            em_callenum = rs("em_callenum")
            em_colonia = rs("em_colonia")
            em_ciudad = rs("em_ciudad")
            em_rfc = rs("em_rfc")
            em_regimen = rs("regimen")
            '
            rec_razonsocial = rs("razonsocial")
            rec_callenum = rs("callenum")
            rec_colonia = rs("colonia")
            rec_ciudad = rs("ciudad")
            rec_rfc = rs("rfc")
            '
            folio_aprobacion = rs("folio_aprobacion")
            folio_emision = rs("folio_emision")
            folio_vigencia = rs("folio_vigencia")
            folio_rango = rs("folio_rango")
            '
            razonsocial = rs("razonsocial")
            callenum = rs("callenum")
            colonia = rs("colonia")
            ciudad = rs("ciudad")
            rfc = rs("rfc")
            importe = rs("importe")
            importetasacero = rs("importetasacero")
            iva = rs("iva")
            total = rs("total")
            divisaid = rs("divisaid")
            fechaHora = rs("fecha_factura").ToString
            condiciones = "Condiciones: " & rs("condiciones").ToString
            enviara = rs("enviara").ToString
            instrucciones = rs("instrucciones")
            If rs("aduana") = "" Or rs("numero_pedimento") = "" Then
                pedimento = ""
            Else
                pedimento = "Aduana: " & rs("aduana") & vbCrLf & "Fecha: " & rs("fecha_pedimento").ToString & vbCrLf & "Número: " & rs("numero_pedimento").ToString
            End If
            expedicionLinea1 = rs("expedicionLinea1")
            expedicionLinea2 = rs("expedicionLinea2")
            expedicionLinea3 = rs("expedicionLinea3")
            porcentaje = rs("porcentaje")
            plantillaid = rs("plantillaid")
            tipocontribuyenteid = rs("tipocontribuyenteid")
            codigo_cbb = rs("codigo_cbb")
            tipopago = rs("tipopago")
            formapago = rs("formapago")
            numctapago = rs("numctapago")
        End If
        rs.Close()

        conn.Close()
        conn.Dispose()
        conn = Nothing

        Dim largo = Len(CStr(Format(CDbl(total), "#,###.00")))
        Dim decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)

        If System.Configuration.ConfigurationManager.AppSettings("divisas") = 1 Then
            If divisaid = 1 Then
                CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
            Else
                CantidadTexto = "( Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD. )"
            End If
        Else
            CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
        End If


        Dim reporte As New Formatos.formato_cbb_neogenis


        reporte.ReportParameters("plantillaId").Value = plantillaid
        reporte.ReportParameters("cfdiId").Value = cfdid
        reporte.ReportParameters("txtFechaEmision").Value = Now.ToShortDateString

        Select Case tipoid
            Case 10
                reporte.ReportParameters("txtDocumento").Value = "Remisión No.    " & serie.ToString & folio.ToString
                reporte.ReportParameters("txtObservaciones4").Value = "NOTA: ESTE COMPROBANTE NO TIENE VALOR FISCAL"
        End Select

        reporte.ReportParameters("txtNoAprobacion").Value = "Aprobación No. " & folio_aprobacion.ToString
        reporte.ReportParameters("txtEmision").Value = folio_emision.ToString
        reporte.ReportParameters("txtRango").Value = folio_rango.ToString

        reporte.ReportParameters("txtCondicionesPago").Value = condiciones
        reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb/nocbb.png")
        reporte.ReportParameters("txtLeyenda").Value = ""

        reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
        reporte.ReportParameters("txtClienteRazonSocial").Value = rec_razonsocial.ToString
        reporte.ReportParameters("txtClienteCalleNum").Value = rec_callenum.ToString
        reporte.ReportParameters("txtClienteColonia").Value = rec_colonia.ToString
        reporte.ReportParameters("txtClienteCiudadEstado").Value = rec_ciudad.ToString
        reporte.ReportParameters("txtClienteRFC").Value = rec_rfc.ToString
        '
        '
        reporte.ReportParameters("txtInstrucciones").Value = instrucciones
        reporte.ReportParameters("txtPedimento").Value = pedimento
        reporte.ReportParameters("txtEnviarA").Value = enviara

        '
        reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
        reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
        reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe, 2).ToString
        reporte.ReportParameters("txtRetIVA").Value = FormatCurrency(0, 2).ToString
        reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
        reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva), 2).ToString
        '
        '   Ajusta cantidad con texto
        '
        total = FormatNumber((importe + iva), 2)
        largo = Len(CStr(Format(CDbl(total), "#,###.00")))
        decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
        '
        CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"

        reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto
        '
        '
        reporte.ReportParameters("txtEmisorRazonSocial").Value = em_razonsocial
        reporte.ReportParameters("txtLugarExpedicion").Value = expedicionLinea1 & vbCrLf & expedicionLinea2 & vbCrLf & expedicionLinea3
        If porcentaje > 0 Then
            reporte.ReportParameters("txtInteres").Value = porcentaje.ToString
        End If
        '
        '
        reporte.ReportParameters("txtRegimen").Value = em_regimen.ToString
        reporte.ReportParameters("txtFormaPago").Value = tipopago.ToString
        reporte.ReportParameters("txtMetodoPago").Value = formapago.ToString
        reporte.ReportParameters("txtNumCtaPago").Value = "Núm. cuenta: " & numctapago.ToString
        reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString
        reporte.ReportParameters("txtLeyenda").Value = ""

        Dim totalPzas As String
        Dim objData As New DataControl
        totalPzas = objData.RunSQLScalarQuery("exec pCFD @cmd=34, @cfdid='" & cfdid.ToString & "'")
        objData = Nothing

        reporte.ReportParameters("txtTotalPiezas").Value = totalPzas.ToString

        '
        Return reporte
    End Function

#End Region

    Protected Sub chkAduana_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAduana.CheckedChanged
        panelInformacionAduanera.Visible = chkAduana.Checked
        valNombreAduana.Enabled = chkAduana.Checked
        valFechaPedimento.Enabled = chkAduana.Checked
        valNumeroPedimento.Enabled = chkAduana.Checked
    End Sub

    Protected Sub cmbMoneda_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMoneda.SelectedIndexChanged
        txtTipoCambio.Text = 0
        If cmbMoneda.SelectedValue <> 1 Then
            txtTipoCambio.Enabled = True
        Else
            txtTipoCambio.Enabled = False
        End If
    End Sub

    Protected Sub cmbTipoDocumento_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbTipoDocumento.SelectedIndexChanged
        If cmbTipoDocumento.SelectedValue > 0 Then
            cmbTipoDocumento.Enabled = False
            If cmbTipoDocumento.SelectedValue = 2 Then
                lblTipoRelacion.Visible = True
                lblUUID.Visible = True
                cmbTipoRelacion.Visible = True
                cmbUUID.Visible = True
                valTipoRelecion.Enabled = True
                valFolioFiscal.Enabled = True
                panelFacturaGlobal.Visible = False
            End If

            If cmbTipoDocumento.SelectedValue = 4 Then
                txtTipoCambio.Enabled = True
            End If
        End If
    End Sub

    Protected Sub btnCancelSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSearch.Click
        gridResults.Visible = False
        itemsList.Visible = True
        txtSearchItem.Text = ""
        txtSearchItem.Focus()
        btnCancelSearch.Visible = False
        btnAgregaConceptos.Visible = False
    End Sub

    Protected Sub gridResults_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles gridResults.ItemCommand
        Select Case e.CommandName
            Case "cmdAdd"
                If Session("CFD") = 0 Then
                    GetCFD()
                End If
                InsertItem(e.CommandArgument, e.Item)
        End Select
    End Sub

    Protected Sub txtSearchItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchItem.TextChanged
        gridResults.Visible = True
        itemsList.Visible = False
        Dim objdata As New DataControl
        Dim cmd As String = "exec pCFD @cmd=30, @txtSearch='" & txtSearchItem.Text & "', @clienteid='" & cmbCliente.SelectedValue.ToString & "', @almacenid='" & cmbAlmacen.SelectedValue.ToString & "'"
        gridResults.DataSource = objdata.FillDataSet(cmd)
        gridResults.DataBind()
        objdata = Nothing
        txtSearchItem.Text = ""
        txtSearchItem.Focus()
        btnCancelSearch.Visible = True
        btnAgregaConceptos.Visible = True
    End Sub

    Protected Sub gridResults_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles gridResults.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim lblDisponibles As Label = DirectCast(e.Item.FindControl("lblDisponibles"), Label)
                Dim txtUnitaryPrice As RadNumericTextBox = DirectCast(e.Item.FindControl("txtUnitaryPrice"), RadNumericTextBox)
                Dim cmbUnidadAduana As DropDownList = DirectCast(e.Item.FindControl("cmbUnidadAduana"), DropDownList)

                Dim ObjCat As New DataControl
                ObjCat.Catalogo(cmbUnidadAduana, "select clave, clave + ' - ' + nombre as descripcion from tblUnidadAduana order by nombre", 0)
                ObjCat = Nothing

                txtUnitaryPrice.Text = e.Item.DataItem("precio")

                If e.Item.DataItem("inventariableBit") = False Then
                    lblDisponibles.Text = "N/A"
                End If
        End Select
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.DataItem("inventariableBit") = False Then
                dataItem("existencia").Text = "N/A"
            End If
        End If
    End Sub

    Protected Sub cmbCliente_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbCliente.SelectedIndexChanged
        Call CargaSucursales()
        If cmbSucursal.SelectedValue <> "" And cmbSucursal.SelectedValue <> "0" And cmbAlmacen.SelectedValue <> "" And cmbAlmacen.SelectedValue <> 0 And cmbProyecto.SelectedValue <> "" And cmbProyecto.SelectedValue <> 0 Then
            Call CargaCliente(cmbCliente.SelectedValue)
            'Call CargaTipoPrecio()
            Call ClearItems()
        Else
            panelSpecificClient.Visible = False
            panelItemsRegistration.Visible = False
        End If
        Try
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(cmbUUID, "select top 4800 uuid, convert(varchar(10), fecha_factura, 103) + ' | ' + isnull(serie,'') + convert(varchar(10), folio) + ' - ' + isnull(uuid,'') as folio from tblCFD where isnull(uuid,'')<>'' and (estatus=1 or estatus=3) and (serie='A' or serie='FD')  and clienteid='" & cmbCliente.SelectedValue.ToString & "' order by fecha_factura desc", 0)
            ObjCat = Nothing
        Catch ex As Exception

        End Try
    End Sub

    Private Sub CargaSucursales()

        Dim ObjData As New DataControl
        ObjData.Catalogo(cmbSucursal, "EXEC pListarSucursales  @clienteid='" & cmbCliente.SelectedValue & "'", 0)
        ObjData = Nothing

    End Sub

    Private Sub CargaTipoPrecio()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("SELECT TP.nombre FROM tblTipoPrecio TP INNER JOIN tblSucursal S ON TP.id = S.tipoprecioId AND S.id='" & cmbSucursal.SelectedValue & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read() Then
                lblTipoPrecioValue.Text = rs("nombre")
            End If

            rs.Close()

            conn.Close()
            conn.Dispose()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try
    End Sub

    Protected Sub btnCancelInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelInvoice.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pCFD @cmd=31, @cfdid='" & Session("CFD").ToString & "'")
        ObjData = Nothing
        '
        Session("CFD") = 0
        '
        Response.Redirect("~/portalcfd/cfd.aspx")
        '
        ''
    End Sub

    'Public Function Truncate(ByVal number As Decimal, ByVal decimals As Integer) As Decimal
    '    Dim Multiplicador = Math.Pow(10, decimals)
    '    Return Math.Truncate(number * Multiplicador) / Multiplicador
    'End Function

    ' Esta función recibe un número y devuelve una cadena de caracteres conteniendo el texto correspondiente al número recibido. 
    ' Los decimales (centavos) se colocan literalmente al final de la cadena con el formato xx/100 (xx son los dígitos del valor decimal). 
    ' La función "habla" sobre todo en número de más de miles de millones. 

    Public Function Num2Text(ByVal nCifra As Object) As String
        ' Defino variables 
        Dim cifra, bloque, decimales, cadena As String
        Dim longituid, posision, unidadmil As Byte

        ' En caso de que unidadmil sea: 
        ' 0 = cientos 
        ' 1 = miles 
        ' 2 = millones 
        ' 3 = miles de millones 
        ' 4 = billones 
        ' 5 = miles de billones 

        ' Reemplazo el símbolo decimal por un punto (.) y luego guardo la parte entera y la decimal por separado 
        ' Es necesario poner el cero a la izquierda del punto así si el valor es de sólo decimales, se lo fuerza 
        ' a colocar el cero para que no genere error 
        cifra = Format(CType(nCifra, Decimal), "###############0.#0")
        decimales = Mid(cifra, Len(cifra) - 1, 2)
        cifra = Left(cifra, Len(cifra) - 3)

        ' Verifico que el valor no sea cero 
        If cifra = "0" Then
            Return IIf(decimales = "00", "cero", "cero con " & decimales & "/100")
        End If

        ' Evaluo su longitud (como mínimo una cadena debe tener 3 dígitos) 
        If Len(cifra) < 3 Then
            cifra = Rellenar(cifra, 3)
        End If

        ' Invierto la cadena 
        cifra = Invertir(cifra)

        ' Inicializo variables 
        posision = 1
        unidadmil = 0
        cadena = ""

        ' Selecciono bloques de a tres cifras empezando desde el final (de la cadena invertida) 
        Do While posision <= Len(cifra)
            ' Selecciono una porción del numero 
            bloque = Mid(cifra, posision, 3)

            ' Transformo el número a cadena 
            cadena = Convertir(bloque, unidadmil) & " " & cadena.Trim

            ' Incremento la cantidad desde donde seleccionar la subcadena 
            posision = posision + 3

            ' Incremento la posisión de la unidad de mil 
            unidadmil = unidadmil + 1
        Loop

        ' Cargo la función 
        Return IIf(decimales = "00", cadena.Trim.ToLower, cadena.Trim.ToLower & " con " & decimales & "/100")
    End Function

    ' Esta función es complemento de la función de conversión. 
    ' En los arrays se agrega una posisión inicial vacía ya que VB.NET empieza de la posisión cero 

    Private Function Convertir(ByVal cadena As String, ByVal unidadmil As Byte) As String
        ' Defino variables 
        Dim centena, decena, unidad As Byte

        ' Invierto la subcadena (la original habia sido invertida en el procedimiento NumeroATexto) 
        cadena = Invertir(cadena)

        ' Determino la longitud de la cadena 
        If Len(cadena) < 3 Then
            cadena = Rellenar(cadena, 3)
        End If

        ' Verifico que la cadena no esté vacía (000) 
        If cadena = "000" Then
            Return ""
        End If

        ' Desarmo el numero (empiezo del dígito cero por el manejo de cadenas de VB.NET) 
        centena = CType(cadena.Substring(0, 1), Byte)
        decena = CType(cadena.Substring(1, 1), Byte)
        unidad = CType(cadena.Substring(2, 1), Byte)
        cadena = ""

        ' Calculo las centenas 
        If centena <> 0 Then
            Dim centenas() As String = {"", IIf(decena = 0 And unidad = 0, "cien", "ciento"), "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"}
            cadena = centenas(centena)
        End If

        ' Calculo las decenas 
        If decena <> 0 Then
            Dim decenas() As String = {"", IIf(unidad = 0, "diez", IIf(unidad >= 6, "dieci", IIf(unidad = 1, "once", IIf(unidad = 2, "doce", IIf(unidad = 3, "trece", IIf(unidad = 4, "catorce", "quince")))))), IIf(unidad = 0, "veinte", "venti"), "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"}
            cadena = cadena & " " & decenas(decena)
        End If

        ' Calculo las unidades (no pregunten por que este IF es necesario ... simplemente funciona) 
        If decena = 1 And unidad < 6 Then
        Else
            Dim unidades() As String = {"", IIf(decena <> 1, IIf(unidadmil = 1, "un", "uno"), ""), "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve"}
            If decena >= 3 And unidad <> 0 Then
                cadena = cadena.Trim & " y "
            End If

            If decena = 0 Then
                cadena = cadena.Trim & " "
            End If
            cadena = cadena & unidades(unidad)
        End If

        ' Evaluo la posision de miles, millones, etc 
        If unidadmil <> 0 Then
            Dim agregado() As String = {"", "mil", IIf((centena = 0) And (decena = 0) And (unidad = 1), "millón", "millones"), "mil millones", "billones", "mil billones"}
            If (centena = 0) And (decena = 0) And (unidad = 1) And unidadmil = 2 Then
                cadena = "un"
            End If
            cadena = cadena & " " & agregado(unidadmil)
        End If

        ' Cargo la función 
        Return cadena.Trim
    End Function

    ' Esta función recibe una cadena de caracteres y la devuelve "invertida". 
    Public Function Invertir(ByVal cadena As String) As String
        ' Defino variables 
        Dim retornar As String

        ' Inviero la cadena 
        For posision As Short = cadena.Length To 1 Step -1
            retornar = retornar & cadena.Substring(posision - 1, 1)
        Next

        ' Retorno la cadena invertida 
        Return retornar
    End Function

    ' Esta función rellena con ceros a la izquierda un número pasado como parámetro. Con el parámetro "cifras" se especifica la cantidad de dígitos a la izquierda. 
    Public Function Rellenar(ByVal valor As Object, ByVal cifras As Byte) As String
        ' Defino variables 
        Dim cadena As String

        ' Verifico el valor pasado 
        If Not IsNumeric(valor) Then
            valor = 0
        Else
            valor = CType(valor, Integer)
        End If

        ' Cargo la cadena 
        cadena = valor.ToString.Trim

        ' Relleno con los ceros que sean necesarios para llenar los dígitos pedidos 
        For puntero As Byte = (Len(cadena) + 1) To cifras
            cadena = "0" & cadena
        Next puntero

        ' Cargo la función 
        Return cadena
    End Function

    Private Sub cmbSucursal_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSucursal.SelectedIndexChanged
        If cmbSucursal.SelectedValue <> "" And cmbSucursal.SelectedValue <> "0" And cmbAlmacen.SelectedValue <> "" And cmbAlmacen.SelectedValue <> 0 And cmbProyecto.SelectedValue <> "" And cmbProyecto.SelectedValue <> 0 Then
            Call CargaCliente(cmbCliente.SelectedValue)
            'Call CargaTipoPrecio()
            Call ClearItems()
        Else
            panelSpecificClient.Visible = False
            panelItemsRegistration.Visible = False
        End If
    End Sub

    Private Sub almacenid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAlmacen.SelectedIndexChanged
        If cmbSucursal.SelectedValue <> "" And cmbSucursal.SelectedValue <> "0" And cmbAlmacen.SelectedValue <> "" And cmbAlmacen.SelectedValue <> 0 And cmbProyecto.SelectedValue <> "" And cmbProyecto.SelectedValue <> 0 Then
            Call CargaCliente(cmbCliente.SelectedValue)
            'Call CargaTipoPrecio()
            Call ClearItems()
        Else
            panelSpecificClient.Visible = False
            panelItemsRegistration.Visible = False
        End If
    End Sub

    Private Sub btnAgregaConceptos_Click(sender As Object, e As EventArgs) Handles btnAgregaConceptos.Click
        '
        Dim productoid As Long = 0
        Dim dblCantidad As Double = 0
        Dim disponibles As Double = 0
        Dim cantidad As Double = 0

        Dim ObjData As New DataControl
        For Each row As GridDataItem In gridResults.MasterTableView.Items
            productoid = row.GetDataKeyValue("productoid")
            disponibles = row.GetDataKeyValue("disponibles")

            Dim lblCodigo As Label = DirectCast(row.FindControl("lblCodigo"), Label)
            Dim txtDescripcion As System.Web.UI.WebControls.TextBox = DirectCast(row.FindControl("txtDescripcion"), System.Web.UI.WebControls.TextBox)
            Dim lblUnidad As Label = DirectCast(row.FindControl("lblUnidad"), Label)
            Dim txtQuantity As RadNumericTextBox = DirectCast(row.FindControl("txtQuantity"), RadNumericTextBox)
            Dim txtUnitaryPrice As RadNumericTextBox = DirectCast(row.FindControl("txtUnitaryPrice"), RadNumericTextBox)
            Dim lblDisponibles As Label = DirectCast(row.FindControl("lblDisponibles"), Label)

            If txtQuantity.Text = "" Then
                cantidad = 0
            Else
                cantidad = txtQuantity.Text
            End If

            Try
                dblCantidad = Convert.ToDecimal(txtQuantity.Text)
            Catch ex As Exception
                dblCantidad = 0
            End Try

            If dblCantidad > 0 Then
                If disponibles >= dblCantidad Or cmbTipoDocumento.SelectedValue = 2 Or lblDisponibles.Text = "N/A" Then
                    '
                    If Session("CFD") = 0 Then
                        GetCFD()
                    End If
                    '
                    '   Agrega la partida
                    '
                    Dim porcentaje_descuento As String
                    Dim descuento As String
                    porcentaje_descuento = ObjData.RunSQLScalarQueryDecimal("EXEC pMisClientes @cmd=7, @clienteid='" & cmbCliente.SelectedValue.ToString & "'")
                    descuento = ((Convert.ToDecimal(porcentaje_descuento) * (Convert.ToDecimal(txtQuantity.Text) * Convert.ToDecimal(txtUnitaryPrice.Text))) / 100)
                    ObjData.RunSQLQuery("EXEC pCFD @cmd=2, @cfdid='" & Session("CFD").ToString & "', @codigo='" & lblCodigo.Text & "', @descripcion='" & txtDescripcion.Text & "', @cantidad='" & txtQuantity.Text & "', @unidad='" & lblUnidad.Text & "', @precio='" & txtUnitaryPrice.Text & "', @productoid='" & productoid.ToString & "', @importe_descuento='" & descuento.ToString & "'")
                End If
            End If
        Next
        ObjData = Nothing
        '
        DisplayItems()
        Call CargaTotales()
        panelResume.Visible = True
        panelDescuento.Visible = True
        gridResults.Visible = False
        itemsList.Visible = True
        txtSearchItem.Text = ""
        txtSearchItem.Focus()
        btnCancelSearch.Visible = False
        btnAgregaConceptos.Visible = False
        lblMensaje.Text = ""
        '
    End Sub

    Private Sub proyectoid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProyecto.SelectedIndexChanged
        If cmbSucursal.SelectedValue <> "" And cmbSucursal.SelectedValue <> "0" And cmbAlmacen.SelectedValue <> "" And cmbAlmacen.SelectedValue <> 0 And cmbProyecto.SelectedValue <> "" And cmbProyecto.SelectedValue <> 0 Then
            Call CargaCliente(cmbCliente.SelectedValue)
            'Call CargaTipoPrecio()
            Call ClearItems()
        Else
            panelSpecificClient.Visible = False
            panelItemsRegistration.Visible = False
        End If
    End Sub

    Private Sub btnAplicarDescuento_Click(sender As Object, e As EventArgs) Handles btnAplicarDescuento.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("EXEC pCFD @cmd=43, @cfdid='" & Session("CFD").ToString & "', @porcentaje_descuento='" & txtDescuento.Text.ToString & "'")
        ObjData = Nothing

        Call CargaTotales()

    End Sub

    Private Function CrearDOM() As XmlDocument
        Dim oDOM As New XmlDocument
        Dim Nodo As XmlNode
        Nodo = oDOM.CreateProcessingInstruction("xml", "version=""1.0"" encoding=""utf-8""")
        oDOM.AppendChild(Nodo)
        Nodo = Nothing
        CrearDOM = oDOM
    End Function

    Private Function CrearNodoComprobante(ByVal TipoDeComprobante As String) As XmlNode
        Dim Comprobante As XmlNode
        Comprobante = m_xmlDOM.CreateElement("cfdi:Comprobante", URI_SAT)
        CrearAtributosComprobante(Comprobante, TipoDeComprobante)
        CrearNodoComprobante = Comprobante
    End Function

    Private Sub CrearAtributosComprobante(ByVal Nodo As XmlElement, ByVal TipoDeComprobante As String)
        Nodo.SetAttribute("xmlns:cfdi", URI_SAT)
        Nodo.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        If cmbComplementoFactura.SelectedValue = 7 Then
            Nodo.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd http://www.sat.gob.mx/ComercioExterior20 http://www.sat.gob.mx/sitio_internet/cfd/ComercioExterior20/ComercioExterior20.xsd")
            Nodo.SetAttribute("xmlns:cce20", "http://www.sat.gob.mx/ComercioExterior20")
        Else
            Nodo.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd")
        End If
        Nodo.SetAttribute("Version", "4.0")

        If serie.ToString.Length > 0 Then
            Nodo.SetAttribute("Serie", serie)
        End If

        Nodo.SetAttribute("Folio", folio)
        Nodo.SetAttribute("Fecha", Format(Now(), "yyyy-MM-ddThh:mm:ss"))
        Nodo.SetAttribute("Sello", "")
        Nodo.SetAttribute("FormaPago", cmbFormaPago.SelectedValue.ToString) '01,02,03,04,05,06,07...
        Nodo.SetAttribute("NoCertificado", "")
        Nodo.SetAttribute("Certificado", "")

        If cmbCondiciones.SelectedValue > 0 Then
            Nodo.SetAttribute("CondicionesDePago", cmbCondiciones.SelectedItem.Text) 'CREDITO, CONTADO, CREDITO A 3 MESES ETC
        End If

        Nodo.SetAttribute("SubTotal", Math.Round(subtotal, 2))

        If descuento > 0 Then
            Nodo.SetAttribute("Descuento", Format(CDbl(descuento), "0.#0"))
        End If

        Dim moneda As String = ""
        Dim ObjData As New DataControl
        moneda = ObjData.RunSQLScalarQueryString("select isnull(clave,'') from tblMoneda where id='" & cmbMoneda.SelectedValue.ToString & "'")
        ObjData = Nothing

        If (moneda <> "MXN" And moneda <> "") Then
            Nodo.SetAttribute("Moneda", moneda)
            Nodo.SetAttribute("TipoCambio", txtTipoCambio.Text)
        Else
            Nodo.SetAttribute("Moneda", "MXN")
        End If

        Nodo.SetAttribute("Total", Math.Round(total, 2))
        Nodo.SetAttribute("TipoDeComprobante", TipoDeComprobante)
        Nodo.SetAttribute("MetodoPago", cmbMetodoPago.SelectedValue.ToString) 'PUE, PID, PPD
        Nodo.SetAttribute("LugarExpedicion", CargaLugarExpedicion())
        Nodo.SetAttribute("Exportacion", cmbExportacion.SelectedValue.ToString) '01, 02, 03
    End Sub

    Private Function CrearNodo(ByVal Nombre As String) As XmlNode
        If urlcomplemento = 0 Then
            CrearNodo = m_xmlDOM.CreateNode(XmlNodeType.Element, Nombre, URI_SAT)
        ElseIf urlcomplemento = 1 Then
            CrearNodo = m_xmlDOM.CreateNode(XmlNodeType.Element, Nombre, "http://www.sat.gob.mx/detallista")
        ElseIf urlcomplemento = 2 Then
            CrearNodo = m_xmlDOM.CreateNode(XmlNodeType.Element, Nombre, URI_SAT_EXT)
        End If
    End Function

    Private Sub IndentarNodo(ByVal Nodo As XmlNode, ByVal Nivel As Long)
        Nodo.AppendChild(m_xmlDOM.CreateTextNode(vbNewLine & New String(ControlChars.Tab, Nivel)))
    End Sub

    Private Sub CrearNodoCfdiRelacionados(ByVal Nodo As XmlNode)
        Dim CfdiRelacionados As XmlElement
        Dim DocumentoRelacionado As XmlElement

        CfdiRelacionados = CrearNodo("cfdi:CfdiRelacionados")
        IndentarNodo(CfdiRelacionados, 1)

        CfdiRelacionados.SetAttribute("TipoRelacion", cmbTipoRelacion.SelectedValue.ToString)
        IndentarNodo(CfdiRelacionados, 2)

        'DocumentoRelacionado = CrearNodo("cfdi:CfdiRelacionado")
        'DocumentoRelacionado.SetAttribute("UUID", cmbUUID.SelectedValue.ToString)

        'CfdiRelacionados.AppendChild(DocumentoRelacionado)
        'IndentarNodo(CfdiRelacionados, 1)
        'Nodo.AppendChild(CfdiRelacionados)
        Dim conn7 As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        conn7.Open()
        Dim SqlCommand7 As SqlCommand = New SqlCommand("exec pCFD @cmd=46, @cfdid='" & Session("CFD").ToString & "'", conn7)
        Dim rs7 = SqlCommand7.ExecuteReader
        While rs7.Read

            IndentarNodo(CfdiRelacionados, 2)
            DocumentoRelacionado = CrearNodo("cfdi:CfdiRelacionado")
            DocumentoRelacionado.SetAttribute("UUID", rs7("uuids_relacionados"))
            CfdiRelacionados.AppendChild(DocumentoRelacionado)
            IndentarNodo(CfdiRelacionados, 1)

            contador += 1

        End While

        conn7.Close()
        conn7.Dispose()
        conn7 = Nothing
        rs7 = Nothing

        Nodo.AppendChild(CfdiRelacionados)

    End Sub

    Private Sub CrearNodoEmisor(ByVal Nodo As XmlNode, ByVal nombre As String, ByVal rfc As String, ByVal Regimen As String)
        Try
            Dim Emisor As XmlElement
            Emisor = CrearNodo("cfdi:Emisor")
            Emisor.SetAttribute("Nombre", nombre.ToUpper)
            Emisor.SetAttribute("Rfc", rfc)
            Emisor.SetAttribute("RegimenFiscal", Regimen)
            Nodo.AppendChild(Emisor)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CrearNodoReceptor(ByVal Nodo As XmlNode, ByVal nombre As String, ByVal rfc As String, ByVal UsoCFDI As String, ByVal DomicilioFiscalReceptor As String, ByVal ResidenciaFiscal As String, ByVal NumRegIdTrib As String, ByVal RegimenFiscalReceptor As String, ByVal identidad_tributaria As String, ByVal clave_pais As String)
        Try
            Dim Receptor As XmlElement
            Receptor = CrearNodo("cfdi:Receptor")
            Receptor.SetAttribute("Rfc", rfc)
            Receptor.SetAttribute("Nombre", nombre.ToUpper)
            Receptor.SetAttribute("RegimenFiscalReceptor", RegimenFiscalReceptor)
            If DomicilioFiscalReceptor.Length > 0 Then
                Receptor.SetAttribute("DomicilioFiscalReceptor", DomicilioFiscalReceptor)
            End If
            If rfc.Trim = "XEXX010101000" Then
                Receptor.SetAttribute("NumRegIdTrib", identidad_tributaria)
                Receptor.SetAttribute("ResidenciaFiscal", clave_pais)
            End If
            Receptor.SetAttribute("UsoCFDI", UsoCFDI)
            Nodo.AppendChild(Receptor)
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub CrearNodoImpuestos(ByVal Nodo As XmlNode)
        Dim AgregarTraslado As Boolean = False
        Dim TasaOCuotas As String = ""
        Dim TipoFactor As String = ""
        Dim TipoImpuesto As String = ""
        Dim Impuestos As XmlElement
        Dim Traslados As XmlElement
        Dim Traslado As XmlElement

        Call CargaTotales()

        Dim Retenciones As XmlElement
        Dim Retencion As XmlElement

        Impuestos = CrearNodo("cfdi:Impuestos")

        If iva > 0 Then
            Impuestos.SetAttribute("TotalImpuestosTrasladados", Format(iva, "#0.00"))
            'Impuestos.SetAttribute("TotalImpuestosTrasladados", Math.Round(iva, 2))
        Else
            Impuestos.SetAttribute("TotalImpuestosTrasladados", "0.00")
        End If

        'Impuestos.SetAttribute("TotalImpuestosRetenidos", "0.00")

        If iva > 0 Then
            TasaOCuotas = "0.160000"
            AgregarTraslado = True
            TipoFactor = "Tasa"
            TipoImpuesto = "002"
        Else
            TasaOCuotas = "0.000000"
            TipoFactor = "Tasa"
            AgregarTraslado = True
            TipoFactor = "Tasa"
            TipoImpuesto = "002"
        End If

        If AplicarRetencion = True Then
            '
            '   Retenciones
            '
            Select Case cmbTipoDocumento.SelectedValue
                Case 3, 6   '   Recibos de honorarios o arrendamiento
                    '
                    '   Retenciones
                    '
                    If tipocontribuyenteid = 1 Then
                    Else
                        '
                        '   ISR
                        '
                        Dim ImporteISR As Double = 0
                        Dim ImporteIVA As Double = 0

                        Retenciones = CrearNodo("cfdi:Retenciones")
                        IndentarNodo(Retenciones, 3)
                        Impuestos.AppendChild(Retenciones)

                        '
                        '   ISR
                        '
                        Retencion = CrearNodo("cfdi:Retencion")
                        Retencion.SetAttribute("Impuesto", "001")
                        ImporteISR = Math.Round((subtotal * 0.1), 2)

                        If ImporteISR >= 2000 Then
                            ImporteISR = Math.Round((subtotal * 0.1), 2)
                        End If
                        Retencion.SetAttribute("Importe", Math.Round(CDbl(ImporteISR), 2))
                        Retenciones.AppendChild(Retencion)
                        '
                        '  IVA
                        '
                        Retencion = CrearNodo("cfdi:Retencion")
                        Retencion.SetAttribute("Impuesto", "002")
                        ImporteIVA = Math.Round((iva / 3) * 2, 2)
                        If ImporteIVA >= 2000 Then
                            ImporteIVA = Math.Round((subtotal * 0.106667), 2)
                        End If
                        Retencion.SetAttribute("Importe", Math.Round(CDbl(ImporteIVA), 2))
                        Retenciones.AppendChild(Retencion)

                        IndentarNodo(Retenciones, 2)

                        Impuestos.AppendChild(Retenciones)
                        IndentarNodo(Impuestos, 1)

                        Impuestos.SetAttribute("TotalImpuestosRetenidos", Math.Round(CDbl(ImporteISR + ImporteIVA), 2))
                        total = Math.Round((total - (ImporteISR) - (ImporteIVA)), 2)
                    End If
                Case 5  ' Carta porte

                Case 7  ' Factura con Retención de 2/3 partes del IVA

                Case 11 ' Retención de 5 al millar (0.5 %)

                Case 13 ' Retención de 16%

                Case 14 ' Honorarios con Retención de 2/3 partes del IVA

            End Select
        End If

        Traslados = CrearNodo("cfdi:Traslados")
        IndentarNodo(Traslados, 3)
        Impuestos.AppendChild(Traslados)

        If AgregarTraslado = True Then

            Traslado = CrearNodo("cfdi:Traslado")
            Traslado.SetAttribute("Impuesto", TipoImpuesto)
            Traslado.SetAttribute("TipoFactor", TipoFactor)
            Traslado.SetAttribute("TasaOCuota", TasaOCuotas)
            If iva > 0 Then
                Traslado.SetAttribute("Importe", Math.Round(iva, 2))
            Else
                Traslado.SetAttribute("Importe", "0.00")
            End If
            Traslado.SetAttribute("Base", Format(CDbl(subtotal - totaldescuento), "0.#0"))
            Traslados.AppendChild(Traslado)

        End If

        IndentarNodo(Traslados, 2)
        Impuestos.AppendChild(Traslados)
        IndentarNodo(Impuestos, 1)
        Nodo.AppendChild(Impuestos)

    End Sub

    Private Function LeerCertificado() As String
        Dim Certificado As String = ""

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try
            Dim cmd As New SqlCommand("exec pCFD @cmd=19, @clienteid='" & Session("clienteid").ToString & "', @cfdid='" & Session("CFD").ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                Certificado = rs("archivo_certificado")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return Certificado

    End Function

    Private Function Leerllave() As String
        Dim Llave As String = ""

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try
            Dim cmd As New SqlCommand("exec pCFD @cmd=19", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                Llave = rs("archivo_llave_privada")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return Llave

    End Function

    Private Function LeerClave() As String
        Dim Contrasena As String = ""

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try
            Dim cmd As New SqlCommand("exec pCFD @cmd=19", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                Contrasena = rs("contrasena_llave_privada")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return Contrasena

    End Function

    Public Function FormatearSerieCert(ByVal Serie As String) As String
        Dim Resultado As String = ""
        Dim I As Integer
        For I = 2 To Len(Serie) Step 2
            Resultado = Resultado & Mid(Serie, I, 1)
        Next
        FormatearSerieCert = Resultado
    End Function

    Function ReadFile(ByVal strArchivo As String) As Byte()
        Dim f As New FileStream(strArchivo, FileMode.Open, FileAccess.Read)
        Dim size As Integer = CInt(f.Length)
        Dim data As Byte() = New Byte(size - 1) {}
        size = f.Read(data, 0, size)
        f.Close()
        Return data
    End Function

    Private Sub SellarCFD(ByVal NodoComprobante As XmlElement)
        Try
            Dim Certificado As String = ""
            Certificado = LeerCertificado()

            Dim Clave As String = ""
            Clave = LeerClave()

            Dim objCert As New X509Certificate2()
            Dim bRawData As Byte() = ReadFile(Server.MapPath("~/portalcfd/certificados/") & Certificado)
            objCert.Import(bRawData)
            Dim cadena As String = Convert.ToBase64String(bRawData)
            NodoComprobante.SetAttribute("NoCertificado", FormatearSerieCert(objCert.SerialNumber))
            NodoComprobante.SetAttribute("Total", Format(total, "#0.00"))
            NodoComprobante.SetAttribute("Certificado", Convert.ToBase64String(bRawData))
            NodoComprobante.SetAttribute("Sello", GenerarSello(Clave))
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Function GenerarSello(ByVal Clave As String) As String
        Try
            Dim pkey As New Chilkat.PrivateKey
            Dim pkeyXml As String
            Dim rsa As New Chilkat.Rsa
            pkey.LoadPkcs8EncryptedFile(Server.MapPath("~/portalcfd/llave/") & Leerllave(), Clave)
            pkeyXml = pkey.GetXml()
            rsa.UnlockComponent("RSAT34MB34N_7F1CD986683M")
            rsa.ImportPrivateKey(pkeyXml)
            rsa.Charset = "utf-8"
            rsa.EncodingMode = "base64"
            rsa.LittleEndian = 0
            Dim base64Sig As String
            base64Sig = rsa.SignStringENC(GetCadenaOriginal(m_xmlDOM.InnerXml), "sha256")
            GenerarSello = base64Sig
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Function

    Public Function GetCadenaOriginal(ByVal xmlCFD As String) As String
        Dim Cadena As String = ""
        Try
            Dim xslt As New XslCompiledTransform
            Dim xmldoc As New XmlDocument
            Dim navigator As XPath.XPathNavigator
            Dim output As New StringWriter
            xmldoc.LoadXml(xmlCFD)
            navigator = xmldoc.CreateNavigator()
            'xslt.Load(Server.MapPath("~/portalcfd/SAT/cadenaoriginal_3_3.xslt"))
            xslt.Load("http://www.sat.gob.mx/sitio_internet/cfd/4/cadenaoriginal_4_0/cadenaoriginal_4_0.xslt")
            xslt.Transform(navigator, Nothing, output)
            Cadena = output.ToString
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try

        Return Cadena

    End Function

    Private Sub btnAceptar_Click(sender As Object, e As EventArgs) Handles btnAceptar.Click
        Response.Redirect("~/portalcfd/cfd.aspx")
    End Sub

    Private Sub AddendasDespuesDelTimbrado()
        Select Case cmbComplementoFactura.SelectedValue
            Case 6
                AgregaAddendaCoopelRopa()
        End Select
    End Sub

#Region "UUids relacionados"
    Private Sub tblRelacionados_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles tblRelacionados.NeedDataSource
        Call tblRelacionados_NeedData("off")
    End Sub

    Private Sub tblRelacionados_NeedData(ByVal state As String)
        tblRelacionados.DataSource = miDataTable1
        If state = "on" Then
            tblRelacionados.DataBind()
        End If
        tblRelacionados.MasterTableView.NoMasterRecordsText = "No se han agregado UUID relacionados"
    End Sub

    Public Sub tblRelacionados_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles tblRelacionados.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                '
                '
                Dim pAux As New DataTable
                pAux = miDataTable1.Clone()
                For Each row As DataRow In miDataTable1.Rows
                    If row("UUID") <> e.CommandArgument Then
                        pAux.ImportRow(row)
                    End If
                Next
                miDataTable1 = New DataTable
                miDataTable1 = pAux.Copy()
                tblRelacionados_NeedData("on")
        End Select
    End Sub

    Protected Sub GetUUIDS_relacionados()
        Dim ObjData As New DataControl
        For i As Integer = 0 To miDataTable1.Rows.Count - 1
            mi_variable = miDataTable1.Rows(i)("UUID").ToString()
            ObjData.RunSQLQuery("EXEC pCFD @cmd=45, @uuid_relacionado='" & mi_variable.ToString & "',@cfdid='" & Session("CFD").ToString & "'")
        Next
        ObjData = Nothing

        Session("uuids_relacionados") = 1

        miDataTable1.Clear()
    End Sub

    Protected Sub btnAddUiid_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUiid.Click

        If Session("Columna1") = 0 Then
            miDataTable1.Dispose()
            miDataTable1.Clear()
            'miDataTable1 = Nothing

            'miDataTable1.Columns.Add("UUID")
            Session("Columna1") = 1
        End If

        'Renglon es la variable que adicionara renglones a mi datatable

        'Renglon("UUID") = txtFolioFiscal.Text
        Renglon("UUID") = cmbUUID.SelectedValue.ToString
        miDataTable1.Rows.Add(Renglon)
        Session("datos") = miDataTable1
        Session("uuid") = miDataTable1.Rows(0)("UUID").ToString()
        'txtFolioFiscal.Text = ""
        cmbUUID.SelectedValue = 0

        'da.Fill(miDataTable1)

        Dim dt As DataTable = TryCast(Session("datos"), DataTable)
        tblRelacionados_NeedData("on")


        Session("uuids_relacionados") = 0
        Session("newcolumna") = 1
    End Sub

#End Region

    Private Sub cmbComplementoFactura_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbComplementoFactura.SelectedIndexChanged
        Select Case cmbComplementoFactura.SelectedValue
            Case 1
                panelComplementoDetallista.Visible = True
                gridResults.MasterTableView.GetColumn("talla").Visible = False
                gridResults.MasterTableView.GetColumn("cajas").Visible = False
                gridResults.MasterTableView.GetColumn("piezasporcaja").Visible = False
            Case 6
                panelAddendaCoppel.Visible = True
                gridResults.MasterTableView.GetColumn("talla").Visible = True
                gridResults.MasterTableView.GetColumn("cajas").Visible = True
                gridResults.MasterTableView.GetColumn("piezasporcaja").Visible = True
            Case 7
                panelComercioExterior.Visible = True
                gridResults.MasterTableView.GetColumn("NoIdentificacion").Visible = True
                gridResults.MasterTableView.GetColumn("FraccArancelaria").Visible = True
                gridResults.MasterTableView.GetColumn("CanAduana").Visible = True
                gridResults.MasterTableView.GetColumn("UnidadAduana").Visible = True
                gridResults.MasterTableView.GetColumn("UnitarioAduana").Visible = True
                gridResults.MasterTableView.GetColumn("ValorDolares").Visible = True
            Case Else
                panelAddendaCoppel.Visible = False
                panelComplementoDetallista.Visible = False
                gridResults.MasterTableView.GetColumn("talla").Visible = False
                gridResults.MasterTableView.GetColumn("cajas").Visible = False
                gridResults.MasterTableView.GetColumn("piezasporcaja").Visible = False
        End Select
    End Sub

#Region "Factura Global"
    Private Sub VerificaFacturaGlobal(ByVal Nodo As XmlNode)
        If panelFacturaGlobal.Visible Then
            Dim InformacionGlobal As XmlElement = CrearNodo("cfdi:InformacionGlobal")
            InformacionGlobal.SetAttribute("Periodicidad", cmbPeriodicidad.SelectedValue)
            InformacionGlobal.SetAttribute("Meses", cmbMes.SelectedValue)
            InformacionGlobal.SetAttribute("Año", txtAnio.Text)
            Nodo.AppendChild(InformacionGlobal)
        End If
    End Sub
#End Region

#Region "Addenda Coopel"
    Private Sub AgregaAddendaCoopelRopa()
        Dim addendaDTO As New AddendaCoppelDTO
        Dim Path As String = Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml"

        addendaDTO.Cfdid = Session("CFD")
        addendaDTO.FechaPromesaEntrega = calFechaPromesaEntrega.SelectedDate
        addendaDTO.FechaOrdenCompra = calFechaOrdenCompraDS.SelectedDate
        addendaDTO.NumReferenciaAdicional = txtNumeroDeReferenciaAdicionalDS.Text
        addendaDTO.FleteCaja = txtCantidadLotes.Text
        addendaDTO.BodegaDestino = txtBodegaDestino.Text
        addendaDTO.BodegaReceptora = txtBodegaReceptora.Text
        addendaDTO.GlnComprador = txtGLNCompradorDS.Text
        addendaDTO.GlnVendedor = txtGLNVendedorDS.Text
        addendaDTO.NumProveedor = txtNumeroEmisorDS.Text
        addendaDTO.Serie = serie
        addendaDTO.Folio = folio
        addendaDTO.OrdenCompra = txtOrdenCompra.Text
        addendaDTO.TipoAddendaid = cmbComplementoFactura.SelectedValue
        addendaDTO.CadenaOriginal = cadOrigComp

        Dim doc As New XmlDocument
        doc.Load(Path)

        Dim addendaDAO As New AddendaCoppelDAO
        Dim addendaCoppel As XmlElement = addendaDAO.GeneraAddendav33(addendaDTO)
        Dim addendaOwn = doc.ImportNode(addendaCoppel, True)

        Dim Addenda As XmlElement
        Addenda = doc.CreateNode(XmlNodeType.Element, "cfdi:Addenda", URI_SAT)
        Addenda.AppendChild(addendaOwn)
        doc.DocumentElement.AppendChild(Addenda)
        doc.Save(Path)
    End Sub
#End Region

    Private Sub CrearNodoComplemento(ByVal Nodo As XmlNode)
        Dim Complemento As XmlElement
        Complemento = CrearNodo("cfdi:Complemento")

        Dim ComercioExterior As XmlElement
        Dim Emisor As XmlElement
        Dim DomicilioEmisor As XmlElement
        Dim Receptor As XmlElement
        Dim DomicilioReceptor As XmlElement
        Dim Mercancias As XmlElement
        Dim Mercancia As XmlElement

        urlcomplemento = 2
        ComercioExterior = CrearNodo("cce20:ComercioExterior")
        ComercioExterior.SetAttribute("Version", "2.0")
        'ComercioExterior.SetAttribute("TipoOperacion", "2")

        If cmbClavePedimento.SelectedValue.ToString <> "-1" Then
            ComercioExterior.SetAttribute("ClaveDePedimento", cmbClavePedimento.SelectedValue)
        End If

        If cmbCertificadoOrigen.SelectedValue.ToString <> "-1" Then
            ComercioExterior.SetAttribute("CertificadoOrigen", cmbCertificadoOrigen.SelectedValue)
        End If

        If txtNumeroCertificadoOrigen.Text.Trim <> "" Then
            ComercioExterior.SetAttribute("NumCertificadoOrigen", txtNumeroCertificadoOrigen.Text)
        End If

        If txtNumeroExportadorConfiable.Text.Trim <> "" Then
            ComercioExterior.SetAttribute("NumeroExportadorConfiable", txtNumeroExportadorConfiable.Text)
        End If

        If cmbClaveIncoterm.SelectedValue.ToString <> "-1" Then
            ComercioExterior.SetAttribute("Incoterm", cmbClaveIncoterm.SelectedValue)
        End If

        'If cmbSubdivision.SelectedValue.ToString <> "-1" Then
        '    ComercioExterior.SetAttribute("Subdivision", cmbSubdivision.SelectedValue)
        'End If

        If txtTipoCambioAddenda.Text > 0 Then
            ComercioExterior.SetAttribute("TipoCambioUSD", Format(CDbl(txtTipoCambioAddenda.Text), "#.#000"))
        End If

        If txtTotalUSD.Text > 0 Then
            ComercioExterior.SetAttribute("TotalUSD", Format(CDbl(txtTotalUSD.Text), "#.#0"))
        End If

        Dim ds As New DataSet
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("select top 1 fac_rfc, curp from tblCliente order by id desc")

        Emisor = CrearNodo("cce20:Emisor")

        If ds.Tables(0).Rows.Count > 0 Then
            If ds.Tables(0).Rows.Item(0)("fac_rfc").ToString.Trim.Length = 13 Then
                Emisor.SetAttribute("Curp", ds.Tables(0).Rows.Item(0)("curp").ToString)
            End If
        End If

        ds = New DataSet
        ds = ObjData.FillDataSet("exec pCFD @cmd=11")

        If ds.Tables(0).Rows.Count > 0 Then
            DomicilioEmisor = CrearNodo("cce20:Domicilio")
            If ds.Tables(0).Rows.Item(0)("fac_calle").ToString <> "" Then
                DomicilioEmisor.SetAttribute("Calle", ds.Tables(0).Rows.Item(0)("fac_calle").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_num_ext").ToString <> "" Then
                DomicilioEmisor.SetAttribute("NumeroExterior", ds.Tables(0).Rows.Item(0)("fac_num_ext").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_num_int").ToString <> "" Then
                DomicilioEmisor.SetAttribute("NumeroInterior", ds.Tables(0).Rows.Item(0)("fac_num_int").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("clave_colonia").ToString <> "" Then
                DomicilioEmisor.SetAttribute("Colonia", ds.Tables(0).Rows.Item(0)("clave_colonia").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("clave_municipio").ToString <> "" Then
                DomicilioEmisor.SetAttribute("Municipio", ds.Tables(0).Rows.Item(0)("clave_municipio").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("claveestado").ToString <> "" Then
                DomicilioEmisor.SetAttribute("Estado", ds.Tables(0).Rows.Item(0)("claveestado").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("clave_pais").ToString <> "" Then
                DomicilioEmisor.SetAttribute("Pais", ds.Tables(0).Rows.Item(0)("clave_pais").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_cp").ToString <> "" Then
                DomicilioEmisor.SetAttribute("CodigoPostal", ds.Tables(0).Rows.Item(0)("fac_cp").ToString)
            End If
            Emisor.AppendChild(DomicilioEmisor)
        End If

        Receptor = CrearNodo("cce20:Receptor")
        Dim numreg = ObjData.RunSQLScalarQueryString("select identidad_tributaria from tblMisClientes where id = " & cmbCliente.SelectedValue.ToString)
        Receptor.SetAttribute("NumRegIdTrib", numreg)
        ds = New DataSet
        ds = ObjData.FillDataSet("EXEC pMisClientes @cmd=2, @clienteid='" & cmbCliente.SelectedValue.ToString & "'")

        If ds.Tables(0).Rows.Count > 0 Then
            DomicilioReceptor = CrearNodo("cce20:Domicilio")
            If ds.Tables(0).Rows.Item(0)("fac_calle").ToString <> "" Then
                DomicilioReceptor.SetAttribute("Calle", ds.Tables(0).Rows.Item(0)("fac_calle").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_num_ext").ToString <> "" Then
                DomicilioReceptor.SetAttribute("NumeroExterior", ds.Tables(0).Rows.Item(0)("fac_num_ext").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_num_int").ToString <> "" Then
                DomicilioReceptor.SetAttribute("NumeroInterior", ds.Tables(0).Rows.Item(0)("fac_num_int").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_colonia").ToString <> "" Then
                DomicilioReceptor.SetAttribute("Colonia", ds.Tables(0).Rows.Item(0)("fac_colonia").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_municipio").ToString <> "" Then
                DomicilioReceptor.SetAttribute("Municipio", ds.Tables(0).Rows.Item(0)("fac_municipio").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("clave_estado").ToString <> "" Then
                DomicilioReceptor.SetAttribute("Estado", ds.Tables(0).Rows.Item(0)("clave_estado").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("clave_pais").ToString <> "" Then
                DomicilioReceptor.SetAttribute("Pais", ds.Tables(0).Rows.Item(0)("clave_pais").ToString)
            End If
            If ds.Tables(0).Rows.Item(0)("fac_cp").ToString <> "" Then
                DomicilioReceptor.SetAttribute("CodigoPostal", ds.Tables(0).Rows.Item(0)("fac_cp").ToString)
            End If

            Receptor.AppendChild(DomicilioReceptor)
        End If

        ComercioExterior.AppendChild(Emisor)
        ComercioExterior.AppendChild(Receptor)

        ds = New DataSet
        ds = ObjData.FillDataSet("exec pCFD @cmd=13, @cfdId='" & Session("CFD").ToString & "'")
        ObjData = Nothing

        If ds.Tables(0).Rows.Count > 0 Then

            Mercancias = CrearNodo("cce20:Mercancias")

            For Each row As DataRow In ds.Tables(0).Rows

                Mercancia = CrearNodo("cce20:Mercancia")

                If row("numero_identificacion").ToString <> "" Then
                    Mercancia.SetAttribute("NoIdentificacion", row("numero_identificacion").ToString)
                End If
                If row("fraccion_arancelaria").ToString <> "" Then
                    Mercancia.SetAttribute("FraccionArancelaria", row("fraccion_arancelaria").ToString)
                End If
                If row("cantidad_aduana").ToString > 0 Then
                    Mercancia.SetAttribute("CantidadAduana", row("cantidad_aduana").ToString)
                End If
                If row("unidad_aduana").ToString <> "" Then
                    Mercancia.SetAttribute("UnidadAduana", row("unidad_aduana").ToString)
                End If
                If row("unitario_aduana").ToString > 0 Then
                    Mercancia.SetAttribute("ValorUnitarioAduana", Format(Math.Round(row("unitario_aduana"), 2), "#0.00"))
                End If
                If row("valor_dolares").ToString > 0 Then
                    Mercancia.SetAttribute("ValorDolares", Format(Math.Round(row("valor_dolares"), 2), "#0.00"))
                End If
                Mercancias.AppendChild(Mercancia)

            Next

            ComercioExterior.AppendChild(Mercancias)

        End If

        Complemento.AppendChild(ComercioExterior)
        IndentarNodo(Complemento, 1)
        urlcomplemento = 0
        Nodo.AppendChild(Complemento)
    End Sub
End Class