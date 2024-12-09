Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class DisponibilidadPorMarketplaces
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim ObjCat As New DataControl
            ObjCat.Catalogo(cmbMarketplaces, "EXEC pMarketPlace @cmd=2", 0)
            ObjCat.Catalogo(cmbMarca, "select id, nombre from tblProyecto order by nombre", 0)
            ObjCat = Nothing
        End If
    End Sub



    Private Sub MarketplaceList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles MarketplaceList.NeedDataSource
        MarketplaceList.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        Call MarketplaceList_NeedData("off")
    End Sub
    Public Sub MarketplaceList_NeedData(ByVal state As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMarketPlace @cmd=6,@id=" & cmbMarketplaces.SelectedValue & ",@marcaid=" & cmbMarca.SelectedValue & ", @sku='" & txtsku.Text & "'", conn)

        Dim ds As DataSet = New DataSet
        Try
            conn.Open()
            cmd.Fill(ds)
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        MarketplaceList.DataSource = ds
        If state = "on" Then
            MarketplaceList.DataBind()
        End If
    End Sub

    Private Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
        Call MarketplaceList_NeedData("on")
    End Sub

End Class