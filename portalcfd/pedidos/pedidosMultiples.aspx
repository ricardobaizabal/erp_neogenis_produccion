<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" EnableEventValidation="false" CodeBehind="pedidosMultiples.aspx.vb" Inherits="erp_neogenis.pedidosMultiples" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function confirmCallbackFn(arg) {
            if (arg) //the user clicked OK
            {
                __doPostBack("<%=HiddenButtonOk.UniqueID %>", "");
            }
            else {
                __doPostBack("<%=HiddenButtonCancel.UniqueID %>", "");
            }
        }
        function OnRequestStart(target, arguments) {
            if ((arguments.get_eventTarget().indexOf("pedidosList") > -1) || (arguments.get_eventTarget().indexOf("btnExportExcel") > -1)) {
                arguments.set_enableAjax(false);
            }
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <telerik:RadWindowManager ID="RadWindowManager1" Localization-OK="Aceptar" Localization-Cancel="Cancelar" runat="server">
    </telerik:RadWindowManager>

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
        <fieldset style="border-color: #cccccc; width: 98%; border-width: 0px; border-style: solid; padding: 10px;">
            <legend title="Pedidos." class="item"><strong>Mis Pedidos Multiples</strong></legend>
            <table id="tblIntMainContent2" border="0" width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend style="padding-right: 6px; color: Black">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/buscador_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblFiltros" Text="Buscador" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                            </legend>
                            <br />
                            <span class="item">Cliente:
                            <asp:DropDownList ID="filtroclienteid" AutoPostBack="true" runat="server" CssClass="box"></asp:DropDownList>&nbsp;&nbsp;&nbsp;Estatus:
                            <%--<asp:DropDownList ID="filtroestatusid" runat="server" CssClass="box"></asp:DropDownList>&nbsp;&nbsp;&nbsp;Palabra clave:--%>
                            <asp:TextBox ID="txtSearch" runat="server" Width="120px" CssClass="box"></asp:TextBox>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnSearch" runat="server" CssClass="boton" Text="Buscar" />&nbsp;&nbsp;&nbsp;
                            </span>
                            <table width="100%" border="0">
                            <tr valign="top" style="height: 20px;">
                                <td colspan="3">&nbsp;</td>
                            </tr>
                            <%--<tr>
                                <td align="left" style="width: 20%;">
                                    <telerik:RadDatePicker ID="fha_ini" runat="server">
                                    </telerik:RadDatePicker>
                                </td>
                                <td align="left" style="width: 20%;">
                                    <telerik:RadDatePicker ID="fha_fin" runat="server">
                                    </telerik:RadDatePicker>
                                </td>
                                <td>&nbsp;</td>
                            </tr>--%>
                        </table>
                            <br />
                            <br />
                        
                        <table width="100%" border="0">
                            <tr valign="top" style="height: 20px;">
                                <td colspan="6">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <strong>Datos de Factura.</strong>
                                </td>
                            </tr>
                            <tr>
                                <%--<td class="item"><strong>Cliente:</strong></td>--%>
                                <td class="item"><strong>Sucursal:</strong></td>
                                <td class="item"><strong>Almacén:</strong></td>
                                <td class="item"><strong>Marca:</strong></td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <%--<td align="left" style="width: 30%;">
                                    <asp:DropDownList ID="cmbCliente" runat="server" AutoPostBack="true" ValidationGroup="grupo1" CssClass="box"></asp:DropDownList>
                                </td>--%>
                                <td align="left" style="width: 15%;">
                                    <asp:DropDownList ID="sucursalid" runat="server" ValidationGroup="grupo1" CssClass="box"></asp:DropDownList>
                                </td>
                                <td align="left" style="width: 15%;">
                                    <asp:DropDownList ID="almacenid" runat="server" ValidationGroup="grupo1" CssClass="box"></asp:DropDownList>
                                </td>
                                <td align="left" style="width: 15%;">
                                    <asp:DropDownList ID="proyectoid" runat="server" ValidationGroup="grupo1" CssClass="box"></asp:DropDownList>
                                </td>
                                <td colspan="2" align="left">
                                    <asp:Button ID="btnFacturarMultiple" runat="server" Text="Facturar" CssClass="item" ValidationGroup="grupo1" />
                                </td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <%--<td align="left" style="width: 30%;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="cmbCliente" class="item" ErrorMessage="Seleccione un cliente" ForeColor="Red" InitialValue="0" SetFocusOnError="True" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                                </td>--%>
                                <td align="left" style="width: 15%;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="sucursalid" class="item" ErrorMessage="Seleccione una sucursal" ForeColor="Red" InitialValue="0" SetFocusOnError="True" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" style="width: 15%;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="almacenid" class="item" ErrorMessage="Seleccione un almacén" ForeColor="Red" InitialValue="0" SetFocusOnError="True" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                                </td>
                                <td align="left" style="width: 15%;">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="proyectoid" class="item" ErrorMessage="Seleccione un proyecto" ForeColor="Red" InitialValue="0" SetFocusOnError="True" ValidationGroup="grupo1"></asp:RequiredFieldValidator>
                                </td>
                                <td>&nbsp;</td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr valign="top" style="height: 20px;">
                                <td align="right" colspan="6">
                                    <asp:Label ID="lblMensaje" ForeColor="Red" runat="server" class="item" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                            </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnExportExcel" runat="server" Text="Exportar a Excel" Visible="false" /><br />

                        <br />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="vertical-align: top;">
                        <%--<asp:CheckBox ID="chkAll" runat="server" AutoPostBack="false" Text="Seleccionar todo" />--%>
                        <telerik:RadGrid ID="pedidosList" runat="server" AllowPaging="True" PageSize="50"
                            AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0" Skin="Simple" Width="100%" AllowMultiRowSelection="true"
                            ExportSettings-ExportOnlyData="false" ExportSettings-IgnorePaging="false">

                            <ExportSettings IgnorePaging="True" FileName="Pedidos">
                                <Excel Format="Biff" />
                            </ExportSettings>

                            <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                            </ClientSettings>

                            <MasterTableView DataKeyNames="id, cliente, estatusid, estatus" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No hay registros para mostrar." CommandItemDisplay="none">
                                <CommandItemSettings ExportToPdfText="Export to PDF" />
                                <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                    <HeaderStyle Width="20px" />
                                </RowIndicatorColumn>
                                <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                    <HeaderStyle Width="20px" />
                                </ExpandCollapseColumn>
                                <Columns>
                                    <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn1" Exportable="false"></telerik:GridClientSelectColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>ID</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblId" runat="server" Text='<%#Eval("id") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Cliente</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCliente" runat="server" Text='<%#Eval("cliente") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Sucursal</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSucursal" runat="server" Text='<%#Eval("sucursal") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Proyecto</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProyecto" runat="server" Text='<%#Eval("proyecto") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Ejecutivo</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEjecutivo" runat="server" Text='<%#Eval("ejecutivo") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Fecha</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFecha_alta" runat="server" Text='<%#Eval("fecha_alta") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Estatus</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstatus" runat="server" Text='<%#Eval("estatus") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Factura</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFactura" runat="server" Text='<%#Eval("factura") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Display="false">
                                        <HeaderTemplate>Guia</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGuia" runat="server" Text='<%#Eval("guia") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Display="false">
                                        <HeaderTemplate>Pagoid</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPagoid" runat="server" Text='<%#Eval("pagoid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Display="false">
                                        <HeaderTemplate>EstatusID</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEstatusId" runat="server" Text='<%#Eval("estatusid") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--<telerik:GridTemplateColumn Display="false">
                                        <HeaderTemplate>Timbrado</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTimbrado" runat="server" Text='<%#Eval("timbrado") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                    <%--<telerik:GridBoundColumn DataField="guia" HeaderText="No. Guía" UniqueName="guia">
                                    </telerik:GridBoundColumn>--%>
                                    <%--::::::::::--%>
                                    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Font-Underline="true" ItemStyle-Font-Bold="true" HeaderText="No. Guía" UniqueName="guia" Exportable="false">
                                        <ItemTemplate>

                                            <telerik:RadTextBox ID="txtGuia" runat="server" CommandArgument='<%# Eval("id") %>' Text='<%# Eval("guia") %>' AutoPostBack="true" OnTextChanged="txtGuia_TextChanged" CommandName="cmdEdit" Width="100px"></telerik:RadTextBox>

                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <%--::::::::::--%>
                                    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-Font-Underline="true" ItemStyle-Font-Bold="true" HeaderText="Id Pago" UniqueName="guia" Exportable="false">
                                        <ItemTemplate>
                                            <telerik:RadTextBox ID="txtpagoid" runat="server" CommandArgument='<%# Eval("id") %>' Text='<%# Eval("pagoid") %>' AutoPostBack="true" OnTextChanged="txtpagoid_TextChanged" CommandName="cmdEdit" Width="40px"></telerik:RadTextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <%--<telerik:GridBoundColumn DataField="orden_compra" HeaderText="Orden Compra" UniqueName="orden_compra">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridTemplateColumn Display="True">
                                        <HeaderTemplate>orden_compra</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrden_compra" runat="server" Text='<%#Eval("orden_compra") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ColEditar" AllowFiltering="true" HeaderText="Editar" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEditar" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEditar" ImageUrl="~/images/action_edit.png" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ColEtapaAbierto" AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" HeaderText="Regresar" Exportable="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEtapaAbierto" runat="server" CommandArgument='<%# Eval("id") %>' Width="26px" CommandName="cmdEtapaAbierto" ImageUrl="~/images/refresh_reload_back.png" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <%--<telerik:GridTemplateColumn UniqueName="ColFacturar40" AllowFiltering="true" HeaderText="" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Exportable="false">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFacturar40" runat="server" Text="Facturar" CommandArgument='<%# Eval("id") %>' CommandName="cmdFacturar40"></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                    <telerik:GridTemplateColumn UniqueName="CheckBoxTemplateColumn" ItemStyle-Width="20" HeaderText="Seleccionar">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkcfdid" runat="server" CssClass="item" Checked='<%# If(IsDBNull(Eval("chkcfdid")), False, Convert.ToBoolean(Eval("chkcfdid"))) %>' AutoPostBack="True" OnCheckedChanged="ToggleRowSelection" />
                                         <asp:Label ID="chkLabel" runat="server" Text='<%# If(IsDBNull(Eval("chkcfdid")), "False", Eval("chkcfdid").ToString()) %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="ColDelete" AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" HeaderText="Eliminar" Exportable="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEliminar" ImageUrl="~/images/action_delete.gif" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <EditFormSettings>
                                    <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                    </EditColumn>
                                </EditFormSettings>

                            </MasterTableView>
                            <ClientSettings>
                                <Selecting AllowRowSelect="true"></Selecting>
                            </ClientSettings>
                        </telerik:RadGrid>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div style="display: none">
                            <telerik:RadGrid ID="ExcelGrid" runat="server" AllowPaging="True" PageSize="50"
                                AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0" Skin="Simple" Width="100%" AllowMultiRowSelection="true"
                                ExportSettings-ExportOnlyData="false" ExportSettings-IgnorePaging="false" OnItemDataBound="RadGrid1_ItemDataBound">

                                <ExportSettings IgnorePaging="True" FileName="Pedidos">
                                    <Excel Format="Biff" />
                                </ExportSettings>

                                <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                                </ClientSettings>

                                <MasterTableView ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No hay registros para mostrar." CommandItemDisplay="none">
                                    <CommandItemSettings ExportToPdfText="Export to PDF" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </RowIndicatorColumn>
                                    <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                        <HeaderStyle Width="20px" />
                                    </ExpandCollapseColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="100" DataField="fecha" HeaderText="Fecha" UniqueName="fecha" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="60" DataField="mes" HeaderText="Mes" UniqueName="mes" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="30" DataField="numero" HeaderText="#" UniqueName="numero" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="250" DataField="cliente" HeaderText="Cliente" UniqueName="cliente" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="120" DataField="marca" HeaderText="Marca" UniqueName="marca" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="LightBlue" HeaderStyle-Width="100" DataField="nopedido" HeaderText="No. Pedido" UniqueName="nopedido" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="300" DataField="modelo" HeaderText="Modelo" UniqueName="modelo" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="150" DataField="sku" HeaderText="SKU" UniqueName="sku" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="100" DataField="totalpiezas" HeaderText="Total Piezas" UniqueName="totalpiezas" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="250" DataField="comprador" HeaderText="Comprador" UniqueName="comprador" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="250" DataField="clientefinal" HeaderText="Comprador Final" UniqueName="comprador" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="150" DataField="guia" HeaderText="Guía de Envío" UniqueName="guia" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="100" DataField="comentarios" HeaderText="Comentarios" UniqueName="comentarios" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="150" DataField="fullshopify" HeaderText="Fullfilled en Shopify" UniqueName="fullshopify" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="150" DataField="metodopago" HeaderText="Forma pago" UniqueName="fullshopify" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="80" DataField="factura" HeaderText="Factura" UniqueName="factura" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="110" DataField="montototal" HeaderText="Total" UniqueName="montototal" HeaderStyle-Font-Size="Small" DataType="System.Decimal" DataFormatString="{0:$###,##0.00}">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn HeaderStyle-BackColor="Silver" HeaderStyle-Width="110" DataField="idpago" HeaderText="ID. Pago" UniqueName="idpago" HeaderStyle-Font-Size="Small">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                    <EditFormSettings>
                                        <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                                        </EditColumn>
                                    </EditFormSettings>

                                </MasterTableView>
                                <ClientSettings>
                                    <Selecting AllowRowSelect="true"></Selecting>
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>
                    </td>
                </tr>
            </table>
        </fieldset>
    </telerik:RadAjaxPanel>
    <asp:Panel ID="panelConfirmacion" runat="server" Visible="false" Width="100%">
        <asp:Button ID="HiddenButtonOk" Text="" Style="display: none;" runat="server" />
        <asp:Button ID="HiddenButtonCancel" Text="" Style="display: none;" runat="server" />
    </asp:Panel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
    <div style="text-align: right; float: inherit; margin-right: 5px">
        <asp:Button ID="Button1" runat="server" Text="Descargar Packing List" OnClick="Button1_Click" CssClass="botones" />
        <br />
        <br />
    </div>
</asp:Content>
