
Partial Class portalcfd_Salir
    Inherits System.Web.UI.Page

#Region "Abandon CFD Portal"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Session.Abandon()
        Response.Redirect("~/Default.aspx")

    End Sub

#End Region

End Class
