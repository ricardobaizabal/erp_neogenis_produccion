Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports Telerik.Web.UI.GridExcelBuilder
Imports System.Globalization
Imports System.Threading

Public Class pedidosMultiples
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("perfilid") <> 1 Then
                pedidosList.MasterTableView.GetColumn("ColEtapaAbierto").Visible = False
            End If

            CrearTablaTemp()

            'Dim ObjCat As New DataControl
            'ObjCat.Catalogo(cmbCliente, "EXEC pCatalogos @cmd=4 ", 0)

            'ObjCat = Nothing

            Call CargaClientes()
            Call CargaSucursales()
            Call CargaAlmacenes()
            Call CargaProyectos()
            'Call MuestraPedidos()
            'Call CargaEstatus()

            'fha_ini.SelectedDate = Now.AddDays(-90)
            'fha_fin.SelectedDate = Now()
        End If
    End Sub

    Private Sub CargaClientes()
        Dim ObjCat As New DataControl
        'ObjCat.Catalogo(cmbCliente, "exec pPedidos @cmd=12, @userid='" & Session("userid").ToString & "'", 0)
        ObjCat.Catalogo(filtroclienteid, "exec pPedidos @cmd=12, @userid='" & Session("userid").ToString & "'", 0, True)
        ObjCat = Nothing
    End Sub

    Private Sub CargaSucursales()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(sucursalid, "EXEC pListarSucursales @clienteid='" & filtroclienteid.SelectedValue & "'", 0)
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

    'Private Sub MuestraPedidos()
    '    Dim ObjData As New DataControl()
    '    Dim dsData As New DataSet()
    '    Dim cmd As String = "exec pPedidos @cmd=2, @userid='" & Session("userid").ToString & "', @clienteid='" & filtroclienteid.SelectedValue.ToString & "', @estatusid='" & filtroestatusid.SelectedValue.ToString & "', @txtSearch='" & txtSearch.Text & "'"
    '    dsData = ObjData.FillDataSet(cmd)
    '    If dsData.Tables(0).Rows.Count > 0 Then
    '        pedidosList.DataSource = dsData
    '        pedidosList.DataBind()
    '    End If
    '    ObjData = Nothing
    'End Sub

    'Private Sub CargaEstatus()
    '    Dim ObjCat As New DataControl
    '    ObjCat.Catalogo(filtroestatusid, "select id, nombre from tblPedidoEstatus order by nombre", 0, True)
    '    ObjCat = Nothing
    'End Sub

    Protected Sub pedidosList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles pedidosList.ItemCommand
        Dim objdata As New DataControl()
        Select Case e.CommandName
            Case "cmdEditar"
                Response.Redirect("editapedido.aspx?id=" & e.CommandArgument)
            Case "cmdEliminar"
                objdata.RunSQLQuery("exec pPedidos @cmd=4, @pedidoid=" & e.CommandArgument)
                objdata = Nothing
               ' Call MuestraPedidos()
            'Case "cmdFacturar40"
            '    Call Facturar40(e.CommandArgument)
            'Case "cmdEtapaAbierto"
            '    Call CambiarEstatus(e.CommandArgument)
            '    'Call MuestraPedidos()
            Case "cmdUpGuia"
        End Select
    End Sub

    Private Sub CambiarEstatus(ByVal pedidoid As Integer)
        Dim ObjData As New DataControl()
        ObjData.RunSQLQuery("exec pPedidos @cmd=26, @userid='" & Session("userid").ToString & "', @pedidoid='" & pedidoid.ToString & "'")
        ObjData = Nothing
    End Sub

    Protected Sub pedidosList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles pedidosList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem

                Dim btnEtapaAbierto As ImageButton = CType(e.Item.FindControl("btnEtapaAbierto"), ImageButton)

                Dim itm As Telerik.Web.UI.GridDataItem
                itm = CType(e.Item, Telerik.Web.UI.GridDataItem)

                Dim intEstatus As Integer = itm.GetDataKeyValue("estatusid")

                'Dim itemTimbrado As Boolean = itm.GetDataKeyValue("timbrado")

                If intEstatus > 3 Then 'Deshabilitar Editar y Eliminar si el estatus es diferente a Abierto
                    CType(itm("ColDelete").Controls(1), ImageButton).Visible = False
                Else
                    btnEtapaAbierto.Visible = False
                End If
                If intEstatus = 4 Then 'Deshabilitar si el estatus es cancelado
                    btnEtapaAbierto.Visible = False
                    CType(itm("ColEditar").Controls(1), ImageButton).Visible = False
                    'CType(itm("ColFacturar").Controls(1), LinkButton).Visible = False
                End If

                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un pedido. ¿Desea continuar?')")
                btnEtapaAbierto.Attributes.Add("onclick", "javascript:return confirm('Va a regresar a estatus abierto un pedido. ¿Desea continuar?')")
        End Select
    End Sub

    Private Sub filtroclienteid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles filtroclienteid.SelectedIndexChanged
        'Call ObtenerItems()
        Call CargaSucursales()
        DisplayItems()
    End Sub


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl()
        Dim dsData As New DataSet()
        dsData = ObjData.FillDataSet("exec pPedidos @cmd=35, @userid='" & Session("userid").ToString & "', @clienteid='" & filtroclienteid.SelectedValue.ToString & "', @txtSearch='" & txtSearch.Text & "'")
        pedidosList.DataSource = dsData
        pedidosList.DataBind()
        ObjData = Nothing

        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '

    End Sub

    Private Sub AgregaPedido()
        Dim id As Integer = 0
        Dim ObjData As New DataControl
        Try
            id = ObjData.RunSQLScalarQuery("exec pPedidos @cmd=1, @userid=" & Session("userid") & ", @estatusid=1, @clienteid='" & filtroclienteid.SelectedValue & "'")
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
        cacheTable.Columns.Add("totalpiezas", GetType(String))
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
                cacheTable.Rows.Add(row("fecha"), row("mes"), i, row("cliente"), row("marca"), row("nopedido"), row("modelo"), row("sku"), row("totalpiezas"), row("comprador"), row("clientefinal"), row("clientefinalciudad"), row("guia"), row("comentarios"), row("fullshopify"), row("metodopago"), row("factura"), row("montototal"), row("idpago"))
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


#Region "Pedidos Multiples"

    Private Sub CrearTablaTemp()
        Dim dt As New DataTable()
        dt.Columns.Add("id")
        dt.Columns.Add("cliente")
        dt.Columns.Add("fecha_alta")
        dt.Columns.Add("estatusid")
        dt.Columns.Add("ejecutivo")
        dt.Columns.Add("estatus")
        dt.Columns.Add("guia")
        'dt.Columns.Add("timbrado")
        dt.Columns.Add("factura")
        dt.Columns.Add("orden_compra")
        dt.Columns.Add("sucursal")
        dt.Columns.Add("proyecto")
        dt.Columns.Add("condicionesid")
        dt.Columns.Add("pagoid")
        dt.Columns.Add("chkcfdid", GetType(Boolean))
        Session("TmpDetalleComplemento") = dt
    End Sub

    Private Sub DisplayItems()
        pedidosList.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
        Session("TmpDetalleComplemento") = ObtenerItems().Tables(0)
        pedidosList.DataSource = Session("TmpDetalleComplemento")
        pedidosList.DataBind()
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
        'RadWindow1.VisibleOnPageLoad = False
    End Sub

    Public Sub CargarSaldoPendiente()
        Dim i As Integer = 0

        Session("TmpDetalleComplemento").Rows.Clear()

        For Each dataItem As Telerik.Web.UI.GridDataItem In pedidosList.MasterTableView.Items
            Dim lblid As String = dataItem.GetDataKeyValue("id").ToString
            Dim lblCliente As Label = DirectCast(dataItem.FindControl("lblCliente"), Label)
            Dim lblSucursal As Label = DirectCast(dataItem.FindControl("lblSucursal"), Label)
            Dim lblProyecto As Label = DirectCast(dataItem.FindControl("lblProyecto"), Label)
            Dim lblEjecutivo As Label = DirectCast(dataItem.FindControl("lblEjecutivo"), Label)
            Dim lblFecha_alta As Label = DirectCast(dataItem.FindControl("lblFecha_alta"), Label)
            Dim lblEstatusId As Label = DirectCast(dataItem.FindControl("lblEstatusId"), Label)
            Dim lblEstatus As Label = DirectCast(dataItem.FindControl("lblEstatus"), Label)
            Dim lblFactura As Label = DirectCast(dataItem.FindControl("lblFactura"), Label)
            ' Dim lblTimbrado As Label = DirectCast(dataItem.FindControl("lblTimbrado"), Label)
            Dim lblGuia As Label = DirectCast(dataItem.FindControl("lblGuia"), Label)
            Dim lblPagoid As Label = DirectCast(dataItem.FindControl("lblPagoid"), Label)
            Dim lblOrden_compra As Label = DirectCast(dataItem.FindControl("lblOrden_compra"), Label)
            Dim chkcfdid As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkcfdid"), System.Web.UI.WebControls.CheckBox)

            Dim dr As DataRow = Session("TmpDetalleComplemento").NewRow()
            dr.Item("id") = lblid.ToString
            dr.Item("cliente") = lblCliente.Text
            dr.Item("fecha_alta") = lblFecha_alta.Text
            dr.Item("estatusid") = lblEstatusId.Text
            dr.Item("estatus") = lblEstatus.Text
            dr.Item("ejecutivo") = lblEjecutivo.Text
            dr.Item("guia") = lblGuia.Text
            'dr.Item("timbrado") = lblTimbrado.text
            dr.Item("factura") = lblFactura.Text
            dr.Item("orden_compra") = lblOrden_compra.Text
            dr.Item("sucursal") = lblSucursal.Text
            dr.Item("proyecto") = lblProyecto.Text
            dr.Item("pagoid") = lblPagoid.Text
            dr.Item("chkcfdid") = chkcfdid.Checked
            Session("TmpDetalleComplemento").Rows.Add(dr)
        Next

        pedidosList.DataSource = Session("TmpDetalleComplemento")
        pedidosList.DataBind()

        'CargaTotalCFDI()
        'panelResume.Visible = True
    End Sub

    Private Sub ValidarExistComplemento()
        If Session("CFD") = 0 Then
            'GetCFD()
        End If

        'If Session("PAGOCFD") = 0 Then
        '    GetPAGOCFD()
        'End If
    End Sub

    Protected Sub GetCFD()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pPedidos @cmd=38, @clienteid='" & filtroclienteid.SelectedValue & "', @sucursalid='" & sucursalid.SelectedValue & "', @tasaid=3, @proyectoid='" & proyectoid.SelectedValue & "', @almacenid='" & almacenid.SelectedValue & "'", conn)

        Try
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                Session("CFD") = rs("cfdid") 'activa la sesion de cfdid para los pedidos
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

    Private Sub pedidosList_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles pedidosList.NeedDataSource
        pedidosList.DataSource = Session("TmpDetalleComplemento")
    End Sub

    Protected Sub AgregaCFDI()

        Dim ObjData As New DataControl()
        For Each rows As DataRow In Session("TmpDetalleComplemento").Rows
            If CBool(rows("chkcfdid")) = True Then
                ObjData.RunSQLQuery("EXEC pPedidos @cmd=36, @clienteid='" & filtroclienteid.SelectedValue & "', @cfdid='" & Session("CFD") & "', @pedidoid='" & rows("id") & "'")
            End If
        Next

    End Sub
    Protected Sub AgregaCFDITabla()
        Dim ObjData As New DataControl()

        ObjData.RunSQLQuery("EXEC pPedidos @cmd=48, @cfdid='" & Session("CFD") & "'")

    End Sub

    Function ObtenerItems() As DataSet

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim cmd As New SqlDataAdapter("EXEC pPedidos @cmd=35,  @clienteid='" & filtroclienteid.SelectedValue & "'", conn)

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
    Private Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnFacturarMultiple.Click

        Try
            Call GetCFD()
            Call AgregaCFDI()
            Call AgregaCFDITabla()

            If Session("CFD") > 0 Then
                Response.Redirect("~/portalcfd/Facturar40.aspx?id=" & Session("CFD").ToString, False)
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString())
            Response.End()

        End Try

    End Sub

#End Region

End Class


