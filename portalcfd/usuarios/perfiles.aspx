<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="perfiles.aspx.vb" Inherits="erp_neogenis.perfiles" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style4
        {
            height: 17px;
        }
        .style5
        {
            height: 14px;
        }
        .style6
        {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblUsersListLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="perfilesList" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" GridLines="None" 
                            PageSize="15" ShowStatusBar="True" 
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" 
                                Name="Users" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Id" 
                                        UniqueName="id"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre" HeaderText="" 
                                        UniqueName="nombre">
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridTemplateColumn HeaderText="Clientes">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkClientesBit" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Proveedores">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkProveedoresBit" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Inventario">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkInventarioBit" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Facturación">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkFacturacionBit" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Pedidos">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkPedidosBit" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Reportes">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkReportesBit" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Configuración">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkConfiguracionBit" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    
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
                <tr>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="height: 5px">
                        <asp:Button ID="btnSave" runat="server" CausesValidation="False" CssClass="item" Text="Guardar" />&nbsp;&nbsp;
                        <asp:Button ID="btnAddProfile" runat="server" CausesValidation="False" CssClass="item" Text="Agregar nuevo perfil" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                    </td>
                </tr>
            </table>
        </fieldset>
        
        <br />
        
        <asp:Panel ID="panelUserRegistration" runat="server" Visible="False">

        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblUserEditLegend" runat="server" Font-Bold="True" 
                    CssClass="item"></asp:Label>
            </legend>

            <br />

            <table width="100%">
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblNombre" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        <asp:Label id="lblPerfil" runat="server" CssClass="item" Font-Bold ="true" Text="Tipo de precio:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="33%" valign="top" colspan="2" style="width: 66%">
                        <telerik:RadTextBox ID="txtNombre" Runat="server" Width="92%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="perfilid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="33%" colspan="2">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtNombre" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" 
                            ControlToValidate="perfilid" InitialValue="0" CssClass="item"></asp:RequiredFieldValidator></td>
                </tr>
                <tr>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style4" width="33%">
                        <asp:Label ID="lblEmail" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="33%">
                        <asp:Label ID="lblContrasena" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="33%">
                        
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="33%">
                        <telerik:RadTextBox ID="txtEmail" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td class="style4" width="33%">
                        <telerik:RadTextBox ID="txtContrasena" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td class="style4" width="33%">
                        
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="33%">
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                            ControlToValidate="txtEmail" CssClass="item" 
                            ValidationExpression=".*@.*\..*"></asp:RegularExpressionValidator>
                    </td>
                    <td class="style4" width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" 
                            ControlToValidate="txtContrasena" CssClass="item" />
                    </td>
                    <td class="style4" width="33%">
                    </td>
                </tr>
                <tr>
                    <td width="33%" class="style5">
                        </td>
                    <td width="33%" class="style5">
                        </td>
                    <td width="33%" class="style5">
                        </td>
                </tr>
                
                <tr>
                    <td valign="bottom" colspan="3">
                        <asp:Button ID="btnSaveUser" runat="server" CssClass="item" />&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CssClass="item" 
                            CausesValidation="False" />
                        
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 66%; height: 5px;">
                        <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                        <asp:HiddenField ID="UsersID" runat="server" Value="0" />
                    </td>
                </tr>
            </table>

        </fieldset>

    </asp:Panel>
    
    </telerik:RadAjaxPanel>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
