<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="ComplementosEmitidos.aspx.vb" Inherits="erp_neogenis.ComplementosEmitidos" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function openRadWindow(url) {
            var radwindow = $find('<%=RadWindow2.ClientID %>');
            radwindow.setUrl(url);
            radwindow.show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br />
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
            Hasta:
        <telerik:RadDatePicker ID="fha_fin" runat="server">
        </telerik:RadDatePicker>
            &nbsp;
        <br />
            <br />
            Cliente:&nbsp;
        <asp:DropDownList ID="clienteid" runat="server" CssClass="box"></asp:DropDownList>&nbsp;
        <asp:Button ID="btnSearch" runat="server" CssClass="boton" Text="Buscar" />&nbsp;&nbsp;
        </span>
        <br />
        <br />
    </fieldset>
    <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblCFDList" runat="server" Font-Bold="true" CssClass="item" Text="Lista de CFDIs"></asp:Label>
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
                        Skin="Simple" AllowFilteringByColumn="false">
                        <PagerStyle Mode="NumericPages"></PagerStyle>
                        <MasterTableView Width="100%" DataKeyNames="id" Name="Clients" AllowMultiColumnSorting="False">
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
                                        <asp:LinkButton ID="lnkEdit" runat="server" Text="editar" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit"></asp:LinkButton>
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

                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkXML" runat="server" Text="xml" CommandArgument='<%# Eval("id") %>' CommandName="cmdXML"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPDF" runat="server" Text="pdf" CommandArgument='<%# Eval("id") %>' CommandName="cmdPDF"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Enviar" UniqueName="">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSend" runat="server" ImageUrl="~/portalcfd/images/envelope.jpg" CommandArgument='<%# Eval("id") %>' CommandName="cmdSend" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Cancelar">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCancelar" runat="server" Text="Cancelar" CommandArgument='<%# Eval("id") %>' CommandName="cmdCancel"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkAcuse" runat="server" Text="Ver acuse" CommandArgument='<%# Eval("id") %>' CommandName="cmdAcuse"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
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
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" BorderStyle="None" BorderWidth="0px" VisibleStatusbar="True" VisibleTitlebar="False">
        <Windows>
            <telerik:RadWindow ID="RadWindow1" runat="server" ShowContentDuringLoad="False" Modal="True" ReloadOnShow="True" VisibleStatusbar="False" VisibleTitlebar="True" BorderStyle="None" BorderWidth="0px" Behaviors="Close" Width="600px" Height="500px" Skin="Silk">
                <ContentTemplate>
                    <table style="width: 95%; height: 100%;" align="center" cellpadding="0" cellspacing="3" border="0">
                        <tr>
                            <td colspan="2" style="height: 10px">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <asp:Label ID="lblFrom" runat="server" Font-Bold="true" CssClass="item" Text="De:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFrom" runat="server" Width="100%" Enabled="false" CssClass="box"></asp:TextBox><%--&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Requerido" ControlToValidate="txtFrom" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <asp:Label ID="lblTo" runat="server" Font-Bold="true" CssClass="item" Text="Para:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTo" runat="server" Width="100%" Enabled="false" CssClass="box"></asp:TextBox><%--&nbsp;<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Requerido" ControlToValidate="txtTo" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                <asp:Label ID="lblCC" runat="server" Font-Bold="true" CssClass="item" Text="CC:"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCC" runat="server" Width="100%" CssClass="box"></asp:TextBox>
                                <br />
                                <span style="color: #FF0000">* Los emails deben ser separados por coma (,) o punto y coma(;).</span>
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
                            <td colspan="2">
                                <asp:TextBox ID="txtMenssage" TextMode="MultiLine" runat="server" Width="100%" Height="200px" CssClass="box"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="height: 15px">
                                <asp:Label ID="lblMensajeEmail" runat="server" Style="color: #FF0000" Font-Bold="true" CssClass="item"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnSendEmail" runat="server" CssClass="boton" Width="100px" Height="30px" Text="Enviar" />
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
                        
        <telerik:RadWindow ID="WinCancel"  Skin="Bootstrap" VisibleTitlebar="true"   runat="server" Modal="true"  AutoSize="false" Behaviors="Close" VisibleOnPageLoad="False" Width="400" Height="200" Localization-Cancel="Cancelar" RenderMode="Lightweight" >
            <ContentTemplate>
            <div style="width:90%; padding:12px;">
            
            <asp:Label ID="lblMotivoCancela" runat="server">Motivo de cancelación:</asp:Label><br /><br />
                <asp:DropDownList ID="cmbMotivoCancela" runat="server" AutoPostBack="true">                    
                </asp:DropDownList>
                <br />
                <asp:Panel runat="server" ID="panelFolioSustituye" Visible="false">
                <br /><br />
                <asp:Label ID="Label3" runat="server">Folio que sustituye:</asp:Label><br /><br />
                <asp:TextBox ID="txtFolioSustituye" runat="server"></asp:TextBox>
                </asp:Panel>
                <div>
                </div>
                <div style="width: 100%;text-align: end;margin-top: 10px;">
                    <asp:HiddenField ID="CancelarId" runat="server" />
                    <asp:Button ID="btnCancelaFactura" runat ="server" Text="Cancelar CFDI" />
                </div>
                </div>
            </ContentTemplate>
        </telerik:RadWindow>
       
        </Windows>
    </telerik:RadWindowManager>
    <telerik:RadWindow ID="RadWindow2" runat="server" Modal="true" CenterIfModal="true" BorderStyle="None" BorderWidth="0px" AutoSize="False" Behaviors="Close" Skin="Bootstrap" VisibleOnPageLoad="False" Width="1024" Height="768">
    </telerik:RadWindow>

</asp:Content>
