<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="Empleados.aspx.vb" Inherits="erp_neogenis.Empleados" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblEmployeeListLegend" runat="server" Text="Lista de Empleados" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%" border="0">
                <tr>
                    <td style="height: 2px">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid ID="employeeslist" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None" AllowSorting="true"
                            PageSize="15" ShowStatusBar="True"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NextPrevAndNumeric" />
                            <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Employees" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Id" UniqueName="id" Visible="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="numero_empleado" HeaderText="No. Empleado" UniqueName="numero_empleado" ItemStyle-Width="90px"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn HeaderText="Nombre" DataField="nombre" SortExpression="nombre" UniqueName="nombre">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("nombre") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="departamento" HeaderText="Departamento" UniqueName="departamento">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="rfc" HeaderText="RFC" UniqueName="rfc">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="numero_seguro_social" HeaderText="No. Seguro Social" UniqueName="numero_seguro_social">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False"
                                        HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btnAgregaEmpleado" runat="server" Text="Agrega Empleado" CausesValidation="False" CssClass="item" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
        <asp:Panel ID="panelEmployeeRegistration" runat="server" Visible="False">
            <fieldset>
                <legend style="padding-right: 6px; color: Black">
                    <asp:Label ID="lblEmployeeEditLegend" runat="server" Text="Agregar/Editar Cliente" Font-Bold="True" CssClass="item"></asp:Label>
                </legend>
                <br />
                <table width="100%" border="0">
                    <tr valign="top">
                        <td width="25%">
                            <asp:Label ID="lblNoEmpleado" runat="server" CssClass="item" Font-Bold="True" Text="No. de Empleado:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblEmail" runat="server" CssClass="item" Font-Bold="True" Text="Email:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblFechaIngreso" runat="server" CssClass="item" Font-Bold="True" Text="Fecha Ingreso:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblAntiguedad" runat="server" CssClass="item" Font-Bold="True" Text="Antigüedad(Semanas):"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <asp:TextBox ID="txtNoEmpleado" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtNoEmpleado" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtEmail" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" CssClass="item" ValidationExpression=".*@.*\..*" ErrorMessage="Formato Inválido"></asp:RegularExpressionValidator>
                        </td>
                        <td width="25%">
                            <telerik:RadDatePicker ID="calFechaIngreso" CultureInfo="Español (México)" DateInput-DateFormat="dd/MM/yyyy" runat="server"></telerik:RadDatePicker>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="calFechaIngreso" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <telerik:RadNumericTextBox ID="txtAntiguedad" Width="200px" runat="server" MinValue="0" NumberFormat-GroupSeparator="" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:Label ID="lblNombre" runat="server" CssClass="item" Font-Bold="True" Text="Nombre(s):"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblApellidoPaterno" runat="server" CssClass="item" Font-Bold="True" Text="Apellido Paterno:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblApellidoMaterno" runat="server" CssClass="item" Font-Bold="True" Text="Apellido Materno:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblSexo" runat="server" CssClass="item" Font-Bold="True" Text="Sexo:"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:TextBox ID="txtNombre" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtApellidoPaterno" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtApellidoPaterno" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtApellidoMaterno" CssClass="item" Width="200px" runat="server"></asp:TextBox>
                        </td>
                        <td width="25%" rowspan="2" class="item">
                            <asp:RadioButtonList ID="rblSexo" runat="server">
                                <asp:ListItem Value="0" Text="Femenino" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Masculino"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:Label ID="lblRFC" runat="server" CssClass="item" Font-Bold="True" Text="RFC:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblCURP" runat="server" CssClass="item" Font-Bold="True" Text="CURP:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblNoSegSocial" runat="server" CssClass="item" Font-Bold="True" Text="No. Seguro Social:"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:TextBox ID="txtRFC" CssClass="item" Width="200px" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtRFC" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="valRFC" runat="server" ControlToValidate="txtRFC" CssClass="item" ForeColor="Red" ErrorMessage=" Formato no válido" SetFocusOnError="True" ValidationExpression="^([a-zA-Z]{3,4})\d{6}([a-zA-Z\w]{3})$"></asp:RegularExpressionValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtCURP" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtCURP" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtNoSegSocial" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtNoSegSocial" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">&nbsp;
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:Label ID="lblCalle" runat="server" CssClass="item" Font-Bold="True" Text="Calle:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblNoExterior" runat="server" CssClass="item" Font-Bold="True" Text="No. Exterior:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblNoInterior" runat="server" CssClass="item" Font-Bold="True" Text="No. Interior:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblColonia" runat="server" CssClass="item" Font-Bold="True" Text="Colonia:"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:TextBox ID="txtCalle" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtCalle" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtNoExterior" CssClass="item" Width="100px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtNoExterior" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtNoInterior" CssClass="item" Width="100px" runat="server"></asp:TextBox>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtColonia" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtColonia" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:Label ID="lblPais" runat="server" CssClass="item" Font-Bold="True" Text="País:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblEstado" runat="server" CssClass="item" Font-Bold="True" Text="Estado:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblMunicipio" runat="server" CssClass="item" Font-Bold="True" Text="Municipio:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblCP" runat="server" CssClass="item" Font-Bold="True" Text="Código Postal:"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:TextBox ID="txtPais" CssClass="item" Text="México" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtPais" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="ddlEstado" Width="200px" runat="server" CssClass="item"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlEstado" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtMunicipio" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtMunicipio" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtCP" CssClass="item" Width="100px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtCP" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:Label ID="lblDepartamento" runat="server" CssClass="item" Font-Bold="True" Text="Departamento:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblPuesto" runat="server" CssClass="item" Font-Bold="True" Text="Puesto:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblRiesgoPuesto" runat="server" CssClass="item" Font-Bold="True" Text="Riesgo Puesto:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblTipoJornada" runat="server" CssClass="item" Font-Bold="True" Text="Tipo Jornada:"></asp:Label>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:DropDownList ID="ddlDepartamento" runat="server" Width="200px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlDepartamento" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtPuesto" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtPuesto" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="ddlRiesgoPuesto" runat="server" Width="200px"></asp:DropDownList>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="ddlTipoJornada" runat="server" Width="200px">
                                <asp:ListItem Value="0" Text="--Seleccione--"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Permanente"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Variable"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddlTipoJornada" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td width="25%">
                            <asp:Label ID="lblRegimenContratacion" runat="server" CssClass="item" Font-Bold="True" Text="Régimen de Contratación:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblPeriodoPago" runat="server" CssClass="item" Font-Bold="True" Text="Período de Pago:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblSalarioBase" runat="server" CssClass="item" Font-Bold="True" Text="Salario Base:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblSDI" runat="server" CssClass="item" Font-Bold="True" Text="Salario Diario Integrado:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <asp:DropDownList ID="ddlRegimenContratacion" runat="server" Width="200px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="ddlRegimenContratacion" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="ddlPeriodoPago" runat="server" Width="200px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlPeriodoPago" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <telerik:RadNumericTextBox ID="txtSalarioBase" Width="200px" runat="server" NumberFormat-DecimalDigits="4"></telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtSalarioBase" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <telerik:RadNumericTextBox ID="txtSDI" runat="server" NumberFormat-DecimalDigits="4"></telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtSDI" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <asp:Label ID="lblMetodoPago" runat="server" CssClass="item" Font-Bold="True" Text="Método de Pago:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblBanco" runat="server" CssClass="item" Font-Bold="True" Text="Banco:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblClabe" runat="server" CssClass="item" Font-Bold="True" Text="CLABE:"></asp:Label>
                        </td>
                        <td width="25%">
                            <asp:Label ID="lblRegistroPatronal" runat="server" CssClass="item" Font-Bold="True" Text="Registro Patronal:"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <asp:DropDownList ID="ddlMetodoPago" runat="server" AutoPostBack="true" Width="200px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="ddlMetodoPago" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:DropDownList ID="ddlBanco" runat="server" Width="200px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="ddlBanco" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClabe" Width="200px" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtClabe" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            <asp:TextBox ID="txtRegistroPatronal" CssClass="item" Width="200px" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" style="width: 66%; height: 5px;">
                            <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                            <asp:HiddenField ID="EmployeeID" runat="server" Value="0" />
                        </td>
                    </tr>
                </table>
                <br />
                <fieldset>
                    <legend style="padding-right: 6px; color: Black">
                        <asp:Label ID="lblPercepcionesTitle" runat="server" Text="Percepciones" Font-Bold="true" CssClass="item"></asp:Label>
                    </legend>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadGrid ID="grdPercepciones" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" GridLines="None" AllowSorting="true"
                                    PageSize="15" ShowStatusBar="True"
                                    Skin="Simple" Width="100%">
                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                    <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Percepciones" Width="100%">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="clave" HeaderText="Clave" UniqueName="clave" ItemStyle-Width="20%">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="50%">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Importe" UniqueName="importe" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <telerik:RadNumericTextBox ID="txtImporte" runat="server" Text='<%#Eval("importe")%>' Value="0" MinValue="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <fieldset>
                    <legend style="padding-right: 6px; color: Black">
                        <asp:Label ID="lblDeduccionesTitle" runat="server" Text="Deducciones" Font-Bold="true" CssClass="item"></asp:Label>
                    </legend>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadGrid ID="grdDeducciones" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" GridLines="None" AllowSorting="true"
                                    PageSize="15" ShowStatusBar="True"
                                    Skin="Simple" Width="100%">
                                    <PagerStyle Mode="NextPrevAndNumeric" />
                                    <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Deducciones" Width="100%">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="clave" HeaderText="Clave" UniqueName="clave" ItemStyle-Width="20%">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="50%">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Importe" UniqueName="importe" ItemStyle-Width="30%">
                                                <ItemTemplate>
                                                    <telerik:RadNumericTextBox ID="txtImporte" runat="server" Text='<%#Eval("importe")%>' Value="0" MinValue="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" />
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <br />
                <table width="100%" border="0">
                    <tr>
                        <td width="75%" align="left">
                            <asp:Label ID="lblMensaje" runat="server" CssClass="item" Font-Bold="True" ForeColor="Red"></asp:Label>
                        </td>
                        <td width="25%" align="right">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="item" CausesValidation="False" />&nbsp;
                            <asp:Button ID="btnGuardarEmpleado" runat="server" Text="Guardar" CssClass="item" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
