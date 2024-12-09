<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="Stocklocation.aspx.vb" Inherits="erp_neogenis.Stocklocation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .style4 {
            height: 25px;
        }
    </style>
    <script type="text/javascript">

        function OnRequestStart(target, arguments) {
            if ((arguments.get_eventTarget().indexOf("pedidosList") > -1) || (arguments.get_eventTarget().indexOf("btnExportExcel") > -1)) {
                arguments.set_enableAjax(false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Panel ID="panelProductList" runat="server">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/buscador_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblFiltros" Text="Buscador" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10%;">
                        <span class="item">Palabra clave:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox ID="txtSearch" runat="server" Width="92%" CssClass="box"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span class="item">Marca:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:DropDownList ID="filtromarcaid" runat="server" Width="95%" CssClass="box"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span class="item">UPC:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox ID="upcSearch" runat="server" Width="92%" CssClass="box"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span class="item">Temporada:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:DropDownList ID="filtrocoleccionid" runat="server" Width="95%" CssClass="box"></asp:DropDownList>
                    </td>
                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="boton" Text="Buscar" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAll" runat="server" CssClass="boton" Text="Ver todo" />&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <div>
                <br />
                <asp:CheckBox ID="ckfiltroAsignado" CssClass="item" runat="server" Text="Mostrar solamente productos Asignados." />
            </div>
        </fieldset>
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/ListadoProductos_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblProductsListLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid ID="productslist" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" GridLines="None" ShowFooter="True"
                            PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <ExportSettings IgnorePaging="True" FileName="ReporteStockLocation">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False" NoMasterRecordsText="No existen ubicaciones registradas" CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Codigo" UniqueName="codigo" ItemStyle-Width="20%" AllowFiltering="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="upc" HeaderText="UPC" UniqueName="upc" ItemStyle-Width="20%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" ItemStyle-Width="30%" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="proyecto" HeaderText="Marca" ItemStyle-Width="15%" UniqueName="proyecto">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="coleccion" HeaderText="Temporada" ItemStyle-Width="20%" UniqueName="coleccion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ubicacion" HeaderText="Ubicación" ItemStyle-Width="20%" UniqueName="ubicacion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%" UniqueName="cantidad">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                    <telerik:RadToolTip runat="server" ID="tooltip"></telerik:RadToolTip>
                </tr>
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
    </asp:Panel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
