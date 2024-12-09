<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_Home" Codebehind="Home.aspx.vb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        #panel 
        {
            width:800px;
        }

        #panel td
        {
            text-align: center;
            line-height:28px;
            font-family: Verdana;
            font-size: 14px;
            color: #333333;
            font-weight: bold;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <fieldset>
        <legend class="item"><strong>Inicio</strong></legend>
        <br />
        <table border="0" cellpadding="2" cellspacing="2" align="center" style="width:980px;" id="panel">
            <tr>
                <td align="center"><asp:ImageButton ID="lnk1" runat="server" PostBackUrl="~/portalcfd/administracion/clientes.aspx" ImageUrl="~/portalcfd/images/icons/clientes1.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk7" runat="server" PostBackUrl="~/portalcfd/proveedores/proveedores.aspx" ImageUrl="~/portalcfd/images/icons/proveedor1.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk4" runat="server" PostBackUrl="~/portalcfd/almacen/productos.aspx" ImageUrl="~/portalcfd/images/icons/inventario1.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk3" runat="server" PostBackUrl="~/portalcfd/CFD.aspx" ImageUrl="~/portalcfd/images/icons/facturacion1.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk9" runat="server" PostBackUrl="~/portalcfd/pedidos/pedidos.aspx" ImageUrl="~/portalcfd/images/icons/pedidos1.jpg" /></td>
            </tr>
            <tr>
                <td colspan="5"><br /></td>
            </tr>
            <tr>
                <td align="center"><asp:ImageButton ID="lnk2" runat="server" PostBackUrl="~/portalcfd/reportes.aspx" ImageUrl="~/portalcfd/images/icons/reportes1.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk8" runat="server" PostBackUrl="~/portalcfd/configuracion.aspx" ImageUrl="~/portalcfd/images/icons/configuracion.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk5" runat="server" PostBackUrl="~/portalcfd/usuarios/usuarios.aspx" ImageUrl="~/portalcfd/images/icons/usuarios.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk11" runat="server" PostBackUrl="~/portalcfd/TimbradoNomina.aspx" ImageUrl="~/portalcfd/images/icons/nomina.jpg" /></td>
                <td align="center"><asp:ImageButton ID="lnk10" runat="server" PostBackUrl="~/portalcfd/Salir.aspx" ImageUrl="~/portalcfd/images/icons/salir.jpg" /></td>
            </tr>
            <tr>
                <td colspan="5"><br /></td>
            </tr>
        </table>
    </fieldset>
</asp:Content>

