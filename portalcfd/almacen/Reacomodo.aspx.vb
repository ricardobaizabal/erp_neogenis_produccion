Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Void
Partial Class portalcfd_almacen_Reacomodo
    Inherits System.Web.UI.Page
    Private ds As DataSet

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

            CrearTablaTemp()

            Dim objCat As New DataControl
            'objCat.Catalogo(filtrocoleccionid, "select id, isnull(codigo,'') + ' - ' + isnull(nombre,'') as nombre from tblColeccion where isnull(borradoBit,0)=0 order by nombre", 0)
            objCat.Catalogo(cmbMarca, "select id, nombre from tblProyecto order by nombre", 0)
            objCat = Nothing

        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pInventario @cmd=14, @marca='" & cmbMarca.SelectedValue.ToString & "', @ubicacion='" & txtUbicacion.Text & "'")
        productslist.DataSource = ds
        productslist.DataBind()
        ObjData = Nothing
    End Sub

    'Function GetProducts() As DataSet
    '    Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
    '    Dim cmd As New SqlDataAdapter("EXEC pInventario @cmd=2, @txtSearch='" & txtSearch.Text & "'", conn)
    '    Dim ds As DataSet = New DataSet
    '    Try
    '        conn.Open()
    '        cmd.Fill(ds)
    '        conn.Close()
    '        conn.Dispose()
    '    Catch ex As Exception
    '    Finally
    '        conn.Close()
    '        conn.Dispose()
    '    End Try
    '    Return ds
    'End Function

    'Protected Sub gridResults_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles gridResults.ItemCommand
    '    Select Case e.CommandName
    '        Case "cmdAdd"
    '            Call InsertItem(e.CommandArgument, e.Item)
    '    End Select
    'End Sub

    Protected Sub productslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles productslist.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                'Dim txtCantidad As RadNumericTextBox = DirectCast(e.Item.FindControl("lblquantityOrigenUbicacion"), RadNumericTextBox)
                Dim lblquantityOrigenUbicacion As Label = CType(e.Item.FindControl("lblquantityOrigenUbicacion"), Label)

                'Dim almacenid As DropDownList = DirectCast(e.Item.FindControl("almacenid"), DropDownList)
                'Dim ObjCat As New DataControl
                'ObjCat.Catalogo(almacenid, "select id, nombre from tblAlmacen order by nombre", 0)
                'ObjCat = Nothing
                'Case Telerik.Web.UI.GridItemType.Footer
                '    If ds.Tables(0).Rows.Count > 0 Then
                '        e.Item.Cells(3).Text = "Totales"
                '        e.Item.Cells(3).Font.Bold = True

                '        'e.Item.Cells(6).Text = ds.Tables(0).Compute("sum(quantityOrigenUbicacion)", "")
                '        'e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Center
                '        'e.Item.Cells(6).Font.Bold = True
                '        '
                '        e.Item.Cells(8).Text = ds.Tables(0).Compute("sum(quantityDestination)", "")
                '        e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Center
                '        e.Item.Cells(8).Font.Bold = True
                '        '
                '        'e.Item.Cells(9).Text = ds.Tables(0).Compute("sum(quantityOrige)", "")
                '        'e.Item.Cells(9).HorizontalAlign = HorizontalAlign.Center
                '        'e.Item.Cells(9).Font.Bold = True
                '    End If
        End Select
    End Sub

    'Protected Sub gridResults_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles gridResults.NeedDataSource
    '    btnAjustar.Visible = True
    '    gridResults.Visible = True
    '    chkAll.Visible = True
    '    gridResults.DataSource = GetProducts()
    'End Sub

    'Private Sub InsertItem(ByVal id As Long, ByVal item As GridItem)
    '    '
    '    '   Instancia elementos
    '    '
    '    Dim lblDisponibles As Label = DirectCast(item.FindControl("lblDisponibles"), Label)
    '    Dim lblCodigo As Label = DirectCast(item.FindControl("lblCodigo"), Label)
    '    Dim lblDescripcion As Label = DirectCast(item.FindControl("lblDescripcion"), Label)
    '    Dim txtCantidad As RadNumericTextBox = DirectCast(item.FindControl("txtCantidad"), RadNumericTextBox)
    '    Dim txtComentario As TextBox = DirectCast(item.FindControl("txtComentario"), TextBox)
    '    Dim almacenid As DropDownList = DirectCast(item.FindControl("almacenid"), DropDownList)

    '    Dim cantidad As Decimal = 0
    '    Try
    '        cantidad = Convert.ToDecimal(txtCantidad.Text)
    '    Catch ex As Exception
    '        cantidad = 0
    '    End Try
    '    If Convert.ToDecimal(lblDisponibles.Text) >= cantidad Then
    '        If almacenid.SelectedValue = 0 Then
    '            lblMensaje.Text = "Debes seleccionar un almacén"
    '        Else
    '            '
    '            '   Agrega ajuste
    '            '
    '            Dim ObjData As New DataControl
    '            ObjData.RunSQLQuery("exec pInventario @cmd=5, @productoid='" & id.ToString & "', @codigo='" & lblCodigo.Text & "', @descripcion='" & lblDescripcion.Text & "', @cantidad='" & txtCantidad.Text & "', @userid='" & Session("userid").ToString & "', @comentario='" & txtComentario.Text & "', @almacenid='" & almacenid.SelectedValue.ToString & "'")
    '            ObjData = Nothing
    '            lblMensaje.Text = ""
    '        End If
    '    Else
    '        lblMensaje.Text = "La cantidad que desea descontar del inventario es mayor a la existencia para este producto"
    '    End If

    '    'btnAjustar.Visible = False
    '    'gridResults.Visible = False
    '    'chkAll.Visible = False
    '    Call MuestraUltimosMovimientos()

    'End Sub

    Private Sub MuestraUltimosMovimientos()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pInventario @cmd=14")
        productslist.DataSource = ds
        productslist.DataBind()
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
        Dim cmd As New SqlCommand("EXEC pInventario @cmd='17', @idCiclico ='" & id.ToString & "'", conn)

        conn.Open()

        cmd.ExecuteReader()

        conn.Close()

        'Call DisplayItems()

    End Sub

    Protected Sub productslist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productslist.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pInventario @cmd=14")
        productslist.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub CrearTablaTemp()
        Dim dt As New DataTable()
        dt.Columns.Add("id")
        dt.Columns.Add("chkReacomodo")
        'dt.Columns.Add("fecha")
        'dt.Columns.Add("identifierId")
        dt.Columns.Add("companyId")
        dt.Columns.Add("productOrigeId")
        dt.Columns.Add("quantityOrige")
        dt.Columns.Add("quantityOrigenUbicacion")
        dt.Columns.Add("barcodeLocationOrige")
        'dt.Columns.Add("chkNuevo")
        dt.Columns.Add("barcodeLocationDestination")
        dt.Columns.Add("quantityDestination")
        dt.Columns.Add("userId")
        Session("TmpDetalleComplemento") = dt
    End Sub

    Protected Sub ToggleRowSelection(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
        'RadWindow1.VisibleOnPageLoad = False
    End Sub

    Public Sub CargarSaldoPendiente()
        Dim i As Integer = 0

        Session("TmpDetalleComplemento").Rows.Clear()

        For Each dataItem As Telerik.Web.UI.GridDataItem In productslist.MasterTableView.Items
            Dim lblid As String = dataItem.GetDataKeyValue("id").ToString
            'Dim lblFecha As Label = DirectCast(dataItem.FindControl("lblFecha"), Label)
            'Dim lblidentifierId As Label = DirectCast(dataItem.FindControl("lblidentifierId"), Label)
            Dim lblcompanyId As Label = DirectCast(dataItem.FindControl("lblcompanyId"), Label)
            Dim lblproductOrigeId As Label = DirectCast(dataItem.FindControl("lblproductOrigeId"), Label)
            Dim lblquantityOrige As Label = DirectCast(dataItem.FindControl("lblquantityOrige"), Label)
            Dim lblquantityOrigenUbicacion As Label = DirectCast(dataItem.FindControl("lblquantityOrigenUbicacion"), Label)
            Dim lblbarcodeLocationOrige As Label = DirectCast(dataItem.FindControl("lblbarcodeLocationOrige"), Label)
            Dim lblbarcodeLocationDestination As Label = DirectCast(dataItem.FindControl("lblbarcodeLocationDestination"), Label)
            Dim lblquantityDestination As Label = DirectCast(dataItem.FindControl("lblquantityDestination"), Label)
            Dim lbluserId As Label = DirectCast(dataItem.FindControl("lbluserId"), Label)
            'Dim chkNuevo As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkNuevo"), System.Web.UI.WebControls.CheckBox)
            Dim chkReacomodo As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkReacomodo"), System.Web.UI.WebControls.CheckBox)


            Dim dr As DataRow = Session("TmpDetalleComplemento").NewRow()
            dr.Item("id") = lblid.ToString
            'dr.Item("fecha") = lblFecha.Text
            'dr.Item("identifierId") = lblidentifierId.Text
            dr.Item("companyId") = lblcompanyId.Text
            dr.Item("productOrigeId") = lblproductOrigeId.Text
            dr.Item("quantityOrige") = lblquantityOrige.Text
            dr.Item("quantityOrigenUbicacion") = lblquantityOrigenUbicacion.Text
            dr.Item("barcodeLocationOrige") = lblbarcodeLocationOrige.Text
            dr.Item("barcodeLocationDestination") = lblbarcodeLocationDestination.Text
            dr.Item("quantityDestination") = lblquantityDestination.Text
            dr.Item("userId") = lbluserId.Text
            'dr.Item("chkNuevo") = chkNuevo.Checked
            dr.Item("chkReacomodo") = chkReacomodo.Checked
            Session("TmpDetalleComplemento").Rows.Add(dr)
        Next

        productslist.DataSource = Session("TmpDetalleComplemento")
        productslist.DataBind()

        'CargaTotalCFDI()
        'panelResume.Visible = True
    End Sub

    Private Sub btnConfirmar_Click(sender As Object, e As EventArgs) Handles btnConfirmar.Click
        Call AgregaInventario()
    End Sub
    Protected Sub AgregaInventario()

        Dim ObjData As New DataControl()
        For Each rows As DataRow In Session("TmpDetalleComplemento").Rows
            If CBool(rows("chkReacomodo")) = True Then
                ' ObjData.RunSQLQuery("EXEC pInventario @cmd=15, @identificador ='" & rows("identifierId") & "', @cantidad='" & rows("quantityDestination") & "', @barcodeLocation='" & rows("barcodeLocationDestination") & "'")
                ObjData.RunSQLQuery("EXEC pInventario @cmd=15,  @id ='" & rows("id") & "', @cantidadDestino='" & rows("quantityDestination") & "', @cantidadOrigen='" & rows("quantityOrige") & "', @cantidadOrigenUbi='" & rows("quantityOrigenUbicacion") & "', @proX='" & rows("productOrigeId") & "', @barcodeLocationOrigen='" & rows("barcodeLocationOrige") & "', @barcodeLocationDestino='" & rows("barcodeLocationDestination") & "'")
            End If
        Next

        ObjData.RunSQLQuery("EXEC pInventario @cmd=19")
        ObjData = Nothing
        MuestraUltimosMovimientos()
        lblMensajeConf.Text = "Ubicaciones y Cantidades actualizadas"
    End Sub

    Protected Sub chkAll_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        CargarSaldoPendiente()
    End Sub

End Class