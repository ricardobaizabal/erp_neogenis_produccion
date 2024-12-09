Imports Newtonsoft.Json
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class Etiquetas2
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim proveedorid As Integer
        Dim fecha_act As String

        If Request("proveedorid") <> Nothing Then
            proveedorid = Request("proveedorid")
        Else
            proveedorid = 0
        End If

        If Request("fecha") <> Nothing Then
            fecha_act = Request("fecha")
        Else
            fecha_act = 0
        End If

        Dim etiqueta As DataSet = GetEtiquetaByProveedorid(proveedorid, fecha_act)

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

    Public Function GetEtiquetaByProveedorid(ByVal proveedorid As Integer, fecha_act As String) As DataSet
        Dim proveedordata As New DataSet
        Dim obj As New DataControl
        proveedordata = obj.FillDataSet("EXEC pStockLocation @cmd=1, @proveedorid=" & proveedorid & ",@fecha_act=" & fecha_act)
        Return proveedordata
    End Function

End Class