<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" CodeBehind="Facturar_Consignacion.aspx.vb" MaintainScrollPositionOnPostback="true" Inherits="erp_neogenis.Facturar_Consignacion" %>

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
                        <strong>Seleccione el cliente:</strong>&nbsp;<asp:RequiredFieldValidator ID="valClienteID" runat="server" InitialValue="0" ErrorMessage="Seleccione el cliente al cual le va a facturar." ControlToValidate="cmbClient" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                        <br />
                        <asp:DropDownList ID="cmbClient" runat="server" CausesValidation="false" CssClass="item" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="item">
                        <strong>Sucursal:</strong>&nbsp;<br />
                        <br />
                        <asp:DropDownList ID="cmbSucursal" runat="server" CssClass="box" Width="80%" AutoPostBack="true"></asp:DropDownList>
                    </td>
                    <td class="item" colspan="2">
                        <strong>Lugar de expedición:</strong>&nbsp;<asp:RequiredFieldValidator ID="valExpedicion" runat="server" ErrorMessage="Especifique el lugar de expedición." ControlToValidate="txtLugarExpedicion" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                        <br />
                        <telerik:RadTextBox ID="txtLugarExpedicion" runat="server" Width="100%" CssClass="item">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td class="item" style="width: 20%">
                        <strong>Tipo de documento:</strong>&nbsp;<asp:RequiredFieldValidator ID="valSerieId" runat="server" InitialValue="0" ErrorMessage="Requerido." ControlToValidate="serieid" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                        <br />
                        <asp:DropDownList ID="serieid" runat="server" CssClass="box" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <td class="item" style="width: 20%">
                        <strong>Forma de pago:</strong>&nbsp;<asp:RequiredFieldValidator ID="valTipoPago" runat="server" InitialValue="0" ErrorMessage="Requerido." ControlToValidate="tipopagoid" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                        <br />
                        <asp:DropDownList ID="tipopagoid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    <td class="item" style="width: 20%">
                        <strong>Tasa:</strong>&nbsp;<asp:RequiredFieldValidator ID="valTasaid" runat="server" InitialValue="0" ErrorMessage="Requerido." ControlToValidate="serieid" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                        <br />
                        <asp:DropDownList ID="tasaid" runat="server" CssClass="box" AutoPostBack="True"></asp:DropDownList>
                    </td>
                    <td class="item" style="width: 20%">
                        <asp:Panel ID="panelDivisas" runat="server" Visible="false">
                            <strong>Tipo de cambio:</strong>&nbsp;<asp:RequiredFieldValidator ID="valTipoCambio" runat="server" ControlToValidate="tipocambio" ErrorMessage="Requerido" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <br />
                            $
                            <telerik:RadNumericTextBox ID="tipocambio" runat="server" NumberFormat-DecimalDigits="2" Value="0"></telerik:RadNumericTextBox>
                        </asp:Panel>
                    </td>
                    <td class="item" style="width: 20%">
                        <strong>Orden de compra:</strong><br />
                        <br />
                        <telerik:RadTextBox ID="txtOrdenCompra" runat="server" CssClass="item">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td class="item" colspan="5">
                        <br />
                        <strong>Proyecto:</strong>
                        <asp:RequiredFieldValidator ID="valproyectoid" runat="server" ControlToValidate="proyectoid" InitialValue="0" ErrorMessage="Requerido." ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                        <br />
                        <asp:DropDownList ID="proyectoid" runat="server" CssClass="box" Width="150px" AutoPostBack="false"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    <br />
    <asp:Panel ID="panelSpecificClient" runat="server" Visible="False">
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
                        <asp:DropDownList ID="condicionesId" runat="server" CssClass="box"></asp:DropDownList>
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
                        <asp:Label ID="lblEnviar" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>

                    <td width="33%">
                        <asp:Label ID="lblMetodoPago" runat="server" CssClass="item" Font-Bold="true"></asp:Label>&nbsp;<asp:RequiredFieldValidator ID="valMetodoPago" runat="server" ErrorMessage="Requerido" ControlToValidate="metodopagoid" CssClass="item" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td colspan="2" width="66%">
                        <telerik:RadTextBox ID="enviara" runat="server" Width="450px" CssClass="item">
                        </telerik:RadTextBox>
                    </td>
                    <td width="33%">
                        <asp:DropDownList ID="metodopagoid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="66%" colspan="2">
                        <asp:Label ID="lblInstruccionesEspeciales" runat="server" CssClass="item"
                            Font-Bold="True" Text="Instrucciones Especiales y/o Consignado a:"></asp:Label>
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
                        <asp:CheckBox ID="chkAduana" CssClass="item" Visible="false" runat="server" Text="Incluye información aduanera" AutoPostBack="true" TextAlign="Right" />
                    </td>
                    <td width="33%">&nbsp;</td>
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

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>Precio Unit.</HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtUnitaryPrice" runat="server" MinValue="0" Value="0"
                                                Skin="Default" Width="80px">
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
            <table width="900" cellspacing="0" cellpadding="1" border="0" align="center">
                <tr>
                    <td>
                        <br />
                        <telerik:RadGrid ID="itemsList" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                            Skin="Simple" Visible="False">
                            <MasterTableView Width="100%" DataKeyNames="id" Name="Items" AllowMultiColumnSorting="False">
                                <Columns>
                                    <%--<telerik:GridTemplateColumn>
                                        <HeaderTemplate>Código</HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lblCodigo" runat="server" Text='<%# eval("codigo") %>' CommandName="cmdView" CommandArgument='<%# Eval("id") & "," & Eval("productoid") & "," & Eval("cantidad")%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                    <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="" UniqueName="descripcion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="unidad" HeaderText="" UniqueName="unidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="cantidad" HeaderText="" UniqueName="cantidad">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="precio" HeaderText="" UniqueName="precio" DataFormatString="{0:C}">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="importe" HeaderText="" UniqueName="importe" DataFormatString="{0:C}">
                                    </telerik:GridBoundColumn>
                                    <%--<telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" UniqueName="estatus" ItemStyle-HorizontalAlign="Center">
                                    </telerik:GridBoundColumn>--%>
                                    <%--<telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" CausesValidation="False" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>--%>
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
                <tr>
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
                </tr>
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
    <telerik:RadWindow ID="RadWindow1" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" VisibleOnPageLoad="false" Behaviors="Close" Width="700" Height="600">
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
    </telerik:RadWindow>
</asp:Content>
