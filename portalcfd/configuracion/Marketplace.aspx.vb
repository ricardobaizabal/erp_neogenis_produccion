Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class Marketplace
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(cmbPrioridades, "EXEC pPrioridades @cmd=2", 0)
            ObjCat = Nothing
        End If
    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        panelRegistration.Visible = True
        InsertOrUpdate.Value = 0
        txtNombre.Text = ""
        cmbPrioridades.SelectedValue = 0
        btnGuardar.Text = "Guardar"
    End Sub

    Private Sub MarketplaceList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles MarketplaceList.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                EditMarketplace(e.CommandArgument)

            Case "cmdDelete"
                DeleteUnidad(e.CommandArgument)

        End Select
    End Sub
    Private Sub MarketplaceList_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles MarketplaceList.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Prioridades" Then
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('Va a borrar este Registro. ¿Desea continuar?');")
            End If
        End If
    End Sub
    Private Sub MarketplaceList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles MarketplaceList.NeedDataSource
        MarketplaceList.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
        Call MarketplaceList_NeedData("off")
    End Sub
    Public Sub MarketplaceList_NeedData(ByVal state As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMarketPlace @cmd=2", conn)

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
    Private Sub EditMarketplace(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try
            Dim cmd As New SqlCommand("EXEC pMarketPlace @cmd=5, @id=" & id, conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()
            If rs.Read Then
                txtNombre.Text = rs("nombre")
                panelRegistration.Visible = True
                btnGuardar.Text = "Actualizar"
                InsertOrUpdate.Value = 1
                MarcaID.Value = id
                Try
                    cmbPrioridades.SelectedValue = rs("prioridadid")
                Catch ex As Exception
                    cmbPrioridades.SelectedValue = 0
                End Try
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally

            conn.Close()
            conn.Dispose()
        End Try
    End Sub
    Private Sub DeleteUnidad(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try
            Dim cmd As New SqlCommand("EXEC pMarketPlace @cmd=4, @id=" & id.ToString, conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()
            rs.Close()
            conn.Close()
            panelRegistration.Visible = False
            Call MarketplaceList_NeedData("on")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            If InsertOrUpdate.Value = 0 Then

                Dim cmd As New SqlCommand("EXEC pMarketPlace @cmd=1, @nombre='" & txtNombre.Text & "', @prioridadid='" & cmbPrioridades.SelectedValue & "'", conn)
                conn.Open()
                cmd.ExecuteReader()
                panelRegistration.Visible = False

                MarketplaceList.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                Call MarketplaceList_NeedData("on")

                conn.Close()
                conn.Dispose()

            Else
                Dim cmd As New SqlCommand("EXEC pMarketPlace @cmd=3, @nombre='" & txtNombre.Text & "', @prioridadid='" & cmbPrioridades.SelectedValue & "', @id=" & MarcaID.Value.ToString, conn)
                conn.Open()
                cmd.ExecuteReader()
                panelRegistration.Visible = False
                MarketplaceList.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                Call MarketplaceList_NeedData("on")
                conn.Close()
                conn.Dispose()
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()

        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        btnGuardar.Text = "btnGuardar"
        txtNombre.Text = ""
        panelRegistration.Visible = False
    End Sub


End Class