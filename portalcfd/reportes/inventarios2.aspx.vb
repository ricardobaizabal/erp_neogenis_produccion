﻿Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class inventarios2
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte de inventarios"
        If Not IsPostBack Then
            Dim ObjData As New DataControl
            ObjData.Catalogo(almacenid, "select id, nombre from tblAlmacen where id <> 4 union select 6, 'Consignación' order by id", 0, True)
            ObjData.Catalogo(proyectoid, "select id, nombre from tblProyecto order by id", 0, True)
            ObjData = Nothing

            ds = GetProducts()
            reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
            reporteGrid.DataSource = ds
            reporteGrid.DataBind()
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        ds = GetProducts()
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
    End Sub

    Private Sub reporteGrid_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles reporteGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(ds.Tables(0).Compute("sum(existencia)", "")) Then
                        e.Item.Cells(6).Text = ds.Tables(0).Compute("sum(existencia)", "").ToString()
                        e.Item.Cells(6).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(6).Font.Bold = True
                    End If
                    If Not IsDBNull(ds.Tables(0).Compute("sum(importe)", "")) Then
                        e.Item.Cells(9).Text = FormatCurrency(ds.Tables(0).Compute("sum(importe)", ""), 2).ToString
                        e.Item.Cells(9).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(9).Font.Bold = True
                    End If
                    If Not IsDBNull(ds.Tables(0).Compute("sum(importe_costo_promedio)", "")) Then
                        e.Item.Cells(11).Text = FormatCurrency(ds.Tables(0).Compute("sum(importe_costo_promedio)", ""), 2).ToString
                        e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(11).Font.Bold = True
                    End If
                End If
        End Select
    End Sub

    Private Sub reporteGrid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles reporteGrid.NeedDataSource
        ds = GetProducts()
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds
    End Sub

    Function GetProducts() As DataSet
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMisInformes @cmd=27, @almacenid='" & almacenid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue.ToString & "'", conn)
        Dim ds As DataSet = New DataSet
        Try
            conn.Open()
            cmd.Fill(ds)
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return ds
    End Function

    Private Sub almacenid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles almacenid.SelectedIndexChanged
        ds = GetProducts()
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
    End Sub

End Class