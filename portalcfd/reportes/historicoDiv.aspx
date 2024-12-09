<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" CodeBehind="historicoDiv.aspx.vb" Inherits="erp_neogenis.historicoDiv" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function OnRequestStart(target, arguments) {
            if (arguments.get_eventTarget().indexOf("reporteGrid") > -1) {
                arguments.set_enableAjax(false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">

        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblReportsLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
                <tr>
                    <td class="item">Seleccione el rango de fechas que desee consultar en base a las fechas factura y el cliente:<br />
                        <br />
                        <table border="0" width="100%">
                            <tr>
                                <td>Desde: </td>
                                <td>
                                    <telerik:RadDatePicker ID="fechaini" runat="server">
                                        <Calendar UseColumnHeadersAsSelectors="False"
                                            UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                    </telerik:RadDatePicker>
                                </td>
                                <td>Hasta: </td>
                                <td>
                                    <telerik:RadDatePicker ID="fechafin" runat="server">
                                        <Calendar UseColumnHeadersAsSelectors="False"
                                            UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                    </telerik:RadDatePicker>
                                </td>
                                <td>Cliente: </td>
                                <td>
                                    <asp:DropDownList ID="clienteid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td>Sucursal: </td>
                                <td>
                                    <asp:DropDownList ID="sucursalid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>Vendedor: </td>
                                <td>
                                    <asp:DropDownList ID="vendedorid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td>Tipo:</td>
                                <td>
                                    <asp:DropDownList ID="tipoid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="8" class="item">
                                    <br />
                                    Criterio de búsqueda (solo en descripciones):
                                    <asp:TextBox ID="txtCriterio" runat="server" CssClass="box" Width="300px"></asp:TextBox>&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8">
                                    <br />
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generar reporte" CssClass="boton" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 5px">
                        <asp:Label ID="lblMensaje" runat="server" Font-Bold="true" Font-Names="Verdana" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black"></legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
                <tr>
                    <td class="item">
                        <telerik:RadGrid ID="reporteGrid" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" GridLines="None" ShowFooter="True"
                            PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <ExportSettings IgnorePaging="True" FileName="ReporteHistorico">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" Name="Productos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="serie" HeaderText="Serie" UniqueName="serie">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="folio" HeaderText="Folio" UniqueName="folio">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha factura" UniqueName="fecha" DataFormatString="{0:d}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" UniqueName="fecha" DataFormatString="{0:d}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código-Sku" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="marca" HeaderText="Marca" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unid. Medida" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="costopromedio" HeaderText="Costo Promedio Unit." UniqueName="costo_promedio" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precio_unitario" HeaderText="Precio Unit." UniqueName="precio_unitario" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" UniqueName="importe" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descuento" HeaderText="Descuento" UniqueName="descuento" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="iva" HeaderText="IVA" UniqueName="iva" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="total" HeaderText="Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </fieldset>

    </telerik:RadAjaxPanel>
</asp:Content>
