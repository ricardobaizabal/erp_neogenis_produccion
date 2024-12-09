Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.Threading
Imports System.IO
Imports System.Linq
Public Class agregaordenparcial
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call MuestraDatosGenerales()
            Call CargaConceptos()
        End If
        '
        btnProcess.Attributes.Add("onclick", "javascript:return confirm('Va a procesar este pedido, una vez procesado no podrá modificarlo. ¿Desea continuar?');")
        '
    End Sub

    Private Sub MuestraDatosGenerales()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("exec pOrdenCompra @cmd=3, @ordenid='" & Request("id").ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                lblClaveEdit.Text = rs("claveEdit").ToString
                Dim ObjData As New DataControl
                ObjData.Catalogo(proveedorid, "select id, razonsocial as nombre from tblMisProveedores order by razonsocial", rs("proveedorid"))
                ObjData.Catalogo(proyectoid, "select id, nombre from tblProyecto order by nombre", rs("marcaid"))
                ObjData = Nothing

                Dim edate1 = rs("fechaEstEnt").ToString
                Dim fechaEstEnt As Date = Date.ParseExact(edate1, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)

                txtfechaEstEnt.SelectedDate = fechaEstEnt
                txtComentarios.Text = rs("comentarios")
                Session("estatusid") = rs("estatusid")

                'If Session("estatusid").ToString = "3" Then
                '    btnProcess.Enabled = False
                '    btnProcess.ToolTip = "Operación no permitida."
                'End If

                If rs("estatusid") <> 1 Then
                    proveedorid.Enabled = False
                    proyectoid.Enabled = False
                    txtComentarios.Enabled = False
                    btnAddorder.Enabled = False
                    btnProcess.Enabled = False
                End If
            End If
        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
    End Sub

    Private Sub CargaConceptos()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=7, @ordenid='" & Request("id").ToString & "'")
        conceptosList.DataSource = ds
        conceptosList.DataBind()

        ObjData = Nothing
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub

    Private Sub ListOrdenesParciales_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles ListOrdenesParciales.NeedDataSource
        Dim ObjData As New DataControl
        ListOrdenesParciales.DataSource = ObjData.FillDataSet("exec pOrdenCompra @cmd=13,  @ordenid='" & Request("id").ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub EliminaOrdenParcial(ByVal idOrdenPracial As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=14, @id='" & idOrdenPracial.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub conceptosList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles conceptosList.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Call EliminaConcepto(e.CommandArgument)
                Call CargaConceptos()
        End Select
    End Sub

    Private Sub ListOrdenesParciales_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles ListOrdenesParciales.ItemCommand
        Select Case e.CommandName
            Case "cmdConceptosParciales"
                'Call MuestraConceptosParciales()
                orderParcialIdHidden.Value = (e.CommandArgument)
                Call MuestraConceptosParciales()
                'ListConceptosParciales.DataSource = orderParcialIdHidden.Value
        End Select
    End Sub
    Private Sub MuestraConceptosParciales()

        'Dim ConceptosParcialesData As New DataSet
        'Dim ordenIdParcial As Integer = orderParcialIdHidden.Value
        Dim obj As New DataControl
        ds = obj.FillDataSet("exec pOrdenCompra @cmd=18, @ordenid='" & orderParcialIdHidden.Value.ToString & "'")
        ListConceptosParciales.DataSource = ds
        ListConceptosParciales.DataBind()
        obj = Nothing
        ListConceptosParciales.Visible = True

    End Sub


    Private Sub conceptosList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles conceptosList.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim ordenIdParcial As Integer = 0
                Dim txtCantidad As RadNumericTextBox = CType(e.Item.FindControl("txtCantidad"), RadNumericTextBox)
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un concepto de la orden de compra. ¿Desea continuar?');")

                If Session("estatusid") <> 1 Then
                    btnDelete.Visible = False
                End If
                '
                'Case GridItemType.Footer
                '    If ds.Tables(0).Rows.Count > 0 Then
                '        e.Item.Cells(4).Text = ds.Tables(0).Compute("sum(cantidad)", "")
                '        e.Item.Cells(4).Font.Bold = True
                '        e.Item.Cells(4).HorizontalAlign = HorizontalAlign.Center
                '        e.Item.Cells(6).Text = FormatCurrency(ds.Tables(0).Compute("sum(costo_variable)", ""), 2).ToString
                '        e.Item.Cells(6).Font.Bold = True
                '        e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                '        e.Item.Cells(7).Text = FormatCurrency(ds.Tables(0).Compute("sum(total)", ""), 2).ToString
                '        e.Item.Cells(7).Font.Bold = True
                '        e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                '    End If
        End Select
    End Sub

    Private Sub conceptosList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles conceptosList.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=7, @ordenid='" & Request("id").ToString & "'")
        conceptosList.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub EliminaConcepto(ByVal conceptoid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=6, @conceptoid='" & conceptoid.ToString & "'")
        ObjData = Nothing
    End Sub

    Protected Sub btnAddorder_Click(sender As Object, e As EventArgs) Handles btnAddorder.Click
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim conceptoId As Long = 0
        Dim editClave As String = ""
        Dim ObjData As New DataControl
        Dim codigo As String = ""
        Dim cantidad_oc As Integer = 0
        Dim disponible_oc As Integer = 0
        Dim ordenId As String = ""
        Dim ordenIdParcial As Integer = 0
        Dim descripcion As String = ""
        Dim costo As Double = 0.00
        Dim moneda As String = ""

        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=19, @ordenId='" & Request("id").ToString & "'")


        For Each row As GridDataItem In conceptosList.MasterTableView.Items
            'conceptoId = row.GetDataKeyValue("id")
            ordenId = row.GetDataKeyValue("ordenId")
            'ordenIdParcial = row.GetDataKeyValue("ordenIdParcial") @ordenIdParcial'" & ordenIdParcial.ToString & "',
            cantidad_oc = row.GetDataKeyValue("cantidadoc")
            disponible_oc = row.GetDataKeyValue("disponible")
            codigo = row.GetDataKeyValue("codigo")
            descripcion = row.GetDataKeyValue("descripcion")
            costo = row.GetDataKeyValue("costo")
            moneda = row.GetDataKeyValue("moneda")
            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
            If Convert.ToDecimal(txtCantidad.Text.ToString) > 0 Then
                ObjData.RunSQLQuery("exec pOrdenCompra @cmd=17,
                                                       @ordenId='" & ordenId.ToString & "',
                                                       @cantidad_oc='" & cantidad_oc.ToString & "',
                                                       @disponible_oc='" & disponible_oc.ToString & "',
                                                       @descripcion='" & descripcion.ToString & "',
                                                       @costo='" & costo.ToString & "',
                                                       @moneda='" & moneda.ToString & "',
                                                       @codigo='" & codigo.ToString & "',
                                                       @cantidad='" & txtCantidad.Text & "'")
            End If
        Next

        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
        lblMensaje.Text = "Datos actualizados."
        '
        Call CargaConceptos()
        Call CargaListaOrdenesParciales()
        '
    End Sub

    Private Sub CargaListaOrdenesParciales()
        Dim ObjData As New DataControl
        ListOrdenesParciales.DataSource = ObjData.FillDataSet("exec pOrdenCompra @cmd=13,  @ordenid='" & Request("id").ToString & "'")
        ListOrdenesParciales.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As System.EventArgs) Handles btnProcess.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=9, @ordenid='" & Request("id").ToString & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub

    Private Sub btnregresaList_Click(sender As Object, e As EventArgs) Handles btnregresaList.Click
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub


    Private Sub btneditaDatosGen_Click(sender As Object, e As System.EventArgs) Handles btneditaDatosGen.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=16, @ordenId='" & Request("id").ToString & "', @proveedorId='" & proveedorid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @comentarios='" & txtComentarios.Text & "',@fechaEstEnt='" & txtfechaEstEnt.SelectedDate.Value.ToShortDateString & "', @marcaid='" & proyectoid.SelectedValue.ToString & "'")
        ObjData = Nothing
        '
        lblMensajeEdit.Text = "Datos actualizados."
    End Sub

    'Private Sub ListConceptosParciales_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles ListConceptosParciales.NeedDataSource
    '    Dim ObjData As New DataControl
    '    Dim ordenIdParcial As Integer = orderParcialIdHidden.Value

    '    ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=18, @ordenid='" & ordenIdParcial.ToString & "'")
    '    ListConceptosParciales.DataSource = ds
    '    ObjData = Nothing
    'End Sub
End Class