Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Empleados
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            ddlBanco.Enabled = False
            txtClabe.Enabled = False
            RequiredFieldValidator23.Enabled = False
            RequiredFieldValidator24.Enabled = False

            Dim ObjData As New DataControl
            ObjData.Catalogo(ddlEstado, "select id, nombre from tblEstado order by nombre", 0)
            ObjData.Catalogo(ddlDepartamento, "select id, nombre from tblDepartamento where borradoBit is null order by nombre", 0)
            ObjData.Catalogo(ddlPeriodoPago, "select id, nombre from tblPeriodoPago", 0)
            ObjData.Catalogo(ddlMetodoPago, "select id, nombre from tblMetodoPagoEmpleado", 0)
            ObjData.Catalogo(ddlRegimenContratacion, "select id,nombre from tblRegimenContratacion", 0)
            ObjData.Catalogo(ddlBanco, "select id,nombre from tblBanco order by id asc", 0)
            ObjData.Catalogo(ddlRiesgoPuesto, "select id,nombre from tblRiesgoPuesto order by id asc", 0)
            ObjData = Nothing
        End If

    End Sub

    Private Sub btnAgregaEmpleado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregaEmpleado.Click

        InsertOrUpdate.Value = 0

        lblMensaje.Text = ""
        txtNoEmpleado.Text = ""
        txtNombre.Text = ""
        txtApellidoPaterno.Text = ""
        txtApellidoMaterno.Text = ""
        rblSexo.SelectedValue = 0
        txtRFC.Text = ""
        txtCURP.Text = ""
        txtNoSegSocial.Text = ""
        txtCalle.Text = ""
        txtNoExterior.Text = ""
        txtNoInterior.Text = ""
        txtColonia.Text = ""
        txtPais.Text = ""
        ddlEstado.SelectedValue = 0
        txtMunicipio.Text = ""
        txtCP.Text = ""
        ddlDepartamento.SelectedValue = 0
        txtPuesto.Text = ""
        ddlTipoJornada.SelectedValue = 0
        calFechaIngreso.Clear()
        ddlPeriodoPago.SelectedValue = 0
        ddlRegimenContratacion.SelectedValue = 0
        txtSalarioBase.Text = ""
        txtSDI.Text = ""
        txtEmail.Text = ""
        ddlMetodoPago.SelectedValue = 0
        ddlBanco.SelectedValue = 0
        txtClabe.Text = ""
        ddlRiesgoPuesto.SelectedValue = 0
        txtAntiguedad.Text = ""
        txtRegistroPatronal.Text = ""

        panelEmployeeRegistration.Visible = True

        grdPercepciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdPercepciones.DataSource = ObtenerPercepciones()
        grdPercepciones.DataBind()

        grdDeducciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdDeducciones.DataSource = ObtenerDeducciones()
        grdDeducciones.DataBind()

    End Sub

    Private Sub btnGuardarEmpleado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardarEmpleado.Click

        lblMensaje.Visible = False

        Dim fechaIngreso As DateTime
        fechaIngreso = calFechaIngreso.SelectedDate

        If InsertOrUpdate.Value = 0 Then

            Dim ObjData As New DataControl
            Dim empleadoId As Integer = 0

            empleadoId = ObjData.RunSQLScalarQuery("select count(id) as total from tblEmpleado where numero_empleado='" & txtNoEmpleado.Text.ToString.Trim & "' and ISNULL(borradoBit,0)=0")

            If empleadoId = 0 Then

                empleadoId = ObjData.RunSQLScalarQuery("EXEC pEmpleados @cmd=4, @numero_empleado='" & txtNoEmpleado.Text.ToString & "', @nombre='" & txtNombre.Text.ToUpper.ToString & "', @apellido_paterno='" & txtApellidoPaterno.Text.ToUpper.ToString & "', @apellido_materno='" & txtApellidoMaterno.Text.ToUpper.ToString & "', @sexo='" & rblSexo.SelectedValue.ToString & "', @rfc='" & txtRFC.Text.ToUpper.ToString & "', @curp='" & txtCURP.Text.ToUpper.ToString & "', @numero_seguro_social='" & txtNoSegSocial.Text.ToString & "', @calle='" & txtCalle.Text.ToString & "', @num_ext='" & txtNoExterior.Text.ToString & "', @num_int='" & txtNoInterior.Text.ToString & "', @colonia='" & txtColonia.Text.ToString & "', @codigo_postal='" & txtCP.Text.ToString & "', @municipio='" & txtMunicipio.Text.ToString & "', @estadoid='" & ddlEstado.SelectedValue.ToString & "', @pais='" & txtPais.Text.ToString & "', @fecha_ingreso='" & fechaIngreso.ToString("yyyyMMdd") & "', @salario_base='" & txtSalarioBase.Text.ToString & "', @salario_diario_integrado='" & txtSDI.Text.ToString & "', @tipo_jornada='" & ddlTipoJornada.SelectedValue.ToString & "', @puesto='" & txtPuesto.Text.ToString & "', @email='" & txtEmail.Text.ToLower.ToString & "', @departamentoid='" & ddlDepartamento.SelectedValue.ToString & "', @periodopagoid='" & ddlPeriodoPago.SelectedValue.ToString & "', @metodopagoid='" & ddlMetodoPago.SelectedValue.ToString & "', @regimencontratacionid='" & ddlRegimenContratacion.SelectedValue.ToString & "', @bancoid='" & ddlBanco.SelectedValue.ToString & "', @clabe='" & txtClabe.Text.ToString & "', @riesgopuestoid='" & ddlRiesgoPuesto.SelectedValue.ToString & "', @antiguedad='" & txtAntiguedad.Text & "', @registro_patronal='" & txtRegistroPatronal.Text.ToString & "'")

                'Insertamos en tabla tblPercepcionEmpleado los montos de las percepciones al empleado
                For Each dataItem As Telerik.Web.UI.GridDataItem In grdPercepciones.MasterTableView.Items
                    Dim percepcionId As String = dataItem.GetDataKeyValue("id").ToString
                    Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)

                    If txtImporte.Text = "" Then
                        txtImporte.Text = "0.00"
                    End If

                    ObjData.RunSQLQuery("EXEC pPercepciones @cmd=7, @empleadoid='" & EmployeeID.Value.ToString & "', @percepcionid='" & percepcionId.ToString & "', @monto='" & txtImporte.Text.ToString & "'")
                Next

                'Insertamos en tabla tblDeduccionEmpleado los montos de las deducciones al empleado
                For Each dataItem As Telerik.Web.UI.GridDataItem In grdDeducciones.MasterTableView.Items
                    Dim deduccionId As String = dataItem.GetDataKeyValue("id").ToString
                    Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)

                    If txtImporte.Text = "" Then
                        txtImporte.Text = "0.00"
                    End If

                    ObjData.RunSQLQuery("EXEC pDeducciones @cmd=7, @empleadoid='" & EmployeeID.Value.ToString & "', @deduccionid='" & deduccionId.ToString & "', @monto='" & txtImporte.Text.ToString & "'")
                Next

                panelEmployeeRegistration.Visible = False

                employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
                employeeslist.DataSource = GetEmployees()
                employeeslist.DataBind()
                ObjData = Nothing
            Else
                lblMensaje.Visible = True
                lblMensaje.Text = "El No. de empleado no se encuentra disponible."
                Return
            End If

        Else
            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
            Dim cmd As New SqlCommand("EXEC pEmpleados @cmd=5, @empleadoid='" & EmployeeID.Value.ToString & "', @numero_empleado='" & txtNoEmpleado.Text.ToString & "', @nombre='" & txtNombre.Text.ToUpper.ToString & "', @apellido_paterno='" & txtApellidoPaterno.Text.ToUpper.ToString & "', @apellido_materno='" & txtApellidoMaterno.Text.ToUpper.ToString & "', @sexo='" & rblSexo.SelectedValue.ToString & "', @rfc='" & txtRFC.Text.ToUpper.ToString & "', @curp='" & txtCURP.Text.ToUpper.ToString & "', @numero_seguro_social='" & txtNoSegSocial.Text.ToString & "', @calle='" & txtCalle.Text.ToString & "', @num_ext='" & txtNoExterior.Text.ToString & "', @num_int='" & txtNoInterior.Text.ToString & "', @colonia='" & txtColonia.Text.ToString & "', @codigo_postal='" & txtCP.Text.ToString & "', @municipio='" & txtMunicipio.Text.ToString & "', @estadoid='" & ddlEstado.SelectedValue.ToString & "', @pais='" & txtPais.Text.ToString & "', @fecha_ingreso='" & fechaIngreso.ToString("yyyyMMdd") & "', @salario_base='" & txtSalarioBase.Text.ToString & "', @salario_diario_integrado='" & txtSDI.Text.ToString & "', @tipo_jornada='" & ddlTipoJornada.SelectedValue.ToString & "', @puesto='" & txtPuesto.Text.ToString & "', @email='" & txtEmail.Text.ToLower.ToString & "', @departamentoid='" & ddlDepartamento.SelectedValue.ToString & "', @periodopagoid='" & ddlPeriodoPago.SelectedValue.ToString & "', @metodopagoid='" & ddlMetodoPago.SelectedValue.ToString & "', @regimencontratacionid='" & ddlRegimenContratacion.SelectedValue.ToString & "', @bancoid='" & ddlBanco.SelectedValue.ToString & "', @clabe='" & txtClabe.Text.ToString & "', @riesgopuestoid='" & ddlRiesgoPuesto.SelectedValue.ToString & "', @antiguedad='" & txtAntiguedad.Text & "', @registro_patronal='" & txtRegistroPatronal.Text.ToString & "'", conn)
            conn.Open()

            cmd.ExecuteReader()

            Dim ObjData As New DataControl
            ObjData.RunSQLQuery("EXEC pPercepciones @cmd=6, @empleadoid='" & EmployeeID.Value.ToString & "'")
            'Insertamos en tabla tblPercepcionEmpleado los montos de las percepciones al empleado
            For Each dataItem As Telerik.Web.UI.GridDataItem In grdPercepciones.MasterTableView.Items
                Dim percepcionId As String = dataItem.GetDataKeyValue("id").ToString
                Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)

                If txtImporte.Text = "" Then
                    txtImporte.Text = "0.00"
                End If
                ObjData.RunSQLQuery("EXEC pPercepciones @cmd=7, @empleadoid='" & EmployeeID.Value.ToString & "', @percepcionid='" & percepcionId.ToString & "', @monto='" & txtImporte.Text.ToString & "'")
            Next

            'Insertamos en tabla tblDeduccionEmpleado los montos de las deducciones al empleado
            ObjData.RunSQLQuery("EXEC pDeducciones @cmd=6, @empleadoid='" & EmployeeID.Value.ToString & "'")
            For Each dataItem As Telerik.Web.UI.GridDataItem In grdDeducciones.MasterTableView.Items
                Dim deduccionId As String = dataItem.GetDataKeyValue("id").ToString
                Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)

                If txtImporte.Text = "" Then
                    txtImporte.Text = "0.00"
                End If
                ObjData.RunSQLQuery("EXEC pDeducciones @cmd=7, @empleadoid='" & EmployeeID.Value.ToString & "', @deduccionid='" & deduccionId.ToString & "', @monto='" & txtImporte.Text.ToString & "'")
            Next

            panelEmployeeRegistration.Visible = False

            employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
            employeeslist.DataSource = GetEmployees()
            employeeslist.DataBind()
            ObjData = Nothing
            conn.Close()
            conn.Dispose()

        End If
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        lblMensaje.Text = ""
        txtNoEmpleado.Text = ""
        txtNombre.Text = ""
        txtApellidoPaterno.Text = ""
        txtApellidoMaterno.Text = ""
        rblSexo.SelectedValue = "0"
        txtRFC.Text = ""
        txtCURP.Text = ""
        txtNoSegSocial.Text = ""
        txtCalle.Text = ""
        txtNoExterior.Text = ""
        txtNoInterior.Text = ""
        txtColonia.Text = ""
        txtPais.Text = ""
        ddlEstado.SelectedValue = "0"
        txtMunicipio.Text = ""
        txtCP.Text = ""
        ddlDepartamento.SelectedValue = "0"
        txtPuesto.Text = ""
        ddlTipoJornada.SelectedValue = "0"
        calFechaIngreso.Clear()
        ddlPeriodoPago.SelectedValue = "0"
        txtSalarioBase.Text = ""
        txtSDI.Text = ""
        txtEmail.Text = ""
        ddlMetodoPago.SelectedValue = 0
        ddlBanco.SelectedValue = 0
        txtClabe.Text = 0
        ddlRiesgoPuesto.SelectedValue = 0
        txtAntiguedad.Text = ""
        txtRegistroPatronal.Text = ""

        panelEmployeeRegistration.Visible = False

    End Sub

    Private Sub employeeslist_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles employeeslist.ItemCommand
        Select Case e.CommandName

            Case "cmdEdit"
                EditEmployee(e.CommandArgument)

            Case "cmdDelete"
                DeleteEmployee(e.CommandArgument)

        End Select
    End Sub

    Private Sub EditEmployee(ByVal id As Integer)

        ObtenerPercepciones(id.ToString)
        ObtenerDeducciones(id.ToString)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pEmpleados @cmd=2, @empleadoid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                txtNoEmpleado.Text = rs("numero_empleado")
                txtNombre.Text = rs("nombre")
                txtApellidoPaterno.Text = rs("apellido_paterno")
                txtApellidoMaterno.Text = rs("apellido_materno")
                rblSexo.SelectedValue = rs("sexo")
                txtRFC.Text = rs("rfc")
                txtCURP.Text = rs("curp")
                txtNoSegSocial.Text = rs("numero_seguro_social")
                txtCalle.Text = rs("calle")
                txtNoExterior.Text = rs("num_ext")
                txtNoInterior.Text = rs("num_int")
                txtColonia.Text = rs("colonia")
                txtPais.Text = rs("pais")
                ddlEstado.SelectedValue = rs("estadoid")
                txtMunicipio.Text = rs("municipio")
                txtCP.Text = rs("codigo_postal")
                ddlDepartamento.SelectedValue = rs("departamentoid")
                txtPuesto.Text = rs("puesto")
                ddlTipoJornada.SelectedValue = rs("tipo_jornadaid")
                calFechaIngreso.SelectedDate = rs("fecha_ingreso").ToString
                ddlPeriodoPago.SelectedValue = rs("periodopagoid")
                ddlRegimenContratacion.SelectedValue = rs("regimencontratacionid")
                ddlMetodoPago.SelectedValue = rs("metodopagoid")
                txtSalarioBase.Text = rs("salario_base")
                txtSDI.Text = rs("salario_diario_integrado")
                txtEmail.Text = rs("email")
                ddlBanco.SelectedValue = rs("bancoid")
                txtClabe.Text = rs("clabe")
                ddlRiesgoPuesto.SelectedValue = rs("riesgopuestoid")
                txtAntiguedad.Text = rs("antiguedad")
                txtRegistroPatronal.Text = rs("registro_patronal")
                '
                panelEmployeeRegistration.Visible = True
                '
                InsertOrUpdate.Value = 1
                EmployeeID.Value = rs("id")

                If ddlMetodoPago.SelectedValue = 1 Then
                    ddlBanco.Enabled = False
                    txtClabe.Enabled = False
                    RequiredFieldValidator23.Enabled = False
                    RequiredFieldValidator24.Enabled = False
                ElseIf ddlMetodoPago.SelectedValue = 2 Or ddlMetodoPago.SelectedValue = 4 Then
                    ddlBanco.Enabled = True
                    txtClabe.Enabled = False
                    RequiredFieldValidator23.Enabled = True
                    RequiredFieldValidator24.Enabled = False
                ElseIf ddlMetodoPago.SelectedValue = 3 Then
                    ddlBanco.Enabled = True
                    txtClabe.Enabled = True
                    RequiredFieldValidator23.Enabled = True
                    RequiredFieldValidator24.Enabled = True
                End If

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub DeleteEmployee(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pEmpleados @cmd='3', @empleadoid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelEmployeeRegistration.Visible = False

            employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
            employeeslist.DataSource = GetEmployees()
            employeeslist.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub employeeslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles employeeslist.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Employees" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('" & Resources.Resource.ClientsDeleteConfirmationMessage & "');")

            End If

        End If
    End Sub

    Private Sub employeeslist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles employeeslist.NeedDataSource
        employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        employeeslist.DataSource = GetEmployees()
    End Sub

    Function GetEmployees() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pEmpleados  @cmd=1", conn)

        Dim ds As DataSet = New DataSet()

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return ds

    End Function

    Function ObtenerPercepciones() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pListarPercepciones @cmd=1", conn)

        Dim ds As DataSet = New DataSet()

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return ds

    End Function

    Function ObtenerDeducciones() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pListarDeducciones @cmd=1", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return ds

    End Function

    Private Sub ObtenerPercepciones(ByVal empleadoid As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pListarPercepciones @cmd=2,@empleadoid='" & empleadoid & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        grdPercepciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdPercepciones.DataSource = ds
        grdPercepciones.DataBind()

    End Sub

    Private Sub ObtenerDeducciones(ByVal empleadoid As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pListarDeducciones @cmd=2,@empleadoid='" & empleadoid & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        grdDeducciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdDeducciones.DataSource = ds
        grdDeducciones.DataBind()

    End Sub

    Protected Sub ddlMetodoPago_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMetodoPago.SelectedIndexChanged
        If ddlMetodoPago.SelectedValue = 1 Then
            ddlBanco.Enabled = False
            txtClabe.Enabled = False
            RequiredFieldValidator23.Enabled = False
            RequiredFieldValidator24.Enabled = False
        ElseIf ddlMetodoPago.SelectedValue = 2 Or ddlMetodoPago.SelectedValue = 4 Then
            ddlBanco.Enabled = True
            txtClabe.Enabled = False
            RequiredFieldValidator23.Enabled = True
            RequiredFieldValidator24.Enabled = False
        ElseIf ddlMetodoPago.SelectedValue = 3 Then
            ddlBanco.Enabled = True
            txtClabe.Enabled = True
            RequiredFieldValidator23.Enabled = True
            RequiredFieldValidator24.Enabled = True
        End If
    End Sub

    Protected Sub grdDeducciones_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdDeducciones.NeedDataSource
        grdDeducciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdDeducciones.DataSource = ObtenerDeducciones()
    End Sub

    Protected Sub grdPercepciones_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdPercepciones.NeedDataSource
        grdPercepciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdPercepciones.DataSource = ObtenerPercepciones()
    End Sub

End Class