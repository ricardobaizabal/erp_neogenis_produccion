Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.Threading
Imports System.IO
Imports System.Linq

Public Class editarorden
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call MuestraDatosGenerales()
            Call CargaConceptos()
            'Call CargaOrdenParcial()
        End If
        '
        'btnProcess.Attributes.Add("onclick", "javascript:return confirm('Va a procesar este pedido, una vez procesado no podrá modificarlo. ¿Desea continuar?');")
        btnConfAlmacenado.Attributes.Add("onclick", "javascript:return confirm('Se dara entrada a la order parcial, ¿Desea continuar?');")
        '
    End Sub

    Private Sub MuestraDatosGenerales()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("exec pOrdenCompra @cmd=3, @ordenid='" & Request("id").ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                lblClaveEdit.Text = rs("claveEdit").ToString
                Dim ObjData As New DataControl
                ObjData.Catalogo(proveedorid, "select id, razonsocial as nombre from tblMisProveedores order by razonsocial", rs("proveedorid"))
                ObjData.Catalogo(proyectoid, "select id, nombre from tblProyecto order by nombre", rs("marcaid"))
                ObjData = Nothing

                Dim edate1 = rs("fechaEstEnt").ToString
                Dim fechaEstEnt As Date = Date.ParseExact(edate1, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)

                txtfechaEstEnt.SelectedDate = fechaEstEnt
                txtComentarios.Text = rs("comentarios")
                Session("estatusid") = rs("estatusid")
                orderParcialIdHidden.Value = 0

                'If Session("estatusid").ToString = "3" Then
                '    btnProcess.Enabled = False
                '    btnProcess.ToolTip = "Operación no permitida."
                'End If

                If rs("estatusid") <> 1 Then
                    proveedorid.Enabled = False
                    proyectoid.Enabled = False
                    txtComentarios.Enabled = False
                    txtSearch.Enabled = False
                    'btnAddorder.Enabled = False
                    'btnProcess.Enabled = False
                    btnSearch.Enabled = False
                    btnCancel.Enabled = False
                    btnCargarExcel.Enabled = False
                    fileUploadExcel.Enabled = False

                End If

            End If

        Catch ex As Exception

        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
    End Sub

    Private Sub CargaConceptos()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=7, @ordenid='" & Request("id").ToString & "'")
        Dim lineas As Integer = ds.Tables(0).Rows.Count

        If lineas = 0 Then
            lineas = 1
        End If

        conceptosList.PageSize = lineas
        conceptosList.DataSource = ds
        conceptosList.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As System.EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        panelSearch.Visible = True
        btnAgregaConceptos.Visible = True
        Dim ObjData As New DataControl
        resultslist.DataSource = ObjData.FillDataSet("exec pMisProductos @cmd=1, @txtSearch='" & txtSearch.Text & "'")
        resultslist.DataBind()
        ObjData = Nothing
    End Sub

    Private Sub resultslist_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles resultslist.ItemCommand
        Select Case e.CommandName
            Case "cmdAdd"
                Call AgregaItem(e.CommandArgument, e.Item)
        End Select
    End Sub

    Private Sub resultslist_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles resultslist.NeedDataSource
        Dim ObjData As New DataControl
        resultslist.DataSource = ObjData.FillDataSet("exec pMisProductos @cmd=1, @txtSearch='" & txtSearch.Text & "'")
        ObjData = Nothing
    End Sub

    Private Sub ActFechaOrdenParcial(ByVal idOrdenPracial As Long)
        Dim ObjData As New DataControl

        Dim fecha As String = ""
        Dim id As String = ""


        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=14, @id='" & idOrdenPracial.ToString & "', @fechaEstEnt='" & fecha.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub AgregaItem(ByVal productoId As Long, ByVal item As GridItem)
        Dim txtCantidad As RadNumericTextBox = DirectCast(item.FindControl("txtCantidad"), RadNumericTextBox)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=5, @ordenId='" & Request("id").ToString & "', @cantidad='" & txtCantidad.Text & "', @productoId='" & productoId.ToString & "'")
        ObjData = Nothing
        '
        txtSearch.Text = ""
        panelSearch.Visible = False
        Call CargaConceptos()
        '
    End Sub

    Private Sub conceptosList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles conceptosList.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Call EliminaConcepto(e.CommandArgument)
                Call CargaConceptos()
            Case "cmdDeleteOCP"
                Call EliminaOrdenParcial(e.CommandArgument)
                Call CargaListaOrdenesParciales()
            Case "cmdDeleteOCP"

        End Select
    End Sub

    Private Sub resultslistCSV_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles resultslistCSV.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim txtCantidad As RadNumericTextBox = CType(e.Item.FindControl("txtCantidad"), RadNumericTextBox)
                txtCantidad.Text = e.Item.DataItem("cantidad")
        End Select

    End Sub

    Private Sub conceptosList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles conceptosList.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim ordenIdParcial As Integer = 0
                Dim txtCantidad As RadNumericTextBox = CType(e.Item.FindControl("txtCantidad"), RadNumericTextBox)

                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un concepto de la orden de compra. ¿Desea continuar?');")

                If Session("estatusid") <> 1 Then
                    btnDelete.Visible = False
                End If

                If e.Item.DataItem("disponible") = 0 Then
                    'txtCantidad.Enabled = False
                End If

        End Select
    End Sub

    Private Sub conceptosList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles conceptosList.NeedDataSource
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pOrdenCompra @cmd=7, @ordenid='" & Request("id").ToString & "'")
        conceptosList.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub EliminaConcepto(ByVal conceptoid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=6, @conceptoid='" & conceptoid.ToString & "'")
        ObjData = Nothing
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        txtSearch.Text = ""
        panelSearch.Visible = False
    End Sub

    Private Sub btnAgregaConceptos_Click(sender As Object, e As EventArgs) Handles btnAgregaConceptos.Click
        '
        Dim productoId As Long = 0
        Dim ObjData As New DataControl
        For Each row As GridDataItem In resultslist.MasterTableView.Items
            productoId = row.GetDataKeyValue("id")
            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
            If Convert.ToDecimal(txtCantidad.Text.ToString) > 0 Then
                ObjData.RunSQLQuery("exec pOrdenCompra @cmd=5, @ordenId='" & Request("id").ToString & "', @cantidad='" & txtCantidad.Text & "', @productoId='" & productoId.ToString & "'")
            End If
        Next
        For Each row As GridDataItem In resultslistCSV.MasterTableView.Items
            productoId = row.GetDataKeyValue("productoid")
            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
            If Convert.ToDecimal(txtCantidad.Text.ToString) > 0 Then
                ObjData.RunSQLQuery("exec pOrdenCompra @cmd=5, @ordenId='" & Request("id").ToString & "', @cantidad='" & txtCantidad.Text & "', @productoId='" & productoId.ToString & "'")
            End If
        Next

        ObjData = Nothing
        '
        txtSearch.Text = ""
        panelSearch.Visible = False
        panelCSV.Visible = False
        panelErrores.Visible = False
        btnAgregaConceptos.Visible = False
        resultslist.DataSource = Nothing
        resultslist.DataBind()
        resultslistCSV.DataSource = Nothing
        resultslistCSV.DataBind()
        erroresList.DataSource = Nothing
        erroresList.DataBind()
        Call CargaConceptos()
    End Sub

#Region "Carga de CVS"

    Protected Sub btnCargarExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargarExcel.Click
        If fileUploadExcel.HasFile Then
            If System.IO.Path.GetExtension(fileUploadExcel.FileName).ToUpper = ".CSV" Then
                panelErrores.Visible = False

                Dim nombreArchivo As String = ""
                nombreArchivo = fileUploadExcel.PostedFile.FileName.ToString

                For i = 1 To 99999
                    If Not File.Exists(Server.MapPath("../proveedores/carga/") & nombreArchivo) Then
                        fileUploadExcel.SaveAs(Server.MapPath("../proveedores/carga/") & nombreArchivo)
                        Exit For
                    Else
                        nombreArchivo = i.ToString + "_" + fileUploadExcel.PostedFile.FileName.ToString
                    End If
                Next

                Dim registros As Integer
                Try
                    registros = System.IO.File.ReadAllLines(Server.MapPath("../proveedores/carga/") & nombreArchivo).Count
                Catch ex As Exception
                    registros = 0
                End Try

                If registros > 0 Then
                    registros = registros - 1
                End If

                Dim registros_error As Integer = 0
                Dim registros_correctos As Integer = 0
                Dim msgerror As String = ""

                Dim cargaid As Long = 0
                Dim ObjData As New DataControl
                cargaid = ObjData.RunSQLScalarQuery("exec pCargaConceptos @cmd=1, @userid='" & Session("userid").ToString & "', @archivo='" & nombreArchivo.ToString & "', @registros='" & registros.ToString & "'")
                cargaidHidden.Value = cargaid

                If registros = 0 Then
                    ObjData.RunSQLQuery("exec pCargaConceptos @cmd=9, @cargaid='" & cargaid.ToString & "")
                End If

                Dim progress As RadProgressContext = RadProgressContext.Current
                progress.SecondaryTotal = registros
                progress.Speed = "N/A"

                ds = New DataSet
                ds = ObjData.FillDataSet("exec pCargaConceptos @cmd=2")

                Dim dtConceptosDetalle As New DataTable

                If ds.Tables.Count > 0 Then
                    dtConceptosDetalle = ds.Tables(0)

                End If

                Dim j As Integer = 0
                Dim line As String = ""

                Dim reader As System.IO.StreamReader = New StreamReader(Server.MapPath("../proveedores/carga/") & nombreArchivo, System.Text.Encoding.Default)
                Do

                    j += 1

                    line = reader.ReadLine

                    If line = Nothing Then
                        Exit Do
                    End If

                    Dim codigo As String = ""
                    Dim cantidad As Integer = 0
                    Dim codigoId As String = ""
                    Dim codigoNoActivo As Integer = 0



                    If j > 1 Then
                        ' CODIGO
                        Try
                            codigo = line.Split(",")(0)
                        Catch ex As Exception
                            codigo = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            cantidad = line.Split(",")(1)
                        Catch ex As Exception
                            cantidad = 0
                        End Try

                        Try
                            Dim rowCodigo() As DataRow = dtConceptosDetalle.Select("codigo = '" & LTrim(RTrim(codigo)).ToString & "'")
                            For Each row As DataRow In rowCodigo
                                codigo = row(0)
                            Next
                        Catch ex As Exception
                            codigo = 0
                        End Try

                        Try
                            Dim rowCantidad() As DataRow = dtConceptosDetalle.Select("cantidad = '" & LTrim(RTrim(cantidad)).ToString & "'")
                            For Each row As DataRow In rowCantidad
                                cantidad = row(0)
                            Next
                        Catch ex As Exception
                            cantidad = 0
                        End Try

                        If codigo.Length > 0 And cantidad > 0 Then
                            Dim productoid As Long

                            codigoNoActivo = ObjData.RunSQLScalarQuery("exec pCargaConceptos @cmd=14, @codigo='" & codigo & "'")

                            If codigoNoActivo > 0 Then
                                ObjData.RunSQLScalarQuery("exec pCargaConceptos @cmd=15, @id='" & codigoNoActivo & "'")
                            End If

                            productoid = ObjData.RunSQLScalarQuery("exec pCargaConceptos @cmd=4, @codigo='" & codigo & "', @cargaid='" & cargaid.ToString & "'")
                            codigoId = ObjData.RunSQLScalarQuery("exec pCargaConceptos @cmd=13, @codigo='" & codigo & "'")

                            If productoid > 0 Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " ya se encuentra registrado."))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptos", p)
                                registros_error = registros_error + 1

                                'ElseIf codigoNoActivo = 0 Then
                                '    Dim p As New ArrayList
                                '    p.Add(New SqlParameter("@cmd", 7))
                                '    p.Add(New SqlParameter("@cargaid", cargaid))
                                '    p.Add(New SqlParameter("@codigo", codigo))
                                '    p.Add(New SqlParameter("@cantidad", cantidad))
                                '    p.Add(New SqlParameter("@error", "El código " & codigo & "Producto no activo."))
                                '    ObjData.ExecuteNonQueryWithParams("pCargaConceptos", p)
                                '    registros_error = registros_error + 1

                            ElseIf codigoId = "0" Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " no está registrado."))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptos", p)
                                registros_error = registros_error + 1

                            Else
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 3))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptos", p)
                                registros_correctos = registros_correctos + 1
                            End If
                        Else

                            msgerror = ""
                            If codigo.Length = 0 Then
                                msgerror = msgerror & "Código es requerido."
                            End If

                            If cantidad = 0 Then
                                msgerror = msgerror & vbCrLf & "Cantidad es requerida."
                            End If

                            If codigoId.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Codigo no registrado."
                            End If

                            'If codigoNoActivo = 0 Then
                            '    msgerror = msgerror & vbCrLf & "Producto no activo."
                            'End If

                            'If unitario = 0 Then
                            '    msgerror = msgerror & vbCrLf & "Precio unitario es requerido."
                            'End If
                            Dim p As New ArrayList
                            p.Add(New SqlParameter("@cmd", 7))
                            p.Add(New SqlParameter("@cargaid", cargaid))
                            p.Add(New SqlParameter("@codigo", codigo))
                            p.Add(New SqlParameter("@cantidad", cantidad))
                            p.Add(New SqlParameter("@error", msgerror))
                            p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                            ObjData.ExecuteNonQueryWithParams("pCargaConceptos", p)
                            registros_error = registros_error + 1
                        End If

                    End If

                    progress.SecondaryValue = j
                    progress.SecondaryPercent = Math.Round((j * 100 / registros), 0)
                    progress.CurrentOperationText = "Registro " & j.ToString()

                    If Not Response.IsClientConnected Then
                        Exit Do
                    End If

                    progress.TimeEstimated = (registros - j) * 100
                    System.Threading.Thread.Sleep(100)

                Loop Until line Is Nothing

                reader.Close()

                If registros_correctos > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 8))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_correctos", registros_correctos))
                    ObjData.ExecuteNonQueryWithParams("pCargaConceptos", p)
                End If

                If registros_error > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 9))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_error", registros_error))
                    ObjData.ExecuteNonQueryWithParams("pCargaConceptos", p)

                    panelErrores.Visible = True
                    erroresList.DataSource = ObjData.FillDataSet("exec pCargaConceptos @cmd=10, @cargaid='" & cargaid.ToString & "'")
                    erroresList.DataBind()

                End If

                If registros_correctos = 0 Then
                    ObjData.RunSQLQuery("exec pCargaConceptos @cmd=11, @cargaid='" & cargaid.ToString & "'")
                End If

                If registros_correctos > 0 And registros_error > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.<br>" & registros_error.ToString & " con error, favor de verificar.", 350, 200, "Alerta", "", "")
                ElseIf registros_correctos > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.", 350, 200, "Alerta", "", "")
                ElseIf registros_error > 0 Then
                    rwAlerta.RadAlert("Se encontraron " & registros_error.ToString & " productos con error, favor de verificar.", 350, 200, "Alerta", "", "")
                End If

                ObjData = Nothing
            Else
                rwAlerta.RadAlert("Formato CSV no válido.", 350, 200, "Alerta", "", "")
            End If
        Else
            rwAlerta.RadAlert("Selecciona un archivo en formato CSV.", 350, 200, "Alerta", "", "")
        End If

        panelCSV.Visible = True
        btnAgregaConceptos.Visible = True
        Dim ObjData2 As New DataControl
        ds = ObjData2.FillDataSet("exec pCargaConceptos @cmd=5, @cargaid='" & cargaidHidden.Value & "'")
        Dim lineas As Integer = ds.Tables(0).Rows.Count
        If lineas = 0 Then
            lineas = 1
        End If
        resultslistCSV.PageSize = lineas
        resultslistCSV.DataSource = ds
        resultslistCSV.DataBind()
        ObjData2.RunSQLQuery("exec pCargaConceptos @cmd=12")

        ObjData2 = Nothing
    End Sub

    Private Sub imgDownload_Click(sender As Object, e As ImageClickEventArgs) Handles imgDownload.Click
        Dim FilePath = Server.MapPath("~/portalcfd/proveedores/carga/") & "FORMATOCARGA.csv"

        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        End If
    End Sub
    Private Sub erroresList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles erroresList.NeedDataSource
        Dim ObjData As New DataControl
        erroresList.DataSource = ObjData.FillDataSet("exec pCargaConceptos @cmd=10, @cargaid='" & cargaidHidden.Value.ToString & "'")
        ObjData = Nothing
    End Sub

#End Region


#Region "Carga de CVS Orden compra parcial"

    Protected Sub btnOCPcsv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOCPcsv.Click
        If fileUploadOCP.HasFile Then
            If System.IO.Path.GetExtension(fileUploadOCP.FileName).ToUpper = ".CSV" Then
                panelErroresOCP.Visible = False

                Dim nombreArchivo As String = ""
                nombreArchivo = fileUploadOCP.PostedFile.FileName.ToString

                For i = 1 To 99999
                    If Not File.Exists(Server.MapPath("../proveedores/cargaocp/") & nombreArchivo) Then
                        fileUploadOCP.SaveAs(Server.MapPath("../proveedores/cargaocp/") & nombreArchivo)
                        Exit For
                    Else
                        nombreArchivo = i.ToString + "_" + fileUploadOCP.PostedFile.FileName.ToString
                    End If
                Next

                Dim registros As Integer
                Try
                    registros = System.IO.File.ReadAllLines(Server.MapPath("../proveedores/cargaocp/") & nombreArchivo).Count
                Catch ex As Exception
                    registros = 0
                End Try

                If registros > 0 Then
                    registros = registros - 1
                End If

                Dim registros_error As Integer = 0
                Dim registros_correctos As Integer = 0
                Dim msgerror As String = ""

                Dim cargaid As Long = 0
                Dim ObjData As New DataControl
                cargaid = ObjData.RunSQLScalarQuery("exec pCargaConceptosOCP @cmd=1, @userid='" & Session("userid").ToString & "', @archivo='" & nombreArchivo.ToString & "', @registros='" & registros.ToString & "'")
                cargaidOCPHidden.Value = cargaid

                If registros = 0 Then
                    ObjData.RunSQLQuery("exec pCargaConceptosOCP @cmd=9, @cargaid='" & cargaid.ToString & "")
                End If

                Dim progress As RadProgressContext = RadProgressContext.Current
                progress.SecondaryTotal = registros
                progress.Speed = "N/A"

                ds = New DataSet
                ds = ObjData.FillDataSet("exec pCargaConceptosOCP @cmd=2")

                Dim dtConceptosDetalleOCP As New DataTable

                If ds.Tables.Count > 0 Then
                    dtConceptosDetalleOCP = ds.Tables(0)

                End If

                Dim j As Integer = 0
                Dim line As String = ""

                Dim reader As System.IO.StreamReader = New StreamReader(Server.MapPath("../proveedores/cargaocp/") & nombreArchivo, System.Text.Encoding.Default)
                Do

                    j += 1

                    line = reader.ReadLine

                    If line = Nothing Then
                        Exit Do
                    End If

                    Dim codigo As String = ""
                    Dim cantidad As Integer = 0
                    Dim codigoId As String = ""
                    Dim codigoNoActivo As Integer = 0
                    Dim codigoConcepPrincipal As String = ""
                    Dim cantidadDisponible As Integer = 0

                    If j > 1 Then
                        ' CODIGO
                        Try
                            codigo = line.Split(",")(0)
                        Catch ex As Exception
                            codigo = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            cantidad = line.Split(",")(1)
                        Catch ex As Exception
                            cantidad = 0
                        End Try

                        Try
                            Dim rowCodigo() As DataRow = dtConceptosDetalleOCP.Select("codigo = '" & LTrim(RTrim(codigo)).ToString & "'")
                            For Each row As DataRow In rowCodigo
                                codigo = row(0)
                            Next
                        Catch ex As Exception
                            codigo = 0
                        End Try

                        Try
                            Dim rowCantidad() As DataRow = dtConceptosDetalleOCP.Select("cantidad = '" & LTrim(RTrim(cantidad)).ToString & "'")
                            For Each row As DataRow In rowCantidad
                                cantidad = row(0)
                            Next
                        Catch ex As Exception
                            cantidad = 0
                        End Try

                        If codigo.Length > 0 And cantidad > 0 Then
                            Dim productoid As Long

                            cantidadDisponible = ObjData.RunSQLScalarQuery("exec pCargaConceptosOCP @cmd=17, @ordenId='" & Request("id").ToString & "',@codigo='" & codigo & "'")

                            codigoConcepPrincipal = ObjData.RunSQLScalarQuery("exec pCargaConceptosOCP @cmd=16, @ordenId='" & Request("id").ToString & "',@codigo='" & codigo & "'")

                            codigoNoActivo = ObjData.RunSQLScalarQuery("exec pCargaConceptosOCP @cmd=14, @codigo='" & codigo & "'")

                            If codigoNoActivo > 0 Then
                                ObjData.RunSQLScalarQuery("exec pCargaConceptosOCP @cmd=15, @id='" & codigoNoActivo & "'")
                            End If

                            productoid = ObjData.RunSQLScalarQuery("exec pCargaConceptosOCP @cmd=4, @codigo='" & codigo & "', @cargaid='" & cargaid.ToString & "'")
                            codigoId = ObjData.RunSQLScalarQuery("exec pCargaConceptosOCP @cmd=13, @codigo='" & codigo & "'")


                            If productoid > 0 Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " ya se encuentra registrado."))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)
                                registros_error = registros_error + 1

                            ElseIf codigoConcepPrincipal = "0" Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " no está registrado en Orden Principal."))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)
                                registros_error = registros_error + 1


                            ElseIf codigoId = "0" Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " no está registrado."))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)
                                registros_error = registros_error + 1

                            ElseIf cantidad > cantidadDisponible Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " Cantidad de entrada es mayor que el disponible."))
                                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)
                                registros_error = registros_error + 1

                            Else
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 3))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                'p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)
                                registros_correctos = registros_correctos + 1
                            End If
                        Else

                            msgerror = ""
                            If codigo.Length = 0 Then
                                msgerror = msgerror & "Código es requerido."
                            End If

                            If cantidad = 0 Then
                                msgerror = msgerror & vbCrLf & "Cantidad es requerida."
                            End If

                            If codigoId.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Codigo no registrado."
                            End If

                            'If codigoNoActivo = 0 Then
                            '    msgerror = msgerror & vbCrLf & "Producto no activo."
                            'End If

                            'If unitario = 0 Then
                            '    msgerror = msgerror & vbCrLf & "Precio unitario es requerido."
                            'End If
                            Dim p As New ArrayList
                            p.Add(New SqlParameter("@cmd", 7))
                            p.Add(New SqlParameter("@cargaid", cargaid))
                            p.Add(New SqlParameter("@codigo", codigo))
                            p.Add(New SqlParameter("@cantidad", cantidad))
                            p.Add(New SqlParameter("@error", msgerror))
                            p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                            ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)
                            registros_error = registros_error + 1
                        End If

                    End If

                    progress.SecondaryValue = j
                    progress.SecondaryPercent = Math.Round((j * 100 / registros), 0)
                    progress.CurrentOperationText = "Registro " & j.ToString()

                    If Not Response.IsClientConnected Then
                        Exit Do
                    End If

                    progress.TimeEstimated = (registros - j) * 100
                    System.Threading.Thread.Sleep(100)

                Loop Until line Is Nothing

                reader.Close()

                If registros_correctos > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 8))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_correctos", registros_correctos))
                    ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)
                End If

                If registros_error > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 9))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_error", registros_error))
                    ObjData.ExecuteNonQueryWithParams("pCargaConceptosOCP", p)

                    panelErroresOCP.Visible = True
                    erroresListOCP.DataSource = ObjData.FillDataSet("exec pCargaConceptosOCP @cmd=10, @cargaid='" & cargaid.ToString & "'")
                    erroresListOCP.DataBind()

                End If

                If registros_correctos = 0 Then
                    ObjData.RunSQLQuery("exec pCargaConceptosOCP @cmd=11, @cargaid='" & cargaid.ToString & "'")
                End If

                If registros_correctos > 0 And registros_error > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.<br>" & registros_error.ToString & " con error, favor de verificar.", 350, 200, "Alerta", "", "")
                ElseIf registros_correctos > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.", 350, 200, "Alerta", "", "")
                ElseIf registros_error > 0 Then
                    rwAlerta.RadAlert("Se encontraron " & registros_error.ToString & " productos con error, favor de verificar.", 350, 200, "Alerta", "", "")
                End If

                ObjData = Nothing
            Else
                rwAlerta.RadAlert("Formato CSV no válido.", 350, 200, "Alerta", "", "")
            End If
        Else
            rwAlerta.RadAlert("Selecciona un archivo en formato CSV.", 350, 200, "Alerta", "", "")
        End If

        panelCSVOCP.Visible = True
        btnAgregaConceptos.Visible = True
        Dim ObjData2 As New DataControl
        ds = ObjData2.FillDataSet("exec pCargaConceptosOCP @cmd=5, @cargaid='" & cargaidOCPHidden.Value & "'")
        ObjData2.RunSQLQuery("exec pCargaConceptosOCP @cmd=12")
        Dim lineas As Integer = ds.Tables(0).Rows.Count
        If lineas = 0 Then
            lineas = 1
        End If
        resultslistCSVOCP.PageSize = lineas
        resultslistCSVOCP.DataSource = ds
        resultslistCSVOCP.DataBind()


        ObjData2 = Nothing
    End Sub

    Private Sub imgdownloadF_Click(sender As Object, e As ImageClickEventArgs) Handles imgdownloadF.Click
        Dim FilePath = Server.MapPath("~/portalcfd/proveedores/cargaocp/") & "FORMATOCARGA.csv"

        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        End If
    End Sub
    Private Sub erroresListOCP_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles erroresListOCP.NeedDataSource
        Dim ObjData As New DataControl
        erroresList.DataSource = ObjData.FillDataSet("exec pCargaConceptosOCP @cmd=10, @cargaid='" & cargaidHidden.Value.ToString & "'")
        ObjData = Nothing
    End Sub

#End Region


    Private Sub ListOrdenesParciales_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles ListOrdenesParciales.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem

                Dim txtfechaEstEntOCP As RadDatePicker = CType(e.Item.FindControl("txtfechaEstEntOCP"), RadDatePicker)
                Dim edate1 = e.Item.DataItem("fecha_est_recepcion")

                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar una orden de compra parcial. ¿Desea continuar?');")

                Dim fechaEstEnt As Date = Date.ParseExact(edate1, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)

                If fechaEstEnt = "01/01/1900" Then
                    txtfechaEstEntOCP = New RadDatePicker
                Else
                    txtfechaEstEntOCP.SelectedDate = fechaEstEnt
                End If

                If e.Item.DataItem("estatusid") = 3 Then
                    btnDelete.Visible = False
                Else
                    btnDelete.Visible = True
                End If

        End Select

    End Sub
    Private Sub ListOrdenesParciales_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles ListOrdenesParciales.NeedDataSource
        Dim ObjData As New DataControl
        ListOrdenesParciales.DataSource = ObjData.FillDataSet("exec pOrdenCompra @cmd=13,  @ordenid='" & Request("id").ToString & "'")
        ObjData = Nothing
    End Sub
    Private Sub ListOrdenesParciales_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles ListOrdenesParciales.ItemCommand
        Select Case e.CommandName
            Case "cmdConceptosParciales"
                Dim commandArgs As String() = e.CommandArgument.ToString().Split(New Char() {","c})
                Dim IdCA As Integer = commandArgs(0)
                Dim ClaveCA As String = commandArgs(1)
                orderParcialIdHidden.Value = (IdCA)
                lblNombreOCP.Text = "-" + ClaveCA.ToString

                Dim ObjData As New DataControl
                Dim estatusid As Integer
                estatusid = ObjData.RunSQLScalarQuery("exec pOrdenCompra @cmd=24, @ordenId='" & Request("id").ToString & "', @ordencompraparcialid='" & orderParcialIdHidden.Value & "'")

                If estatusid = 3 Then
                    btnConfAlmacenado.Visible = False
                    btnConfAlmacenado.BackColor = Drawing.Color.Gray
                Else
                    btnConfAlmacenado.Visible = True
                    btnConfAlmacenado.BackColor = Drawing.Color.Green
                End If

                Call MuestraConceptosParciales()
            Case "cmdDeleteOCP"
                Call EliminaOrdenParcial(e.CommandArgument)
                Call CargaConceptos()
                Call CargaListaOrdenesParciales()
                Call MuestraConceptosParciales()
                lblNombreOCP.Text = " "
        End Select
    End Sub
    Private Sub EliminaOrdenParcial(ByVal idOrdenPracial As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=14, @id='" & idOrdenPracial.ToString & "'")
        ObjData = Nothing
    End Sub
    Private Sub CargaListaOrdenesParciales()
        Dim ObjData As New DataControl
        ListOrdenesParciales.DataSource = ObjData.FillDataSet("exec pOrdenCompra @cmd=13,  @ordenid='" & Request("id").ToString & "'")
        ListOrdenesParciales.DataBind()
        ObjData = Nothing
    End Sub
    Private Sub MuestraConceptosParciales()
        Dim obj As New DataControl
        ds = obj.FillDataSet("exec pOrdenCompra @cmd=18, @ordenid='" & orderParcialIdHidden.Value & "'")
        ListConceptosParciales.DataSource = ds
        ListConceptosParciales.DataBind()
        obj = Nothing
        ListConceptosParciales.Visible = True
    End Sub
    Protected Sub btnAddorderParcial_Click(sender As Object, e As EventArgs) Handles btnAddorderParcial.Click
        Dim ObjData2 As New DataControl

        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        Dim codigo As String = ""
        Dim cantidad_oc As Integer = 0
        Dim disponible_oc As Integer = 0
        Dim ordenId As String = ""
        Dim ordenIdParcial As Integer = 0
        Dim descripcion As String = ""
        Dim costo As Double = 0.00
        Dim moneda As String = ""
        Dim conceptoId As Long = 0
        Dim editClave As String = ""
        Dim productoId As Integer = 0

        Dim consecutivoId As Integer = 0

        consecutivoId = ObjData.RunSQLScalarQuery("exec pOrdenCompra @cmd=19, @ordenId='" & Request("id").ToString & "'")

        ObjData.RunSQLScalarQuery("exec pOrdenCompra @cmd=22, @ordenId='" & Request("id").ToString & "', @consecutivo='" & consecutivoId & "'")

        For Each row As GridDataItem In conceptosList.MasterTableView.Items
            ordenId = row.GetDataKeyValue("ordenId")
            cantidad_oc = row.GetDataKeyValue("cantidad")
            disponible_oc = row.GetDataKeyValue("disponible")
            codigo = row.GetDataKeyValue("codigo")
            descripcion = row.GetDataKeyValue("descripcion")
            costo = row.GetDataKeyValue("costo")
            moneda = row.GetDataKeyValue("moneda")
            Dim txtCantidad As RadNumericTextBox = DirectCast(row.FindControl("txtCantidad"), RadNumericTextBox)
            If Convert.ToDecimal(txtCantidad.Text.ToString) > 0 Then

                Dim p As New ArrayList
                p.Add(New SqlParameter("@cmd", 17))
                p.Add(New SqlParameter("@ordenId", ordenId.ToString))
                p.Add(New SqlParameter("@cantidad_oc", cantidad_oc.ToString))
                p.Add(New SqlParameter("@disponible_oc", disponible_oc.ToString))
                p.Add(New SqlParameter("@descripcion", descripcion.ToString))
                p.Add(New SqlParameter("@costo", costo.ToString))
                p.Add(New SqlParameter("@moneda", moneda.ToString))
                p.Add(New SqlParameter("@codigo", codigo.ToString))
                p.Add(New SqlParameter("@cantidad", txtCantidad.Text))
                ObjData.ExecuteNonQueryWithParams("pOrdenCompra", p)


            End If
        Next        '

        For Each row As GridDataItem In resultslistCSVOCP.MasterTableView.Items
            ordenId = row.GetDataKeyValue("ordenId")
            cantidad_oc = row.GetDataKeyValue("cantidad")
            codigo = row.GetDataKeyValue("codigo")
            productoId = row.GetDataKeyValue("productoid")
            descripcion = row.GetDataKeyValue("descripcion")
            moneda = row.GetDataKeyValue("moneda")
            If Convert.ToDecimal(cantidad_oc) > 0 Then

                Dim p As New ArrayList
                p.Add(New SqlParameter("@cmd", 25))
                p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                p.Add(New SqlParameter("@descripcion", descripcion.ToString))
                p.Add(New SqlParameter("@moneda", moneda.ToString))
                p.Add(New SqlParameter("@codigo", codigo.ToString))
                p.Add(New SqlParameter("@productoId", productoId.ToString))
                p.Add(New SqlParameter("@cantidad", cantidad_oc.ToString))
                ObjData.ExecuteNonQueryWithParams("pOrdenCompra", p)


            End If
        Next
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
        lblMensaje.Text = "Orden parcial creada."
        '

        Call CargaConceptos()
        Call CargaListaOrdenesParciales()
        panelCSVOCP.Visible = False
        panelErroresOCP.Visible = False

        resultslistCSVOCP.DataSource = Nothing
        resultslistCSVOCP.DataBind()
        erroresListOCP.DataSource = Nothing
        erroresListOCP.DataBind()

    End Sub
    Private Sub btnregresaList_Click(sender As Object, e As EventArgs) Handles btnregresaList.Click
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub
    Private Sub btneditaDatosGen_Click(sender As Object, e As System.EventArgs) Handles btneditaDatosGen.Click

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '

        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=16, @ordenId='" & Request("id").ToString & "', @proveedorId='" & proveedorid.SelectedValue.ToString & "', @userid='" & Session("userid").ToString & "', @comentarios='" & txtComentarios.Text & "',@fechaEstEnt='" & txtfechaEstEnt.SelectedDate.Value.ToShortDateString & "', @marcaid='" & proyectoid.SelectedValue.ToString & "'")
        ObjData = Nothing
        '
        lblMensajeEdit.Text = "Datos actualizados."
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
    End Sub
    Private Sub ListConceptosParciales_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles ListConceptosParciales.NeedDataSource
        Dim obj As New DataControl
        ds = obj.FillDataSet("exec pOrdenCompra @cmd=18, @ordenid='" & orderParcialIdHidden.Value.ToString & "'")
        ListConceptosParciales.DataSource = ds
        obj = Nothing
        ListConceptosParciales.Visible = True
    End Sub
    Private Sub btnConfAlmacenado_Click(sender As Object, e As EventArgs) Handles btnConfAlmacenado.Click

        Dim ObjData As New DataControl

        Dim Confirmar As Integer = orderParcialIdHidden.Value

        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=23, @ordenId='" & Request("id").ToString & "', @ordencompraparcialid='" & Confirmar & "', @userid='" & Session("userid").ToString & "'")
        ObjData.RunSQLQuery("exec pOrdenCompra @cmd=26, @ordenId='" & Request("id").ToString & "'")

        ObjData = Nothing


        btnConfAlmacenado.Visible = False
        btnConfAlmacenado.BackColor = Drawing.Color.Gray
        Call CargaListaOrdenesParciales()
    End Sub

End Class