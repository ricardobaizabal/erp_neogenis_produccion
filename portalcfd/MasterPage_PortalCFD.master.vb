
Partial Class portalcfd_MasterPage_PortalCFD
    Inherits System.Web.UI.MasterPage

#Region "Redirect User To The Default Page If The Session Is Over"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Session("razonsocial") Is Nothing Then
            Session.Abandon()
            Response.Redirect("~/Default.aspx")
        End If

    End Sub

#End Region

End Class

