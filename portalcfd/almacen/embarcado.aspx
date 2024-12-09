<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="embarcado.aspx.vb" Inherits="erp_neogenis.embarcado" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <fieldset style="min-height: 350px;">
        <legend style="padding-right: 6px; color: Black;">
            <asp:Label ID="lblFacturas" runat="server" Font-Bold="true" CssClass="item" Text="Facturas emitidas sin embarcar"></asp:Label>
        </legend>
        <table width="100%">
            <tr>
                <td style="height: 5px">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="cfdList" runat="server" Width="100%" ShowStatusBar="True"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="15" GridLines="None"
                        Skin="Simple" AllowFilteringByColumn="false">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <MasterTableView Width="100%" DataKeyNames="id" NoMasterRecordsText="No hay registros para mostrar." Name="Facturas" AllowMultiColumnSorting="False">
                            <Columns>
                                <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" UniqueName="fecha" DataFormatString="{0:d}" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" UniqueName="cliente" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="rfc" HeaderText="RFC" UniqueName="rfc" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" UniqueName="estatus" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="serie" HeaderText="Serie" UniqueName="serie" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="folio" HeaderText="Folio" UniqueName="folio" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="editar" CommandArgument='<%# Eval("id") %>' CommandName="cmdView" Visible="true"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
