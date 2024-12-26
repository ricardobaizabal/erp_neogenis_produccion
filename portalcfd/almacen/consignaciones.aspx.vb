Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Reporting.Processing

Public Class consignaciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaListado()
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(estatusid, "select id, nombre from tblConsignacionEstatus order by nombre", 0)
            ObjCat.Catalogo(proyectoid, "select id, nombre from tblProyecto order by nombre", 0)
            ObjCat = Nothing
        End If
    End Sub

    Private Sub CargaListado()
        Dim ObjData As New DataControl
        consignacionList.DataSource = ObjData.FillDataSet("exec pConsignaciones @cmd=5, @estatusid='" & estatusid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue & "'")
        consignacionList.MasterTableView.NoDetailRecordsText = "No se encontraron registros para mostrar."
        consignacionList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub consignacionList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles consignacionList.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                Response.Redirect("~/portalcfd/almacen/editaconsignacion.aspx?id=" & e.CommandArgument.ToString)
            Case "cmdDelete"
                Call BorraConsignacion(e.CommandArgument)
                Call CargaListado()
            Case "cmdPDF"
                'Call DownloadPDF(e.CommandArgument)
        End Select
    End Sub

    Private Sub consignacionList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles consignacionList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim lnkPDF As LinkButton = CType(e.Item.FindControl("lnkPDF"), LinkButton)
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un lote de consignación. ¿Desea continuar?')")

                If e.Item.DataItem("estatusid") = 2 Then
                    btnDelete.Enabled = False
                    btnDelete.ToolTip = "No se puede borrar una transferencia procesada."
                ElseIf e.Item.DataItem("estatusid") = 3 Then
                    btnDelete.Enabled = False
                    btnDelete.ToolTip = "No se puede borrar una transferencia cerrada."
                End If
                
        End Select
    End Sub

    Private Sub BorraConsignacion(ByVal consignacionid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pConsignaciones @cmd=7, @consignacionid='" & consignacionid.ToString & "'")
        ObjData = Nothing
    End Sub

    'Private Sub DownloadPDF(ByVal transferenciaid As Long)
    '    '
    '    Dim FilePath = Server.MapPath("~/portalcfd/pdf/") & "ng_transferencia_" & transferenciaid.ToString & ".pdf"
    '    If File.Exists(FilePath) Then
    '        Dim FileName As String = Path.GetFileName(FilePath)
    '        Response.Clear()
    '        Response.ContentType = "application/octet-stream"
    '        Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
    '        Response.Flush()
    '        Response.WriteFile(FilePath)
    '        Response.End()
    '    Else
    '        GuardaPDF(GeneraPDF_Documento(transferenciaid), FilePath)

    '        Dim FileName As String = Path.GetFileName(FilePath)
    '        Response.Clear()
    '        Response.ContentType = "application/octet-stream"
    '        Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
    '        Response.Flush()
    '        Response.WriteFile(FilePath)
    '        Response.End()
    '    End If
    '    '
    'End Sub

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Call CargaListado()
    End Sub

    Private Sub consignacionList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles consignacionList.NeedDataSource
        Dim ObjData As New DataControl
        consignacionList.DataSource = ObjData.FillDataSet("exec pConsignaciones @cmd=5, @estatusid='" & estatusid.SelectedValue.ToString & "'")
        consignacionList.MasterTableView.NoDetailRecordsText = "No se encontraron registros para mostrar."
        ObjData = Nothing
    End Sub

End Class
