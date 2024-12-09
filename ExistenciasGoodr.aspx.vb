Imports System.IO
Public Class ExistenciasGoodr
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds As New DataSet
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisProductos @cmd=12, @proyectoid=7")


        Dim csv As String = String.Empty

        If ds.Tables(0).Rows.Count > 0 Then
            For Each row As DataRow In ds.Tables(0).Rows
                csv += row("codigo").ToString().Replace(",", ";") & ","c & row("descripcion").ToString().Replace(",", ";") & ","c & row("disponibles").ToString().Replace(",", ";")
                csv += vbCr & vbLf
            Next
        End If

        'Exporting to CSV.
        Dim folderPath As String = "C:\CSV\"
        If Not Directory.Exists(folderPath) Then
            Directory.CreateDirectory(folderPath)
        End If
        File.WriteAllText(folderPath & "existencias.csv", csv)

        Response.Write(csv)
    End Sub

End Class