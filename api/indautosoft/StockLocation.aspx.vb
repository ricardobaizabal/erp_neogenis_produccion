Imports Newtonsoft.Json
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class Etiquetas5
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim etiqueta As DataSet = GetEtiquetaStockLocationByProductoid()

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

    Public Function GetEtiquetaStockLocationByProductoid() As DataSet
        Dim productodata As New DataSet
        Dim obj As New DataControl
        productodata = obj.FillDataSet("EXEC pStockLocation @cmd=4")
        Return productodata
    End Function

End Class