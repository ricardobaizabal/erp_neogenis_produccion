<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_almacen_kardex" Codebehind="kardex.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
               <asp:Label ID="lblEntradas" runat="server" Font-Bold="true" CssClass="item" Text="Kardex de Producto"></asp:Label>
            </legend>
            <br />
            <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
                <tr>
                    <td class="item">
                        Escriba el código o alguna palabra clave para encontrar el producto:<br /><br />
                        <asp:TextBox ID="txtSearch" width="250px" runat="server" CssClass="box"></asp:TextBox>&nbsp;<asp:button ID="btnSearch" runat="server" CssClass="boton" Text="buscar" CausesValidation="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <telerik:RadGrid ID="gridResults" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                            Skin="Simple" Visible="false">
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Items" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Código</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCodigo" runat="server" Text='<%# eval("codigo") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle" />
                                    </telerik:GridTemplateColumn>
                                    
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Descripción</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescripcion" runat="server" Text='<%# eval("descripcion") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle" />
                                    </telerik:GridTemplateColumn>
                                    
                                    
                                    
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"
                                        UniqueName="Add">
                                        <ItemTemplate>
                                            <asp:LinkButton id="lnkview" runat="server" CommandArgument ='<%# Eval("id") %>' CommandName="cmdView" Text="Ver kardex"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Middle" />
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
            <br />
        </fieldset>
        <br />
        
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblProductsListLegend" Text="Kardex del producto seleccionado" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid ID="productslist" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                            Skin="Simple">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" UniqueName="fecha">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="documento" HeaderText="Documento" UniqueName="documento">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="movimiento" HeaderText="Movimiento" UniqueName="movimiento">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unidad" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad" UniqueName="cantidad" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="existencia" HeaderText="Existencia" UniqueName="existencia" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="almacen" HeaderText="Almacén" UniqueName="almacen" ItemStyle-HorizontalAlign="Right">
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

