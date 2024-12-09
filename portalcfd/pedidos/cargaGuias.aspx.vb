Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Telerik.Web.UI
Imports Telerik.Reporting.Processing
Imports System.IO
Imports System.Xml
Imports System.Linq

Public Class cargaGuias
    Inherits System.Web.UI.Page
    Dim datos As New DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblPedidoError.Visible = False
        If Not IsPostBack Then
            Dim dTable As New DataTable()
            productosList.DataSource = dTable

            Dim ObjCat As New DataControl
            ObjCat.Catalogo(cmbOrigen, "select isnull(id,0) as id ,isnull(nombre,'') as nombre  from tblOrigenGuias order by id", 0)
            ObjCat = Nothing

            ' lblUser.Text = Session("contacto").ToString()
            '
            ' btnAuth.Attributes.Add("onclick", "javascript:return confirm('Va a autorizar un pedido y enviarlo a almacén. ¿Desea continuar?');")
            ' btnPack.Attributes.Add("onclick", "javascript:return confirm('Va a marcar como empaquetado el producto. ¿Desea continuar?');")
            '
            '
            '
        End If
    End Sub

    Protected Sub productosList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles productosList.ItemCommand
    End Sub

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub
    Private Function GeneraPDF_Pedido(ByVal pedidoid As Long) As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim folio As Long = 0
        Dim almacen As String = ""
        Dim proyecto As String = ""
        Dim vendedor As String = ""

        Dim fecha As String = ""
        Dim cliente As String = ""
        Dim sucursal As String = ""
        Dim orden_compra As String = ""

        Dim ds As DataSet = New DataSet

        Dim cmd As New SqlCommand("EXEC pPedidos @cmd=14, @pedidoid='" & pedidoid.ToString & "'", conn)
        conn.Open()
        Dim rs As SqlDataReader
        rs = cmd.ExecuteReader()

        If rs.Read Then
            folio = rs("id")
            almacen = rs("almacen")
            proyecto = rs("proyecto")
            vendedor = rs("vendedor")
            fecha = rs("fecha")
            cliente = rs("cliente")
            sucursal = rs("sucursal")
            orden_compra = rs("orden_compra")
        End If
        rs.Close()

        conn.Close()
        conn.Dispose()
        conn = Nothing

        Dim reporte As New FormatosPDF.formato_pedidos_neogenis

        reporte.ReportParameters("plantillaId").Value = 5
        reporte.ReportParameters("pedidoId").Value = pedidoid

        reporte.ReportParameters("txtFolio").Value = folio
        reporte.ReportParameters("txtAlmacen").Value = almacen
        reporte.ReportParameters("txtProyecto").Value = proyecto
        reporte.ReportParameters("txtVendedor").Value = vendedor

        reporte.ReportParameters("txtFecha").Value = fecha
        reporte.ReportParameters("txtCliente").Value = cliente
        reporte.ReportParameters("txtSucursal").Value = sucursal
        reporte.ReportParameters("txtOC").Value = orden_compra
        reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))

        Dim totalPzas As String
        Dim objData As New DataControl
        totalPzas = objData.RunSQLScalarQuery("exec pPedidos @cmd=15, @pedidoid='" & pedidoid.ToString & "'")
        objData = Nothing
        '
        reporte.ReportParameters("txtTotalPiezas").Value = totalPzas.ToString
        '
        Return reporte

    End Function


    Private Function getNameXml()
        Dim ObjData As New DataControl
        Dim filename As String
        filename = ObjData.RunSQLScalarQueryString("exec pAsnAutomatico @cmd=3,@pedidoid=" & Request("id"))
        Return filename
    End Function

#Region "Carga de CVS de guias en Pedidos"

    Private Sub btnCargaPedidosCsv_Click(sender As Object, e As EventArgs) Handles btnCargaPedidosCsv.Click

        If fileUploadPedido.HasFile Then
            If System.IO.Path.GetExtension(fileUploadPedido.FileName).ToUpper = ".CSV" Then
                panelErroresPedidos.Visible = False

                Dim nombreArchivo As String = ""
                nombreArchivo = fileUploadPedido.PostedFile.FileName.ToString

                For i = 1 To 99999
                    If Not File.Exists(Server.MapPath("../pedidos/cargaGuias/") & nombreArchivo) Then
                        fileUploadPedido.SaveAs(Server.MapPath("../pedidos/cargaGuias/") & nombreArchivo)
                        Exit For
                    Else
                        nombreArchivo = i.ToString + "_" + fileUploadPedido.PostedFile.FileName.ToString
                    End If
                Next

                Dim registros As Integer
                Try
                    registros = System.IO.File.ReadAllLines(Server.MapPath("../pedidos/cargaGuias/") & nombreArchivo).Count
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
                cargaid = ObjData.RunSQLScalarQuery("exec pCargaGuiasPedidos @cmd=1, @userid='" & Session("userid").ToString & "', @archivo='" & nombreArchivo.ToString & "', @registros='" & registros.ToString & "'")
                cargaidHidden.Value = cargaid

                If registros = 0 Then
                    ObjData.RunSQLQuery("exec pCargaGuiasPedidos @cmd=9, @cargaid='" & cargaid.ToString & "")
                End If

                Dim progress As RadProgressContext = RadProgressContext.Current
                progress.SecondaryTotal = registros
                progress.Speed = "N/A"

                datos = New DataSet
                datos = ObjData.FillDataSet("exec pCargaGuiasPedidos @cmd=2")

                Dim tblguiasPedidos As New DataTable

                If datos.Tables.Count > 0 Then
                    tblguiasPedidos = datos.Tables(0)

                End If

                Dim j As Integer = 0
                Dim line As String = ""

                Dim reader As System.IO.StreamReader = New StreamReader(Server.MapPath("../pedidos/cargaGuias/") & nombreArchivo, System.Text.Encoding.Default)
                Do

                    j += 1

                    line = reader.ReadLine

                    If line = Nothing Then
                        Exit Do
                    End If

                    Dim ordencompra As String = ""
                    Dim ordencompraId As Integer = 0
                    Dim idpago As String = ""
                    Dim guia As String = ""
                    Dim cantidad As Integer = 0
                    Dim codigoId As String = ""
                    Dim codigoNoActivo As Integer = 0
                    Dim codigoConcepPrincipal As String = ""
                    Dim cantidadDisponible As Integer = 0

                    If j > 1 Then
                        ' CODIGO
                        Try
                            ordencompra = line.Split(",")(0)
                        Catch ex As Exception
                            ordencompra = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            idpago = line.Split(",")(1)
                        Catch ex As Exception
                            idpago = 0
                        End Try

                        Try
                            guia = line.Split(",")(2)
                        Catch ex As Exception
                            guia = 0
                        End Try

                        Try
                            Dim rowOrdencompra() As DataRow = tblguiasPedidos.Select("orden_compra = '" & LTrim(RTrim(ordencompra)).ToString & "'")
                            For Each row As DataRow In rowOrdencompra
                                ordencompra = row(0)
                            Next
                        Catch ex As Exception
                            ordencompra = 0
                        End Try

                        Try
                            Dim rowIdPago() As DataRow = tblguiasPedidos.Select("idpago = '" & LTrim(RTrim(idpago)).ToString & "'")
                            For Each row As DataRow In rowIdPago
                                idpago = row(0)
                            Next
                        Catch ex As Exception
                            idpago = 0
                        End Try

                        Try
                            Dim rowGuia() As DataRow = tblguiasPedidos.Select("guia = '" & LTrim(RTrim(guia)).ToString & "'")
                            For Each row As DataRow In rowGuia
                                guia = row(0)
                            Next
                        Catch ex As Exception
                            guia = 0
                        End Try

                        If ordencompra.Length > 0 And idpago.Length > 0 And guia.Length > 0 Then

                            If cmbOrigen.SelectedValue = 1 Then
                                ordencompraId = ObjData.RunSQLScalarQuery("exec pCargaGuiasPedidos @cmd=13, @ordenCompra='" & ordencompra & "'")
                            ElseIf cmbOrigen.SelectedValue = 2 Then
                                ordencompraId = ObjData.RunSQLScalarQuery("exec pCargaGuiasPedidos @cmd=14, @ordenCompra='" & ordencompra & "'")
                            End If

                            If ordencompraId = 0 Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@ordenCompra", ordencompra))
                                p.Add(New SqlParameter("@idPago", idpago))
                                p.Add(New SqlParameter("@guia", guia))
                                p.Add(New SqlParameter("@error", "La orden de compra " & ordencompra & " no está registrado."))
                                ObjData.ExecuteNonQueryWithParams("pCargaGuiasPedidos", p)
                                registros_error = registros_error + 1
                            Else
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 3))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@ordenCompra", ordencompra))
                                p.Add(New SqlParameter("@idPago", idpago))
                                p.Add(New SqlParameter("@guia", guia))
                                ObjData.ExecuteNonQueryWithParams("pCargaGuiasPedidos", p)
                                registros_correctos = registros_correctos + 1
                            End If
                        Else

                            msgerror = ""
                            If ordencompra.Length = 0 Then
                                msgerror = msgerror & "orden de compra es requerido."
                            End If

                            If idpago.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Cantidad es requerida."
                            End If

                            If guia.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Codigo no registrado."
                            End If

                            Dim p As New ArrayList
                            p.Add(New SqlParameter("@cmd", 7))
                            p.Add(New SqlParameter("@cargaid", cargaid))
                            p.Add(New SqlParameter("@ordenCompra", ordencompra))
                            p.Add(New SqlParameter("@idPago", idpago))
                            p.Add(New SqlParameter("@guia", guia))
                            p.Add(New SqlParameter("@error", msgerror))
                            'p.Add(New SqlParameter("@pedidoId", Request("id").ToString))
                            ObjData.ExecuteNonQueryWithParams("pCargaGuiasPedidos", p)
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
                    ObjData.ExecuteNonQueryWithParams("pCargaGuiasPedidos", p)
                End If

                If registros_error > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 9))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_error", registros_error))
                    ObjData.ExecuteNonQueryWithParams("pCargaGuiasPedidos", p)

                    panelErroresPedidos.Visible = True
                    erroresListPedidos.DataSource = ObjData.FillDataSet("exec pCargaGuiasPedidos @cmd=10, @cargaid='" & cargaid.ToString & "'")
                    erroresListPedidos.DataBind()

                End If

                If registros_correctos = 0 Then
                    ObjData.RunSQLQuery("exec pCargaGuiasPedidos @cmd=11, @cargaid='" & cargaid.ToString & "'")
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

        panelCSVPedidos.Visible = True
        btnAgregaConceptos.Visible = True
        Dim ObjData2 As New DataControl
        datos = ObjData2.FillDataSet("exec pCargaGuiasPedidos @cmd=5, @cargaid='" & cargaidHidden.Value & "'")
        ObjData2.RunSQLQuery("exec pCargaGuiasPedidos @cmd=12")
        Dim lineas As Integer = datos.Tables(0).Rows.Count
        If lineas = 0 Then
            lineas = 1
        End If
        resultslistCSVPedidos.PageSize = lineas
        resultslistCSVPedidos.DataSource = datos
        resultslistCSVPedidos.DataBind()

        ObjData2 = Nothing

    End Sub

    Private Sub btnAgregaConceptos_Click(sender As Object, e As EventArgs) Handles btnAgregaConceptos.Click

        Dim productoId As Long = 0
        Dim strOrdenCompra, strIdPago, strGuia As String
        'Dim dblCantidad, dlPrecio As Double
        Dim disponibles As Double = 0
        Dim valida As Integer = 0
        Dim mensaje As String = ""
        Dim CantidadP As Integer = 0

        Dim ObjData As New DataControl
        For Each row As GridDataItem In resultslistCSVPedidos.MasterTableView.Items
            'productoId = row.GetDataKeyValue("productoid")
            strOrdenCompra = row.GetDataKeyValue("orden_compra")
            strIdPago = row.GetDataKeyValue("idPago")
            strGuia = row.GetDataKeyValue("guia")

            If strOrdenCompra.Length > 0 And strIdPago.Length > 0 And strGuia.Length > 0 Then
                ObjData.RunSQLQuery("exec pPedidos @cmd=43, @orden_compra='" & strOrdenCompra & "', @idPago = '" & strIdPago & "', @origen = '" & cmbOrigen.SelectedValue.ToString & "', @guia='" & strGuia & "'")
                mensaje = "Datos guardados correctamente"
            Else
                mensaje = "Favor de validar los datos de la orden compra" & strOrdenCompra
            End If
        Next

        ObjData = Nothing
        '
        lblMensaje.Text = mensaje
        '
    End Sub

    Private Sub imgdownloadF_Click(sender As Object, e As ImageClickEventArgs) Handles imgdownloadF.Click
        Dim FilePath = Server.MapPath("~/portalcfd/pedidos/cargaGuias/") & "FORMATOCARGA.csv"

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
    Private Sub erroresListPedido_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles erroresListPedidos.NeedDataSource
        Dim ObjData As New DataControl
        erroresListPedidos.DataSource = ObjData.FillDataSet("exec pCargaGuiasPedidos @cmd=10, @cargaid='" & cargaidHidden.Value.ToString & "'")
        ObjData = Nothing
    End Sub

#End Region

End Class
