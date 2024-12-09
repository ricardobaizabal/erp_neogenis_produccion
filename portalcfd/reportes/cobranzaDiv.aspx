<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="cobranzaDiv.aspx.vb" Inherits="erp_neogenis.cobranzaDiv" %>

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

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap">
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
                    <td class="item" style="width: 25%;">Cuenta Concentradora:<br />
                        <asp:DropDownList ID="cmbCuentaConcentradora" runat="server" CssClass="box" AutoPostBack="true" Width="50%"></asp:DropDownList>
                    </td>
                    <td class="item">&nbsp;</td>
                    <td class="item">&nbsp; <br /></td>
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
                                <td>Tipo: </td>
                                <td>
                                    <asp:DropDownList ID="tipoid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td>Proyecto: </td>
                                <td>
                                    <asp:DropDownList ID="proyectoid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
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
                    <td style="height: 5px"></td>
                </tr>
                <tr>
                    <td class="item">
                        <telerik:RadGrid ID="reporteGrid" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None" ShowFooter="true"
                            PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <ExportSettings IgnorePaging="True" FileName="ReporteCobranza">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Ingresos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="serie" HeaderText="Serie" UniqueName="serie">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Folio</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFolio" runat="server" Text='<%# Eval("folio") %>' CommandArgument='<%# Eval("id") %>' CommandName="cmdFolio"></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="fecha_factura" HeaderText="Fecha factura" UniqueName="fecha_factura" DataFormatString="{0:d}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha pago" UniqueName="fecha" DataFormatString="{0:d}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cliente" ItemStyle-HorizontalAlign="Left" HeaderText="Cliente" UniqueName="cliente">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="sucursal" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="sucursal" HeaderButtonType="LinkButton" HeaderText="Sucursal" UniqueName="sucursal">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="vendedor" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="vendedor" HeaderButtonType="LinkButton" HeaderText="Vendedor" UniqueName="vendedor">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="coordinadorV" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="coordinadorv" HeaderButtonType="LinkButton" HeaderText="Coordinador" UniqueName="coordinadorv">
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
                                    <telerik:GridBoundColumn DataField="tipo_pago" HeaderText="Forma Pago" UniqueName="tipo_pago" ItemStyle-HorizontalAlign="Left">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" UniqueName="refrencia" ItemStyle-HorizontalAlign="Left">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="proyecto" AllowSorting="false" HeaderText="Proyecto" UniqueName="proyecto" ItemStyle-HorizontalAlign="Left">
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
