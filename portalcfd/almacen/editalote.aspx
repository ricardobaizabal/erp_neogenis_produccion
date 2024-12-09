<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="editalote.aspx.vb" Inherits="erp_neogenis.editalote" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblTransferencia" runat="server" Font-Bold="true" CssClass="item" Text="Editando lote de transferencia"></asp:Label>
        </legend>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
            <tr>
                <td class="item" style="line-height: 20px;">
                    <strong>Folio de Transferencia:</strong>
                    <asp:Label ID="lblFolio" runat="server"></asp:Label><br />
                    <strong>Fecha:</strong>
                    <asp:Label ID="lblFecha" runat="server"></asp:Label><br />
                    <strong>Origen:</strong>
                    <asp:Label ID="lblOrigen" runat="server"></asp:Label><br />
                    <strong>Destino:</strong>
                    <asp:Label ID="lblDestino" runat="server"></asp:Label><br />
                    <strong>Usuario:</strong>
                    <asp:Label ID="lblUsuario" runat="server"></asp:Label><br />
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
        <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
            <tr>
                <td class="item">
                    <table border="0" cellpadding="0" cellspacing="0" align="center" width="100%">
                        <tr>
                            <td style="width:5%;">
                                <strong>Buscar:</strong>
                            </td>
                            <td style="width:15%;">
                                <asp:TextBox ID="txtSearchItem" runat="server" CssClass="box" AutoPostBack="false"></asp:TextBox>&nbsp;
                            </td>
                            <td style="width:6%;">
                                <asp:Button ID="btnSearch" CausesValidation="false" runat="server" Text="Buscar" />
                            </td>
                            <td>&nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--<tr style="height:30px;">
                <td>&nbsp;</td>
            </tr>--%>
            <tr>
                <td>
                    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr style="height:30px;">
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="gridResults" runat="server" Width="100%" ShowStatusBar="True" AutoGenerateColumns="False" AllowPaging="False" GridLines="None" Skin="Simple">
                        <MasterTableView Width="100%" DataKeyNames="productoid, codigo" Name="Items" AllowMultiColumnSorting="False">
                            <Columns>
                                <%--<telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductoId" runat="server" Text='<%# Eval("productoid")%>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>--%>

                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>Código</HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCodigo" runat="server" Text='<%# eval("codigo") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn>
                                    <HeaderTemplate>Cant.</HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadNumericTextBox ID="txtCantidad" runat="server" Skin="Default" Width="50px"
                                            MinValue="0" Value='0'>
                                            <NumberFormat DecimalDigits="4" GroupSeparator="" />
                                        </telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>

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

                                <%--<telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Add">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnAdd" runat="server" CommandArgument='<%# Eval("productoid")%>' CommandName="cmdAdd" ImageUrl="~/portalcfd/images/action_add.gif" CausesValidation="False" ToolTip="Agregar producto como partida" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>--%>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr style="height:20px;">
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <div>
                        <table style="width:100%;">
                            <tr>
                                <td style="width:70%;" align="left">
                                    <asp:Label ID="Label2" runat="server" ForeColor="Red" CssClass="item"></asp:Label>
                                </td>
                                <td style="width:30%;" align="right">
                                    <asp:Button ID="btnAgregar" runat="server" CausesValidation="false" Text="Agregar" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
    <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="item" Text="Lista de productos para Transferencia"></asp:Label>
        </legend>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
            <tr>
                <td>
                    <telerik:RadGrid ID="productslist" runat="server" Width="100%" ShowStatusBar="True"
                        AutoGenerateColumns="False" AllowPaging="False" PageSize="50" GridLines="None"
                        Skin="Simple">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
                            <Columns>
                                <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="lote" HeaderText="Lote" UniqueName="lote">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="caducidad" HeaderText="Caducidad" UniqueName="caducidad">
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" CausesValidation="false" />
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
    </fieldset>
    <br />
    <br />
    <div align="right">
        <asp:Button ID="btnProcesar" runat="server" CausesValidation="false" Text="Procesar transferencia" />&nbsp;<asp:Button ID="btnRegresar" runat="server" Text="Regresar" CausesValidation="false" />
    </div>
    <br />
    <br />
    <br />
    <br />
    <br />
    <br />
</asp:Content>