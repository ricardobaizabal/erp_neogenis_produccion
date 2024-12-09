Public Class agregarlote
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        If Not IsPostBack Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(origenid, "select id, nombre from tblAlmacen order by nombre", 0)
            ObjCat.Catalogo(destinoid, "select id, nombre from tblAlmacen order by nombre", 0)
            ObjCat = Nothing
        End If
    End Sub

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Dim TransferenciaId As Long = 0
        Dim ObjData As New DataControl
        TransferenciaId = ObjData.RunSQLScalarQuery("exec pTransferencia @cmd=1, @userid='" & Session("userid").ToString & "', @origenid='" & origenid.SelectedValue.ToString & "', @destinoid='" & destinoid.SelectedValue.ToString & "', @comentario='" & txtComentario.Text & "'")
        ObjData = Nothing
        Response.Redirect("~/portalcfd/almacen/editalote.aspx?id=" & TransferenciaId.ToString)
    End Sub

End Class