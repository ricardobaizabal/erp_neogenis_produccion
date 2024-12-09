<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_folios" Codebehind="folios.aspx.vb" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .titulos 
        {
            font-family:verdana;
            font-size:medium;
            color:Purple;
        }
        .style4
        {
            height: 17px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <br />

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
        
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Image ID="imgPanel1" runat="server" ImageUrl="~/portalcfd/images/comprobant.jpg" ImageAlign="AbsMiddle" />&nbsp;<asp:Label ID="lblFoliosLegend" runat="server" Font-Bold="true" Text="Administración de Folios" CssClass="item"></asp:Label>
                
            </legend>
            <br />
            <table width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lblTipo" runat="server" CssClass="item" Font-Bold="True" Text="Tipo:"></asp:Label>&nbsp;<asp:RequiredFieldValidator ID="valTipo" runat="server" ErrorMessage="* Requerido" ControlToValidate="tipoid" SetFocusOnError="true" InitialValue="0" ValidationGroup="valFolio" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan="4">
                        <asp:Label ID="lblAnnioSolicitud" runat="server" CssClass="item" Font-Bold="True" Text="Año de solicitud:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="tipoid" runat="server" ValidationGroup="valFolio" CssClass="box"></asp:DropDownList>
                    </td>
                    <td colspan="4">
                        <telerik:RadNumericTextBox ID="txtAnnioSolicitud" Runat="server" Width="80px" 
                            DataType="System.Int64">
                            <NumberFormat DecimalDigits="0" />
                        </telerik:RadNumericTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="16%">
                        <asp:Label ID="lblSerie" runat="server" CssClass="item" Font-Bold="True" Text="Serie:"></asp:Label>
                    </td>
                    <td class="style4" width="16%">
                        <asp:Label ID="lblFolioInicial" runat="server" CssClass="item" Font-Bold="True" Text="Folio inicial:"></asp:Label>
                    </td>
                    <td class="style4" width="16%">
                        <asp:Label ID="lblFolioFinal" runat="server" CssClass="item" Font-Bold="True" Text="Folio final:"></asp:Label>
                    </td>
                    <td class="style4" width="16%">
                        <asp:Label ID="lblAprobacion" runat="server" CssClass="item" Font-Bold="True" Text="Aprobación:"></asp:Label>
                    </td>
                    <td class="style4" width="14%">
                        <asp:Label ID="lblEmision" runat="server" CssClass="item" Font-Bold="True" Text="Fec. de emisión:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="20%">
                        <telerik:RadTextBox ID="txtSerie" Runat="server" Width="65%">
                        </telerik:RadTextBox>
                    </td>
                    <td class="style4" width="20%">
                        <telerik:RadNumericTextBox ID="txtFolioInicial" Runat="server" Width="65%" 
                            DataType="System.Int64">
                            <NumberFormat DecimalDigits="0" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td class="style4" width="20%">
                        <telerik:RadNumericTextBox ID="txtFolioFinal" Runat="server" Width="65%" 
                            DataType="System.Int64">
                            <NumberFormat DecimalDigits="0" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td class="style4" width="20%">
                        <telerik:RadNumericTextBox ID="txtAprobacion" Runat="server" Width="85%" 
                            DataType="System.Int64">
                            <NumberFormat DecimalDigits="0" />
                        </telerik:RadNumericTextBox>
                    </td>
                    <td class="style4" width="14%">
                        <telerik:RadDatePicker ID="fecha_emision" runat="server">
                        </telerik:RadDatePicker>
                    </td>
                    
                </tr>
                <tr>
                    <td colspan="5"><br />
                        <asp:Button ID="btnSave" Text="Guardar" runat="server" ValidationGroup="valFolio" />
                        &nbsp;<asp:Label ID="lblMensaje" runat="server" Font-Bold="True" 
                            Font-Names="Verdana" Font-Size="Small" ForeColor="#009933"></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                         <telerik:RadGrid ID="FoliosGrid" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="100" GridLines="None"
                            Skin="Simple">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
                            <MasterTableView Width="100%" Name="Folios" AllowMultiColumnSorting="False">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="tipo" HeaderText="Tipo" UniqueName="tipo">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="serie" HeaderText="Serie" UniqueName="serie">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="aprobacion" HeaderText="No. de aprobación" UniqueName="aprobacion">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="annio_solicitud" HeaderText="Año de solicitud" UniqueName="annio_solicitud">
                                    </telerik:GridBoundColumn>
                                    
                                    <telerik:GridBoundColumn DataField="folioInicial" HeaderText="Folio inicial" UniqueName="folioInicial" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="folioFinal" HeaderText="Folio final" UniqueName="folioFinal" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="folios" HeaderText="Total de folios" UniqueName="folios" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="foliosUtilizados" HeaderText="Folios utilizados" UniqueName="foliosUtilizados" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"
                                        UniqueName="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("aprobacion") + "," + Eval("annio_solicitud").tostring + "," + Eval("tipoid").tostring %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" />
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
        </fieldset>
    </telerik:RadAjaxPanel>
</asp:Content>



