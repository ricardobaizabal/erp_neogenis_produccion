<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_Datos" Codebehind="Datos.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <style type="text/css">

        .titulos 
        {
            font-family:verdana;
            font-size:medium;
            color:Purple;
        }
        
        .descargable 
        {
	        font-size: 8pt;
	        text-decoration: none;            
            font-family:verdana;
            font-weight:bold;
            color:Purple;
        }
        
        .style2
        {
            height: 16px;
        }
        
        .style3
        {
            height: 10px;
        }
        
        </style>
    
    <script type="text/javascript">

        //On Save Button Click Temporarily Disables Ajax To Perform Upload Actions
    
        function conditionalPostback(sender, args) 
        {
            if (args.EventTarget == "<%= btnSaveData.UniqueID %>") 
            {
                args.EnableAjax = false;
            }
        }

        //Validate The File Extensions On The Images RadUpload Control & That The Control Always Request For A File

        function validateRadUpload1(source, arguments) 
        {
            arguments.IsValid = getRadUpload('<%= RadUpload1.ClientID %>').validateExtensions();
        }

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="conditionalPostback">

        <br />

        <asp:Panel ID="panelData" runat="server">

        <fieldset>
            <legend style="padding-right: 6px; color: Black">
                <asp:Label ID="lblDataLegend" runat="server" CssClass="item" 
                    Font-Bold="true"></asp:Label>
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
                        <asp:Label ID="lblEmailContact" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="33%" valign="top" colspan="2" style="width: 66%">
                        <telerik:RadTextBox ID="txtSocialReason" Runat="server" Width="92%">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmailContact" Runat="server" Width="85%">
                            <PasswordStrengthSettings CalculationWeightings="50;15;15;20" 
                                IndicatorElementBaseStyle="riStrengthBar" IndicatorElementID="" 
                                MinimumLowerCaseCharacters="2" MinimumNumericCharacters="2" 
                                MinimumSymbolCharacters="2" MinimumUpperCaseCharacters="2" 
                                OnClientPasswordStrengthCalculating="" PreferredPasswordLength="10" 
                                RequiresUpperAndLowerCaseCharacters="True" ShowIndicator="False" 
                                TextStrengthDescriptions="Very Weak;Weak;Medium;Strong;Very Strong" 
                                TextStrengthDescriptionStyles="riStrengthBarL0;riStrengthBarL1;riStrengthBarL2;riStrengthBarL3;riStrengthBarL4;riStrengthBarL5;" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtSocialReason" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        </td>
                    <td width="33%">
                        </td>
                </tr>
                <tr>
                    <td width="33%" class="style3">
                        </td>
                    <td width="33%" class="style3">
                        </td>
                    <td width="33%" class="style3">
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
                        <asp:Label ID="lblPassword" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
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
                        <telerik:RadTextBox ID="txtPassword" Runat="server" Width="85%">
                            <PasswordStrengthSettings CalculationWeightings="50;15;15;20" 
                                IndicatorElementBaseStyle="riStrengthBar" IndicatorElementID="" 
                                MinimumLowerCaseCharacters="2" MinimumNumericCharacters="2" 
                                MinimumSymbolCharacters="2" MinimumUpperCaseCharacters="2" 
                                OnClientPasswordStrengthCalculating="" PreferredPasswordLength="10" 
                                RequiresUpperAndLowerCaseCharacters="True" ShowIndicator="False" 
                                TextStrengthDescriptions="Very Weak;Weak;Medium;Strong;Very Strong" 
                                TextStrengthDescriptionStyles="riStrengthBarL0;riStrengthBarL1;riStrengthBarL2;riStrengthBarL3;riStrengthBarL4;riStrengthBarL5;" />
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtStreet" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtExtNumber" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td align="left" width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td width="33%" class="style2">
                        </td>
                    <td width="33%" class="style2">
                        </td>
                    <td width="33%" class="style2">
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
                        <asp:Label ID="lblColony" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
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
                        <telerik:RadTextBox ID="txtColony" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                            ControlToValidate="txtCountry" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                            ControlToValidate="cmbStates" CssClass="item" InitialValue="-- Seleccione --"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                            ControlToValidate="txtColony" CssClass="item"></asp:RequiredFieldValidator>
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
                        
                        <asp:Label ID="lblTownship" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        
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
                    <td width="33%" valign="bottom">
                        
                        <telerik:RadTextBox ID="txtTownship" Runat="server" Width="85%">
                        </telerik:RadTextBox>
                        
                        </td>
                </tr>
                <tr>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" 
                            ControlToValidate="txtZipCode" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" 
                            ControlToValidate="txtRFC" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" 
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
                    <td width="66%" colspan="2">
                        <asp:Label ID="lblLogo" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                    </td>
                    
                    <td width="33%">
                        <asp:Label ID="lblRegimen" runat="server" CssClass="item" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 66%">
                        <telerik:RadUpload ID="RadUpload1" runat="server" 
                            AllowedFileExtensions=".jpg,.JPG,.jpeg,.JPEG,.gif,.GIF,.png,.PNG" 
                            ControlObjectsVisibility="None" InputSize="57" MaxFileInputsCount="1" 
                            MaxFileSize="10485760">
                            <Localization Add="Agregar" Remove="Eliminar" Select="Buscar" />
                        </telerik:RadUpload>&nbsp;<asp:Label ID="lblLogoName" runat="server" CssClass="item"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="regimenid" CssClass="box" Width="85%" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td width="33%" colspan="2">&nbsp;</td>
                    <td width="33%">
                        <asp:RequiredFieldValidator ID="valRegimen" runat="server" InitialValue="0"
                            ControlToValidate="regimenid" CssClass="item"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td width="33%">
                        <asp:Label ID="Label1" runat="server" CssClass="item" Font-Bold="True" Text="Logo para formato:"></asp:Label>
                    </td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 66%">
                        <telerik:RadUpload ID="RadUpload2" runat="server" 
                            AllowedFileExtensions=".jpg,.JPG,.jpeg,.JPEG,.gif,.GIF,.png,.PNG" 
                            ControlObjectsVisibility="None" InputSize="57" MaxFileInputsCount="1" 
                            MaxFileSize="10485760">
                            <Localization Add="Agregar" Remove="Eliminar" Select="Buscar" />
                        </telerik:RadUpload>&nbsp;<asp:Label ID="lblLogoName2" runat="server" CssClass="item"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" class="item">
                        <strong>Plantilla de colores:</strong><br />
                        <asp:DropDownList ID="plantillaid" CssClass="box" runat="server"></asp:DropDownList>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3"><hr /></td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item" style="background-color:GrayText; color:white; width:400px; vertical-align:middle; height:30px; padding-left:10px;">
                        <strong>Expedido en:</strong>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Calle y Número:</strong><br />
                        <telerik:RadTextBox ID="txtExpedidoLinea1" Runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Colonia y C.P.:</strong><br />
                        <telerik:RadTextBox ID="txtExpedidoLinea2" Runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Ciudad y Estado:</strong><br />
                        <telerik:RadTextBox ID="txtExpedidoLinea3" Runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Porcentaje para pagaré:</strong><br />
                        <telerik:RadNumericTextBox ID="porcentaje" runat="server" NumberFormat-DecimalDigits="0" NumberFormat-GroupSizes="9" Type="Percent"></telerik:RadNumericTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3"><br /><hr /></td>
                </tr>
                
                
                <tr>
                    <td colspan="3" class="item" style="background-color:GrayText; color:white; width:400px; vertical-align:middle; height:30px; padding-left:10px;">
                        <strong>Configuración para envíos de correo:</strong>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Correo desde el cual saldrán los anexos:</strong><br />
                        <telerik:RadTextBox ID="email_from" Runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Servidor de correo saliente:</strong><br />
                        <telerik:RadTextBox ID="email_smtp_server" Runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Puerto de salida:</strong><br />
                        <telerik:RadTextBox ID="email_smtp_port" Runat="server" Width="50px" Text="25">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Smtp Username:</strong><br />
                        <telerik:RadTextBox ID="email_smtp_username" Runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3" class="item">
                        <br />
                        <strong>Smtp Password:</strong><br />
                        <telerik:RadTextBox ID="email_smtp_password" Runat="server" Width="400px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                
                <tr>
                    <td width="33%">
                        <asp:CustomValidator ID="ValidateExtensions" runat="server" 
                            ClientValidationFunction="validateRadUpload1" CssClass="item" Display="Dynamic"></asp:CustomValidator>
                    </td>
                    <td width="33%">
                        &nbsp;</td>
                    <td width="33%">
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnSaveData" runat="server" CssClass="item" />
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
            </table>

        </fieldset>

    </asp:Panel>

        <br />
        
    </telerik:RadAjaxPanel>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="WebBlue"
        Width="100%">
    </telerik:RadAjaxLoadingPanel>

</asp:Content>