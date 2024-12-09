<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="comisiones.aspx.vb" Inherits="erp_neogenis.comisiones" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function OnRequestStart(target, arguments) {
            if (arguments.get_eventTarget().indexOf("reporteGrid") > -1) {
                arguments.set_enableAjax(false);
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default">
    </telerik:RadAjaxLoadingPanel>

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblReportsLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table border="0" width="100%">
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
                <tr>
                    <td class="item">Seleccione el rango de fechas que desee consultar en base a las fechas de pago:<br />
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
                                <td colspan="3">
                                    <asp:DropDownList ID="clienteid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <%--<td>Sucursal: </td>
                                <td>
                                    <asp:DropDownList ID="sucursalid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>--%>
                            </tr>
                            <tr>
                                <%--<td>Vendedor: </td>
                                <td>
                                    <asp:DropDownList ID="vendedorid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td>Tipo: </td>
                                <td>
                                    <asp:DropDownList ID="tipoid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td>Proyecto: </td>
                                <td>
                                    <asp:DropDownList ID="proyectoid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>--%>
                                <td>
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generar" CssClass="boton" />
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
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
                    <td style="height: 5px" colspan="5"></td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <telerik:RadGrid ID="reporteGrid" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" GridLines="None" ShowFooter="True"
                            PageSize="300" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <ExportSettings IgnorePaging="True" FileName="ReporteComisiones">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView DataKeyNames="id" Name="Productos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="serie" HeaderText="Serie" UniqueName="serie">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="folio" HeaderText="Folio" UniqueName="folio">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha_factura" HeaderText="Fecha factura" UniqueName="fecha_factura" DataFormatString="{0:d}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha pago" UniqueName="fecha" DataFormatString="{0:d}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cliente" ItemStyle-HorizontalAlign="Left" HeaderText="Cliente" UniqueName="cliente">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="sucursal" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="sucursal"  HeaderText="Sucursal" UniqueName="sucursal">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="vendedor" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="vendedor"  HeaderText="Vendedor" UniqueName="vendedor">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="comiEjec" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="vendedor"  HeaderText="%" UniqueName="vendedor">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ComisionEjecutivo" ItemStyle-HorizontalAlign="Left" AllowSorting="true" DataFormatString="{0:c}" SortExpression="comisionEjecutivo" HeaderText="Comision Ejecutivo" UniqueName="comisionEjecutivo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="coordinadorV" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="coordinadorv"  HeaderText="Coordinador" UniqueName="coordinadorv">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="comiCoord" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="coordinadorv"  HeaderText="%" UniqueName="comiCoord">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="ComisionCoordinador" ItemStyle-HorizontalAlign="Left" AllowSorting="true" DataFormatString="{0:c}" SortExpression="coordinadorv"  HeaderText="Comision Coordinador" UniqueName="comisioncoordinador">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="importe" AllowSorting="false" HeaderText="Importe" UniqueName="importe" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descuento" AllowSorting="false" HeaderText="Descuento" UniqueName="descuento" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="iva" HeaderText="IVA" UniqueName="iva" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="total" HeaderText="Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="estatus_cobranza" HeaderText="Estatus" UniqueName="estatus_cobranza" ItemStyle-HorizontalAlign="Left">
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
