Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading

Public Class recibir
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            txtCaducidad.SelectedDate = Date.Today
            Call MuestraDetallesPartida()
            Call CargaCatalogo()
            Call CargaPartidasRecepcion()
        End If
    End Sub

    Private Sub MuestraDetallesPartida()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("exec pOrdenCompra @cmd=11, @conceptoid='" & Request("id").ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                lblCodigo.Text = rs("codigo")
                lblDescripcion.Text = rs("descripcion")
                lblCantidad.Text = rs("cantidad")
                lblPerecederoBit.Text = rs("perecederoBit")

                If lblPerecederoBit.Text = "False" Then
                    txtCaducidad.Enabled = False
                    txtLote.Enabled = False
                    valCaducidad.Enabled = False
                    valLote.Enabled = False
                End If

            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
    End Sub

    Private Sub CargaCatalogo()
        Dim ObjData As New DataControl
        ObjData.Catalogo(almacenid, "select id, nombre from tblAlmacen order by nombre", 0)
        ObjData = Nothing
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As System.EventArgs) Handles btnAdd.Click
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try
            Dim fecha_caducidad As DateTime
            fecha_caducidad = txtCaducidad.SelectedDate

            Dim cmd As New SqlCommand("exec pControlInventario @cmd=1, @ordendetalleid='" & Request("id").ToString & "', @caducidad='" & fecha_caducidad.ToString("yyyyMMdd") & "', @lote='" & txtLote.Text & "', @costo_variable='" & txtCostoVariable.Text & "', @almacenid='" & almacenid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @cantidad='" & txtCantidad.Text & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                lblMensaje.Text = rs("mensaje")
            End If

        Catch ex As Exception

        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
        Call CargaPartidasRecepcion()
        '
    End Sub

    Private Sub CargaPartidasRecepcion()
        Dim ObjData As New DataControl
        conceptosList.DataSource = ObjData.FillDataSet("exec pControlInventario @cmd=2, @ordendetalleid='" & Request("id").ToString & "'")
        conceptosList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub conceptosList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles conceptosList.ItemCommand
        Select Case e.CommandName
            Case "cmdDel"
                Call EliminaPartida(e.CommandArgument)
                Call CargaPartidasRecepcion()
        End Select
    End Sub

    Private Sub conceptosList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles conceptosList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar una partida de recepción. ¿Desea continuar?');")
        End Select
    End Sub

    Private Sub conceptosList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles conceptosList.NeedDataSource

    End Sub

    Private Sub EliminaPartida(ByVal partidaid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pControlInventario @cmd=3, @inventarioid='" & partidaid.ToString & "'")
        ObjData = Nothing
    End Sub

End Class