
Partial Class portalcfd_UserControls_PortalCFD_SystemHeader_PortalCFD
    Inherits System.Web.UI.UserControl

#Region "Show Date, User Name & User IP On The Master Page Header"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'lblSocialReason.Text = Session("razonsocial")
        'lblContact.Text = Session("contacto")
        'lblDate.Text = Now.ToString("D")
        'lblHost.Text = Request.ServerVariables("REMOTE_ADDR").ToString
        imgLogo.ImageUrl = "~/portalcfd/logos/" & Session("logo")
    End Sub

#End Region

End Class
