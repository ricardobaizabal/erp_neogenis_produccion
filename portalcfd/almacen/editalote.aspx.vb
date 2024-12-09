Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class editalote
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        If Not IsPostBack Then
            Call MuestraEtiqueta()
            Call CargaCatalogo()
            Call MuestraDetalle()
        End If
    End Sub

    Private Sub CargaCatalogo()
        Dim ObjData As New DataControl
        Dim almacenid As Integer = ObjData.RunSQLScalarQuery("EXEC pTransferencia @cmd=9, @transferenciaid='" & Request("id").ToString & "'")
        'ObjData.Catalogo(productoid, "EXEC pMisProductos @cmd=9, @almacenid='" & almacenid.ToString & "'", 0)
        ObjData = Nothing
    End Sub

    Private Sub MuestraEtiqueta()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try

            Dim cmd As New SqlCommand("EXEC pTransferencia @cmd=2, @transferenciaid='" & Request("id").ToString & "'", conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                lblFolio.Text = rs("id").ToString
                lblFecha.Text = rs("fecha").ToString
                lblOrigen.Text = rs("origen")
                lblDestino.Text = rs("destino")
                lblUsuario.Text = rs("usuario")
                lblComentario.Text = Replace(rs("comentario"), vbCrLf, "<br />")
                '
                If rs("estatusid") = 2 Then
                    btnProcesar.Enabled = False
                    btnProcesar.ToolTip = "Esta transferencia ya ha sido procesada"
                    productslist.Columns(productslist.Columns.Count - 1).Visible = False
                    txtSearchItem.Enabled = False
                    btnSearch.Enabled = False
                    btnSearch.ToolTip = "Esta transferencia ya ha sido procesada"
                    btnAgregar.Enabled = False
                    btnAgregar.ToolTip = "Esta transferencia ya ha sido procesada"
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

    Private Sub MuestraDetalle()
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pTransferencia @cmd=6, @transferenciaid='" & Request("id").ToString & "'")
        productslist.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub Busqueda()
        Dim objdata As New DataControl
        gridResults.DataSource = objdata.FillDataSet("exec pTransferencia @cmd=12, @txtSearch='" & txtSearchItem.Text & "', @transferenciaid='" & Request("id").ToString & "'")
        gridResults.DataBind()
        objdata = Nothing
        txtSearchItem.Focus()
    End Sub

    Protected Sub btnRegresar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegresar.Click
        Response.Redirect("~/portalcfd/almacen/transferencias.aspx")
    End Sub

    Protected Sub btnProcesar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnProcesar.Click
        '
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pTransferencia @cmd=8, @transferenciaid='" & Request("id").ToString & "'")
        ObjData = Nothing
        '
        Response.Redirect("~/portalcfd/almacen/transferencias.aspx")
    End Sub

    Private Sub productslist_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles productslist.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pTransferencia @cmd=4, @transferenciadetalleid='" & e.CommandArgument.ToString & "'")
                ObjData = Nothing
                Response.Redirect("~/portalcfd/almacen/editalote.aspx?id=" & Request("id").ToString)
        End Select
    End Sub

    Private Sub productslist_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles productslist.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un elemento de transferencia. ¿Desea continuar?');")
        End Select
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        lblMensaje.Text = ""
        Call Busqueda()
    End Sub

    Private Sub gridResults_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles gridResults.ItemCommand
        Select e.CommandName
            Case "cmdAdd"
                InsertItem(e.CommandArgument, e.Item)
        End Select
    End Sub

    Protected Sub InsertItem(ByVal productoid As Integer, ByVal item As GridItem)
        '
        ' Instancía objetos del grid
        '
        Dim lblExistencia As Label = DirectCast(item.FindControl("lblExistencia"), Label)
        Dim lblDisponibles As Label = DirectCast(item.FindControl("lblDisponibles"), Label)
        Dim txtCantidad As RadNumericTextBox = DirectCast(item.FindControl("txtCantidad"), RadNumericTextBox)
        Dim cantidad As Decimal = 0
        If txtCantidad.Text = "" Then
            cantidad = 0
        Else
            cantidad = txtCantidad.Text
        End If
        If cantidad <= Convert.ToDecimal(lblDisponibles.Text) Then
            If cantidad > 0 Then
                '
                '   Agrega la partida
                '
                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pTransferencia @cmd=3, @transferenciaid='" & Request("id").ToString & "', @productoid='" & productoid.ToString & "', @cantidad='" & txtCantidad.Text & "'")
                ObjData = Nothing
            Else
                lblMensaje.Text = "Debes proporcionar la cantidad a transferir."
            End If
            '
        Else
            lblMensaje.Text = "La cantidad proporcionada es mayor a la disponibilidad actual para este producto."
        End If
        Call MuestraDetalle()
        Call Busqueda()
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        '
        Dim productoid As Long = 0
        Dim codigo As String = ""
        Dim mensaje As String = ""
        '
        For Each row As GridDataItem In gridResults.MasterTableView.Items

            productoid = row.GetDataKeyValue("productoid")
            codigo = row.GetDataKeyValue("codigo")

            Dim lblExistencia As Label = DirectCast(row.FindControl("lblExistencia"), Label)
            Dim lblDisponibles As Label = DirectCast(row.FindControl("lblDisponibles"), Label)
            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
            Dim cantidad As Decimal = 0
            If txtCantidad.Text = "" Then
                cantidad = 0
            Else
                cantidad = txtCantidad.Text
            End If
            If cantidad <= Convert.ToDecimal(lblDisponibles.Text) Then
                If cantidad > 0 Then
                    '
                    '   Agrega la partida
                    '
                    Dim ObjData As New DataControl
                    ObjData.RunSQLQuery("exec pTransferencia @cmd=3, @transferenciaid='" & Request("id").ToString & "', @productoid='" & productoid.ToString & "', @cantidad='" & txtCantidad.Text & "'")
                    ObjData = Nothing
                End If
            Else
                'lblMensaje.Text = "La cantidad proporcionada es mayor a la disponibilidad actual para este producto: " & codigo
                mensaje += "La cantidad proporcionada es mayor a la disponibilidad actual para este producto: " & codigo & "</br>"
            End If
        Next

        lblMensaje.Text = mensaje
        Call MuestraDetalle()
        Call Busqueda()

    End Sub

    'Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
    '    Dim ObjData As New DataControl
    '    ObjData.RunSQLQuery("exec pTransferencia @cmd=3, @transferenciaid='" & Request("id").ToString & "', @productoid='" & productoid.SelectedValue.ToString & "', @cantidad='" & txtCantidad.Text & "'")
    '    ObjData = Nothing
    '    txtCantidad.Text = ""
    '    productoid.SelectedIndex = 0
    '    Call MuestraDetalle()
    'End Sub

End Class