Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Globalization

Public Class RecepcionMercancia
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte de recepción de mercancia"
        If Not IsPostBack Then

            Dim ObjData As New DataControl
            ObjData.Catalogo(marcaid, "select id, nombre from tblProyecto order by id", 0, True)
            ObjData.Catalogo(coleccionid, "select id, isnull(codigo,'') + ' - ' + isnull(nombre,'') as nombre from tblColeccion where isnull(borradoBit,0)=0 order by nombre", 0)
            ObjData = Nothing

            ds = GetProducts()
            Dim boundColumn As GridBoundColumn
            Dim ColumanCount As Integer = ds.Tables(0).Columns.Count - 1
            If (ds.Tables(0).Columns.Count > 8) Then
                For x = 9 To ColumanCount
                    boundColumn = New GridBoundColumn()
                    reporteGrid.MasterTableView.Columns.Add(boundColumn)
                    boundColumn.DataField = ds.Tables(0).Columns(x).ColumnName
                    Dim NombreColuman As String = ds.Tables(0).Columns(x).ColumnName
                    boundColumn.HeaderText = NombreColuman.Replace("_", " ")
                Next
            End If

            reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
            reporteGrid.DataSource = ds
            reporteGrid.DataBind()
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click

        reporteGrid.MasterTableView.Columns.Clear()
        ds = GetProducts()
        Dim boundColumn As GridBoundColumn
        Dim ColumanCount As Integer = ds.Tables(0).Columns.Count - 1
        If (ds.Tables(0).Columns.Count > 0) Then
            For x = 0 To ColumanCount
                boundColumn = New GridBoundColumn()
                reporteGrid.MasterTableView.Columns.Add(boundColumn)
                boundColumn.DataField = ds.Tables(0).Columns(x).ColumnName
                Dim NombreColuman As String = ds.Tables(0).Columns(x).ColumnName
                Dim Mayus As String = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NombreColuman)
                boundColumn.HeaderText = Mayus.Replace("_", " ")
            Next
        End If
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
    End Sub

    Private Sub reporteGrid_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles reporteGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(ds.Tables(0).Compute("sum(existencia)", "")) Then
                        e.Item.Cells(7).Text = ds.Tables(0).Compute("sum(existencia)", "").ToString()
                        e.Item.Cells(7).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(7).Font.Bold = True
                    End If
                    If Not IsDBNull(ds.Tables(0).Compute("sum(totaloc)", "")) Then
                        e.Item.Cells(8).Text = ds.Tables(0).Compute("sum(totaloc)", "").ToString()
                        e.Item.Cells(8).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(8).Font.Bold = True
                    End If
                End If
        End Select
    End Sub

    Private Sub reporteGrid_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles reporteGrid.NeedDataSource
        'ds = GetProducts()
        'reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        'reporteGrid.DataSource = ds

        reporteGrid.MasterTableView.Columns.Clear()
        ds = GetProducts()
        Dim boundColumn As GridBoundColumn
        Dim ColumanCount As Integer = ds.Tables(0).Columns.Count - 1
        If (ds.Tables(0).Columns.Count > 0) Then
            For x = 0 To ColumanCount
                boundColumn = New GridBoundColumn()
                reporteGrid.MasterTableView.Columns.Add(boundColumn)
                boundColumn.DataField = ds.Tables(0).Columns(x).ColumnName
                Dim NombreColuman As String = ds.Tables(0).Columns(x).ColumnName
                Dim Mayus As String = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(NombreColuman)
                boundColumn.HeaderText = Mayus.Replace("_", " ")
            Next
        End If
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds

    End Sub

    Function GetProducts() As DataSet
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pReporteRecepcionMercancia @marcaid='" & marcaid.SelectedValue.ToString & "', @temporadaid='" & coleccionid.SelectedValue.ToString & "'", conn)
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