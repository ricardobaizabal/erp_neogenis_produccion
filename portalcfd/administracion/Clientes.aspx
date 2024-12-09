<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_Clientes" CodeBehind="Clientes.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4 {
            height: 17px;
        }

        .style5 {
            height: 14px;
        }

        .style6 {
            height: 21px;
        }
    </style>
    <script type="text/javascript">
        function OnRequestStart(target, arguments) {
            if (arguments.get_eventTarget().indexOf("clientslist") > -1) {
                arguments.set_enableAjax(false);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblClientsListLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table width="100%">
                <tr>
                    <td style="width: 10%;">
                        <asp:Label ID="lblPalabraClave" runat="server" Text="Palabra clave:" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtPalabraClave" runat="server" Width="200px"></telerik:RadTextBox><br />
                    </td>
                    <td style="width: 8%;">
                        <asp:Label ID="lblEstatusFiltro" runat="server" Text="Estatus:" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td style="width: 12%;">
                        <asp:DropDownList ID="estatusclienteid" AutoPostBack="false" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td style="width: 10%;">
                        <asp:Button ID="btnBuscar" Text="Buscar" CausesValidation="false" runat="server" CssClass="item" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <br />
            <table width="100%">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="clientslist" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None"
                            OnNeedDataSource="clientslist_NeedDataSource" PageSize="15" ShowStatusBar="True"
                            Skin="Simple" Width="100%">
                            <ExportSettings HideStructureColumns="true" IgnorePaging="True" FileName="CatalogoClientes">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Clients" Width="100%" CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="No. Cte" UniqueName="id"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Razón Social" UniqueName="Edit">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("razonsocial") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="nombre_comercial" HeaderText="Nombre comercial" UniqueName="nombre_comercial">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="contacto" HeaderText="" UniqueName="contacto">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="telefono_contacto" HeaderText="" UniqueName="telefono_contacto">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="condiciones" HeaderText="Condiciones" UniqueName="condiciones">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="rfc" HeaderText="" UniqueName="rfc">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="agenteVentas" HeaderText="Agente Ventas" UniqueName="agenteventas">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="avporcentaje" HeaderText="AV. %" UniqueName="avporcentaje">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="coordinador" HeaderText="Coordinador" UniqueName="coordinador">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cporcentaje" HeaderText="C. %" UniqueName="cporcentajes">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
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
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="height: 5px">
                        <asp:Button ID="btnAddClient" runat="server" CausesValidation="False" CssClass="item" TabIndex="6" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
        <asp:Panel ID="panelClientRegistration" runat="server" Visible="False">
            <br />
            <fieldset>
                <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Simple" MultiPageID="RadMultiPage1" SelectedIndex="0" CausesValidation="False">
                    <Tabs>
                        <telerik:RadTab Text="Datos Generales" TabIndex="0" Value="0" Enabled="True" Selected="true">
                        </telerik:RadTab>
                        <telerik:RadTab Text="Cuentas Bancarias" TabIndex="1" Value="1" Enabled="False">
                        </telerik:RadTab>
                    </Tabs>
                </telerik:RadTabStrip>
                <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Height="100%" Width="100%">
                    <telerik:RadPageView ID="RadPageView1" runat="server" Width="100%" Selected="true">
                        <br />
                        <table width="100%" border="0">
                            <tr>
                                <td colspan="3" style="width: 100%; background-color: GrayText; color: White; font-family: Arial; padding-left: 10px; height: 25px;">
                                    <asp:Label ID="lblSeccion1" runat="server" Text="Datos de facturación"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblComercialName" runat="server" CssClass="item" Font-Bold="True" Text="Nombre comercial:"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="Label6" runat="server" CssClass="item" Font-Bold="True" Text="Denominación/Razón Social:"/>
                                </td>
                                <td>
                                    <asp:Label ID="lblEstatusCliente" runat="server" Text="Estatus:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtNombreComercial" runat="server" Width="85%"></telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtDenominacionRaznScial" runat="server" Width="85%"></telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <asp:DropDownList ID="estatusid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>                                
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="valNombreCOmercial" runat="server" ValidationGroup="Grupo1" ControlToValidate="txtNombreComercial" CssClass="error" SetFocusOnError="true" ErrorMessage="Requerido"></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="Grupo1" ControlToValidate="txtDenominacionRaznScial" CssClass="error" SetFocusOnError="true" ErrorMessage="Requerido"></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="valEstatus" runat="server" ValidationGroup="Grupo1" ControlToValidate="estatusid" InitialValue="0" CssClass="error" SetFocusOnError="true" ErrorMessage="Requerido"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblSocialReason" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">
                                    <asp:Label ID="lblPriceType" runat="server" CssClass="item" Font-Bold="true" Text="Tipo de precio:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%" valign="top" colspan="2" style="width: 66%">
                                    <telerik:RadTextBox ID="txtSocialReason" runat="server" Width="92%">
                                    </telerik:RadTextBox>
                                </td>
                                <td>
                                    <asp:DropDownList ID="tipoprecioid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="valRazonsocial" runat="server" ValidationGroup="Grupo1" ControlToValidate="txtSocialReason" SetFocusOnError="true" CssClass="error" ErrorMessage="Requerido"></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblContact" runat="server" CssClass="item" Text="Contacto de Compras:" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblContactPhone" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtContact" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtContactPhone" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblContactEmail" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblCopia" runat="server" Text="Copia:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblCopiaOcilta" runat="server" Text="Copia Oculta:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtContactEmail" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtCC" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtCCO" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Formato no válido"
                                        ControlToValidate="txtContactEmail" CssClass="error"
                                        ValidationExpression=".*@.*\..*" ValidationGroup="Grupo1"></asp:RegularExpressionValidator>
                                </td>
                                <td width="33%">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Formato no válido"
                                        ControlToValidate="txtCC" CssClass="error"
                                        ValidationExpression=".*@.*\..*" ValidationGroup="Grupo1"></asp:RegularExpressionValidator>
                                </td>
                                <td width="33%">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Formato no válido"
                                        ControlToValidate="txtCCO" CssClass="error"
                                        ValidationExpression=".*@.*\..*" ValidationGroup="Grupo1"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="Label3" runat="server" CssClass="item" Text="Contacto de Pagos:" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="Label4" runat="server" CssClass="item" Text="Email del Contacto:" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="Label5" runat="server" CssClass="item" Text="Teléfono del Contacto:" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtContactoPagos" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEmailContactoPagos" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtTelefonoContactoPagos" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtEmailContactoPagos" CssClass="error" SetFocusOnError="true" ValidationExpression=".*@.*\..*" ValidationGroup="Grupo1"></asp:RegularExpressionValidator>
                                </td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%" class="style5"></td>
                                <td width="33%" class="style5"></td>
                                <td width="33%" class="style5"></td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblStreet" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%" align="left">
                                    <asp:Label ID="lblExtNumber" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblIntNumber" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left" width="33%">
                                    <asp:Label ID="lblColony" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtStreet" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtExtNumber" runat="server" Width="35%">
                                    </telerik:RadTextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <telerik:RadTextBox ID="txtIntNumber" runat="server" Width="35%">
                                    </telerik:RadTextBox>
                                </td>
                                <td align="left" width="33%">
                                    <telerik:RadTextBox ID="txtColony" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblCountry" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblState" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblTownship" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtCountry" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <asp:DropDownList ID="dropEstado" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtTownship" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblZipCode" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblRFC" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblCondiciones" runat="server" CssClass="item" Font-Bold="true" Text="Condiciones:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtZipCode" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtRFC" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>

                                <td>
                                    <asp:DropDownList ID="condicionesid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>

                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="reqRFC" runat="server" ControlToValidate="txtRFC" CssClass="error" ValidationGroup="Grupo1" ErrorMessage="Requerido" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="valRFC" CssClass="error" runat="server" ControlToValidate="txtRFC" SetFocusOnError="True" ValidationExpression="^([a-zA-Z,&]{3,4})\d{6}([a-zA-Z\w]{3})$" ValidationGroup="Grupo1"></asp:RegularExpressionValidator>
                                </td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblContribuyente" runat="server" Text="Tipo de contribuyente / honorarios:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                     <asp:Label ID="lblRegimen" runat="server" CssClass="item" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                   <asp:Label ID="lblFormaPago" runat="server" CssClass="item" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DropDownList ID="tipoContribuyenteid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="regimenid" CssClass="box" Width="85%" runat="server"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:DropDownList ID="formapagoid" runat="server" Width="85%" CssClass="box"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="valTipoContribuyente" runat="server" ValidationGroup="Grupo1" InitialValue="0" SetFocusOnError="true" ForeColor="Red" ControlToValidate="tipoContribuyenteid" ErrorMessage="Requerido" CssClass="item"></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">
                                   <asp:RequiredFieldValidator ID="valRegimen" runat="server" InitialValue="0" ControlToValidate="regimenid" CssClass="item" ValidationGroup="Grupo1" SetFocusOnError="true" ForeColor="Red" ErrorMessage="Requerido" ></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="valFormaPago" runat="server" ValidationGroup="Grupo1" InitialValue="0" Text="Requerido" SetFocusOnError="true" ForeColor="Red" ControlToValidate="formapagoid" ErrorMessage="Requerido" CssClass="item"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNumCtaPago" runat="server" CssClass="item" Font-Bold="true"></asp:Label>
                                </td>
                                <td>
                                   
                                     <asp:Label ID="lblLimiteCredito" runat="server" Text="Límite de Crédito:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td>
                                     <asp:Label ID="lblDescuento" runat="server" Text="Descuento:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtNumCtaPago" runat="server" Width="55%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadNumericTextBox ID="txtLimiteCredito" NumberFormat-DecimalDigits="2" NumberFormat-GroupSeparator="," Type="Currency" Value="0" MinValue="0" runat="server" Width="30%">
                                    </telerik:RadNumericTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadNumericTextBox ID="txtDescuento" NumberFormat-DecimalDigits="4" NumberFormat-GroupSeparator="" Type="Percent" Value="0" MinValue="0" runat="server" Width="30%">
                                    </telerik:RadNumericTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="valLimiteCredito" runat="server" ValidationGroup="Grupo1" InitialValue="0" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtLimiteCredito" ErrorMessage="Requerido" CssClass="item"></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                             <tr>
                                <td width="33%">
                                    <asp:Label ID="lblEjecutivo" runat="server" Text="Ejecutivo que lo atiende:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblCoordinadorCuenta" runat="server" Text="Coordinador de Cuenta:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblCuentaConcentradora" runat="server" CssClass="item" Font-Bold="true" Text="Cuenta concentradora:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                        <asp:DropDownList ID="ejecutivoid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td width="33%">
                                    <asp:DropDownList ID="cmbCoordinadorC" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td width="33%">
                                    <asp:DropDownList ID="cuentaconcentradoraid" CssClass="box" Width="69%" runat="server"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="Grupo1" InitialValue="0" SetFocusOnError="true" ForeColor="Red" ControlToValidate="ejecutivoid" ErrorMessage="Requerido" CssClass="item"></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ValidationGroup="Grupo1" InitialValue="0" SetFocusOnError="true" ForeColor="Red" ControlToValidate="cmbCoordinadorC" ErrorMessage="Requerido" CssClass="item"></asp:RequiredFieldValidator>
                                </td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblComisionEjecutivo" runat="server" Text="Comisión Ejecutivo:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblComisiónCoordinador" runat="server" Text="Comisión Coordinador:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblChkFacAutomaticas" runat="server" Text="Facturas Automaticas" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadNumericTextBox ID="txtComisionEjecutivo" NumberFormat-DecimalDigits="4" NumberFormat-GroupSeparator="" Type="Percent" Value="0" MinValue="-1" runat="server" Width="30%">
                                    </telerik:RadNumericTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadNumericTextBox ID="txtComisionCoordinador" NumberFormat-DecimalDigits="4" NumberFormat-GroupSeparator="" Type="Percent" Value="0" MinValue="-1" runat="server" Width="30%">
                                    </telerik:RadNumericTextBox>
                                </td>
                                <td width="33%">
                                    <asp:CheckBox runat="server" ID="chkFacAutomaticas" /> 
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ValidationGroup="Grupo1" InitialValue="0" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtComisionEjecutivo" ErrorMessage="Requerido" CssClass="item"></asp:RequiredFieldValidator>--%>

                                </td>
                                <td width="33%">
                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ValidationGroup="Grupo1" InitialValue="0" SetFocusOnError="true" ForeColor="Red" ControlToValidate="txtComisionCoordinador" ErrorMessage="Requerido" CssClass="item"></asp:RequiredFieldValidator>--%>

                                </td>
                                <td width="33%">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="item" style="width: 20%">
                                    <strong>Método de pago:</strong>&nbsp;<asp:RequiredFieldValidator ID="valMetodoPago" runat="server" InitialValue="0" ErrorMessage="Requerido" ControlToValidate="cmbMetodoPago" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </td>
                                <td width="66%" colspan="2">
                                    <asp:Label ID="lblUsoCFDI" runat="server" CssClass="item" Font-Bold="True" Text="Uso de CFDI:"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="item" style="width: 20%">
                                    <asp:DropDownList ID="cmbMetodoPago" runat="server" CssClass="box" Width="80%"></asp:DropDownList>
                                </td>
                                <td colspan="2" width="66%">
                                    <asp:DropDownList ID="cmbUsoCFD" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <%--<td colspan="1">
                                    <strong class="item">Exportación</strong>&nbsp;<asp:RequiredFieldValidator ID="valExportacion" runat="server" InitialValue="0" ErrorMessage="Requerido" CssClass="item" ControlToValidate="cmbExportacion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                </td>--%>
                                <td></td>
                                <%--<td><asp:RequiredFieldValidator ID="valProyecto" runat="server" ControlToValidate="cmbProyecto" InitialValue="0" ErrorMessage="Requerido" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator><br /></td>--%>
                            </tr>
                            <tr>
                                <td class="item" style="width: 20%">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="item" style="width: 20%">&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="panelSucursal" runat="server" Visible="False">
                                        <br />
                                        <fieldset>
                                            <legend style="padding-right: 6px; color: Black">
                                                <asp:Label ID="Label2" runat="server" Text="Sucursales asignadas a cliente:" Font-Bold="True" CssClass="item"></asp:Label>
                                            </legend>
                                            <table border="0" cellpadding="2" cellspacing="0" width="50%">
                                                <tr>
                                                    <td style="width: 80%">
                                                        <asp:Label ID="lblAgenteVentas" runat="server" Text="Agente de ventas:" CssClass="item" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td style="width: 20%">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80%">
                                                        <asp:DropDownList ID="vendedorid" runat="server" CssClass="box" Width="200px"></asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" InitialValue="0" SetFocusOnError="true" ControlToValidate="vendedorid" ErrorMessage="Requerido" ValidationGroup="gvsucursal" ForeColor="Red" CssClass="item"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td style="width: 20%">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 80%">
                                                        <asp:Label ID="lblSucursal" runat="server" CssClass="item" Text="Sucursal:" Font-Bold="True"></asp:Label>
                                                    </td>
                                                    <td style="width: 20%">&nbsp;</td>
                                                </tr>
                                                <tr valign="top">
                                                    <td style="width: 80%">
                                                        <telerik:RadTextBox ID="txtSucursal" runat="server" Width="300px"></telerik:RadTextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" SetFocusOnError="true" ControlToValidate="txtSucursal" ErrorMessage="Requerido" ValidationGroup="gvsucursal" ForeColor="Red" CssClass="item"></asp:RequiredFieldValidator>
                                                    </td>
                                                    <td align="right">
                                                        <asp:Button ID="btnGuardarSucursal" Text="Guardar Sucursal" ValidationGroup="gvsucursal" runat="server" CssClass="item" />
                                                        <br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2">&nbsp;</td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <telerik:RadGrid ID="grdSucursal"
                                                            runat="server"
                                                            AllowPaging="True"
                                                            AutoGenerateColumns="False"
                                                            GridLines="None"
                                                            PageSize="15"
                                                            ShowStatusBar="True"
                                                            Skin="Simple"
                                                            Width="100%">
                                                            <PagerStyle Mode="NumericPages" />
                                                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Sucursales" Width="100%">
                                                                <Columns>
                                                                    <telerik:GridBoundColumn DataField="id" HeaderText="Id" Visible="false" UniqueName="id"></telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Sucursal" UniqueName="Sucursal">
                                                                        <ItemTemplate>
                                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("Sucursal") %>' CausesValidation="false"></asp:LinkButton>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="Left" />
                                                                    </telerik:GridTemplateColumn>

                                                                    <telerik:GridBoundColumn DataField="AgenteDeVenta" HeaderText="Agente de venta" UniqueName="AgenteDeVenta"></telerik:GridBoundColumn>

                                                                    <telerik:GridTemplateColumn AllowFiltering="False"
                                                                        HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="btnDelete" runat="server"
                                                                                CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete"
                                                                                ImageUrl="~/images/action_delete.gif" />
                                                                        </ItemTemplate>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </telerik:GridTemplateColumn>
                                                                </Columns>
                                                            </MasterTableView>
                                                        </telerik:RadGrid>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <br />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="width: 100%; background-color: GrayText; color: White; font-family: Arial; padding-left: 10px; height: 25px;">
                                    <asp:Label ID="lblDatosEnvio" runat="server" Text="Datos de envío"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblEnvioCalle" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%" align="left">
                                    <asp:Label ID="lblEnvioNoExt" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblEnvioNoInt" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td align="left" width="33%">
                                    <asp:Label ID="lblEnvioColonia" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEnvioCalle" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEnvioNoExt" runat="server" Width="35%">
                                    </telerik:RadTextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <telerik:RadTextBox ID="txtEnvioNoInt" runat="server" Width="35%">
                            </telerik:RadTextBox>
                                </td>
                                <td align="left" width="33%">
                                    <telerik:RadTextBox ID="txtEnvioColonia" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblEnvioEstado" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblEnvioMunicipio" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblEnvioCP" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:DropDownList ID="dropEnvEstado" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEnvioMunicipio" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEnvioCP" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblEnvioNombre" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblEnvioEmail" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                                <td width="33%">
                                    <asp:Label ID="lblEnvioTelefono" runat="server" Text="Teléfono:" CssClass="item" Font-Bold="True"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEnvioNombre" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEnvioEmail" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td width="33%">
                                    <telerik:RadTextBox ID="txtEnvioTelefono" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td width="33%">
                                    <asp:Label ID="lblObservaciones" runat="server" CssClass="item" Font-Bold="True" Text="Observaciones:"></asp:Label>
                                </td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <telerik:RadTextBox ID="txtObservaciones" TextMode="MultiLine" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                                <td width="33%">&nbsp;</td>
                            </tr>
                            <tr>
                                <td valign="bottom" colspan="3">
                                    <asp:Button ID="btnSaveClient" runat="server" CssClass="item" CausesValidation="true" ValidationGroup="Grupo1" />&nbsp;
                                    <asp:Button ID="btnCancel" runat="server" CssClass="item" CausesValidation="False" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                                    <asp:HiddenField ID="ClientsID" runat="server" Value="0" />
                                    <asp:HiddenField ID="SucursalID" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPageView>
                    <telerik:RadPageView ID="RadPageView2" runat="server">
                        <br />
                        <table border="0" style="width: 100%;">
                            <tr>
                                <td colspan="7" style="width: 100%; background-color: GrayText; color: White; font-family: Arial; padding-left: 10px; height: 25px;">
                                    <asp:Label ID="Label1" runat="server" Text="Agregar / Editar Cuentas Bancarias"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="7">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblBanco" runat="server" CssClass="item" Font-Bold="True" Text="Banco Nacional:"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblBancoExtr" runat="server" CssClass="item" Font-Bold="True" Text="Banco Extranjero:"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblRfc1" runat="server" CssClass="item" Font-Bold="True" Text="RFC:"></asp:Label>
                                </td>
                                <td style="width: 10%;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 30%;">
                                    <telerik:RadTextBox ID="txtBanco" runat="server" Width="95%">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 30%;">
                                    <telerik:RadTextBox ID="txtBancoExtr" runat="server" Width="95%">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 30%;">
                                    <telerik:RadTextBox ID="txtRFCBAK" runat="server" Width="85%">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 10%;"></td>
                            </tr>
                            <tr>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 30%;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ValidationGroup="vDatosCuenta" ControlToValidate="txtRFCBAK" SetFocusOnError="True" CssClass="item" Text="Requerido" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator5" CssClass="item" runat="server" ValidationGroup="vDatosCuenta" ControlToValidate="txtRFCBAK" SetFocusOnError="True" ValidationExpression="^([a-zA-Z&]{3,4})\d{6}([a-zA-Z\w]{3})$" ForeColor="Red"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 30%;">
                                    <asp:Label ID="lblNonCuenta" runat="server" CssClass="item" Font-Bold="True" Text="Número de Cuenta:"></asp:Label>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="Label7" runat="server" CssClass="item" Font-Bold="True" Text="Predeterminado:"></asp:Label>
                                </td>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 10%;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 30%;">
                                    <telerik:RadTextBox ID="txtCuenta" runat="server" Width="96%">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 30%;">
                                    <asp:CheckBox runat="server" ID="chkPredeterminado" />
                                </td>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 10%;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 30%;">
                                    <asp:RequiredFieldValidator ID="valCuenta" runat="server" SetFocusOnError="true" ControlToValidate="txtCuenta" ValidationGroup="vDatosCuenta" Text="Requerido" ForeColor="Red" CssClass="item"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 30%;">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 30%;">&nbsp;</td>
                                <td style="width: 30%;">
                                    <asp:Button ID="btnGuardar" runat="server" ValidationGroup="vDatosCuenta" Text="Guardar" />&nbsp;&nbsp;
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" style="width: 66%; height: 5px;">
                                    <asp:HiddenField ID="CuentaID" runat="server" Value="0" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <fieldset>
                            <legend style="padding-right: 6px; color: Black">
                                <asp:Label ID="lblSucursalesListLegend" runat="server" Text="Listado de Cuentas Bancarias" Font-Bold="true" CssClass="item"></asp:Label>
                            </legend>
                            <table border="0" style="width: 100%;">
                                <tr>
                                    <td style="height: 5px">
                                        <telerik:RadGrid ID="cuentasList" runat="server" AllowPaging="True"
                                            AutoGenerateColumns="False" GridLines="None"
                                            PageSize="20" ShowStatusBar="True"
                                            Skin="Simple" Width="100%">
                                            <PagerStyle Mode="NumericPages" />
                                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Cuentas" NoMasterRecordsText="No se encontraron datos para mostrar" Width="100%">
                                                <Columns>
                                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Folio" UniqueName="id">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("id") %>' CausesValidation="false"></asp:LinkButton>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridBoundColumn DataField="banconacional" HeaderText="Banco Nacional" UniqueName="banconacional">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="bancoextranjero" HeaderText="Banco Extranjero" UniqueName="bancoextranjero">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="rfc" HeaderText="RFC" UniqueName="rfc">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="numctapago" HeaderText="Cuenta Bancaria" UniqueName="numctapago">
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Predeterminado" UniqueName="">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgPredeterminado" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icons/arrow.gif" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
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
                                    <td>&nbsp;</td>
                                </tr>
                            </table>
                        </fieldset>
                    </telerik:RadPageView>
                </telerik:RadMultiPage>
            </fieldset>
        </asp:Panel>
    </telerik:RadAjaxPanel>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>

</asp:Content>