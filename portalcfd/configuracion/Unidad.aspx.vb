Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Unidad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then
            Dim objCat As New DataControl
            objCat.Catalogo(claveUnidad, "select codigo, codigo + ' - ' + nombre as descripcion from tblClaveUnidad order by nombre", 0)
            objCat = Nothing

            btnGuardar.Text = Resources.Resource.btnSave
            btnCancelar.Text = Resources.Resource.btnCancel
        End If
    End Sub
    Private Sub Unidadlist_ItemCommand(ByVal sender As Object, ByVal e As GridCommandEventArgs) Handles Unidadlist.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                EditUnidad(e.CommandArgument)

            Case "cmdDelete"
                DeleteUnidad(e.CommandArgument)

        End Select
    End Sub
    Private Sub EditUnidad(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCatalogoUnidad @cmd=3, @id=" & id, conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                txtNombre.Text = rs("nombre")

                Dim objCat As New DataControl
                objCat.Catalogo(claveUnidad, "select codigo, codigo + ' - ' + nombre as descripcion from tblClaveUnidad order by nombre", rs("clave"))
                objCat = Nothing

                panelRegistration.Visible = True
                btnGuardar.Text = "Actualizar"
                InsertOrUpdate.Value = 1
                UnidadID.Value = id

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

            Dim cmd As New SqlCommand("EXEC pCatalogoUnidad @cmd=4, @id=" & id.ToString, conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelRegistration.Visible = False

            Unidadlist.DataSource = GetUnidad()
            Unidadlist.DataBind()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub
    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        InsertOrUpdate.Value = 0

        txtNombre.Text = ""
        btnGuardar.Text = "Guardar"
        panelRegistration.Visible = True
        claveUnidad.SelectedValue = 0
    End Sub
    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            If InsertOrUpdate.Value = 0 Then

                Dim cmd As New SqlCommand("EXEC pCatalogoUnidad @cmd=1, @nombre='" & txtNombre.Text & "',@clave='" & claveUnidad.SelectedValue & "'", conn)
                conn.Open()

                cmd.ExecuteReader()

                panelRegistration.Visible = False

                Unidadlist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                Unidadlist.DataSource = GetUnidad()
                Unidadlist.DataBind()

                conn.Close()
                conn.Dispose()

            Else
                Dim cmd As New SqlCommand("EXEC pCatalogoUnidad @cmd=5, @nombre='" & txtNombre.Text & "', @clave='" & claveUnidad.SelectedValue & "', @id=" & UnidadID.Value.ToString, conn)

                conn.Open()

                cmd.ExecuteReader()

                panelRegistration.Visible = False

                Unidadlist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                Unidadlist.DataSource = GetUnidad()
                Unidadlist.DataBind()

                conn.Close()
                conn.Dispose()

            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try
    End Sub
    Function GetUnidad() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCatalogoUnidad @cmd=6", conn)

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
    Private Sub Unidadlist_NeedDataSource(ByVal sender As Object, ByVal e As GridNeedDataSourceEventArgs) Handles Unidadlist.NeedDataSource
        If Not e.IsFromDetailTable Then
            Unidadlist.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
            Unidadlist.DataSource = GetUnidad()
        End If
    End Sub
    Private Sub Unidadlist_ItemDataBound(ByVal sender As Object, ByVal e As GridItemEventArgs) Handles Unidadlist.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Unidad" Then
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('Va a borrar este Registro. ¿Desea continuar?');")
            End If
        End If
    End Sub
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        btnGuardar.Text = "btnGuardar"
        txtNombre.Text = ""
        panelRegistration.Visible = False
    End Sub

End Class