<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_almacen_ajustes" CodeBehind="ajustes.aspx.vb" %>

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
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        <asp:Panel ID="PanelAgregarAjuste" runat="server" Visible="false">
            <fieldset>
                        <legend style="padding-right: 6px; color: Black">
                            <asp:Label ID="lblEntradas" runat="server" Font-Bold="true" CssClass="item" Text="Ajustes de Almacen"></asp:Label>
                        </legend>
                        <br />
                        <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                            <tr>
                                <td class="item">Escriba el código o alguna palabra clave para encontrar el producto:<br />
                                    <br />
                                    <asp:Panel ID="panelBusqueda" DefaultButton="btnSearch" runat="server">
                                        <asp:TextBox ID="txtSearch" Width="250px" runat="server" CssClass="box"></asp:TextBox>&nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="boton" Text="buscar" CausesValidation="false" />
                                    </asp:Panel>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="item">
                                    <asp:Label ID="lblMensaje" runat="server" class="item" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:CheckBox ID="chkAll" runat="server" Text="Seleccionar todo" Visible="False" CssClass="item" Font-Bold="true" /><br /><br />
                                    <telerik:RadGrid ID="gridResults" runat="server" Width="100%" ShowStatusBar="True"
                                        AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                                        Skin="Simple" Visible="False">
                                        <MasterTableView Width="100%" DataKeyNames="id" Name="Items" AllowMultiColumnSorting="False">
                                            <Columns>

                                                <telerik:GridTemplateColumn UniqueName="chkcfdid">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkItem" runat="server" CssClass="item" />
                                                        <asp:Label ID="lblProductoId" runat="server" Text='<%# Eval("id")%>'></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn>
                                                    <HeaderTemplate>Código</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCodigo" runat="server" Text='<%#Eval("codigo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn>
                                                    <HeaderTemplate>Descripción</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDescripcion" runat="server" Text='<%#Eval("descripcion") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn>
                                                    <HeaderTemplate>Unidad</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblUnidad" runat="server" Text='<%#Eval("unidad") %>'></asp:Label>
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

                                                <telerik:GridTemplateColumn>
                                                    <HeaderTemplate>Almacén</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:DropDownList ID="almacenid" AutoPostBack="true" runat="server" OnSelectedIndexChanged="almacenid_SelectedIndexChanged" CssClass="item"></asp:DropDownList>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn>
                                                    <HeaderTemplate>Disponibles</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDisponibles" runat="server" Text='<%# Eval("disponibles")%>'></asp:Label>
                                                        <asp:Label ID="lblMonterrey" runat="server" Text='<%# Eval("monterrey")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblMexico" runat="server" Text='<%# Eval("mexico")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblGuadalajara" runat="server" Text='<%# Eval("guadalajara")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblMermas" runat="server" Text='<%# Eval("mermas")%>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblMatriz" runat="server" Text='<%# Eval("matriz")%>' Visible="false"></asp:Label>
                                                    </ItemTemplate>
                                                    <ItemStyle VerticalAlign="Middle" />
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn>
                                                    <HeaderTemplate>Motivo del ajuste</HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtComentario" TextMode="MultiLine" runat="server" CssClass="box" Width="300px" Height="50px"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Add">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnAdd" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdAdd" ImageUrl="~/portalcfd/images/action_minus.png" CausesValidation="False" ToolTip="Agregar ajuste de inventario para este producto" />
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
                                <td><br />
                                    <asp:Button ID="btnAjustar" CausesValidation="false" Visible="false" runat="server" Text="Aplicar Ajuste" />
                                </td>
                            </tr>
                        </table>
                        <br />
                    </fieldset>
        </asp:Panel>
        
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
                    <td>
                        <telerik:RadGrid ID="productslist" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="50" GridLines="None"
                            Skin="Simple">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" UniqueName="fecha">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <%--<telerik:GridBoundColumn DataField="costo_unitario" HeaderText="Costo unitario" UniqueName="costo_unitario" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="costo_unitario_var" HeaderText="Costo Un. Var." UniqueName="costo_unitario_var" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:c}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="lote" HeaderText="Lote" UniqueName="lote">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="caducidad" HeaderText="Fec. Caducidad" UniqueName="caducidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="documento" HeaderText="Documento" UniqueName="documento">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridBoundColumn DataField="almacen" HeaderText="Almacén" UniqueName="almacen">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="comentario" HeaderText="Comentarios" UniqueName="comentarios">
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
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

