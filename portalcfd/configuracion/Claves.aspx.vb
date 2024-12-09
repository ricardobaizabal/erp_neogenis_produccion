Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Claves
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then
        End If
    End Sub

    Function GetProductos() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCatalogoProductos @cmd=6", conn)

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

    Private Sub itemsList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles itemsList.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Dim objdata As New DataControl
                objdata.RunSQLQuery("EXEC pCatalogoProductos @cmd=4, @id='" & e.CommandArgument & "'")
                objdata = Nothing
                DisplayItems()
        End Select
    End Sub

    Private Sub DisplayItems()
        Dim ObjData As New DataControl
        itemsList.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        itemsList.DataSource = GetProductos()
        itemsList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub gridResults_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles gridResults.ItemCommand
        Select Case e.CommandName
            Case "cmdAdd"
                InsertItem(e.CommandArgument)
                DisplayItems()
                gridResults.Visible = False
                itemsList.Visible = True
                txtSearchItem.Text = ""
                txtSearchItem.Focus()
                btnCancelSearch.Visible = False
        End Select
    End Sub

    Protected Sub InsertItem(ByVal codigo As String)
        '
        '   Agrega la partida
        '
        Dim objdata As New DataControl
        objdata.RunSQLQuery("EXEC pCatalogoProductos @cmd=1, @clave='" & codigo.ToString & "'")
        objdata = Nothing
        '
    End Sub

    Private Sub txtSearchItem_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearchItem.TextChanged
        gridResults.Visible = True
        itemsList.Visible = False
        Dim objdata As New DataControl
        gridResults.DataSource = objdata.FillDataSet("exec pCatalogoProductos @cmd=5, @txtSearch='" & txtSearchItem.Text & "'")
        gridResults.DataBind()
        objdata = Nothing
        txtSearchItem.Text = ""
        txtSearchItem.Focus()
        btnCancelSearch.Visible = True
    End Sub

    Private Sub itemsList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles itemsList.NeedDataSource
        itemsList.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        itemsList.DataSource = GetProductos()
    End Sub

    Private Sub btnCancelSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelSearch.Click
        gridResults.Visible = False
        itemsList.Visible = True
        txtSearchItem.Text = ""
        txtSearchItem.Focus()
        btnCancelSearch.Visible = False
    End Sub

End Class