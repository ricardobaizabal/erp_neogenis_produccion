Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Partial Class portalcfd_usuarios_usuarios
    Inherits System.Web.UI.Page

#Region "Load Initial Values"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If System.Configuration.ConfigurationManager.AppSettings("usuarios") = 0 Then
            Response.Redirect("~/portalcfd/usuarios/informacion.aspx")
        End If
        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then

            '''''''''''''''''''
            'Fieldsets Legends'
            '''''''''''''''''''

            lblUsersListLegend.Text = "Listado de Usuarios"
            lblUserEditLegend.Text = "Agregar/Editar Usuario"

            '''''''''''''''''''''''''''''''''
            'Combobox Values & Empty Message'
            '''''''''''''''''''''''''''''''''


            ''''''''''''''
            'Label Titles'
            ''''''''''''''

            lblNombre.Text = "Nombre:"
            lblSucursal.Text = "Sucursal:"
            lblEmail.Text = "Email:"
            lblContrasena.Text = "Contraseña:"
            lblPerfil.Text = "Perfil:"

            '''''''''''''''''''
            'Validators Titles'
            '''''''''''''''''''

            RequiredFieldValidator1.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator2.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator3.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator4.Text = Resources.Resource.validatorMessage
            RegularExpressionValidator1.Text = Resources.Resource.validatorMessageEmail
            ''''''''''''''''
            'Buttons Titles'
            ''''''''''''''''

            btnAddUser.Text = "Agregar usuario"
            btnSaveUser.Text = Resources.Resource.btnSave
            btnCancel.Text = Resources.Resource.btnCancel
            '
            '
            Dim ObjData As New DataControl
            ObjData.Catalogo(perfilid, "select id, nombre from tblUsuarioPerfil", 0)
            ObjData.Catalogo(sucursalid, "select id, sucursal from tblSucursal where ISNULL(borradoBit,0) = 0 and id <> 4 order by sucursal", 0)
            ObjData.Catalogo(cmbcoordinadorC, " select id, nombre from tblUsuario where isnull(borradobit,0)=0 and perfilid=6 order by nombre", 0)
            ObjData = Nothing
            '

            Call CoordinadorVisible()

        End If

        If Session("perfilid") <> 1 Then
            If Session("userid") <> 13 Or Session("userid") <> 18 Or Session("userid") <> 32 Then
                Response.Redirect("~/portalcfd/Home.aspx")
                'ElseIf Session("userid") <> 8 Then
                '    Response.Redirect("~/portalcfd/Home.aspx")

                'ElseIf Session("userid") <> 32 Then
                '    Response.Redirect("~/portalcfd/Home.aspx")
            End If
        End If

    End Sub

#End Region

#Region "Load List Of Users"

    Function GetUsers() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pUsuarios  @cmd=2", conn)

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

#End Region

#Region "Telerik Grid Users Loading Events"

    Protected Sub userslist_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles userslist.NeedDataSource

        If Not e.IsFromDetailTable Then

            userslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
            userslist.DataSource = GetUsers()

        End If

    End Sub

#End Region

#Region "Telerik Grid Language Modification(Spanish)"

    Protected Sub userslist_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles userslist.Init

        userslist.PagerStyle.NextPagesToolTip = "Ver mas"
        userslist.PagerStyle.NextPageToolTip = "Siguiente"
        userslist.PagerStyle.PrevPagesToolTip = "Ver más"
        userslist.PagerStyle.PrevPageToolTip = "Atrás"
        userslist.PagerStyle.LastPageToolTip = "Última Página"
        userslist.PagerStyle.FirstPageToolTip = "Primera Página"
        userslist.PagerStyle.PagerTextFormat = "{4}    Página {0} de {1}, Registros {2} al {3} de {5}"
        userslist.SortingSettings.SortToolTip = "Ordernar"
        userslist.SortingSettings.SortedAscToolTip = "Ordenar Asc"
        userslist.SortingSettings.SortedDescToolTip = "Ordenar Desc"


        Dim menu As Telerik.Web.UI.GridFilterMenu = userslist.FilterMenu
        Dim i As Integer = 0

        While i < menu.Items.Count

            If menu.Items(i).Text = "NoFilter" Or menu.Items(i).Text = "Contains" Then
                i = i + 1
            Else
                menu.Items.RemoveAt(i)
            End If

        End While

        Call ModificaIdiomaGrid()

    End Sub

    Private Sub ModificaIdiomaGrid()

        userslist.GroupingSettings.CaseSensitive = False

        Dim Menu As Telerik.Web.UI.GridFilterMenu = userslist.FilterMenu
        Dim item As Telerik.Web.UI.RadMenuItem

        For Each item In Menu.Items

            ''''''''''''''''''''''''''''''''''''''''''''''
            'Change The Text For The StartsWith Menu Item'
            ''''''''''''''''''''''''''''''''''''''''''''''

            If item.Text = "StartsWith" Then
                item.Text = "Empieza con"
            End If

            If item.Text = "NoFilter" Then
                item.Text = "Sin Filtro"
            End If

            If item.Text = "Contains" Then
                item.Text = "Contiene"
            End If

            If item.Text = "EndsWith" Then
                item.Text = "Termina con"
            End If

        Next

    End Sub

#End Region

#Region "Telerik Grid Users Editing & Deleting Events"

    Protected Sub userslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles userslist.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Users" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('Va a borrar un usuario. ¿Desea continuar?');")

            End If

        End If

    End Sub

    Protected Sub userslist_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles userslist.ItemCommand

        Select Case e.CommandName

            Case "cmdEdit"
                EditUser(e.CommandArgument)
                Call CoordinadorVisible()

            Case "cmdDelete"
                DeleteUser(e.CommandArgument)

        End Select

    End Sub

    Private Sub DeleteUser(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pUsuarios @cmd='6', @userid ='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelUserRegistration.Visible = False

            userslist.DataSource = GetUsers()
            userslist.DataBind()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub EditUser(ByVal id As Integer)

        Call CoordinadorVisible()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pUsuarios @cmd='4', @userid='" & id & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                txtNombre.Text = rs("nombre")
                txtEmail.Text = rs("email")
                txtContrasena.Text = rs("contrasena")
                perfilid.SelectedValue = rs("perfilid")
                sucursalid.SelectedValue = rs("sucursalid")
                cmbcoordinadorC.SelectedValue = rs("coordinador_Cid")

                panelUserRegistration.Visible = True

                InsertOrUpdate.Value = 1
                UsersID.Value = id

            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

#End Region

#Region "Telerik Grid Users Column Names (From Resource File)"

    Protected Sub clientslist_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles userslist.ItemCreated

        If TypeOf e.Item Is GridHeaderItem Then

            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)

            If e.Item.OwnerTableView.Name = "Users" Then

                header("nombre").Text = "Nombre"
                header("email").Text = "Email"

            End If

        End If

    End Sub

#End Region

#Region "Display User Data Panel"

    Protected Sub btnAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUser.Click

        Call CoordinadorVisible()

        perfilid.SelectedValue = 0
        InsertOrUpdate.Value = 0
        cmbcoordinadorC.SelectedValue = 0
        txtNombre.Text = ""
        txtEmail.Text = ""
        txtContrasena.Text = ""
        panelUserRegistration.Visible = True

    End Sub

#End Region

#Region "Save Client"

    Protected Sub btnSaveUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveUser.Click

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            If InsertOrUpdate.Value = 0 Then

                Dim cmd As New SqlCommand("EXEC pUsuarios @cmd=3, @nombre='" & txtNombre.Text & "', @email='" & txtEmail.Text & "', @contrasena='" & txtContrasena.Text & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @perfilid='" & perfilid.SelectedValue.ToString & "', @coordinadorCid='" & cmbcoordinadorC.SelectedValue.ToString & "'", conn)

                conn.Open()

                cmd.ExecuteReader()

                panelUserRegistration.Visible = False

                userslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                userslist.DataSource = GetUsers()
                userslist.DataBind()

                conn.Close()
                conn.Dispose()

            Else

                Dim cmd As New SqlCommand("EXEC pUsuarios @cmd=5, @nombre='" & txtNombre.Text & "', @email='" & txtEmail.Text & "', @contrasena='" & txtContrasena.Text & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @perfilid='" & perfilid.SelectedValue.ToString & "', @userid='" & UsersID.Value.ToString & "', @coordinadorCid='" & cmbcoordinadorC.SelectedValue.ToString & "'", conn)

                conn.Open()

                cmd.ExecuteReader()

                panelUserRegistration.Visible = False

                userslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                userslist.DataSource = GetUsers()
                userslist.DataBind()

                conn.Close()
                conn.Dispose()

            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing

        End Try

    End Sub

#End Region

#Region "Cancel User (Save/Edit)"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        InsertOrUpdate.Value = 0

        txtNombre.Text = ""
        txtEmail.Text = ""
        txtContrasena.Text = ""
        panelUserRegistration.Visible = False

    End Sub

#End Region

    Private Sub perfilid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles perfilid.SelectedIndexChanged

        Call CoordinadorVisible()

    End Sub

    Private Sub CoordinadorVisible()

        If perfilid.SelectedValue = 3 Then
            lblCoordinaroCuenta.Visible = True
            cmbcoordinadorC.Visible = True
        Else
            lblCoordinaroCuenta.Visible = False
            cmbcoordinadorC.Visible = False
        End If

    End Sub

End Class
