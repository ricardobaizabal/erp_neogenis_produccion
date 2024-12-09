<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="recepcionMercancia.aspx.vb" Inherits="erp_neogenis.RecepcionMercancia" %>

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
                    <td class="item">
                        <br />
                        <table border="0" width="30%">
                            <tr valign="top">
                                <td>Marca:</td>
                                <td>
                                    <asp:DropDownList ID="marcaid" runat="server" AutoPostBack="false" CssClass="box"></asp:DropDownList>
                                </td>
                                <td>Temporada:</td>
                                <td>
                                    <asp:DropDownList ID="coleccionid" runat="server" CssClass="box"></asp:DropDownList>
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
                            PageSize="300" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <ExportSettings IgnorePaging="True" FileName="ReporteInventarios">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView DataKeyNames="productoid" Name="Productos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="productoid" AllowSorting="false" HeaderText="Producto Id" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="codigosku" HeaderText="SKU" UniqueName="sku">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" ItemStyle-Width="20%" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="marca" ItemStyle-HorizontalAlign="Left" HeaderText="Marca" UniqueName="marca">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="temporada" ItemStyle-HorizontalAlign="Left" HeaderText="Temporada" UniqueName="temporada">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" UniqueName="cliente" DataFormatString="{0:N2}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="existencia" ItemStyle-HorizontalAlign="Right" HeaderText="Existencia" UniqueName="existencia" Aggregate="Sum" FooterText=" " FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="totaloc" ItemStyle-HorizontalAlign="Right" HeaderText="Total" UniqueName="totaloc" Aggregate="Sum" FooterText=" " FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="totalinvoc" ItemStyle-HorizontalAlign="Right" HeaderText="Totalinvoc" UniqueName="totalinvoc">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="acciones" ItemStyle-HorizontalAlign="Right" HeaderText="Acciones" UniqueName="acciones">
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
