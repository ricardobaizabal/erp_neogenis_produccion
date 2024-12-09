Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class inventarios
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte de inventarios"
        If Not IsPostBack Then
            Dim ObjData As New DataControl
            ObjData.Catalogo(almacenid, "select id, nombre from tblAlmacen where id <> 4 order by nombre", 0, True)
            ObjData = Nothing

            ds = GetProducts()
            reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
            reporteGrid.DataSource = ds

            reporteGrid.Columns(3).Visible = False
            reporteGrid.Columns(4).Visible = False
            reporteGrid.Columns(5).Visible = False
            reporteGrid.Columns(6).Visible = False
            reporteGrid.Columns(7).Visible = False

            reporteGrid.Columns(8).Visible = False
            reporteGrid.Columns(9).Visible = False
            reporteGrid.Columns(10).Visible = False
            reporteGrid.Columns(11).Visible = False

            reporteGrid.Columns(12).Visible = False
            reporteGrid.Columns(13).Visible = False
            reporteGrid.Columns(14).Visible = False
            reporteGrid.Columns(15).Visible = False
            reporteGrid.Columns(16).Visible = False
            reporteGrid.Columns(17).Visible = False
            reporteGrid.Columns(18).Visible = False
            reporteGrid.Columns(19).Visible = False
            reporteGrid.Columns(20).Visible = False
            reporteGrid.Columns(21).Visible = False
            reporteGrid.Columns(22).Visible = False
            reporteGrid.Columns(23).Visible = False
            reporteGrid.Columns(24).Visible = False
            reporteGrid.Columns(25).Visible = False
            reporteGrid.Columns(26).Visible = False
            reporteGrid.Columns(27).Visible = False
            reporteGrid.Columns(28).Visible = False
            reporteGrid.Columns(29).Visible = False
            reporteGrid.Columns(30).Visible = False
            reporteGrid.Columns(31).Visible = False

            reporteGrid.Columns(32).Visible = True

            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = False

            reporteGrid.Rebind()
        End If
    End Sub

    Private Sub btnGenerate_Click(sender As Object, e As EventArgs) Handles btnGenerate.Click
        'Call OcultaColumnas()
        ds = GetProducts()
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
        '
    End Sub

    Private Sub reporteGrid_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles reporteGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Footer
                If almacenid.SelectedValue = 0 And tipoid.SelectedValue <> 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then
                        If tipoid.SelectedValue = 1 Then
                            If Not IsDBNull(ds.Tables(0).Compute("sum(total_unitario)", "")) Then
                                e.Item.Cells(35).Text = FormatCurrency(ds.Tables(0).Compute("sum(total_unitario)", ""), 2).ToString
                                e.Item.Cells(35).HorizontalAlign = HorizontalAlign.Right
                                e.Item.Cells(35).Font.Bold = True
                            End If
                        ElseIf tipoid.SelectedValue = 2 Then
                            If Not IsDBNull(ds.Tables(0).Compute("sum(total_unitario2)", "")) Then
                                e.Item.Cells(36).Text = FormatCurrency(ds.Tables(0).Compute("sum(total_unitario2)", ""), 2).ToString
                                e.Item.Cells(36).HorizontalAlign = HorizontalAlign.Right
                                e.Item.Cells(36).Font.Bold = True
                            End If
                        ElseIf tipoid.SelectedValue = 3 Then
                            If Not IsDBNull(ds.Tables(0).Compute("sum(total_unitario3)", "")) Then
                                e.Item.Cells(37).Text = FormatCurrency(ds.Tables(0).Compute("sum(total_unitario3)", ""), 2).ToString
                                e.Item.Cells(37).HorizontalAlign = HorizontalAlign.Right
                                e.Item.Cells(37).Font.Bold = True
                            End If
                        ElseIf tipoid.SelectedValue = 4 Then
                            If Not IsDBNull(ds.Tables(0).Compute("sum(total_unitario4)", "")) Then
                                e.Item.Cells(38).Text = FormatCurrency(ds.Tables(0).Compute("sum(total_unitario4)", ""), 2).ToString
                                e.Item.Cells(38).HorizontalAlign = HorizontalAlign.Right
                                e.Item.Cells(38).Font.Bold = True
                            End If
                        ElseIf tipoid.SelectedValue = 5 Then
                            If Not IsDBNull(ds.Tables(0).Compute("sum(total_costo_promedio)", "")) Then
                                e.Item.Cells(39).Text = FormatCurrency(ds.Tables(0).Compute("sum(total_costo_promedio)", ""), 2).ToString
                                e.Item.Cells(39).HorizontalAlign = HorizontalAlign.Right
                                e.Item.Cells(39).Font.Bold = True
                            End If
                        End If
                    End If
                End If
                If almacenid.SelectedValue <> 0 And tipoid.SelectedValue <> 0 Then
                    If ds.Tables(0).Rows.Count > 0 Then

                        If almacenid.SelectedValue = 1 Then 'Monterrey
                            If tipoid.SelectedValue = 1 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mty_unitario)", "")) Then
                                    e.Item.Cells(14).Text = FormatCurrency(ds.Tables(0).Compute("sum(mty_unitario)", ""), 2).ToString
                                    e.Item.Cells(14).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(14).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 2 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mty_unitario2)", "")) Then
                                    e.Item.Cells(15).Text = FormatCurrency(ds.Tables(0).Compute("sum(mty_unitario2)", ""), 2).ToString
                                    e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(15).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 3 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mty_unitario3)", "")) Then
                                    e.Item.Cells(16).Text = FormatCurrency(ds.Tables(0).Compute("sum(mty_unitario3)", ""), 2).ToString
                                    e.Item.Cells(16).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(16).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 4 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mty_unitario4)", "")) Then
                                    e.Item.Cells(17).Text = FormatCurrency(ds.Tables(0).Compute("sum(mty_unitario4)", ""), 2).ToString
                                    e.Item.Cells(17).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(17).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 5 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mty_costo_promedio)", "")) Then
                                    e.Item.Cells(18).Text = FormatCurrency(ds.Tables(0).Compute("sum(mty_costo_promedio)", ""), 2).ToString
                                    e.Item.Cells(18).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(18).Font.Bold = True
                                End If
                            End If
                        ElseIf almacenid.SelectedValue = 2 Then 'Mexico
                            If tipoid.SelectedValue = 1 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mex_unitario)", "")) Then
                                    e.Item.Cells(19).Text = FormatCurrency(ds.Tables(0).Compute("sum(mex_unitario)", ""), 2).ToString
                                    e.Item.Cells(19).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(19).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 2 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mex_unitario2)", "")) Then
                                    e.Item.Cells(20).Text = FormatCurrency(ds.Tables(0).Compute("sum(mex_unitario2)", ""), 2).ToString
                                    e.Item.Cells(20).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(20).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 3 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mex_unitario3)", "")) Then
                                    e.Item.Cells(21).Text = FormatCurrency(ds.Tables(0).Compute("sum(mex_unitario3)", ""), 2).ToString
                                    e.Item.Cells(21).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(21).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 4 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mex_unitario4)", "")) Then
                                    e.Item.Cells(22).Text = FormatCurrency(ds.Tables(0).Compute("sum(mex_unitario4)", ""), 2).ToString
                                    e.Item.Cells(22).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(22).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 5 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(mex_costo_promedio)", "")) Then
                                    e.Item.Cells(23).Text = FormatCurrency(ds.Tables(0).Compute("sum(mex_costo_promedio)", ""), 2).ToString
                                    e.Item.Cells(23).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(23).Font.Bold = True
                                End If
                            End If
                        ElseIf almacenid.SelectedValue = 3 Then 'Guadalajara
                            If tipoid.SelectedValue = 1 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(gdl_unitario)", "")) Then
                                    e.Item.Cells(24).Text = FormatCurrency(ds.Tables(0).Compute("sum(gdl_unitario)", ""), 2).ToString
                                    e.Item.Cells(24).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(24).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 2 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(gdl_unitario2)", "")) Then
                                    e.Item.Cells(25).Text = FormatCurrency(ds.Tables(0).Compute("sum(gdl_unitario2)", ""), 2).ToString
                                    e.Item.Cells(25).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(25).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 3 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(gdl_unitario3)", "")) Then
                                    e.Item.Cells(26).Text = FormatCurrency(ds.Tables(0).Compute("sum(gdl_unitario3)", ""), 2).ToString
                                    e.Item.Cells(26).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(26).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 4 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(gdl_unitario4)", "")) Then
                                    e.Item.Cells(27).Text = FormatCurrency(ds.Tables(0).Compute("sum(gdl_unitario4)", ""), 2).ToString
                                    e.Item.Cells(27).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(27).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 5 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(gdl_costo_promedio)", "")) Then
                                    e.Item.Cells(28).Text = FormatCurrency(ds.Tables(0).Compute("sum(gdl_costo_promedio)", ""), 2).ToString
                                    e.Item.Cells(28).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(28).Font.Bold = True
                                End If
                            End If
                        ElseIf almacenid.SelectedValue = 5 Then 'Matriz 
                            If tipoid.SelectedValue = 1 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(alm_unitario)", "")) Then
                                    e.Item.Cells(29).Text = FormatCurrency(ds.Tables(0).Compute("sum(alm_unitario)", ""), 2).ToString
                                    e.Item.Cells(29).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(29).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 2 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(alm_unitario2)", "")) Then
                                    e.Item.Cells(30).Text = FormatCurrency(ds.Tables(0).Compute("sum(alm_unitario2)", ""), 2).ToString
                                    e.Item.Cells(30).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(30).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 3 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(alm_unitario3)", "")) Then
                                    e.Item.Cells(31).Text = FormatCurrency(ds.Tables(0).Compute("sum(alm_unitario3)", ""), 2).ToString
                                    e.Item.Cells(31).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(31).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 4 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(alm_unitario4)", "")) Then
                                    e.Item.Cells(32).Text = FormatCurrency(ds.Tables(0).Compute("sum(alm_unitario4)", ""), 2).ToString
                                    e.Item.Cells(32).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(32).Font.Bold = True
                                End If
                            ElseIf tipoid.SelectedValue = 5 Then
                                If Not IsDBNull(ds.Tables(0).Compute("sum(alm_costo_promedio)", "")) Then
                                    e.Item.Cells(33).Text = FormatCurrency(ds.Tables(0).Compute("sum(alm_costo_promedio)", ""), 2).ToString
                                    e.Item.Cells(33).HorizontalAlign = HorizontalAlign.Right
                                    e.Item.Cells(33).Font.Bold = True
                                End If
                            End If
                        End If
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
        Dim cmd As New SqlDataAdapter("EXEC pMisInformes @cmd=24, @almacenid='" & almacenid.SelectedValue.ToString & "'", conn)
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
        Call OcultaColumnas()
        ds = GetProducts()
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
    End Sub

    Private Sub tipoid_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tipoid.SelectedIndexChanged
        If tipoid.SelectedValue = 0 Then
            reporteGrid.Columns(3).Visible = False
            reporteGrid.Columns(4).Visible = False
            reporteGrid.Columns(5).Visible = False
            reporteGrid.Columns(6).Visible = False
            reporteGrid.Columns(7).Visible = False

            reporteGrid.Columns(32).Visible = True
            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = False
        ElseIf tipoid.SelectedValue = 1 Then
            reporteGrid.Columns(3).Visible = True
            reporteGrid.Columns(4).Visible = False
            reporteGrid.Columns(5).Visible = False
            reporteGrid.Columns(6).Visible = False
            reporteGrid.Columns(7).Visible = False

            reporteGrid.Columns(32).Visible = False
            reporteGrid.Columns(33).Visible = True
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = False
        ElseIf tipoid.SelectedValue = 2 Then
            reporteGrid.Columns(3).Visible = False
            reporteGrid.Columns(4).Visible = True
            reporteGrid.Columns(5).Visible = False
            reporteGrid.Columns(6).Visible = False
            reporteGrid.Columns(7).Visible = False

            reporteGrid.Columns(32).Visible = False
            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = True
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = False
        ElseIf tipoid.SelectedValue = 3 Then
            reporteGrid.Columns(3).Visible = False
            reporteGrid.Columns(4).Visible = False
            reporteGrid.Columns(5).Visible = True
            reporteGrid.Columns(6).Visible = False
            reporteGrid.Columns(7).Visible = False

            reporteGrid.Columns(32).Visible = False
            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = True
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = False
        ElseIf tipoid.SelectedValue = 4 Then
            reporteGrid.Columns(3).Visible = False
            reporteGrid.Columns(4).Visible = False
            reporteGrid.Columns(5).Visible = False
            reporteGrid.Columns(6).Visible = True
            reporteGrid.Columns(7).Visible = False

            reporteGrid.Columns(32).Visible = False
            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = True
            reporteGrid.Columns(37).Visible = False
        ElseIf tipoid.SelectedValue = 5 Then
            reporteGrid.Columns(3).Visible = False
            reporteGrid.Columns(4).Visible = False
            reporteGrid.Columns(5).Visible = False
            reporteGrid.Columns(6).Visible = False
            reporteGrid.Columns(7).Visible = True

            reporteGrid.Columns(32).Visible = False
            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = True
        End If

        Call OcultaColumnas()
        ds = GetProducts()
        reporteGrid.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()

    End Sub

    Private Sub OcultaColumnas()
        If almacenid.SelectedValue = 0 Then 'Todos
            reporteGrid.Columns(8).Visible = False
            reporteGrid.Columns(9).Visible = False
            reporteGrid.Columns(10).Visible = False
            reporteGrid.Columns(11).Visible = False

            reporteGrid.Columns(12).Visible = False
            reporteGrid.Columns(13).Visible = False
            reporteGrid.Columns(14).Visible = False
            reporteGrid.Columns(15).Visible = False
            reporteGrid.Columns(16).Visible = False
            reporteGrid.Columns(17).Visible = False
            reporteGrid.Columns(18).Visible = False
            reporteGrid.Columns(19).Visible = False
            reporteGrid.Columns(20).Visible = False
            reporteGrid.Columns(21).Visible = False
            reporteGrid.Columns(22).Visible = False
            reporteGrid.Columns(23).Visible = False
            reporteGrid.Columns(24).Visible = False
            reporteGrid.Columns(25).Visible = False
            reporteGrid.Columns(26).Visible = False
            reporteGrid.Columns(27).Visible = False
            reporteGrid.Columns(28).Visible = False
            reporteGrid.Columns(29).Visible = False
            reporteGrid.Columns(30).Visible = False
            reporteGrid.Columns(31).Visible = False

            reporteGrid.Columns(32).Visible = True

            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = False

            If tipoid.SelectedValue = 0 Then
                reporteGrid.Columns(33).Visible = False
                reporteGrid.Columns(34).Visible = False
                reporteGrid.Columns(35).Visible = False
                reporteGrid.Columns(36).Visible = False
                reporteGrid.Columns(37).Visible = False
            ElseIf tipoid.SelectedValue = 1 Then
                reporteGrid.Columns(33).Visible = True
                reporteGrid.Columns(34).Visible = False
                reporteGrid.Columns(35).Visible = False
                reporteGrid.Columns(36).Visible = False
                reporteGrid.Columns(37).Visible = False
            ElseIf tipoid.SelectedValue = 2 Then
                reporteGrid.Columns(33).Visible = False
                reporteGrid.Columns(34).Visible = True
                reporteGrid.Columns(35).Visible = False
                reporteGrid.Columns(36).Visible = False
                reporteGrid.Columns(37).Visible = False
            ElseIf tipoid.SelectedValue = 3 Then
                reporteGrid.Columns(33).Visible = False
                reporteGrid.Columns(34).Visible = False
                reporteGrid.Columns(35).Visible = True
                reporteGrid.Columns(36).Visible = False
                reporteGrid.Columns(37).Visible = False
            ElseIf tipoid.SelectedValue = 4 Then
                reporteGrid.Columns(33).Visible = False
                reporteGrid.Columns(34).Visible = False
                reporteGrid.Columns(35).Visible = False
                reporteGrid.Columns(36).Visible = True
                reporteGrid.Columns(37).Visible = False
            ElseIf tipoid.SelectedValue = 5 Then
                reporteGrid.Columns(33).Visible = False
                reporteGrid.Columns(34).Visible = False
                reporteGrid.Columns(35).Visible = False
                reporteGrid.Columns(36).Visible = False
                reporteGrid.Columns(37).Visible = True
            End If

        ElseIf almacenid.SelectedValue = 1 Then 'Monterrey
            reporteGrid.Columns(8).Visible = False
            reporteGrid.Columns(9).Visible = True
            reporteGrid.Columns(10).Visible = False
            reporteGrid.Columns(11).Visible = False

            If tipoid.SelectedValue = 0 Then
                reporteGrid.Columns(3).Visible = False
                reporteGrid.Columns(4).Visible = False
                reporteGrid.Columns(5).Visible = False
                reporteGrid.Columns(6).Visible = False
                reporteGrid.Columns(7).Visible = False
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 1 Then
                reporteGrid.Columns(12).Visible = True
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 2 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = True
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 3 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = True
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 4 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = True
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 5 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = True
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            End If
        ElseIf almacenid.SelectedValue = 2 Then 'Mexico
            reporteGrid.Columns(8).Visible = True
            reporteGrid.Columns(9).Visible = False
            reporteGrid.Columns(10).Visible = False
            reporteGrid.Columns(11).Visible = False
            If tipoid.SelectedValue = 0 Then
                reporteGrid.Columns(3).Visible = False
                reporteGrid.Columns(4).Visible = False
                reporteGrid.Columns(5).Visible = False
                reporteGrid.Columns(6).Visible = False
                reporteGrid.Columns(7).Visible = False
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 1 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = True
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 2 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = True
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 3 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = True
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 4 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = True
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 5 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = True
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            End If
        ElseIf almacenid.SelectedValue = 3 Then 'Guadalajara
            reporteGrid.Columns(8).Visible = False
            reporteGrid.Columns(9).Visible = False
            reporteGrid.Columns(10).Visible = True
            reporteGrid.Columns(11).Visible = False

            If tipoid.SelectedValue = 0 Then
                reporteGrid.Columns(3).Visible = False
                reporteGrid.Columns(4).Visible = False
                reporteGrid.Columns(5).Visible = False
                reporteGrid.Columns(6).Visible = False
                reporteGrid.Columns(7).Visible = False
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 1 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = True
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 2 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = True
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 3 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = True
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 4 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = True
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 5 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = True
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            End If
        ElseIf almacenid.SelectedValue = 5 Then 'Matriz
            reporteGrid.Columns(8).Visible = False
            reporteGrid.Columns(9).Visible = False
            reporteGrid.Columns(10).Visible = False
            reporteGrid.Columns(11).Visible = True
            If tipoid.SelectedValue = 0 Then
                reporteGrid.Columns(3).Visible = False
                reporteGrid.Columns(4).Visible = False
                reporteGrid.Columns(5).Visible = False
                reporteGrid.Columns(6).Visible = False
                reporteGrid.Columns(7).Visible = False
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False
                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False
                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False
                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 1 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False

                reporteGrid.Columns(27).Visible = True
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 2 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False

                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = True
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 3 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False

                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = True
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 4 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False

                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = True
                reporteGrid.Columns(31).Visible = False
            ElseIf tipoid.SelectedValue = 5 Then
                reporteGrid.Columns(12).Visible = False
                reporteGrid.Columns(13).Visible = False
                reporteGrid.Columns(14).Visible = False
                reporteGrid.Columns(15).Visible = False
                reporteGrid.Columns(16).Visible = False

                reporteGrid.Columns(17).Visible = False
                reporteGrid.Columns(18).Visible = False
                reporteGrid.Columns(19).Visible = False
                reporteGrid.Columns(20).Visible = False
                reporteGrid.Columns(21).Visible = False

                reporteGrid.Columns(22).Visible = False
                reporteGrid.Columns(23).Visible = False
                reporteGrid.Columns(24).Visible = False
                reporteGrid.Columns(25).Visible = False
                reporteGrid.Columns(26).Visible = False

                reporteGrid.Columns(27).Visible = False
                reporteGrid.Columns(28).Visible = False
                reporteGrid.Columns(29).Visible = False
                reporteGrid.Columns(30).Visible = False
                reporteGrid.Columns(31).Visible = True
            End If
        End If

        If almacenid.SelectedValue <> 0 Then 'Diferente de Todos
            reporteGrid.Columns(32).Visible = False

            reporteGrid.Columns(33).Visible = False
            reporteGrid.Columns(34).Visible = False
            reporteGrid.Columns(35).Visible = False
            reporteGrid.Columns(36).Visible = False
            reporteGrid.Columns(37).Visible = False
        End If

        reporteGrid.Rebind()
    End Sub

End Class