<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" MaintainScrollPositionOnPostback="true" Inherits="erp_neogenis.portalcfd_almacen_Reacomodo" CodeBehind="Reacomodo.aspx.vb" %>

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
            if (arguments.get_eventTarget().indexOf("productslist") > -1) {
                arguments.set_enableAjax(false);
            }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel2" runat="server" Skin="Bootstrap">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/buscador_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblFiltros" Text="Buscador" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table border="0" style="width: 100%;">
                <tr>
                    <td align="right" style="width: 10%;">
                        <span class="item">Marca:</span>
                    </td>
                    <td align="right" style="width: 20%;">
                        <asp:DropDownList ID="cmbMarca" runat="server" Width="95%" CssClass="box"></asp:DropDownList>
                    </td>
                    <td align="right">
                        <span class="item">Ubicacion:</span>
                    </td>
                    <td align="left" style="width: 20%;">
                        <asp:TextBox ID="txtUbicacion" runat="server" Width="92%" CssClass="box"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="boton" Text="Buscar" />&nbsp;&nbsp;&nbsp;
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblProductsListLegend" Text="Ultimos movimientos de ajustes a almacen" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px"></td>
                </tr>
                <tr>
                    <td align="left" class="item">
                        <asp:Label ID="lblMensaje" runat="server" class="item" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="item" colspan="5">&nbsp;&nbsp;
                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" Text="Seleccionar todo" OnCheckedChanged="chkAll_CheckedChanged" />
                        <telerik:RadGrid ID="productslist" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="50" GridLines="None" ShowFooter="true"
                            Skin="Simple">
                            <ExportSettings IgnorePaging="True" FileName="Reacomodo">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" ItemStyle-Width="20">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkReacomodo" runat="server" CssClass="item" Checked='<%# IIf(Eval("chkReacomodo") Is DBNull.Value, "False", Eval("chkReacomodo"))%>' AutoPostBack="True" OnCheckedChanged="ToggleRowSelection" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Marca</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblcompanyId" runat="server" Text='<%#Eval("companyId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Producto</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblproductOrigeId" runat="server" Text='<%#Eval("productOrigeId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Ubicación Origen</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblbarcodeLocationOrige" runat="server" Text='<%#Eval("barcodeLocationOrige") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Cant. Origen Ubicacion</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblquantityOrigenUbicacion" runat="server" Text='<%#Eval("quantityOrigenUbicacion") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Ubicación Destino</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblbarcodeLocationDestination" runat="server" Text='<%#Eval("barcodeLocationDestination") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Cantidad Destino</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblquantityDestination" runat="server" Text='<%#Eval("quantityDestination") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Cant. Total Reacomodo</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblquantityOrige" runat="server" Text='<%#Eval("quantityOrige") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign ="center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>Usuario</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbluserId" runat="server" Text='<%#Eval("userId") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" CausesValidation="False" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnConfirmar" runat="server" CssClass="boton" Text="Confirmar Cambios" />&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMensajeConf" Text="" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>

