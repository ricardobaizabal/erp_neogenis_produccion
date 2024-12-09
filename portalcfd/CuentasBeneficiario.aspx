<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" CodeBehind="CuentasBeneficiario.aspx.vb" Inherits="erp_neogenis.CuentasBeneficiario" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/ListaProveedores_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblUsersListLegend" runat="server" Font-Bold="true" CssClass="item">Listado de Cuentas de Beneficiario</asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="Cuentaslist" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None" PageSize="15" ShowStatusBar="True"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id"
                                Name="Cuenta" Width="100%">
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Folio" HeaderStyle-Font-Bold="true" UniqueName="nombre">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("id") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="banco" HeaderText="Banco" HeaderStyle-Font-Bold="true" UniqueName="banco">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="numctapago" HeaderText="Cuenta Bancaria" UniqueName="numctapago" HeaderStyle-Font-Bold="true">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="rfc" HeaderText="RFC" UniqueName="rfc" HeaderStyle-Font-Bold="true">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Predeterminado" UniqueName="predeterminado" HeaderStyle-Font-Bold="true">
                                        <ItemTemplate>
                                            <asp:Image ID="imgPredeterminado" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icons/arrow.gif" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-Font-Bold="true"
                                        HeaderStyle-HorizontalAlign="Center" UniqueName="Delete" HeaderText="Eliminar">
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
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="height: 5px">
                        <asp:Button ID="btnAdd" runat="server" Text="Agrega Cuenta Bancaria" CausesValidation="False" CssClass="item" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px"></td>
                </tr>
            </table>
        </fieldset>
        <br />
        <asp:Panel ID="panelRegistration" runat="server" Visible="False">
            <fieldset>
                <legend style="padding-right: 6px; color: Black">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/AgreEditUsuario_03.jpg" ImageAlign="AbsMiddle" />&nbsp;
                    <asp:Label ID="lblUserEditLegend" runat="server" Font-Bold="True" CssClass="item">Listado de Cuentas de Beneficiario</asp:Label>
                </legend>
                <br />
                <table style="width: 100%;" border="0">
                    <tr>
                        <td colspan="3">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 33%;">
                            <asp:Label ID="lblBanco" runat="server" CssClass="item" Font-Bold="True" Text="Banco:"></asp:Label>
                        </td>
                        <td style="width: 33%;">
                            <asp:Label ID="lblNonCuenta" runat="server" CssClass="item" Font-Bold="True" Text="Número de Cuenta:"></asp:Label>
                        </td>
                        <td style="width: 33%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 33%;">
                            <telerik:RadTextBox ID="txtBanco" runat="server" Width="95%">
                            </telerik:RadTextBox>
                        </td>
                        <td style="width: 33%;">
                            <telerik:RadTextBox ID="txtCuenta" runat="server" Width="96%">
                            </telerik:RadTextBox>
                        </td>
                        <td style="width: 33%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 33%;">&nbsp;</td>
                        <td style="width: 33%;">
                            <asp:RequiredFieldValidator ID="valCuenta" runat="server" SetFocusOnError="true" ControlToValidate="txtCuenta" ValidationGroup="vDatosCuenta" CssClass="item" ErrorMessage="Requerido" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                        <td style="width: 33%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 33%;">
                            <asp:Label ID="Label1" runat="server" CssClass="item" Font-Bold="True" Text="RFC:"></asp:Label>
                        </td>
                        <td style="width: 33%;">&nbsp;</td>
                        <td style="width: 33%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 33%;">
                            <telerik:RadTextBox ID="txtRFC" runat="server" Width="95%">
                            </telerik:RadTextBox>
                        </td>
                        <td style="width: 33%;">
                            <asp:CheckBox runat="server" ID="chkPredeterminado" CssClass="item" Text="Predeterminado" />
                        </td>
                        <td style="width: 33%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td style="width: 33%;">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ValidationGroup="vDatosCuenta" ControlToValidate="txtRFC" SetFocusOnError="true" ForeColor="Red" CssClass="item" ErrorMessage="Requerido"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" CssClass="item" runat="server" ControlToValidate="txtRFC" SetFocusOnError="True" ForeColor="Red" ValidationExpression="^([a-zA-Z&]{3,4})\d{6}([a-zA-Z\w]{3})$"></asp:RegularExpressionValidator>
                        </td>
                        <td style="width: 33%;">&nbsp;</td>
                        <td style="width: 33%;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 5px;">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 5px;">
                            <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                            <asp:HiddenField ID="CuentaID" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" align="right">
                            <asp:Button ID="btnGuardar" Text="Guardar" runat="server" CssClass="item" ValidationGroup="vDatosCuenta" />&nbsp;<asp:Button ID="btnCancelar" CausesValidation="false" Text="Cancelar" runat="server" CssClass="item" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
