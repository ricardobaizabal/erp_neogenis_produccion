Imports Newtonsoft.Json
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class ProductoStockL
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim productoid As Integer
        Dim fecha_act As String
        'Dim encabezado As String = "stockLocations:"

        If Request("productoid") <> Nothing Then
            productoid = Request("productoid")
        Else
            productoid = 0
        End If

        If Request("fecha") <> Nothing Then
            fecha_act = Request("fecha")
        Else
            fecha_act = 0
        End If

        Dim etiqueta As DataSet = GetEtiquetaStockLocationByProductoid(productoid, fecha_act)

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
        'Response.Write(encavezado)
        Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(parentRow))

    End Sub

    Public Function GetEtiquetaStockLocationByProductoid(ByVal productoid As Integer, fecha_act As String) As DataSet
        Dim productodata As New DataSet
        Dim obj As New DataControl
        productodata = obj.FillDataSet("EXEC pStockLocation @cmd=5, @productoid=" & productoid & ",@fecha_act=" & fecha_act)
        Return productodata
    End Function

End Class