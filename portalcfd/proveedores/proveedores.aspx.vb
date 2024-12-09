Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Partial Class portalcfd_proveedores_proveedores
    Inherits System.Web.UI.Page


#Region "Load Initial Values"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then

            '''''''''''''''''''
            'Fieldsets Legends'
            '''''''''''''''''''

            lblClientsListLegend.Text = "Listado de Proveedores"
            lblClientEditLegend.Text = "Agregar/Editar Proveedor"

            '''''''''''''''''''''''''''''''''
            'Combobox Values & Empty Message'
            '''''''''''''''''''''''''''''''''

            Dim TelerikRadComboBox As New FillRadComboBox
            TelerikRadComboBox.FillRadComboBox(cmbStates, "EXEC pCatalogos @cmd=1")

            cmbStates.Text = Resources.Resource.cmbEmptyMessage

            ''''''''''''''
            'Label Titles'
            ''''''''''''''

            lblSocialReason.Text = Resources.Resource.lblSocialReason
            lblContact.Text = Resources.Resource.lblContact
            lblContactEmail.Text = Resources.Resource.lblContactEmail
            lblContactPhone.Text = Resources.Resource.lblContactPhone
            lblStreet.Text = Resources.Resource.lblStreet
            lblExtNumber.Text = Resources.Resource.lblExtNumber
            lblIntNumber.Text = Resources.Resource.lblIntNumber
            lblColony.Text = Resources.Resource.lblColony
            lblCountry.Text = Resources.Resource.lblCountry
            lblState.Text = Resources.Resource.lblState
            lblTownship.Text = Resources.Resource.lblTownship
            lblZipCode.Text = Resources.Resource.lblZipCode
            lblRFC.Text = Resources.Resource.lblRFC

            '''''''''''''''''''
            'Validators Titles'
            '''''''''''''''''''

            RequiredFieldValidator1.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator2.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator3.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator4.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator5.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator6.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator7.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator8.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator9.Text = Resources.Resource.validatorMessage

            RegularExpressionValidator1.Text = Resources.Resource.validatorMessageEmail

            ''''''''''''''''
            'Buttons Titles'
            ''''''''''''''''

            btnAddProvider.Text = "Agregar Proveedor"
            btnSaveClient.Text = Resources.Resource.btnSave
            btnCancel.Text = Resources.Resource.btnCancel
            '
            '
            '
            Dim ObjData As New DataControl
            ObjData.Catalogo(condicionesid, "select id, nombre from tblCondiciones", 0)
            ObjData = Nothing

        End If

    End Sub

#End Region

#Region "Load List Of Providers"

    Function GetProviders() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMisProveedores  @cmd=1", conn)

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

#Region "Telerik Grid Providers Loading Events"

    Protected Sub providerslist_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles providerslist.NeedDataSource

        If Not e.IsFromDetailTable Then

            providerslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
            providerslist.DataSource = GetProviders()

        End If

    End Sub

#End Region

#Region "Telerik Grid Language Modification(Spanish)"

    Protected Sub clientslist_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles providerslist.Init

        providerslist.PagerStyle.NextPagesToolTip = "Ver mas"
        providerslist.PagerStyle.NextPageToolTip = "Siguiente"
        providerslist.PagerStyle.PrevPagesToolTip = "Ver más"
        providerslist.PagerStyle.PrevPageToolTip = "Atrás"
        providerslist.PagerStyle.LastPageToolTip = "Última Página"
        providerslist.PagerStyle.FirstPageToolTip = "Primera Página"
        providerslist.PagerStyle.PagerTextFormat = "{4}    Página {0} de {1}, Registros {2} al {3} de {5}"
        providerslist.SortingSettings.SortToolTip = "Ordernar"
        providerslist.SortingSettings.SortedAscToolTip = "Ordenar Asc"
        providerslist.SortingSettings.SortedDescToolTip = "Ordenar Desc"


        Dim menu As Telerik.Web.UI.GridFilterMenu = providerslist.FilterMenu
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

        providerslist.GroupingSettings.CaseSensitive = False

        Dim Menu As Telerik.Web.UI.GridFilterMenu = providerslist.FilterMenu
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

#Region "Telerik Grid Providers Editing & Deleting Events"

    Protected Sub providerslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles providerslist.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Providers" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('" & Resources.Resource.ProviderDeleteConfirmationMessage & "');")

            End If

        End If

    End Sub

    Protected Sub providerslist_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles providerslist.ItemCommand

        Select Case e.CommandName

            Case "cmdEdit"
                EditProvider(e.CommandArgument)

            Case "cmdDelete"
                DeleteProvider(e.CommandArgument)

        End Select

    End Sub

    Private Sub DeleteProvider(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pMisProveedores @cmd='3', @proveedorId ='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelClientRegistration.Visible = False

            providerslist.DataSource = GetProviders()
            providerslist.DataBind()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub EditProvider(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pMisProveedores @cmd='2', @proveedorId='" & id & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                txtSocialReason.Text = rs("razonsocial")
                txtContact.Text = rs("contacto")
                txtContactEmail.Text = rs("email_contacto")
                txtContactPhone.Text = rs("telefono_contacto")
                txtStreet.Text = rs("fac_calle")
                txtExtNumber.Text = rs("fac_num_ext")
                txtIntNumber.Text = rs("fac_num_int")
                txtColony.Text = rs("fac_colonia")
                txtCountry.Text = rs("fac_pais")
                cmbStates.SelectedValue = rs("fac_estadoid")
                txtTownship.Text = rs("fac_municipio")
                txtZipCode.Text = rs("fac_cp")
                txtRFC.Text = rs("rfc")
                '
                Dim ObjData As New DataControl
                ObjData.Catalogo(condicionesid, "select id, nombre from tblCondiciones", rs("condicionesid"))
                ObjData = Nothing


                panelClientRegistration.Visible = True

                InsertOrUpdate.Value = 1
                ClientsID.Value = id

            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

#End Region

#Region "Telerik Grid Providers Column Names (From Resource File)"

    Protected Sub providerslist_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles providerslist.ItemCreated

        If TypeOf e.Item Is GridHeaderItem Then

            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)

            If e.Item.OwnerTableView.Name = "Providers" Then

                header("razonsocial").Text = Resources.Resource.gridColumnNameSocialReason
                header("contacto").Text = Resources.Resource.gridColumnNameContact
                header("telefono_contacto").Text = Resources.Resource.gridColumnNameContactPhone
                header("rfc").Text = Resources.Resource.gridColumnNameRFC

            End If

        End If

    End Sub

#End Region

#Region "Display Provider Data Panel"

    Protected Sub btnAddProvider_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddProvider.Click

        InsertOrUpdate.Value = 0

        txtSocialReason.Text = ""
        txtContact.Text = ""
        txtContactEmail.Text = ""
        txtContactPhone.Text = ""
        txtStreet.Text = ""
        txtExtNumber.Text = ""
        txtIntNumber.Text = ""
        txtColony.Text = ""
        txtCountry.Text = ""
        txtTownship.Text = ""
        txtZipCode.Text = ""

        Dim TelerikRadComboBox As New FillRadComboBox
        TelerikRadComboBox.FillRadComboBox(cmbStates, "EXEC pCatalogos @cmd=1")

        cmbStates.Text = Resources.Resource.cmbEmptyMessage

        txtRFC.Text = ""

        panelClientRegistration.Visible = True

    End Sub

#End Region

#Region "Save Provider"

    Protected Sub btnSaveClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveClient.Click

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            If InsertOrUpdate.Value = 0 Then

                Dim cmd As New SqlCommand("EXEC pMisProveedores @cmd=4, @razonsocial='" & txtSocialReason.Text & "', @contacto='" & txtContact.Text & "', @email_contacto='" & txtContactEmail.Text & "', @telefono_contacto='" & txtContactPhone.Text & "', @fac_calle='" & txtStreet.Text & "', @fac_num_int='" & txtIntNumber.Text & "', @fac_num_ext='" & txtExtNumber.Text & "', @fac_colonia='" & txtColony.Text & "', @fac_pais='" & txtCountry.Text & "', @fac_municipio='" & txtTownship.Text & "', @fac_estadoid='" & cmbStates.SelectedValue & "', @fac_cp='" & txtZipCode.Text & "', @fac_rfc='" & txtRFC.Text & "', @condicionesid='" & condicionesid.SelectedValue.ToString & "'", conn)

                conn.Open()

                cmd.ExecuteReader()

                panelClientRegistration.Visible = False

                providerslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                providerslist.DataSource = GetProviders()
                providerslist.DataBind()

                conn.Close()
                conn.Dispose()

            Else

                Dim cmd As New SqlCommand("EXEC pMisProveedores @cmd=5, @proveedorId='" & ClientsID.Value & "', @razonsocial='" & txtSocialReason.Text & "', @contacto='" & txtContact.Text & "', @email_contacto='" & txtContactEmail.Text & "', @telefono_contacto='" & txtContactPhone.Text & "', @fac_calle='" & txtStreet.Text & "', @fac_num_int='" & txtIntNumber.Text & "', @fac_num_ext='" & txtExtNumber.Text & "', @fac_colonia='" & txtColony.Text & "',  @fac_pais='" & txtCountry.Text & "', @fac_municipio='" & txtTownship.Text & "', @fac_estadoid='" & cmbStates.SelectedValue & "', @fac_cp='" & txtZipCode.Text & "', @fac_rfc='" & txtRFC.Text & "', @condicionesid='" & condicionesid.SelectedValue.ToString & "'", conn)

                conn.Open()

                cmd.ExecuteReader()

                panelClientRegistration.Visible = False

                providerslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                providerslist.DataSource = GetProviders()
                providerslist.DataBind()

                conn.Close()
                conn.Dispose()

            End If

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

#End Region

#Region "Cancel Provider (Save/Edit)"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        InsertOrUpdate.Value = 0

        txtSocialReason.Text = ""
        txtContact.Text = ""
        txtContactEmail.Text = ""
        txtContactPhone.Text = ""
        txtStreet.Text = ""
        txtExtNumber.Text = ""
        txtIntNumber.Text = ""
        txtColony.Text = ""
        txtCountry.Text = ""
        txtTownship.Text = ""
        txtZipCode.Text = ""

        Dim TelerikRadComboBox As New FillRadComboBox
        TelerikRadComboBox.FillRadComboBox(cmbStates, "EXEC pCatalogos @cmd=1")

        cmbStates.Text = Resources.Resource.cmbEmptyMessage

        txtRFC.Text = ""

        panelClientRegistration.Visible = False

    End Sub

#End Region
End Class
