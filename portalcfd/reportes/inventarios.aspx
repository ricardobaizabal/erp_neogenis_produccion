<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="inventarios.aspx.vb" Inherits="erp_neogenis.inventarios" %>

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
                        <table border="0" width="50%">
                            <tr valign="top">
                                <td>Almacén:</td>
                                <td>
                                    <asp:DropDownList ID="almacenid" runat="server" AutoPostBack="true" CssClass="box"></asp:DropDownList>
                                </td>
                                <td>Tipo:</td>
                                <td>
                                    <asp:DropDownList ID="tipoid" runat="server" AutoPostBack="true" CssClass="box">
                                        <asp:ListItem Value="0" Text="--Seleccione--" />
                                        <asp:ListItem Value="1" Text="Precio Unit 1" />
                                        <asp:ListItem Value="2" Text="Precio Unit 2" />
                                        <asp:ListItem Value="3" Text="Precio Unit 3" />
                                        <asp:ListItem Value="4" Text="Precio Unit 4" />
                                        <asp:ListItem Value="5" Text="Costo Promedio" />
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generar" CssClass="boton" />
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
                            <MasterTableView DataKeyNames="id" Name="Productos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" AllowSorting="false" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unidad" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" ItemStyle-Width="20%" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario" ItemStyle-HorizontalAlign="Right" HeaderText="Precio Unit. 1" UniqueName="unitario" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario2" ItemStyle-HorizontalAlign="Right" HeaderText="Precio Unit. 2" UniqueName="unitario2" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario3" ItemStyle-HorizontalAlign="Right" HeaderText="Precio Unit. 3" UniqueName="unitario3" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario4" ItemStyle-HorizontalAlign="Right" HeaderText="Precio Unit. 4" UniqueName="unitario4" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="costo_promedio" ItemStyle-HorizontalAlign="Right" HeaderText="Costo Promedio" UniqueName="costo_promedio" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="mexico" HeaderText="México" UniqueName="existencia">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="monterrey" HeaderText="Monterrey" UniqueName="existencia">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="guadalajara" HeaderText="Guadalajara" UniqueName="existencia">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="matriz" HeaderText="Matriz" UniqueName="existencia">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="mty_unitario" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mty_unitario" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mty_unitario2" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mty_unitario2" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mty_unitario3" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mty_unitario3" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mty_unitario4" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mty_unitario4" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mty_costo_promedio" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mty_costo_promedio" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="mex_unitario" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mex_unitario" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mex_unitario2" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mex_unitario2" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mex_unitario3" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mex_unitario3" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mex_unitario4" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mex_unitario4" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="mex_costo_promedio" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="mex_costo_promedio" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="gdl_unitario" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="gdl_unitario" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="gdl_unitario2" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="gdl_unitario2" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="gdl_unitario3" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="gdl_unitario3" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="gdl_unitario4" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="gdl_unitario4" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="gdl_costo_promedio" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="gdl_costo_promedio" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="alm_unitario" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="alm_unitario" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="alm_unitario2" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="alm_unitario2" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="alm_unitario3" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="alm_unitario3" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="alm_unitario4" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="alm_unitario4" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="alm_costo_promedio" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="alm_costo_promedio" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>


                                    <telerik:GridBoundColumn DataField="total" HeaderText="Total" UniqueName="total">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="total_unitario" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="total_unitario" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="total_unitario2" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="total_unitario2" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="total_unitario3" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="total_unitario3" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="total_unitario4" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="total_unitario4" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="total_costo_promedio" ItemStyle-HorizontalAlign="Right" HeaderText="Importe" UniqueName="total_costo_promedio" DataFormatString="{0:0.0000}">
                                    </telerik:GridBoundColumn>

                                    <%--<telerik:GridBoundColumn DataField="mermas" HeaderText="Mermas" UniqueName="existencia">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="proceso" HeaderText="En proceso" UniqueName="proceso">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="disponibles" HeaderText="Disponibles" UniqueName="disponibles">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>--%>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </fieldset>
    </telerik:RadAjaxPanel>

</asp:Content>
