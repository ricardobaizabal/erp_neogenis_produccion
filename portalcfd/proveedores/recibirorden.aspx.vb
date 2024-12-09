Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.Threading
Public Class recibirorden
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            btnProcesar.Attributes.Add("onclick", "javascript:return confirm('Va a dar entrada al almacén los productos seleccionados. ¿Desea continuar?');")
            btnProcessCancel.Attributes.Add("onclick", "javascript:return confirm('Va a cancelar un documento. ¿Desea continuar?');")
            Call MuestraDatosGenerales()
            Call CargaConceptos()
            'Call CargaOrdenParcial()
        End If
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
                lblFechaEstEnt.Text = rs("fechaEstEnt")
                txtComentarios.Text = rs("comentarios")
                Session("estatusid") = rs("estatusid")
                '
                If rs("estatusid") = 2 Then
                    btnProcessCancel.Enabled = False
                    btnProcessCancel.ToolTip = "Oporación no permitida"
                End If
                '
                If rs("estatusid") = 3 Or rs("estatusid") = 4 Then
                    proveedorid.Enabled = False
                    proyectoid.Enabled = False
                    txtComentarios.Enabled = False
                    btnProcesar.Enabled = False
                    btnProcessCancel.Enabled = False
                    btnProcesar.ToolTip = "Oporación no permitida"
                    btnProcessCancel.ToolTip = "Oporación no permitida"
                End If
                '
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
        ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=10, @ordenid='" & Request("id").ToString & "'")
        conceptosList.DataSource = ds
        conceptosList.DataBind()
        ObjData = Nothing
    End Sub

    'Private Sub CargaOrdenParcial()
    '    Dim ObjData As New DataControl
    '    ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=13, @ordenid='" & Request("id").ToString & "'")
    '    OrdenParcialList.DataSource = ds
    '    OrdenParcialList.DataBind()
    '    ObjData = Nothing
    'End Sub

    Private Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub
    
    Private Sub conceptosList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles conceptosList.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Call EliminaConcepto(e.CommandArgument)
                Call CargaConceptos()
        End Select
    End Sub

    Private Sub conceptosList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles conceptosList.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim almacenid As DropDownList = CType(e.Item.FindControl("almacenid"), DropDownList)
                Dim ObjData As New DataControl
                ObjData.Catalogo(almacenid, "select id, nombre from tblAlmacen order by nombre", 0)
                ObjData = Nothing

                Dim btnAdd As ImageButton = CType(e.Item.FindControl("btnAdd"), ImageButton)
                btnAdd.Attributes.Add("onClick", "javascript:openRadWindow(" & e.Item.DataItem("id").ToString() & "); return false;")
                btnAdd.CausesValidation = False

            Case GridItemType.Footer
                'If ds.Tables(0).Rows.Count > 0 Then
                '    e.Item.Cells(4).Text = ds.Tables(0).Compute("sum(cantidad)", "")
                '    e.Item.Cells(4).Font.Bold = True
                '    e.Item.Cells(4).HorizontalAlign = HorizontalAlign.Center
                '    e.Item.Cells(6).Text = FormatCurrency(ds.Tables(0).Compute("sum(total)", ""), 2).ToString
                '    e.Item.Cells(6).Font.Bold = True
                '    e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                'End If
        End Select
    End Sub

    Private Sub conceptosList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles conceptosList.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=10, @ordenid='" & Request("id").ToString & "'")
        conceptosList.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub EliminaConcepto(ByVal conceptoid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=6, @conceptoid='" & conceptoid.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub btnProcessCancel_Click(sender As Object, e As EventArgs) Handles btnProcessCancel.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=12, @ordenid='" & Request("id").ToString & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub

    Private Sub btnProcesar_Click(sender As Object, e As EventArgs) Handles btnProcesar.Click

        Dim cantidad_pedida As Long = 0
        Dim ordendetalleid As Long = 0
        Dim codigo As String = ""
        Dim mensaje As String = ""

        For Each row As GridDataItem In conceptosList.MasterTableView.Items

            ordendetalleid = row.GetDataKeyValue("id")
            cantidad_pedida = row.GetDataKeyValue("cantidad")
            codigo = row.GetDataKeyValue("codigo")

            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
            Dim txtCostoVariable As RadNumericTextBox = DirectCast(row.FindControl("txtCostoVariable"), RadNumericTextBox)
            Dim almacenid As DropDownList = DirectCast(row.FindControl("almacenid"), DropDownList)
            Dim cantidad As Decimal = 0
            Dim costo_variable As Decimal = 0

            Try
                cantidad = Convert.ToDecimal(txtCantidad.Text)
            Catch ex As Exception
                cantidad = 0
            End Try

            If cantidad > 0 Then
                If almacenid.SelectedValue > 0 Then
                    Dim ObjData As New DataControl
                    Dim resultado As String = ""
                    resultado = ObjData.RunSQLScalarQueryString("exec pControlInventario @cmd=1, @ordendetalleid='" & ordendetalleid.ToString & "', @costo_variable='" & txtCostoVariable.Text & "', @almacenid='" & almacenid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @cantidad='" & cantidad.ToString & "'")


                    If resultado.Length > 0 Then
                        mensaje += mensaje & resultado & "</br>"
                    End If

                    ObjData = Nothing
                Else
                    mensaje += "Debes seleccionar almaceén para el código: " & codigo.ToString & "</br>"
                End If

            End If

        Next
        Dim ObjData2 As New DataControl
        ObjData2.RunSQLQuery("exec pOrdenCompra @cmd=15, @ordenId='" & Request("id").ToString & "',@ordendetalleid='" & ordendetalleid.ToString & "'")
        ObjData2 = Nothing


        lblMensaje.ForeColor = Drawing.Color.Red
        lblMensaje.Text = mensaje

        Call CargaConceptos()
        'Call CargaOrdenParcial()

    End Sub

End Class