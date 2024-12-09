<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="agregarorden.aspx.vb" Inherits="erp_neogenis.agregarorden" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblEditorOrdenes" runat="server" Font-Bold="true" CssClass="item" Text="Agregar Orden de Compra"></asp:Label>
        </legend>
        <br />
        
        <table width="100%" cellspacing="2" cellpadding="2" align="center" style="line-height:25px;">
             <tr>
                <td class="item">
                    <strong>Clave: </strong>
                    <telerik:RadTextBox ID="txtclaveEdit" runat="server" Width="100px"></telerik:RadTextBox><br />
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Proveedor: </strong><br />
                    <asp:DropDownList ID="proveedorid" runat="server" CssClass="item"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Marca: </strong><br />
                    <asp:DropDownList ID="proyectoid" runat="server" CssClass="item"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Fecha Estimado de Entega: </strong><br />
                    <telerik:RadDatePicker ID="fechaEstEnt" runat="server">
                        <Calendar UseColumnHeadersAsSelectors="False" UseRowHeadersAsSelectors="False" ViewSelectorText="x">
                        </Calendar>
                        <DateInput runat="server" DateFormat = "dd/MM/yyyy" /> 
                        <DatePopupButton HoverImageUrl="" ImageUrl="" />
                    </telerik:RadDatePicker>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Comentarios: </strong><br />
                    <telerik:RadTextBox id="txtComentarios" runat="server" TextMode="MultiLine" Width="600px" Height="90px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button ID="btnAddorder" runat="server" CssClass="item" Text="Guardar" />&nbsp;&nbsp;<asp:Button ID="btnCancelar" runat="server" CssClass="item" Text="Cancelar" CausesValidation="false" />
                </td>
            </tr>
        </table>
        <br /><br />
    </fieldset>
</asp:Content>
