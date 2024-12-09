<%@ Page Language="vb" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" CodeBehind="Marketplace.aspx.vb" Inherits="erp_neogenis.Marketplace" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/ListaProveedores_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblUsersListLegend" runat="server" Font-Bold="true" CssClass="item">Listado de Marketplaces</asp:Label>
            </legend>
            <table style="width: 50%;" border="0">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="MarketplaceList" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None" PageSize="15" ShowStatusBar="True"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Marcas" Width="100%">
                                <Columns>
                                    <%--<telerik:GridBoundColumn DataField="id" HeaderText="Folio" HeaderStyle-Font-Bold="true" UniqueName="id">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Nombre" HeaderStyle-Font-Bold="true" UniqueName="nombre">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("nombre") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                     <telerik:GridBoundColumn DataField="prioridad" HeaderText="Prioridad" UniqueName="id" HeaderStyle-Font-Size="Small"/>
                                    <telerik:GridBoundColumn DataField="porcentaje" HeaderText="Porcentaje" UniqueName="porcentaje" HeaderStyle-Font-Size="Small"/>                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete" HeaderText="Eliminar" Visible="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server"
                                                CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete"
                                                ImageUrl="~/images/action_delete.gif" OnClientClick="return confirm('Advertencia: este registro podria estar ligado a uno o más registros, ¿Desea continuar?');" />
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
                        <asp:Button ID="btnAdd" runat="server" Text="Agregar Nuevo" CausesValidation="False" CssClass="item" />
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
                        <td colspan="1">
                            <asp:Label ID="lblNombre" runat="server" CssClass="item" Font-Bold="True" Text="Nombre:"></asp:Label>
                             <asp:RequiredFieldValidator ID="valNombreMarca" runat="server" ForeColor="Red" ControlToValidate="txtNombre" CssClass="item" Text="Requerido"></asp:RequiredFieldValidator><br />
                            <asp:TextBox ID="txtNombre" runat="server" Width="92%" CssClass="box"/>
                        </td>
                        
                        <td colspan="1">
                            <asp:Label ID="Label1" runat="server" CssClass="item" Font-Bold="True" Text="Prioridad:"></asp:Label>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" InitialValue="0" runat="server" ForeColor="Red" ControlToValidate="cmbPrioridades" CssClass="item" Text="Requerido"></asp:RequiredFieldValidator><br />
                            <asp:DropDownList ID="cmbPrioridades" runat="server" Width="95%" CssClass="box" DataValueField="id" DataTextField="nombre"></asp:DropDownList>                            
                        </td>
                        <td>                            
                            <br />
                            <asp:Button ID="btnGuardar" Text="Guardar" runat="server" CssClass="item" />&nbsp;<asp:Button ID="btnCancelar" CausesValidation="false" Text="Cancelar" runat="server" CssClass="item" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 5px;">
                            <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                            <asp:HiddenField ID="MarcaID" runat="server" Value="0" />
                        </td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>