<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" CodeBehind="Facturar40.aspx.vb" MaintainScrollPositionOnPostback="true" Inherits="erp_neogenis.Facturar40" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .titulos {
            font-family: verdana;
            font-size: medium;
            color: Purple;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <asp:Panel ID="panelClients" runat="server">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="imgPanel1" runat="server" ImageUrl="~/portalcfd/images/comprobant.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblClientsSelectionLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table border="0" cellpadding="2" cellspacing="0" align="left" width="100%">
                <tr>
                    <td class="item" colspan="2">
                        <strong>Seleccione el cliente:</strong>&nbsp;<asp:RequiredFieldValidator ID="valClienteID" runat="server" InitialValue="0" ErrorMessage="Seleccione el cliente al cual le va a facturar." ControlToValidate="cmbCliente" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td class="item">
                        <strong>Sucursal:</strong>
                    </td>
                    <td class="item" colspan="2">
                        <strong>Marca:</strong> <asp:RequiredFieldValidator ID="valproyectoid" runat="server" ControlToValidate="cmbProyecto" InitialValue="0" ErrorMessage="Requerido." ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="item" colspan="2">
                        <asp:DropDownList ID="cmbCliente" runat="server" CausesValidation="false" CssClass="item" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="item">
                        <asp:DropDownList ID="cmbSucursal" runat="server" CssClass="box" Width="80%" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="item" colspan="2">
                        <asp:DropDownList id="cmbProyecto" runat="server" CssClass="box" Width="150px" AutoPostBack="false"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="item" style="width: 20%">
                        <strong>Tipo de documento:</strong>&nbsp;<asp:RequiredFieldValidator ID="valSerieId" runat="server" InitialValue="0" ErrorMessage="Requerido" ForeColor="Red" ControlToValidate="cmbTipoDocumento" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td class="item" style="width: 20%">
                        <strong>Método de pago:</strong>&nbsp;<asp:RequiredFieldValidator ID="valMetodoPago" runat="server" InitialValue="0" ErrorMessage="Requerido" ControlToValidate="cmbMetodoPago" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td class="item" style="width: 20%">
                        <strong>Moneda:</strong>&nbsp;<asp:RequiredFieldValidator ID="valMoneda" runat="server" InitialValue="0" ErrorMessage="Requerido" ControlToValidate="cmbMoneda" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td class="item" style="width: 20%">
                        <strong>Tipo de cambio:</strong>&nbsp;<asp:RequiredFieldValidator ID="valTipoCambio" runat="server" ControlToValidate="txtTipoCambio" ForeColor="Red" ErrorMessage="Requerido" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                    </td>
                    <td class="item" style="width: 20%">
                        <strong>Orden de compra:</strong>
                    </td>
                </tr>
                <tr>
                    <td class="item" style="width: 20%">
                        <asp:DropDownList ID="cmbTipoDocumento" runat="server" CssClass="box" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <td class="item" style="width: 20%">
                        <asp:DropDownList ID="cmbMetodoPago" runat="server" CssClass="box" Width="80%"></asp:DropDownList>
                    </td>
                    <td class="item" style="width: 20%">
                        <asp:DropDownList ID="cmbMoneda" runat="server" CssClass="box" Width="80%" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <td class="item" style="width: 20%">    
                        <telerik:RadNumericTextBox ID="txtTipoCambio" runat="server" NumberFormat-DecimalDigits="4" Value="0"></telerik:RadNumericTextBox>
                    </td>
                    <td class="item" style="width: 20%">
                        <telerik:RadTextBox ID="txtOrdenCompra" runat="server" CssClass="item">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="item" colspan="1">
                        <strong>Exportación</strong>&nbsp;<asp:RequiredFieldValidator ID="valExportacion" runat="server" InitialValue="0" ErrorMessage="Requerido" CssClass="item" ControlToValidate="cmbExportacion" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="1">
                        <asp:DropDownList ID="cmbExportacion" runat="server" CssClass="box" Width="90%"></asp:DropDownList>
                    </td>
                    <td colspan="4"></td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelSpecificClient" runat="server">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/portalcfd/images/datClient.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblClientData" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table width="100%" border="0">
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblSocialReason" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblContact" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblCondiciones" runat="server" CssClass="item" Font-Bold="True" Text="Condiciones"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblSocialReasonValue" runat="server" CssClass="item"
                            Font-Bold="False"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblContactValue" runat="server" CssClass="item"
                            Font-Bold="False"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:DropDownList ID="cmbCondiciones" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                </tr>

                <tr>
                    <td width="33%">
                        <asp:Label ID="lblContactPhone" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblRFC" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblTipoPrecio" runat="server" CssClass="item" Font-Bold="True" Text="Tipo de Precio:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblContactPhoneValue" runat="server" CssClass="item"
                            Font-Bold="False"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblRFCValue" runat="server" CssClass="item" Font-Bold="False"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblTipoPrecioValue" runat="server" CssClass="item" Font-Bold="False"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td width="66%" colspan="2">
                        <asp:Label ID="lblUsoCFDI" runat="server" CssClass="item" Font-Bold="True" Text="Uso de CFDI:"></asp:Label>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Requerido" ControlToValidate="cmbUsoCFD" ForeColor="Red" CssClass="item" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>

                    <td width="33%">
                        <asp:Label ID="lblFormaPago" runat="server" Text="Forma de pago:" CssClass="item" Font-Bold="true"></asp:Label>&nbsp;<asp:RequiredFieldValidator ID="valFormaPago" runat="server" ErrorMessage="Requerido" ControlToValidate="cmbFormaPago" ForeColor="Red" CssClass="item" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" width="66%">
                        <asp:DropDownList ID="cmbUsoCFD" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td width="33%">
                        <asp:DropDownList ID="cmbFormaPago" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="66%" colspan="2">
                        <asp:Label ID="lblObservaciones" runat="server" CssClass="item" Font-Bold="True" Text="Observaciones:"></asp:Label>
                    </td>

                    <td width="33%">
                        <asp:Label ID="lblNumCtaPago" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" width="66%">
                        <telerik:RadTextBox ID="instrucciones" runat="server" Width="450px" CssClass="item" TextMode="MultiLine" Height="40px">
                        </telerik:RadTextBox>
                    </td>
                    <td width="33%">
                        <telerik:RadTextBox ID="txtNumCtaPago" runat="server" CssClass="item">
                        </telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td width="33%">
                         <br />
                        <asp:Label ID="lblComplementoFactura" runat="server" CssClass="item" Font-Bold="True" Text="Complemento:"></asp:Label>&nbsp;&nbsp;
                        <asp:DropDownList ID="cmbComplementoFactura" runat="server" AutoPostBack="true" Visible="true" CssClass="box" Width="46%"></asp:DropDownList>
                    </td>
                    <td width="33%">
                        <asp:CheckBox ID="chkDescuentoAplicado" CssClass="item" runat="server" Text="Factura con descuento aplicado" AutoPostBack="true" TextAlign="Right" />
                    </td>
                    <td width="33%">
                        <asp:CheckBox ID="chkAduana" CssClass="item" Visible="false" runat="server" Text="Incluye información aduanera" AutoPostBack="true" TextAlign="Right" />
                    </td>                    
                    <td width="33%">&nbsp;</td>
                </tr>
                <asp:Panel ID="panelInformacionAduanera" runat="server" Visible="false">
                    <tr>
                        <td colspan="3" class="item" style="line-height: 20px;">
                            <strong>Nombre de la aduana:</strong>
                            <asp:RequiredFieldValidator ID="valNombreAduana" runat="server" ControlToValidate="nombreaduana" ErrorMessage="Escriba el nombre de la aduana." SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <telerik:RadTextBox ID="nombreaduana" runat="server" Width="450px" CssClass="item">
                            </telerik:RadTextBox>
                            <br />
                            <strong>Fecha de pedimento:</strong>
                            <asp:RequiredFieldValidator ID="valFechaPedimento" runat="server" ControlToValidate="fechapedimento" ErrorMessage="Selecciona la fecha del pedimento." SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <telerik:RadDatePicker ID="fechapedimento" runat="server">
                            </telerik:RadDatePicker>
                            <br />
                            <strong>Número de pedimento:</strong>
                            <asp:RequiredFieldValidator ID="valNumeroPedimento" runat="server" ControlToValidate="numeropedimento" ErrorMessage="Escriba el número de pedimento." SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <telerik:RadTextBox ID="numeropedimento" runat="server" Width="450px" CssClass="item">
                            </telerik:RadTextBox>
                            <br />
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        </fieldset>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelFacturaGlobal" runat="server" Visible="false">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image6" runat="server" ImageUrl="~/portalcfd/images/comprobant.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="true" CssClass="item" Text="Factura al Público en General - Factura Global"></asp:Label>
            </legend>
            <br />
            <table border="0" cellpadding="2" cellspacing="0" align="left" width="50%">
                <tr>
                    <td class="item">
                        <strong>Periodicidad:</strong>
                        <asp:RequiredFieldValidator ID="valcmbPeriodicidad" runat="server" ControlToValidate="cmbPeriodicidad" ErrorMessage="Requerido*" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                    <td class="item">
                        <strong>Mes:</strong>
                        <asp:RequiredFieldValidator ID="valcmbMes" runat="server" ControlToValidate="cmbMes" ErrorMessage="Requerido*" SetFocusOnError="true" InitialValue="0"></asp:RequiredFieldValidator>
                    </td>
                    <td class="item">
                        <strong>Año:</strong>
                        <asp:RequiredFieldValidator ID="valtxtAnio" runat="server" ControlToValidate="txtAnio" ErrorMessage="Requerido*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="item">
                        <asp:DropDownList ID="cmbPeriodicidad" runat="server" CssClass="box" Width="15em"></asp:DropDownList>
                    </td>
                    <td class="item">
                        <asp:DropDownList ID="cmbMes" runat="server" CssClass="box" Width="15em"></asp:DropDownList>
                    </td>
                    <td class="item">
                        <telerik:RadNumericTextBox Type="Number" NumberFormat-GroupSeparator="" NumberFormat-DecimalDigits="0" MaxLength="4" ID="txtAnio" runat="server" Width="8em" CssClass="item">
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
            </table>
         </fieldset>
       <br />
     </asp:Panel>
    <asp:Panel ID="panelComplementoDetallista" runat="server" Visible="false">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image5" runat="server" ImageUrl="~/portalcfd/images/ListaProveedores_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="Label1" runat="server" Text="Datos Complemento Detallista" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%" border="0">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblDatosGeneralesComplementos" runat="server" Text="DATOS GENERALES" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblDatosCompradorVendedor" runat="server" Text="DATOS COMPRADOR y VENDEDOR" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="25%">&nbsp;</td>
                    <td width="25%">&nbsp;</td>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblDatosDelComprador" runat="server" Text="Datos del comprador(Buyer)" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblFechaOrdenCompra" runat="server" Text="Fecha orden de compra (FECHOC):" Font-Bold="true" CssClass="item" ></asp:Label>
                       
                    </td>
                    <td width="25%">
                        <telerik:RadDatePicker ID="calFechaOrdenCompra" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                    <td width="25%">
                        <asp:Label ID="lblGLNComprador" runat="server" Text="GLN: *" Font-Bold="true" CssClass="item" ForeColor="Red"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtGLNComprador" runat="server" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblNumeroReferenciaAdicional" runat="server" Text="Número de referencia adicional: " Font-Bold="true" CssClass="item" ></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtNumeroDeReferenciaAdicional" runat="server">
                        </telerik:RadTextBox>
                    </td>
                    <td width="25%">
                        <asp:Label ID="lblNumeroDepartamento" runat="server" Text="Número de Departamento(NUMDPT): *" Font-Bold="true" CssClass="item" ForeColor="Red"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtNumeroDepartamento" runat="server" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblNumeroContraRecibo" runat="server" Text="Número de contra-recibo(CONTRA): " Font-Bold="true" CssClass="item" ></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtNumeroDeContraRecibo" runat="server">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="2" align="center">
                        <asp:Label ID="lblDatosDelVendedor" runat="server" Text="Datos del vendedor(Seller)" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblFechaContraRecibo" runat="server" Text="Fecha de contra-recibo(FECCON):" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadDatePicker ID="calFechaContraRecibo" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                    <td width="25%">
                        <asp:Label ID="lblGLNVendedor" runat="server" Text="GLN: *" Font-Bold="true" CssClass="item" ForeColor="Red"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtGLNVendedor" runat="server" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="25%">&nbsp;</td>
                    <td width="25%">&nbsp;</td>
                    <td width="25%">
                        <asp:Label ID="lblNumeroEmisor" runat="server" Text="Número de Emisor(NUMEMI): *" Font-Bold="true" CssClass="item" ForeColor="Red"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtNumeroEmisor" runat="server" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>

    <br />

    <asp:Panel ID="panelComercioExterior" runat="server" Visible="false">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image8" runat="server" ImageUrl="~/portalcfd/images/ListaProveedores_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="Label9" runat="server" Text="Datos Complemento Comercio Exterior" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table style="width: 100%;" border="0" cellpadding="3" cellspacing="3">
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblTipoOperacion" runat="server" CssClass="item" Text="Tipo de operación:" Font-Bold="True"></asp:Label>
                        &nbsp;<span class="item" style="color: red;">* requerido</span>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="cmbTipoOperacion" runat="server" Enabled="false" CssClass="box">
                            <asp:ListItem Text="2 - EXPORTACION" Value="2" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblClavePedimento" runat="server" CssClass="item" Text="Clave de pedimento:" Font-Bold="True"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:DropDownList ID="cmbClavePedimento" runat="server" CssClass="box">
                            <asp:ListItem Text="--SELECCIONE--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="A1 - IMPORTACION O EXPORTACION DEFINITIVA" Value="A1" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblCertificadoOrigen" runat="server" Text="Certificado origen:" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbCertificadoOrigen" runat="server" AutoPostBack="true" CssClass="box">
                            <asp:ListItem Text="--SELECCIONE--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="0 - NO FUNGE COMO CERTIFICADO DE ORIGEN" Value="0"></asp:ListItem>
                            <asp:ListItem Text="1 - FUNGE COMO CERTIFICADO DE ORIGEN" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblNumeroCertificadoOrigen" runat="server" Text="Número certificado origen:" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNumeroCertificadoOrigen" runat="server" Width="350px" CssClass="item">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblNumeroExportadorConfiable" runat="server" Text="Número exportador confiable:" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNumeroExportadorConfiable" runat="server" Width="350px" CssClass="item">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblClaveINCOTERM" runat="server" Text="Clave del INCOTERM:" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbClaveIncoterm" runat="server" CssClass="box">
                            <asp:ListItem Text="--SELECCIONE--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="CFR - COSTE Y FLETE (PUERTO DE DESTINO CONVENIDO)." Value="CFR"></asp:ListItem>
                            <asp:ListItem Text="CIF - COSTE, SEGURO Y FLETE (PUERTO DE DESTINO CONVENIDO)." Value="CIF"></asp:ListItem>
                            <asp:ListItem Text="CPT - TRANSPORTE PAGADO HASTA (EL LUGAR DE DESTINO CONVENIDO)." Value="CPT"></asp:ListItem>
                            <asp:ListItem Text="CIP - TRANSPORTE Y SEGURO PAGADOS HASTA (LUGAR DE DESTINO CONVENIDO)." Value="CIP"></asp:ListItem>
                            <asp:ListItem Text="DAF - ENTREGADA EN FRONTERA (LUGAR CONVENIDO)." Value="DAF"></asp:ListItem>
                            <asp:ListItem Text="DAP - ENTREGADA EN LUGAR." Value="DAP"></asp:ListItem>
                            <asp:ListItem Text="DAT - ENTREGADA EN TERMINAL." Value="DAT"></asp:ListItem>
                            <asp:ListItem Text="DES - ENTREGADA SOBRE BUQUE (PUERTO DE DESTINO CONVENIDO)." Value="DES"></asp:ListItem>
                            <asp:ListItem Text="DEQ - ENTREGADA EN MUELLE (PUERTO DE DESTINO CONVENIDO)." Value="DEQ"></asp:ListItem>
                            <asp:ListItem Text="DDU - ENTREGADA DERECHOS NO PAGADOS (LUGAR DE DESTINO CONVENIDO)." Value="DDU"></asp:ListItem>
                            <asp:ListItem Text="DDP - ENTREGADA DERECHOS PAGADOS (LUGAR DE DESTINO CONVENIDO)." Value="DDP"></asp:ListItem>
                            <asp:ListItem Text="EXW - EN FABRICA (LUGAR CONVENIDO)." Value="EXW"></asp:ListItem>
                            <asp:ListItem Text="FCA - FRANCO TRANSPORTISTA (LUGAR DESIGNADO)." Value="FCA"></asp:ListItem>
                            <asp:ListItem Text="FAS - FRANCO AL COSTADO DEL BUQUE (PUERTO DE CARGA CONVENIDO)." Value="FAS"></asp:ListItem>
                            <asp:ListItem Text="FOB - FRANCO A BORDO (PUERTO DE CARGA CONVENIDO)." Value="FOB"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblSubdivision" runat="server" Text="Subdivisión:" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;<span class="item" style="color: red;">* requerido</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="cmbSubdivision" runat="server" CssClass="box">
                            <asp:ListItem Text="--SELECCIONE--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="NO" Value="0"></asp:ListItem>
                            <asp:ListItem Text="SI" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblTipoCambioAddenda" runat="server" Text="Tipo de cambio:" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;<span class="item" style="color: red;">* requerido</span>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="txtTipoCambioAddenda" runat="server" NumberFormat-DecimalDigits="4" Width="350px"  CssClass="item"></telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 25%;">
                        <asp:Label ID="lblTotalUSD" runat="server" Text="Total USD:" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;<span class="item" style="color: red;">* requerido</span>
                    </td>
                    <td>
                        <telerik:RadNumericTextBox ID="txtTotalUSD" runat="server"  Width="100px" CssClass="item">
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelAddendaCoppel" runat="server" Visible="false">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image7" runat="server" ImageUrl="~/portalcfd/images/ListaProveedores_03.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="Label3" runat="server" Text="Datos Addenda Coppel Ropa" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%" border="0">
                <tr>
                    <td colspan="2" align="center">
                        <asp:Label ID="Label4" runat="server" Text="DATOS GENERALES" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td colspan="2" align="center">
                        <asp:Label ID="Label5" runat="server" Text="Datos del vendedor(Seller)" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblFechaPromesaEntrega" runat="server" Text="Fecha promesa de entrega:" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadDatePicker ID="calFechaPromesaEntrega" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                    <td width="25%" align="right">
                        <asp:Label ID="Label13" runat="server" Text="GLN:" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadTextBox ID="txtGLNVendedorDS" runat="server" Text="0000000000000">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <asp:Label ID="Label6" runat="server" Text="Fecha orden de compra (FECHOC):" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadDatePicker ID="calFechaOrdenCompraDS" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                    <td width="25%">
                         &nbsp;
                    </td>
                    <td width="25%">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td width="25%">
                        <asp:Label ID="lblBodegaDestino" runat="server" Text="Bodega Destino:" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td width="25%">
                        <asp:DropDownList ID="cmbBodegaDestinoCoppel" runat="server" CssClass="item"></asp:DropDownList>
                    </td>
                    <td width="25%">&nbsp;</td>
                    <td width="25%">&nbsp;</td>
                </tr> 
                 <tr>
                    <td width="25%">
                        <asp:Label ID="Label7" runat="server" Text="Cantidad de Cajas:" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                    <td width="25%">
                        <telerik:RadNumericTextBox ID="txtNumLotes" runat="server" Type="Number" NumberFormat-DecimalDigits="0">
                        </telerik:RadNumericTextBox>
                    </td>
                    <td width="25%">&nbsp;</td>
                    <td width="25%">&nbsp;</td>
                </tr> 
                <tr>
                    <td colspan="4">&nbsp;</td>
                </tr>
                <tr>
                    <td width="25%" align="right">
                        <asp:Button ID="btnAddenda" runat="server"  Visible="false"   CausesValidation="false" Text="Generar Addenda" CssClass="item" />
                    </td>
                    <td width="25%">&nbsp;</td>
                    <td width="25%">&nbsp;</td>
                    <td width="25%">&nbsp;</td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />

    <asp:Panel ID="panelItemsRegistration" runat="server" Visible="False">
        <fieldset style="padding: 10px;">
            <asp:HiddenField ID="productoid" Value="0" runat="server" /> 
            <asp:HiddenField ID="partidaid" Value="0" runat="server" />
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image2" runat="server" ImageUrl="~/portalcfd/images/concept.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblClientItems" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <br />
            <table width="900" cellspacing="0" cellpadding="1" border="0" align="center">
                <tr>
                    <td valign="bottom" class="item">
                        <strong>Buscar:</strong>
                        <asp:TextBox ID="txtSearchItem" runat="server" CssClass="box" AutoPostBack="true"></asp:TextBox>&nbsp;presione enter después de escribir el código
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid ID="gridResults" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                            Skin="Simple" Visible="False">
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Items" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Descripción</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDescripcion" runat="server" Text='<%# eval("descripcion") %>' Width="480px" CssClass="box" TextMode="MultiLine" Height="25px"></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Unidad</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnidad" runat="server" Text='<%# eval("unidad") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Cant.</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtQuantity" runat="server" Skin="Default" Width="50px"
                                                MinValue="0" Value='0'>
                                                <NumberFormat DecimalDigits="4" GroupSeparator="" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="talla" Visible="false">
                                        <HeaderTemplate>Talla</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtTalla" runat="server" MinValue="0" Value="0" Skin="Default" Width="50px">
                                                <NumberFormat DecimalDigits="2" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="cajas" Visible="false">
                                        <HeaderTemplate>Cajas</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtCajas" runat="server" MinValue="0" Value="0" Skin="Default" Width="50px">
                                                <NumberFormat DecimalDigits="2" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="piezasporcaja" Visible="false">
                                        <HeaderTemplate>Pzas x Caja</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtPiezasPorCaja" runat="server" MinValue="0" Value="0" Skin="Default" Width="50px">
                                                <NumberFormat DecimalDigits="2" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Precio Unit.</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtUnitaryPrice" runat="server" MinValue="0" Value="0"
                                                Skin="Default" Width="80px">
                                                <NumberFormat DecimalDigits="4" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="NoIdentificacion" Visible="false">
                                        <HeaderTemplate>No. Identificacion *</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNoIdentificacion" runat="server" Text='<%#Eval("codigo") %>' Width="100px" Skin="Default"></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="FraccArancelaria" Visible="false">
                                        <HeaderTemplate>Fracc. Arancelaria</HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<asp:DropDownList ID="cmbFraccionArancelaria" runat="server" CausesValidation="false" CssClass="item"></asp:DropDownList>--%>
                                            <asp:TextBox ID="txtFraccArancelaria" runat="server" Width="100px" Skin="Default"></asp:TextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="CanAduana" Visible="false">
                                        <HeaderTemplate>Cant. Aduana</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtCantAduana" runat="server" MinValue="0" Value="0" Skin="Default" Width="80px">
                                                <NumberFormat DecimalDigits="4" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="UnidadAduana" Visible="false">
                                        <HeaderTemplate>Unidad Aduana</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:DropDownList ID="cmbUnidadAduana" runat="server" CausesValidation="false" CssClass="item"></asp:DropDownList>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                     <telerik:GridTemplateColumn UniqueName="UnitarioAduana" Visible="false">
                                        <HeaderTemplate>Unitario Aduana</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtUnitarioAduana" runat="server" MinValue="0" Value="0" Skin="Default" Width="80px">
                                                <NumberFormat DecimalDigits="4" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="ValorDolares" Visible="false">
                                        <HeaderTemplate>Valor Dólares *</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtValorDolares" runat="server" MinValue="0" Value="0" Skin="Default" Width="80px">
                                                <NumberFormat DecimalDigits="4" GroupSeparator="," />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridBoundColumn DataField="lote" HeaderText="Lote" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="caducidad" HeaderText="Caducidad" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="existencia" HeaderText="Existencia"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="almacen" HeaderText="Almacen"></telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Add">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnAdd" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdAdd" ImageUrl="~/portalcfd/images/action_add.gif" CausesValidation="False" ToolTip="Agregar producto como partida" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <asp:Button ID="btnCancelSearch" Visible="false" runat="server" CausesValidation="False" CssClass="item" Text="Cancelar" />
                    </td>
                </tr>
            </table>
            <br />
            <table width="80%" cellspacing="0" cellpadding="1" border="0" align="center">
                <tr>
                    <td>
                        <br />
                        <telerik:RadGrid ID="itemsList" runat="server" Width="100%" ShowStatusBar="True" ShowFooter="true"
                            AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                            Skin="Simple" Visible="False">
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Items" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="" UniqueName="descripcion">
                                        <ItemStyle VerticalAlign="Top" />
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="" UniqueName="cantidad">
                                    </telerik:GridBoundColumn>
                                    <%--<telerik:GridBoundColumn DataField="precio" HeaderText="" UniqueName="precio" DataFormatString="{0:C}">
                                    </telerik:GridBoundColumn>--%>
                                    <telerik:GridTemplateColumn UniqueName="talla" Visible="false">
                                        <HeaderTemplate>Talla</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtTalla" runat="server" Text='<%# Eval("talla") %>' MinValue="0" Value="0" Skin="Default" Width="50px" AutoPostBack="true" OnTextChanged="ActulizaAtributoPartida_PorNombreDelCampo">
                                                <NumberFormat DecimalDigits="0"  />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="cajas" Visible="false">
                                        <HeaderTemplate>Cajas</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtCajas" runat="server" Text='<%# Eval("cajas") %>'  MinValue="0" Value="0" Skin="Default" Width="50px"  AutoPostBack="true"  OnTextChanged="ActulizaAtributoPartida_PorNombreDelCampo">
                                                <NumberFormat DecimalDigits="0" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn UniqueName="piezasporcaja" Visible="false">
                                        <HeaderTemplate>Pzas x Caja</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtPiezasPorCaja" runat="server" Text='<%# Eval("piezasporcaja") %>'  MinValue="0" Value="0" Skin="Default" Width="50px" AutoPostBack="true"  OnTextChanged="ActulizaAtributoPartida_PorNombreDelCampo">
                                                <NumberFormat DecimalDigits="0" />
                                            </telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" HeaderText="Precio Unitario" UniqueName="precio">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtPrecio" Width="90px" AutoPostBack="true" Type="Currency" Text='<%# Eval("precio") %>' NumberFormat-DecimalDigits="2" OnTextChanged="txtPrecio_TextChanged" runat="server"></telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="descuento" HeaderText="Descuento" UniqueName="descuento" DataFormatString="{0:C3}" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="iva" HeaderText="Iva" UniqueName="iva" DataFormatString="{0:C3}" Aggregate="Sum" FooterStyle-Font-Bold="true" FooterStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="importe" HeaderText="" UniqueName="importe" DataFormatString="{0:C3}">
                                    </telerik:GridBoundColumn>

                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
            </table>
        </fieldset>

    </asp:Panel>

    <br />

    <asp:Panel ID="panelDescuento" runat="server" Visible="False">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image4" runat="server" ImageUrl="~/portalcfd/images/descuento.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblDescDescuento" runat="server" Text="Descuento a nivel factura" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>

            <br />

            <table width="100%" border="0" align="left">
                <tr>
                    <td align="left" style="width: 8%">
                        <asp:Label ID="lblDescuentoGeneral" runat="server" CssClass="item" Font-Bold="True" Text="% Descuento:"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <telerik:RadNumericTextBox ID="txtDescuento" Type="Percent" MinValue="0" MaxValue="100" Width="90%" runat="server"></telerik:RadNumericTextBox>
                    </td>
                    <td>
                        <asp:Button ID="btnAplicarDescuento" runat="server" ValidationGroup="vgDescuento" Text="Aplicar Descuento" CssClass="item" />&nbsp;&nbsp;
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="0" ValidationGroup="vgDescuento" ErrorMessage="Proporcione un porcentaje de descuento" ControlToValidate="txtDescuento" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>

        </fieldset>
    </asp:Panel>

    <br />

    <asp:Panel ID="panelResume" runat="server" Visible="False">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="Image3" runat="server" ImageUrl="~/portalcfd/images/resumen.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblResume" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>

            <br />

            <table width="100%" align="left">
                <tr>
                    <td width="16%" align="left" style="width: 32%">
                        <asp:Label ID="lblSubTotal" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;<asp:Label ID="lblSubTotalValue" runat="server" CssClass="item"
                            Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="16%" align="left" style="width: 32%">
                        <asp:Label ID="lblImporteTasaCero" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;<asp:Label ID="lblImporteTasaCeroValue" runat="server" CssClass="item"
                            Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="16%" align="left" style="width: 32%">
                        <asp:Label ID="lblDescuento" runat="server" CssClass="item" Font-Bold="True" Text="Descuento="></asp:Label>
                        &nbsp;<asp:Label ID="lblDescuentoValue" runat="server" CssClass="item"
                            Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="16%" align="left" style="width: 32%">
                        <asp:Label ID="lblIVA" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;<asp:Label ID="lblIVAValue" runat="server" CssClass="item" Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <%--<tr>
                    <td align="left" style="width: 32%">
                        <asp:Label ID="lblRetISR" runat="server" CssClass="item" Font-Bold="True" Text="Ret. ISR="></asp:Label>
                        &nbsp;<asp:Label ID="lblRetISRValue" runat="server" CssClass="item" Font-Bold="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 32%">
                        <asp:Label ID="lblRetIVA" runat="server" CssClass="item" Font-Bold="True" Text="Ret. IVA="></asp:Label>
                        &nbsp;<asp:Label ID="lblRetIVAValue" runat="server" CssClass="item" Font-Bold="False"></asp:Label>
                    </td>
                </tr>--%>
                <asp:Panel ID="panelRetencion" runat="server" Visible="false">
                    <tr>
                        <td width="16%" align="left" style="width: 32%">
                            <asp:Label ID="lblRet" runat="server" CssClass="item" Font-Bold="True" Text="Retención 4%="></asp:Label>
                            &nbsp;<asp:Label ID="lblRetValue" runat="server" CssClass="item" Font-Bold="False"></asp:Label>
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td width="16%" align="left" style="width: 32%">
                        <asp:Label ID="lblTotal" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;<asp:Label ID="lblTotalValue" runat="server" CssClass="item" Font-Bold="False"></asp:Label>
                    </td>
                </tr>

                <tr>
                    <td align="left" width="16%">
                        <br />
                        <br />
                        <asp:Button ID="btnCreateInvoice" runat="server" CausesValidation="True" CssClass="item" />&nbsp;&nbsp;
                        <asp:Button ID="btnCancelInvoice" runat="server" CausesValidation="False" CssClass="item" />
                        <br />
                        <br />
                    </td>
                </tr>
            </table>

        </fieldset>
    </asp:Panel>

    <telerik:RadWindow ID="RadWindow1" runat="server" Modal="true" CenterIfModal="true" AutoSize="False" Behaviors="Close" VisibleOnPageLoad="False" Width="740" Height="600">
        <ContentTemplate>
            <br />
            <table align="center" width="95%">
                <tr>
                    <td>
                        <asp:TextBox ID="txtErrores" TextMode="MultiLine" Width="100%" Rows="32" ReadOnly="true" CssClass="item" runat="server"></asp:TextBox>
                    </td>
                    <tr>
                        <td align="left" width="16%">
                            <br />
                            <br />
                            <asp:Button ID="btnAceptar" runat="server" CausesValidation="true" CssClass="item" Text="Aceptar" />
                            <br />
                            <br />
                        </td>
                    </tr>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </telerik:RadWindow>
    <%--<telerik:RadWindow ID="RadWindow1" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" VisibleOnPageLoad="false" Behaviors="Close" Width="700" Height="600">
        <ContentTemplate>
            <br />
            <table align="center" width="90%" border="0">
                <tr>
                    <td>
                        <telerik:RadGrid ID="itemsInventoryList" runat="server" Width="100%" ShowStatusBar="True" AutoGenerateColumns="False" AllowPaging="False" GridLines="None" Skin="Simple">
                            <MasterTableView Width="100%" DataKeyNames="id,productoid,existencia" Name="Items" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="ID" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripcion" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="Unidad" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="almacen" HeaderText="Almacén" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="lote" HeaderText="Lote" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="caducidad" HeaderText="Caducidad" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="existencia" HeaderText="Existencia" ItemStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Cantidad</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtCantidad" runat="server" MinValue="0" Value="0" Skin="Default" Width="80px" OnTextChanged="txtCantidad_TextChanged" AutoPostBack="true">
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
                    <td style="text-align: right">
                        <asp:Label ID="lblTotalDetalle" runat="server" Text="Total: 0" Font-Bold="true" CssClass="item"></asp:Label>
                    </td>
                </tr>
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
                    </td>
                </tr>
            </table>
            <br />
        </ContentTemplate>
    </telerik:RadWindow>--%>

</asp:Content>
