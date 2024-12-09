Public Class perfiles
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaPerfiles()
        End If
    End Sub

    Private Sub CargaPerfiles()
        Dim ObjData As New DataControl
        perfilesList.DataSource = ObjData.FillDataSet("exec pPerfiles @cmd=1")
        perfilesList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub perfilesList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles perfilesList.NeedDataSource
        Dim ObjData As New DataControl
        perfilesList.DataSource = ObjData.FillDataSet("exec pPerfiles @cmd=1")
        ObjData = Nothing
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        '
    End Sub
End Class