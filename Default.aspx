<%@ Page Language="VB" AutoEventWireup="false" CodeBehind="Default.aspx.vb" Inherits="erp_neogenis._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
    <link href="style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <script type="text/javascript">
            //Put your JavaScript code here.
        </script>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1">
            <div style="width: 100%;" align="center">
                <br />
                <br />
                <br />
                <br />
                <br />
                <div align="center" id="login">
                    <table border="0" cellpadding="10" cellspacing="0" align="center" style="width:300px; height:300px; padding-top:180px;">
                        <tr>
                            <td align="center">
                                <table border="0" cellpadding="1" cellspacing="3" align="center">
                                    <tr>
                                        <td align="right">Email: </td>
                                        <td>
                                            <asp:TextBox ID="email" runat="server" Width="150px"></asp:TextBox>&nbsp;&nbsp;<asp:RequiredFieldValidator ID="valEmail" runat="server" ForeColor="Red" ControlToValidate="email" ErrorMessage="* Requerido" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">Contraseña: </td>
                                        <td>
                                            <asp:TextBox ID="contrasena" runat="server" Width="150px" TextMode="Password"></asp:TextBox>&nbsp;&nbsp;<asp:RequiredFieldValidator ID="valContrasena" runat="server" ForeColor="Red" ControlToValidate="contrasena" ErrorMessage="* Requerido" SetFocusOnError="True"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td align="left">
                                            <br />
                                            <asp:Button ID="btnLogin" runat="server" Text="Entrar" /><br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkRemember" runat="server" Text="Recordar mis datos" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <br />
                                            <asp:Label ID="lblMensaje" ForeColor="Red" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
        </telerik:RadAjaxLoadingPanel>
    </form>
</body>
</html>
