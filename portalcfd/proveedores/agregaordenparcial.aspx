<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="agregaordenparcial.aspx.vb" Inherits="erp_neogenis.agregaordenparcial" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Panel ID="panelOrdenCompraParcial" runat="server" Visible="true">
        <br />
        <fieldset>
            <telerik:RadTabStrip ID="RadTabStrip1" runat="server" Skin="Simple" MultiPageID="RadMultiPage1" SelectedIndex="0" CausesValidation="False">
                <Tabs>
                    <telerik:RadTab Text="Datos Generales Orden de Compra" TabIndex="0" Value="0" Enabled="True" Selected="true">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Conceptos para Orden de Compra" TabIndex="1" Value="1" Enabled="true">
                    </telerik:RadTab>
                    <telerik:RadTab Text="Orden de Compra Parcial" TabIndex="2" Value="2" Enabled="true">
                    </telerik:RadTab>
                </Tabs>
            </telerik:RadTabStrip>
            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Height="100%" Width="100%">
                <telerik:RadPageView ID="RadPageView1" runat="server" Width="100%" Selected="true">
                    <br />
                    <table width="100%" cellspacing="2" cellpadding="2" align="center" style="line-height: 25px;">
                        <tr>
                            <td class="item">
                                <strong>Clave: </strong>
                                <asp:Label ID="lblClaveEdit" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="item">
                                <strong>Proveedor: </strong>
                                <br />
                                <asp:DropDownList ID="proveedorid" runat="server" Width="300px" CssClass="item"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="item">
                                <strong>Marca: </strong>
                                <br />
                                <asp:DropDownList ID="proyectoid" runat="server" Width="150px" CssClass="item"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="item">
                                <strong>Fecha Estimado de Entega: </strong>
                                <br />
                                <telerik:RadDatePicker ID="txtfechaEstEnt" runat="server" CultureInfo="Español (México)" DateInput-DateFormat="dd/MM/yyyy">
                                    <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                    </Calendar>
                                    <DatePopupButton HoverImageUrl="" ImageUrl="" />
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td class="item">
                                <strong>Comentarios: </strong>
                                <br />
                                <telerik:RadTextBox ID="txtComentarios" runat="server" TextMode="MultiLine" Width="600px" Height="90px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="item">
                                <asp:Button ID="btnregresaList" runat="server" Text="Regresar al listado" CausesValidation="false" />&nbsp;&nbsp;
                                <asp:Button ID="btneditaDatosGen" runat="server" Text="Guardar Datos Generales" BackColor="Green" ForeColor="White" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblMensajeEdit" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <br />
                    <br />
                    <br />
                    <telerik:RadGrid ID="conceptosList" runat="server" Width="100%" ShowStatusBar="True"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="20" GridLines="None"
                        Skin="Simple" ShowFooter="true">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <MasterTableView Width="100%" DataKeyNames="id,ordenId,cantidad,codigo,descripcion,costo,moneda" Name="Products" AllowMultiColumnSorting="False">
                            <Columns>
                                <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ordenId" HeaderText="OrdenId" UniqueName="ordenid" Visible="false">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="ordenIdParcial" HeaderText="OrdenIdParcial" UniqueName="ordenidparcial" Visible="false">
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad OC" UniqueName="cantidadoc" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cantidad" HeaderText="Disponible" UniqueName="disponible" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Cant. O.C. Parcial" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtCantidad" runat="server" Value="0" MinValue="0" Width="80px" MaxValue='<%# Eval("cantidad") %>'></telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="costo" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="costo_variable" HeaderText="Costo Variable" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="total" HeaderText="Costo Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Del" Visible="false">
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
                    <br />
                    <%--<span style="color: Red;">* campos requeridos</span>--%>
                    <br />
                    <asp:Panel runat="server">
                        <div style="text-align: right;">
                            <asp:Button ID="btnCancelar" runat="server" Text="Regresar al listado" CausesValidation="false" />&nbsp;&nbsp;
                            <asp:Button ID="btnAddorder" runat="server" Text="Agregar conceptos a OC Parcial" BackColor="Green" ForeColor="White" />
                            <asp:Button ID="btnProcess" runat="server" Text="Procesar orden" BackColor="Green" ForeColor="White" Visible="false" />
                        </div>
                    </asp:Panel>
                    <br />
                    <br />
                    <asp:Label ID="lblMensaje" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
                    <br />
                    <br />
                </telerik:RadPageView>

                <telerik:RadPageView ID="RadPageView3" runat="server">
                    <table width="100%">
                        <tr>
                            <td style="height: 2px"></td>
                        </tr>
                        <tr>
                            <td style="height: 5px">
                                <telerik:RadGrid ID="ListOrdenesParciales" runat="server" AllowPaging="True"
                                    AutoGenerateColumns="False" GridLines="None"
                                    PageSize="15" ShowStatusBar="True"
                                    Skin="Simple" Width="100%">
                                    <PagerStyle Mode="NumericPages" />
                                    <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Orders" Width="100%">
                                        <Columns>
                                            <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="No. Orden"
                                                UniqueName="id">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdConceptosParciales" Text='<%# Eval("claveEdit") %>' CausesValidation="false"></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Left" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="id" HeaderText="ID" UniqueName="id">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_alta" HeaderText="Fecha" UniqueName="fecha">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="fecha_recepcion" HeaderText="Fecha" UniqueName="fecha">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" ItemStyle-HorizontalAlign="Center" UniqueName="estatus">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" />
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
                            <td style="height: 2px"></td>
                        </tr>
                        <tr>
                            <td align="right" style="height: 5px">
                                <asp:Button ID="Button1" Text="Agregar orden" runat="server" CausesValidation="False"
                                    CssClass="item" TabIndex="6" />
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 2px"></td>
                        </tr>
                    </table>
                    <br />
                    <br />
                    <br />
                    <telerik:RadGrid ID="ListConceptosParciales" runat="server" Width="100%" ShowStatusBar="True"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="20" GridLines="None"
                        Skin="Simple" ShowFooter="true">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
                            <Columns>
                                <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ordenId" HeaderText="OrdenId" UniqueName="ordenid" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="ordenIdParcial" HeaderText="OrdenIdParcial" UniqueName="ordenidparcial" Visible="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad OC" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cantidad" HeaderText="Disponible" UniqueName="disponible" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Cant. O.C. Parcial" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right">
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtCantidad" runat="server" Value="0" MinValue="0" Width="80px" MaxValue='<%# Eval("cantidad") %>'></telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="costo" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="costo_variable" HeaderText="Costo Variable" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="total" HeaderText="Costo Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Del" Visible="false">
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
                    <br />

                </telerik:RadPageView>

            </telerik:RadMultiPage>
            <br />
            <br />
            <br />
            <asp:Panel ID="panelErrores" runat="server" Visible="False">
                <legend style="padding-right: 6px; color: Black">
                    <asp:Label ID="lblListadoProductos" runat="server" Font-Bold="true" CssClass="item" Text="Listado de errores en carga masiva"></asp:Label>
                </legend>
                <br />
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
                                        <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="costo" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="costo_variable" HeaderText="Costo Variable" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="total" HeaderText="Costo Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="error" HeaderText="Error" UniqueName="error">
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
            </asp:Panel>
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
</asp:Content>
<%--http://localhost:8060/portalcfd/proveedores/agregaordenparcial.aspx?id=30252--%>