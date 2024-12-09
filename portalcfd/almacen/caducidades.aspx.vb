Public Class caducidades
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call MuestraInventario()
        End If
    End Sub

    Private Sub MuestraInventario()
        Dim ObjData As New DataControl
        conceptosList.DataSource = ObjData.FillDataSet("exec pControlInventario @cmd=4")
        conceptosList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub conceptosList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles conceptosList.NeedDataSource
        Dim ObjData As New DataControl
        conceptosList.DataSource = ObjData.FillDataSet("exec pControlInventario @cmd=4")
        ObjData = Nothing
    End Sub
End Class