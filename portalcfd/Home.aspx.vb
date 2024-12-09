
Partial Class portalcfd_Home
    Inherits System.Web.UI.Page

#Region "Load Initial Values"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        '
        '   Define permisos
        '
        Select Case Session("perfilid")
            Case 3
                lnk7.Enabled = False
                lnk7.ToolTip = "Acceso restringido"
                lnk2.Enabled = False
                lnk2.ToolTip = "Acceso restringido"
                lnk8.Enabled = False
                lnk8.ToolTip = "Acceso restringido"
                lnk5.Enabled = False
                lnk5.ToolTip = "Acceso restringido"
        End Select
        '
    End Sub

#End Region

End Class
