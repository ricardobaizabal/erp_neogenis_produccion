<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="~/portalcfd/almacen/edita_embarcado.aspx.vb" Inherits="erp_neogenis.edita_embarcado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <fieldset style="padding: 10px;">
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="Label1" Text="Datos de factura" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
        </legend>
        <br />
        <table>
            <tr>
                <td class="item">
                    <strong>Cliente:</strong>
                </td>
                <td>
                    <asp:Label ID="lblCliente" Text="" runat="server" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>RFC:</strong>
                </td>
                <td>
                    <asp:Label ID="lblRFC" Text="" runat="server" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Serie:</strong>
                </td>
                <td>
                    <asp:Label ID="lblSerie" Text="" runat="server" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Folio:</strong>
                </td>
                <td>
                    <asp:Label ID="lblFolio" Text="" runat="server" CssClass="item"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </fieldset>
    <br />
    <fieldset style="padding: 10px;">
        <legend style="padding-right: 6px; color: Black">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/portalcfd/images/concept.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblConceptos" Text="Conceptos" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
        </legend>
        <br />
        <asp:HiddenField ID="partidaid" Value="0" runat="server" />
        <telerik:RadGrid ID="itemsList" runat="server" Width="100%" ShowStatusBar="True"
            AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
            Skin="Simple" Visible="True">
            <MasterTableView Width="100%" DataKeyNames="id" Name="Items" AllowMultiColumnSorting="False">
                <Columns>
                    <telerik:GridTemplateColumn>
                        <HeaderTemplate>Código</HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lblCodigo" runat="server" Text='<%# eval("codigo") %>' CommandName="cmdView" CommandArgument='<%# Eval("productoid") & "," & Eval("cantidad") & "," & Eval("id") & "," & Eval("pedidoid")%>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                        <ItemStyle VerticalAlign="Top" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unidad" UniqueName="unidad">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="precio" HeaderText="Precio Unit" UniqueName="precio" DataFormatString="{0:C}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="importe" HeaderText="Importe" UniqueName="importe" DataFormatString="{0:C}">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" UniqueName="estatus" ItemStyle-HorizontalAlign="Center">
                    </telerik:GridBoundColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <br />
    </fieldset>
    <telerik:RadWindow ID="RadWindow1" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" VisibleOnPageLoad="false" Behaviors="Close" Width="700" Height="600">
        <ContentTemplate>
            <br />
            <table align="center" width="90%" border="0">
                <tr>
                    <td style="height: 3px">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblCantidad" runat="server" Text="" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="height: 3px">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid ID="itemsInventoryList" runat="server" Width="100%" ShowStatusBar="True" AutoGenerateColumns="False" AllowPaging="False" GridLines="None" Skin="Simple">
                            <MasterTableView Width="100%" DataKeyNames="id,productoid,existencia" Name="Items" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Folio" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unidad" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="almacen" HeaderText="Almacén" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="lote" HeaderText="Lote" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="caducidad" HeaderText="Caducidad" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="existencia" HeaderText="Existencia" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Cantidad</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtCantidad" runat="server" MinValue="0" Value="0" Skin="Default" Width="80px" AutoPostBack="false">
                                                <NumberFormat DecimalDigits="4" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td style="height: 3px">&nbsp;</td>
                </tr>
                <%--<tr>
                    <td style="text-align: right">
                        <asp:Label ID="lblTotalDetalle" runat="server" Text="Total: 0" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>--%>
                <tr>
                    <td style="text-align: left">
                        <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <br />
                        <br />
                        <asp:Button ID="btnGuardarDetalle" runat="server" Text="Guardar" CausesValidation="False" CssClass="item" />
                        <asp:HiddenField ID="totalPiezasProcesadas" Value="0" runat="server" />
                        <asp:HiddenField ID="totalPiezasPartida" Value="0" runat="server" />
                        <asp:HiddenField ID="pedidoID" Value="0" runat="server" />
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </telerik:RadWindow>
</asp:Content>
