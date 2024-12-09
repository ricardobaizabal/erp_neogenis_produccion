<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="cartera.aspx.vb" Inherits="erp_neogenis.cartera" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function OnRequestStart(target, arguments) {
            if (arguments.get_eventTarget().indexOf("detalleGrid") > -1) {
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
                <asp:Label ID="lblReportsLegend" runat="server" Font-Bold="true" CssClass="item" Text="Reporte de cartera"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px"></td>
                </tr>
                <tr>
                    <td class="item">
                        <table border="0" width="80%" style="line-height: 20px;">
                            <tr>
                                <td class="item" style="width: 25%;"> Cuenta Concentradora:<br />
                                    <asp:DropDownList ID="cmbCuentaConcentradora" runat="server" CssClass="box" AutoPostBack="true" Width="95%"></asp:DropDownList>
                                </td>
                                <td class="item">&nbsp;</td>
                                <td class="item">&nbsp;</td>
                                <br />
                            </tr>
                            <tr>
                                <td>Cliente:<br />
                                    <asp:DropDownList ID="clienteid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td>Sucursal:<br />
                                    <asp:DropDownList ID="sucursalid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td>Vendedor:<br />
                                    <asp:DropDownList ID="vendedorid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                                </td>
                                <td style="width: 20px"></td>
                                <td>
                                    <br />
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generar reporte" CssClass="boton" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px"></td>
                </tr>
                <tr>
                    <td style="height: 5px">
                        <asp:Label ID="lblMensaje" runat="server" Font-Bold="true" Font-Names="Verdana" Font-Size="Small" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>

        <br />

        <asp:Panel ID="panelDetalle" runat="server" Width="100%">
            <fieldset class="item">
                <legend style="padding-right: 6px; color: Black; font-weight: bold;">Detalle</legend>
                <table width="100%">
                    <tr>
                        <td style="height: 5px"></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadGrid ID="detalleGrid" runat="server" AllowPaging="True"
                                AutoGenerateColumns="False" GridLines="None" ShowFooter="True"
                                PageSize="200" ShowStatusBar="True" ShowHeader="true" AllowSorting="True"
                                Skin="Simple" Width="100%" ExportSettings-ExportOnlyData="False">
                                <ExportSettings IgnorePaging="True" FileName="ReporteDetalleCartera">
                                    <Excel Format="Biff" />
                                </ExportSettings>
                                <PagerStyle Mode="NumericPages" />
                                <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Ingresos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                    <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="serie" ItemStyle-Width="20px" HeaderText="Serie" UniqueName="serie">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn ItemStyle-Width="20px" UniqueName="folio">
                                            <HeaderTemplate>
                                                Folio
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkFolio" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdFolio" Text='<%# Eval("folio") %>'></asp:LinkButton>
                                                <asp:Label ID="lblFolio" runat="server" Visible="false"></asp:Label>
                                                <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="lnkFolio" RelativeTo="Element" Position="BottomCenter" RenderInPageRoot="true" ManualClose="true">
                                                    <%#Eval("DetalleFactura")%>
                                                </telerik:RadToolTip>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="fecha" DataFormatString="{0:d}" HeaderText="Fecha factura" UniqueName="fecha" ItemStyle-Width="40px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="fecha_promesa" DataFormatString="{0:d}" HeaderText="Fecha promesa" UniqueName="fecha_promesa" ItemStyle-Width="40px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="dias" SortExpression="dias" HeaderText="Días de atraso" UniqueName="dias" ItemStyle-Width="50px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="razonsocial" HeaderText="Razón Social" SortExpression="razonsocial" UniqueName="razonsocial">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="sucursal" HeaderText="Sucursal" SortExpression="sucursal" UniqueName="sucursal">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="vendedor" HeaderText="Vendedor" SortExpression="vendedor" UniqueName="vendedor">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="importe" HeaderText="importe" SortExpression="importe" UniqueName="importe">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn DataField="importeTasa16" AllowSorting="false" HeaderText="Importe Tasa 16" UniqueName="importeTasa16" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="importeTasa0" AllowSorting="false" HeaderText="Importe Tasa 0" UniqueName="importeTasa0" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>--%>
                                        <telerik:GridBoundColumn DataField="descuento" AllowSorting="false" HeaderText="Descuento" UniqueName="descuento" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="iva" DataFormatString="{0:c}" HeaderText="IVA" ItemStyle-HorizontalAlign="Right" UniqueName="iva" ItemStyle-Width="40px">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="pagado" AllowSorting="false" HeaderText="Pagado" UniqueName="pagado" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="saldo" AllowSorting="false" HeaderText="Saldo" UniqueName="saldo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="total" DataFormatString="{0:c}" HeaderText="Total" ItemStyle-HorizontalAlign="Right" UniqueName="total">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="estatus_cobranza" HeaderText="Estatus" ItemStyle-HorizontalAlign="Left" UniqueName="estatus_cobranza">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="proyecto" HeaderText="Proyecto" ItemStyle-HorizontalAlign="Left" UniqueName="proyecto">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="pdf">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPDF" runat="server" Text="pdf" CommandArgument='<%# Eval("id") %>' CommandName="cmdPDF"></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 20px"></td>
                    </tr>
                </table>
            </fieldset>
        </asp:Panel>
    
    </telerik:RadAjaxPanel>

</asp:Content>