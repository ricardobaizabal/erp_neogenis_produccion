<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="caducidades.aspx.vb" Inherits="erp_neogenis.caducidades" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <telerik:RadGrid ID="conceptosList" runat="server" Width="100%" ShowStatusBar="True"
        AutoGenerateColumns="False" AllowPaging="True" PageSize="15" GridLines="None"
        Skin="Simple" ShowFooter="false">
        <PagerStyle Mode="NumericPages"></PagerStyle>
        <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
            <Columns>
                <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo" ItemStyle-HorizontalAlign="left">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-HorizontalAlign="left">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad Recibida" UniqueName="cantidad" ItemStyle-HorizontalAlign="Center">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="lote" HeaderText="Lote" UniqueName="cantidad">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="fecha_caducidad" HeaderText="Caducidad" UniqueName="caducidad">
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="existencia" HeaderText="Existencia" UniqueName="existencia" ItemStyle-HorizontalAlign="Right">
                </telerik:GridBoundColumn>
            </Columns>
        </MasterTableView>
    </telerik:RadGrid>
</asp:Content>

