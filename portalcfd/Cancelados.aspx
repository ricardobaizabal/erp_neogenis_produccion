<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="Cancelados.aspx.vb" Inherits="erp_neogenis.Cancelados" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
    function openRadWindow(url) {
        var radwindow = $find('<%=RadWindow1.ClientID %>');
        radwindow.setUrl(url);
        radwindow.show();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <br />
    <fieldset style="padding: 10px;">
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="item" Text="Encontrar CFDI"></asp:Label>
        </legend>
        <br />
        <span class="item">Año:</span>&nbsp;
        <asp:DropDownList ID="cmbAnio" runat="server">
            <asp:ListItem Value="2018" Text="2018"></asp:ListItem>
            <asp:ListItem Value="2019" Text="2019"></asp:ListItem>
            <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
        </asp:DropDownList>
        <span class="item">Serie:
        <asp:TextBox ID="txtSerie" runat="server" Text="A"></asp:TextBox>&nbsp;Folio:
        <asp:TextBox ID="txtFolio" runat="server"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnView" runat="server" Width="90px" Text="Consultar" />&nbsp;&nbsp;<asp:Label ID="lblMensaje" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
        </span>
        <br />
        <br />
    </fieldset>
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblCFDList" runat="server" Font-Bold="true" CssClass="item" Text="Lista de CFDs"></asp:Label>
        </legend>
        <table width="100%">
            <tr>
                <td style="height: 5px">&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="cfdlist" runat="server" Width="100%" ShowStatusBar="True" ShowFooter="true"
                        AutoGenerateColumns="False" AllowPaging="True" PageSize="15" GridLines="None"
                        Skin="Simple" AllowFilteringByColumn="false">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <FooterStyle Font-Bold="true" HorizontalAlign="Right" />
                        <MasterTableView Width="100%" DataKeyNames="id, consignacionid" NoMasterRecordsText="No se encontraron registros para mostrar." Name="Clients" AllowMultiColumnSorting="False">
                            <Columns>
                                <telerik:GridBoundColumn DataField="consignacionid" HeaderText="consignacionid" UniqueName="consignacionid" Visible="false" DataFormatString="{0:d}" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="fecha" HeaderText="Fecha" UniqueName="fecha" DataFormatString="{0:d}" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" UniqueName="cliente" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="rfc" HeaderText="RFC" UniqueName="rfc" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="estatuscfdi" HeaderText="Estatus" UniqueName="estatuscfdi" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" UniqueName="estatus" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="serie" HeaderText="Serie" UniqueName="serie" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="folio" HeaderText="Folio" UniqueName="folio" AllowFiltering="false">
                                </telerik:GridBoundColumn>
                                <%--<telerik:GridBoundColumn DataField="uuid" HeaderText="UUID" UniqueName="uuid" AllowFiltering="false">
                                </telerik:GridBoundColumn>--%>
                                <telerik:GridBoundColumn DataField="total" AllowSorting="false" HeaderText="Total" UniqueName="total" DataFormatString="{0:c}" Aggregate="Sum" ItemStyle-HorizontalAlign="Right">
                                </telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCancelar" runat="server" Text="Cancelar" CommandArgument='<%# Eval("id") %>' CommandName="cmdCancel"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkAcuse" runat="server" Text="Ver Acuse" CommandArgument='<%# Eval("id") %>' CommandName="cmdAcuse"></asp:LinkButton>
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
                <td style="height: 5px">&nbsp;</td>
            </tr>
            <tr>
                <td style="height: 5px">&nbsp;</td>
            </tr>
        </table>
    </fieldset>
    <telerik:RadWindow ID="RadWindow1" runat="server" Modal="true" CenterIfModal="true" AutoSize="False" Behaviors="Close" VisibleOnPageLoad="False" Width="1200" Height="900">
    </telerik:RadWindow>
</asp:Content>