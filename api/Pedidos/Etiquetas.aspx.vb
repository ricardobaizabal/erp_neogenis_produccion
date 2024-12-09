Imports Newtonsoft.Json
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class Etiquetas1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim pedidoid As Integer = Request("pedidoid")
        If pedidoid <> Nothing Then
            Dim etiquetes As DataSet = GetEtiquetaDeCajasByPedidoid(pedidoid)

            Dim parentRow As List(Of Dictionary(Of String, Object)) = New List(Of Dictionary(Of String, Object))()

            Dim childRow As Dictionary(Of String, Object) = New Dictionary(Of String, Object)()
            For Each dr As DataRow In etiquetes.Tables(0).Rows
                childRow = New Dictionary(Of String, Object)()
                For Each dc As DataColumn In etiquetes.Tables(0).Columns
                    childRow.Add(dc.ColumnName, dr(dc))
                Next
                parentRow.Add(childRow)
            Next
            Response.ClearHeaders()
            Response.ContentType = "text/plain"
            Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(parentRow))
        End If
    End Sub

    Public Function GetEtiquetaDeCajasByPedidoid(ByVal pedidoid As Integer) As DataSet
        Dim pedidodata As New DataSet
        Dim obj As New DataControl
        pedidodata = obj.FillDataSet("EXEC pPedidos @cmd=30, @pedidoid=" & pedidoid)
        Return pedidodata
    End Function

End Class