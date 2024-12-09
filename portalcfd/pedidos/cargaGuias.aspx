<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="cargaGuias.aspx.vb" Inherits="erp_neogenis.cargaGuias" MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        //function OnRequestStart(target, arguments) {
        //    if ((arguments.get_eventTarget().indexOf("productosList") > -1) || (arguments.get_eventTarget().indexOf("btnImprimir") > -1) || (arguments.get_eventTarget().indexOf("btnAuth") > -1)) {
        //        arguments.set_enableAjax(false);
        //    }
        //}
        function confirmCallbackFn(arg) {
            if (arg) //the user clicked OK
            {
                __doPostBack("<%=HiddenButtonOk.UniqueID %>", "");
            }
            else {
                __doPostBack("<%=HiddenButtonCancel.UniqueID %>", "");
            }
        }
    </script>
    <style type="text/css">
        .m-1em {
            margin: 1em;
        }

        .text-aling-end {
            text-align: end;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">--%>
    <div class="item"><a href="pedidos.aspx">Mis Pedidos</a></div>
    <br />
    <fieldset style="border-color: #cccccc; width: 98%; border-width: 1px; border-style: solid; padding: 10px;">
        <legend title="Pedidos." class="item"><strong>Agregar / Editar Pedido</strong></legend>
        <table id="tblIntMainContent2" width="100%" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" style="vertical-align: top;">
                    <table id="tblProductos" runat="server" width="100%" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" cellpadding="5">
                                    <tr>
                                        <td align="right" class="item">
                                            <fieldset style="width: 98%">
                                                <legend style="padding-right: 6px; color: Black">
                                                    <asp:Label ID="lblPedidoscsv" runat="server" Font-Bold="true" CssClass="item" Text="Carga de Conceptos Pedidos por CSV."></asp:Label>
                                                </legend>
                                                <div>
                                                    <table width="100%" cellspacing="3" cellpadding="3" align="right" style="line-height: 25px;" border="0">
                                                        <tr>
                                                            <td class="item" style="width: 27%;">
                                                                <strong>Seleccione archivo en formato CSV: </strong>
                                                            </td>
                                                            <td style="width: 35%;">
                                                                <asp:FileUpload ID="fileUploadPedido" runat="server" />
                                                            </td>
                                                            <td style="width: 10%;">
                                                                <asp:Button ID="btnCargaPedidosCsv" runat="server" CssClass="item" Visible="True" CausesValidation="true" ValidationGroup="GroupCarga" Text="Cargar Guias CSV" />
                                                                &nbsp;<asp:ImageButton ID="imgdownloadF" runat="server" ImageUrl="~/portalcfd/images/download.png" />
                                                                <span>Formato</span>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <strong>Origen WEB</strong>
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="cmbOrigen" runat="server"></asp:DropDownList>
                                                            </td>
                                                            <td width="33%">&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td width="33%">
                                                                <asp:Label ID="lblMensaje" runat="server" ForeColor="Green"></asp:Label>
                                                            </td>
                                                            <td width="33%">
                                                                <asp:RequiredFieldValidator ID="valOrigen" runat="server" ErrorMessage="Requerido Seleccione Origen Web" ControlToValidate="cmbOrigen" ValidationGroup="GroupCarga" ForeColor="Red" CssClass="item" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                            </td>
                                                            <td width="33%">
                                                                <asp:HiddenField ID="cargaidHidden" Value="0" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </fieldset>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Panel ID="panelCSVPedidos" runat="server" Visible="false">
                                    <fieldset style="width: 98%">
                                        <legend style="padding-right: 6px; color: Black">
                                            <asp:Label ID="lblConceptosCargaPedidos" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Carga CSV Correctos OCP"></asp:Label>
                                        </legend>
                                        <telerik:RadGrid ID="resultslistCSVPedidos" CssClass="grids" Width="100%" runat="server" AllowPaging="True" PageSize="50" AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0" GridLines="None" Skin="Simple" HeaderStyle-Font-Size="Small" ShowHeader="true">
                                            <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True"></ClientSettings>
                                            <MasterTableView DataKeyNames="id,orden_compra,idPago,guia" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No hay registros para mostrar.">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="orden_compra" ItemStyle-Width="10%" FilterControlAltText="Filter column column" HeaderText="Orden Compra" UniqueName="orden_compra" HeaderStyle-Font-Size="Small">
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="idPago" ItemStyle-Width="5%" HeaderText="Id Pago" UniqueName="idPago">
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="guia" runat="server" ItemStyle-Width="10%" HeaderText="Guia" UniqueName="guia">
                                                        <ItemStyle HorizontalAlign="center" />
                                                    </telerik:GridBoundColumn>
                                                </Columns>
                                                <EditFormSettings>
                                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                    </EditColumn>
                                                </EditFormSettings>
                                            </MasterTableView>
                                        </telerik:RadGrid>
                                    </fieldset>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="panelErroresPedidos" runat="server" Visible="False">
                                    <fieldset style="width: 82%">
                                        <legend style="padding-right: 6px; color: Black">
                                            <asp:Label ID="Label2" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Carga CSV con Error"></asp:Label>
                                        </legend>
                                        <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                                            <tr>
                                                <td>
                                                    <telerik:RadGrid ID="erroresListPedidos" runat="server" Width="100%" ShowStatusBar="True" ShowFooter="true"
                                                        AutoGenerateColumns="False" AllowPaging="False" PageSize="50" GridLines="None" ExportSettings-ExportOnlyData="True"
                                                        Skin="Simple">
                                                        <PagerStyle Mode="NumericPages"></PagerStyle>
                                                        <ExportSettings IgnorePaging="true" FileName="CargaMasivaProductosError">
                                                            <Excel Format="ExcelML" />
                                                        </ExportSettings>
                                                        <MasterTableView Width="100%" DataKeyNames="id" Name="Productos" AllowMultiColumnSorting="False" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                                            <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="false" ExportToExcelText="Exportar a Excel"></CommandItemSettings>
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="orden_compra" ItemStyle-Width="10%" FilterControlAltText="Filter column column" HeaderText="Orden Compra" UniqueName="orden_compra" HeaderStyle-Font-Size="Small">
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="idPago" ItemStyle-Width="5%" HeaderText="Id Pago" UniqueName="idPago">
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="guia" runat="server" ItemStyle-Width="10%" HeaderText="Guia" UniqueName="guia">
                                                                    <ItemStyle HorizontalAlign="center" />
                                                                </telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="40%" FilterControlAltText="Filter column2 column" HeaderText="Descripción" UniqueName="descripcion"></telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="error" ItemStyle-Width="70%" HeaderText="Error" UniqueName="error"></telerik:GridBoundColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <asp:HiddenField ID="cargaidHidden2" Value="0" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </asp:Panel>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="panel1" Visible="false" runat="server">
                                    <asp:Label ID="lblProdsTitulo" runat="server" Font-Bold="true" Font-Size="Small" class="item" Text="Lista de Productos"></asp:Label><br />
                                    <br />
                                    <telerik:RadGrid Width="100%" ID="productosList" CssClass="grids" runat="server" AllowPaging="True"
                                        PageSize="50" AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0"
                                        GridLines="None" Skin="Simple" HeaderStyle-Font-Size="Small" ShowHeader="true">
                                        <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True"></ClientSettings>
                                        <MasterTableView DataKeyNames="id" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No hay registros para mostrar.">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="ordencompra" ItemStyle-Width="10%" FilterControlAltText="Filter column column" HeaderText="Referencia" UniqueName="codigo" HeaderStyle-Font-Size="Small">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="existencia" ItemStyle-Width="50px" HeaderText="Existencia"></telerik:GridBoundColumn>--%>
                                                <telerik:GridBoundColumn DataField="idpago" ItemStyle-Width="5%" HeaderText="Id Pago" UniqueName="idpago">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="guia" UniqueName="ColCantidad" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="txtGuia" Width="120px" runat="server"></telerik:RadNumericTextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <EditFormSettings>
                                                <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                                </EditColumn>
                                            </EditFormSettings>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <div align="right">
                        <asp:Button ID="btnAgregaConceptos" runat="server" CssClass="item" Visible="False" Text="Actualizar Información" />&nbsp;&nbsp;
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" style="vertical-align: middle; height: 30px;">
                    <asp:Label ID="lblPedidoError" runat="server" CssClass="merror2" Visible="false"></asp:Label>
                </td>
            </tr>
        </table>
    </fieldset>
    <%--</telerik:RadAjaxPanel>--%>
    <telerik:RadWindowManager ID="RadWindowManager1" Localization-OK="Aceptar" Localization-Cancel="Cancelar" runat="server">
    </telerik:RadWindowManager>
    <asp:Button ID="HiddenButtonOk" Text="" Style="display: none;" runat="server" />
    <asp:Button ID="HiddenButtonCancel" Text="" Style="display: none;" runat="server" />
    <!-- End Modal Cancelar -->
    <br />
    <br />
    <br />
    <telerik:RadWindowManager ID="rwAlerta" runat="server" Skin="Default" EnableShadow="false">
        <Localization OK="Aceptar" Cancel="Cancelar" />
    </telerik:RadWindowManager>
    <br />
    <br />
    <br />
</asp:Content>
