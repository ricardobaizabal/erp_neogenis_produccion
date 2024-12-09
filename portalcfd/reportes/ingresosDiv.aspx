<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="ingresosDiv.aspx.vb" Inherits="erp_neogenis.ingresosDiv" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        checked = false;
        function checkedAll(frm1) {
            var aa = frm1;
            if (checked == false) {
                checked = true
            }
            else {
                checked = false
            }
            for (var i = 0; i < aa.elements.length; i++) {
                aa.elements[i].checked = checked;
            }
        }

        function OnRequestStart(target, arguments) {
            if (arguments.get_eventTarget().indexOf("reporteGrid") > -1) {
                arguments.set_enableAjax(false);
            }
        }

    </script>
    <style type="text/css" >
        .text-aling-rigth{
            text-align: right;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap">
    </telerik:RadAjaxLoadingPanel>

    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">--%>

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
                <td class="item">Seleccione el rango de fechas que desee consultar:<br />
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
                                <asp:DropDownList ID="clienteid" runat="server" CssClass="box" AutoPostBack="true" Width="80%"></asp:DropDownList>
                            </td>
                            <td>Sucursal: </td>
                            <td>
                                <asp:DropDownList ID="sucursalid" runat="server" CssClass="box" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="10">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>Vendedor: </td>
                            <td>
                                <asp:DropDownList ID="vendedorid" runat="server" CssClass="box"></asp:DropDownList>
                            </td>
                            <td>Tipo: </td>
                            <td>
                                <asp:DropDownList ID="tipoid" runat="server" CssClass="box" AutoPostBack="False"></asp:DropDownList>
                            </td>
                            <td>Marca: </td>
                            <td colspan="1">
                                <asp:DropDownList ID="proyectoid" runat="server" CssClass="box"></asp:DropDownList>
                            </td>
                            <td colspan="1" class="text-aling-rigth">Estatus: </td>
                            <td colspan="1">
                                <asp:DropDownList ID="estatuscobranza" runat="server" CssClass="box"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button ID="btnGenerate" runat="server" Text="Generar" CssClass="boton" OnClick="btnGenerate_Click" />
                            </td>
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
        <%--<legend style="padding-right: 6px; color: Black"></legend>--%>
        <table width="100%">
            <tr>
                <td class="item" colspan="5">&nbsp;&nbsp;<asp:CheckBox ID="chkAll" runat="server" Visible="false" Text="Seleccionar todo" /><br />
                    <br />
                    <telerik:RadGrid ID="reporteGrid" runat="server" AllowPaging="True" AllowSorting="True"
                        AutoGenerateColumns="False" GridLines="None" ShowFooter="True"
                        PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                        Skin="Simple" Width="100%">
                        <ExportSettings HideStructureColumns="true" IgnorePaging="True" FileName="ReporteFacturacion">
                            <Excel Format="Biff" />
                        </ExportSettings>
                        <PagerStyle Mode="NumericPages" />
                        <MasterTableView DataKeyNames="id" Name="Ingresos" Width="100%" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                            <Columns>
                                <telerik:GridTemplateColumn UniqueName="chkcfdid" Visible="true">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkcfdid" runat="server" CssClass="item" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>Serie</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerie" runat="server" Text='<%# Eval("serie") %>'></asp:Label>
                                        <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="lblSerie" RelativeTo="Element" Position="BottomCenter" RenderInPageRoot="true" Text='<%#Eval("detalle")%>' ManualClose="true"></telerik:RadToolTip>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <%--<telerik:GridBoundColumn DataField="folio" HeaderText="Folio" AllowSorting="false" UniqueName="folio">
                                    </telerik:GridBoundColumn>--%>
                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>Folio</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFolio" runat="server" Text='<%# Eval("folio") %>' CommandArgument='<%# Eval("id") %>' CommandName="cmdFolio"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" AllowSorting="false" UniqueName="fecha" DataFormatString="{0:d}">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cliente" ItemStyle-HorizontalAlign="Left" AllowSorting="false" HeaderText="Cliente" UniqueName="cliente">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="sucursal" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="sucursal" HeaderButtonType="LinkButton" HeaderText="Sucursal" UniqueName="sucursal">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="vendedor" ItemStyle-HorizontalAlign="Left" AllowSorting="true" SortExpression="vendedor" HeaderButtonType="LinkButton" HeaderText="Vendedor" UniqueName="vendedor">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="importe" AllowSorting="false" HeaderText="Importe" UniqueName="importe" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descuento" AllowSorting="false" HeaderText="Descuento" UniqueName="descuento" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="iva" AllowSorting="false" HeaderText="IVA" UniqueName="iva" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="total" AllowSorting="false" HeaderText="Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="pagado" AllowSorting="false" HeaderText="C. P." UniqueName="pagado" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="pagos" AllowSorting="false" HeaderText="Pagos" UniqueName="pagos" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="nc" AllowSorting="false" HeaderText="N.C" UniqueName="nc" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="saldo" AllowSorting="false" HeaderText="Saldo" UniqueName="saldo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="estatus_cobranza" AllowSorting="false" HeaderText="Estatus" UniqueName="estatus_cobranza" ItemStyle-HorizontalAlign="Left">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="proyecto" AllowSorting="false" HeaderText="Proyecto" UniqueName="proyecto" ItemStyle-HorizontalAlign="Left">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="complementos" AllowSorting="false" HeaderText="Folios C.P." UniqueName="complementos" ItemStyle-HorizontalAlign="Left">
                                </telerik:GridBoundColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>

        </table>
    </fieldset>
    <br />
    <asp:Panel runat="server" ID="panelCobrar" Visible="true">
        <fieldset>
            <legend style="padding-right: 6px; color: Black"></legend>
            <table border="0" width="80%">
                <tr>
                    <td class="item">Estatus:&nbsp;<asp:RequiredFieldValidator ID="valEstatus" ControlToValidate="estatus_cobranzaid" ErrorMessage="* requerido" ForeColor="Red" SetFocusOnError="true" InitialValue="0" ValidationGroup="valUpdateAll" runat="server"></asp:RequiredFieldValidator><br />
                        <br />
                        <asp:DropDownList ID="estatus_cobranzaid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td class="item">Tipo de pago:&nbsp;<asp:RequiredFieldValidator ID="valTipoPago" ControlToValidate="tipo_pagoid" ErrorMessage="* requerido" ForeColor="Red" SetFocusOnError="true" InitialValue="0" ValidationGroup="valUpdateAll" runat="server"></asp:RequiredFieldValidator><br />
                        <br />
                        <asp:DropDownList ID="tipo_pagoid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td class="item">Referencia:<br />
                        <br />
                        <asp:TextBox ID="referencia" runat="server" CssClass="box"></asp:TextBox>
                    </td>
                    <td class="item">Fecha de pago:&nbsp;<asp:RequiredFieldValidator ID="valFecha" ControlToValidate="fechapago" ErrorMessage="* requerido" ForeColor="Red" SetFocusOnError="true" ValidationGroup="valUpdateAll" runat="server"></asp:RequiredFieldValidator><br />
                        <br />
                        <telerik:RadDatePicker ID="fechapago" runat="server"></telerik:RadDatePicker>
                    </td>
                    <td class="item">
                        <br />
                        <br />
                        <asp:Button ID="btnPayAll" ValidationGroup="valUpdateAll" runat="server" Text="Aplicar" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                        <br />
                        <asp:Label ID="lblMensajeActualiza" runat="server" ForeColor="Green"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <%--</telerik:RadAjaxPanel>--%>
</asp:Content>
