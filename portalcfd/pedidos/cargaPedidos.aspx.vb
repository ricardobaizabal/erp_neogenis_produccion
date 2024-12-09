Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports Telerik.Web.UI
Imports Telerik.Reporting.Processing
Imports System.IO
Imports System.Xml
Imports System.Linq
Public Class cargaPedidos
    Inherits System.Web.UI.Page
    Dim datos As New DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblPedidoError.Visible = False
        If Not IsPostBack Then
            Dim dTable As New DataTable()
            productosList.DataSource = dTable

            Dim ObjCat As New DataControl
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

        Dim marca As String = ""
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
            marca = rs("marca")
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

        reporte.ReportParameters("txtFecha").Value = marca
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
                    If Not File.Exists(Server.MapPath("../pedidos/cargaPedidosInternet/") & nombreArchivo) Then
                        fileUploadPedido.SaveAs(Server.MapPath("../pedidos/cargaPedidosInternet/") & nombreArchivo)
                        Exit For
                    Else
                        nombreArchivo = i.ToString + "_" + fileUploadPedido.PostedFile.FileName.ToString
                    End If
                Next

                Dim registros As Integer
                Try
                    registros = System.IO.File.ReadAllLines(Server.MapPath("../pedidos/cargaPedidosInternet/") & nombreArchivo).Count
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
                cargaid = ObjData.RunSQLScalarQuery("exec pCargaPedidosCsv @cmd=1, @userid='" & Session("userid").ToString & "', @archivo='" & nombreArchivo.ToString & "', @registros='" & registros.ToString & "'")
                cargaidHidden.Value = cargaid

                If registros = 0 Then
                    ObjData.RunSQLQuery("exec pCargaPedidosCsv @cmd=9, @cargaid='" & cargaid.ToString & "")
                End If

                Dim progress As RadProgressContext = RadProgressContext.Current
                progress.SecondaryTotal = registros
                progress.Speed = "N/A"

                datos = New DataSet
                datos = ObjData.FillDataSet("exec pCargaPedidosCsv @cmd=2")

                Dim tblcargaPedidos As New DataTable

                If datos.Tables.Count > 0 Then
                    tblcargaPedidos = datos.Tables(0)

                End If

                Dim j As Integer = 0
                Dim line As String = ""

                Dim reader As System.IO.StreamReader = New StreamReader(Server.MapPath("../pedidos/cargaPedidosInternet/") & nombreArchivo, System.Text.Encoding.Default)
                Do

                    j += 1

                    line = reader.ReadLine

                    If line = Nothing Then
                        Exit Do
                    End If

                    Dim ordencompra As String = ""
                    Dim ordencompraId As Integer = 0
                    Dim ordenCompraExiste As Integer = 0
                    Dim guia As String = ""
                    Dim cantidad As Integer = 0
                    Dim codigoId As String = ""
                    Dim codigoNoActivo As Integer = 0
                    Dim codigoConcepPrincipal As String = ""
                    Dim cantidadDisponible As Integer = 0
                    Dim precio As Decimal = 0
                    Dim sku As String = ""
                    Dim skuid As Integer = 0
                    Dim marca As String = ""


                    If j > 1 Then
                        ' CODIGO
                        Try
                            ordencompra = line.Split(",")(0)
                        Catch ex As Exception
                            ordencompra = ""
                        End Try

                        ' CANTIDAD
                        Try
                            cantidad = line.Split(",")(1)
                        Catch ex As Exception
                            cantidad = 0
                        End Try
                        ' PRECIO POR UNIDAD
                        Try
                            precio = line.Split(",")(2)
                        Catch ex As Exception
                            precio = 0
                        End Try

                        ' SKU
                        Try
                            sku = line.Split(",")(3)
                        Catch ex As Exception
                            sku = 0
                        End Try

                        ' marca
                        Try
                            marca = line.Split(",")(4)
                        Catch ex As Exception
                            marca = ""
                        End Try
                            Try

                            Dim rowOrdencompra() As DataRow = tblcargaPedidos.Select("ordenCompra = '" & LTrim(RTrim(ordencompra)).ToString & "'")
                            For Each row As DataRow In rowOrdencompra
                                    ordencompra = row(0)
                                Next
                            Catch ex As Exception
                                ordencompra = 0
                        End Try

                        Try
                            Dim rowCantidad() As DataRow = tblcargaPedidos.Select("cantidad = '" & LTrim(RTrim(cantidad)).ToString & "'")
                            For Each row As DataRow In rowCantidad
                                cantidad = row(0)
                            Next
                        Catch ex As Exception
                            cantidad = 0
                        End Try

                        Try
                            Dim rowPrecio() As DataRow = tblcargaPedidos.Select("precio = '" & LTrim(RTrim(precio)).ToString & "'")
                            For Each row As DataRow In rowPrecio
                                precio = row(0)
                            Next
                        Catch ex As Exception
                            precio = 0
                        End Try

                        Try
                            Dim rowMarca() As DataRow = tblcargaPedidos.Select("marca = '" & LTrim(RTrim(marca)).ToString & "'")
                            For Each row As DataRow In rowMarca
                                marca = row(0)
                            Next
                        Catch ex As Exception
                            marca = 0
                        End Try
                        If ordencompra.Length > 0 And sku.Length And cantidad > 0 And precio > 0 And marca.Length > 0 Then

                            skuid = ObjData.RunSQLScalarQuery("exec pCargaPedidosCsv @cmd=13, @sku='" & sku & "'")
                            cantidadDisponible = ObjData.RunSQLScalarQuery("exec pCargaPedidosCsv @cmd=17,@codigo='" & sku & "'")

                            codigoNoActivo = ObjData.RunSQLScalarQuery("exec pCargaPedidosCsv @cmd=14, @codigo='" & sku & "'")

                            ordenCompraExiste = ObjData.RunSQLScalarQuery("exec pCargaPedidosCsv @cmd=18, @ordenCompra='" & ordencompra & "'")

                            'If codigoNoActivo > 0 Then
                            '    ObjData.RunSQLScalarQuery("exec pCargaPedidosCsv @cmd=15, @id='" & codigoNoActivo & "'")
                            'End If

                            If skuid = 0 Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@ordenCompra", ordencompra))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@precio", precio))
                                p.Add(New SqlParameter("@sku", sku))
                                p.Add(New SqlParameter("@marca", marca))
                                p.Add(New SqlParameter("@error", "No existe el sku " & sku))
                                ObjData.ExecuteNonQueryWithParams("pCargaPedidosCsv", p)
                                registros_error = registros_error + 1

                            ElseIf cantidad > cantidadDisponible Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@ordenCompra", ordencompra))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@precio", precio))
                                p.Add(New SqlParameter("@sku", sku))
                                p.Add(New SqlParameter("@marca", marca))
                                p.Add(New SqlParameter("@error", "La cantidad de entrada es mayor a lo disponible del Sku" & sku))
                                ObjData.ExecuteNonQueryWithParams("pCargaPedidosCsv", p)
                                registros_error = registros_error + 1
                            ElseIf ordenCompraExiste > 0 Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@ordenCompra", ordencompra))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@precio", precio))
                                p.Add(New SqlParameter("@sku", sku))
                                p.Add(New SqlParameter("@marca", marca))
                                p.Add(New SqlParameter("@error", "La orden de compra " & ordencompra & " esta registrada en otro pedido"))
                                ObjData.ExecuteNonQueryWithParams("pCargaPedidosCsv", p)
                                registros_error = registros_error + 1

                            Else
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 3))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@ordenCompra", ordencompra))
                                p.Add(New SqlParameter("@cantidad", cantidad))
                                p.Add(New SqlParameter("@precio", precio))
                                p.Add(New SqlParameter("@sku", sku))
                                p.Add(New SqlParameter("@marca", marca))
                                ObjData.ExecuteNonQueryWithParams("pCargaPedidosCsv", p)
                                registros_correctos = registros_correctos + 1
                            End If
                        Else

                            msgerror = ""
                            If ordencompra.Length = 0 Then
                                msgerror = msgerror & "orden de compra es requerido."
                            End If

                            If sku.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Codigo es requerido."
                            End If

                            If cantidad = 0 Then
                                msgerror = msgerror & vbCrLf & "Cantidad es requerido."
                            End If

                            If marca.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "marca es requerido."
                            End If
                            If precio = 0 Then
                                msgerror = msgerror & vbCrLf & "Precio es requerido."
                            End If
                            Dim p As New ArrayList
                            p.Add(New SqlParameter("@cmd", 7))
                            p.Add(New SqlParameter("@cargaid", cargaid))
                            p.Add(New SqlParameter("@ordenCompra", ordencompra))
                            p.Add(New SqlParameter("@cantidad", cantidad))
                            p.Add(New SqlParameter("@precio", precio))
                            p.Add(New SqlParameter("@marca", marca))
                            p.Add(New SqlParameter("@error", msgerror))
                            'p.Add(New SqlParameter("@pedidoId", Request("id").ToString))
                            ObjData.ExecuteNonQueryWithParams("pCargaPedidosCsv", p)
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
                    ObjData.ExecuteNonQueryWithParams("pCargaPedidosCsv", p)
                End If

                If registros_error > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 9))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_error", registros_error))
                    ObjData.ExecuteNonQueryWithParams("pCargaPedidosCsv", p)

                    panelErroresPedidos.Visible = True
                    erroresListPedidos.DataSource = ObjData.FillDataSet("exec pCargaPedidosCsv @cmd=10, @cargaid='" & cargaid.ToString & "'")
                    erroresListPedidos.DataBind()

                End If

                If registros_correctos = 0 Then
                    ObjData.RunSQLQuery("exec pCargaPedidosCsv @cmd=11, @cargaid='" & cargaid.ToString & "'")
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
        datos = ObjData2.FillDataSet("exec pCargaPedidosCsv @cmd=5, @cargaid='" & cargaidHidden.Value & "'")
        ObjData2.RunSQLQuery("exec pCargaPedidosCsv @cmd=12")
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

        'Dim productoId As Long = 0

        Dim strCantidad As Single
        Dim strPrecio As Decimal
        Dim strOrdenCompra, strSku, strMarca As String

        'Dim dblCantidad, dlPrecio As Double
        Dim disponibles As Double = 0
        Dim valida As Integer = 0
        Dim mensaje As String = ""
        Dim CantidadP As Integer = 0

        Dim ObjData As New DataControl
        For Each row As GridDataItem In resultslistCSVPedidos.MasterTableView.Items
            'productoId = row.GetDataKeyValue("productoid")
            strOrdenCompra = row.GetDataKeyValue("ordenCompra")
            strSku = row.GetDataKeyValue("sku")
            strCantidad = row.GetDataKeyValue("cantidad")
            strPrecio = row.GetDataKeyValue("precio")
            strMarca = row.GetDataKeyValue("marca")




            If strOrdenCompra.Length > 0 And strSku.Length And strCantidad > 0 And strPrecio > 0 Then
                ObjData.RunSQLQuery("exec pPedidos @cmd=44, @orden_compra='" & strOrdenCompra & "', @userid='" & Session("userid") & "', @cantidad = '" & strCantidad & "', @sku = '" & strSku & "', @marca = '" & strMarca & "', @precio='" & strPrecio & "'")
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
        Dim FilePath = Server.MapPath("~/portalcfd/pedidos/cargaPedidosInternet/") & "FORMATOCARGA.csv"

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
        erroresListPedidos.DataSource = ObjData.FillDataSet("exec pCargaPedidosCsv @cmd=10, @cargaid='" & cargaidHidden.Value.ToString & "'")
        ObjData = Nothing
    End Sub

#End Region

End Class
