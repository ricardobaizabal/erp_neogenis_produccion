Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Partial Class portalcfd_almacen_ajustes
    Inherits System.Web.UI.Page

    Private Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        chkAll.Attributes.Add("onclick", "checkedAll(" & Me.Form.ClientID.ToString & ");")
        If Not IsPostBack Then
            Dim idsPudenAgregar As New List(Of Integer) From {0, 17, 18, 32, 34}
            If idsPudenAgregar.Contains(Session("userid")) Then
                PanelAgregarAjuste.Visible = True
            End If
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        lblMensaje.Text = ""
        btnAjustar.Visible = True
        gridResults.Visible = True
        chkAll.Visible = True
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
                Dim txtCantidad As RadNumericTextBox = DirectCast(e.Item.FindControl("txtCantidad"), RadNumericTextBox)
                txtCantidad.Text = "1"

                Dim almacenid As DropDownList = DirectCast(e.Item.FindControl("almacenid"), DropDownList)
                Dim ObjCat As New DataControl
                ObjCat.Catalogo(almacenid, "select id, nombre from tblAlmacen order by nombre", 0)
                ObjCat = Nothing
        End Select
    End Sub

    Protected Sub gridResults_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles gridResults.NeedDataSource
        btnAjustar.Visible = True
        gridResults.Visible = True
        chkAll.Visible = True
        gridResults.DataSource = GetProducts()
    End Sub

    Private Sub InsertItem(ByVal id As Long, ByVal item As GridItem)
        '
        '   Instancia elementos
        '
        Dim lblDisponibles As Label = DirectCast(item.FindControl("lblDisponibles"), Label)
        Dim lblCodigo As Label = DirectCast(item.FindControl("lblCodigo"), Label)
        Dim lblDescripcion As Label = DirectCast(item.FindControl("lblDescripcion"), Label)
        Dim txtCantidad As RadNumericTextBox = DirectCast(item.FindControl("txtCantidad"), RadNumericTextBox)
        Dim txtComentario As TextBox = DirectCast(item.FindControl("txtComentario"), TextBox)
        Dim almacenid As DropDownList = DirectCast(item.FindControl("almacenid"), DropDownList)

        Dim cantidad As Decimal = 0
        Try
            cantidad = Convert.ToDecimal(txtCantidad.Text)
        Catch ex As Exception
            cantidad = 0
        End Try
        If Convert.ToDecimal(lblDisponibles.Text) >= cantidad Then
            If almacenid.SelectedValue = 0 Then
                lblMensaje.Text = "Debes seleccionar un almacén"
            Else
                '
                '   Agrega ajuste
                '
                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pInventario @cmd=5, @productoid='" & id.ToString & "', @codigo='" & lblCodigo.Text & "', @descripcion='" & lblDescripcion.Text & "', @cantidad='" & txtCantidad.Text & "', @userid='" & Session("userid").ToString & "', @comentario='" & txtComentario.Text & "', @almacenid='" & almacenid.SelectedValue.ToString & "'")
                ObjData = Nothing
                lblMensaje.Text = ""
            End If
        Else
            lblMensaje.Text = "La cantidad que desea descontar del inventario es mayor a la existencia para este producto"
        End If

        btnAjustar.Visible = False
        gridResults.Visible = False
        chkAll.Visible = False
        Call MuestraUltimosMovimientos()

    End Sub

    Private Sub MuestraUltimosMovimientos()
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pInventario @cmd=6")
        productslist.DataBind()
        ObjData = Nothing
    End Sub

    Protected Sub productslist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productslist.NeedDataSource
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pInventario @cmd=6")
        ObjData = Nothing
    End Sub

    Private Sub btnAjustar_Click(sender As Object, e As EventArgs) Handles btnAjustar.Click
        Dim listErrores As New List(Of String)
        Dim elementos As Integer = 0
        For Each row As GridDataItem In gridResults.Items
            Dim chkItem As CheckBox = DirectCast(row.FindControl("chkItem"), CheckBox)
            If chkItem.Checked = True Then
                elementos += 1
                Dim almacenid As DropDownList = DirectCast(row.FindControl("almacenid"), DropDownList)
                Dim lblProductoId As Label = DirectCast(row.FindControl("lblProductoId"), Label)
                Dim lblDisponibles As Label = DirectCast(row.FindControl("lblDisponibles"), Label)
                Dim lblCodigo As Label = DirectCast(row.FindControl("lblCodigo"), Label)
                Dim lblDescripcion As Label = DirectCast(row.FindControl("lblDescripcion"), Label)
                Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
                Dim txtComentario As TextBox = DirectCast(row.FindControl("txtComentario"), TextBox)
                Dim cantidad As Decimal = 0
                Try
                    cantidad = Convert.ToDecimal(txtCantidad.Text)
                Catch ex As Exception
                    cantidad = 0
                End Try
                If Convert.ToDecimal(lblDisponibles.Text) >= cantidad Then
                    If almacenid.SelectedValue = 0 Then
                        listErrores.Add("Debes seleccionar un almacén")
                    Else
                        '
                        '   Agrega ajuste
                        '
                        Dim ObjData As New DataControl
                        ObjData.RunSQLQuery("exec pInventario @cmd=5, @productoid='" & lblProductoId.Text & "', @codigo='" & lblCodigo.Text & "', @descripcion='" & lblDescripcion.Text & "', @cantidad='" & txtCantidad.Text & "', @userid='" & Session("userid").ToString & "', @comentario='" & txtComentario.Text & "', @almacenid='" & almacenid.SelectedValue.ToString & "'")
                        ObjData = Nothing
                        lblMensaje.Text = ""
                    End If
                Else
                    listErrores.Add("La cantidad que desea ajustar del producto " & lblDescripcion.Text & " es mayor a la disponibilidad para este producto.")
                End If
            End If
        Next
        Dim mensaje As String
        If elementos <= 0 Then
            mensaje = "Debes seleccionar almenos un registro para el ajuste."
        Else
            mensaje = String.Join(vbCrLf, listErrores.ToArray())
        End If
        btnAjustar.Visible = False
        gridResults.Visible = False
        chkAll.Visible = False
        Call MuestraUltimosMovimientos()
        lblMensaje.Text = mensaje
    End Sub

    Public Sub almacenid_SelectedIndexChanged(sender As Object, e As EventArgs)
        For Each dataItem As Telerik.Web.UI.GridDataItem In gridResults.MasterTableView.Items
            'Dim deduccionId As String = dataItem.GetDataKeyValue("id").ToString
            Dim almacenid As DropDownList = DirectCast(dataItem.FindControl("almacenid"), DropDownList)
            Dim lblDisponibles As Label = DirectCast(dataItem.FindControl("lblDisponibles"), Label)

            Dim lblMonterrey As Label = DirectCast(dataItem.FindControl("lblMonterrey"), Label)
            Dim lblMexico As Label = DirectCast(dataItem.FindControl("lblMexico"), Label)
            Dim lblGuadalajara As Label = DirectCast(dataItem.FindControl("lblGuadalajara"), Label)
            Dim lblMermas As Label = DirectCast(dataItem.FindControl("lblMermas"), Label)
            Dim lblMatriz As Label = DirectCast(dataItem.FindControl("lblMatriz"), Label)

            If almacenid.SelectedValue = 1 Then
                lblDisponibles.Text = lblMonterrey.Text
            ElseIf almacenid.SelectedValue = 2 Then
                lblDisponibles.Text = lblMexico.Text
            ElseIf almacenid.SelectedValue = 3 Then
                lblDisponibles.Text = lblGuadalajara.Text
            ElseIf almacenid.SelectedValue = 4 Then
                lblDisponibles.Text = lblMermas.Text
            ElseIf almacenid.SelectedValue = 5 Then
                lblDisponibles.Text = lblMatriz.Text
            End If

        Next
    End Sub

End Class