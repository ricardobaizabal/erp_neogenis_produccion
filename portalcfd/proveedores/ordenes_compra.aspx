<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="ordenes_compra.aspx.vb" Inherits="erp_neogenis.ordenes_compra" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblOrdenesCompra" Text="Ordenes de Compra" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table width="100%">
                <tr>
                    <td style="height: 2px"></td>
                </tr>
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="ordersList" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None"
                            PageSize="15" ShowStatusBar="True"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Orders" Width="100%">
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="No. Orden"
                                        UniqueName="id">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("clave") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha Est. Ent." UniqueName="fecha">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="proveedor" HeaderText="Proveedor" UniqueName="proveedor">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="productos" HeaderText="Cant. Prod." ItemStyle-HorizontalAlign="Right" UniqueName="productos">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="costo_variable" DataFormatString="{0:C}" HeaderText="Costo Variable" ItemStyle-HorizontalAlign="Right" UniqueName="costo_variable" Visible="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" ItemStyle-HorizontalAlign="Center" UniqueName="estatus">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete" Visible="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkRecibir" runat="server" CommandArgument='<%# eval("id") %>' CommandName="cmdReceive" Text="recibir"></asp:LinkButton>
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
                    <td style="height: 2px"></td>
                </tr>
                <tr>
                    <td align="right" style="height: 5px">
                        <asp:Button ID="btnAddOrder" Text="Agregar orden" runat="server" CausesValidation="False"
                            CssClass="item" TabIndex="6" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px"></td>
                </tr>
            </table>
        </fieldset>
        <br />
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
