
Partial Class portalcfd_Reportes
    Inherits System.Web.UI.Page

#Region "Load Initial Values"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend
        '
        If System.Configuration.ConfigurationManager.AppSettings("retencion4") = 1 Then
            lnkReport2.NavigateUrl = "~/portalcfd/reportes/ingresosRet.aspx"
            lnkReport3.NavigateUrl = "~/portalcfd/reportes/cobranzaRet.aspx"
            lnkReport4.NavigateUrl = "~/portalcfd/reportes/historicoRet.aspx"
            lnkReport5.NavigateUrl = "~/portalcfd/reportes/carteraRet.aspx"
        End If
        '
        If System.Configuration.ConfigurationManager.AppSettings("divisas") = 1 Then
            lnkReport2.NavigateUrl = "~/portalcfd/reportes/ingresosDiv.aspx"
            lnkReport3.NavigateUrl = "~/portalcfd/reportes/cobranzaDiv.aspx"
            lnkReport4.NavigateUrl = "~/portalcfd/reportes/historicoDiv.aspx"
            lnkReport5.NavigateUrl = "~/portalcfd/reportes/carteraGraf.aspx"
        End If
        '
        userpanel.Visible = False
        '
        If System.Configuration.ConfigurationManager.AppSettings("inventarios") = 1 Then
            almacenpanel.Visible = True
        End If
        '
        If Session("perfilid") = 3 Or Session("perfilid") = 5 Then
            lnkReport1.Enabled = False
            lnkReport4.Enabled = False
            lnkReport7.Enabled = False
            lnkReport8.Enabled = False
            lnkReport9.Enabled = False
        End If
    End Sub

#End Region

End Class
