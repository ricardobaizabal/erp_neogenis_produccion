<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.CuentasConcentradoras" CodeBehind="CuentasConcentradoras.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4 {
            height: 17px;
        }

        .style5 {
            height: 14px;
        }

        .style6 {
            height: 21px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        <br />
       
            <fieldset>
                <legend style="padding-right: 6px; color: Black">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/images/icons/AgreEditUsuario_03.jpg" ImageAlign="AbsMiddle" />&nbsp;
                    <asp:Label ID="lblUserEditLegend" runat="server" Font-Bold="True" CssClass="item"></asp:Label>
                </legend>

                <br />

                <table width="50%" border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblNombre" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                            <asp:RequiredFieldValidator ID="valtxtNombre" runat="server" ForeColor="Red" ValidationGroup="valcuenta"
                                                            ControlToValidate="txtNombre" CssClass="item" Text="Requerido"></asp:RequiredFieldValidator>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 35%">
                            <telerik:RadTextBox ID="txtNombre" runat="server" Width="65%">
                            </telerik:RadTextBox>&nbsp;&nbsp;&nbsp;                           
                            <asp:Button ID="btnSave" Text="Agregar" runat="server" CssClass="item" CausesValidation="true" ValidationGroup="valcuenta" />&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnCancelar" CausesValidation="false" Text="Cancelar" runat="server" CssClass="item" Visible="false" OnClick="cleanInputClientAutorizado" />
                        </td>
                    </tr>
                    <tr valign="top">
                          <td colspan="4" align="right">
                              
                             
                          </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 5px;">
                            <asp:HiddenField ID="cuentaconcentradoraid" runat="server" Value="0" />
                        </td>
                    </tr>
                </table>
                 <table width="50%" border="0">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="cuentasList" runat="server" AllowPaging="True"
                            AutoGenerateColumns="False" GridLines="None" PageSize="15" ShowStatusBar="True"
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id"
                                Name="Unidad" Width="100%" NoMasterRecordsText="No se encontraron registros.">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Folio" HeaderStyle-Font-Bold="true"
                                        UniqueName="id">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="Nombre" HeaderStyle-Font-Bold="true"
                                        UniqueName="nombre">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>'
                                                CommandName="cmdEditar" Text='<%# Eval("nombre") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-Font-Bold="true"
                                        HeaderStyle-HorizontalAlign="Center" UniqueName="Delete" HeaderText="Eliminar">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server"
                                                CommandArgument='<%# Eval("id") %>' CommandName="cmdEliminar"
                                                ImageUrl="~/images/action_delete.gif" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td style="height: 2px"></td>
                </tr>
            </table>
        </fieldset>
           
        <br />
        

        
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>