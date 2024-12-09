<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_CFD_Detalle" CodeBehind="CFD_Detalle.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
    <a href="./reportes/ingresosDiv.aspx" class="" >volver</a>
    <br />
     <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblEstatusCobranza" runat="server" Font-Bold="true" CssClass="item" Text="CFDI Estatus de cobranza"></asp:Label>
        </legend>
        <br />
        <table width="90%">
            <tr>
                <td colspan="5" class="item">
                    <strong>Cliente: </strong>
                    <asp:Label ID="lblCliente" runat="server" Font-Bold="true"></asp:Label><br /><br />
                    <strong>Documento: </strong>
                    <asp:Label ID="lblDocumento" runat="server" Font-Bold="true"></asp:Label><br /><br />
                    <strong>Total Factura: </strong>
                    <asp:Label ID="lblTotalFactura" runat="server" Font-Bold="true"></asp:Label><br /><br /><br />
                </td>
            </tr>
            <tr>
                <td class="item">Tipo de pago:<br />
                    <br />
                    <asp:DropDownList ID="tipo_pagoid" runat="server" CssClass="box"></asp:DropDownList>
                </td>
                <td class="item">Referencia:<br />
                    <br />
                    <asp:TextBox ID="referencia" runat="server" CssClass="box"></asp:TextBox></td>
                <td class="item">Monto:<br />
                    <br />
                    <telerik:RadNumericTextBox ID="monto" NumberFormat-DecimalDigits="2" Value="0" MinValue="0" runat="server" CssClass="box"></telerik:RadNumericTextBox>
                </td>
                <td class="item">Fecha de pago:<br />
                    <br />
                    <telerik:RadDatePicker ID="fechapago" runat="server" DateInput-DateFormat="dd/MM/yyyy"></telerik:RadDatePicker>
                </td>
                <td style="vertical-align: bottom">
                    <br />
                    <asp:Button ID="btnSave" runat="server" Text="Guardar" CssClass="boton" CausesValidation="true" ValidationGroup="valpago" />
                </td>
            </tr>
            <tr>
                <td class="item">
                    <asp:RequiredFieldValidator ID="valTipoPago" ControlToValidate="tipo_pagoid" ErrorMessage="* requerido" ForeColor="Red" SetFocusOnError="true" InitialValue="0" runat="server"  ValidationGroup="valpago"></asp:RequiredFieldValidator>
                </td>
                <td class="item">&nbsp;</td><td class="item">&nbsp;</td>
                <td class="item">
                    <asp:RequiredFieldValidator ID="valFecha" ControlToValidate="fechapago" ErrorMessage="* requerido" ForeColor="Red" SetFocusOnError="true" runat="server"  ValidationGroup="valpago"></asp:RequiredFieldValidator>
                </td>
                <td class="item">&nbsp;</td>
            </tr>
            <tr>
                <td class="item" style="text-align:left;">
                    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <br />
        <telerik:RadGrid ID="cfdiListPagos" runat="server" Width="100%" ShowStatusBar="True"
            AutoGenerateColumns="False" AllowPaging="True" PageSize="15" GridLines="None" ShowFooter="true"
            Skin="Simple" AllowFilteringByColumn="false">
            <PagerStyle Mode="NumericPages"></PagerStyle>
            <MasterTableView Width="100%" DataKeyNames="id" Name="Clients" AllowMultiColumnSorting="False">
                <Columns>
                    <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" UniqueName="fecha" DataFormatString="{0:d}" AllowFiltering="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="tipo_pago" HeaderText="Tipo Pago" UniqueName="tipo_pago" AllowFiltering="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" UniqueName="referencia" AllowFiltering="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="monto" HeaderText="Monto" UniqueName="monto" DataFormatString="{0:c}" AllowFiltering="false" ItemStyle-HorizontalAlign="Right">
                    </telerik:GridBoundColumn>
                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Borrar">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkBorrar" runat="server" Text="Eliminar" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete"></asp:LinkButton>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                   </telerik:GridTemplateColumn>
                </Columns>
            </MasterTableView>
        </telerik:RadGrid>
        <br />
        <br />
    </fieldset>
    <br />
    <br />
</asp:Content>