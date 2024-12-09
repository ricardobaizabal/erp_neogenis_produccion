<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="agregarlote.aspx.vb" Inherits="erp_neogenis.agregarlote" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
           <asp:Label ID="lblEntradas" runat="server" Font-Bold="true" CssClass="item" Text="Agregando nuevo lote de transferencia"></asp:Label>
        </legend>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" align="center" width="100%">
            <tr>
                <td class="item">
                    Origen: <asp:DropDownList id="origenid" runat="server"></asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="valOrigen" runat="server" ErrorMessage="* requerido" ForeColor="Red" ControlToValidate="origenid" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator><br /><br />
                    Destino: <asp:DropDownList ID="destinoid" runat="server"></asp:DropDownList>&nbsp;<asp:RequiredFieldValidator ID="valDestino" runat="server" ErrorMessage="* requerido" ForeColor="Red" ControlToValidate="destinoid" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator><br /><br />
                    Comentario:<br />
                    <asp:TextBox ID="txtComentario" runat="server" Width="600px" Height="80px" 
                        TextMode="MultiLine"></asp:TextBox><br /><br />
                    <asp:Button ID="btnAdd" runat="server" Text="Guardar" /><br />
                </td>
            </tr>
            <tr><td><br /></td></tr>
        </table>
    </fieldset>
</asp:Content>
