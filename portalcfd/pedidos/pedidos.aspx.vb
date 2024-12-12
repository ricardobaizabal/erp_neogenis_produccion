Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder
Imports System.Globalization
Imports System.Threading
Imports Ionic.Zip
Imports System.Web.Services.Protocols
Imports Library_Class_Neogenis
Imports System.IO
Imports System.Xml
Imports System.Xml.Xsl
Imports System.Security.Cryptography.X509Certificates
Imports Telerik.Reporting.Processing
Imports ThoughtWorks.QRCode.Codec


' PRODUCCION
Public Class pedidos
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
    Private tipoid As Integer = 0
    Private tipoprecioid As Integer
    Private cadOrigComp As String
    Private NumRegIdTribx As String = ""
    Private clienteidx As Integer = 0
    Private facAutConsecutivo As Integer = 0

    Private m_xmlDOM As New XmlDocument
    Const URI_SAT = "http://www.sat.gob.mx/cfd/4"

    Const URI_SAT_COMPLEMENTO = "http://www.sat.gob.mx/detallista"
    Private listErrores As New List(Of String)
    Private Comprobante As XmlNode
    Private docXML As String = ""
    Dim UUID As String = ""
    Dim AplicarRetencion As Boolean = False

    Private qrBackColor As Integer = System.Drawing.Color.FromArgb(255, 255, 255, 255).ToArgb
    Private qrForeColor As Integer = System.Drawing.Color.FromArgb(255, 0, 0, 0).ToArgb
    Private data As Byte()
    Private urlcomplemento As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'chkAll.Attributes.Add("onclick", "checkedAll(" & Me.Form.ClientID.ToString & ");")
        'chkAllcons.Attributes.Add("onclick", "checkedAllcons(" & Me.Form.ClientID.ToString & ");")
        If Not IsPostBack Then
            If Session("perfilid") <> 1 Then
                pedidosList.MasterTableView.GetColumn("ColEtapaAbierto").Visible = False
            End If
            CrearTablaTemp()

            Call CargaClientes()
            Call CargaSucursales()
            Call CargaAlmacenes()
            Call CargaProyectos()
            Call MuestraPedidos()
            Call CargaEstatus()
        End If
    End Sub

    Private Sub CargaClientes()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(clienteid, "exec pPedidos @cmd=12, @userid='" & Session("userid").ToString & "'", 0)
        ObjCat.Catalogo(filtroclienteid, "exec pPedidos @cmd=12, @userid='" & Session("userid").ToString & "'", 0, True)
        ObjCat = Nothing
    End Sub

    Private Sub CargaSucursales()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(sucursalid, "EXEC pListarSucursales @clienteid='" & clienteid.SelectedValue & "'", 0)
        ObjCat = Nothing
    End Sub

    Private Sub CargaAlmacenes()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(almacenid, "select id, nombre from tblAlmacen where id<>4 order by nombre", 5)
        ObjCat = Nothing
    End Sub

    Private Sub CargaProyectos()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(proyectoid, "select id, nombre from tblProyecto order by nombre", 0)
        ObjCat = Nothing
    End Sub

    Private Sub MuestraPedidos()
        Dim ObjData As New DataControl()
        Dim dsData As New DataSet()
        Dim cmd As String = "exec pPedidos @cmd=2, @userid='" & Session("userid").ToString & "', @clienteid='" & filtroclienteid.SelectedValue.ToString & "', @estatusid='" & filtroestatusid.SelectedValue.ToString & "', @txtSearch='" & txtSearch.Text & "'"
        dsData = ObjData.FillDataSet(cmd)
        If dsData.Tables(0).Rows.Count > 0 Then
            pedidosList.DataSource = dsData
            pedidosList.DataBind()
        End If
        ObjData = Nothing
    End Sub

    Private Sub CargaEstatus()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(filtroestatusid, "select id, nombre from tblPedidoEstatus order by nombre", 0, True)
        ObjCat.Catalogo(filtromarcaid, "select id, nombre from tblProyecto order by nombre", 0, True)
        ObjCat.Catalogo(filtroestatusola, "select id, descripcion from tblstatusola order by descripcion", 0, True)
        ObjCat = Nothing
    End Sub

    Protected Sub pedidosList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles pedidosList.ItemCommand
        Dim objdata As New DataControl()
        Select Case e.CommandName
            Case "cmdEditar"
                Response.Redirect("editapedido.aspx?id=" & e.CommandArgument)
            Case "cmdEliminar"
                objdata.RunSQLQuery("exec pPedidos @cmd=4, @pedidoid=" & e.CommandArgument)
                objdata = Nothing
                Call MuestraPedidos()
            Case "cmdFacturar40"
                Call Facturar40(e.CommandArgument)
            Case "cmdEtapaAbierto"
                Call CambiarEstatus(e.CommandArgument)
                Call MuestraPedidos()
            Case "cmdUpGuia"
        End Select
    End Sub

    Private Sub CambiarEstatus(ByVal pedidoid As Integer)
        Dim ObjData As New DataControl()
        ObjData.RunSQLQuery("exec pPedidos @cmd=26, @userid='" & Session("userid").ToString & "', @pedidoid='" & pedidoid.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub pedidosList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles pedidosList.NeedDataSource
        Dim ObjData As New DataControl()
        Dim dsData As New DataSet()
        dsData = ObjData.FillDataSet("exec pPedidos @cmd=2, @userid='" & Session("userid").ToString & "', @clienteid='" & filtroclienteid.SelectedValue.ToString & "', @estatusid='" & filtroestatusid.SelectedValue.ToString & "', @marcaid='" & filtromarcaid.SelectedValue.ToString & "', @txtSearch='" & txtSearch.Text & "'")
        pedidosList.DataSource = dsData
        ObjData = Nothing
    End Sub

    Protected Sub btnCrearPedido_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCrearPedido.Click

        Dim saldo As Decimal
        Dim ObjData As New DataControl
        saldo = ObjData.RunSQLScalarQueryDecimal("exec pPedidos @cmd=39, @clienteid='" & clienteid.SelectedValue.ToString & "'")
        'saldo = ObjData.RunSQLScalarQueryDecimal("select round(isnull(sum(a.importe),0) + isnull(sum(a.iva),0)-isnull(sum(a.importe_descuento),0) ,3) as saldo from tblCFD_Partidas a inner join tblCFD b on b.id=a.cfdid where b.estatus_cobranzaId=1 and b.estatus<>3 and DATEDIFF(D, b.fecha_promesa, GETDATE())>0 and b.timbrado=1 and dbo.fnTipoDocumentoId(b.serie, b.folio)=1 and b.clienteid='" & clienteid.SelectedValue.ToString & "'")
        'saldo = ObjData.RunSQLScalarQueryDecimal("select (isnull(a.importe,0)) as saldo from tblCFD_Partidas a inner join tblCFD b on b.id=a.cfdid where b.estatus_cobranzaId=1 and b.estatus<>3 and DATEDIFF(D, b.fecha_promesa, GETDATE())>0 and b.timbrado=1 and dbo.fnTipoDocumentoId(b.serie, b.folio)=1 and b.clienteid='" & clienteid.SelectedValue.ToString & "'")

        ObjData = Nothing

        If Session("perfilid") <> 1 Then
            If saldo > 0 Then
                'RadWindowManager1.RadAlert("El cliente presenta un saldo pendiente de " & FormatCurrency(saldo, 2) & " <b>Debito a esto, no se podrá generar el pedido.</b>", 250, 200, "Alerta", Nothing)
                RadWindowManager1.RadConfirm("El cliente presenta un saldo pendiente de <b>" & FormatCurrency(saldo, 2) & "</b><br/><br/>¿Desea agregar el pedido al cliente?<br/><br/>", "confirmCallbackFn", 300, 180, Nothing, "Confirmación")
            Else
                Call AgregaPedido()
            End If
        Else
            If saldo > 0 Then
                RadWindowManager1.RadConfirm("¿Desea agregar el pedido al cliente?<br/><b>SALDO : " & FormatCurrency(saldo, 2) & "<b>?<br/><br/>", "confirmCallbackFn", 300, 180, Nothing, "Confirmación")
            Else
                Call AgregaPedido()
            End If
        End If

    End Sub

    Protected Sub pedidosList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles pedidosList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem

                Dim btnEtapaAbierto As ImageButton = CType(e.Item.FindControl("btnEtapaAbierto"), ImageButton)


                Dim itm As Telerik.Web.UI.GridDataItem
                itm = CType(e.Item, Telerik.Web.UI.GridDataItem)

                Dim chkFacAutomaticas As Boolean = itm.GetDataKeyValue("chkFacAutomaticas")
                Dim chkconsolidado As Boolean = itm.GetDataKeyValue("chkconsolidado")
                Dim intEstatus As Integer = itm.GetDataKeyValue("estatusid")

                'Dim itemTimbrado As Boolean = itm.GetDataKeyValue("timbrado")

                If intEstatus > 3 Then 'Deshabilitar Editar y Eliminar si el estatus es diferente a Abierto
                    CType(itm("ColDelete").Controls(1), ImageButton).Visible = False
                Else
                    btnEtapaAbierto.Visible = False
                End If

                If chkFacAutomaticas Then
                    DirectCast(itm.FindControl("chkcfdid"), WebControls.CheckBox).Visible = True
                Else
                    DirectCast(itm.FindControl("chkcfdid"), WebControls.CheckBox).Visible = False
                End If

                If chkFacAutomaticas Then
                    DirectCast(itm.FindControl("chkcons"), WebControls.CheckBox).Visible = True
                Else
                    DirectCast(itm.FindControl("chkcons"), WebControls.CheckBox).Visible = False
                End If

                If intEstatus = 4 Then 'Deshabilitar si el estatus es cancelado
                    btnEtapaAbierto.Visible = False
                    CType(itm("ColEditar").Controls(1), ImageButton).Visible = False
                    'CType(itm("ColFacturar").Controls(1), LinkButton).Visible = False
                End If
                'If intEstatus = 6 Then 'Habilitar el estatus facturar
                '    If Session("perfilid").ToString = "1" And itemTimbrado = False Then
                '        CType(itm("ColFacturar").Controls(1), LinkButton).Visible = True
                '    Else
                '        CType(itm("ColFacturar").Controls(1), LinkButton).Visible = False
                '    End If
                'Else
                '    CType(itm("ColFacturar").Controls(1), LinkButton).Visible = False
                'End If

                If Session("perfilid").ToString <> "1" Then
                    CType(itm("ColFacturar40").Controls(1), LinkButton).Visible = False
                End If

                If intEstatus >= 7 Then 'Deshabilitar el cambio de estatus a abierto
                    btnEtapaAbierto.Visible = False
                    CType(itm("ColFacturar40").Controls(1), LinkButton).Visible = False
                    DirectCast(itm.FindControl("chkcfdid"), WebControls.CheckBox).Visible = False
                    DirectCast(itm.FindControl("chkcons"), WebControls.CheckBox).Visible = False
                Else
                    If intEstatus <= 6 Then
                        CType(itm("ColFacturar40").Controls(1), LinkButton).Visible = True
                    End If
                End If

                'If intEstatus <> 8 Then 'Deshabilitar si el estatus es cancelado
                '    CType(itm("ColFacturar").Controls(1), LinkButton).Visible = False
                'End If
                'If Session("perfilid").ToString = "3" Or Session("perfilid").ToString = "5" Or itemTimbrado = True Then
                '    CType(itm("ColFacturar").Controls(1), LinkButton).Visible = False
                'Else
                '    If intEstatus = 5 And itemTimbrado = False Then
                '        CType(itm("ColFacturar").Controls(1), LinkButton).Visible = True
                '    Else
                '        CType(itm("ColFacturar").Controls(1), LinkButton).Visible = False
                '    End If
                'End If

                If (e.Item.DataItem("condicionesid") = 1) Then
                    e.Item.ForeColor = Drawing.Color.Green
                End If

                If (e.Item.DataItem("estatusid") = 4 Or e.Item.DataItem("estatusid") = 9 Or e.Item.DataItem("estatusid") = 10) Then
                    e.Item.ForeColor = Drawing.Color.Red
                End If

                If (e.Item.DataItem("olaestatusid") <> 3) And (e.Item.DataItem("olaid") <> 0) Then
                    CType(itm("ColEditar").Controls(1), ImageButton).Visible = False
                End If

                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un pedido. ¿Desea continuar?')")
                btnEtapaAbierto.Attributes.Add("onclick", "javascript:return confirm('Va a regresar a estatus abierto un pedido. ¿Desea continuar?')")
        End Select
    End Sub

    Private Sub clienteid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles clienteid.SelectedIndexChanged
        Call CargaSucursales()
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

    Protected Sub Facturar(ByVal pedidoid As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pPedidos @cmd=16, @pedidoid='" & pedidoid & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                If Convert.ToDecimal(rs("cfdid")) <= 0 Then
                    GetCFD(pedidoid, rs("clienteid"), rs("sucursalid"), rs("tasaid"), rs("almacenid"), rs("orden_compra").ToString)
                Else
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

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Dim ObjData As New DataControl()
        Dim dsData As New DataSet()
        dsData = ObjData.FillDataSet("exec pPedidos @cmd=2, @userid='" & Session("userid").ToString & "', @clienteid='" & filtroclienteid.SelectedValue.ToString & "', @estatusid='" & filtroestatusid.SelectedValue.ToString & "', @marcaid='" & filtromarcaid.SelectedValue.ToString & "', @estatusola='" & filtroestatusola.SelectedValue.ToString & "', @txtSearch='" & txtSearch.Text & "'")
        pedidosList.DataSource = dsData
        pedidosList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub HiddenButtonOk_Click(sender As Object, e As EventArgs) Handles HiddenButtonOk.Click
        Call AgregaPedido()
    End Sub

    Private Sub AgregaPedido()
        Dim id As Integer = 0
        Dim ObjData As New DataControl
        Try
            id = ObjData.RunSQLScalarQuery("exec pPedidos @cmd=1, @userid=" & Session("userid") & ", @estatusid=1, @clienteid='" & clienteid.SelectedValue & "', @sucursalid='" & sucursalid.SelectedValue & "', @tasaid=3, @orden_compra='" & txtOrdenCompra.Text & "', @proyectoid='" & proyectoid.SelectedValue & "', @almacenid='" & almacenid.SelectedValue & "'")
            Response.Redirect("editapedido.aspx?id=" & id.ToString())
        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.Message.ToString()
        End Try
        ObjData = Nothing
    End Sub
    Sub actualiza_pagoid(ByVal pedidoid As Integer, ByVal pagoid As String)
        Dim ObjData As New DataControl
        Try
            ObjData.RunSQLScalarQuery("exec pPedidos @cmd=27, @pedidoid = '" & pedidoid & "', @pagoid='" & pagoid & "'")
        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.Message.ToString()
        End Try
        ObjData = Nothing
    End Sub
    Sub actualiza_guia(ByVal pedidoid As Integer, ByVal guia As String)
        Dim ObjData As New DataControl
        Try
            ObjData.RunSQLScalarQuery("exec pPedidos @cmd=28, @pedidoid = '" & pedidoid & "', @guia='" & guia & "'")
        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.Message.ToString()
        End Try
        ObjData = Nothing
    End Sub

    Sub txtGuia_TextChanged(sender As Object, e As EventArgs)
        Dim txt = DirectCast(sender, RadTextBox)
        Dim cell = DirectCast(txt.Parent, GridTableCell)
        Dim item As GridDataItem = cell.Item

        Dim index As Integer = item.ItemIndex
        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidosList.MasterTableView.Items
            Dim id As Integer = dataItem.GetDataKeyValue("id")
            If dataItem.ItemIndex = index Then
                actualiza_guia(id, txt.Text)
                Exit For
            End If

        Next
    End Sub
    Function ValidarCFDI()
        Dim Validar As Boolean = False
        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidosList.MasterTableView.Items
            Dim chkcfdid As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("id"), System.Web.UI.WebControls.CheckBox)

            If chkcfdid.Checked = True Then
                Validar = True
            End If
        Next
        Return Validar

        Dim checked As Boolean = False
        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidosList.MasterTableView.Items
            Dim chkcons As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("id"), System.Web.UI.WebControls.CheckBox)

            If chkcons.Checked = True Then
                checked = True
            End If
        Next
        Return checked


    End Function
    Sub txtpagoid_TextChanged(sender As Object, e As EventArgs)
        Dim txt = DirectCast(sender, RadTextBox)
        Dim cell = DirectCast(txt.Parent, GridTableCell)
        Dim item As GridDataItem = cell.Item
        Dim index As Integer = item.ItemIndex
        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidosList.MasterTableView.Items
            Dim id As Integer = dataItem.GetDataKeyValue("id")
            If dataItem.ItemIndex = index Then
                actualiza_pagoid(id, txt.Text)
                Exit For
            End If

        Next
    End Sub
#Region "Reporte Excel"
    Private selectedItems As New System.Collections.Generic.List(Of Integer)()
    Private selectedPedidos As New System.Collections.Generic.List(Of Integer)()

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Dim cacheGrid As New RadGrid
        'cacheGrid.AutoGenerateColumns = False
        Dim cacheTable As New DataTable
        '
        '
        cacheTable.Columns.Add("fecha", GetType(String))
        cacheTable.Columns.Add("mes", GetType(String))
        cacheTable.Columns.Add("numero", GetType(Integer))
        cacheTable.Columns.Add("cliente", GetType(String))
        cacheTable.Columns.Add("marca", GetType(String))
        cacheTable.Columns.Add("nopedido", GetType(String))
        cacheTable.Columns.Add("modelo", GetType(String))
        cacheTable.Columns.Add("sku", GetType(String))
        cacheTable.Columns.Add("upc", GetType(String))
        cacheTable.Columns.Add("totalpiezas", GetType(String))
        cacheTable.Columns.Add("ubicacion", GetType(String))
        cacheTable.Columns.Add("comprador", GetType(String))
        cacheTable.Columns.Add("clientefinal", GetType(String))
        cacheTable.Columns.Add("clientefinalciudad", GetType(String))
        cacheTable.Columns.Add("guia", GetType(String))
        cacheTable.Columns.Add("comentarios", GetType(String))
        cacheTable.Columns.Add("fullshopify", GetType(String))
        cacheTable.Columns.Add("metodopago", GetType(String))
        cacheTable.Columns.Add("factura", GetType(String))
        cacheTable.Columns.Add("montototal", GetType(String))
        cacheTable.Columns.Add("idpago", GetType(String))
        '
        '
        For Each item As GridDataItem In pedidosList.SelectedItems
            selectedItems.Add(item.ItemIndex)
            selectedPedidos.Add(item.GetDataKeyValue("id"))
        Next
        Dim i As Integer = 1
        For Each pedidoid In selectedPedidos
            Dim Obj As New DataControl
            Dim dt As New DataSet

            dt = Obj.FillDataSet("EXEC pPedidos @cmd=29, @pedidoid=" & pedidoid)
            For Each row As DataRow In dt.Tables(0).Rows

                If row("factura") = "0" Then
                    row("factura") = " "
                End If
                cacheTable.Rows.Add(row("fecha"), row("mes"), i, row("cliente"), row("marca"), row("nopedido"), row("modelo"), row("sku"), row("upc"), row("totalpiezas"), row("ubicacion"), row("comprador"), row("clientefinal"), row("clientefinalciudad"), row("guia"), row("comentarios"), row("fullshopify"), row("metodopago"), row("factura"), row("montototal"), row("idpago"))
                i += 1
            Next
        Next
        '
        '
        ExcelGrid.DataSource = cacheTable
        ExcelGrid.DataBind()
        '

        ExcelGrid.ExportSettings.OpenInNewWindow = True

        ExcelGrid.ExportSettings.FileName = "Pedidos_" & Format(Now(), "ddMMyy HHmmss")

        ExcelGrid.MasterTableView.ExportToExcel()


    End Sub

    Private Sub ExcelGrid_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles ExcelGrid.NeedDataSource
        Dim dt As New DataTable
        ExcelGrid.DataSource = dt
    End Sub

    Sub RadGrid1_ItemDataBound(sender As Object, e As GridItemEventArgs)

    End Sub
#End Region


#Region "Mandar a consigna"

    Private Sub btnConsigna_Click(sender As Object, e As EventArgs) Handles btnConsigna.Click
        Try
            ' Inicializar objetos de control
            Dim ObjData As New DataControl()

            ' Bandera para verificar si se procesaron pedidos
            Dim pedidosProcesados As Boolean = False

            ' Iterar sobre las filas de la tabla temporal
            For Each rows As DataRow In Session("TmpDetalleComplemento").Rows
                If CBool(rows("chkcons")) = True Then
                    ' Crear conexión y comando SQL
                    Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                    Dim cmd As New SqlCommand("exec pConsignaciones @cmd=22, @pedidoid='" & rows("id") & "', @userid='" & Session("userid").ToString & "'", conn)

                    Try
                        conn.Open()

                        ' Ejecutar el comando y leer el resultado
                        Dim rs As SqlDataReader = cmd.ExecuteReader()
                        If rs.Read() Then
                            Dim consolidadoid As Integer = Convert.ToInt32(rs("consignacionid"))
                            Dim pedidoid As Integer = rows("id")
                            ' Guardar el consolidadoid en una variable de sesión o procesarlo según sea necesario
                            Session("consolidadoid") = consolidadoid

                            GetDetallePedido(consolidadoid, pedidoid)
                            EliminaPedido(pedidoid)
                            pedidosProcesados = True
                        End If

                    Catch ex As Exception
                        lblMensaje.Text = "Error: " & ex.Message.ToString()
                    Finally
                        ' Cerrar y liberar recursos
                        conn.Close()
                        conn.Dispose()
                    End Try
                End If
            Next

            ObjData = Nothing

            ' Recargar la página si se procesaron pedidos
            If pedidosProcesados Then
                Response.Redirect(Request.RawUrl, False)
                Context.ApplicationInstance.CompleteRequest()
            End If

        Catch ex As Exception
            ' Manejo global de excepciones
            Response.Write(ex.Message.ToString())
            Response.End()
        End Try
    End Sub

    Private Sub EliminaPedido(pedidoid As Integer)

        ' Establecer la conexión con la base de datos
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        ' Comando para ejecutar el procedimiento almacenado
        Dim cmd As New SqlCommand("exec pPedidos @cmd=49, @pedidoid=@PedidoId", conn)
        cmd.Parameters.AddWithValue("@PedidoId", pedidoid)

        Try
            ' Abrir la conexión
            conn.Open()

            ' Ejecutar el comando y leer el resultado
            Dim reader As SqlDataReader = cmd.ExecuteReader()

            If reader.Read() Then
                ' Asignar el mensaje al Label
                lblMensaje.Text = reader("Mensaje").ToString()
            Else
                lblMensaje.Text = "No se devolvió ningún mensaje."
            End If

            reader.Close()
        Catch ex As Exception
            ' Manejar errores
            lblMensaje.Text = "Error: " & ex.Message.ToString()
        Finally
            ' Cerrar la conexión
            conn.Close()
            conn.Dispose()
        End Try
    End Sub

    Private Sub GetDetallePedido(consignacionId As Integer, pedidoid As Integer)
        ' Establecer la conexión con la base de datos
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        ' Comando para ejecutar el primer procedimiento almacenado pPedidosConceptos
        Dim cmd As New SqlCommand("exec pPedidosConceptos @cmd=2, @pedidoid=@PedidoId", conn)
        cmd.Parameters.AddWithValue("@PedidoId", pedidoid)

        Try
            conn.Open()

            ' Cargar los resultados en un DataTable
            Dim dt As New DataTable()
            dt.Load(cmd.ExecuteReader())

            ' Iterar sobre los registros del DataTable
            For Each row As DataRow In dt.Rows
                ' Extraer los datos necesarios del DataTable
                Dim productoId As Integer = Convert.ToInt32(row("productoId"))
                Dim cantidad As Integer = Convert.ToInt32(row("cantidad"))

                ' Ahora ejecutar el segundo procedimiento pConsignaciones por cada registro obtenido
                Dim consignacionCmd As New SqlCommand("exec pConsignaciones @cmd=3, @consignacionid=@ConsignacionId, @productoid=@ProductoId, @cantidad=@Cantidad", conn)
                consignacionCmd.Parameters.AddWithValue("@ConsignacionId", consignacionId)  ' Usar el consignacionId pasado como parámetro
                consignacionCmd.Parameters.AddWithValue("@ProductoId", productoId)
                consignacionCmd.Parameters.AddWithValue("@Cantidad", cantidad)

                ' Ejecutar el segundo procedimiento
                consignacionCmd.ExecuteNonQuery()
            Next

        Catch ex As Exception
            lblMensaje.Text = "Error: " & ex.Message.ToString()
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub






#End Region

#Region "Facturas Automaticas"

    Private Sub btnFacturasAutomaticas_Click(sender As Object, e As EventArgs) Handles btnFacturasAutomaticas.Click

        Try

            Dim ObjData As New DataControl()
            Dim ObjData2 As New DataControl()

            facAutConsecutivo = ObjData2.RunSQLScalarQuery("exec pPedidos @cmd=42")
            ObjData2 = Nothing

            For Each rows As DataRow In Session("TmpDetalleComplemento").Rows
                If CBool(rows("chkcfdid")) = True Then

                    Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                    Dim cmd As New SqlCommand("exec pPedidos @cmd=16, @pedidoid='" & rows("id") & "'", conn)

                    Try
                        conn.Open()

                        Dim rs As SqlDataReader
                        rs = cmd.ExecuteReader()

                        If rs.Read Then
                            If Convert.ToDecimal(rs("cfdid")) <= 0 Then


                                Session("clienteid") = rs("clienteid")
                                GetCFDAutomatico(rows("id"), rs("clienteid"), rs("sucursalid"), rs("tasaid"), rs("almacenid"), rs("orden_compra").ToString, 4)
                                'ObjData.RunSQLQuery("EXEC pPedidos @cmd=36, @clienteid='" & Session("clienteid") & "', @cfdid='" & Session("CFD") & "', @pedidoid='" & rows("id") & "'")
                            Else
                                '    Response.Redirect("~/portalcfd/Facturar40.aspx?id=" & rs("cfdid").ToString, False)
                                ' para guardar los errores , crear un nomero de lote para guardar y despues buscar por algun error.
                                lblErrores.Text = "Intento de factura previemente echo, eliminar y volver a Intentar"
                                lblErrores.ForeColor = Drawing.Color.Red
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


                End If
            Next
            ObjData = Nothing

            'Call GetCFD()

            'Call AgregaCFDI()

            'If Session("CFD") > 0 Then
            '    Response.Redirect("~/portalcfd/Facturar40.aspx?id=" & Session("CFD").ToString, False)
            'End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Response.End()

        End Try

    End Sub

    'Protected Sub GetCFD(ByVal pedidoid As Integer)
    '    ' Crea el Id de CFD 
    '    Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
    '    Dim cmd As New SqlCommand("EXEC pPedidos @cmd=38, @clienteid='" & filtroclienteid.SelectedValue & "', @sucursalid='" & sucursalid.SelectedValue & "', @tasaid=3, @proyectoid='" & proyectoid.SelectedValue & "', @almacenid='" & almacenid.SelectedValue & "'", conn)

    '    Try
    '        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
    '        Dim cmd As New SqlCommand("exec pPedidos @cmd=16, @pedidoid='" & pedidoid & "'", conn)
    '        'conn.Open()
    '        Dim rs As SqlDataReader
    '        rs = cmd.ExecuteReader()



    '        If rs.Read Then
    '            Session("CFD") = rs("cfdid") 'activa la sesion de cfdid para los pedidos
    '        End If

    '        conn.Close()
    '        conn.Dispose()

    '    Catch ex As Exception
    '        Response.Write(ex.Message.ToString)
    '        Response.End()
    '    Finally

    '        conn.Close()
    '        conn.Dispose()

    '    End Try

    'End Sub

    Protected Sub GetCFDAutomatico(ByVal pedidoidx As Integer, ByVal clienteidx As Integer, ByVal sucursalidx As Integer, ByVal tasaidx As Integer, ByVal almacenidx As Integer, ByVal orden_comprax As String, Optional ByVal version As Integer = 3)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pPedidos @cmd=40, @clienteid='" & clienteidx.ToString & "', @sucursalid='" & sucursalidx.ToString & "', @almacenid='" & almacenidx.ToString & "', @tasaid='" & tasaidx.ToString & "', @pedidoid='" & pedidoidx & "', @orden_compra='" & orden_comprax.ToString & "'", conn)


        Try
            Dim Timbrado As Boolean = False
            Dim MensageError As String = " "
            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                If Convert.ToDecimal(rs("cfdid")) > 0 Then
                    Dim ObjData As New DataControl

                    Session("CFD") = rs("cfdid")
                    'Response.Redirect("~/portalcfd/Facturar40.aspx?id=" & rs("cfdid").ToString, False)

                    '  Protected Sub btnCreateInvoice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreateInvoice.Click
                    If Page.IsValid Then
                        '
                        '   Agregado para remisionar
                        '
                        '   Rutina de generación de XML CFDI Versión 4.0 para Pedidos en Automatico
                        '
                        Call CargaTotales()
                        '
                        '   Guadar Metodo de Pago
                        '
                        'Call GuadarMetodoPago()
                        '
                        m_xmlDOM = CrearDOM()
                        '
                        '   Verifica que tipo de comprobante se va a emitir
                        '
                        Dim TipoDeComprobante As String = Nothing
                        'Select Case tipoid
                        '    Case 1, 3, 4, 5, 6
                        '   Ingreso
                        TipoDeComprobante = "I"
                        '    Case 2, 8
                        '        '   Egreso (Nota de Crédito)
                        '        TipoDeComprobante = "E"
                        'End Select
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
                        '   Agrega los datos del emisor
                        '
                        Call ConfiguraEmisor()
                        '
                        '   Asigna los datos del receptor
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
                        'Dim replaceBit As Boolean = True
                        'If cmbComplementoFactura.SelectedValue = 1 Then
                        '    replaceBit = False
                        '    CrearNodoComplementoDetallista(Comprobante)
                        'End If
                        '
                        '   Agrega complemento comercio exterior
                        '
                        'If cmbComplementoFactura.SelectedValue = 7 Then
                        '    CrearNodoComplemento(Comprobante)
                        '    IndentarNodo(Comprobante, 0)
                        'End If
                        '
                        '   Sellar Comprobante
                        '
                        SellarCFD(Comprobante)
                        'If replaceBit = True Then
                        '    m_xmlDOM.InnerXml = (Replace(m_xmlDOM.InnerXml, "schemaLocation", "xsi:schemaLocation", , , CompareMethod.Text))
                        'End If
                        m_xmlDOM.InnerXml = (Replace(m_xmlDOM.InnerXml, "schemaLocation", "xsi:schemaLocation", , , CompareMethod.Text))
                        m_xmlDOM.Save(Server.MapPath("~/portalCFD/cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & ".xml")
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
                                            MensageError = "Código: " & ex.Detail.FirstChild.SelectSingleNode("codigo").InnerText & vbCrLf & vbCrLf & "Error: " & ex.Message.ToString & vbCrLf & vbCrLf & "Detalle: " & ex.Detail.FirstChild.SelectSingleNode("error").InnerText
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
                                            Catch soex As SoapException
                                                Call cfdnotimbrado()
                                                Timbrado = False
                                                MensageError = "Código: " & soex.Detail.FirstChild.SelectSingleNode("codigo").InnerText & vbCrLf & "Error: " & soex.Message.ToString & vbCrLf & "Detalle: " & soex.Detail.FirstChild.SelectSingleNode("error").InnerText
                                            End Try
                                        End If
                                    End Try
                                End Try


                                cadOrigComp = CadenaOriginalComplemento()

                                If Timbrado = True Then
                                    '
                                    '   Marca el cfd como timbrado
                                    '
                                    Call cfdtimbrado()
                                    '
                                    '   Genera Código Bidimensional
                                    '
                                    Call generacbb()
                                    '
                                    '   Descarga Inventario si hay folio y fué timbrado el cfdi
                                    '
                                    Call DescargaInventario(Session("CFD"))

                                    'Call VerificaTimbrado(Session("CFD"))
                                    '
                                    '   Actualiza estatus de pedido
                                    '
                                    Call ActualizaEstatusPedido(Session("CFD"))
                                    '
                                    '   Genera PDF
                                    '
                                    If Not File.Exists(Server.MapPath("~/portalcfd/pdf") & "\ng_" & serie.ToString & folio.ToString & ".pdf") Then
                                        GuardaPDF(GeneraPDF(Session("CFD")), Server.MapPath("~/portalcfd/pdf") & "\ng_" & serie.ToString & folio.ToString & ".pdf")
                                    End If
                                    '
                                    '   Agrega addendas
                                    '   
                                    'Call AddendasDespuesDelTimbrado()
                                    '
                                    '   Verifica timbrado y rescate de folio
                                    '
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

                    If Timbrado = True Then
                        'Response.Redirect("~/portalcfd/cfd.aspx")
                        'Dim ObjData As New DataControl
                        Try
                            MensageError = "Correcto"
                            ObjData.RunSQLScalarQuery("exec pPedidos @cmd=41,  @correcto=1, @error=0, @cfdid = '" & Session("CFD") & "', @idCarga='" & facAutConsecutivo & "', @MensajeError='" & MensageError.ToString & "'")
                        Catch ex As Exception
                            lblMensaje.Text = "Error: " + ex.Message.ToString()
                        End Try
                        ObjData = Nothing
                    Else
                        lblErrores.Text = MensageError.ToString
                        ' Agreagr tabla para guardar Informacion de Facturas Erroneas.
                        'Dim ObjData As New DataControl
                        Try
                            ' MensageError = "Con Err"
                            ObjData.RunSQLScalarQuery("exec pPedidos @cmd=41,  @correcto=0, @error=1, @cfdid = '" & Session("CFD") & "', @idCarga='" & facAutConsecutivo & "', @MensajeError='" & MensageError.ToString & "'")
                        Catch ex As Exception
                            lblMensaje.Text = "Error: " + ex.Message.ToString()
                        End Try
                        ObjData = Nothing

                    End If
                    Session("CFD") = 0
                End If
            End If

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            lblMensaje.Text = "Error: " + ex.Message.ToString()
        Finally

            conn.Close()
            conn.Dispose()
            Call MuestraPedidos()

        End Try
    End Sub

    Private Sub CargaTotales()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pCFD @cmd=16, @cfdid='" & Session("CFD").ToString & "'", conn)
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                tieneIva16 = rs("tieneIva16")
                tieneIvaTasaCero = rs("tieneIvaTasaCero")
                subtotal = rs("importe")
                iva = rs("iva")
                tipoid = rs("tipoid")
                descuento = rs("totaldescuento")
                totaldescuento = rs("totaldescuento")
                total = rs("total")

                'lblSubTotalValue.Text = FormatCurrency(rs("importe_pesos"), 2).ToString
                'lblImporteTasaCeroValue.Text = FormatCurrency(rs("importetasacero"), 2).ToString
                'lblDescuentoValue.Text = FormatCurrency(rs("totaldescuento"), 2).ToString
                'lblIVAValue.Text = FormatCurrency(rs("iva_pesos"), 2).ToString
                'lblTotalValue.Text = FormatCurrency(rs("total_pesos"), 2).ToString

                'If System.Configuration.ConfigurationManager.AppSettings("retencion4") = 1 And tipoid = 5 Then
                '    panelRetencion.Visible = True
                '    lblRetValue.Text = FormatCurrency(rs("importe") * 0.04, 2).ToString
                '    lblTotalValue.Text = FormatCurrency(rs("total") - (rs("importe") * 0.04) - rs("totaldescuento"), 2).ToString
                'End If

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Response.End()
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
    End Sub

    'Private Sub GuadarMetodoPago()
    '    Dim Objdata As New DataControl
    '    Dim cmdFacturaGlobal As String = ""
    '    If panelFacturaGlobal.Visible Then
    '        cmdFacturaGlobal = "',@periodicidad_id='" & cmbPeriodicidad.SelectedValue &
    '                           "',@fac_global_mes='" & cmbMes.SelectedValue &
    '                           "',@fac_global_anio='" & txtAnio.Text
    '    End If
    '    Objdata.RunSQLQuery("exec pCFD @cmd=25, @metodopagoid='" & cmbMetodoPago.SelectedValue & "', @usocfdi='" & cmbUsoCFD.SelectedValue & "', @tipodocumentoid='" & cmbTipoDocumento.SelectedValue & "', @cfdid='" & Session("CFD").ToString & cmdFacturaGlobal & "'")
    '    Objdata = Nothing
    'End Sub

    Private Function CrearDOM() As XmlDocument
        Dim oDOM As New XmlDocument
        Dim Nodo As XmlNode
        Nodo = oDOM.CreateProcessingInstruction("xml", "version=""1.0"" encoding=""utf-8""")
        oDOM.AppendChild(Nodo)
        Nodo = Nothing
        CrearDOM = oDOM
    End Function

    Private Sub AsignaSerieFolio()
        '
        '   Obtiene serie y folio
        '
        Dim aprobacion As String = ""
        Dim annioaprobacion As String = ""

        Dim SQLUpdate As String = ""

        SQLUpdate = "exec pCFD @cmd=53, @cfdid='" & Session("CFD").ToString & "'"

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
                tipoid = rs("tipoid")
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

    Private Function CrearNodoComprobante(ByVal TipoDeComprobante As String) As XmlNode
        Dim Comprobante As XmlNode
        Comprobante = m_xmlDOM.CreateElement("cfdi:Comprobante", URI_SAT)
        CrearAtributosComprobante(Comprobante, TipoDeComprobante)
        CrearNodoComprobante = Comprobante
    End Function

    Private Sub CrearAtributosComprobante(ByVal Nodo As XmlElement, ByVal TipoDeComprobante As String)

        Dim FormaPagoX As String = "99"
        Dim CondicionesX As String = "Contado"

        Nodo.SetAttribute("xmlns:cfdi", URI_SAT)
        Nodo.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance")
        Nodo.SetAttribute("xsi:schemaLocation", "http://www.sat.gob.mx/cfd/4 http://www.sat.gob.mx/sitio_internet/cfd/4/cfdv40.xsd")

        Nodo.SetAttribute("Version", "4.0")

        If serie.ToString.Length > 0 Then
            Nodo.SetAttribute("Serie", serie)
        End If

        Nodo.SetAttribute("Folio", folio)
        Nodo.SetAttribute("Fecha", Format(Now(), "yyyy-MM-ddThh:mm:ss"))
        Nodo.SetAttribute("Sello", "")
        Nodo.SetAttribute("FormaPago", FormaPagoX) '01,02,03,04,05,06,07...
        Nodo.SetAttribute("NoCertificado", "")
        Nodo.SetAttribute("Certificado", "")
        Nodo.SetAttribute("CondicionesDePago", CondicionesX) 'CREDITO, CONTADO, CREDITO A 3 MESES ETC
        Nodo.SetAttribute("SubTotal", Math.Round(subtotal, 2))

        If descuento > 0 Then
            Nodo.SetAttribute("Descuento", Format(CDbl(descuento), "0.#0"))
            'Nodo.SetAttribute("Descuento", Math.Round(descuento, 2))
        End If

        Nodo.SetAttribute("Moneda", "MXN")

        Dim MetodoPagoX As String = "PPD"
        Dim ExportacionX As String = "01"

        Nodo.SetAttribute("Total", Math.Round(total, 2))
        Nodo.SetAttribute("TipoDeComprobante", TipoDeComprobante)
        Nodo.SetAttribute("MetodoPago", MetodoPagoX) 'PUE, PID, PPD
        Nodo.SetAttribute("LugarExpedicion", CargaLugarExpedicion())
        Nodo.SetAttribute("Exportacion", ExportacionX) '01, 02, 03
    End Sub
    Private Function CargaLugarExpedicion() As String
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCliente @cmd=3, @clienteid=1", conn)
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
    Private Sub IndentarNodo(ByVal Nodo As XmlNode, ByVal Nivel As Long)
        Nodo.AppendChild(m_xmlDOM.CreateTextNode(vbNewLine & New String(ControlChars.Tab, Nivel)))
    End Sub

#Region "Factura Global"
    Private Sub VerificaFacturaGlobal(ByVal Nodo As XmlNode)
        Dim Periocidadx As String = "04"
        Dim PerioMesx As String = Format(DateTime.Now.Month, "0#")
        Dim PerioAnio As String = Format(DateTime.Now.Year, "000#")

        Dim InformacionGlobal As XmlElement = CrearNodo("cfdi:InformacionGlobal")
        InformacionGlobal.SetAttribute("Periodicidad", Periocidadx)
        InformacionGlobal.SetAttribute("Meses", PerioMesx)
        InformacionGlobal.SetAttribute("Año", PerioAnio)
        Nodo.AppendChild(InformacionGlobal)
    End Sub
#End Region

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

    Private Function CrearNodo(ByVal Nombre As String) As XmlNode
        'CrearNodo = m_xmlDOM.CreateNode(XmlNodeType.Element, Nombre, URI_SAT)
        If urlcomplemento = 0 Then
            CrearNodo = m_xmlDOM.CreateNode(XmlNodeType.Element, Nombre, URI_SAT)
        ElseIf urlcomplemento = 1 Then
            CrearNodo = m_xmlDOM.CreateNode(XmlNodeType.Element, Nombre, "http://www.sat.gob.mx/detallista")
        ElseIf urlcomplemento = 2 Then
            CrearNodo = m_xmlDOM.CreateNode(XmlNodeType.Element, Nombre, "http://www.sat.gob.mx/ComercioExterior11")
        End If
    End Function
    Private Sub ConfiguraReceptor()
        '
        '   Obtiene datos del receptor
        '
        Dim connR As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdR As New SqlCommand("exec pCFD @cmd=12, @clienteId='" & Session("clienteid").ToString & "'", connR)
        Try

            Dim UsoCFDI As String = "S01"
            connR.Open()

            Dim rs As SqlDataReader
            rs = cmdR.ExecuteReader()

            If rs.Read Then
                CrearNodoReceptor(Comprobante, rs("denominacion_razon_social"), rs("fac_rfc"), UsoCFDI, rs("fac_cp"), rs("clave_pais"), rs("identidad_tributaria"), rs("regimenfiscalid"))
                NumRegIdTribx = rs("identidad_tributaria")
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

    Private Sub CrearNodoReceptor(ByVal Nodo As XmlNode, ByVal nombre As String, ByVal rfc As String, ByVal UsoCFDI As String, ByVal DomicilioFiscalReceptor As String, ByVal ResidenciaFiscal As String, ByVal NumRegIdTrib As String, ByVal RegimenFiscalReceptor As String)
        Dim Receptor As XmlElement
        Receptor = CrearNodo("cfdi:Receptor")
        Receptor.SetAttribute("Rfc", rfc)
        Receptor.SetAttribute("Nombre", nombre.ToUpper)
        Receptor.SetAttribute("RegimenFiscalReceptor", RegimenFiscalReceptor)
        If DomicilioFiscalReceptor.Length > 0 Then
            Receptor.SetAttribute("DomicilioFiscalReceptor", DomicilioFiscalReceptor)
        End If
        If ResidenciaFiscal.Length > 0 Then
            Receptor.SetAttribute("ResidenciaFiscal", ResidenciaFiscal)
        End If
        If NumRegIdTrib.Length > 0 Then
            Receptor.SetAttribute("NumRegIdTrib", NumRegIdTrib)
        End If
        Receptor.SetAttribute("UsoCFDI", UsoCFDI)
        Nodo.AppendChild(Receptor)
    End Sub
    Private Sub CrearNodoConceptos(ByVal Nodo As XmlNode)
        Dim ObjData As New DataControl
        '
        '   Revisa y elimina registros previos de impuestos
        '
        ObjData.RunSQLQuery("exec pCFDTraslados @cmd=6,@cfdid=" & Session("CFD"))
        ObjData.RunSQLQuery("exec pCFDRetenciones @cmd=6,@cfdid=" & Session("CFD"))
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
                ' If iva > 0 Then
                Traslados = CrearNodo("cfdi:Traslados")
                Traslado = CrearNodo("cfdi:Traslado")

                If rs("descuento") > 0 Then
                    'Base = Math.Round(rs("importe") - rs("descuento"), 6)
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
        Else
            Impuestos.SetAttribute("TotalImpuestosTrasladados", Format(iva, "#0.00"))
        End If

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
            Select Case tipoid
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
                        Retencion.SetAttribute("Importe", Math.Round(CDbl(ImporteISR), 2))
                        Retenciones.AppendChild(Retencion)
                        '
                        '  IVA
                        '
                        Retencion = CrearNodo("cfdi:Retencion")
                        Retencion.SetAttribute("Impuesto", "002")
                        ImporteIVA = Math.Round((subtotal * 0.106667), 2)
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
                Traslado.SetAttribute("Importe", Format(iva, "#0.00"))
            Else
                Traslado.SetAttribute("Importe", "0.00")
            End If
            Traslado.SetAttribute("Base", Format(CDbl(subtotal - descuento), "0.#0"))
            Traslados.AppendChild(Traslado)

        End If

        IndentarNodo(Traslados, 2)
        Impuestos.AppendChild(Traslados)
        IndentarNodo(Impuestos, 1)
        Nodo.AppendChild(Impuestos)

    End Sub
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

    Function ReadFile(ByVal strArchivo As String) As Byte()
        Dim f As New FileStream(strArchivo, FileMode.Open, FileAccess.Read)
        Dim size As Integer = CInt(f.Length)
        Dim data As Byte() = New Byte(size - 1) {}
        size = f.Read(data, 0, size)
        f.Close()
        Return data
    End Function
    Public Function FormatearSerieCert(ByVal Serie As String) As String
        Dim Resultado As String = ""
        Dim I As Integer
        For I = 2 To Len(Serie) Step 2
            Resultado = Resultado & Mid(Serie, I, 1)
        Next
        FormatearSerieCert = Resultado
    End Function
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

    Private Sub cfdnotimbrado()
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pCFD @cmd=23, @cfdid='" & Session("CFD").ToString & "'")
        ObjData.RunSQLQuery("exec pCFD @cmd=31, @cfdid='" & Session("CFD").ToString & "'")
        ObjData = Nothing
    End Sub
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
    End Function

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
    Private Sub cfdtimbrado()
        Dim uuid As String
        uuid = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        Dim Objdata As New DataControl
        Objdata.RunSQLQuery("exec pCFD @cmd=24, @uuid='" & uuid.ToString & "', @cfdid='" & Session("CFD").ToString & "', @subtotal='" & subtotal.ToString & "', @descuento='" & descuento.ToString & "', @iva='" & iva.ToString & "', @total='" & total.ToString & "'")
        Objdata = Nothing
    End Sub
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
        rfcE = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Emisor")
        rfcR = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
        total = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Total", "cfdi:Comprobante")
        UUID = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        sello = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloCFD", "tfd:TimbreFiscalDigital")
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
    Private Sub DescargaInventario(ByVal cfdid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pControlInventario @cmd=5, @cfdid='" & cfdid.ToString & "', @userid='" & Session("userid").ToString & "'")
        ObjData = Nothing
    End Sub
    Private Sub ActualizaEstatusPedido(ByVal cfdid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pPedidos @cmd=23, @cfdid='" & cfdid.ToString & "'")
        ObjData = Nothing
    End Sub



    'Protected Sub AgregaCFDI(ByVal pedidoid As Integer, ByVal guia As String)
    '    'Inserta las partidas de los pedidos en TblCFD_partidas.
    '    Dim ObjData As New DataControl()
    '    For Each rows As DataRow In Session("TmpDetalleComplemento").Rows
    '        If CBool(rows("chkcfdid")) = True Then
    '            ObjData.RunSQLQuery("EXEC pPedidos @cmd=36, @clienteid='" & filtroclienteid.SelectedValue & "', @cfdid='" & Session("CFD") & "', @pedidoid='" & rows("id") & "'")
    '        End If
    '    Next
    '    ObjData = Nothing

    'End Sub

    Private Sub CrearTablaTemp()
        Dim dt As New DataTable()
        dt.Columns.Add("id")
        dt.Columns.Add("chkFacAutomaticas")
        dt.Columns.Add("cliente")
        dt.Columns.Add("fecha_alta")
        dt.Columns.Add("estatusid")
        dt.Columns.Add("ejecutivo")
        dt.Columns.Add("estatus")
        dt.Columns.Add("guia")
        dt.Columns.Add("olaid")
        dt.Columns.Add("olaestatusid")
        dt.Columns.Add("bodega")
        'dt.Columns.Add("timbrado")
        dt.Columns.Add("factura")
        dt.Columns.Add("orden_compra")
        dt.Columns.Add("sucursal")
        dt.Columns.Add("proyecto")
        dt.Columns.Add("condicionesid")
        dt.Columns.Add("pagoid")
        dt.Columns.Add("totalEmpaquetado")
        dt.Columns.Add("chkcfdid")
        dt.Columns.Add("chkcons")
        dt.Columns.Add("ola_estatus")
        Session("TmpDetalleComplemento") = dt
    End Sub

    Private Sub DisplayItems()
        pedidosList.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
        Session("TmpDetalleComplemento") = ObtenerItems().Tables(0)
        pedidosList.DataSource = Session("TmpDetalleComplemento")
        pedidosList.DataBind()
    End Sub

    Function ObtenerItems() As DataSet

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim cmd As New SqlDataAdapter("exec pPedidos @cmd=2, @userid='" & Session("userid").ToString & "', @clienteid='" & filtroclienteid.SelectedValue.ToString & "', @estatusid='" & filtroestatusid.SelectedValue.ToString & "', @txtSearch='" & txtSearch.Text & "'", conn)

        Dim ds As DataSet = New DataSet

        Try
            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Response.End()
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")

        Return ds

    End Function

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
        'RadWindow1.VisibleOnPageLoad = False
    End Sub

    Public Sub CargarSaldoPendiente()
        'Dim i As Integer = 0

        Session("TmpDetalleComplemento").Rows.Clear()

        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidosList.MasterTableView.Items
            Dim lblid As String = dataItem.GetDataKeyValue("id").ToString
            Dim lblchkFacAutomaticas As String = dataItem.GetDataKeyValue("chkFacAutomaticas").ToString
            Dim lblcondicionesid As String = dataItem.GetDataKeyValue("condicionesid").ToString
            Dim lblCliente As Label = DirectCast(dataItem.FindControl("lblCliente"), Label)
            Dim lblSucursal As Label = DirectCast(dataItem.FindControl("lblSucursal"), Label)
            Dim lblProyecto As Label = DirectCast(dataItem.FindControl("lblProyecto"), Label)
            Dim lblEjecutivo As Label = DirectCast(dataItem.FindControl("lblEjecutivo"), Label)
            Dim lblFecha_alta As Label = DirectCast(dataItem.FindControl("lblFecha_alta"), Label)
            Dim lblEstatusId As Label = DirectCast(dataItem.FindControl("lblEstatusId"), Label)
            Dim lblEstatus As Label = DirectCast(dataItem.FindControl("lblEstatus"), Label)
            Dim lblFactura As Label = DirectCast(dataItem.FindControl("lblFactura"), Label)
            Dim lblola As Label = DirectCast(dataItem.FindControl("lblola"), Label)
            Dim ola_estatus As Label = DirectCast(dataItem.FindControl("ola_estatus"), Label)
            Dim lblbodega As Label = DirectCast(dataItem.FindControl("lblbodega"), Label)
            'Dim lblTimbrado As Label = DirectCast(dataItem.FindControl("lblTimbrado"), Label)
            Dim lblGuia As Label = DirectCast(dataItem.FindControl("lblGuia"), Label)
            Dim lblPagoid As Label = DirectCast(dataItem.FindControl("lblPagoid"), Label)
            Dim lblOrden_compra As Label = DirectCast(dataItem.FindControl("lblOrden_compra"), Label)
            Dim totalEmpaquetado As Label = DirectCast(dataItem.FindControl("totalEmpaquetado"), Label)
            Dim chkcfdid As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkcfdid"), System.Web.UI.WebControls.CheckBox)
            Dim chkcons As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkcons"), System.Web.UI.WebControls.CheckBox)

            Dim dr As DataRow = Session("TmpDetalleComplemento").NewRow()
            dr.Item("id") = lblid.ToString
            dr.Item("chkFacAutomaticas") = lblchkFacAutomaticas.ToString
            dr.Item("condicionesid") = lblcondicionesid.ToString
            dr.Item("cliente") = lblCliente.Text
            dr.Item("fecha_alta") = lblFecha_alta.Text
            dr.Item("estatusid") = lblEstatusId.Text
            dr.Item("estatus") = lblEstatus.Text
            dr.Item("ejecutivo") = lblEjecutivo.Text
            dr.Item("guia") = lblGuia.Text
            dr.Item("olaid") = lblola.Text
            dr.Item("olaestatusid") = lblola.Text
            dr.Item("bodega") = lblbodega.Text
            'dr.Item("timbrado") = lblTimbrado.Text
            dr.Item("factura") = lblFactura.Text
            dr.Item("orden_compra") = lblOrden_compra.Text
            dr.Item("sucursal") = lblSucursal.Text
            dr.Item("proyecto") = lblProyecto.Text
            dr.Item("pagoid") = lblPagoid.Text
            dr.Item("ola_estatus") = ola_estatus.Text
            dr.Item("totalEmpaquetado") = totalEmpaquetado.Text

            dr.Item("chkcfdid") = chkcfdid.Checked
            dr.Item("chkcons") = chkcons.Checked
            Session("TmpDetalleComplemento").Rows.Add(dr)
        Next

        pedidosList.DataSource = Session("TmpDetalleComplemento")
        pedidosList.DataBind()

        'CargaTotalCFDI()
        'panelResume.Visible = True
    End Sub

    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
    End Sub


    Protected Sub chkAllcons_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
    End Sub
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

                total = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Total", "cfdi:Comprobante")
                importe = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SubTotal", "cfdi:Comprobante")
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

                If tipoid = 2 Then
                    reporte.ReportParameters("txtTipoRelacion").Value = "Tipo Relación: " & tiporelacion
                    reporte.ReportParameters("txtUUIDRelacionado").Value = "UUID Relacionado: " & uuid_relacionado
                ElseIf tipoid = 5 Then
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

                Dim Periocidadx As Integer = 4
                Dim PerioMesx As Integer = Format(DateTime.Now.Month, "0#")
                Dim PerioAnio As String = Format(DateTime.Now.Year, "000#")

                instrucciones = instrucciones + " | " + "Factura Global con Periodicidad " & Periocidadx.ToString & ", Correspondiente al Mes de " & PerioMesx.ToString & " del Año " & PerioAnio.ToString & "."

                reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString
                reporte.ReportParameters("txtUsoCFDI").Value = "Uso de CFDI: " & usoCFDI

                Return reporte

        End Select

    End Function
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
            tipoid = 1
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


End Class


