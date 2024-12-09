<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="almacenes.aspx.vb" Inherits="erp_neogenis.almacenes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblAlmacenes" runat="server" Font-Bold="true" CssClass="item" Text="Almacenes"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="almacenesList" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" GridLines="None" 
                            PageSize="15" ShowStatusBar="True" 
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" 
                                Name="Almacenes" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Folio" 
                                        UniqueName="id"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Nombre" 
                                        UniqueName="nombre">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' 
                                                CommandName="cmdEdit" Text='<%# Eval("nombre") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
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
                        <asp:Button ID="btnAddWarehouse" runat="server" CausesValidation="False" 
                            CssClass="item" Text="Agregar" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                        <asp:HiddenField ID="almacenid" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
        </fieldset>

    
        
    <br />

    <asp:Panel ID="panelWareHouseDetail" runat="server" Visible="False">
        <fieldset>
            <table width="100%">
                <tr>    
                    <td colspan="3" style="width:100%; background-color:GrayText; color:White; font-family:Arial; padding-left:10px; height:25px;">
                        <asp:Label id="lblSeccion1" runat="server" Text="Datos Generales"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="item">
                        Nombre:
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtNombre" Runat="server" Width="500px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:Button ID="btnSaveWareHouse" Text="Guardar" runat="server" CssClass="item" />&nbsp;
                        <asp:Button ID="btnCancel" Text="Cancelar" runat="server" CssClass="item" CausesValidation="false" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>

    </telerik:RadAjaxPanel>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
