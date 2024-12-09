Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Globalization

Public Class OrdenCompraReporte
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte Ordenes de Compra"
        If Not IsPostBack Then
            fechaini.SelectedDate = DateAdd(DateInterval.Day, -7, Now)
            fechafin.SelectedDate = Now
            Dim ObjData As New DataControl
            ObjData.Catalogo(marcaid, "select id, nombre from tblProyecto order by id", 0, True)
            ObjData.Catalogo(proveedorid, "select id, razonsocial from tblMisProveedores order by id", 0, True)


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
                    If Not IsDBNull(ds.Tables(0).Compute("sum(cantidadRecibida)", "")) Then
                        e.Item.Cells(5).Text = ds.Tables(0).Compute("sum(cantidadRecibida)", "").ToString()
                        e.Item.Cells(5).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(5).Font.Bold = True
                    End If
                    If Not IsDBNull(ds.Tables(0).Compute("sum(costo)", "")) Then
                        e.Item.Cells(7).Text = FormatCurrency(ds.Tables(0).Compute("sum(costo)", ""), 2).ToString
                        e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(7).Font.Bold = True
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
        Dim cmd As New SqlDataAdapter("EXEC pMisInformes @cmd=29, @fechaini='" & fechaini.SelectedDate.Value.ToString("yyyy-MM-dd") & "', @fechafin='" & fechafin.SelectedDate.Value.ToString("yyyy-MM-dd") & "', @proveedorid='" & proveedorid.SelectedValue.ToString & "', @marcaId='" & marcaid.SelectedValue.ToString & "'", conn)
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


End Class