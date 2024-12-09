Imports Newtonsoft.Json
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class OrdenCompra2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ordencompraid As Integer

        If Request("ordencompraid") <> Nothing Then
            ordencompraid = Request("ordencompraid")
        Else
            ordencompraid = 0
        End If

        Dim etiqueta As DataSet = GetEtiquetaByOrdencompraid2(ordencompraid)

        Dim parentRow As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))()

        Dim childRow As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
        For Each dr As DataRow In etiqueta.Tables(0).Rows
            childRow = New Dictionary(Of String, Object)()
            For Each dc As DataColumn In etiqueta.Tables(0).Columns
                childRow.Add(dc.ColumnName, dr(dc))
            Next
            parentRow.Add(childRow)
        Next
        Response.ClearHeaders()
        Response.ContentType = "text/plain"
        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(parentRow))

    End Sub

    Public Function GetEtiquetaByOrdencompraid2(ByVal ordencompraid As Integer) As DataSet
        Dim ordencompradata As New DataSet
        Dim obj As New DataControl
        ordencompradata = obj.FillDataSet("EXEC pStockLocation @cmd=6, @ordencompraid=" & ordencompraid)
        Return ordencompradata
    End Function
End Class