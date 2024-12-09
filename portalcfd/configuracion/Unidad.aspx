<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" CodeBehind="Unidad.aspx.vb" Inherits="erp_neogenis.Unidad" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/ListaProveedores_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblUsersListLegend" runat="server" Font-Bold="true" CssClass="item">Listado de Unidades de Medida</asp:Label>
            </legend>
            <table style="width: 50%;" border="0">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="Unidadlist" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None" PageSize="15" ShowStatusBar="True"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Unidad" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Folio" HeaderStyle-Font-Bold="true" UniqueName="id">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Nombre" HeaderStyle-Font-Bold="true" UniqueName="nombre">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("nombre") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete" HeaderText="Eliminar">
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
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar Unidad" CausesValidation="False" CssClass="item" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
        <asp:Panel ID="panelRegistration" runat="server" Visible="False">
            <fieldset>
                <legend style="padding-right: 6px; color: Black">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/AgreEditUsuario_03.jpg" ImageAlign="AbsMiddle" />&nbsp;
                    <asp:Label ID="lblUserEditLegend" runat="server" Font-Bold="True" CssClass="item"></asp:Label>
                </legend>
                <br />
                <table style="width: 50%;" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblNombre" runat="server" CssClass="item" Font-Bold="True" Text="Nombre:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lblCLaveUnidadSAT" runat="server" CssClass="item" Font-Bold="True" Text="Clave Unidad de Medida SAT"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 35%">
                            <telerik:RadTextBox ID="txtNombre" runat="server" Width="65%">
                            </telerik:RadTextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ForeColor="Red" ControlToValidate="txtNombre" CssClass="item" Text="Requerido"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:DropDownList ID="claveUnidad" runat="server" CssClass="box" Width="100%"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" ControlToValidate="claveUnidad" CssClass="item" Text="Requerido" InitialValue="0"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td colspan="4" align="right">
                            <asp:Button ID="btnGuardar" Text="Guardar" runat="server" CssClass="item" />&nbsp;<asp:Button ID="btnCancelar" CausesValidation="false" Text="Cancelar" runat="server" CssClass="item" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 5px;">
                            <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                            <asp:HiddenField ID="UnidadID" runat="server" Value="0" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>