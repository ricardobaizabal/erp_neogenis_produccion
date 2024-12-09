<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="editapedido.aspx.vb" Inherits="erp_neogenis.editapedido" MaintainScrollPositionOnPostback="true" %>

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
                                        <td class="item">
                                            <strong>Id Pedido: </strong>
                                            <asp:Label ID="lblidpedido" runat="server" class="item" Text=""></asp:Label>
                                        </td>
                                    </tr>          
                                    <tr>
                                        <td class="item">
                                            <strong>No. Ola: </strong>
                                            <asp:Label ID="lblola" runat="server" class="item" Text=""></asp:Label>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td class="item">
                                            <strong>Cliente: </strong>
                                            <asp:Label ID="lblRazonsocial" runat="server" class="item" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="item">
                                            <strong>Sucursal: </strong>
                                            <asp:Label ID="lblSucursal" runat="server" class="item" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="item">
                                            <strong>Almacén: </strong>
                                            <asp:Label ID="lblAlmacen" runat="server" class="item" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="item">
                                            <strong>Marca: </strong>
                                            <asp:Label ID="lblProyecto" runat="server" class="item" Text=""></asp:Label>
                                        </td>
                                    </tr>
                                    <asp:Panel ID="panelDatosFactura" runat="server" Visible="false">
                                        <tr>
                                            <td class="item">
                                                <strong>Orden de compra: </strong>
                                                <asp:Label ID="lblOrdenCompra" runat="server" class="item" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="item">
                                                <strong>Serie y folio: </strong>
                                                <asp:Label ID="lblSeriefolio" runat="server" class="item" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="item">
                                                <strong>Fecha de facturación: </strong>
                                                <asp:Label ID="lblFechafacturacion" runat="server" class="item" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="item">
                                                <strong>Total de la factura: </strong>
                                                <asp:Label ID="lblTotalFactura" runat="server" class="item" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    <tr>
                                        <td align="right" class="item">
                                            <asp:Panel ID="panelBusqueda" DefaultButton="btnSearch" runat="server">
                                                <asp:Label ID="lblBuscar" runat="server" class="item" Text="Buscar productos:"></asp:Label>
                                                <telerik:RadTextBox ID="txtSearch" runat="server" Width="200px" ValidationGroup="ValSearch"></telerik:RadTextBox>&nbsp;
                                                    <asp:Button ID="btnSearch" runat="server" Text="Buscar" CssClass="botones" ValidationGroup="ValSearch" />
                                                <asp:Button ID="btnCancelarBusqueda" runat="server" Text="Cancelar Búsqueda" CssClass="botones" />
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtSearch" ErrorMessage="Debe ingresar un texto sobre productos a buscar." ValidationGroup="ValSearch" class="item" ForeColor="Red" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="item">
                                            <fieldset style="width: 98%">
                                                <legend style="padding-right: 6px; color: Black">
                                                    <asp:Label ID="lblPedidoscsv" runat="server" Font-Bold="true" CssClass="item" Text="Carga de Conceptos Pedidos por CSV."></asp:Label>
                                                </legend>
                                                <div>
                                                    <table width="100%" cellspacing="3" cellpadding="3" align="right" style="line-height: 25px;">
                                                        <tr>
                                                            <td class="item" style="width: 27%;">
                                                                <strong>Seleccione archivo en formato CSV: </strong>
                                                            </td>
                                                            <td style="width: 35%;">
                                                                <asp:FileUpload ID="fileUploadPedido" runat="server" />
                                                            </td>
                                                            <td style="width: 10%;">
                                                                <asp:Button ID="btnCargaPedidosCsv" runat="server" CssClass="item" Visible="True" Text="Carga Pedidos CSV" />
                                                            </td>
                                                            <td class="item">
                                                                <asp:ImageButton ID="imgdownloadF" runat="server" ImageUrl="~/portalcfd/images/download.png" />
                                                                <span>Formato</span>
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td colspan="4">
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
                            <td align="left" class="item">
                                <asp:Label ID="lblMensaje" runat="server" class="item" ForeColor="Red"></asp:Label>
                                <asp:HiddenField runat="server" ID="ClienteId" Value="0" />
                                <asp:HiddenField runat="server" ID="estatusId" Value="0" />
                                <asp:HiddenField runat="server" ID="almacenId" Value="0" />
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
                                            <MasterTableView DataKeyNames="productoid,codigo,descripcion,unidad,unitario,disponibles,Cantidad" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No hay registros para mostrar.">
                                                <Columns>
                                                    <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="10%" FilterControlAltText="Filter column column" HeaderText="Código" UniqueName="codigo" HeaderStyle-Font-Size="Small"></telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="disponibles" ItemStyle-Width="5%" HeaderText="Disponibles" UniqueName="disponibles">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="Cantidad" runat="server" ItemStyle-Width="10%" HeaderText="Cantidad" UniqueName="cantidad"></telerik:GridBoundColumn>
                                                    <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="50%" FilterControlAltText="Filter column2 column" HeaderText="Descripción" UniqueName="descripcion">
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
                                                                <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="10%" FilterControlAltText="Filter column column" HeaderText="Código" UniqueName="codigo" HeaderStyle-Font-Size="Small"></telerik:GridBoundColumn>
                                                                <telerik:GridBoundColumn DataField="Cantidad" runat="server" ItemStyle-Width="10%" HeaderText="Cantidad" UniqueName="cantidad"></telerik:GridBoundColumn>
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

                                        <MasterTableView DataKeyNames="productoid,codigo,descripcion,unidad,unitario,existencia,disponibles" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No hay registros para mostrar.">

                                            <Columns>
                                                <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="10%" FilterControlAltText="Filter column column" HeaderText="Código" UniqueName="codigo" HeaderStyle-Font-Size="Small">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="existencia" ItemStyle-Width="50px" HeaderText="Existencia"></telerik:GridBoundColumn>--%>
                                                <telerik:GridBoundColumn DataField="disponibles" ItemStyle-Width="5%" HeaderText="Disponibles" UniqueName="disponibles">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn HeaderText="Cantidad" UniqueName="ColCantidad" ItemStyle-Width="10%">
                                                    <ItemTemplate>
                                                        <telerik:RadNumericTextBox ID="txtCantidad" Width="60px" Type="Number" NumberFormat-DecimalDigits="2" runat="server"></telerik:RadNumericTextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="50%" FilterControlAltText="Filter column2 column" HeaderText="Descripción" UniqueName="descripcion">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridNumericColumn DataField="unitario" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="10%" FilterControlAltText="Filter column column" HeaderText="Precio" UniqueName="unitario" DataType="System.Decimal" DataFormatString="{0:$###,##0.00}" NumericType="Currency">
                                                    </telerik:GridNumericColumn>
                                                    <telerik:GridBoundColumn DataField="unidad" FilterControlAltText="Filter column2 column" HeaderText="Unidad" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Right" UniqueName="unidad">
                                                    </telerik:GridBoundColumn>--%>
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
                        <asp:Button ID="btnAgregaConceptos" runat="server" CssClass="item" Visible="False" Text="Agregar Conceptos" />&nbsp;&nbsp;
                    </div>
                </td>
            </tr>
            <tr>
                <td align="left" style="vertical-align: top;">
                    <br />
                    <asp:Label ID="lblPedidotitulo" runat="server" Font-Bold="true" Font-Size="Small" class="item" Text="Detalle del pedido"></asp:Label>
                    <br />
                    <br />
                    <telerik:RadGrid Width="99.8%" ID="pedidodetallelist" runat="server" AllowPaging="True"
                        PageSize="50" AllowSorting="True" AutoGenerateColumns="False" CellSpacing="0"
                        Skin="Simple" HeaderStyle-Font-Size="Small" ShowHeader="true" ShowFooter="true">
                        <ClientSettings AllowColumnsReorder="True" ReorderColumnsOnClient="True">
                        </ClientSettings>
                        <ExportSettings HideStructureColumns="true" IgnorePaging="True" FileName="DetallePedido">
                            <Excel Format="Biff" />
                        </ExportSettings>
                        <MasterTableView DataKeyNames="id,pedidoid,productoid,codigo,descripcion,unidad,cantidad,precio,importe,itemsInBox" ShowHeadersWhenNoRecords="true" NoMasterRecordsText="No hay registros para mostrar." CommandItemDisplay="Top">
                            <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                            <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </RowIndicatorColumn>
                            <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column" Visible="True">
                                <HeaderStyle Width="20px" />
                            </ExpandCollapseColumn>
                            <Columns>
                                <telerik:GridBoundColumn DataField="upc" FilterControlAltText="Filter column column" HeaderText="UPC" UniqueName="upc" HeaderStyle-Font-Size="Small" Display="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="codigo" FilterControlAltText="Filter column column" HeaderText="Código" UniqueName="codigo" HeaderStyle-Font-Size="Small">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="descripcion" FilterControlAltText="Filter column2 column" HeaderText="Descripción" UniqueName="descripcion">
                                </telerik:GridBoundColumn>

                                <telerik:GridBoundColumn DataField="unidad" ItemStyle-HorizontalAlign="Right" FilterControlAltText="Filter column2 column" HeaderText="Unidad" UniqueName="unidad" Exportable="false">
                                </telerik:GridBoundColumn>

                                <%--<telerik:GridBoundColumn DataField="lote" HeaderText="Lote" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="caducidad" HeaderText="Caducidad" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>--%>

                                <telerik:GridNumericColumn DataField="precio" ItemStyle-HorizontalAlign="Right" FilterControlAltText="Filter column column" HeaderText="Precio" UniqueName="precio" DataType="System.Decimal" DataFormatString="{0:$###,##0.00}" NumericType="Currency">
                                </telerik:GridNumericColumn>

                                <%--<telerik:GridBoundColumn DataField="existencia" HeaderText="Existencia" UniqueName="existencia">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>

                                    <telerik:GridBoundColumn DataField="disponibles" HeaderText="Disponibles" UniqueName="disponibles">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridBoundColumn>--%>

                                <telerik:GridNumericColumn
                                    Aggregate="Sum" FooterText=" " FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true"
                                    DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad">
                                    <ItemStyle HorizontalAlign="Right" />
                                </telerik:GridNumericColumn>
                                <%--<telerik:GridTemplateColumn HeaderText="Cantidad" UniqueName="ColCantidad" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtCantidad" Width="60px" Type="Number" NumberFormat-DecimalDigits="2" MinValue="0" MaxValue="999" EnabledStyle-HorizontalAlign="Right" DisabledStyle-HorizontalAlign="Right" ReadOnlyStyle-HorizontalAlign="Right" runat="server">
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>

                                <%--<telerik:GridNumericColumn DataField="importe" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" FilterControlAltText="Filter column column" HeaderText="Importe" UniqueName="importe" DataType="System.Decimal" DataFormatString="{0:$###,##0.00}" NumericType="Currency" Aggregate="Sum" FooterAggregateFormatString="TOTAL: {0:$###,##0.00}">
                                    </telerik:GridNumericColumn>--%>

                                <telerik:GridNumericColumn
                                    Aggregate="Sum" FooterText="" FooterAggregateFormatString="{0:C}" FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true"
                                    DataField="importe" ItemStyle-HorizontalAlign="Right" FilterControlAltText="Filter column column" HeaderText="Importe" UniqueName="importe">
                                </telerik:GridNumericColumn>
                                <%--::::::::::--%>
                                <telerik:GridTemplateColumn HeaderStyle-Width="150px" AllowFiltering="false" ItemStyle-Font-Underline="true" ItemStyle-Font-Bold="true" HeaderText="No. Caja" UniqueName="noCajaColum" Exportable="false" Visible="true">
                                    <ItemTemplate>
                                        <telerik:RadTextBox ID="txtNoCaja" runat="server" Text='<%# Eval("noCaja") %>' AutoPostBack="true" OnTextChanged="txtNoCaja_TextChanged" Width="150px" MaxLength="20"></telerik:RadTextBox>
                                        <%--<asp:ImageButton Width="14" Height="14" ImageAlign="Middle" ID="btnViewBox2" runat="server" CommandArgument='<%# Eval("id") %>' OnClick="btnViewBox2_Click" CommandName="viewBoxs" ImageUrl="~/images/action_add.gif" />--%>

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width="50px" AllowFiltering="false" ItemStyle-Font-Underline="true" ItemStyle-Font-Bold="true" HeaderText="Cajas" UniqueName="viewboxs" Exportable="false" Visible="true">
                                    <ItemTemplate>
                                        <div style="display: flex">
                                            <asp:ImageButton Width="35" ID="btnViewBox" runat="server" CommandArgument='<%# Eval("ids") + "," + Eval("noCaja") %>' CommandName="viewBoxs" ImageUrl="~/images/icon-box.png" />
                                            <%--<asp:ImageButton Width="35" ID="btnViewBox" runat="server" CommandArgument='<%# Eval("Id") %>' CommandName="viewBoxs" ImageUrl="~/images/icon-box.png" />--%>
                                            <asp:Label ID="lblnumItems" Text='<%# Eval("itemsInBox") %>' ForeColor="Green" Font-Bold="false" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridNumericColumn
                                    Aggregate="Sum" FooterText=" " FooterStyle-HorizontalAlign="Right" FooterStyle-Font-Bold="true"
                                    DataField="empaquetado" HeaderText="Empaquetado" UniqueName="empaquetado">
                                    <ItemStyle HorizontalAlign="Right" />
                                </telerik:GridNumericColumn>
                                <%--::::::::::--%>
                                <%--<telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="ColRecalcularProducto" HeaderText="Recalcular">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnRecalcular" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdRecalcular" ImageUrl="~/images/action_calculate.gif" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>--%>

                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="ColEliminarProducto" HeaderText="Remover" Exportable="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEliminar" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEliminar" ImageUrl="~/images/action_delete.gif" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <EditFormSettings>
                                <EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
                            </EditFormSettings>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td align="center" style="vertical-align: middle; height: 30px;">
                    <asp:Label ID="lblPedidoError" runat="server" CssClass="merror2" Visible="false"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="left" style="height: 70px;" class="item">
                    <asp:Button ID="btnColocarPedido" runat="server" Text="Colocar" CssClass="botones" />&nbsp;&nbsp;&nbsp; 
                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" CssClass="botones" />&nbsp;
                    <asp:Button ID="btnAuth" runat="server" Text="Autorizar" CssClass="botones" />&nbsp;
                    <asp:Button ID="btnRechazar" runat="server" Text="Rechazar" CssClass="botones" />&nbsp;
                    <asp:Button ID="btnReactivar" runat="server" Text="Reactivar" CssClass="botones" />&nbsp;
                    <asp:Button ID="btnPack" runat="server" Text="Empaquetado" CssClass="botones" />&nbsp;
                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar Pedido" CssClass="botones" CausesValidation="false" />&nbsp; No. Guía:
                    <telerik:RadTextBox ID="txtGuia" Width="120px" runat="server"></telerik:RadTextBox>&nbsp;
                    <asp:Button ID="btnSent" runat="server" Text="Enviado" CssClass="botones" />&nbsp;
                    <asp:Button ID="btnImprimir" runat="server" Text="Descargar PDF" CssClass="botones" CausesValidation="false" />
                    <asp:Button ID="btnAsn" runat="server" Text="Generar 3pl" CssClass="botones" CausesValidation="false" Visible="false" />
                    <asp:Button ID="btnFacturar" runat="server" Text="Facturar" CssClass="botones" CausesValidation="false" />
                </td>
            </tr>
        </table>
    </fieldset>
    <%--</telerik:RadAjaxPanel>--%>
    <telerik:RadWindowManager ID="RadWindowManager1" Localization-OK="Aceptar" Localization-Cancel="Cancelar" runat="server">
    </telerik:RadWindowManager>
    <asp:Button ID="HiddenButtonOk" Text="" Style="display: none;" runat="server" />
    <asp:Button ID="HiddenButtonCancel" Text="" Style="display: none;" runat="server" />
    <!-- Start Modal Cancelar -->
    <telerik:RadWindow ID="WinAsnAutomatico" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" Behaviors="Close" VisibleOnPageLoad="False" Width="450" Height="250">
        <ContentTemplate>
            <table class="m-1em">
                <tr>
                    <td>Fecha de embarque
                    </td>
                    <td>Fecha y hora de entrega
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadDatePicker ID="fchEmbarque" runat="server">
                            <DateInput runat="server" DateFormat="dd MMM yyyy" />
                        </telerik:RadDatePicker>
                    </td>
                    <td>
                        <telerik:RadDateTimePicker ID="fchEntrega" runat="server" Width="200px">
                            <TimeView runat="server" TimeFormat="HH:mm"></TimeView>
                            <DateInput runat="server" DateFormat="dd MMM yyyy HH:mm">
                            </DateInput>
                        </telerik:RadDateTimePicker>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td>No. De Tienda 
                    </td>
                    <td>
                        <asp:Label runat="server" ID="lblNocaja" Text="No. de Caja" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadNumericTextBox ID="txtNoTienda" Width="60%" runat="server" Type="Number" MaxLength="5" NumberFormat-DecimalSeparator=" " NumberFormat-GroupSeparator="" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNoCaja" runat="server" MaxLength="19" Width="60%" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td class="text-aling-end" colspan="2">
                        <telerik:RadButton ID="btnDowload3pl" runat="server" Text="Descargar"></telerik:RadButton>
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfEsPrueba" Value="0" runat="server" />
        </ContentTemplate>
    </telerik:RadWindow>
    <!-- End Modal Cancelar -->
    <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>--%>

    <!-- Start Modal Cancelar -->
     <telerik:RadWindow ID="WinCajas" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" Behaviors="Close" VisibleOnPageLoad="False" Width="600" Height="300">
        <ContentTemplate>
            <div style="padding: 12px;">
                <table class="m-10" style="width: 100%;">
                    <tr>
                        <td>
                            <strong>Número de Caja</strong>
                        </td>
                        <td>
                            <strong>Cantidad</strong>
                        </td>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtNumCaja" runat="server" Width="280px" MaxLength="500"></asp:TextBox>
                        </td>
                        <td>
                            <telerik:RadNumericTextBox ID="txtCantidadCaja" runat="server" Type="Number"  Width="120px"></telerik:RadNumericTextBox>
                        </td>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" InitialValue="0" ValidationGroup="CajasVal" ErrorMessage="Proporcione un número de caja" ControlToValidate="txtNumCaja" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0" ValidationGroup="CajasVal" ErrorMessage="Proporcione una Cantidada"  ControlToValidate="txtCantidadCaja" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Guia</strong>
                        </td>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:TextBox ID="txtGuiaCaja" runat="server" Width="280px" MaxLength="500"></asp:TextBox>

                        </td>
                        <td>
                            <telerik:RadButton ID="btnSaveCaja" runat="server" CausesValidation="True" Text="Guardar"></telerik:RadButton>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                        </td>
                        <td>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblMensajeCaja" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                        </td>
                        <td>
                            <asp:HiddenField ID="conceptoid" runat="server" Value="0" />
                            <asp:HiddenField ID="nocajaHF" runat="server" Value="0" />
                            <asp:HiddenField ID="cajaid" runat="server" Value="0" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <br />
                            <telerik:RadGrid ID="CajasList" runat="server" CssClass="w-100"
                                ShowStatusBar="true" AutoGenerateColumns="False" AllowPaging="True"
                                PageSize="50" GridLines="None" AllowSorting="True" ShowHeader="true" ShowFooter="true">
                                <ExportSettings FileName="Prestamos en Ruta" ExportOnlyData="true">
                                    <Excel Format="Biff" />
                                </ExportSettings>
                                <MasterTableView DataKeyNames="id" Name="tblList" AllowMultiColumnSorting="False" NoMasterRecordsText="No se han agregado registros" CommandItemDisplay="None">
                                    <CommandItemSettings ShowExportToExcelButton="true" ShowRefreshButton="false" ShowAddNewRecordButton="false" />
                                    <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column"
                                        Visible="True">
                                    </RowIndicatorColumn>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="id" HeaderText="Folio"
                                            UniqueName="id" Display="false">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Editar" Exportable="false" UniqueName="editar">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="lnkEditar" ImageAlign="AbsMiddle" runat="server" ImageUrl="~/images/action_edit.png" CommandArgument='<%# Eval("id") %>' CommandName="cmdEditar" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridBoundColumn DataField="nocaja" HeaderText="Caja"
                                            UniqueName="nocaja">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridNumericColumn Aggregate="Sum" FooterText="Total: " DataField="numCantdad" HeaderText="Cantidad" FooterStyle-HorizontalAlign="Center" UniqueName="numCantdad" NumericType="Number" DecimalDigits="2">
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </telerik:GridNumericColumn>
                                        <telerik:GridBoundColumn DataField="guia" HeaderText="Guia" UniqueName="guia">
                                             <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle HorizontalAlign="Center" />
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="False"
                                            HeaderStyle-HorizontalAlign="Center" UniqueName="Delete" HeaderText="Eliminar" Exportable="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEliminar" ImageUrl="~/images/action_delete.gif" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </telerik:RadWindow>
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
