<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="Departamentos.aspx.vb" Inherits="erp_neogenis.Departamentos" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
    <fieldset>
        <legend style="padding-right:6px; color:Black">
            <asp:Label ID="lblDepartamentosList" runat="server" Text="Lista de Departamentos" Font-Bold="true" CssClass="item"></asp:Label>
        </legend>
        <table width="100%" border="0">
            <tr>
                <td style="height:2px">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="grdDepartamentos" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" GridLines="None" AllowSorting="true"
                        PageSize="15" ShowStatusBar="True" 
                        Skin="Simple" Width="50%">
                        <PagerStyle Mode="NextPrevAndNumeric" />
                        <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Departamentos" Width="100%">
                            <Columns>
                                <telerik:GridBoundColumn DataField="id" HeaderText="Id" UniqueName="id"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Nombre" DataField="nombre" SortExpression="nombre" UniqueName="nombre">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("nombre") %>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridTemplateColumn>
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
                <td align="right">
                    <asp:Button ID="btnAgregaDepto" runat="server" Text="Agrega Departamento" CausesValidation="False" CssClass="item" />
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <asp:Panel ID="panelRegistroDepto" runat="server" Visible="False">
        <fieldset>
            <legend style="padding-right:6px; color:Black">
                <asp:Label ID="lblRegistroDeptoTitle" runat="server" Text="Agregar/Editar Departamento" Font-Bold="True" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table width="100%" border="0">
                <tr valign="top">
                    <td width="25%">
                        <asp:Label ID="lblNombre" runat="server" CssClass="item" Font-Bold="True" Text="Nombre:"></asp:Label>
                    </td>
                    <td width="75%">
                        &nbsp;
                    </td>
                </tr>
                <tr valign="top">
                    <td width="25%">
                        <asp:TextBox ID="txtNombre" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtNombre" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td width="75%">
                        <asp:Button ID="btnGuardar" Text="Guardar" runat="server" CssClass="item" />&nbsp;<asp:Button ID="btnCancelar" CausesValidation="false" Text="Cancelar" runat="server" CssClass="item" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                        <asp:HiddenField ID="departamentoID" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
