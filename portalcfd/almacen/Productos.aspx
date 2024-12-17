<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_Productos" MaintainScrollPositionOnPostback="true" CodeBehind="Productos.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4 {
            height: 25px;
        }
    </style>
    <script type="text/javascript">

        function OnRequestStart(target, arguments) {
            if ((arguments.get_eventTarget().indexOf("pedidosList") > -1) || (arguments.get_eventTarget().indexOf("btnExportExcel") > -1)) {
                arguments.set_enableAjax(false);
            }
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">--%>
    <asp:Panel ID="panelProductList" runat="server">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/buscador_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblFiltros" Text="Buscador" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table border="0" style="width: 100%;">
                <tr>
                    <td style="width: 10%;">
                        <span class="item">Palabra clave:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox ID="txtSearch" runat="server" Width="92%" CssClass="box"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span class="item">Marca:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:DropDownList ID="filtromarcaid" runat="server" Width="95%" CssClass="box"></asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span class="item">UPC:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:TextBox ID="upcSearch" runat="server" Width="92%" CssClass="box"></asp:TextBox>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <span class="item">Temporada:</span>
                    </td>
                    <td style="width: 20%;">
                        <asp:DropDownList ID="filtrocoleccionid" runat="server" Width="95%" CssClass="box"></asp:DropDownList>
                    </td>
                    <td align="left">
                        <asp:Button ID="btnSearch" runat="server" CssClass="boton" Text="Buscar" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAll" runat="server" CssClass="boton" Text="Ver todo" />&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <div>
                <br />
                <asp:CheckBox ID="ckfiltroExistencia" CssClass="item" runat="server" Text="Mostrar solamente productos con existencia disponible." />
                <%--&nbsp;&nbsp;&nbsp;<asp:Label ID="lblEstatusFiltro" runat="server" Text="Estatus:" CssClass="item" Font-Bold="True"></asp:Label>&nbsp;&nbsp;<asp:DropDownList ID="estatusproductoid" AutoPostBack="false" runat="server" CssClass="box"></asp:DropDownList>--%>
            </div> 
        </fieldset>
        <br />
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/ListadoProductos_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblProductsListLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid ID="productslist" runat="server" AllowPaging="True" AllowSorting="True"
                            AutoGenerateColumns="False" GridLines="None" ShowFooter="True"
                            PageSize="50" ShowStatusBar="True" ExportSettings-ExportOnlyData="False"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <ExportSettings IgnorePaging="True" FileName="CatalogoProductos">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False" NoMasterRecordsText="No existen productos registrados" CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="shfg" UniqueName="Codigo" ItemStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Text='<%# Eval("codigo") %>' CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" CausesValidation="false" Width="100px"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="upc" HeaderText="UPC" UniqueName="upc">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="" ItemStyle-Width="30%" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion_corta" HeaderText="Descripción Corta" ItemStyle-Width="30%" UniqueName="descripcion_corta" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="proyecto" HeaderText="Marca" ItemStyle-Width="10%" UniqueName="proyecto">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="coleccion" HeaderText="Temporada" ItemStyle-Width="10%" UniqueName="coleccion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario" HeaderText="" UniqueName="unitario" DataFormatString="{0:c}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario2" HeaderText="" UniqueName="unitario2" DataFormatString="{0:c}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario3" HeaderText="" UniqueName="unitario3" DataFormatString="{0:c}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unitario4" HeaderText="" UniqueName="unitario4" DataFormatString="{0:c}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="costo" HeaderText="Costo Estándar " UniqueName="costo" Display="false" DataFormatString="{0:c}" Exportable="false" HeaderStyle-Width="100" />
                                    <telerik:GridBoundColumn DataField="promedio" HeaderText="Costo Promedio" UniqueName="promedio" Display="false" DataFormatString="{0:c}" Exportable="false" HeaderStyle-Width="100" />
                                    <telerik:GridTemplateColumn UniqueName="TemplateColumn1">
                                        <HeaderTemplate>Matriz</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lnkVer" runat="server" Text='<%# Eval("matriz") %>'></asp:Label>
                                            <telerik:RadToolTip ID="RadToolTip2" runat="server" TargetControlID="lnkVer" RelativeTo="Element" Position="BottomCenter" RenderInPageRoot="true" ManualClose="true"><%#Eval("ubicacion")%></telerik:RadToolTip>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%--<telerik:GridNumericColumn Aggregate="Sum" FooterText=" " FooterStyle-HorizontalAlign="Right"
                                        FooterStyle-Font-Bold="true" DataField="proceso" HeaderText="En proceso" UniqueName="proceso">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridNumericColumn>--%>
                                    <telerik:GridTemplateColumn UniqueName="TemplateColumn2" ItemStyle-HorizontalAlign="Right">
                                        <HeaderTemplate>En proceso</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lnkVerProceso" runat="server" Text='<%# Eval("proceso") %>'></asp:Label>
                                            <telerik:RadToolTip ID="RadToolTip3" runat="server" TargetControlID="lnkVerProceso" RelativeTo="Element" Position="BottomCenter" RenderInPageRoot="true" ManualClose="true"><%#Eval("en_proceso")%></telerik:RadToolTip>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn ItemStyle-Width="20px" UniqueName="consignacion" AllowSorting="true">
                                        <HeaderTemplate>En consignación</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFolio" runat="server" Text='<%# Eval("consignacion") %>'></asp:Label>
                                            <telerik:RadToolTip ID="RadToolTip1" runat="server" TargetControlID="lblFolio" RelativeTo="Element" Position="BottomCenter" RenderInPageRoot="true" ManualClose="true"><%#Eval("detalle_consignacion")%></telerik:RadToolTip>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridNumericColumn Aggregate="Sum" FooterText=" " FooterStyle-HorizontalAlign="Right"
                                        FooterStyle-Font-Bold="true" DataField="disponibles" HeaderText="Disponibles" UniqueName="disponibles">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </telerik:GridNumericColumn>

                                    <telerik:GridBoundColumn DataField="modeloEstilo" HeaderText="Modelo-Estilo" UniqueName="modeloEstilo" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="plataforma" HeaderText="Plataforma" UniqueName="plataforma" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="genero" HeaderText="Género" UniqueName="genero" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="tallaUSA" HeaderText="Talla USA" UniqueName="tallaUSA" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="tallaMX" HeaderText="Talla MX" UniqueName="tallaMX" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="color" HeaderText="Color" UniqueName="color" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="colorMx" HeaderText="Color Mx" UniqueName="colorMx" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="material" HeaderText="Material" UniqueName="material" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="pesoKg" HeaderText="Peso" UniqueName="pesoKg" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="empaqueAlto" HeaderText="Empaque alto" UniqueName="empaqueAlto" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="empaqueLargo" HeaderText="Empaque largo" UniqueName="empaqueLargo" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="empaqueAncho" HeaderText="Empaque ancho" UniqueName="empaqueAncho" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unidad Medida" UniqueName="unidad" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="claveprodserv" HeaderText="Clave Producto" UniqueName="claveprodserv" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridBoundColumn DataField="Tasa" HeaderText="Tasa" UniqueName="Tasa" Display="false" Exportable="false" HeaderStyle-Width="100" />

                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete" Exportable="false">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" BorderStyle="None" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                    <telerik:RadToolTip runat="server" ID="tooltip"></telerik:RadToolTip>
                </tr>
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="height: 5px">
                        <asp:Button ID="btnDowloadCatalogo" Text="Descargar catálogo" runat="server" CausesValidation="False" CssClass="item" TabIndex="6" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnAddProduct" runat="server" CausesValidation="False" CssClass="item" TabIndex="6" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 5px">&nbsp;</td>
                </tr>
            </table>
        </fieldset>
        <br />
    </asp:Panel>

    <asp:Panel ID="panelProductRegistration" runat="server" Visible="False">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/images/icons/AgregarEditarProducto_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblProductEditLegend" runat="server" Font-Bold="True" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table border="0" width="100%">
                <tr>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblCode" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblUnit" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblFoto" runat="server" CssClass="item" Font-Bold="true" Text="Foto:"></asp:Label>
                    </td>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblColeccion" runat="server" CssClass="item" Font-Bold="true" Text="Temporada:"></asp:Label>
                    </td>
                    <td class="style4" rowspan="6" align="right">
                        <asp:Image ID="imgFoto" runat="server" Width="100%" ImageAlign="AbsMiddle" /><br />
                        <asp:Label ID="lblImgFoto" runat="server" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="20%">
                        <telerik:RadTextBox ID="txtCode" runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td class="style4" width="20%">
                        <asp:DropDownList ID="cboclaveunidad" runat="server" CssClass="box" Width="85%"></asp:DropDownList>
                    </td>
                    <td class="style4" width="20%">
                        <asp:FileUpload ID="foto" Width="100%" runat="server" />
                    </td>
                    <td class="style4" width="20%">
                        <asp:DropDownList ID="coleccionid" runat="server" Width="95%" CssClass="box"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="20%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Requerido" ControlToValidate="txtCode" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style4" width="20%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Requerido" InitialValue="0" ControlToValidate="cboclaveunidad" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                </tr>
                <tr>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblUnitaryPrice" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblUnitaryPrice2" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblUnitaryPrice3" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="20%">
                        <asp:Label ID="lblUnitaryPrice4" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="20%">
                        <telerik:RadNumericTextBox ID="txtUnitaryPrice" runat="server" MinValue="0" Type="Currency"
                            Skin="Default" Value="0" Width="85%">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td class="style4" width="20%">
                        <telerik:RadNumericTextBox ID="txtUnitaryPrice2" runat="server" MinValue="0" Type="Currency"
                            Skin="Default" Value="0" Width="85%">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td class="style4" width="20%">
                        <telerik:RadNumericTextBox ID="txtUnitaryPrice3" runat="server" MinValue="0" Type="Currency"
                            Skin="Default" Value="0" Width="85%">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td class="style4" width="20%">
                        <telerik:RadNumericTextBox ID="txtUnitaryPrice4" runat="server" MinValue="0" Type="Currency"
                            Skin="Default" Value="0" Width="85%">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="20%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtUnitaryPrice" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="20%">
                        <asp:Label ID="lblUPC" runat="server" CssClass="item" Text="UPC:" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="20%">
                        <asp:Label ID="lblClaveProdServ" runat="server" CssClass="item" Text="Clave producto / servicio:" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="20%">&nbsp;</td>
                    <td width="20%">&nbsp;</td>
                </tr>
                <tr>
                    <td width="20%">
                        <telerik:RadTextBox ID="txtUPC" runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td width="20%">
                        <asp:DropDownList ID="cboproductoserv" runat="server" CssClass="box" Width="85%"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="20%">&nbsp;</td>
                    <td width="20%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Requerido" InitialValue="0" ControlToValidate="cboproductoserv" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                        <asp:Label ID="lblDescription" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="20%">&nbsp;</td>
                    <td width="20%">
                        <asp:Label ID="lblDescriptionCorta" runat="server" Text="Descripción Corta:" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtDescription" runat="server" Width="95%">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtDescriptionCorta" runat="server" Width="60%">
                        </telerik:RadTextBox>
                    </td>
                    <td width="20%">&nbsp;</td>
                    <td width="20%">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtDescription" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td colspan="5" style="line-height: 30px;">
                        <asp:Label ID="lblPresentacion" runat="server" CssClass="item" Font-Bold="true" Text="Presentación:"></asp:Label><br />
                        <telerik:RadTextBox ID="txtPresentacion" runat="server" Width="95%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblTasa" runat="server" CssClass="item" Font-Bold="true" Text="Tasa:"></asp:Label>&nbsp;<asp:RequiredFieldValidator ID="valTasa" runat="server" ControlToValidate="tasaid" ForeColor="Red" SetFocusOnError="true" CssClass="item" ErrorMessage="Requerido" InitialValue="0"></asp:RequiredFieldValidator><br />
                        <asp:DropDownList ID="tasaid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblMaximo" runat="server" CssClass="item" Font-Bold="true" Text="Máximo:"></asp:Label><br />
                        <telerik:RadNumericTextBox ID="txtMaximo" runat="server" MinValue="0"
                            Skin="Default" Value="0">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblMinimo" runat="server" CssClass="item" Font-Bold="true" Text="Mínimo:"></asp:Label><br />
                        <telerik:RadNumericTextBox ID="txtMinimo" runat="server" MinValue="0"
                            Skin="Default" Value="0">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblReorden" runat="server" CssClass="item" Font-Bold="true" Text="Punto reorden:"></asp:Label><br />
                        <telerik:RadNumericTextBox ID="txtReorden" runat="server" MinValue="0"
                            Skin="Default" Value="0">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblCostoStd" runat="server" CssClass="item" Font-Bold="true" Text="Costo estándar:"></asp:Label><br />
                        <telerik:RadNumericTextBox ID="txtCostoStd" runat="server" MinValue="0" Type="Currency"
                            Skin="Default" Value="0">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblCompraMin" runat="server" CssClass="item" Font-Bold="true" Text="Compra mínima:"></asp:Label><br />
                        <telerik:RadNumericTextBox ID="txtCompraMinima" runat="server" MinValue="0"
                            Skin="Default" Value="0">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblTiempoEntrega" runat="server" CssClass="item" Font-Bold="true" Text="Tiempo de entrega:"></asp:Label><br />
                        <telerik:RadTextBox ID="txtTiempoEntrega" runat="server">
                        </telerik:RadTextBox>
                    </td>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblMoneda" runat="server" CssClass="item" Font-Bold="true" Text="Moneda:"></asp:Label><br />
                        <asp:DropDownList ID="monedaid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td style="line-height: 30px;">
                        <asp:Label ID="lblPesoUnitario" runat="server" CssClass="item" Font-Bold="true" Text="Peso Unit. (Kgs):"></asp:Label><br />
                        <telerik:RadNumericTextBox ID="txtPesoUnitario" runat="server" MinValue="0"
                            Skin="Default" Value="0">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblCostoProm" runat="server" CssClass="item" Font-Bold="true" Text="Costo promedio:"></asp:Label><br />
                        <telerik:RadNumericTextBox ID="txtCostoProm" runat="server" MinValue="0"
                            Skin="Default" Value="0" Enabled="true">
                            <NumberFormat DecimalDigits="2" GroupSeparator="," />
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblProyecto" runat="server" CssClass="item" Font-Bold="true" Text="Marca:"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red" SetFocusOnError="true" ErrorMessage="Requerido" ControlToValidate="proyectoid" InitialValue="0" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="2">
                        <asp:Label ID="lblProveedor" runat="server" CssClass="item" Font-Bold="true" Text="Proveedor:"></asp:Label>
                    </td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="proyectoid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td colspan="2">
                        <asp:DropDownList ID="proveedorid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td style="line-height: 30px; vertical-align: bottom;">
                        <asp:CheckBox ID="chkInventariableBit" Font-Bold="true" runat="server" Text="Producto Inventariable" CssClass="item" />&nbsp;&nbsp;
                        <%--<asp:CheckBox ID="chkManiobraBit" Font-Bold="true" runat="server" Text="Este producto será considerado como una maniobra" CssClass="item" />--%>
                    </td>
                    <td style="line-height: 30px; vertical-align: bottom;">
                        <asp:CheckBox ID="chkPerecederoBit" Font-Bold="true" runat="server" Text="Producto Perecedero" CssClass="item" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="5" style="border-spacing: 0px;">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 20%;">
                                    <asp:Label ID="lblmodeloEstilo" runat="server" CssClass="item" Font-Bold="true" Text="Modelo-Estilo:" /><br />
                                    <telerik:RadTextBox Style="margin-top: 8px;" ID="txtmodeloEstilo" runat="server" Width="88%" />
                                </td>
                                <td style="width: 20%;">
                                    <asp:Label ID="lblplataforma" runat="server" CssClass="item" Font-Bold="true" Text="Plataforma:" /><br />
                                    <telerik:RadTextBox Style="margin-top: 8px;" ID="txtplataforma" runat="server" Width="88%" />
                                </td>
                                <td style="width: 40%;">
                                    <div style="width: 100%; display: flex;">
                                        <div style="width: 50%;">
                                            <asp:Label ID="lblgeneroid" runat="server" CssClass="item" Font-Bold="true" Text="Género:" /><br />
                                            <asp:DropDownList ID="cmbgeneroid" DataTextField="descripcion" DataValueField="id" runat="server" CssClass="box" Width="50%" Style="margin-top: 8px;"></asp:DropDownList>
                                        </div>
                                        <div style="width: 100%; display: flex;">
                                            <div style="width: 100%">
                                                <asp:Label ID="lbltallaUSA" runat="server" CssClass="item" Font-Bold="true" Text="Talla USA:" /><br />
                                                <telerik:RadTextBox Style="margin-top: 8px;" ID="txttallaUSA" runat="server" Width="76%" />
                                            </div>
                                            <div style="width: 100%">
                                                <asp:Label ID="lbltallaMX" runat="server" CssClass="item" Font-Bold="true" Text="Talla MX:" /><br />
                                                <telerik:RadTextBox Style="margin-top: 8px;" ID="txttallaMX" runat="server" Width="76%" />
                                            </div>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 20%;">
                                    <asp:Label ID="lblmaterial" runat="server" CssClass="item" Font-Bold="true" Text="Material:" /><br />
                                    <telerik:RadTextBox Style="margin-top: 8px;" ID="txtmaterial" runat="server" Width="88%" />
                                </td>
                                <td style="width: 20%;">
                                    <asp:Label ID="lblpesoKg" runat="server" CssClass="item" Font-Bold="true" Text="Peso:" /><br />
                                    <telerik:RadTextBox Style="margin-top: 8px;" ID="txtpesoKg" runat="server" Width="80%" />
                                </td>
                                <td colspan="2">
                                    <div style="width: 100%; display: flex;">
                                        <div style="width: 33%">
                                            <asp:Label ID="lblempaqueAlto" runat="server" CssClass="item" Font-Bold="true" Text="Empaque alto:" /><br />
                                            <telerik:RadTextBox Style="margin-top: 8px;" ID="txtempaqueAlto" runat="server" Width="70%" />
                                        </div>
                                        <div style="width: 34%">
                                            <asp:Label ID="lblempaqueLargo" runat="server" CssClass="item" Font-Bold="true" Text="Empaque largo:" /><br />
                                            <telerik:RadTextBox Style="margin-top: 8px;" ID="txtempaqueLargo" runat="server" Width="70%" />
                                        </div>
                                        <div style="width: 31%">
                                            <asp:Label ID="lblempaqueAncho" runat="server" CssClass="item" Font-Bold="true" Text="Empaque ancho:" /><br />
                                            <telerik:RadTextBox Style="margin-top: 8px;" ID="txtempaqueAncho" runat="server" Width="70%" />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="20%;">
                        <asp:Label ID="lblcolor" runat="server" CssClass="item" Font-Bold="true" Text="Color:" /><br />
                    </td>
                    <td class="style4" width="20%;">
                        <asp:Label ID="lblcolorMx" runat="server" CssClass="item" Font-Bold="true" Text="Color MX:" /><br />
                    </td>
                    <td class="style4" width="20%">
                        <asp:Label ID="Label2" runat="server" CssClass="item" Font-Bold="True" Text="Objeto de impuesto"></asp:Label>
                    </td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                </tr>
                <tr>
                    <td class="style4" width="20%">
                        <telerik:RadTextBox Style="margin-top: 8px;" ID="txtcolor" runat="server" Width="76%" /></td>
                    <td class="style4" width="20%">
                        <telerik:RadTextBox Style="margin-top: 8px;" ID="txtcolorMX" runat="server" Width="76%" /></td>
                    <td class="style4" width="20%">
                        <asp:DropDownList ID="cbmObjetoImpuesto" runat="server" CssClass="box" Width="85%"></asp:DropDownList>
                    </td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                </tr>
                <tr>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Requerido" InitialValue="0" ControlToValidate="cbmObjetoImpuesto" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td class="style4" width="20%">&nbsp;</td>
                    <td class="style4" width="20%">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="lblMarketplace" runat="server" CssClass="item" Font-Bold="true" Text="Marketplaces:" Width="100%"></asp:Label><br />
                        <asp:CheckBoxList ID="ckMarketplaces" runat="server" DataTextField="nombre" DataValueField="id" CellPadding="5" RepeatColumns="5" CssClass="item" RepeatDirection="Horizontal" Width="100%"></asp:CheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />

                        <asp:Button ID="btnSaveProduct" runat="server" Width="80px" CssClass="item" />&nbsp;&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" Width="80px" CssClass="item" CausesValidation="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <asp:Label ID="lblMensaje" runat="server" Font-Bold="true" CssClass="item" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="width: 66%; height: 5px;">
                        <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                        <asp:HiddenField ID="ProductID" runat="server" Value="0" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="panelStockLocator" runat="server" Visible="False">
                <table border="0" width="100%">
                    <tr>
                        <td colspan="6" style="width: 100%; background-color: GrayText; color: White; font-family: Arial; padding-left: 10px; height: 25px;">
                            <asp:Label ID="Label3" runat="server" Text="Stock Location"></asp:Label>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <br />
                            <telerik:RadGrid ID="ProductCodesList" runat="server" Width="30%" ShowStatusBar="True"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="10" GridLines="None"
                                Skin="Simple">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <MasterTableView Width="100%" NoMasterRecordsText="No se encontraron registros." DataKeyNames="id" Name="Codes" AllowMultiColumnSorting="False">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="CodigoBarrasLocacion" ItemStyle-Width="35" HeaderText="Codigo de Barras Locacion" UniqueName="CodigoBarrasLocacion">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="Cantidad" HeaderText="Cantidad" ItemStyle-Width="25" UniqueName="Cantidad">
                                        </telerik:GridBoundColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <br />
            </asp:Panel>
            <asp:Panel ID="panelCodigosAlternos" runat="server" Visible="False">
                <table border="0" width="100%">
                    <tr>
                        <td style="width: 100%; background-color: GrayText; color: White; font-family: Arial; padding-left: 10px; height: 25px;">
                            <asp:Label ID="Label1" runat="server" Text="Códigos alternos para clientes"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="clienteid" runat="server" CssClass="item"></asp:DropDownList>&nbsp;&nbsp;&nbsp;<telerik:RadTextBox ID="txtClientCode" runat="server" CssClass="item"></telerik:RadTextBox>&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnAddCode" runat="server" Text="Agregar" CssClass="item" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <telerik:RadGrid ID="ClientCodesList" runat="server" Width="70%" ShowStatusBar="True"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" GridLines="None"
                                Skin="Simple">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <MasterTableView Width="100%" DataKeyNames="id" Name="Codes" AllowMultiColumnSorting="False" NoMasterRecordsText="No se encontraron registros.">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" UniqueName="cliente">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                        </telerik:GridBoundColumn>

                                        <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"
                                            UniqueName="Delete">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>'
                                                    CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" />
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
            </asp:Panel>
        </fieldset>
        <br />
        <br />
        <br />
    </asp:Panel>

    <asp:Panel ID="panelFileUp" runat="server">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="Label5" runat="server" Font-Bold="true" CssClass="item" Text="Carga de Productos por CSV."></asp:Label>
            </legend>
            <table width="100%" cellspacing="3" cellpadding="3" align="right" style="line-height: 25px;">
                <tr>
                    <td class="item" style="width: 27%;">
                        <strong>Seleccione archivo en formato CSV: </strong>
                    </td>
                    <td style="width: 35%;">
                        <asp:FileUpload ID="fileUploadExcel1" runat="server" />
                    </td>
                    <td style="width: 10%;">
                        <asp:Button ID="btnCargarExcel" runat="server" Text="Cargar CSV" />
                    </td>
                    <td class="item">
                        <asp:ImageButton ID="imgDownload" runat="server" ImageUrl="~/portalcfd/images/download.png" />
                        <span>Formato</span>
                    </td>
                    <%--<td class="item">
                        <asp:Button ID="btnAgregaConceptos" runat="server" CssClass="item" Visible="false" Text="Agregar Productos" />&nbsp;&nbsp;
                    </td>--%>
                    <td class="item">
                        <asp:Label ID="Label6" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                        <asp:Button ID="btnAddorderParcial" runat="server" Enabled="false" Text="Dar de Alta Productos" BackColor="Green" ForeColor="White" />
                    </td>
                    <td colspan="5">
                        <asp:Label ID="lblMensajeCSV" runat="server" Font-Bold="true" CssClass="item" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:HiddenField ID="cargaidHidden" Value="0" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelCSV" runat="server" Visible="false">
        <fieldset style="width: 100%">
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblConceptosCarga" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Carga CSV Correctos"></asp:Label>
            </legend>
            <telerik:RadGrid ID="resultslistCSV" runat="server" Width="100%" ShowStatusBar="True"
                AutoGenerateColumns="False" AllowPaging="True" GridLines="None" Skin="Simple">
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView NoMasterRecordsText="No se encontraron registros." Width="100%" DataKeyNames="codigo,upc,descripcion,claveSat,unidad,marcaId,coleccionId,precioUnit1,precioUnit2,precioUnit3,precioUnit4,precioUnit5,precioUnit6" Name="Products" AllowMultiColumnSorting="False">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="upc" ItemStyle-Width="100" HeaderText="upc" UniqueName="upc">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="250" HeaderText="Descripción" UniqueName="descripcion">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="claveSat" ItemStyle-Width="100" HeaderText="claveSat" UniqueName="claveSat">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="unidad" ItemStyle-Width="100" HeaderText="unidad" UniqueName="unidad">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="marca" ItemStyle-Width="100" HeaderText="Marca" UniqueName="marca">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="marcaId" ItemStyle-Width="100" HeaderText="MarcaId" UniqueName="marcaId">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="coleccion" ItemStyle-Width="100" HeaderText="Coleccion" UniqueName="coleccionId">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="coleccionId" ItemStyle-Width="100" HeaderText="ColeccionID" UniqueName="coleccionId">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit1" ItemStyle-Width="100" HeaderText="precioUnit1" UniqueName="precioUnit1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit2" ItemStyle-Width="100" HeaderText="precioUnit2" UniqueName="precioUnit2">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit3" ItemStyle-Width="100" HeaderText="precioUnit3" UniqueName="precioUnit3">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit4" ItemStyle-Width="100" HeaderText="precioUnit4" UniqueName="precioUnit4">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit5" ItemStyle-Width="100" HeaderText="precioUnit5" UniqueName="precioUnit5">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit6" ItemStyle-Width="100" HeaderText="precioUnit6" UniqueName="precioUnit6">
                        </telerik:GridBoundColumn>
                        <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" Visible="false" UniqueName="Add">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAdd" runat="server" CommandArgument='<%# Eval("codigo") %>'
                                    CommandName="cmdAdd" ImageUrl="~/portalcfd/images/action_add.gif" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelErrores" runat="server" Visible="False">
        <fieldset style="width: 100%">
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblListadoProductos" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Carga CSV con Error"></asp:Label>
            </legend>
            <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="erroresList" runat="server" Width="100%" ShowStatusBar="True" ShowFooter="true"
                            AutoGenerateColumns="False" AllowPaging="False" PageSize="50" GridLines="None" ExportSettings-ExportOnlyData="True"
                            Skin="Simple">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <ExportSettings IgnorePaging="true" FileName="CargaMasivaProductosError">
                                <Excel Format="ExcelML" />
                            </ExportSettings>
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Productos" AllowMultiColumnSorting="False" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="false" ExportToExcelText="Exportar a Excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="upc" ItemStyle-Width="100" HeaderText="upc" UniqueName="upc">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="250" HeaderText="Descripción" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="claveSat" ItemStyle-Width="100" HeaderText="claveSat" UniqueName="claveSat">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" ItemStyle-Width="100" HeaderText="unidad" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="marca" ItemStyle-Width="100" HeaderText="Marca" UniqueName="marca">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="coleccion" ItemStyle-Width="100" HeaderText="Coleccion" UniqueName="coleccionId">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit1" ItemStyle-Width="100" HeaderText="precioUnit1" UniqueName="precioUnit1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit2" ItemStyle-Width="100" HeaderText="precioUnit2" UniqueName="precioUnit2">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit3" ItemStyle-Width="100" HeaderText="precioUnit3" UniqueName="precioUnit3">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit4" ItemStyle-Width="100" HeaderText="precioUnit4" UniqueName="precioUnit4">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit5" ItemStyle-Width="100" HeaderText="precioUnit5" UniqueName="precioUnit5">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit6" ItemStyle-Width="100" HeaderText="precioUnit6" UniqueName="precioUnit6">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="error" ItemStyle-Width="100" HeaderText="Error" UniqueName="error">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px;">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:HiddenField ID="orderParcialIdHidden" Value="0" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />


     <asp:Panel ID="panelFileUpCambioPrecio" runat="server">
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="Label7" runat="server" Font-Bold="true" CssClass="item" Text="Carga de Cambio de Precios por CSV."></asp:Label>
            </legend>
            <table width="100%" cellspacing="3" cellpadding="3" align="right" style="line-height: 25px;">
                <tr>
                    <td class="item" style="width: 27%;">
                        <strong>Seleccione archivo en formato CSV: </strong>
                    </td>
                    <td style="width: 35%;">
                        <asp:FileUpload ID="fileUpload1" runat="server" />
                    </td>
                    <td style="width: 10%;">
                        <asp:Button ID="btnCargarExcelCP" runat="server" Text="Cargar CSV" />
                    </td>
                    <td class="item">
                        <asp:ImageButton ID="imgDownloadCP" runat="server" ImageUrl="~/portalcfd/images/download.png" />
                        <span>Formato</span>
                    </td>
                    <%--<td class="item">
                        <asp:Button ID="btnAgregaConceptos" runat="server" CssClass="item" Visible="false" Text="Agregar Productos" />&nbsp;&nbsp;
                    </td>--%>
                    <td class="item">
                        <asp:Label ID="Label8" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                        <asp:Button ID="btnCambiarPrecios" runat="server" Enabled="false" Text="Cambiar Precios" BackColor="Green" ForeColor="White" />
                    </td>
                    <td colspan="5">
                        <asp:Label ID="lblMensajeCSVCP" runat="server" Font-Bold="true" CssClass="item" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:HiddenField ID="cargaidHiddenCP" Value="0" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelCSVCP" runat="server" Visible="false">
        <fieldset style="width: 100%">
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblConceptosCargaCP" runat="server" Font-Bold="true" CssClass="item" Text="Cambio de Precios Carga CSV Correctos"></asp:Label>
            </legend>
            <telerik:RadGrid ID="resultslistCSVCP" runat="server" Width="100%" ShowStatusBar="True"
                AutoGenerateColumns="False" AllowPaging="True" GridLines="None" Skin="Simple">
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView NoMasterRecordsText="No se encontraron registros." Width="100%" DataKeyNames="codigo,precioUnit1,precioUnit2,precioUnit3,precioUnit4" Name="Products" AllowMultiColumnSorting="False">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit1" ItemStyle-Width="100" HeaderText="precioUnit1" UniqueName="precioUnit1">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit2" ItemStyle-Width="100" HeaderText="precioUnit2" UniqueName="precioUnit2">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit3" ItemStyle-Width="100" HeaderText="precioUnit3" UniqueName="precioUnit3">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="precioUnit4" ItemStyle-Width="100" HeaderText="precioUnit4" UniqueName="precioUnit4">
                        </telerik:GridBoundColumn>
                       
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
        </fieldset>
     </asp:Panel>
    <br />
    <asp:Panel ID="panelErroresCP" runat="server" Visible="False">
        <fieldset style="width: 100%">
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblListadoProductosCP" runat="server" Font-Bold="true" CssClass="item" Text="Cambio de Precios Carga CSV con Error"></asp:Label>
            </legend>
            <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="erroresListCP" runat="server" Width="100%" ShowStatusBar="True" ShowFooter="true"
                            AutoGenerateColumns="False" AllowPaging="False" PageSize="50" GridLines="None" ExportSettings-ExportOnlyData="True"
                            Skin="Simple">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <ExportSettings IgnorePaging="true" FileName="CargaMasivaProductosError">
                                <Excel Format="ExcelML" />
                            </ExportSettings>
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Productos" AllowMultiColumnSorting="False" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="false" ExportToExcelText="Exportar a Excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit1" ItemStyle-Width="100" HeaderText="precioUnit1" UniqueName="precioUnit1">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit2" ItemStyle-Width="100" HeaderText="precioUnit2" UniqueName="precioUnit2">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit3" ItemStyle-Width="100" HeaderText="precioUnit3" UniqueName="precioUnit3">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precioUnit4" ItemStyle-Width="100" HeaderText="precioUnit4" UniqueName="precioUnit4">
                                    </telerik:GridBoundColumn>
                                   
                                    <telerik:GridBoundColumn DataField="error" ItemStyle-Width="100" HeaderText="Error" UniqueName="error">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td style="height: 15px;">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:HiddenField ID="HiddenField2" Value="0" runat="server" />
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
     <br />
    <br />
    <br />
    <telerik:RadWindowManager ID="rwAlerta" runat="server" Skin="Default" EnableShadow="false">
        <Localization OK="Aceptar" Cancel="Cancelar" />
    </telerik:RadWindowManager>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
    <telerik:RadWindowManager ID="rwAlertaCP" runat="server" Skin="Default" EnableShadow="false">
        <Localization OK="Aceptar" Cancel="Cancelar" />
    </telerik:RadWindowManager>
    <%--</telerik:RadAjaxPanel>--%>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>
