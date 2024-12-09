<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_proveedores_proveedores" Codebehind="proveedores.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style4
        {
            height: 17px;
        }
        .style5
        {
            height: 14px;
        }
        .style6
        {
            height: 21px;
        }
    </style>
    <script type="text/javascript">
        function OnRequestStart(target, arguments) {
            if (arguments.get_eventTarget().indexOf("providerslist") > -1) {
                arguments.set_enableAjax(false);
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
        
        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblClientsListLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td style="height: 5px">
                        <telerik:RadGrid ID="providerslist" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" GridLines="None" 
                            OnNeedDataSource="providerslist_NeedDataSource" PageSize="15" ShowStatusBar="True" 
                            Skin="Simple" Width="100%">
                            <ExportSettings HideStructureColumns="true" IgnorePaging="True" FileName="CatalogoProveedores">
                                <Excel Format="Biff" />
                            </ExportSettings>
                            <PagerStyle Mode="NumericPages" />
                            <MasterTableView AllowMultiColumnSorting="False" DataKeyNames="id" Name="Providers" Width="100%" CommandItemDisplay="Top">
                                <CommandItemSettings ShowRefreshButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ExportToPdfText="Exportar a pdf" ExportToExcelText="Exportar a excel"></CommandItemSettings>
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="No. Proveedor" UniqueName="id"></telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="true" HeaderText="" UniqueName="razonsocial">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdEdit" Text='<%# Eval("razonsocial") %>' CausesValidation="false"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridBoundColumn DataField="contacto" HeaderText="" UniqueName="contacto">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="telefono_contacto" HeaderText="" UniqueName="telefono_contacto">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="condiciones" HeaderText="Condiciones" UniqueName="condiciones">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="rfc" HeaderText="" UniqueName="rfc">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnDelete" runat="server" 
                                                CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" 
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
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td align="right" style="height: 5px">
                        <asp:Button ID="btnAddProvider" runat="server" CausesValidation="False" 
                            CssClass="item" TabIndex="6" />
                    </td>
                </tr>
                <tr>
                    <td style="height: 2px">
                    </td>
                </tr>
            </table>
        </fieldset>
        
        <br />
        
        <asp:Panel ID="panelClientRegistration" runat="server" Visible="False">

        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblClientEditLegend" runat="server" Font-Bold="True" 
                    CssClass="item"></asp:Label>
            </legend>

            <br />

            <table width="100%">
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblSocialReason" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%" valign="top" colspan="2" style="width: 66%">
                        <telerik:RadTextBox ID="txtSocialReason" Runat="server" Width="92%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ForeColor="Red"
                            ControlToValidate="txtSocialReason" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style4" width="33%">
                        <asp:Label ID="lblContact" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="33%">
                        <asp:Label ID="lblContactEmail" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td class="style4" width="33%">
                        <asp:Label ID="lblContactPhone" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="33%">
                        <telerik:RadTextBox ID="txtContact" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td class="style4" width="33%">
                        <telerik:RadTextBox ID="txtContactEmail" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td class="style4" width="33%">
                        <telerik:RadTextBox ID="txtContactPhone" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style4" width="33%">
                        &nbsp;</td>
                    <td class="style4" width="33%">
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ForeColor="Red"
                            ControlToValidate="txtContactEmail" CssClass="item" 
                            ValidationExpression=".*@.*\..*"></asp:RegularExpressionValidator>
                    </td>
                    <td class="style4" width="33%">
                    </td>
                </tr>
                <tr>
                    <td width="33%" class="style5">
                        </td>
                    <td width="33%" class="style5">
                        </td>
                    <td width="33%" class="style5">
                        </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblStreet" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%" align="left">
                        <asp:Label ID="lblExtNumber" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Label ID="lblIntNumber" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td align="left" width="33%">
                        <asp:Label ID="lblColony" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <telerik:RadTextBox ID="txtStreet" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td width="33%">
                        <telerik:RadTextBox ID="txtExtNumber" Runat="server" Width="35%">
                        </telerik:RadTextBox>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <telerik:RadTextBox ID="txtIntNumber" Runat="server" Width="35%">
                        </telerik:RadTextBox>
                    </td>
                    <td align="left" width="33%">
                        <telerik:RadTextBox ID="txtColony" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ForeColor="Red"
                            ControlToValidate="txtStreet" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ForeColor="Red"
                            ControlToValidate="txtExtNumber" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td align="left" width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ForeColor="Red"
                            ControlToValidate="txtColony" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="33%" class="style6">
                        </td>
                    <td width="33%" class="style6">
                        </td>
                    <td width="33%" class="style6">
                        </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblCountry" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblState" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblTownship" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <telerik:RadTextBox ID="txtCountry" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td width="33%">
                        <telerik:RadComboBox ID="cmbStates" Runat="server" AllowCustomText="True" 
                            CausesValidation="true" Width="87%">
                        </telerik:RadComboBox>
                    </td>
                    <td width="33%">
                        <telerik:RadTextBox ID="txtTownship" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ForeColor="Red"
                            ControlToValidate="txtCountry" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ForeColor="Red"
                            ControlToValidate="cmbStates" CssClass="item" InitialValue="-- Seleccione --"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ForeColor="Red"
                            ControlToValidate="txtTownship" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:Label ID="lblZipCode" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblRFC" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    <td width="33%">
                        <asp:Label ID="lblCondiciones" runat="server" CssClass="item" Font-Bold="true" Text="Condiciones:"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <telerik:RadTextBox ID="txtZipCode" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    <td width="33%">
                        <telerik:RadTextBox ID="txtRFC" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                    
                    <td>
                        <asp:DropDownList ID="condicionesid" runat="server" CssClass="box"></asp:DropDownList>
                    </td>
                    
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ForeColor="Red"
                            ControlToValidate="txtZipCode" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ForeColor="Red"
                            ControlToValidate="txtRFC" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%" class="style6">
                        </td>
                    <td width="33%" class="style6">
                        </td>
                    <td width="33%" class="style6">
                        </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td valign="bottom" colspan="3">
                        <asp:Button ID="btnSaveClient" runat="server" CssClass="item" />&nbsp;
                        <asp:Button ID="btnCancel" runat="server" CssClass="item" 
                            CausesValidation="False" />
                        
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 66%; height: 5px;">
                        <asp:HiddenField ID="InsertOrUpdate" runat="server" Value="0" />
                        <asp:HiddenField ID="ClientsID" runat="server" Value="0" />
                    </td>
                </tr>
            </table>

        </fieldset>

    </asp:Panel>
    
    </telerik:RadAjaxPanel>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>

