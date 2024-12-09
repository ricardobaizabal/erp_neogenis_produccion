Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class portalcfd_Clientes
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

            lblClientsListLegend.Text = Resources.Resource.lblClientsListLegend

            '''''''''''''''''''''''''''''''''
            'Combobox Values & Empty Message'
            '''''''''''''''''''''''''''''''''

            ''''''''''''''
            'Label Titles'
            ''''''''''''''

            lblSocialReason.Text = Resources.Resource.lblSocialReason
            lblContact.Text = "Contacto de compras:"
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
            lblFormaPago.Text = "Forma de pago:"
            lblNumCtaPago.Text = Resources.Resource.lblNumCtaPago
            lblRegimen.Text = Resources.Resource.lblRegimen

            lblEnvioCalle.Text = Resources.Resource.lblEnvioCalle
            lblEnvioColonia.Text = Resources.Resource.lblEnvioColonia
            lblEnvioCP.Text = Resources.Resource.lblEnvioCP
            lblEnvioEmail.Text = Resources.Resource.lblEnvioEmail
            lblEnvioEstado.Text = Resources.Resource.lblEnvioEstado
            lblEnvioMunicipio.Text = Resources.Resource.lblEnvioMunicipio
            lblEnvioNoExt.Text = Resources.Resource.lblEnvioNoExt
            lblEnvioNoInt.Text = Resources.Resource.lblEnvioNoInt
            lblEnvioNombre.Text = Resources.Resource.lblContact

            RegularExpressionValidator1.Text = Resources.Resource.validatorMessageEmail
            RegularExpressionValidator2.Text = Resources.Resource.validatorMessageEmail
            valRFC.Text = Resources.Resource.validatorMessageRfc

            ''''''''''''''''
            'Buttons Titles'
            ''''''''''''''''

            btnAddClient.Text = Resources.Resource.btnAddClient
            btnSaveClient.Text = Resources.Resource.btnSave
            btnCancel.Text = Resources.Resource.btnCancel

            Dim ObjData As New DataControl
            ObjData.Catalogo(tipoprecioid, "select id, nombre from tblTipoPrecio", 0)
            ObjData.Catalogo(condicionesid, "select id, nombre from tblCondiciones", 0)
            ObjData.Catalogo(tipoContribuyenteid, "select id, nombre from tblTipoContribuyente", 0)
            Call SetCmbRegFiscal()
            ObjData.CatalogoStr(formapagoid, "select id, id + ' - ' + nombre as descripcion from tblFormaPago order by nombre", 0)
            ObjData.Catalogo(dropEstado, "select id, nombre from tblEstado order by nombre", 0)
            ObjData.Catalogo(dropEnvEstado, "select id, nombre from tblEstado order by nombre", 0)
            ObjData.Catalogo(ejecutivoid, "select id, nombre from tblUsuario where isnull(borradobit,0)=0 order by nombre", 0)
            'ObjData.Catalogo(ejecutivoid, "select id, nombre from tblUsuario where isnull(borradobit,0)=0 and perfilid=3 or perfilid=5 order by nombre", 0)
            ObjData.Catalogo(cmbCoordinadorC, "select id, nombre from tblUsuario where isnull(borradobit,0)=0 order by nombre", 0)
            ObjData.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradobit,0)=0 and perfilid=3 or perfilid=5 order by nombre", 0)
            ObjData.Catalogo(estatusid, "select id, nombre from tblEstatusCliente order by nombre", 0)
            ObjData.Catalogo(estatusclienteid, "select id, nombre from tblEstatusCliente order by nombre", 1)
            ObjData.Catalogo(cuentaconcentradoraid, "select id, nombre from tblCuentasConcentradoras where isnull(borradorbit,0)=0 order by nombre", 0)
            ObjData.Catalogo(cmbMetodoPago, "select id, id + ' - ' + nombre from tblMetodoPago order by nombre", "PPD")
            ObjData.Catalogo(cmbUsoCFD, "select codigo, codigo + ' - ' + descripcion as nombre from tblUsoCFDI order by descripcion", 1)


            ObjData = Nothing

            If Session("perfilid") <> 1 Then
                vendedorid.SelectedValue = Session("userid")
                vendedorid.Enabled = False
                btnAddClient.Visible = False
                btnGuardarSucursal.Visible = False
                btnSaveClient.Enabled = False
            End If

            If Session("userid") = 24 Then
                vendedorid.Enabled = True
                btnAddClient.Visible = True
                btnGuardarSucursal.Visible = True
                btnSaveClient.Enabled = True
            End If

            If Session("userid") = 27 Then
                vendedorid.Enabled = True
                btnAddClient.Visible = True
                btnGuardarSucursal.Visible = True
                btnSaveClient.Enabled = True
            End If

            If Session("userid") = 38 Then
                vendedorid.Enabled = True
                btnAddClient.Visible = True
                btnGuardarSucursal.Visible = True
                btnSaveClient.Enabled = True
            End If

        End If

    End Sub

#End Region

#Region "Load List Of Clients"
    Private Sub SetCmbRegFiscal(Optional ByVal contribuyenteid As Integer = 0, Optional ByVal sel As Integer = 0)
        Dim ObjData As New DataControl
        Dim sqlwhere As String
        Select Case tipoContribuyenteid.SelectedValue
            Case 1
                sqlwhere = "where fisica='Sí' "
            Case 2
                sqlwhere = "where moral='Sí' "
            Case Else
                sqlwhere = ""
        End Select
        ObjData.Catalogo(regimenid, "select id, id + ' - ' + nombre as descripcion " & "from tblRegimenFiscal " & sqlwhere & " order by nombre ", sel)
        ObjData = Nothing
    End Sub
    Function GetClients() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMisClientes @cmd=1, @clienteUnionId='" & Session("clienteid") & "', @estatusid='" & estatusclienteid.SelectedValue.ToString() & "', @texto='" & txtPalabraClave.Text.ToString & "'", conn)

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

#Region "Telerik Grid Clients Loading Events"

    Protected Sub clientslist_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles clientslist.NeedDataSource

        If Not e.IsFromDetailTable Then

            clientslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
            clientslist.DataSource = GetClients()

        End If

    End Sub

#End Region

#Region "Telerik Grid Language Modification(Spanish)"

    Protected Sub clientslist_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles clientslist.Init

        clientslist.PagerStyle.NextPagesToolTip = "Ver mas"
        clientslist.PagerStyle.NextPageToolTip = "Siguiente"
        clientslist.PagerStyle.PrevPagesToolTip = "Ver más"
        clientslist.PagerStyle.PrevPageToolTip = "Atrás"
        clientslist.PagerStyle.LastPageToolTip = "Última Página"
        clientslist.PagerStyle.FirstPageToolTip = "Primera Página"
        clientslist.PagerStyle.PagerTextFormat = "{4}    Página {0} de {1}, Registros {2} al {3} de {5}"
        clientslist.SortingSettings.SortToolTip = "Ordernar"
        clientslist.SortingSettings.SortedAscToolTip = "Ordenar Asc"
        clientslist.SortingSettings.SortedDescToolTip = "Ordenar Desc"


        Dim menu As Telerik.Web.UI.GridFilterMenu = clientslist.FilterMenu
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

        clientslist.GroupingSettings.CaseSensitive = False

        Dim Menu As Telerik.Web.UI.GridFilterMenu = clientslist.FilterMenu
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

#Region "Telerik Grid Clients Editing & Deleting Events"

    Protected Sub clientslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles clientslist.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Clients" Then

                Dim lnkEdit As LinkButton = CType(dataItem("Edit").FindControl("lnkEdit"), LinkButton)
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('" & Resources.Resource.ClientsDeleteConfirmationMessage & "');")
                If Session("perfilid") <> 1 Then
                    lnkdel.Visible = False
                    lnkEdit.Enabled = False
                End If

                If Session("userid") = 24 Then
                    lnkdel.Visible = True
                    lnkEdit.Enabled = True
                End If

                If Session("userid") = 27 Then
                    lnkdel.Visible = True
                    lnkEdit.Enabled = True
                End If

                If Session("userid") = 38 Then
                    lnkdel.Visible = True
                    lnkEdit.Enabled = True
                End If

            End If

        End If

    End Sub

    Protected Sub clientslist_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles clientslist.ItemCommand

        Select Case e.CommandName

            Case "cmdEdit"
                EditClient(e.CommandArgument)

            Case "cmdDelete"
                DeleteClient(e.CommandArgument)

        End Select

    End Sub

    Private Sub DeleteClient(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pMisClientes @cmd='3', @clienteId ='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelClientRegistration.Visible = False
            panelSucursal.Visible = False

            clientslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
            clientslist.DataSource = GetClients()
            clientslist.DataBind()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub EditClient(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pMisClientes @cmd=2, @clienteId='" & id & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                txtNombreComercial.Text = rs("nombre_comercial")
                txtSocialReason.Text = rs("razonsocial")
                txtDenominacionRaznScial.Text = rs("denominacion_razon_social")
                txtContact.Text = rs("contacto")
                txtContactEmail.Text = rs("email_contacto")
                txtCC.Text = rs("email_cc")
                txtCCO.Text = rs("email_cco")
                txtContactPhone.Text = rs("telefono_contacto")

                txtContactoPagos.Text = rs("contacto_pagos")
                txtEmailContactoPagos.Text = rs("email_contacto_pagos")
                txtTelefonoContactoPagos.Text = rs("telefono_contacto_pagos")

                txtStreet.Text = rs("fac_calle")
                txtExtNumber.Text = rs("fac_num_ext")
                txtIntNumber.Text = rs("fac_num_int")
                txtColony.Text = rs("fac_colonia")
                txtCountry.Text = rs("fac_pais")
                txtTownship.Text = rs("fac_municipio")
                txtZipCode.Text = rs("fac_cp")
                '
                txtEnvioCalle.Text = rs("env_calle")
                txtEnvioNoExt.Text = rs("env_num_ext")
                txtEnvioNoInt.Text = rs("env_num_int")
                txtEnvioColonia.Text = rs("env_colonia")
                txtEnvioCP.Text = rs("env_cp")
                txtEnvioMunicipio.Text = rs("env_municipio")
                txtEnvioNombre.Text = rs("env_contacto")
                txtEnvioEmail.Text = rs("env_email")
                txtEnvioTelefono.Text = rs("env_telefono")
                '
                txtRFC.Text = rs("rfc")
                txtNumCtaPago.Text = rs("numctapago")
                txtDescuento.Text = rs("descuento")
                txtLimiteCredito.Text = rs("limite_credito")
                txtComisionEjecutivo.Text = rs("comision_Ejecutivo")
                txtComisionCoordinador.Text = rs("comision_CoordinadorC")
                chkFacAutomaticas.Checked = rs("chkFacAutomaticas")
                '
                Dim ObjData As New DataControl
                ObjData.Catalogo(tipoprecioid, "select id, nombre from tblTipoPrecio", rs("tipoprecioid"))
                ObjData.Catalogo(condicionesid, "select id, nombre from tblCondiciones", rs("condicionesid"))
                ObjData.Catalogo(tipoContribuyenteid, "select id, nombre from tblTipoContribuyente", rs("tipoContribuyenteid"))
                Call SetCmbRegFiscal(rs("tipoContribuyenteid"), rs("regimenfiscalid"))

                ObjData.CatalogoStr(formapagoid, "select id, id + ' - ' + nombre as descripcion from tblFormaPago order by nombre", rs("formapagoid"))
                ObjData.Catalogo(dropEstado, "select id, nombre from tblEstado order by nombre", rs("fac_estadoid"))
                ObjData.Catalogo(dropEnvEstado, "select id, nombre from tblEstado order by nombre", rs("env_estadoid"))
                ObjData.Catalogo(ejecutivoid, "select id, nombre from tblUsuario where isnull(borradobit,0)=0 order by nombre", rs("ejecutivoid"))
                ObjData.Catalogo(estatusid, "select id, nombre from tblEstatusCliente order by nombre", rs("estatusid"))
                ObjData.Catalogo(cmbCoordinadorC, "select id, nombre from tblUsuario where isnull(borradobit,0)=0 order by nombre", rs("coordinadorCid"))
                ObjData.Catalogo(cuentaconcentradoraid, "select id, nombre from tblCuentasConcentradoras where isnull(borradorbit,0)=0 order by nombre", rs("cuenta_concentradoraid"))

                ObjData = Nothing

                txtObservaciones.Text = rs("observaciones")

                panelClientRegistration.Visible = True
                panelSucursal.Visible = True
                RadTabStrip1.Tabs(1).Enabled = True
                cuentasList.MasterTableView.NoMasterRecordsText = "No se encontraron datos para mostrar"
                cuentasList.DataSource = ObtenerCuentas()
                cuentasList.DataBind()

                InsertOrUpdate.Value = 1
                ClientsID.Value = id

                grdSucursal.MasterTableView.NoMasterRecordsText = "No existen sucursales registradas"
                grdSucursal.DataSource = CargaSucursales()
                grdSucursal.DataBind()

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

#End Region

#Region "Telerik Grid Clients Column Names (From Resource File)"

    Protected Sub clientslist_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles clientslist.ItemCreated

        If TypeOf e.Item Is GridHeaderItem Then

            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)

            If e.Item.OwnerTableView.Name = "Clients" Then

                header("contacto").Text = Resources.Resource.gridColumnNameContact
                header("telefono_contacto").Text = Resources.Resource.gridColumnNameContactPhone
                header("rfc").Text = Resources.Resource.gridColumnNameRFC

            End If

        End If

    End Sub

#End Region

#Region "Display Client Data Panel"

    Protected Sub btnAddClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddClient.Click

        InsertOrUpdate.Value = 0

        txtSocialReason.Text = ""
        txtNombreComercial.Text = ""
        txtDenominacionRaznScial.Text = ""
        txtContact.Text = ""
        txtContactEmail.Text = ""
        txtCC.Text = ""
        txtCCO.Text = ""
        txtContactPhone.Text = ""
        txtContactoPagos.Text = ""
        txtEmailContactoPagos.Text = ""
        txtTelefonoContactoPagos.Text = ""
        txtStreet.Text = ""
        txtExtNumber.Text = ""
        txtIntNumber.Text = ""
        txtColony.Text = ""
        dropEstado.SelectedValue = 0
        ejecutivoid.SelectedIndex = 0
        txtCountry.Text = ""
        txtTownship.Text = ""
        txtZipCode.Text = ""
        txtRFC.Text = ""
        txtDescuento.Text = 0
        txtLimiteCredito.Text = 0
        formapagoid.SelectedValue = 0
        tipoContribuyenteid.SelectedValue = 0
        regimenid.SelectedValue = 0
        txtNumCtaPago.Text = ""
        cmbCoordinadorC.SelectedValue = 0
        txtComisionEjecutivo.Text = 0
        txtComisionCoordinador.Text = 0
        cuentaconcentradoraid.SelectedValue = 0
        chkFacAutomaticas.Checked = 0
        panelClientRegistration.Visible = True
        panelSucursal.Visible = False

    End Sub

#End Region

#Region "Save Client"

    Protected Sub btnSaveClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveClient.Click

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            If InsertOrUpdate.Value = 0 Then

                Dim cmd As New SqlCommand("EXEC pMisClientes @cmd=4, @clienteUnionId='" & Session("clienteid").ToString & "', @razonsocial='" & txtSocialReason.Text & "', @contacto='" & txtContact.Text & "', @email_contacto='" & txtContactEmail.Text & "', @email_cc='" & txtCC.Text & "', @email_cco='" & txtCCO.Text & "', @telefono_contacto='" & txtContactPhone.Text & "', @contacto_pagos='" & txtContactoPagos.Text & "', @email_contacto_pagos='" & txtEmailContactoPagos.Text & "', @telefono_contacto_pagos='" & txtTelefonoContactoPagos.Text & "', @fac_calle='" & txtStreet.Text & "', @fac_num_int='" & txtIntNumber.Text & "', @fac_num_ext='" & txtExtNumber.Text & "', @fac_colonia='" & txtColony.Text & "', @fac_pais='" & txtCountry.Text & "', @fac_municipio='" & txtTownship.Text & "', @fac_estadoid=" & dropEstado.SelectedValue.ToString() & ", @fac_cp='" & txtZipCode.Text & "', @fac_rfc='" & txtRFC.Text & "', @tipoprecioid='" & tipoprecioid.SelectedValue.ToString & "', @condicionesid='" & condicionesid.SelectedValue.ToString & "', @tipocontribuyenteid='" & tipoContribuyenteid.SelectedValue.ToString & "', @formapagoid='" & formapagoid.SelectedValue.ToString & "', @numctapago='" & txtNumCtaPago.Text & "', @env_calle='" & txtEnvioCalle.Text & "', @env_num_ext='" & txtEnvioNoExt.Text & "', @env_num_int='" & txtEnvioNoInt.Text & "', @env_colonia='" & txtEnvioColonia.Text & "', @env_municipio='" & txtEnvioMunicipio.Text & "', @env_estadoid='" & dropEnvEstado.SelectedValue.ToString & "', @env_cp='" & txtEnvioCP.Text & "', @env_contacto='" & txtEnvioNombre.Text & "', @env_email='" & txtEnvioEmail.Text & "', @env_telefono='" & txtEnvioTelefono.Text & "', @ejecutivoid='" & ejecutivoid.SelectedValue.ToString & "', @observaciones='" & txtObservaciones.Text & "', @nombre_comercial='" & txtNombreComercial.Text & "', @descuento='" & txtDescuento.Text & "', @limite_credito='" & txtLimiteCredito.Text & "', @regimenfiscalid='" & regimenid.SelectedValue & "', @denominacion_razon_social='" & txtDenominacionRaznScial.Text & "', @comisionEjecutivo='" & txtComisionEjecutivo.Text & "', @comisionCoordinadorC='" & txtComisionCoordinador.Text & "', @coordinadorCid='" & cmbCoordinadorC.SelectedValue & "', @cuenta_concentradoraid='" & cuentaconcentradoraid.SelectedValue & "', @chkFacAutomaticas='" & chkFacAutomaticas.Checked & "'", conn)

                conn.Open()

                cmd.ExecuteReader()

                panelClientRegistration.Visible = False
                panelSucursal.Visible = False

                clientslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                clientslist.DataSource = GetClients()
                clientslist.DataBind()

                conn.Close()
                conn.Dispose()

            Else

                Dim cmd As New SqlCommand("EXEC pMisClientes @cmd=5, @clienteid='" & ClientsID.Value & "', @razonsocial='" & txtSocialReason.Text & "', @contacto='" & txtContact.Text & "', @email_contacto='" & txtContactEmail.Text & "', @email_cc='" & txtCC.Text & "', @email_cco='" & txtCCO.Text & "', @telefono_contacto='" & txtContactPhone.Text & "', @contacto_pagos='" & txtContactoPagos.Text & "', @email_contacto_pagos='" & txtEmailContactoPagos.Text & "', @telefono_contacto_pagos='" & txtTelefonoContactoPagos.Text & "', @fac_calle='" & txtStreet.Text & "', @fac_num_int='" & txtIntNumber.Text & "', @fac_num_ext='" & txtExtNumber.Text & "', @fac_colonia='" & txtColony.Text & "',  @fac_pais='" & txtCountry.Text & "', @fac_municipio='" & txtTownship.Text & "', @fac_estadoid=" & dropEstado.SelectedValue.ToString() & ", @fac_cp='" & txtZipCode.Text & "', @fac_rfc='" & txtRFC.Text & "', @tipoprecioid='" & tipoprecioid.SelectedValue.ToString & "', @condicionesid='" & condicionesid.SelectedValue.ToString & "', @tipocontribuyenteid='" & tipoContribuyenteid.SelectedValue.ToString & "', @formapagoid='" & formapagoid.SelectedValue.ToString & "', @numctapago='" & txtNumCtaPago.Text & "', @env_calle='" & txtEnvioCalle.Text & "', @env_num_ext='" & txtEnvioNoExt.Text & "', @env_num_int='" & txtEnvioNoInt.Text & "', @env_colonia='" & txtEnvioColonia.Text & "', @env_municipio='" & txtEnvioMunicipio.Text & "', @env_estadoid='" & dropEnvEstado.SelectedValue.ToString & "', @env_cp='" & txtEnvioCP.Text & "', @env_contacto='" & txtEnvioNombre.Text & "', @env_email='" & txtEnvioEmail.Text & "', @env_telefono='" & txtEnvioTelefono.Text & "', @ejecutivoid='" & ejecutivoid.SelectedValue.ToString & "', @observaciones='" & txtObservaciones.Text & "', @nombre_comercial='" & txtNombreComercial.Text & "', @descuento='" & txtDescuento.Text & "', @estatusid='" & estatusid.SelectedValue.ToString() & "', @limite_credito='" & txtLimiteCredito.Text & "', @regimenfiscalid='" & regimenid.SelectedValue & "', @denominacion_razon_social='" & txtDenominacionRaznScial.Text & "', @comisionEjecutivo='" & txtComisionEjecutivo.Text & "', @comisionCoordinadorC='" & txtComisionCoordinador.Text & "', @coordinadorCid='" & cmbCoordinadorC.SelectedValue & "', @cuenta_concentradoraid='" & cuentaconcentradoraid.SelectedValue & "', @chkFacAutomaticas='" & chkFacAutomaticas.Checked & "'", conn)

                conn.Open()

                cmd.ExecuteReader()

                panelClientRegistration.Visible = False
                panelSucursal.Visible = False

                clientslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                clientslist.DataSource = GetClients()
                clientslist.DataBind()

                conn.Close()
                conn.Dispose()

                'RadTabStrip1.Tabs(1).Enabled = False
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            Response.End()
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

#End Region

#Region "Cancel Client (Save/Edit)"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        InsertOrUpdate.Value = 0

        txtSocialReason.Text = ""
        txtContact.Text = ""
        txtContactEmail.Text = ""
        txtContactPhone.Text = ""
        txtContactoPagos.Text = ""
        txtEmailContactoPagos.Text = ""
        txtTelefonoContactoPagos.Text = ""
        txtStreet.Text = ""
        txtExtNumber.Text = ""
        txtIntNumber.Text = ""
        txtColony.Text = ""
        dropEstado.SelectedValue = 0
        txtCountry.Text = ""
        txtTownship.Text = ""
        txtZipCode.Text = ""
        txtRFC.Text = ""
        txtDescuento.Text = 0
        txtLimiteCredito.Text = 0
        cmbCoordinadorC.SelectedValue = 0
        txtComisionEjecutivo.Text = 0
        txtComisionCoordinador.Text = 0

        panelClientRegistration.Visible = False
        panelSucursal.Visible = False

    End Sub

#End Region

#Region "Guardar / Editar / Eliminar / Listar Sucursal"
    Private Sub btnGuardarSucursal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardarSucursal.Click

        Try

            If SucursalID.Value = 0 Then

                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                Dim cmd As New SqlCommand("EXEC pAgregaSucursal @sucursal='" & txtSucursal.Text & "', @clienteid='" & ClientsID.Value.ToString() & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "'", conn)
                conn.Open()

                cmd.ExecuteReader()
                conn.Close()
                conn.Dispose()

                grdSucursal.MasterTableView.NoMasterRecordsText = "No existen sucursales registradas"
                grdSucursal.DataSource = CargaSucursales()
                grdSucursal.DataBind()

            Else

                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                Dim cmd As New SqlCommand("EXEC pEditaSucursal @sucursalid='" & SucursalID.Value.ToString & "', @clienteid='" & ClientsID.Value.ToString() & "', @vendedorid ='" & vendedorid.SelectedValue.ToString & "', @sucursal='" & txtSucursal.Text & "'", conn)
                conn.Open()

                cmd.ExecuteReader()
                conn.Close()
                conn.Dispose()

                SucursalID.Value = 0

                grdSucursal.MasterTableView.NoMasterRecordsText = "No existen sucursales registradas"
                grdSucursal.DataSource = CargaSucursales()
                grdSucursal.DataBind()

            End If

            vendedorid.SelectedValue = 0
            txtSucursal.Text = ""

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        End Try

    End Sub

    Private Sub EditaSucursal(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try
            Dim cmd As New SqlCommand("select id, isnull(vendedorid,0) as vendedorid, sucursal from tblSucursalCliente where id='" & id & "'", conn)
            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                vendedorid.SelectedValue = rs("vendedorid")
                'tipoprecioid.SelectedValue = rs("tipoprecioid")
                txtSucursal.Text = rs("sucursal")
                SucursalID.Value = id

            End If

            rs.Close()
            grdSucursal.MasterTableView.NoMasterRecordsText = "No existen sucursales registradas"
            grdSucursal.DataSource = CargaSucursales()
            grdSucursal.DataBind()

        Catch ex As Exception

        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub EliminaSucursal(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try
            Dim cmd As New SqlCommand("EXEC pEliminaSucursal @sucursalid ='" & id.ToString & "'", conn)
            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()
            rs.Close()
            conn.Close()

            grdSucursal.DataSource = CargaSucursales()
            grdSucursal.DataBind()

        Catch ex As Exception

        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Function CargaSucursales() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pListarSucursales  @clienteid='" & ClientsID.Value.ToString & "'", conn)

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

    Protected Sub grdSucursal_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdSucursal.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Sucursales" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('¿Está seguro que desea borrar esta sucursal de la base de datos?');")
                If Session("perfilid") = 3 Then
                    lnkdel.Visible = False
                End If
            End If

        End If

    End Sub

    Protected Sub grdSucursal_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdSucursal.ItemCommand
        Select Case e.CommandName

            Case "cmdEdit"
                EditaSucursal(e.CommandArgument)

            Case "cmdDelete"
                EliminaSucursal(e.CommandArgument)

        End Select
    End Sub

    Private Sub grdSucursal_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdSucursal.NeedDataSource
        grdSucursal.MasterTableView.NoMasterRecordsText = "No existen sucursales registradas"
        grdSucursal.DataSource = CargaSucursales()
    End Sub

    Private Sub estatusclienteid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles estatusclienteid.SelectedIndexChanged
        clientslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
        clientslist.DataSource = GetClients()
        clientslist.DataBind()
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        clientslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
        clientslist.DataSource = GetClients()
        clientslist.DataBind()
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click
        If Page.IsValid Then
            Dim objData As New DataControl
            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
            Dim sql As String = ""
            Dim IdCuentas As Integer = 0

            If CuentaID.Value = 0 Then
                IdCuentas = objData.RunSQLScalarQuery("EXEC pCatalogoCuentas @cmd=1, @banconacional='" & txtBanco.Text & "', @bancoextranjero='" & txtBancoExtr.Text & "',@rfc='" & txtRFCBAK.Text & "', @numctapago='" & txtCuenta.Text & "', @predeterminadoBit='" & chkPredeterminado.Checked & "', @clienteid='" & ClientsID.Value & "'")
                ClearItems()
            Else
                objData.RunSQLScalarQuery("EXEC pCatalogoCuentas @cmd=4, @banconacional='" & txtBanco.Text & "', @bancoextranjero='" & txtBancoExtr.Text & "', @rfc='" & txtRFCBAK.Text & "', @numctapago='" & txtCuenta.Text & "',@predeterminadoBit='" & chkPredeterminado.Checked & "', @clienteid='" & ClientsID.Value & "', @id='" & CuentaID.Value & "'")
                ClearItems()
                objData = Nothing
            End If

            cuentasList.MasterTableView.NoMasterRecordsText = "No se encontraron datos para mostrar"
            cuentasList.DataSource = ObtenerCuentas()
            cuentasList.DataBind()

        End If
    End Sub

    Function ObtenerCuentas() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim qry As String = "EXEC pCatalogoCuentas @cmd=5, @clienteid='" & ClientsID.Value & "'"

        Dim cmd As New SqlDataAdapter(qry, conn)

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

        Return ds

    End Function

    Private Sub DeleteCuenta(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCatalogoCuentas @cmd=3, @id='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            cuentasList.MasterTableView.NoMasterRecordsText = "No se encontraron datos para mostrar"
            cuentasList.DataSource = ObtenerCuentas()
            cuentasList.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub CargarCuenta()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCatalogoCuentas @cmd=2, @id='" & CuentaID.Value & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                txtBanco.Text = rs("banconacional")
                txtBancoExtr.Text = rs("bancoextranjero")
                txtCuenta.Text = rs("numctapago")
                txtRFCBAK.Text = rs("rfc")
                chkPredeterminado.Checked = rs("predeterminadoBit")
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
            Response.End()
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        ClearItems()
    End Sub

    Private Sub cuentasList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles cuentasList.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                CuentaID.Value = e.CommandArgument
                Call CargarCuenta()
                btnCancelar.Visible = True
            Case "cmdDelete"
                Call DeleteCuenta(e.CommandArgument)
        End Select
    End Sub

    Private Sub cuentasList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles cuentasList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim imgPredeterminado As System.Web.UI.WebControls.Image = CType(e.Item.FindControl("imgPredeterminado"), System.Web.UI.WebControls.Image)
                imgPredeterminado.Visible = e.Item.DataItem("predeterminadoBit")
        End Select
    End Sub

    Sub ClearItems()
        txtBanco.Text = ""
        txtBancoExtr.Text = ""
        txtCuenta.Text = ""
        txtRFCBAK.Text = ""
        chkPredeterminado.Checked = False
        txtBanco.Focus()
        CuentaID.Value = 0
    End Sub

    Private Sub cuentasList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles cuentasList.NeedDataSource
        cuentasList.MasterTableView.NoMasterRecordsText = "No se encontraron datos para mostrar"
        cuentasList.DataSource = ObtenerCuentas()
    End Sub

    Private Sub tipoContribuyenteid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tipoContribuyenteid.SelectedIndexChanged
        Call SetCmbRegFiscal(tipoContribuyenteid.SelectedValue)
    End Sub
End Class
