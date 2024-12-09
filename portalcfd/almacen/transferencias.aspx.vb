Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Reporting.Processing
Public Class transferencias
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            '
            Call CargaListado()
            '
        End If
    End Sub

    Private Sub CargaListado()
        Dim ObjData As New DataControl
        loteslist.DataSource = ObjData.FillDataSet("exec pTransferencia @cmd=5")
        loteslist.DataBind()
        ObjData = Nothing
    End Sub

    Protected Sub loteslist_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles loteslist.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                Response.Redirect("~/portalcfd/almacen/editalote.aspx?id=" & e.CommandArgument.ToString)
            Case "cmdDelete"
                Call BorraTransferencia(e.CommandArgument)
                Call CargaListado()
            Case "cmdPDF"
                Call DownloadPDF(e.CommandArgument)
        End Select
    End Sub

    Protected Sub loteslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles loteslist.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar una transferencia. ¿Desea continuar?')")

                If e.Item.DataItem("estatusid") = 2 Then
                    btnDelete.Enabled = False
                    btnDelete.ToolTip = "No se puede borrar una transferencia procesada."
                End If
        End Select
    End Sub

    Private Sub BorraTransferencia(ByVal transferenciaid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pTransferencia @cmd=7, @transferenciaid='" & transferenciaid.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub DownloadPDF(ByVal transferenciaid As Long)
        '
        Dim FilePath = Server.MapPath("~/portalcfd/pdf/") & "ng_transferencia_" & transferenciaid.ToString & ".pdf"
        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        Else
            GuardaPDF(GeneraPDF_Documento(transferenciaid), FilePath)

            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        End If
        '
    End Sub

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub

    Private Function GeneraPDF_Documento(ByVal transferenciaid As Long) As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim folio As Long = 0
        Dim fecha As String = ""
        Dim origen As String = ""
        Dim destino As String = ""
        Dim comentarios As String = ""
        Dim elaboro As String = ""
        Dim ds As DataSet = New DataSet

        Dim cmd As New SqlCommand("EXEC pTransferencia @cmd=2, @transferenciaid='" & transferenciaid.ToString & "'", conn)
        conn.Open()
        Dim rs As SqlDataReader
        rs = cmd.ExecuteReader()

        If rs.Read Then
            folio = rs("id")
            fecha = rs("fecha")
            origen = rs("origen")
            destino = rs("destino")
            comentarios = rs("comentario")
            elaboro = rs("usuario")
            
        End If
        rs.Close()

        conn.Close()
        conn.Dispose()
        conn = Nothing


        Dim reporte As New Formatos.formato_embarque_neogenis


        reporte.ReportParameters("plantillaId").Value = 5
        reporte.ReportParameters("transferenciaId").Value = transferenciaid

        reporte.ReportParameters("txtFolio").Value = folio
        reporte.ReportParameters("txtFecha").Value = fecha
        reporte.ReportParameters("txtOrigen").Value = origen
        reporte.ReportParameters("txtDestino").Value = destino
        reporte.ReportParameters("txtComentarios").Value = comentarios
        reporte.ReportParameters("txtElaboro").Value = elaboro
        reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))

        Dim totalPzas As String
        Dim objData As New DataControl
        totalPzas = objData.RunSQLScalarQuery("exec pTransferencia @cmd=11, @transferenciaid='" & transferenciaid.ToString & "'")
        objData = Nothing

        reporte.ReportParameters("txtTotalPiezas").Value = totalPzas.ToString

        '
        Return reporte

    End Function

End Class