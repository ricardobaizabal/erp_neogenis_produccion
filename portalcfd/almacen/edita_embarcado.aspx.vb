Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI

Public Class edita_embarcado
    Inherits System.Web.UI.Page
    Dim listMensajes As New List(Of String)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            MuestraDetalle()
            DisplayItems()
        End If
    End Sub

    Private Sub DisplayItems()
        Dim ds As DataSet
        Dim ObjData As New DataControl
        itemsList.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
        ds = ObjData.FillDataSet("EXEC pCFD @cmd=3, @cfdid='" & Request("id").ToString & "'")
        itemsList.DataSource = ds
        itemsList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub MuestraDetalle()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCFD @cmd=18, @cfdid='" & Request("id").ToString & "'", conn)
        Dim clienteid As Long = 0
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read() Then
                lblCliente.Text = rs("razonsocial")
                lblRFC.Text = rs("rfc_cliente")
                lblSerie.Text = rs("serie")
                lblFolio.Text = rs("folio")
            End If

            rs.Close()

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Redirect(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        ''
    End Sub

    Protected Sub itemsList_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles itemsList.ItemCommand
        Select Case e.CommandName
            Case "cmdView"
                Dim commandArgs As String() = e.CommandArgument.ToString().Split(New Char() {","c})
                Dim productoid As String = commandArgs(0)
                Dim cantidad As String = commandArgs(1)
                totalPiezasPartida.Value = commandArgs(1)
                partidaid.Value = commandArgs(2)
                pedidoID.Value = commandArgs(3)

                lblCantidad.Text = "Cantidad solicitada: <strong>" + cantidad + "</strong>"
                Dim ds As DataSet
                Dim ObjData As New DataControl
                itemsInventoryList.MasterTableView.NoMasterRecordsText = Resources.Resource.ItemsEmptyGridMessage
                ds = ObjData.FillDataSet("EXEC pEmbarques @cmd=2, @productoid='" & productoid & "'")
                itemsInventoryList.DataSource = ds
                itemsInventoryList.DataBind()
                ObjData = Nothing
                RadWindow1.VisibleOnPageLoad = True
        End Select
    End Sub

    Sub txtCantidad_TextChanged(sender As Object, e As EventArgs)
        Dim txt = DirectCast(sender, RadNumericTextBox)
        Dim cell = DirectCast(txt.Parent, GridTableCell)
        Dim item As GridDataItem = cell.Item
        Dim index As Integer = item.ItemIndex
        Dim message As String = ""
        Dim total As Decimal = 0
        For Each dataItem As Telerik.Web.UI.GridDataItem In itemsInventoryList.MasterTableView.Items
            Dim inventarioId As String = dataItem.GetDataKeyValue("id").ToString
            Dim existencia As Decimal = dataItem.GetDataKeyValue("existencia")
            Dim txtCantidad As RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), RadNumericTextBox)
            If txtCantidad.Text = "" Then
                txtCantidad.Text = 0
            End If
            total += Convert.ToDecimal(txtCantidad.Text)
            'lblTotalDetalle.Text = "Total: " & total.ToString
            totalPiezasProcesadas.Value = total
            If dataItem.ItemIndex = index Then
                If Convert.ToDecimal(txtCantidad.Text) > existencia Then
                    listMensajes.Add("- La cantidad proporcionada es mayor a la existencia.")
                    message = String.Join(Environment.NewLine, listMensajes.ToArray())
                End If
            End If
        Next
        lblMensaje.Text = message
    End Sub

    Private Sub btnGuardarDetalle_Click(sender As Object, e As EventArgs) Handles btnGuardarDetalle.Click
        Dim message As String = ""
        Dim total As Decimal = 0
        For Each dataItem As Telerik.Web.UI.GridDataItem In itemsInventoryList.MasterTableView.Items
            Dim inventarioId As String = dataItem.GetDataKeyValue("id").ToString
            Dim existencia As Decimal = dataItem.GetDataKeyValue("existencia")
            Dim txtCantidad As RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), RadNumericTextBox)

            If Convert.ToDecimal(txtCantidad.Text) > existencia Then
                listMensajes.Add("- La cantidad procesada con el Folio: " & inventarioId & " es mayor a la existencia.")
            End If
            If txtCantidad.Text = "" Then
                txtCantidad.Text = 0
            End If
            total += Convert.ToDecimal(txtCantidad.Text)
            totalPiezasProcesadas.Value = total
        Next
        message = String.Join(Environment.NewLine, listMensajes.ToArray())
        If message = "" Then
            If totalPiezasPartida.Value = totalPiezasProcesadas.Value Then
                For Each dataItem As Telerik.Web.UI.GridDataItem In itemsInventoryList.MasterTableView.Items
                    Dim inventarioId As String = dataItem.GetDataKeyValue("id").ToString
                    Dim productoId As String = dataItem.GetDataKeyValue("productoid").ToString
                    Dim existencia As Decimal = dataItem.GetDataKeyValue("existencia")
                    Dim txtCantidad As RadNumericTextBox = DirectCast(dataItem.FindControl("txtCantidad"), RadNumericTextBox)

                    Dim ObjData As New DataControl
                    ObjData.RunSQLQuery("exec pEmbarques @cmd=3, @cfdid='" & Request("id").ToString & "', @partidaid='" & partidaid.Value.ToString & "', @productoid='" & productoId.ToString & "', @cantidad='" & txtCantidad.Text.ToString & "', @inventarioid='" & inventarioId.ToString & "'")
                    ObjData = Nothing

                    Call DisplayItems()
                    Call ValidaFacturar()
                    'lblMensaje.ForeColor = Drawing.Color.Green
                    'lblMensaje.Text = "Cantidad procesada con éxito."
                    RadWindow1.VisibleOnPageLoad = False
                Next
            Else
                listMensajes.Add("- El total de productos procesados no es igual al total del registro de la factura.")
                message = String.Join(Environment.NewLine, listMensajes.ToArray())
                lblMensaje.ForeColor = Drawing.Color.Red
                lblMensaje.Text = message
            End If
        Else
            lblMensaje.ForeColor = Drawing.Color.Red
            lblMensaje.Text = message
        End If
    End Sub

    Private Sub DescargaInventario()
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pDescargaInventario @userid='" & Session("userid").ToString & "', @cfdid='" & Request("id").ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub ValidaFacturar()
        Dim partidas_pendientes As Decimal = 0

        Dim ObjData As New DataControl
        partidas_pendientes = ObjData.RunSQLScalarQueryDecimal("exec pEmbarques @cmd=4, @cfdid='" & Request("id").ToString & "'")
        ObjData = Nothing

        If partidas_pendientes <= 0 Then
            Call DescargaInventario()
            Call ActualizaEstatusEmbarcado()
            If pedidoID.Value > 0 Then
                Call DesapartaProductos()
            End If
            Response.Redirect("~/portalcfd/almacen/embarcado.aspx")
        End If

    End Sub

    Private Sub ActualizaEstatusEmbarcado()
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pEmbarques @cmd=5, @cfdid='" & Request("id").ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub DesapartaProductos()
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pSeparaProductos @cmd=2, @cfdid='" & Request("id").ToString & "'")
        ObjData = Nothing
    End Sub

End Class