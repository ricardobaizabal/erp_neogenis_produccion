Imports System.Data
Imports System.Data.SqlClient
Public Class almacenes
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaAlmacenes()
        End If
    End Sub

    Protected Sub btnSaveWareHouse_Click(sender As Object, e As EventArgs) Handles btnSaveWareHouse.Click
        Dim sql As String = ""
        If almacenid.Value = 0 Then
            sql = "exec pAlmacen @cmd=2, @nombre='" & txtNombre.Text & "'"
        Else
            sql = "exec pAlmacen @cmd=4, @almacenid='" & almacenid.Value.ToString & "', @nombre='" & txtNombre.Text & "'"
            almacenid.Value = 0
            txtNombre.Text = ""
        End If
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery(sql)
        ObjData = Nothing
        panelWareHouseDetail.Visible = False
        Call CargaAlmacenes()
    End Sub

    Protected Sub btnAddWarehouse_Click(sender As Object, e As EventArgs) Handles btnAddWarehouse.Click
        panelWareHouseDetail.Visible = True
    End Sub

    Private Sub CargaAlmacenes()
        Dim ObjData As New DataControl
        almacenesList.DataSource = ObjData.FillDataSet("exec pAlmacen @cmd=1")
        almacenesList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        txtNombre.Text = ""
        panelWareHouseDetail.Visible = False
    End Sub

    Private Sub almacenesList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles almacenesList.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                Call EditaAlmacen(e.CommandArgument)
            Case "cmdDelete"
                Call BorraAlmacen(e.CommandArgument)
        End Select
    End Sub

    Private Sub EditaAlmacen(ByVal id As Integer)
        panelWareHouseDetail.Visible = True
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pAlmacen @cmd=3, @almacenId='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                txtNombre.Text = rs("nombre")
                almacenid.Value = id

            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub BorraAlmacen(ByVal id As Integer)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pAlmacen @cmd=5, @almacenid='" & id.ToString & "'")
        ObjData = Nothing
        Call CargaAlmacenes()
    End Sub

    Private Sub almacenesList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles almacenesList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a borrar un almacén. ¿Desea continuar?');")
        End Select
    End Sub

    Private Sub almacenesList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles almacenesList.NeedDataSource
        Dim ObjData As New DataControl
        almacenesList.DataSource = ObjData.FillDataSet("exec pAlmacen @cmd=1")
        ObjData = Nothing
    End Sub
End Class