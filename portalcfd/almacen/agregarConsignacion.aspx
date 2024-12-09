<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="agregarConsignacion.aspx.vb" Inherits="erp_neogenis.agregarConsignacion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
           <asp:Label ID="lblEntradas" runat="server" Font-Bold="true" CssClass="item" Text="Agregando nuevo lote de consignación"></asp:Label>
        </legend>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
            <tr>
                <td class="item">
                    Almacén Origen: <asp:DropDownList id="almacenid" runat="server"></asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="valOrigen" runat="server" ErrorMessage="* requerido" ForeColor="Red" ControlToValidate="almacenid" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator><br /><br />
                    Cliente: <asp:DropDownList ID="clienteid" runat="server" AutoPostBack="true"></asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="valDestino" runat="server" ErrorMessage="* requerido" ForeColor="Red" ControlToValidate="clienteid" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator><br /><br />
                    Sucursal: <asp:DropDownList ID="sucursalid" runat="server"></asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="valSucursal" runat="server" ErrorMessage="* requerido" ForeColor="Red" ControlToValidate="sucursalid" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator><br /><br />
                    Orden de compra: <telerik:RadTextBox ID="txtOrdenCompra" Width="100px" Runat="server" CssClass="item"></telerik:RadTextBox><br /><br />
                    Comentario:<br /><br />
                    <asp:TextBox ID="txtComentario" runat="server" Width="600px" Height="80px" TextMode="MultiLine"></asp:TextBox><br /><br />
                    <asp:Button ID="btnAdd" runat="server" Text="Guardar" /><br />
                </td>
            </tr>
            <tr><td><br /></td></tr>
        </table>
    </fieldset>
</asp:Content>
