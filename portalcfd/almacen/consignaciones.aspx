<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="consignaciones.aspx.vb" Inherits="erp_neogenis.consignaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />

    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Image ID="Image2" runat="server" ImageUrl="~/images/icons/ListadoProductos_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblProductsListLegend" runat="server" Font-Bold="true" CssClass="item" Text="Lotes de Consignación"></asp:Label>
        </legend>
        <br />
        <table width="100%">
            <tr>
                <td style="height: 5px">
                    [<asp:HyperLink CssClass="item" ID="lnkAddConsignacion" runat="server" Text="Crear nuevo lote de consignación" NavigateUrl="~/portalcfd/almacen/agregarConsignacion.aspx"></asp:HyperLink>]
                </td>
            </tr>
            <td style="height: 5px">&nbsp;</td>
            <tr>
                <td class="item">
                    <strong>Estatus:</strong>&nbsp;&nbsp;<asp:DropDownList id="estatusid" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;   
                    <strong>Marca:</strong>&nbsp;&nbsp;<asp:DropDownList id="proyectoid" runat="server"></asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;   
                    <asp:Button ID="btnSearch" runat="server" Text="Consultar" /><br />
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                    <telerik:RadGrid ID="consignacionList" runat="server" Width="100%" ShowStatusBar="True" AllowSorting="True"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="50" GridLines="None"
                        Skin="Simple">
                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                        <MasterTableView Width="100%" DataKeyNames="id" Name="Lotes" AllowMultiColumnSorting="False" NoMasterRecordsText="No se encontraron registros para mostrar.">
                            <Columns>
                                <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="No. Lote" UniqueName="id">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text='<%# Eval("id") %>' CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" UniqueName="fecha">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="almacen" HeaderText="Almacén Origen" UniqueName="origen">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" UniqueName="destino">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="piezas" ItemStyle-Width="50px" HeaderText="Piezas" UniqueName="piezas">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="existencia" ItemStyle-Width="50px" HeaderText="Existencia" UniqueName="existencia">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" UniqueName="estatus">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPDF" runat="server" Text="pdf" CommandArgument='<%# Eval("id") %>' CommandName="cmdPDF"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>--%>
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
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td style="height: 2px"></td>
            </tr>
        </table>
    </fieldset>
    <br />
</asp:Content>
