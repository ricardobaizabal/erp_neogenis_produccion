Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Telerik.Web.UI
Imports Telerik.Reporting.Processing
Imports System.IO
Imports System.Xml
Imports System.Linq

Public Class editapedido
    Inherits System.Web.UI.Page
    Dim datos As New DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblPedidoError.Visible = False
        If Not IsPostBack Then
            Dim dTable As New DataTable()
            productosList.DataSource = dTable
            pedidodetallelist.DataSource = dTable
            'intEstatusPedido = ObtenerEstatusPedido()
            Call DetallePedido()
            If estatusId.Value = 4 Then 'No mostrar nada si el estatus es cancelado
                Response.Redirect("pedidos.aspx?id=" & Request("id").ToString())
            End If
            If estatusId.Value > 1 Then 'Deshabilitar edición de pedido si su estatus no es abierto
                lblBuscar.Visible = False
                txtSearch.Visible = False
                btnSearch.Visible = False
                btnCancelarBusqueda.Visible = False
                btnColocarPedido.Visible = False
            Else
                lblBuscar.Visible = True
                txtSearch.Visible = True
                btnSearch.Visible = True
                btnCancelarBusqueda.Visible = True
            End If
            ' lblUser.Text = Session("contacto").ToString()
            MuestraPedido()
            '
            ' btnAuth.Attributes.Add("onclick", "javascript:return confirm('Va a autorizar un pedido y enviarlo a almacén. ¿Desea continuar?');")
            ' btnPack.Attributes.Add("onclick", "javascript:return confirm('Va a marcar como empaquetado el producto. ¿Desea continuar?');")
            '
            Select Case estatusId.Value
                Case 1  '   Abierto
                    txtGuia.Enabled = False
                    btnAuth.Enabled = False
                    btnRechazar.Enabled = False
                    btnPack.Enabled = False
                    btnSent.Enabled = False
                    btnReactivar.Enabled = False
                    btnFacturar.Enabled = True
                Case 2, 3  '   En proceso
                    txtGuia.Enabled = False
                    btnColocarPedido.Enabled = False
                    btnPack.Enabled = False
                    btnCancel.Enabled = False
                    btnSent.Enabled = False
                    btnReactivar.Enabled = False
                    btnFacturar.Enabled = True
                Case 5  ' Autorizado
                    txtGuia.Enabled = False
                    btnColocarPedido.Enabled = False
                    btnAuth.Enabled = False
                    btnRechazar.Enabled = False
                    btnCancel.Enabled = False
                    btnSent.Enabled = False
                    btnReactivar.Enabled = False
                    btnFacturar.Enabled = True
                Case 6 'Empaquetado
                    btnColocarPedido.Enabled = False
                    btnAuth.Enabled = False
                    btnRechazar.Enabled = False
                    btnPack.Enabled = False
                    btnCancel.Enabled = False
                    btnSearch.Enabled = False
                    txtGuia.Enabled = False
                    btnSent.Enabled = False
                    btnReactivar.Enabled = False
                    btnFacturar.Enabled = True
                Case 7 'Facturado
                    txtGuia.Enabled = False
                    btnColocarPedido.Enabled = False
                    btnAuth.Enabled = False
                    btnRechazar.Enabled = False
                    btnPack.Enabled = False
                    btnCancel.Enabled = False
                    btnSearch.Enabled = False
                    txtGuia.Enabled = True
                    btnSent.Enabled = True
                    btnReactivar.Enabled = False
                    btnFacturar.Enabled = False
                Case 10 'Rechazado
                    txtGuia.Enabled = False
                    btnAuth.Enabled = False
                    btnRechazar.Enabled = False
                    btnCancel.Enabled = False
                    btnColocarPedido.Enabled = False
                    btnPack.Enabled = False
                    btnSearch.Enabled = False
                    btnSent.Enabled = False
                    btnReactivar.Enabled = True
                    btnFacturar.Enabled = False
                Case Else
                    txtGuia.Enabled = False
                    btnAuth.Enabled = False
                    btnRechazar.Enabled = False
                    btnCancel.Enabled = False
                    btnColocarPedido.Enabled = False
                    btnPack.Enabled = False
                    btnSearch.Enabled = False
                    btnSent.Enabled = False
                    btnReactivar.Enabled = False
                    btnFacturar.Enabled = False
            End Select
            '
            Select Case Session("perfilid")
                Case 3, 5
                    txtGuia.Enabled = False
                    btnAuth.Enabled = False
                    btnPack.Enabled = False
                    btnSent.Enabled = False
                    txtGuia.Enabled = False
                    btnRechazar.Enabled = False
                    btnReactivar.Enabled = False
                    btnFacturar.Enabled = False
            End Select
            '
            If esTimbrado() Then
                btnAsn.Visible = True
                muestraDatosFacturacion()
                pedidodetallelist.MasterTableView.Columns.FindByUniqueName("noCajaColum").Visible = True
                pedidodetallelist.MasterTableView.Columns.FindByUniqueName("viewboxs").Visible = True
            End If
        End If
    End Sub

    Private Sub DetallePedido()
        Dim ObjData As New DataControl()
        Dim dsData As New DataSet()
        Try
            dsData = ObjData.FillDataSet("exec pPedidos @cmd=5, @pedidoid=" & Request("id").ToString)
            If Not IsNothing(dsData) Then
                For Each row As DataRow In dsData.Tables(0).Rows
                    ClienteId.Value = row("clienteid")
                    estatusId.Value = row("estatusid")
                    almacenId.Value = row("almacenid")
                    lblRazonsocial.Text = row("cliente")
                    lblSucursal.Text = row("sucursal")
                    lblAlmacen.Text = row("almacen")
                    lblProyecto.Text = row("proyecto")
                    lblOrdenCompra.Text = row("orden_compra")
                    txtGuia.Text = row("guia")
                    lblidpedido.Text = row("idpedido")
                    lblola.Text = row("ola")
                    txtGuiaCaja.Text = row("guia")
                Next
            End If
        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.ToString()
        End Try
        ObjData = Nothing
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        MuestraProductos()
        panel1.Visible = True
        btnAgregaConceptos.Visible = True
        lblMensaje.Text = ""
    End Sub

    Private Sub MuestraProductos()
        Dim ObjData As New DataControl()
        Dim dsData As New DataSet()
        dsData = ObjData.FillDataSet("exec pPedidos @cmd=22, @txtSearch='" & txtSearch.Text & "', @clienteid='" & ClienteId.Value.ToString & "', @almacenid='" & almacenId.Value.ToString & "'")
        If Not IsNothing(dsData) Then
            productosList.DataSource = dsData
            productosList.DataBind()
        End If
        ObjData = Nothing
    End Sub

    Protected Sub productosList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles productosList.ItemCommand
    End Sub

    Private Sub productosList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productosList.NeedDataSource
        If txtSearch.Text <> "" Then
            Dim ObjData As New DataControl()
            Dim dsData As New DataSet()
            dsData = ObjData.FillDataSet("exec pPedidos @cmd=22, @txtSearch='" & txtSearch.Text & "', @clienteid='" & ClienteId.Value.ToString & "', @almacenid='" & almacenId.Value.ToString & "'")
            productosList.DataSource = dsData
            ObjData = Nothing
        End If

    End Sub

    Private Sub MuestraPedido()
        Dim ObjData As New DataControl()
        datos = ObjData.FillDataSet("exec pPedidosConceptos @cmd=2, @pedidoid=" & Request("id"))
        If Not IsNothing(datos) Then
            pedidodetallelist.DataSource = datos
            pedidodetallelist.DataBind()
        End If
        ObjData = Nothing
    End Sub

    Protected Sub pedidodetallelist_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles pedidodetallelist.ItemCommand
        Dim objdata As New DataControl()
        Select Case e.CommandName
            Case "cmdRecalcular"
                Dim strCodigo, strDescripcion, strunidad As String
                Dim dblCantidad, dlPrecio As Double
                Dim disponibles As Double = 0
                Dim intProductoID As Int64
                Dim existencia As Decimal = 0
                Dim itm As Telerik.Web.UI.GridDataItem
                itm = CType(e.Item, Telerik.Web.UI.GridDataItem)
                If CType(itm("ColCantidad").Controls(1), Telerik.Web.UI.RadNumericTextBox).Value.HasValue Then
                    strCodigo = itm.GetDataKeyValue("codigo")
                    strDescripcion = itm.GetDataKeyValue("descripcion")
                    strunidad = itm.GetDataKeyValue("unidad")
                    dblCantidad = CType(itm("ColCantidad").Controls(1), Telerik.Web.UI.RadNumericTextBox).Value
                    dlPrecio = itm.GetDataKeyValue("precio")
                    existencia = itm.GetDataKeyValue("existencia")
                    intProductoID = itm.GetDataKeyValue("productoid")
                    If existencia >= dblCantidad Then
                        lblMensaje.Text = ""
                        objdata.RunSQLQuery("exec pPedidosConceptos @cmd=4, @conceptoid=" & e.CommandArgument & ", @productoid=" & intProductoID & ", @codigo='" & strCodigo & "', @descripcion = '" & strDescripcion & "', @cantidad=" & dblCantidad & ", @unidad='" & strunidad & "', @precio=" & dlPrecio & ", @importe=0, @iva=0")
                        objdata = Nothing
                    Else
                        lblMensaje.Text = "*La cantidad solicitada es mayor a la disponibilidad para este producto"
                    End If
                End If
            Case "cmdEliminar"
                objdata.RunSQLQuery("exec pPedidosConceptos @cmd=3, @conceptoid=" & e.CommandArgument)
                objdata = Nothing
            Case "ExportToExcel"
                pedidodetallelist.MasterTableView.GetColumn("upc").Display = True
            Case "viewBoxs"
                Dim commandArgs As String() = e.CommandArgument.ToString().Split(New Char() {","c})
                Dim firstArgVal As String = commandArgs(0)
                Dim secondArgVal As String = commandArgs(1)

                If secondArgVal = "" Then
                    MuestraCajas(firstArgVal, "0")
                Else
                    MuestraCajas(firstArgVal, secondArgVal)
                End If
                'MuestraCajas(e.CommandArgument)


        End Select
        Call MuestraPedido()
    End Sub

    Private Sub pedidodetallelist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles pedidodetallelist.ItemDataBound
        Dim itm As Telerik.Web.UI.GridDataItem

        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                'Dim txtCantidad As Telerik.Web.UI.RadNumericTextBox = CType(e.Item.FindControl("txtCantidad"), Telerik.Web.UI.RadNumericTextBox)
                'Dim imgBtnRecalcular As ImageButton = CType(e.Item.FindControl("btnRecalcular"), ImageButton)
                Dim imgBtnEliminar As ImageButton = CType(e.Item.FindControl("btnEliminar"), ImageButton)
                Dim btnViewBox As ImageButton = CType(e.Item.FindControl("btnViewBox"), ImageButton)
                Dim lblnumItems As Label = CType(e.Item.FindControl("lblnumItems"), Label)
                Dim itemsInBox As String
                imgBtnEliminar.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un concepto del pedido. ¿Desea continuar?');")

                itm = CType(e.Item, Telerik.Web.UI.GridDataItem)
                itemsInBox = itm.GetDataKeyValue("itemsInBox")

                If itemsInBox <> "0" Then
                    btnViewBox.ImageUrl = "~/images/icon-box-close.png"
                    lblnumItems.Visible = True
                End If

                If estatusId.Value > 1 Then 'Deshabilitar edición si es el estatus no es abierto
                    'txtCantidad.Enabled = False
                    'imgBtnRecalcular.Visible = False
                    imgBtnEliminar.Visible = False
                End If
                'Case Telerik.Web.UI.GridItemType.Footer
                '    If datos.Tables(0).Rows.Count > 0 Then
                '        e.Item.Cells(7).Text = datos.Tables(0).Compute("sum(cantidad)", "")
                '        e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                '        e.Item.Cells(7).Font.Bold = True
                '        '
                '        e.Item.Cells(8).Text = FormatCurrency(datos.Tables(0).Compute("sum(importe)", ""), 2).ToString
                '        e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
                '        e.Item.Cells(8).Font.Bold = True
                '        '
                'End If
        End Select
    End Sub

    Private Sub btnColocarPedido_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnColocarPedido.Click
        Dim objdata As New DataControl()
        Dim intError As Integer = 0
        Try
            objdata.RunSQLQuery("exec pPedidos @cmd=6, @pedidoid=" & Request("id").ToString())

            Dim email_pedidos As String = System.Configuration.ConfigurationManager.AppSettings("email_pedidos").ToString

            'Dim email As String = ""
            'email = objdata.RunSQLScalarQueryString("select isnull(email,'') as email from tblUsuario where id='" & Session("userid").ToString & "'")

            'Dim ObjComm As New Communications
            'ObjComm.EmailTo = email_pedidos
            'ObjComm.EmailSubject = "Notificación de pedido No." & Request("id").ToString() & "  -  " & DateTime.Now.ToString("dd/MM/yyyy")
            'ObjComm.EmailFrom = email
            'ObjComm.EmailBody = ""
            'ObjComm.EmailSend()
            'ObjComm = Nothing

        Catch ex As Exception
            intError = 1
            lblPedidoError.Visible = True
            lblPedidoError.Text = "Error: " + Left(ex.ToString(), 200)
        End Try
        objdata = Nothing
        If intError = 0 Then
            Response.Redirect("pedidos.aspx?id=" & Request("id"))
        End If
    End Sub

    Private Sub btnRegresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegresar.Click
        Response.Redirect("pedidos.aspx")
    End Sub

    Private Sub pedidodetallelist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles pedidodetallelist.NeedDataSource
        pedidodetallelist_NeedData(canBind:=False)
    End Sub
    Private Sub pedidodetallelist_NeedData(Optional ByVal canBind As Boolean = True)
        WinAsnAutomatico.VisibleOnPageLoad = False
        Dim ObjData As New DataControl()
        Dim dsData As DataSet
        dsData = ObjData.FillDataSet("exec pPedidosConceptos @cmd=2, @pedidoid=" & Request("id").ToString)
        pedidodetallelist.DataSource = dsData
        If canBind Then
            pedidodetallelist.DataBind()
        End If

        ObjData = Nothing
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pPedidos @cmd=3, @pedidoid='" & Request("id").ToString & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/pedidos/pedidos.aspx")
    End Sub

    Private Sub btnAuth_Click(sender As Object, e As System.EventArgs) Handles btnAuth.Click

        Dim limite_credito As Decimal
        Dim saldo As Decimal
        Dim total_pedido As Decimal

        Dim ObjData As New DataControl
        limite_credito = ObjData.RunSQLScalarQueryDecimal("select isnull(limite_credito,0) from tblMisClientes where id='" & ClienteId.Value.ToString & "'")
        saldo = ObjData.RunSQLScalarQueryDecimal("select isnull(sum(a.importe),0)+isnull(sum(a.iva),0)-isnull(sum(a.importe_descuento),0) as saldo from tblCFD_Partidas a inner join tblCFD b on b.id=a.cfdid where b.estatus_cobranzaId=1 and b.estatus<>3 and DATEDIFF(D, b.fecha_promesa, GETDATE())>0 and b.timbrado=1 and dbo.fnTipoDocumentoId(b.serie, b.folio)=1 and b.clienteid='" & ClienteId.Value.ToString & "'")
        total_pedido = ObjData.RunSQLScalarQueryDecimal("select sum(isnull(importe,0)+isnull(iva,0)) from tblPedidosConceptos where pedidoid='" & Request("id").ToString() & "'")

        If limite_credito > total_pedido Then
            If saldo > 0 Then
                RadWindowManager1.RadConfirm("El cliente presenta un saldo pendiente de <b>" & FormatCurrency(saldo, 2) & "</b><br/><br/>¿Desea agregar el pedido al cliente?<br/><br/>", "confirmCallbackFn", 300, 180, Nothing, "Confirmación")
            Else
                ObjData.RunSQLQuery("exec pPedidos @cmd=9, @pedidoid='" & Request("id").ToString & "'")
                ObjData = Nothing
                Response.Redirect("~/portalcfd/pedidos/pedidos.aspx")
            End If
        Else
            RadWindowManager1.RadAlert("El monto total <b>" & FormatCurrency(total_pedido, 2) & "</b> del pedido excede al limite de credito autorizado: <b>" & FormatCurrency(limite_credito, 2) & "</b>", 250, 200, "Alerta", Nothing)
        End If

    End Sub

    Private Sub btnPack_Click(sender As Object, e As System.EventArgs) Handles btnPack.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pPedidos @cmd=10, @pedidoid='" & Request("id").ToString & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/pedidos/pedidos.aspx")
    End Sub

    Private Sub btnSent_Click(sender As Object, e As System.EventArgs) Handles btnSent.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pPedidos @cmd=11, @pedidoid='" & Request("id").ToString & "', @guia='" & txtGuia.Text & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/pedidos/pedidos.aspx")
    End Sub

    Private Sub btnCancelarBusqueda_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelarBusqueda.Click
        Response.Redirect("editapedido.aspx?id=" & Request("id"))
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click

        Dim FilePath = Server.MapPath("~/portalcfd/pedidos/documentos/") & "ng_pedido_" & Request("id").ToString() & ".pdf"
        GuardaPDF(GeneraPDF_Pedido(Request("id")), FilePath)
        Dim FileName As String = Path.GetFileName(FilePath)
        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
        Response.Flush()
        Response.WriteFile(FilePath)
        Response.End()

        'If File.Exists(FilePath) Then
        '    Dim FileName As String = Path.GetFileName(FilePath)
        '    Response.Clear()
        '    Response.ContentType = "application/octet-stream"
        '    Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
        '    Response.Flush()
        '    Response.WriteFile(FilePath)
        '    Response.End()
        'Else
        '    GuardaPDF(GeneraPDF_Pedido(Request("id")), FilePath)

        '    Dim FileName As String = Path.GetFileName(FilePath)
        '    Response.Clear()
        '    Response.ContentType = "application/octet-stream"
        '    Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
        '    Response.Flush()
        '    Response.WriteFile(FilePath)
        '    Response.End()
        'End If
    End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        Facturar40(Request("id").ToString())
    End Sub

    Protected Sub Facturar40(ByVal pedidoid As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pPedidos @cmd=16, @pedidoid='" & pedidoid & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                If Convert.ToDecimal(rs("cfdid")) <= 0 Then
                    GetCFD(pedidoid, rs("clienteid"), rs("sucursalid"), rs("tasaid"), rs("almacenid"), rs("orden_compra").ToString, 4)
                Else
                    GetCFD(pedidoid, rs("clienteid"), rs("sucursalid"), rs("tasaid"), rs("almacenid"), rs("orden_compra").ToString, 4)
                    'Response.Redirect("~/portalcfd/Facturar40.aspx?id=" & rs("cfdid").ToString, False)
                End If
            End If

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.Message.ToString()
        Finally

            conn.Close()
            conn.Dispose()

        End Try
    End Sub

    Protected Sub GetCFD(ByVal pedidoid As Integer, ByVal clienteid As Integer, ByVal sucursalid As Integer, ByVal tasaid As Integer, ByVal almacenid As Integer, ByVal orden_compra As String, Optional ByVal version As Integer = 3)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pPedidos @cmd=17, @clienteid='" & clienteid.ToString & "', @sucursalid='" & sucursalid.ToString & "', @almacenid='" & almacenid.ToString & "', @tasaid='" & tasaid.ToString & "', @pedidoid='" & pedidoid & "', @orden_compra='" & orden_compra.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                If Convert.ToDecimal(rs("cfdid")) > 0 Then
                    Response.Redirect("~/portalcfd/Facturar40.aspx?id=" & rs("cfdid").ToString, False)
                End If
            End If

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.Message.ToString()
        Finally

            conn.Close()
            conn.Dispose()

        End Try
    End Sub

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub

    Private Function GeneraPDF_Pedido(ByVal pedidoid As Long) As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim folio As Long = 0
        Dim almacen As String = ""
        Dim proyecto As String = ""
        Dim vendedor As String = ""

        Dim fecha As String = ""
        Dim cliente As String = ""
        Dim sucursal As String = ""
        Dim orden_compra As String = ""

        Dim ds As DataSet = New DataSet

        Dim cmd As New SqlCommand("EXEC pPedidos @cmd=14, @pedidoid='" & pedidoid.ToString & "'", conn)
        conn.Open()
        Dim rs As SqlDataReader
        rs = cmd.ExecuteReader()

        If rs.Read Then
            folio = rs("id")
            almacen = rs("almacen")
            proyecto = rs("proyecto")
            vendedor = rs("vendedor")
            fecha = rs("fecha")
            cliente = rs("cliente")
            sucursal = rs("sucursal")
            orden_compra = rs("orden_compra")
        End If
        rs.Close()

        conn.Close()
        conn.Dispose()
        conn = Nothing

        Dim reporte As New FormatosPDF.formato_pedidos_neogenis

        reporte.ReportParameters("plantillaId").Value = 5
        reporte.ReportParameters("pedidoId").Value = pedidoid

        reporte.ReportParameters("txtFolio").Value = folio
        reporte.ReportParameters("txtAlmacen").Value = almacen
        reporte.ReportParameters("txtProyecto").Value = proyecto
        reporte.ReportParameters("txtVendedor").Value = vendedor

        reporte.ReportParameters("txtFecha").Value = fecha
        reporte.ReportParameters("txtCliente").Value = cliente
        reporte.ReportParameters("txtSucursal").Value = sucursal
        reporte.ReportParameters("txtOC").Value = orden_compra
        reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))

        Dim totalPzas As String
        Dim objData As New DataControl
        totalPzas = objData.RunSQLScalarQuery("exec pPedidos @cmd=15, @pedidoid='" & pedidoid.ToString & "'")
        objData = Nothing
        '
        reporte.ReportParameters("txtTotalPiezas").Value = totalPzas.ToString
        '
        Return reporte

    End Function

    Private Sub btnAgregaConceptos_Click(sender As Object, e As EventArgs) Handles btnAgregaConceptos.Click
        Dim productoId As Long = 0
        Dim strCodigo, strDescripcion, strunidad As String
        Dim dblCantidad, dlPrecio As Double
        Dim disponibles As Double = 0
        Dim valida As Integer = 0
        Dim mensaje As String = ""
        Dim CantidadP As Integer = 0
        Dim ObjData As New DataControl
        For Each row As GridDataItem In productosList.MasterTableView.Items
            productoId = row.GetDataKeyValue("productoid")
            strCodigo = row.GetDataKeyValue("codigo")
            strDescripcion = row.GetDataKeyValue("descripcion")
            strunidad = row.GetDataKeyValue("unidad")
            dlPrecio = row.GetDataKeyValue("unitario")
            disponibles = row.GetDataKeyValue("disponibles")

            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)

            Try
                dblCantidad = Convert.ToDecimal(txtCantidad.Text)
            Catch ex As Exception
                dblCantidad = 0
            End Try

            If dblCantidad > 0 Then
                If disponibles >= dblCantidad Then
                    ObjData.RunSQLQuery("exec pPedidosConceptos @cmd=1, @pedidoid=" & Request("id") & ", @productoid=" & productoId.ToString & ", @codigo='" & strCodigo & "', @descripcion = '" & strDescripcion & "', @cantidad=" & dblCantidad & ", @unidad='" & strunidad & "', @precio='" & dlPrecio & "'")
                Else
                    valida = valida + 1
                    mensaje = mensaje & "La cantidad proporcionada es mayor a la disponibilidad actual para este producto.<br/>"
                End If
            End If
        Next

        For Each row As GridDataItem In resultslistCSVPedidos.MasterTableView.Items
            productoId = row.GetDataKeyValue("productoid")
            strCodigo = row.GetDataKeyValue("codigo")
            strDescripcion = row.GetDataKeyValue("descripcion")
            strunidad = row.GetDataKeyValue("unidad")
            dlPrecio = row.GetDataKeyValue("unitario")
            disponibles = row.GetDataKeyValue("disponibles")
            CantidadP = row.GetDataKeyValue("Cantidad")

            Try
                dblCantidad = Convert.ToDecimal(CantidadP)
            Catch ex As Exception
                dblCantidad = 0
            End Try

            If dblCantidad > 0 Then
                If disponibles >= dblCantidad Then
                    ObjData.RunSQLQuery("exec pPedidosConceptos @cmd=1, @pedidoid=" & Request("id") & ", @productoid=" & productoId.ToString & ", @codigo='" & strCodigo & "', @descripcion = '" & strDescripcion & "', @cantidad=" & dblCantidad & ", @unidad='" & strunidad & "', @precio='" & dlPrecio & "'")
                Else
                    valida = valida + 1
                    mensaje = mensaje & "La cantidad proporcionada es mayor a la disponibilidad actual para este producto.<br/>"
                End If
            End If
        Next

        ObjData = Nothing
        '

        If valida = 0 Then
            Response.Redirect("editapedido.aspx?id=" & Request("id"))
        Else
            Call MuestraPedido()
            Call MuestraProductos()
            lblMensaje.Text = mensaje
        End If
        '
    End Sub

    Private Sub HiddenButtonOk_Click(sender As Object, e As EventArgs) Handles HiddenButtonOk.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pPedidos @cmd=9, @pedidoid='" & Request("id").ToString & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/pedidos/pedidos.aspx")
    End Sub

    Private Sub btnRechazar_Click(sender As Object, e As EventArgs) Handles btnRechazar.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pPedidos @cmd=24, @pedidoid='" & Request("id").ToString & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/pedidos/pedidos.aspx")
    End Sub

    Private Sub btnReactivar_Click(sender As Object, e As EventArgs) Handles btnReactivar.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pPedidos @cmd=25, @pedidoid='" & Request("id").ToString & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/pedidos/pedidos.aspx")
    End Sub
    Private Function esTimbrado()
        Dim ObjData As New DataControl
        Dim timbrado As String
        timbrado = ObjData.RunSQLScalarQueryString("exec pAsnAutomatico @cmd=1, @pedidoid=" & Request("id"))
        If timbrado = "True" Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Function GetAtrribTimbre(ByVal ElementName As String, ByVal Attibute As String, ByVal Optional noMatch As String = "") As String
        Dim DocXml As XmlDocument = New XmlDocument()
        DocXml.Load("~/portalcfd/cfd_storage/ng_" & getNameXml() & "_timbrado.xml")
        Dim matchValue = ""
        Dim yesMatch = True
        Dim xmlNode = DocXml.GetElementsByTagName(ElementName)
        For Each xmlElement As XmlElement In xmlNode

            If xmlElement.Name = ElementName Then
                matchValue = xmlElement.GetAttribute(Attibute)
                yesMatch = xmlElement.HasAttribute(Attibute)
            End If
        Next

        If yesMatch Then
            Return matchValue
        Else
            Return noMatch
        End If
    End Function
    Private Function getNameXml()
        Dim ObjData As New DataControl
        Dim filename As String
        filename = ObjData.RunSQLScalarQueryString("exec pAsnAutomatico @cmd=3,@pedidoid=" & Request("id"))
        Return filename
    End Function


    Private Sub muestraDatosFacturacion()
        panelDatosFactura.Visible = True
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim Table As New DataTable
        Dim Qry As String = "EXEC pAsnAutomatico @cmd=5 ,@pedidoid=" & Request("id")
        Dim Adapter As New SqlDataAdapter(Qry, conn)
        Adapter.Fill(Table)

        For Each row As DataRow In Table.Rows
            lblSeriefolio.Text = row("serieFolio")
            lblFechafacturacion.Text = Format(row("fechaFactura"), "dd MMM yyyy")
            lblTotalFactura.Text = FormatCurrency(row("total"), 2)
        Next
    End Sub
    Sub txtNoCaja_TextChanged(sender As Object, e As EventArgs)
        Dim txtnoCaja = DirectCast(sender, RadTextBox)
        Dim cell = DirectCast(txtnoCaja.Parent, GridTableCell)
        Dim item As GridDataItem = cell.Item

        Dim index As Integer = item.ItemIndex
        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidodetallelist.MasterTableView.Items
            Dim id As Integer = dataItem.GetDataKeyValue("id")
            If dataItem.ItemIndex = index Then
                actualiza_noCaja(id, txtnoCaja.Text)
                Exit For
            End If
        Next
        pedidodetallelist_NeedData()
    End Sub
    Sub actualiza_noCaja(ByVal partidaid As Integer, ByVal noCaja As String)
        Dim ObjData As New DataControl
        Try
            ObjData.RunSQLScalarQuery("exec pAsnAutomatico @cmd=6, @conceptoid = '" & partidaid & "', @noCaja='" & noCaja & "'")
            'ObjData.RunSQLScalarQuery("exec pAsnAutomatico @cmd=8, @conceptoid = '" & partidaid & "', @noCaja='" & noCaja & "'")
        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.Message.ToString()
        End Try
        ObjData = Nothing
        WinAsnAutomatico.VisibleOnPageLoad = False
        WinCajas.VisibleOnPageLoad = False
    End Sub

    Protected Sub btnViewBox2_Click(sender As Object, e As ImageClickEventArgs)
        Dim txtnoCaja = DirectCast(sender, RadTextBox)
        Dim cell = DirectCast(txtnoCaja.Parent, GridTableCell)
        Dim item As GridDataItem = cell.Item

        Dim index As Integer = item.ItemIndex
        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidodetallelist.MasterTableView.Items
            Dim id As Integer = dataItem.GetDataKeyValue("id")
            If dataItem.ItemIndex = index Then
                actualiza_noCaja(id, txtnoCaja.Text)
                Exit For
            End If
        Next
        pedidodetallelist_NeedData()
    End Sub

    Private Sub MuestraModalAnsAutomatico()
        WinAsnAutomatico.VisibleOnPageLoad = True
        WinCajas.VisibleOnPageLoad = False

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim Qry As String = "EXEC pAsnAutomatico @cmd=4, @pedidoid=" & Request("id")

        Dim Table As New DataTable
        Dim Adapter As New SqlDataAdapter(Qry, conn)
        Adapter.Fill(Table)

        For Each row As DataRow In Table.Rows
            fchEmbarque.SelectedDate = row("fecha_embarque")
            fchEntrega.SelectedDate = row("fecha_entrega")
            txtNoTienda.Text = row("noTienda")
            txtNoCaja.Text = row("noCaja")
        Next
    End Sub
    Private Sub btnAsn_Click(sender As Object, e As EventArgs) Handles btnAsn.Click
        MuestraModalAnsAutomatico()
    End Sub
    Private Sub btnDowload3pl_Click(sender As Object, e As EventArgs) Handles btnDowload3pl.Click
        Dim cmd As Integer = 7
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim Qry As String = "EXEC pAsnAutomatico @cmd=" & cmd & " ,@pedidoid=" & Request("id") &
                                                                               ", @fecha_embarque='" & Format(fchEmbarque.SelectedDate, "yyyy/MM/dd 00:00:00") &
                                                                               "', @fecha_entrega='" & Format(fchEntrega.SelectedDate, "yyyy/MM/dd HH:mm:ss") &
                                                                               "', @noTienda='" & txtNoTienda.Text &
                                                                               "', @noCaja='" & txtNoCaja.Text &
                                                                               "'"
        Dim Table As New DataTable
        Dim Adapter As New SqlDataAdapter(Qry, conn)
        Adapter.Fill(Table)

        Dim txt As String = ""

        For Each row As DataRow In Table.Rows
            txt += $"{row("tupla")}" + vbCrLf
        Next

        Dim utf8WithoutBom As New System.Text.UTF8Encoding(True)

        Dim buffer = Encoding.UTF8.GetBytes(txt)
        Dim ms = New MemoryStream(buffer)

        ms.Position = 0
        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", "attachment; filename=""" & $"Pedido_{Request("id")}.3pl" & """")
        Response.Flush()
        Response.BinaryWrite(ms.ToArray)
        Response.End()
    End Sub

#Region "Cajas"
    Private Sub MuestraCajas(_conceptoid As Integer, _nocaja As String)
        WinCajas.VisibleOnPageLoad = True
        WinAsnAutomatico.VisibleOnPageLoad = False
        lblMensajeCaja.Text = ""
        conceptoid.Value = _conceptoid
        nocajaHF.Value = _nocaja
        CajasList_NeedData()
        'cleanInputCaja()
    End Sub
    'Private Sub MuestraCajas(_conceptoid As Integer)
    '    WinCajas.VisibleOnPageLoad = True
    '    WinAsnAutomatico.VisibleOnPageLoad = False
    '    lblMensajeCaja.Text = ""
    '    conceptoid.Value = _conceptoid
    '    CajasList_NeedData()
    '    cleanInputCaja()
    'End Sub

    Private Sub CajasList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles CajasList.NeedDataSource
        CajasList_NeedData(canBind:=False)
    End Sub
    Private Sub CajasList_NeedData(Optional canBind As Boolean = True)
        Dim _cajaDTO As New PedidoConceptocajas
        _cajaDTO.conceptoid = conceptoid.Value
        _cajaDTO.pedidoid = Request("id").ToString()
        CajasList.DataSource = _cajaDTO.GetAll
        If canBind Then
            CajasList.DataBind()
        End If
    End Sub
    Private Sub btnSaveCaja_Click(sender As Object, e As EventArgs) Handles btnSaveCaja.Click
        If Page.IsValid Then
            Dim _cajaDTO As New PedidoConceptocajas
            Dim result As String
            _cajaDTO.id = cajaid.Value
            _cajaDTO.conceptoid = conceptoid.Value
            _cajaDTO.numCantdad = txtCantidadCaja.Text
            _cajaDTO.nocaja = txtNumCaja.Text
            _cajaDTO.guia = txtGuiaCaja.Text
            _cajaDTO.pedidoid = Request("id").ToString()
            result = _cajaDTO.Save()
            If String.IsNullOrEmpty(result) Then
                cleanInputCaja()
                CajasList_NeedData()
                pedidodetallelist_NeedData()
                Response.Redirect(Request.Url.AbsoluteUri)
            End If
            lblMensajeCaja.Text = result
        End If
    End Sub
    Private Sub cleanInputCaja()
        cajaid.Value = 0
        btnSaveCaja.Text = "Guardar"
        txtCantidadCaja.Text = ""
        txtNumCaja.Text = ""
        txtGuiaCaja.Text = ""
    End Sub

    Private Sub CajaEdit(id As Integer)
        Dim _cajaDTO As New PedidoConceptocajas
        _cajaDTO.Find(id)
        cajaid.Value = id
        txtNumCaja.Text = _cajaDTO.nocaja
        txtCantidadCaja.Text = _cajaDTO.numCantdad
        txtGuiaCaja.Text = _cajaDTO.guia
        btnSaveCaja.Text = "Actualizar"
    End Sub

    Private Sub CajasList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles CajasList.ItemCommand
        Select Case e.CommandName
            Case "cmdEditar"
                CajaEdit(e.CommandArgument)
            Case "cmdEliminar"
                Dim _cajaDTO As New PedidoConceptocajas
                _cajaDTO.Remove(e.CommandArgument)
                CajasList_NeedData()
        End Select
    End Sub
#End Region


#Region "Carga de CVS de Pedidos"


    Private Sub btnCargaPedidosCsv_Click(sender As Object, e As EventArgs) Handles btnCargaPedidosCsv.Click

        If fileUploadPedido.HasFile Then
            If System.IO.Path.GetExtension(fileUploadPedido.FileName).ToUpper = ".CSV" Then
                panelErroresPedidos.Visible = False

                Dim nombreArchivo As String = ""
                nombreArchivo = fileUploadPedido.PostedFile.FileName.ToString

                For i = 1 To 99999
                    If Not File.Exists(Server.MapPath("../pedidos/cargaPedido/") & nombreArchivo) Then
                        fileUploadPedido.SaveAs(Server.MapPath("../pedidos/cargaPedido/") & nombreArchivo)
                        Exit For
                    Else
                        nombreArchivo = i.ToString + "_" + fileUploadPedido.PostedFile.FileName.ToString
                    End If
                Next

                Dim registros As Integer
                Try
                    registros = System.IO.File.ReadAllLines(Server.MapPath("../pedidos/cargaPedido/") & nombreArchivo).Count
                Catch ex As Exception
                    registros = 0
                End Try

                If registros > 0 Then
                    registros = registros - 1
                End If

                Dim registros_error As Integer = 0
                Dim registros_correctos As Integer = 0
                Dim msgerror As String = ""

                Dim cargaid As Long = 0
                Dim ObjData As New DataControl
                cargaid = ObjData.RunSQLScalarQuery("exec pCargaConceptosPedidos @cmd=1, @userid='" & Session("userid").ToString & "', @archivo='" & nombreArchivo.ToString & "', @registros='" & registros.ToString & "'")
                cargaidHidden.Value = cargaid

                If registros = 0 Then
                    ObjData.RunSQLQuery("exec pCargaConceptos @cmd=9, @cargaid='" & cargaid.ToString & "")
                End If

                Dim progress As RadProgressContext = RadProgressContext.Current
                progress.SecondaryTotal = registros
                progress.Speed = "N/A"

                datos = New DataSet
                datos = ObjData.FillDataSet("exec pCargaConceptosPedidos @cmd=2")

                Dim dtConceptosDetalleOCP As New DataTable

                If datos.Tables.Count > 0 Then
                    dtConceptosDetalleOCP = datos.Tables(0)

                End If

                Dim j As Integer = 0
                Dim line As String = ""

                Dim reader As System.IO.StreamReader = New StreamReader(Server.MapPath("../pedidos/cargaPedido/") & nombreArchivo, System.Text.Encoding.Default)
                Do

                    j += 1

                    line = reader.ReadLine

                    If line = Nothing Then
                        Exit Do
                    End If

                    Dim codigo As String = ""
                    Dim cantidad As Integer = 0
                    Dim codigoId As String = ""
                    Dim codigoNoActivo As Integer = 0
                    Dim codigoConcepPrincipal As String = ""
                    Dim cantidadDisponible As Integer = 0

                    If j > 1 Then
                        ' CODIGO
                        Try
                            codigo = line.Split(",")(0)
                        Catch ex As Exception
                            codigo = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            cantidad = line.Split(",")(1)
                        Catch ex As Exception
                            cantidad = 0
                        End Try

                        Try
                            Dim rowCodigo() As DataRow = dtConceptosDetalleOCP.Select("codigo = '" & LTrim(RTrim(codigo)).ToString & "'")
                            For Each row As DataRow In rowCodigo
                                codigo = row(0)
                            Next
                        Catch ex As Exception
                            codigo = 0
                        End Try

                        Try
                            Dim rowCantidad() As DataRow = dtConceptosDetalleOCP.Select("cantidad = '" & LTrim(RTrim(cantidad)).ToString & "'")
                            For Each row As DataRow In rowCantidad
                                cantidad = row(0)
                            Next
                        Catch ex As Exception
                            cantidad = 0
                        End Try

                        If codigo.Length > 0 And cantidad > 0 Then
                            Dim productoid As Long

                            cantidadDisponible = ObjData.RunSQLScalarQuery("exec pCargaConceptosPedidos @cmd=17, @ordenId='" & Request("id").ToString & "',@codigo='" & codigo & "'")

                            codigoNoActivo = ObjData.RunSQLScalarQuery("exec pCargaConceptosPedidos @cmd=14, @codigo='" & codigo & "'")

                            If codigoNoActivo > 0 Then
                                ObjData.RunSQLScalarQuery("exec pCargaConceptosPedidos @cmd=15, @id='" & codigoNoActivo & "'")
                            End If

                            productoid = ObjData.RunSQLScalarQuery("exec pCargaConceptosPedidos @cmd=4, @codigo='" & codigo & "', @cargaid='" & cargaid.ToString & "'")
                            codigoId = ObjData.RunSQLScalarQuery("exec pCargaConceptosPedidos @cmd=13, @codigo='" & codigo & "'")


                            If productoid > 0 Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " ya se encuentra registrado."))
                                p.Add(New SqlParameter("@pedidoId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosPedidos", p)
                                registros_error = registros_error + 1

                            ElseIf codigoId = "0" Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " no está registrado."))
                                p.Add(New SqlParameter("@pedidoId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosPedidos", p)
                                registros_error = registros_error + 1

                            ElseIf cantidad > cantidadDisponible Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " Cantidad de entrada es mayor que el disponible."))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosPedidos", p)
                                registros_error = registros_error + 1

                            Else
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 3))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                'p.Add(New SqlParameter("@pedidoId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosPedidos", p)
                                registros_correctos = registros_correctos + 1
                            End If
                        Else

                            msgerror = ""
                            If codigo.Length = 0 Then
                                msgerror = msgerror & "Código es requerido."
                            End If

                            If cantidad = 0 Then
                                msgerror = msgerror & vbCrLf & "Cantidad es requerida."
                            End If

                            If codigoId.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Codigo no registrado."
                            End If

                            Dim p As New ArrayList
                            p.Add(New SqlParameter("@cmd", 7))
                            p.Add(New SqlParameter("@cargaid", cargaid))
                            p.Add(New SqlParameter("@codigo", codigo))
                            p.Add(New SqlParameter("@cantidad", cantidad))
                            p.Add(New SqlParameter("@error", msgerror))
                            p.Add(New SqlParameter("@pedidoId", Request("id").ToString))
                            ObjData.ExecuteNonQueryWithParams("pCargaConceptosPedidos", p)
                            registros_error = registros_error + 1
                        End If

                    End If

                    progress.SecondaryValue = j
                    progress.SecondaryPercent = Math.Round((j * 100 / registros), 0)
                    progress.CurrentOperationText = "Registro " & j.ToString()

                    If Not Response.IsClientConnected Then
                        Exit Do
                    End If

                    progress.TimeEstimated = (registros - j) * 100
                    System.Threading.Thread.Sleep(100)

                Loop Until line Is Nothing

                reader.Close()

                If registros_correctos > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 8))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_correctos", registros_correctos))
                    ObjData.ExecuteNonQueryWithParams("pCargaConceptosPedidos", p)
                End If

                If registros_error > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 9))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_error", registros_error))
                    ObjData.ExecuteNonQueryWithParams("pCargaConceptosPedidos", p)

                    panelErroresPedidos.Visible = True
                    erroresListPedidos.DataSource = ObjData.FillDataSet("exec pCargaConceptosPedidos @cmd=10, @cargaid='" & cargaid.ToString & "'")
                    erroresListPedidos.DataBind()

                End If

                If registros_correctos = 0 Then
                    ObjData.RunSQLQuery("exec pCargaConceptosPedidos @cmd=11, @cargaid='" & cargaid.ToString & "'")
                End If

                If registros_correctos > 0 And registros_error > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.<br>" & registros_error.ToString & " con error, favor de verificar.", 350, 200, "Alerta", "", "")
                ElseIf registros_correctos > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.", 350, 200, "Alerta", "", "")
                ElseIf registros_error > 0 Then
                    rwAlerta.RadAlert("Se encontraron " & registros_error.ToString & " productos con error, favor de verificar.", 350, 200, "Alerta", "", "")
                End If

                ObjData = Nothing
            Else
                rwAlerta.RadAlert("Formato CSV no válido.", 350, 200, "Alerta", "", "")
            End If
        Else
            rwAlerta.RadAlert("Selecciona un archivo en formato CSV.", 350, 200, "Alerta", "", "")
        End If

        panelCSVPedidos.Visible = True
        btnAgregaConceptos.Visible = True
        Dim ObjData2 As New DataControl
        datos = ObjData2.FillDataSet("exec pCargaConceptosPedidos @cmd=5, @cargaid='" & cargaidHidden.Value & "', @clienteid='" & ClienteId.Value.ToString & "', @almacenid='" & almacenId.Value.ToString & "'")
        ObjData2.RunSQLQuery("exec pCargaConceptosPedidos @cmd=12")
        Dim lineas As Integer = datos.Tables(0).Rows.Count
        If lineas = 0 Then
            lineas = 1
        End If

        resultslistCSVPedidos.PageSize = lineas
        resultslistCSVPedidos.DataSource = datos
        resultslistCSVPedidos.DataBind()

        ObjData2 = Nothing

    End Sub


    Private Sub imgdownloadF_Click(sender As Object, e As ImageClickEventArgs) Handles imgdownloadF.Click
        Dim FilePath = Server.MapPath("~/portalcfd/pedidos/cargaPedido/") & "FORMATOCARGA.csv"

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
    Private Sub erroresListPedido_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles erroresListPedidos.NeedDataSource
        Dim ObjData As New DataControl
        erroresListPedidos.DataSource = ObjData.FillDataSet("exec pCargaConceptosPedidos @cmd=10, @cargaid='" & cargaidHidden.Value.ToString & "'")
        ObjData = Nothing
    End Sub

#End Region

End Class
Public Class PedidoConceptocajas
    Public Property id As Integer
    Public Property conceptoid As Integer
    Public Property nocaja As String
    Public Property numCantdad As Decimal
    Public Property guia As String
    Public Property pedidoid As Integer
    Dim Obj As New DataControl

    Private Function GetParametersSql(ByVal cmd As String) As SqlParameter()
        Dim parameters() As SqlParameter = {
                New SqlParameter("@cmd", SqlDbType.Int) With {.Value = cmd},
                New SqlParameter("@id", SqlDbType.Int) With {.Value = id},
                New SqlParameter("@conceptoid", SqlDbType.Int) With {.Value = conceptoid},
                New SqlParameter("@nocaja", SqlDbType.VarChar) With {.Value = nocaja},
                New SqlParameter("@numCantdad", SqlDbType.Float) With {.Value = numCantdad},
                New SqlParameter("@guiacaja", SqlDbType.VarChar) With {.Value = guia},
                New SqlParameter("@pedidoid", SqlDbType.Int) With {.Value = pedidoid}}

        Return parameters
    End Function

    Public Function Save() As String
        Dim cmd As String
        Dim result As String
        If id = 0 Then
            cmd = 1
        Else
            cmd = 4
        End If
        result = Obj.ExecProcedureTwoWay("pPedidoConceptoCajas", 1, GetParametersSql(cmd))
        Return result
    End Function

    Public Sub Find(ByVal _id As Integer)
        Dim result As DataSet
        result = Obj.FillDataSet("exec pPedidoConceptoCajas @cmd=3, @id = " & _id)
        For Each row As DataRow In result.Tables(0).Rows
            id = _id
            conceptoid = row("conceptoid")
            nocaja = row("nocaja")
            numCantdad = row("numCantdad")
            guia = row("guia")
        Next
    End Sub

    Public Function GetAll() As DataSet
        Dim result As DataSet
        result = Obj.FillDataSet("exec pPedidoConceptoCajas @cmd=2 ,@conceptoid = " & conceptoid)
        Return result
    End Function

    Public Sub Remove(ByVal _id As Integer)
        Obj.FillDataSet("exec pPedidoconceptocajas @cmd=5, @id = " & _id)
    End Sub
End Class