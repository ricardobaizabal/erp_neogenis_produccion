<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="recibir.aspx.vb" Inherits="erp_neogenis.recibir" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>NeoGenis</title>
    <link href="../Styles/Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <script type="text/javascript">
            //Put your JavaScript code here.
        </script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <div>
            <br />
            <fieldset class="item">
                <legend style="padding-right: 6px; color: Black">
                    <asp:Label ID="lblRecibirPartidas" runat="server" Font-Bold="true" CssClass="item" Text="Recibir productos"></asp:Label>
                </legend>
                <br />
                Código:
                <asp:Label ID="lblCodigo" runat="server"></asp:Label><br />
                Descripción:
                <asp:Label ID="lblDescripcion" runat="server"></asp:Label><br />
                Cantidad solicitada:
                <asp:Label ID="lblCantidad" runat="server"></asp:Label><br />
                <br />
                <asp:Label ID="lblPerecederoBit" Visible="False" runat="server"></asp:Label>
                <table width="100%" cellspacing="2" cellpadding="2" align="left" style="line-height: 25px;">
                    <tr>
                        <td class="item">Cantidad:
                            <asp:RequiredFieldValidator ID="valCantidad" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtCantidad" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <telerik:RadNumericTextBox ID="txtCantidad" Width="100px" runat="server"></telerik:RadNumericTextBox>
                        </td>
                        <td class="item">Fecha de caducidad:
                            <asp:RequiredFieldValidator ID="valCaducidad" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtCaducidad" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <telerik:RadDatePicker ID="txtCaducidad" runat="server"></telerik:RadDatePicker>
                        </td>
                        <td class="item">Lote:
                            <asp:RequiredFieldValidator ID="valLote" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtLote" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <telerik:RadTextBox ID="txtLote" runat="server"></telerik:RadTextBox>
                        </td>
                        <td class="item">Costo variable:
                            <asp:RequiredFieldValidator ID="valCostoVariable" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="txtCostoVariable" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <telerik:RadNumericTextBox ID="txtCostoVariable" Width="100px" runat="server"></telerik:RadNumericTextBox>
                        </td>
                        <td class="item">Almacén:
                            <asp:RequiredFieldValidator ID="valAlmacen" runat="server" ErrorMessage="*" ForeColor="Red" ControlToValidate="almacenid" InitialValue="0" SetFocusOnError="true"></asp:RequiredFieldValidator><br />
                            <asp:DropDownList ID="almacenid" runat="server" CssClass="item"></asp:DropDownList>
                        </td>
                        <td>
                            <br />
                            <asp:Button ID="btnAdd" runat="server" Text="Aplicar" CssClass="item" />
                        </td>
                        <td style="width: 30%"></td>
                    </tr>
                    <tr>
                        <td colspan="6" class="item"><span style="color: Red;">Todos los campos marcados con (*) son requeridos.</span></td>
                    </tr>
                    <tr>
                        <td colspan="6" class="item">
                            <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" CssClass="item"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <br />
                            <telerik:RadGrid ID="conceptosList" runat="server" Width="100%" ShowStatusBar="True"
                                AutoGenerateColumns="False" AllowPaging="True" PageSize="15" GridLines="None"
                                Skin="Simple" ShowFooter="false">
                                <PagerStyle Mode="NumericPages"></PagerStyle>
                                <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cantidad Recibida" UniqueName="cantidad" ItemStyle-HorizontalAlign="Center">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="lote" HeaderText="Lote" UniqueName="cantidad">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="caducidad" HeaderText="Caducidad" UniqueName="caducidad">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="costo_variable" HeaderText="Costo Variable Prom. (MXN)" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                        </telerik:GridBoundColumn>


                                        <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"
                                            UniqueName="Del">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>'
                                                    CommandName="cmdDel" ImageUrl="~/images/action_delete.gif" />
                                            </ItemTemplate>
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
        </div>
    </form>
</body>
</html>
