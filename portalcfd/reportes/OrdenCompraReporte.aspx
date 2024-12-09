<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="OrdenCompraReporte.aspx.vb" Inherits="erp_neogenis.OrdenCompraReporte" %>

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
                    <td class="item">
                        <br />
                        <table border="0" width="30%">
                            <tr valign="top">
                               <td>Fecha Recepción:</td> 
                              </tr>
                            <tr valign="top">
                              <td>
                                    <telerik:RadDatePicker ID="fechaini" runat="server">
                                        <Calendar UseColumnHeadersAsSelectors="False"
                                            UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                    </telerik:RadDatePicker>
                                </td>
                                 <td>
                                    <telerik:RadDatePicker ID="fechafin" runat="server">
                                        <Calendar UseColumnHeadersAsSelectors="False"
                                            UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                    </telerik:RadDatePicker>
                                </td>
                                </tr>
                            <tr valign="top">
                                <td>Marca:</td>
                                <td>
                                    <asp:DropDownList ID="marcaid" runat="server" AutoPostBack="false" CssClass="box"></asp:DropDownList>
                                </td>
                               
                                <td>Proveedor:</td>
                                <td>
                                    <asp:DropDownList ID="proveedorid" runat="server" CssClass="box"></asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generar" CssClass="boton" />
                                </td>
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
                            PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <ExportSettings IgnorePaging="True" FileName="ReporteInventarios">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView DataKeyNames="ordenCompra" Name="Productos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="ordenCompra" AllowSorting="false" HeaderText="Orden de Compra" UniqueName="ordencompra">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="marca" HeaderText="Marca" UniqueName="marca">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="sku" HeaderText="SKU" UniqueName="sku">
                                    </telerik:GridBoundColumn>    
                                    <telerik:GridBoundColumn DataField="cantidadRecibida" HeaderText="Cantidad" UniqueName="cantidad">
                                    </telerik:GridBoundColumn>                                     
                                   <telerik:GridDateTimeColumn DataField="fechaRecepcion" HeaderText="Fecha recepción" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-Width="20%" UniqueName="descripcion" />                                   
                                    <telerik:GridBoundColumn DataField="costo" ItemStyle-HorizontalAlign="Right" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:C2}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="moneda" HeaderText="Unidad" UniqueName="unidad">
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