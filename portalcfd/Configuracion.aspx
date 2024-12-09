<%@ Page Title="" Language="VB" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master"
    AutoEventWireup="false" Inherits="erp_neogenis.portalcfd_Configuracion" Codebehind="Configuracion.aspx.vb" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

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
        
    </style>
    
    <script type="text/javascript">

        //On Save Button Click Temporarily Disables Ajax To Perform Upload Actions
    
        function conditionalPostback(sender, args) 
        {
            if (args.EventTarget == "<%= btnSavePrivateKey.UniqueID %>") 
            {
                args.EnableAjax = false;
            }

            if (args.EventTarget == "<%= btnSaveCertificate.UniqueID %>") 
            {
                args.EnableAjax = false;
            }

            if (args.EventTarget == "<%= fileIcon.UniqueID %>") 
            {
                args.EnableAjax = false;
            }

            if (args.EventTarget == "<%= lnkDownloadPrivateKey.UniqueID %>") 
            {
                args.EnableAjax = false;
            }
        }

        //Validate The File Extensions On The Images RadUpload Control & That The Control Always Request For A File

        function validateRadUpload1(source, arguments) 
        {
            arguments.IsValid = getRadUpload('<%= RadUpload1.ClientID %>').validateExtensions();
        }
        
        function validateRadUpload2(source, e)
        {
           e.IsValid = false;

           var upload = $find("<%= RadUpload1.ClientID %>");
           var key = "<%= lnkDownloadPrivateKey.Text %>";

           var inputs = upload.getFileInputs();
           for (var i = 0; i < inputs.length; i++)
           {
               //Check For Empty String Or Invalid Extension
               if (inputs[i].value != "" || key != "") 
               {
                   e.IsValid = true;
                   break;
               }    
           }
       }

       //Validate The File Extensions On The Images RadUpload Control & That The Control Always Request For A File

       function validateRadUpload3(source, arguments) 
       {
           arguments.IsValid = getRadUpload('<%= RadUpload2.ClientID %>').validateExtensions();
       }

       function validateRadUpload4(source, e) 
       {
           e.IsValid = false;

           var upload = $find("<%= RadUpload2.ClientID %>");
           var inputs = upload.getFileInputs();
           for (var i = 0; i < inputs.length; i++) {
               //Check For Empty String Or Invalid Extension
               if (inputs[i].value != "") {
                   e.IsValid = true;
                   break;
               }
           }
       } 

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <br />

    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="conditionalPostback">

        <asp:Panel ID="panelPrivateKey" runat="server">
            <fieldset>
                <legend style="padding-right: 6px; color: Black">
                    <asp:Label ID="lblPrivateKeyLegend" runat="server" Font-Bold="true" CssClass="titulos"></asp:Label>
                </legend>
                <br />

                <table>
                    <tr>
                        <td width="25%">
                            <asp:Label ID="lblPrivateKey" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%" colspan="2" style="width: 50%">
                            <asp:Label ID="lblPrivateKeyDownload" runat="server" CssClass="item" Font-Bold="True" 
                                Visible="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="width: 50%" width="25%">
                            <telerik:RadUpload ID="RadUpload1" runat="server" 
                                AllowedFileExtensions=".key,.KEY"
                                ControlObjectsVisibility="None" InputSize="57" MaxFileInputsCount="1" 
                                MaxFileSize="10485760">
                                <Localization Add="Agregar" Remove="Eliminar" Select="Buscar" />
                            </telerik:RadUpload>
                        </td>
                        <td width="25%" align="left" colspan="2" style="width: 50%">
                            <asp:ImageButton ID="fileIcon" runat="server" 
                                ImageUrl="~/images/icons/download_icon.gif" Visible="False" 
                                style="height: 15px" />
                            &nbsp;
                            <asp:LinkButton ID="lnkDownloadPrivateKey" runat="server" 
                                CausesValidation="False" CssClass="descargable" Visible="False"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%" colspan="2" style="width: 50%">
                            <asp:CustomValidator ID="CustomValidator" runat="server" 
                                ClientValidationFunction="validateRadUpload2" CssClass="item" 
                                Display="Dynamic" ValidationGroup="PrivateKey"></asp:CustomValidator>
                            &nbsp;<asp:CustomValidator ID="ValidateExtensions" runat="server" 
                                ClientValidationFunction="validateRadUpload1" CssClass="item" 
                                Display="Dynamic" ValidationGroup="PrivateKey"></asp:CustomValidator>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <asp:Label ID="lblPasword" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <telerik:RadTextBox ID="txtPassword" Runat="server" Width="50%" ValidationGroup="PrivateKey">
                            </telerik:RadTextBox>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                                ControlToValidate="txtPassword" CssClass="item" 
                                ValidationGroup="PrivateKey"></asp:RequiredFieldValidator>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%" align="right">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%" align="right">
                            <asp:Button ID="btnSavePrivateKey" runat="server" CssClass="item" 
                                ValidationGroup="PrivateKey" />
                        </td>
                    </tr>
                </table>

                <asp:HiddenField ID="fileName" runat="server" />
                <br />
            </fieldset>
        </asp:Panel>

        <br />

        <asp:Panel ID="panelCertificate" runat="server">
            <fieldset>
                <legend style="padding-right: 6px; color: Black">
                    <asp:Label ID="lblCertificateLegend" runat="server" Font-Bold="true" CssClass="titulos"></asp:Label>
                </legend>
                <br />

                <table>
                    <tr>
                        <td width="25%">
                            <asp:Label ID="lblCertificate" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            <asp:Label ID="lblActivate" runat="server" CssClass="item" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%" colspan="2" style="width: 50%">
                            <telerik:RadUpload ID="RadUpload2" runat="server" 
                                AllowedFileExtensions=".cer,.CER" 
                                ControlObjectsVisibility="None" InputSize="57" MaxFileInputsCount="1" 
                                MaxFileSize="10485760">
                                <Localization Add="Agregar" Remove="Eliminar" Select="Buscar" />
                            </telerik:RadUpload>
                        </td>
                        <td width="25%" valign="top">
                            <asp:CheckBox ID="chckActivate" runat="server" CssClass="item" />
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%" colspan="2" style="width: 50%">
                            <asp:CustomValidator ID="CustomValidator2" runat="server" 
                                ClientValidationFunction="validateRadUpload4" CssClass="item" 
                                Display="Dynamic" ValidationGroup="Certificate"></asp:CustomValidator>
                            <asp:CustomValidator ID="ValidateExtensions2" runat="server" 
                                ClientValidationFunction="validateRadUpload3" CssClass="item" 
                                Display="Dynamic" ValidationGroup="Certificate"></asp:CustomValidator>
                        </td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td align="right" width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <fieldset>
                                <legend style="padding-right: 6px; color: Black">
                                    <asp:Label ID="lblCertificatesListLegend" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                                </legend>
                                <br />
                                <telerik:RadGrid ID="certificatesList" runat="server" Width="100%" ShowStatusBar="True"
                                    AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                                    Skin="Simple" Visible="True">
                                    <MasterTableView Width="100%" DataKeyNames="id" Name="Certificates" AllowMultiColumnSorting="False">
                                        <Columns>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"
                                                UniqueName="download">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnDownload" runat="server" CommandArgument='<%# Eval("id") %>'
                                                        CommandName="cmdDownloadCertificate" ImageUrl="~/images/icons/download_icon.gif" CausesValidation="False" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                            </telerik:GridTemplateColumn>
                                            
                                            <telerik:GridBoundColumn DataField="certificado" HeaderText="" UniqueName="certificado">
                                            </telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="" UniqueName="activo"
                                                ItemStyle-HorizontalAlign="Center" ItemStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:Image ID="imgActive" runat="server" border="0" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"
                                                UniqueName="delete">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>'
                                                        CommandName="cmdDeleteCertificate" ImageUrl="~/images/action_delete.gif" CausesValidation="False" />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <ItemStyle HorizontalAlign="Center" Width="15%" />
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                                <br />
                            </fieldset>
                        </td>
                    </tr>
                    <tr>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td align="right" width="25%">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td width="25%">
                            &nbsp;</td>
                        <td align="right" width="25%">
                            <asp:Button ID="btnSaveCertificate" runat="server" CssClass="item" 
                                ValidationGroup="Certificate" />
                        </td>
                    </tr>
                </table>

                <br />
            </fieldset>
        </asp:Panel>
    </telerik:RadAjaxPanel>
    
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="WebBlue"
        Width="100%">
    </telerik:RadAjaxLoadingPanel>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="certificatesList" />
        </Triggers>
    </asp:UpdatePanel>

</asp:Content>
