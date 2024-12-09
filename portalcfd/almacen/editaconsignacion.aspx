<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" MaintainScrollPositionOnPostback="true" CodeBehind="editaconsignacion.aspx.vb" Inherits="erp_neogenis.editaconsignacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        var elementToFocusID = null;
        function OnRequestStart(target, arguments) {
            elementToFocusID = document.activeElement.id;
            var eventTarget = arguments.get_eventTarget();
            if (eventTarget == "<%= btnRegresarInventario.UniqueID%>") {
                return confirm('Va a regresar al inventario de Natural GS los productos seleccionados, ¿Desea continuar?');
            }
            if ((arguments.get_eventTarget().indexOf("btnFacturar") > -1) || (arguments.get_eventTarget().indexOf("btnImprimir") > -1) || (arguments.get_eventTarget().indexOf("btnRegresar") > -1) || (arguments.get_eventTarget().indexOf("productsList") > -1)) {
                arguments.set_enableAjax(false);
            }
        }

        function onResponseEnd(sender, args) {
            if (elementToFocusID) {
                if (document.getElementById(elementToFocusID)) {
                    document.getElementById(elementToFocusID).focus();
                }
            }

            elementToFocus = null;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart" ClientEvents-OnResponseEnd="onResponseEnd">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblConsignacion" runat="server" Font-Bold="true" CssClass="item" Text="Editando lote de consignación"></asp:Label>
            </legend>
            <br />
            <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                <tr>
                    <td class="item" style="line-height: 20px;">
                        <strong>No. Lote:</strong>
                        <asp:Label ID="lblFolio" runat="server"></asp:Label><br />
                        <strong>Fecha:</strong>
                        <asp:Label ID="lblFecha" runat="server"></asp:Label><br />
                        <strong>Almacén Origen:</strong>
                        <asp:Label ID="lblAlmacenOrigen" runat="server"></asp:Label><br />
                        <strong>Cliente:</strong>
                        <asp:Label ID="lblCliente" runat="server"></asp:Label><br />
                        <strong>Vendedor:</strong>
                        <asp:Label ID="lblVendedor" runat="server"></asp:Label><br />
                        <strong>Comentario:</strong><br />
                        <asp:Label ID="lblComentario" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblAgregaProducto" runat="server" Font-Bold="true" CssClass="item" Text="Agregar productos"></asp:Label>
            </legend>
            <br />
            <asp:Panel DefaultButton="btnSearch" runat="server">
                <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                    <tr>
                        <td class="item">
                            <table border="0" cellpadding="0" cellspacing="0" align="center" width="100%">
                                <tr>
                                    <td style="width: 5%;">
                                        <strong>Buscar:</strong>
                                    </td>
                                    <td style="width: 15%;">
                                        <asp:TextBox ID="txtSearchItem" runat="server" CssClass="box" AutoPostBack="false"></asp:TextBox>&nbsp;
                                    </td>
                                    <td style="width: 6%;">
                                        <asp:Button ID="btnSearch" CausesValidation="false" runat="server" Text="Buscar" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" CssClass="item"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr style="height: 30px;">
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadGrid ID="gridResults" runat="server" Width="100%" ShowStatusBar="True" AutoGenerateColumns="False" AllowPaging="False" GridLines="None" Skin="WebBlue">
                                <MasterTableView Width="100%" DataKeyNames="productoid,existencia,disponibles" Name="Items" AllowMultiColumnSorting="False">
                                    <Columns>

                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>Código</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCodigo" runat="server" Text='<%# eval("codigo") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>Cant.</HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox ID="txtCantidad" runat="server" Skin="Default" Width="50px" MinValue="0" Value='0'>
                                                    <NumberFormat DecimalDigits="4" GroupSeparator="" />
                                                </telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>Unidad</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblUnidad" runat="server" Text='<%# eval("unidad") %>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>Existencia</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblExistencia" runat="server" Text='<%# Eval("existencia")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn>
                                            <HeaderTemplate>Disponibles</HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDisponibles" runat="server" Text='<%# Eval("disponibles")%>'></asp:Label>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <%--<telerik:GridBoundColumn DataField="almacen" HeaderText="Almacen"></telerik:GridBoundColumn>--%>

                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <div align="right">
                                <asp:Button ID="btnAgregaConceptos" runat="server" CssClass="item" Visible="False" Text="Agregar Conceptos" />&nbsp;&nbsp;
                            </div>
                        </td>
                    </tr>
                    <tr style="height: 5px;">
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
        </fieldset>
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="item" Text="Lista de productos en Consignación"></asp:Label>
            </legend>
            <br />
            <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="productsList" runat="server" Width="100%" ShowStatusBar="True" AutoGenerateColumns="False" GridLines="None" ShowFooter="true" PageSize="50" Skin="WebBlue">
                            <ExportSettings IgnorePaging="True" FileName="DetalleConsignacion">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView Width="100%" DataKeyNames="productoid,codigo,descripcion,unidad,precio,cantidad,disponible" Name="Products" AllowMultiColumnSorting="False" CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo" ItemStyle-Width="100px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100px">
                                        <HeaderTemplate>Cant. Fact/Reg</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtCantidad" runat="server" Skin="Default" AutoPostBack="true" OnTextChanged="txtCantidad_TextChanged" Width="60px" MinValue="0" Value="0">
                                                <NumberFormat DecimalDigits="4" GroupSeparator="" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unidad" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precio" HeaderText="Unitario" UniqueName="precio" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" UniqueName="importe" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Inicial" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="facturado" HeaderText="Facturado" UniqueName="facturado" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="regresado" HeaderText="Regresado" UniqueName="regresado" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="disponible" HeaderText="Disponible" UniqueName="disponible" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="60px">
                                    </telerik:GridBoundColumn>
                                    <%--<telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" CausesValidation="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>--%>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Label ID="lblTotalPiezas" Font-Bold="true" Font-Size="Medium" runat="server" CssClass="item"></asp:Label>
                    </td>
                </tr>
            </table>
        </fieldset>
        <br />
        <br />
        <%--<asp:Button ID="btnProcesar" runat="server" CausesValidation="false" Text="Procesar consignación" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
        <asp:Button ID="btnFacturar" runat="server" Text="Facturar" CausesValidation="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnRegresarInventario" runat="server" Text="Regresar a Natural" CausesValidation="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" CssClass="botones" CausesValidation="false" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CausesValidation="false" /><br />
        <br />
        <asp:Label ID="lblMensajeFacturar" runat="server" ForeColor="Red" CssClass="item"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <br />
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
