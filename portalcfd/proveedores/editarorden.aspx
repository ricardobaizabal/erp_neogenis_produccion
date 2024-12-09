<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="editarorden.aspx.vb" Inherits="erp_neogenis.editarorden" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <asp:Panel ID="panelOrdenCompraParcial" runat="server" Visible="true">
        <fieldset style="width: 100%">
            <%--<legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblEditorOrdenes" runat="server" Font-Bold="true" CssClass="item" Text="Editor de Orden de Compra"></asp:Label>
        </legend>--%>
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
            <br />
            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" SelectedIndex="0" Height="100%" Width="100%">
                <telerik:RadPageView ID="RadPageView1" runat="server" Width="100%" Selected="true">
                    <fieldset>

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
                                    <telerik:RadDatePicker ID="txtfechaEstEnt" runat="server">
                                        <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                                        </Calendar>
                                        <DateInput runat="server" DateFormat="dd/MM/yyyy" />
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
                    </fieldset>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView2" runat="server">
                    <fieldset>
                        <br />
                        <table width="100%" cellspacing="3" cellpadding="3" align="center" style="line-height: 25px;">
                            <tr>
                                <td class="item">Producto: &nbsp;<asp:TextBox ID="txtSearch" runat="server" CssClass="item"></asp:TextBox>&nbsp;<asp:Button ID="btnSearch" runat="server" Text="Buscar" CausesValidation="false" CssClass="item" />&nbsp;&nbsp;<asp:Button ID="btnCancel" runat="server" CausesValidation="false" Text="Cancelar búsqueda" CssClass="item" /><br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                        <fieldset style="width: 98%">
                            <legend style="padding-right: 6px; color: Black">
                                <asp:Label ID="Label3" runat="server" Font-Bold="true" CssClass="item" Text="Carga de Conceptos Orden Principal por CSV."></asp:Label>
                            </legend>
                            <table width="100%" cellspacing="3" cellpadding="3" align="right" style="line-height: 25px;">
                                <tr>
                                    <td class="item" style="width: 27%;">
                                        <strong>Seleccione archivo en formato CSV: </strong>
                                    </td>
                                    <td style="width: 35%;">
                                        <asp:FileUpload ID="fileUploadExcel" runat="server" />
                                    </td>
                                    <td style="width: 10%;">
                                        <asp:Button ID="btnCargarExcel" runat="server" Text="Cargar CSV" />
                                    </td>
                                    <td class="item">
                                        <asp:ImageButton ID="imgDownload" runat="server" ImageUrl="~/portalcfd/images/download.png" />
                                        <span>Formato</span>
                                    </td>
                                    <td class="item">
                                        <asp:Button ID="btnAgregaConceptos" runat="server" CssClass="item" Visible="false" Text="Agregar Conceptos" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <asp:HiddenField ID="cargaidHidden" Value="0" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <br />
                        <asp:Panel ID="panelSearch" runat="server" Visible="false">
                            <fieldset style="width: 100%">
                                <legend style="padding-right: 6px; color: Black">
                                    <asp:Label ID="lblConceptos" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Busqueda"></asp:Label>
                                </legend>
                                <telerik:RadGrid ID="resultslist" runat="server" Width="100%" ShowStatusBar="True"
                                    AutoGenerateColumns="False" AllowPaging="True" PageSize="450" GridLines="None"
                                    Skin="Simple">
                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                    <MasterTableView NoMasterRecordsText="No se encontraron registros." Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="250" HeaderText="Descripción" UniqueName="descripcion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="150" HeaderText="Cantidad" UniqueName="cantidad">
                                                <ItemTemplate>
                                                    <telerik:RadNumericTextBox ID="txtCantidad" Value="0" MinValue="0" runat="server" Width="80px"></telerik:RadNumericTextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="costo" ItemStyle-Width="100" HeaderText="Costo" UniqueName="unitario" DataFormatString="{0:c}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="moneda" ItemStyle-Width="100" HeaderText="Moneda" UniqueName="moneda">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" Visible="false" UniqueName="Add">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnAdd" runat="server" CommandArgument='<%# Eval("id") %>'
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
                        <asp:Panel ID="panelCSV" runat="server" Visible="false">
                            <fieldset style="width: 100%">
                                <legend style="padding-right: 6px; color: Black">
                                    <asp:Label ID="lblConceptosCarga" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Carga CSV Correctos"></asp:Label>
                                </legend>
                                <telerik:RadGrid ID="resultslistCSV" runat="server" Width="100%" ShowStatusBar="True"
                                    AutoGenerateColumns="False" AllowPaging="True" GridLines="None" Skin="Simple">
                                    <PagerStyle Mode="NumericPages"></PagerStyle>
                                    <MasterTableView NoMasterRecordsText="No se encontraron registros." Width="100%" DataKeyNames="productoid" Name="Products" AllowMultiColumnSorting="False">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="250" HeaderText="Descripción" UniqueName="descripcion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="150" HeaderText="Cantidad" UniqueName="cantidad">
                                                <ItemTemplate>
                                                    <telerik:RadNumericTextBox ID="txtCantidad" runat="server" Width="80px"></telerik:RadNumericTextBox>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" />
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridBoundColumn DataField="costo" ItemStyle-Width="100" HeaderText="Costo" UniqueName="unitario" DataFormatString="{0:c}">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="moneda" ItemStyle-Width="100" HeaderText="Moneda" UniqueName="moneda">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" Visible="false" UniqueName="Add">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnAdd" runat="server" CommandArgument='<%# Eval("productoid") %>'
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
                                                        <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo" ItemStyle-Width="100">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="200">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad" ItemStyle-Width="150">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="costo" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="costo_variable" HeaderText="Costo Variable" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="total" HeaderText="Costo Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda" ItemStyle-Width="100">
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
                            </fieldset>
                        </asp:Panel>
                        <br />
                        <fieldset style="width: 98%">
                            <legend style="padding-right: 6px; color: Black">
                                <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Cargados Orden Compra"></asp:Label>
                            </legend>
                            <telerik:RadGrid ID="conceptosList" runat="server" Width="100%" ShowStatusBar="True" AutoGenerateColumns="False" AllowPaging="false" GridLines="None" ExportSettings-ExportOnlyData="True" ShowHeader="true" Skin="Simple" ShowFooter="true">
                                <ExportSettings IgnorePaging="true" FileName="ConceptosCaragados">
                                    <Excel Format="ExcelML" />
                                </ExportSettings>
                                <MasterTableView NoMasterRecordsText="No se encontraron registros." Width="100%" DataKeyNames="id,ordenId,cantidad,disponible,codigo,descripcion,costo,moneda" Name="Products" CommandItemDisplay="Top" AllowMultiColumnSorting="False">
                                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="false" ExportToExcelText="Exportar a Excel"></CommandItemSettings>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ordenId" HeaderText="OrdenId" UniqueName="ordenid" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <%--<telerik:GridBoundColumn DataField="ordenIdParcial" HeaderText="OrdenIdParcial" UniqueName="ordenidparcial" Visible="false">
                                    </telerik:GridBoundColumn>--%>
                                        <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="250" HeaderText="Descripción" UniqueName="descripcion">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidad" ItemStyle-Width="80" HeaderText="Cantidad OC" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="disponible" ItemStyle-Width="80" HeaderText="Disponible" UniqueName="disponible" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Cant. O.C. Parcial" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox ID="txtCantidad" runat="server"
                                                    MinValue="0"
                                                    Width="80px"
                                                    MaxValue='<%# Eval("disponible") %>'
                                                    Value='<%# Eval("disponible") %>'>
                                                </telerik:RadNumericTextBox>
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
                        </fieldset>
                        <br />
                        <fieldset style="width: 98%">
                            <legend style="padding-right: 6px; color: Black">
                                <asp:Label ID="lblordenparcialcsv" runat="server" Font-Bold="true" CssClass="item" Text="Carga de Conceptos Orden Parcial por CSV."></asp:Label>
                            </legend>
                            <div>
                                <table width="100%" cellspacing="3" cellpadding="3" align="right" style="line-height: 25px;">
                                    <tr>
                                        <td class="item" style="width: 27%;">
                                            <strong>Seleccione archivo en formato CSV: </strong>
                                        </td>
                                        <td style="width: 35%;">
                                            <asp:FileUpload ID="fileUploadOCP" runat="server" />
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Button ID="btnOCPcsv" runat="server" Text="Cargar CSV" />
                                        </td>
                                        <td class="item">
                                            <asp:ImageButton ID="imgdownloadF" runat="server" ImageUrl="~/portalcfd/images/download.png" />
                                            <span>Formato</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <asp:HiddenField ID="cargaidOCPHidden" Value="0" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </fieldset>
                        <br />
                        <asp:Panel ID="panelCSVOCP" runat="server" Visible="false">
                            <fieldset style="width: 98%">
                                <legend style="padding-right: 6px; color: Black">
                                    <asp:Label ID="lblConceptosCargaOCP" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Carga CSV Correctos OCP"></asp:Label>
                                </legend>
                                <telerik:RadGrid ID="resultslistCSVOCP" runat="server" Width="100%" ShowStatusBar="True"
                                    AutoGenerateColumns="False" AllowPaging="false" GridLines="None" Skin="Simple" ShowFooter="true">
                                    <MasterTableView NoMasterRecordsText="No se encontraron registros." Width="100%" DataKeyNames="id,cantidad,codigo,descripcion,costo,moneda,productoid" Name="Products" AllowMultiColumnSorting="False">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="codigo" ItemStyle-Width="100" HeaderText="Código" UniqueName="codigo">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="descripcion" ItemStyle-Width="250" HeaderText="Descripción" UniqueName="descripcion">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="disponible" ItemStyle-Width="80" HeaderText="Disponible" UniqueName="disponible" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="cantidad" ItemStyle-Width="80" HeaderText="Cant. O.C. Parcual" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="costo" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="total" HeaderText="Costo Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn DataField="productoid" ItemStyle-Width="250" HeaderText="Producto Id" UniqueName="productoid" Visible="false">
                                            </telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </fieldset>
                        </asp:Panel>
                        <br />
                        <asp:Panel ID="panelErroresOCP" runat="server" Visible="False">
                            <fieldset style="width: 98%">
                                <legend style="padding-right: 6px; color: Black">
                                    <asp:Label ID="Label2" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos Carga CSV con Error"></asp:Label>
                                </legend>
                                <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadGrid ID="erroresListOCP" runat="server" Width="100%" ShowStatusBar="True" ShowFooter="true"
                                                AutoGenerateColumns="False" AllowPaging="False" PageSize="50" GridLines="None" ExportSettings-ExportOnlyData="True"
                                                Skin="Simple">
                                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                                <ExportSettings IgnorePaging="true" FileName="CargaMasivaProductosError">
                                                    <Excel Format="ExcelML" />
                                                </ExportSettings>
                                                <MasterTableView Width="100%" DataKeyNames="id" Name="Productos" AllowMultiColumnSorting="False" NoMasterRecordsText="No se encontraron registros." CommandItemDisplay="Top">
                                                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="false" ExportToExcelText="Exportar a Excel"></CommandItemSettings>
                                                    <Columns>
                                                        <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo" ItemStyle-Width="100">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="200">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad" ItemStyle-Width="150">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="costo" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="costo_variable" HeaderText="Costo Variable" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="total" HeaderText="Costo Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" Visible="false">
                                                        </telerik:GridBoundColumn>
                                                        <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda" ItemStyle-Width="100">
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
                                            <asp:HiddenField ID="HiddenField2" Value="0" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </asp:Panel>
                        <br />
                        <%--<span style="color: Red;">* campos requeridos</span>--%>
                        <br />
                        <br />
                        <asp:Panel runat="server">
                            <div style="text-align: left;">
                                <asp:Button ID="btnCancelar" runat="server" Text="Regresar al listado" CausesValidation="false" />
                            </div>
                            <div style="text-align: right;">
                                <asp:Label ID="lblMensaje" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>

                                <asp:Button ID="btnAddorderParcial" runat="server" Text="Agregar conceptos a OC Parcial" BackColor="Green" ForeColor="White" />
                            </div>
                        </asp:Panel>
                        <br />
                        <br />

                        <br />
                        <br />
                    </fieldset>
                </telerik:RadPageView>
                <telerik:RadPageView ID="RadPageView3" runat="server">
                    <fieldset>
                        <br />
                        <table width="100%">
                            <tr>
                                <td style="height: 2px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="ListOrdenesParciales" runat="server" AllowPaging="True"
                                        AutoGenerateColumns="False" GridLines="None" SelectedItemStyle-HorizontalAlign="Center"
                                        PageSize="15" ShowStatusBar="True"
                                        Skin="Simple" Width="650px">
                                        <PagerStyle Mode="NumericPages" />
                                        <MasterTableView NoMasterRecordsText="No se encontraron registros." AllowMultiColumnSorting="False" DataKeyNames="id" Name="Orders" Style="width: 650px">
                                            <Columns>
                                                <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="No. Orden" ItemStyle-Width="150" UniqueName="id">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") + "," + Eval("claveEdit")%>' CommandName="cmdConceptosParciales" Text='<%# Eval("claveEdit") %>' CausesValidation="false"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="id" HeaderText="ID" ItemStyle-HorizontalAlign="Center" UniqueName="id" Visible="false" HeaderStyle-HorizontalAlign="Center">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="fecha_alta" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" HeaderText="Fecha" HeaderStyle-HorizontalAlign="Center" UniqueName="fecha">
                                                </telerik:GridBoundColumn>
                                                <%--<telerik:GridBoundColumn DataField="fecha_est_recepcion" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" HeaderText="Fecha Est. Recepción" UniqueName="fecha_est_recepcion" HeaderStyle-HorizontalAlign="Center">
                                            </telerik:GridBoundColumn>--%>
                                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Fecha Est. Recepción" ItemStyle-Width="200" UniqueName="txtfechaEstEntOCP" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <telerik:RadDatePicker ID="txtfechaEstEntOCP" runat="server" CultureInfo="Español (México)" SkipMinMaxDateValidationOnServer="true" DateInput-DateFormat="dd/MM/yyyy"></telerik:RadDatePicker>
                                                        <%--<asp:ImageButton ID="btnAddFecha" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdAddFecha" ImageUrl="~/images/action_add.gif" />--%>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="fecha_recepcion" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" HeaderText="Fecha Recepción" UniqueName="fecha_recepcion" HeaderStyle-HorizontalAlign="Center">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" ItemStyle-Width="100" ItemStyle-HorizontalAlign="Center" UniqueName="estatus" HeaderStyle-HorizontalAlign="Center">
                                                </telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn AllowFiltering="False" ItemStyle-Width="50" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDeleteOCP" ImageUrl="~/images/action_delete.gif" />
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
                                <td style="height: 2px">&nbsp;</td>
                            </tr>
                            <tr>
                                <td style="height: 2px">&nbsp;</td>
                            </tr>
                        </table>
                        <br />
                        <fieldset style="width: 98%">
                            <legend style="padding-right: 6px; color: Black">
                                <asp:Label ID="lblConceptosParciales" runat="server" Font-Bold="true" CssClass="item" Text="Lista conceptos parciales"></asp:Label>
                                <asp:Label ID="lblNombreOCP" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                            </legend>
                            <telerik:RadGrid ID="ListConceptosParciales" runat="server" ShowStatusBar="True"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="20" GridLines="None"
                                Skin="Simple" ShowFooter="true" ExportSettings-ExportOnlyData="True" Width="650px">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <ExportSettings IgnorePaging="true" FileName="Lista Conceptos Parciales">
                                    <Excel Format="ExcelML" />
                                </ExportSettings>
                                <MasterTableView NoMasterRecordsText="No se encontraron registros." Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False" CommandItemDisplay="Top">
                                    <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" ShowExportToExcelButton="true" ShowExportToPdfButton="false" ExportToExcelText="Exportar a Excel"></CommandItemSettings>
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ordenId" HeaderText="OrdenId" UniqueName="ordenid" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="ordenIdParcial" HeaderText="OrdenIdParcial" UniqueName="ordenidparcial" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidadoc" HeaderText="Cantidad Solicitada" UniqueName="cantidadoc" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad Solicitada" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="disponible" HeaderText="Cantidad Recibida" UniqueName="disponible" ItemStyle-HorizontalAlign="Right" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterText=" " FooterStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="costo" Visible="false" HeaderText="Costo" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="costo_variable" Visible="false" HeaderText="Costo Variable" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="total" HeaderText="Costo Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" Visible="false">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="moneda" HeaderText="Moneda" UniqueName="moneda" Visible="false">
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
                            <div style="text-align: left;">
                                <asp:Button ID="btnConfAlmacenado" runat="server" Text="Confirmar Consolidado" BackColor="Green" ForeColor="White" Visible="false" />
                            </div>
                        </fieldset>
                        <br />
                    </fieldset>
                </telerik:RadPageView>
            </telerik:RadMultiPage>
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
