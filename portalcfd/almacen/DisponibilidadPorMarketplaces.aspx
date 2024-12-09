<%@ Page Language="vb" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" CodeBehind="DisponibilidadPorMarketplaces.aspx.vb" Inherits="erp_neogenis.DisponibilidadPorMarketplaces" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript">
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/ListaProveedores_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblUsersListLegend" runat="server" Font-Bold="true" CssClass="item">Disponibilidad Por Marketplaces</asp:Label>
            </legend>

            <table style="width: 100%;" border="0">
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td colspan="1">
                        <div style="width:70%;display:inline-flex;">
                            <div style="width:20%;">
                                <asp:Label ID="lblMarketplace" runat="server" CssClass="item" Font-Bold="True" Text="Marketplaces:"></asp:Label><br />
                                <asp:DropDownList ID="cmbMarketplaces" runat="server" Width="95%" CssClass="box" DataValueField="id" DataTextField="nombre" Style="margin-top:10px"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                            </div>
                            <div style="width:20%;">
                                <asp:Label ID="Label1" runat="server" CssClass="item" Font-Bold="True" Text="Marca:"></asp:Label><br />
                                <asp:DropDownList ID="cmbMarca" runat="server" Width="95%" CssClass="box" DataValueField="id" DataTextField="nombre" Style="margin-top:10px"></asp:DropDownList>&nbsp;&nbsp;&nbsp;
                            </div>
                            <div style="width:20%;">
                                <asp:Label ID="Label2" runat="server" CssClass="item" Font-Bold="True" Text="Sku:"></asp:Label><br />
                                <telerik:RadTextBox ID="txtsku" runat="server" Width="95%" Style="margin-top:6px"></telerik:RadTextBox>
                            </div>
                            <div>
                                <asp:Button ID="btnVer" Text="Ver" runat="server" CssClass="item" Style="margin-top:18px" Width="60px" />
                            </div>
                            
                        </div>
                     </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="MarketplaceList" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" GridLines="None" ShowFooter="True"
                            PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%" >
                            <PagerStyle Mode="NumericPages" />
                            <ExportSettings IgnorePaging="True" FileName="DisponibilidadPorMarketplaces" OpenInNewWindow="true">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <MasterTableView AllowMultiColumnSorting="False"  Name="Marcas" Width="100%" CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="id" HeaderStyle-Font-Size="Small"/>
                                    <telerik:GridBoundColumn DataField="upc" HeaderText="UPC" UniqueName="upc" HeaderStyle-Font-Size="Small"/>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripcion" UniqueName="id" HeaderStyle-Font-Size="Small"/>
                                    <telerik:GridBoundColumn DataField="disponibles" HeaderText="Disponibles" UniqueName="id" HeaderStyle-Font-Size="Small"/>
                                    <telerik:GridBoundColumn DataField="prioridad" HeaderText="Prioridad" UniqueName="id" HeaderStyle-Font-Size="Small"/>
                                    <telerik:GridBoundColumn DataField="porcentaje" HeaderText="Porcentaje" UniqueName="porcentaje" HeaderStyle-Font-Size="Small"/> 
                                    <telerik:GridBoundColumn DataField="asignados" HeaderText="Asignados" UniqueName="asigandos" HeaderStyle-Font-Size="Small"/> 
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 2px">&nbsp;</td>
                </tr>
            </table>

        </fieldset>
        <br />
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>