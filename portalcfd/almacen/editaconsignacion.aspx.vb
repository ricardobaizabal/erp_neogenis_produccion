Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports Telerik.Reporting.Processing
Imports System.IO

Public Class editaconsignacion
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        If Not IsPostBack Then
            Call MuestraEtiqueta()
            Call CargaCatalogo()
            Call MuestraDetalle()
            If Session("perfilid") = 3 Or Session("perfilid") = 5 Then
                btnFacturar.Enabled = False
                btnRegresarInventario.Enabled = False
                btnImprimir.Enabled = False
            End If
        End If
    End Sub

    Private Sub MuestraEtiqueta()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try

            Dim cmd As New SqlCommand("EXEC pConsignaciones @cmd=2, @consignacionid='" & Request("id").ToString & "'", conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                lblFolio.Text = rs("id").ToString
                lblFecha.Text = rs("fecha").ToString
                lblAlmacenOrigen.Text = rs("almacen")
                lblCliente.Text = rs("cliente")
                lblVendedor.Text = rs("vendedor")
                lblComentario.Text = Replace(rs("comentario"), vbCrLf, "<br />")
                '
                If rs("estatusid") = 1 Then
                    btnRegresarInventario.Enabled = False
                    btnImprimir.Enabled = False
                    btnFacturar.Enabled = False
                    btnFacturar.ToolTip = "Esta consignación no ha sido procesada"
                    btnRegresarInventario.ToolTip = "Esta consignación no ha sido procesada"
                    productsList.Columns(productsList.Columns.Count - 2).Visible = False
                ElseIf rs("estatusid") = 2 Then
                    'btnProcesar.Enabled = False
                    'btnProcesar.ToolTip = "Esta consignación ya ha sido procesada"
                    'productsList.Columns(productsList.Columns.Count - 1).Visible = False
                    'txtSearchItem.Enabled = False
                    'btnSearch.Enabled = False
                    'btnSearch.ToolTip = "Esta consignación ya ha sido procesada"
                ElseIf rs("estatusid") = 3 Then
                    btnRegresarInventario.Enabled = False
                    btnFacturar.Enabled = False
                    'btnProcesar.Enabled = False
                    'btnProcesar.ToolTip = "Esta consignación ya ha sido cerrada"
                    productsList.Columns(productsList.Columns.Count - 1).Visible = False
                    txtSearchItem.Enabled = False
                    btnSearch.Enabled = False
                    btnSearch.ToolTip = "Esta consignación ya ha sido cerrada"
                End If
                '
            End If
        Catch ex As Exception
            '
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
    End Sub

    Private Sub CargaCatalogo()
        Dim ObjData As New DataControl
        Dim almacenid As Integer = ObjData.RunSQLScalarQuery("EXEC pConsignaciones @cmd=9, @consignacionid='" & Request("id").ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub MuestraDetalle()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pConsignaciones @cmd=6, @consignacionid='" & Request("id").ToString & "'")
        productsList.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        productsList.DataSource = ds
        productsList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect("~/portalcfd/almacen/consignaciones.aspx")
    End Sub

    'Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click
    '    Dim ObjData As New DataControl
    '    ObjData.RunSQLQuery("exec pConsignaciones @cmd=8, @consignacionid='" & Request("id").ToString & "'")
    '    ObjData = Nothing
    '    '
    '    Response.Redirect("~/portalcfd/almacen/consignaciones.aspx")
    'End Sub

    'Private Sub productsList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles productsList.ItemCommand
    '    Select Case e.CommandName
    '        Case "cmdDelete"
    '            Dim ObjData As New DataControl
    '            ObjData.RunSQLQuery("exec pConsignaciones @cmd=4, @consignaciondetalleid='" & e.CommandArgument.ToString & "'")
    '            ObjData = Nothing
    '            Response.Redirect("~/portalcfd/almacen/editaconsignacion.aspx?id=" & Request("id").ToString)
    '    End Select
    'End Sub

    Private Sub productsList_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles productsList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                'Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                'btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un elemento de consignación. ¿Desea continuar?');")
            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    '
                    e.Item.Cells(7).Text = FormatCurrency(ds.Tables(0).Compute("sum(importe)", ""), 2).ToString
                    e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(7).Font.Bold = True
                    '
                    e.Item.Cells(8).Text = ds.Tables(0).Compute("sum(cantidad)", "").ToString
                    e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(8).Font.Bold = True
                    '
                    e.Item.Cells(9).Text = ds.Tables(0).Compute("sum(facturado)", "").ToString
                    e.Item.Cells(9).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(9).Font.Bold = True
                    '
                    e.Item.Cells(10).Text = ds.Tables(0).Compute("sum(regresado)", "").ToString
                    e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(10).Font.Bold = True
                    '
                    e.Item.Cells(11).Text = ds.Tables(0).Compute("sum(disponible)", "").ToString
                    e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(11).Font.Bold = True
                    '
                End If
        End Select
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        btnAgregaConceptos.Visible = True
        Call buscarProducto()
    End Sub

    Private Sub buscarProducto()
        Dim objdata As New DataControl
        gridResults.DataSource = objdata.FillDataSet("exec pConsignaciones @cmd=12, @txtSearch='" & txtSearchItem.Text & "', @consignacionid='" & Request("id").ToString & "'")
        gridResults.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        gridResults.DataBind()
        objdata = Nothing
        txtSearchItem.Text = ""
        txtSearchItem.Focus()
    End Sub

    'Private Sub gridResults_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles gridResults.ItemCommand
    '    Select Case e.CommandName
    '        Case "cmdAdd"
    '            InsertItem(e.CommandArgument, e.Item)
    '    End Select
    'End Sub

    'Protected Sub InsertItem(ByVal inventarioid As Integer, ByVal item As GridItem)
    '    '
    '    ' Instancía objetos del grid
    '    '
    '    Dim lblProductoId As Label = DirectCast(item.FindControl("lblProductoId"), Label)
    '    Dim lblExistencia As Label = DirectCast(item.FindControl("lblExistencia"), Label)
    '    Dim txtCantidad As RadNumericTextBox = DirectCast(item.FindControl("txtCantidad"), RadNumericTextBox)
    '    Dim cantidad As Decimal = 0
    '    Dim existencia As Decimal = 0
    '    If txtCantidad.Text = "" Then
    '        cantidad = 0
    '    Else
    '        cantidad = txtCantidad.Text
    '    End If
    '    If cantidad = 0 Then
    '        lblMensaje.Text = "Proporcione la cantidad que desea."
    '        Return
    '    End If
    '    Try
    '        existencia = Convert.ToDecimal(lblExistencia.Text)
    '    Catch ex As Exception
    '        existencia = 0
    '    End Try
    '    If cantidad <= existencia Then
    '        '
    '        '   Agrega la partida
    '        '
    '        Dim ObjData As New DataControl
    '        ObjData.RunSQLQuery("exec pConsignaciones @cmd=3, @consignacionid='" & Request("id").ToString & "', @productoid='" & lblProductoId.Text.ToString & "', @cantidad='" & txtCantidad.Text & "', @inventarioid='" & inventarioid.ToString & "'")
    '        ObjData = Nothing
    '        '
    '        Response.Redirect("~/portalcfd/almacen/editaconsignacion.aspx?id=" & Request("id").ToString)
    '        '
    '        '
    '    Else
    '        lblMensaje.Text = "La cantidad proporcionada es mayor a la existencia actual para este producto."
    '    End If
    '    '
    '    'Call MuestraDetalle()
    '    'Call buscarProducto()
    '    '
    'End Sub

    Private Sub btnFacturar_Click(sender As Object, e As EventArgs) Handles btnFacturar.Click
        Dim listErrores As New List(Of String)
        Dim message As String = ""
        Dim validaFacturar As Integer = 0

        For Each dataItem As Telerik.Web.UI.GridDataItem In productsList.MasterTableView.Items
            Dim disponible As String = dataItem.GetDataKeyValue("disponible").ToString()
            Dim txtCantidad As Telerik.Web.UI.RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), Telerik.Web.UI.RadNumericTextBox) '' --Cantidad Solicitada
            If Convert.ToDecimal(txtCantidad.Text) > 0 Then
                validaFacturar = validaFacturar + 1
                If Convert.ToDecimal(disponible) < Convert.ToDecimal(txtCantidad.Text) Then
                    listErrores.Add("*La cantidad solicitada (" & txtCantidad.Text & ") es mayor a la disponibilidad (" & disponible & ") para este producto. Para poder FACTURAR proporcione la cantidad correcta.")
                    message = String.Join(Environment.NewLine, listErrores.ToArray())
                    lblMensajeFacturar.Text = message
                    Return
                End If
            End If
        Next

        If validaFacturar > 0 Then

            Dim DataControl As New DataControl
            DataControl.RunSQLQuery("exec pConsignaciones @cmd=20, @consignacionid='" & Request("id") & "'")

            For Each dataItem As Telerik.Web.UI.GridDataItem In productsList.MasterTableView.Items
                Dim txtCantidad As Telerik.Web.UI.RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), Telerik.Web.UI.RadNumericTextBox)

                If Convert.ToDecimal(txtCantidad.Text) > 0 Then
                    Dim productoid As String = dataItem.GetDataKeyValue("productoid").ToString()
                    Dim codigo As String = dataItem.GetDataKeyValue("codigo").ToString()
                    Dim descripcion As String = dataItem.GetDataKeyValue("descripcion").ToString()
                    Dim unidad As String = dataItem.GetDataKeyValue("unidad").ToString()
                    Dim precio As String = dataItem.GetDataKeyValue("precio").ToString()
                    Dim cantidad As String = dataItem.GetDataKeyValue("cantidad").ToString()
                    'Dim lote As String = dataItem.GetDataKeyValue("lote").ToString()
                    'Dim caducidad As String = dataItem.GetDataKeyValue("caducidad").ToString()
                    lblMensajeFacturar.Text = ""
                    DataControl.RunSQLQuery("exec pConsignaciones @cmd=14, @consignacionid='" & Request("id") & "', @productoid='" & productoid.ToString & "', @codigo='" & codigo & "', @descripcion='" & descripcion & "', @cantidad='" & txtCantidad.Text & "', @unidad='" & unidad & "', @precio='" & precio & "'")
                End If

            Next

            Dim cfdid As Long = 0
            cfdid = DataControl.RunSQLScalarQuery("exec pConsignaciones @cmd=15, @consignacionid='" & Request("id") & "'")
            Response.Redirect("~/portalcfd/Factura_Consignacion40.aspx?id=" & cfdid.ToString & "&cid=" & Request("id"), False)
            DataControl = Nothing
        Else
            lblMensajeFacturar.Text = "Proporcione la cantidad que desea facturar"
        End If

    End Sub

    Private Sub productsList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles productsList.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pConsignaciones @cmd=6, @consignacionid='" & Request("id").ToString & "'")
        productsList.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        productsList.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub btnRegresarInventario_Click(sender As Object, e As EventArgs) Handles btnRegresarInventario.Click
        Dim listErrores As New List(Of String)
        Dim message As String = ""
        Dim validaRegresar As Integer = 0

        For Each dataItem As Telerik.Web.UI.GridDataItem In productsList.MasterTableView.Items
            Dim disponible As String = dataItem.GetDataKeyValue("disponible").ToString()
            Dim txtCantidad As Telerik.Web.UI.RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), Telerik.Web.UI.RadNumericTextBox) '' --Cantidad Solicitada
            If Convert.ToDecimal(txtCantidad.Text) > 0 Then
                validaRegresar = validaRegresar + 1
                If Convert.ToDecimal(disponible) < Convert.ToDecimal(txtCantidad.Text) Then
                    listErrores.Add("*La cantidad solicitada (" & txtCantidad.Text & ") es mayor a la disponibilidad (" & disponible & ") para este producto. Para poder FACTURAR proporcione la cantidad correcta.")
                    message = String.Join(Environment.NewLine, listErrores.ToArray())
                    lblMensajeFacturar.Text = message
                    Return
                End If
            End If
        Next

        If validaRegresar > 0 Then
            Dim DataControl As New DataControl
            For Each dataItem As Telerik.Web.UI.GridDataItem In productsList.MasterTableView.Items
                Dim txtCantidad As Telerik.Web.UI.RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), Telerik.Web.UI.RadNumericTextBox)
                If Convert.ToDecimal(txtCantidad.Text) > 0 Then
                    Dim productoid As String = dataItem.GetDataKeyValue("productoid").ToString()

                    lblMensajeFacturar.Text = ""
                    DataControl.RunSQLQuery("exec pRegresaInventarioConsignacionCliente @productoid='" & productoid.ToString & "', @userid='" & Session("userid").ToString & "', @consignacionid='" & Request("id") & "', @cantidad='" & txtCantidad.Text.ToString & "'")
                End If
            Next
            DataControl = Nothing
            '
            '   Actualiza estatus de consignación
            '
            Call ActualizaEstatusConsignacion()
            '
            Response.Redirect("~/portalcfd/almacen/editaconsignacion.aspx?id=" & Request("id").ToString)
            '
        Else
            lblMensajeFacturar.Text = "Proporcione la cantidad que desea regresar"
        End If
    End Sub
    'TODO: Adecuar este còdigo para que regresa consigna a pedidos
    'soy el mandaloriano
    Private Sub btnRegresarPedidos_Click(sender As Object, e As EventArgs) Handles btnRegresarPedidos.Click
        Dim listErrores As New List(Of String)
        Dim message As String = ""
        Dim validaRegresar As Integer = 0
        'valido que no este facturado ni en parcial

        For Each dataItem As Telerik.Web.UI.GridDataItem In productsList.MasterTableView.Items
            Dim disponible As String = dataItem.GetDataKeyValue("disponible").ToString()
            Dim txtCantidad As Telerik.Web.UI.RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), Telerik.Web.UI.RadNumericTextBox) '' --Cantidad Solicitada
            If Convert.ToDecimal(txtCantidad.Text) > 0 Then
                validaRegresar = validaRegresar + 1
                If Convert.ToDecimal(disponible) < Convert.ToDecimal(txtCantidad.Text) Then
                    listErrores.Add("*La cantidad solicitada (" & txtCantidad.Text & ") es mayor a la disponibilidad (" & disponible & ") para este producto. Para poder FACTURAR proporcione la cantidad correcta.")
                    message = String.Join(Environment.NewLine, listErrores.ToArray())
                    lblMensajeFacturar.Text = message
                    Return
                End If
            End If
        Next

        If validaRegresar > 0 Then
            Dim DataControl As New DataControl
            For Each dataItem As Telerik.Web.UI.GridDataItem In productsList.MasterTableView.Items
                Dim txtCantidad As Telerik.Web.UI.RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), Telerik.Web.UI.RadNumericTextBox)
                If Convert.ToDecimal(txtCantidad.Text) > 0 Then
                    Dim productoid As String = dataItem.GetDataKeyValue("productoid").ToString()

                    lblMensajeFacturar.Text = ""
                    DataControl.RunSQLQuery("exec pRegresaInventarioConsignacionCliente @productoid='" & productoid.ToString & "', @userid='" & Session("userid").ToString & "', @consignacionid='" & Request("id") & "', @cantidad='" & txtCantidad.Text.ToString & "'")
                End If
            Next
            DataControl = Nothing
            '
            '   Actualiza estatus de consignación
            '
            Call ActualizaEstatusConsignacion()
            '
            Response.Redirect("~/portalcfd/almacen/editaconsignacion.aspx?id=" & Request("id").ToString)
            '
        Else
            lblMensajeFacturar.Text = "Proporcione la cantidad que desea regresar"
        End If
    End Sub

    Private Sub ActualizaEstatusConsignacion()
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pConsignaciones @cmd=21, @consignacionid='" & Request("id").ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub btnImprimir_Click(sender As Object, e As EventArgs) Handles btnImprimir.Click
        Dim FilePath = Server.MapPath("~/portalcfd/almacen/consignaciones/") & "ng_consignacion_" & Request("id").ToString() & ".pdf"
        'If File.Exists(FilePath) Then
        '    Dim FileName As String = Path.GetFileName(FilePath)
        '    Response.Clear()
        '    Response.ContentType = "application/octet-stream"
        '    Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
        '    Response.Flush()
        '    Response.WriteFile(FilePath)
        '    Response.End()
        'Else
        '    GuardaPDF(GeneraPDF_Consignacion(), FilePath)

        '    Dim FileName As String = Path.GetFileName(FilePath)
        '    Response.Clear()
        '    Response.ContentType = "application/octet-stream"
        '    Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
        '    Response.Flush()
        '    Response.WriteFile(FilePath)
        '    Response.End()
        'End If
        GuardaPDF(GeneraPDF_Consignacion(), FilePath)

        Dim FileName As String = Path.GetFileName(FilePath)
        Response.Clear()
        Response.ContentType = "application/octet-stream"
        Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
        Response.Flush()
        Response.WriteFile(FilePath)
        Response.End()
    End Sub

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub

    Private Function GeneraPDF_Consignacion() As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim consignacionid As Long = 0
        Dim lote As Long = 0
        Dim fecha As String = ""
        Dim cliente As String = ""
        Dim vendedor As String = ""
        Dim sucursal As String = ""
        Dim comentario As String = ""
        Dim subtotal, descuento, iva, total As Decimal

        Dim ds As DataSet = New DataSet

        Dim cmd As New SqlCommand("EXEC pConsignaciones @cmd=2, @consignacionid='" & Request("id").ToString() & "'", conn)
        conn.Open()
        Dim rs As SqlDataReader
        rs = cmd.ExecuteReader()

        If rs.Read Then
            consignacionid = rs("id")
            lote = rs("lote")
            fecha = rs("fecha")
            cliente = rs("cliente")
            sucursal = rs("sucursal")
            vendedor = rs("vendedor")
            comentario = rs("comentario")
            subtotal = rs("subtotal")
            descuento = rs("descuento")
            iva = rs("iva")
            total = rs("total")
        End If
        rs.Close()

        conn.Close()
        conn.Dispose()
        conn = Nothing

        Dim reporte As New Formatos.formato_consignacion_neogenis

        reporte.ReportParameters("plantillaId").Value = 5
        reporte.ReportParameters("consignacionId").Value = consignacionid

        reporte.ReportParameters("txtLote").Value = lote
        reporte.ReportParameters("txtFecha").Value = fecha
        reporte.ReportParameters("txtCliente").Value = cliente
        reporte.ReportParameters("txtSucursal").Value = sucursal
        reporte.ReportParameters("txtVendedor").Value = vendedor
        reporte.ReportParameters("txtComentarios").Value = comentario
        reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
        reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(subtotal, 2)
        reporte.ReportParameters("txtDescuento").Value = FormatCurrency(descuento, 2)
        reporte.ReportParameters("txtIva").Value = FormatCurrency(iva, 2)
        reporte.ReportParameters("txtTotal").Value = FormatCurrency(total, 2)

        Dim totalPzas As String
        Dim objData As New DataControl
        totalPzas = objData.RunSQLScalarQuery("exec pConsignaciones @cmd=11, @consignacionid='" & consignacionid.ToString & "'")
        objData = Nothing
        '
        reporte.ReportParameters("txtTotalPiezas").Value = totalPzas.ToString
        '
        Return reporte

    End Function

    Private Sub gridResults_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles gridResults.NeedDataSource
        Dim objdata As New DataControl
        gridResults.DataSource = objdata.FillDataSet("exec pConsignaciones @cmd=12, @txtSearch='" & txtSearchItem.Text & "', @consignacionid='" & Request("id").ToString & "'")
        gridResults.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        objdata = Nothing
    End Sub

    Private Sub btnAgregaConceptos_Click(sender As Object, e As EventArgs) Handles btnAgregaConceptos.Click
        '
        Dim inventarioId As Long = 0
        Dim productoId As Long = 0
        Dim disponibles As Decimal = 0
        Dim cantidad As Decimal = 0

        Dim ObjData As New DataControl
        For Each row As GridDataItem In gridResults.MasterTableView.Items

            productoId = row.GetDataKeyValue("productoid")
            disponibles = row.GetDataKeyValue("disponibles")

            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
            Dim lblDisponibles As Label = DirectCast(row.FindControl("lblDisponibles"), Label)

            Try
                cantidad = Convert.ToDecimal(txtCantidad.Text)
            Catch ex As Exception
                cantidad = 0
            End Try

            If cantidad > 0 Then
                If disponibles >= cantidad Or lblDisponibles.Text = "N/A" Then
                    ObjData.RunSQLQuery("exec pConsignaciones @cmd=3, @consignacionid='" & Request("id").ToString & "', @productoid='" & productoId.ToString & "', @cantidad='" & txtCantidad.Text & "'")
                End If
            End If
        Next
        ObjData = Nothing
        '
        Response.Redirect("editaconsignacion.aspx?id=" & Request("id"))
        '
    End Sub

    Sub txtCantidad_TextChanged(sender As Object, e As EventArgs)
        Dim total As Decimal = 0
        Dim cantidad As Decimal = 0
        For Each dataItem As Telerik.Web.UI.GridDataItem In productsList.MasterTableView.Items
            Dim txtCantidad As RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), RadNumericTextBox)
            Try
                cantidad = Convert.ToDecimal(txtCantidad.Text.Trim())
            Catch ex As Exception
                cantidad = 0
            End Try
            total += cantidad
        Next
        lblTotalPiezas.Text = "Total Piezas: " & total.ToString()
    End Sub

    Private Sub gridResults_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles gridResults.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim lblDisponibles As Label = DirectCast(e.Item.FindControl("lblDisponibles"), Label)
                Dim lblExistencia As Label = DirectCast(e.Item.FindControl("lblExistencia"), Label)

                If e.Item.DataItem("inventariableBit") = False Then
                    lblDisponibles.Text = "N/A"
                    lblExistencia.Text = "N/A"
                End If

        End Select
    End Sub
End Class