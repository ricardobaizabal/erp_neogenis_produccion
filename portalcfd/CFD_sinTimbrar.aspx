﻿<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_CFD_sinTimbrar" CodeBehind="CFD_sinTimbrar.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function OnRequestStart(target, arguments) {
            if ((arguments.get_eventTarget().indexOf("lnkPDF") > -1) || (arguments.get_eventTarget().indexOf("lnkXML") > -1) || (arguments.get_eventTarget().indexOf("lnkAcuseCancelado") > -1) || (arguments.get_eventTarget().indexOf("imgSend") > -1)) {
                arguments.set_enableAjax(false);
            }
        }
        function openRadWindow(url) {
            var radwindow = $find('<%=RadWindow2.ClientID %>');
            radwindow.setUrl(url);
            radwindow.show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="Label1" runat="server" Font-Bold="true" CssClass="item" Text="Encontrar CFDI"></asp:Label>
            </legend>
            <br />
            <span class="item">Serie:
            <asp:TextBox ID="txtSerie" runat="server" CssClass="box"></asp:TextBox>&nbsp;Folio:
            <asp:TextBox ID="txtFolio" runat="server" CssClass="box"></asp:TextBox>&nbsp;&nbsp;<asp:Button
                ID="btnView" runat="server" Text="Ver" CssClass="box" />&nbsp;&nbsp;<asp:Label ID="lblMensaje" Font-Bold="true" ForeColor="Red" runat="server"></asp:Label>
            </span>
            <br />
            <br />
        </fieldset>
        <br />
        <fieldset style="padding: 10px;">
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="Label2" runat="server" Font-Bold="true" CssClass="item" Text="Buscador"></asp:Label>
            </legend>
            <br />
            <span class="item">Desde:
            <telerik:RadDatePicker ID="fha_ini" runat="server">
            </telerik:RadDatePicker>
                hasta:
            <telerik:RadDatePicker ID="fha_fin" runat="server">
            </telerik:RadDatePicker>
                &nbsp;
            Tipo de documento:&nbsp;
            <asp:DropDownList ID="tipoid" runat="server" CssClass="box"></asp:DropDownList>&nbsp;
            <br />
                <br />
                Cliente:&nbsp;
            <asp:DropDownList ID="clienteid" runat="server" CssClass="box"></asp:DropDownList>&nbsp;
            <asp:Button ID="btnSearch" runat="server" CssClass="boton" Text="Buscar" />
            </span>
            <br />
            <br />
        </fieldset>
        <br />
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
                        <telerik:RadGrid ID="cfdlist" runat="server" Width="100%" ShowStatusBar="True"
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="15" GridLines="None"
                            Skin="Simple" OnNeedDataSource="cfdlist_NeedDataSource" AllowFilteringByColumn="false">
                            <PagerStyle Mode="NumericPages"></PagerStyle>
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
                                    <telerik:GridBoundColumn DataField="estatus" HeaderText="Estatus" UniqueName="estatus" AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="serie" HeaderText="Serie" UniqueName="serie" AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="folio" HeaderText="Folio" UniqueName="folio" AllowFiltering="false">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="total" AllowSorting="false" HeaderText="Total" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                                    </telerik:GridBoundColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" Text="editar" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Visible="true"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Timbrado" UniqueName="">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTimbrado" runat="server"></asp:Label>
                                            <asp:Image ID="imgTimbrado" runat="server" ImageAlign="AbsMiddle" ImageUrl="~/images/icons/arrow.gif" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </telerik:GridTemplateColumn>

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
        <telerik:RadWindowManager ID="RadAlert" runat="server" Skin="Bootstrap" EnableShadow="false" Localization-OK="Aceptar" Localization-Cancel="Cancelar" RenderMode="Lightweight"></telerik:RadWindowManager>
        <telerik:RadWindow ID="RadWindow1" runat="server" ShowContentDuringLoad="False" Modal="True" ReloadOnShow="True" VisibleStatusbar="False" VisibleTitlebar="True" BorderStyle="None" BorderWidth="0px" Behaviors="Close" Width="600px" Height="550px" Skin="Bootstrap">
            <ContentTemplate>
                <table style="width: 95%; height: 400px;" align="center" cellpadding="0" cellspacing="3" border="0">
                    <tr>
                        <td colspan="2" style="height: 10px">&nbsp;</td>
                    </tr>
                    <tr valign="top">
                        <td style="width: 10%">
                            <asp:Label ID="lblFrom" runat="server" Font-Bold="true" CssClass="item" Text="De:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFrom" runat="server" Width="100%" Enabled="false" CssClass="box"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="vgEmail" ErrorMessage="Requerido" ControlToValidate="txtFrom" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="width: 10%">
                            <asp:Label ID="lblTo" runat="server" Font-Bold="true" CssClass="item" Text="Para:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTo" runat="server" Width="100%" Enabled="false" CssClass="box"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ValidationGroup="vgEmail" ErrorMessage="Requerido" ControlToValidate="txtTo" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="width: 10%">
                            <asp:Label ID="lblCC" runat="server" Font-Bold="true" CssClass="item" Text="CC:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCC" runat="server" Width="100%" CssClass="box"></asp:TextBox>&nbsp;
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Formato no válido" ValidationGroup="vgEmail"
                                    ControlToValidate="txtCC" CssClass="error"
                                    ValidationExpression=".*@.*\..*"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td style="width: 10%">
                            <asp:Label ID="lblCC0" runat="server" Font-Bold="true" CssClass="item" Text="CCO:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCCO" runat="server" Width="100%" CssClass="box"></asp:TextBox>&nbsp;
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ForeColor="Red" SetFocusOnError="true" Text="Formato no válido" ValidationGroup="vgEmail"
                                    ControlToValidate="txtCCO" CssClass="error"
                                    ValidationExpression=".*@.*\..*"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 10%">
                            <asp:Label ID="lblSubject" runat="server" Font-Bold="true" CssClass="item" Text="Asunto:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" Width="100%" CssClass="box"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtMenssage" TextMode="MultiLine" runat="server" Width="100%" Height="180px" CssClass="box"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 15px">
                            <asp:Label ID="lblMensajeEmail" runat="server" Style="color: #FF0000" Font-Bold="true" CssClass="item"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button ID="btnSendEmail" runat="server" ValidationGroup="vgEmail" CssClass="boton" Width="100px" Height="30px" Text="Enviar" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 10px">
                            <asp:HiddenField ID="tempcfdid" runat="server" Value="0" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadWindow ID="RadWindow2" runat="server" Modal="true" CenterIfModal="true" BorderStyle="None" BorderWidth="0px" AutoSize="False" Behaviors="Close" Skin="Bootstrap" VisibleOnPageLoad="False" Width="1024" Height="768">
        </telerik:RadWindow>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
           
</asp:Content>
