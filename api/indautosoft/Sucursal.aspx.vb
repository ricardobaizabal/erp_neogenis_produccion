Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters
Imports Newtonsoft.Json.Linq
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class Sucursal
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sucursalid As Integer
        Dim fecha_act As String

        If Request("sucursalid") <> Nothing Then
            sucursalid = Request("sucursalid")
        Else
            sucursalid = 0
        End If

        If Request("fecha") <> Nothing Then
            fecha_act = Request("fecha")
        Else
            fecha_act = 0
        End If

        Dim etiqueta As DataSet = GetSucursal(sucursalid, fecha_act)

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

    Public Function GetSucursal(ByVal sucursalid As Integer, fecha_act As String) As DataSet
        Dim sucursaldata As New DataSet
        Dim obj As New DataControl
        sucursaldata = obj.FillDataSet("EXEC pStockLocation @cmd=8, @sucursalid=" & sucursalid & ", @fecha_act=" & fecha_act)
        Return sucursaldata
    End Function
End Class