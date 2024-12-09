Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Globalization
Imports Telerik.Web.UI

Partial Class portalcfd_almacen_entradas
    Inherits System.Web.UI.Page
    Private Sub portalcfd_almacen_entradas_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim idsPudenAgregar As New List(Of Integer) From {0, 18, 32, 34}
            If idsPudenAgregar.Contains(Session("userid")) Then
                PanelAgregarEntrada.Visible = True
            End If
        End If
    End Sub
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        gridResults.Visible = True
        gridResults.DataSource = GetProducts()
        gridResults.DataBind()
    End Sub

    Function GetProducts() As DataSet
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pInventario @cmd=2, @txtSearch='" & txtSearch.Text & "'", conn)
        Dim ds As DataSet = New DataSet
        Try
            conn.Open()
            cmd.Fill(ds)
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return ds
    End Function

    Protected Sub gridResults_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles gridResults.ItemCommand
        Select Case e.CommandName
            Case "cmdAdd"
                Call InsertItem(e.CommandArgument, e.Item)
        End Select
    End Sub

    Protected Sub gridResults_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles gridResults.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim lblPerecederoBit As Label = DirectCast(e.Item.FindControl("lblPerecederoBit"), Label)
                Dim txtCantidad As RadNumericTextBox = DirectCast(e.Item.FindControl("txtCantidad"), RadNumericTextBox)
                Dim txtCostoUnitario As RadNumericTextBox = DirectCast(e.Item.FindControl("txtCostoUnitario"), RadNumericTextBox)
                'Dim txtLote As TextBox = DirectCast(e.Item.FindControl("txtLote"), TextBox)
                'Dim caducidad As RadDatePicker = DirectCast(e.Item.FindControl("caducidad"), RadDatePicker)

                txtCantidad.Text = "1"
                txtCostoUnitario.Text = e.Item.DataItem("costo_estandar")

                'If lblPerecederoBit.Text = "False" Then
                '    txtLote.Visible = False
                '    caducidad.Visible = False
                'Else
                '    caducidad.SelectedDate = Date.Today
                'End If

                Dim almacenid As DropDownList = DirectCast(e.Item.FindControl("almacenid"), DropDownList)
                Dim ObjCat As New DataControl
                ObjCat.Catalogo(almacenid, "select id, nombre from tblAlmacen order by nombre", 0)
                ObjCat = Nothing

        End Select
    End Sub

    Protected Sub gridResults_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles gridResults.NeedDataSource
        gridResults.Visible = True
        gridResults.DataSource = GetProducts()
    End Sub

    Private Sub InsertItem(ByVal id As Long, ByVal item As GridItem)
        '
        '   Instancia elementos
        '
        lblMensaje.Text = ""

        Dim lblCodigo As Label = DirectCast(item.FindControl("lblCodigo"), Label)
        Dim lblDescripcion As Label = DirectCast(item.FindControl("lblDescripcion"), Label)
        Dim txtCantidad As RadNumericTextBox = DirectCast(item.FindControl("txtCantidad"), RadNumericTextBox)
        Dim txtCostoUnitario As RadNumericTextBox = DirectCast(item.FindControl("txtCostoUnitario"), RadNumericTextBox)
        Dim txtCostoUnitarioVariable As RadNumericTextBox = DirectCast(item.FindControl("txtCostoUnitarioVariable"), RadNumericTextBox)
        Dim txtDocumento As TextBox = DirectCast(item.FindControl("txtDocumento"), TextBox)
        Dim txtComentario As TextBox = DirectCast(item.FindControl("txtComentario"), TextBox)
        Dim almacenid As DropDownList = DirectCast(item.FindControl("almacenid"), DropDownList)

        If almacenid.SelectedValue = "0" Then
            lblMensaje.Text = "Selecciona un almacén"
            Return
        Else
            lblMensaje.Text = ""
        End If
        '
        '   Agrega entrada
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pInventario @cmd=3, @productoid='" & id.ToString & "', @codigo='" & lblCodigo.Text & "', @descripcion='" & lblDescripcion.Text & "', @cantidad='" & txtCantidad.Text & "', @costo_unitario='" & txtCostoUnitario.Text & "', @costo_unitario_var='" & txtCostoUnitarioVariable.Text & "', @documento='" & txtDocumento.Text & "', @userid='" & Session("userid").ToString & "', @comentario='" & txtComentario.Text & "', @almacenid='" & almacenid.SelectedValue.ToString & "'")
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
        '
        gridResults.Visible = False
        Call MuestraUltimosMovimientos()
        '
    End Sub

    Private Sub MuestraUltimosMovimientos()
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pInventario @cmd=4")
        productslist.DataBind()
        ObjData = Nothing
    End Sub

    Protected Sub productslist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productslist.NeedDataSource
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pInventario @cmd=4")
        ObjData = Nothing
    End Sub


End Class
