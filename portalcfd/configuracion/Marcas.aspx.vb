Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Marcas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then
            btnGuardar.Text = Resources.Resource.btnSave
            btnCancelar.Text = Resources.Resource.btnCancel
        End If

    End Sub

    Private Sub MarcasList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles MarcasList.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                EditMarca(e.CommandArgument)

            Case "cmdDelete"
                DeleteUnidad(e.CommandArgument)

        End Select
    End Sub

    Private Sub MarcasList_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles MarcasList.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Marcas" Then
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('Va a borrar este Registro. ¿Desea continuar?');")
            End If
        End If
    End Sub

    Private Sub MarcasList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles MarcasList.NeedDataSource
        If Not e.IsFromDetailTable Then
            MarcasList.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
            MarcasList.DataSource = GetMarcas()
        End If
    End Sub

    Function GetMarcas() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCatalogoMarcas @cmd=6", conn)

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

    Private Sub EditMarca(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCatalogoMarcas @cmd=3, @id=" & id, conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                txtNombre.Text = rs("nombre")
                panelRegistration.Visible = True
                btnGuardar.Text = "Actualizar"
                InsertOrUpdate.Value = 1
                MarcaID.Value = id
            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub DeleteUnidad(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCatalogoMarcas @cmd=4, @id=" & id.ToString, conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelRegistration.Visible = False

            MarcasList.DataSource = GetMarcas()
            MarcasList.DataBind()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            If InsertOrUpdate.Value = 0 Then

                Dim cmd As New SqlCommand("EXEC pCatalogoMarcas @cmd=1, @nombre='" & txtNombre.Text & "'", conn)
                conn.Open()

                cmd.ExecuteReader()

                panelRegistration.Visible = False

                MarcasList.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                MarcasList.DataSource = GetMarcas()
                MarcasList.DataBind()

                conn.Close()
                conn.Dispose()

            Else
                Dim cmd As New SqlCommand("EXEC pCatalogoMarcas @cmd=5, @nombre='" & txtNombre.Text & "', @id=" & MarcaID.Value.ToString, conn)

                conn.Open()

                cmd.ExecuteReader()

                panelRegistration.Visible = False

                MarcasList.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                MarcasList.DataSource = GetMarcas()
                MarcasList.DataBind()

                conn.Close()
                conn.Dispose()

            End If

        Catch ex As Exception

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

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        InsertOrUpdate.Value = 0
        txtNombre.Text = ""
        btnGuardar.Text = "Guardar"
        panelRegistration.Visible = True
    End Sub

End Class