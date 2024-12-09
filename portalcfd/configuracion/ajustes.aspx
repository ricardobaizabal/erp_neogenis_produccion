<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="ajustes.aspx.vb" Inherits="erp_neogenis.ajustes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <fieldset class="item">
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblDataLegend" runat="server" CssClass="item" 
                Font-Bold="true" Text="Ajustes del Sistema"></asp:Label>
        </legend>

        <br />

        Tipo de cambio:<br /><br />
        <telerik:RadNumericTextBox ID="tipo_cambio" runat="server" Width="60px" NumberFormat-DecimalDigits="4" Type="Currency"></telerik:RadNumericTextBox> MXN<br /><br />

        <asp:Button ID="btnSave" runat="server" text="Guardar" CssClass="item" />
        <br />
        <asp:Label ID="lblMensaje" runat="server" ForeColor="Green" Font-Bold="true"></asp:Label>
        <br /><br />
    </fieldset>
    <br /><br /><br />
</asp:Content>
