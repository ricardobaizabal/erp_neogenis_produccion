Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Partial Class portalcfd_almacen_kardex
    Inherits System.Web.UI.Page
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
            Case "cmdView"
                Call MuestraKardex(e.CommandArgument)
        End Select
    End Sub

    Private Sub MuestraKardex(ByVal productoid As Long)
        Dim ObjData As New DataControl
        productslist.DataSource = ObjData.FillDataSet("exec pInventario @cmd=8, @productoid='" & productoid.ToString & "'")
        productslist.DataBind()
        ObjData = Nothing
    End Sub

End Class