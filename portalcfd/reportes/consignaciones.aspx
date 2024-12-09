<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="consignaciones.aspx.vb" Inherits="erp_neogenis.consignaciones1" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:content id="Content1" contentplaceholderid="head" runat="Server">
     <script type="text/javascript">
         function OnRequestStart(target, arguments) {
             if (arguments.get_eventTarget().indexOf("reporteGrid") > -1) {
                 arguments.set_enableAjax(false);
             }
         }
</script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/filtros_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblFiltros" Text="Filtros" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td class="item">
                        
                        <table>
                            <tr>
                                <td>
                                    <strong>Cliente:</strong>&nbsp;&nbsp;<asp:DropDownList ID="clienteid" runat="server" AutoPostBack="false"></asp:DropDownList>
                                </td>
                                <td style="width: 5px"></td>
                                <td>
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generar reporte" CssClass="boton" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>        
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">        
                <asp:Image ID="imgPanel1" runat="server" ImageUrl="~/images/icons/reportes_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblReportsLegend" runat="server" Font-Bold="true" CssClass="item" Text="Reporte de Costos de Inventario"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px"></td>
                </tr>
                <tr>
                    <td class="item">
                        <telerik:RadGrid ID="reporteGrid" runat="server" AllowPaging="false" 
                            AutoGenerateColumns="False" GridLines="None" ShowFooter="true" 
                            PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <ExportSettings IgnorePaging="True" FileName="ReporteConsignaciones">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" Name="Consignaciones" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" UniqueName="fecha">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" UniqueName="cliente">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="facturado" HeaderText="Facturado" UniqueName="facturado" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="regresado" HeaderText="Regresado" UniqueName="regresado" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="existencia" HeaderText="Existencia" UniqueName="existencia" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" DataFormatString="{0:c}" UniqueName="importe" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <%--<telerik:GridBoundColumn DataField="monto" HeaderText="Monto" UniqueName="monto" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>--%>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </fieldset>
    </telerik:RadAjaxPanel>
</asp:content>
