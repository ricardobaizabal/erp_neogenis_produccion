<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="Percepciones.aspx.vb" Inherits="erp_neogenis.Percepciones" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
    <fieldset>
        <legend style="padding-right:6px; color:Black">
            <asp:Label ID="lblPercionesList" runat="server" Text="Lista de Percepciones" Font-Bold="true" CssClass="item"></asp:Label>
        </legend>
        <table width="100%" border="0">
            <tr>
                <td style="height:2px">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="grdPercepciones" runat="server" AllowPaging="True" 
                        AutoGenerateColumns="False" GridLines="None" AllowSorting="true" 
                        PageSize="15" ShowStatusBar="True" 
                        Skin="Simple" Width="50%">
                        <PagerStyle Mode="NextPrevAndNumeric" />
                        <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Percepciones" Width="100%">
                            <Columns>
                                <telerik:GridBoundColumn DataField="id" HeaderText="Id" UniqueName="id"></telerik:GridBoundColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Clave" DataField="clave" SortExpression="clave" UniqueName="clave">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("clave") %>' CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="tipopercepcion" HeaderText="Tipo Percepción" UniqueName="tipopercepcion"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Exento" HeaderStyle-HorizontalAlign="Center" UniqueName="Exento">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExento" Visible="false" runat="server" Text='<%# Eval("exentoBit") %>'></asp:Label>
                                        <asp:CheckBox ID="exentoBit" Enabled="false" runat="server" Checked="false" CssClass="item" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
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
                    <asp:Button ID="btnAgregaPercepcion" runat="server" Text="Agrega Percepción" CausesValidation="False" CssClass="item" />
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <asp:Panel ID="panelRegistroPercepcion" runat="server" Visible="False">
        <fieldset>
            <legend style="padding-right:6px; color:Black">
                <asp:Label ID="lblRegistroPercepcionTitle" runat="server" Text="Agregar/Editar Percepción" Font-Bold="True" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table width="100%" border="0">
                <tr valign="top">
                    <td width="25%">
                        <asp:Label ID="lblTipoPercepcion" runat="server" CssClass="item" Font-Bold="True" Text="Tipo Percepción:"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:Label ID="lblClave" runat="server" CssClass="item" Font-Bold="True" Text="Clave:"></asp:Label>
                    </td>
                    <td width="35%">
                        <asp:Label ID="lblDescripcion" runat="server" CssClass="item" Font-Bold="True" Text="Descripción:"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:Label ID="lblExento" runat="server" CssClass="item" Font-Bold="True" Text="Exento:"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td width="25%">
                        <asp:DropDownList ID="ddlTipoPercepcion" runat="server" Width="200px"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlTipoPercepcion" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td width="25%">
                        <asp:TextBox ID="txtClave" CssClass="item" Width="200px" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtClave" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td width="35%">
                        <asp:TextBox ID="txtDescripcion" CssClass="item" Width="90%" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtDescripcion" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td width="15%">
                        <asp:CheckBox ID="exentoBit" runat="server" Checked="false" CssClass="item" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4" align="right">
                        <asp:Button ID="btnGuardar" Text="Guardar" runat="server" CssClass="item" />&nbsp;<asp:Button ID="btnCancelar" CausesValidation="false" Text="Cancelar" runat="server" CssClass="item" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="4">
                        <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                        <asp:HiddenField ID="percepcionID" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>