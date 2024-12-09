Imports System.Data
Imports System.Data.SqlClient

Public Class embarcado
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        If Not IsPostBack Then
            Call MuestraLista()
        End If
    End Sub

    Private Sub MuestraLista()

        Dim ObjData As New DataControl
        cfdList.DataSource = ObjData.FillDataSet("exec pEmbarques @cmd=1")
        cfdList.DataBind()
        ObjData = Nothing

    End Sub

    Private Sub cfdList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles cfdList.ItemCommand
        Select Case e.CommandName
            Case "cmdView"
                Response.Redirect("~/portalcfd/almacen/edita_embarcado.aspx?id=" & e.CommandArgument.ToString)
        End Select
    End Sub

End Class