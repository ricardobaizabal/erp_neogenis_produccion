Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports Telerik.Reporting.Processing
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XPath.XPathItem
Imports System.Xml.XPath.XPathNavigator
Imports System.Xml.Serialization
Imports System.Security.Cryptography.X509Certificates
Imports erp_neogenis.CfdiNomina
Imports erp_neogenis.complementoNomina
Imports FirmaSAT.Sat
Imports ThoughtWorks.QRCode.Codec
Imports ThoughtWorks.QRCode.Codec.Util
Public Class TimbradoNomina
    Inherits System.Web.UI.Page
    Private archivoLlavePrivada As String = ""
    Private contrasenaLlavePrivada As String = ""
    Private archivoCertificado As String = ""
    Private ComprobanteXML As New Comprobante
    Private docXML As String = ""
    Dim listErrores As New List(Of String)
    Dim resultado As String = ""
    Private archivoSys As String = ""
    Private total_timbrados As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Call CargaLugarExpedicion()
            Call MuestraCorridas()

            calFechaInicial.SelectedDate = Date.Now
            calFechaFinal.SelectedDate = Date.Now

            Dim ObjData As New DataControl
            ObjData.Catalogo(ddlPeriodoPago, "select id, nombre from tblPeriodoPago", 0)
            'ObjData.Catalogo(ddlMoneda, "select id, nombre from tblMoneda", 1)
            ObjData = Nothing
            btnTinbrarNomina.Visible = False

        End If

    End Sub

    Private Sub MuestraCorridas()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCorridasNomina @cmd=5", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        corridasList.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        corridasList.DataSource = ds
        corridasList.DataBind()
    End Sub

    Private Sub MuestraDatosCorrida()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCorridasNomina @cmd=6, @corridaid='" & corridaId.Value.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read() Then

                Dim edate1 = rs("fecha_inicial").ToString
                Dim fecha_inicial As Date = Date.ParseExact(edate1, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)
                Dim edate2 = rs("fecha_final").ToString
                Dim fecha_final As Date = Date.ParseExact(edate2, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)

                ddlPeriodoPago.SelectedValue = rs("periodo_pagoid")
                calFechaInicial.SelectedDate = fecha_inicial
                calFechaFinal.SelectedDate = fecha_final
                txtDiasLaborados.Text = rs("dias_laborados")
                ddlPeriodoPago.Enabled = False
                calFechaInicial.Enabled = False
                calFechaFinal.Enabled = False
                txtDiasLaborados.Enabled = False
                btnGeneraCorrida.Enabled = False
            End If
            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub

    Private Function CargaLugarExpedicion() As String
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCliente @cmd=3", conn)
        Dim LugarExpedicion As String = ""
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read() Then
                LugarExpedicion = rs("expedicionLinea3")
            End If

            rs.Close()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        '
        Return LugarExpedicion
        '
    End Function

    Function GetEmployees() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pEmpleados  @cmd=6, @periodopagoid='" & ddlPeriodoPago.SelectedValue.ToString & "', @corridaid='" & corridaId.Value.ToString & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return ds

    End Function

    Private Sub GetEmployeesTimbrados()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pEmpleados  @cmd=7, @periodopagoid='" & ddlPeriodoPago.SelectedValue.ToString & "', @corridaid='" & corridaId.Value.ToString & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        employeeslist.DataSource = ds
        employeeslist.DataBind()

    End Sub

    Private Sub ddlPeriodoPago_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlPeriodoPago.SelectedIndexChanged
        '
        calFechaFinal.SelectedDate = Date.Now
        If ddlPeriodoPago.SelectedItem.Text = "Semanal" Then
            calFechaInicial.SelectedDate = CDate(calFechaFinal.SelectedDate).AddDays(-7)
            If calFechaFinal.SelectedDate > calFechaInicial.SelectedDate Then
                txtDiasLaborados.Text = DateDiff(DateInterval.Day, CDate(calFechaInicial.SelectedDate), CDate(calFechaFinal.SelectedDate))
            Else
                txtDiasLaborados.Text = "0"
            End If
        ElseIf ddlPeriodoPago.SelectedItem.Text = "Catorcenal" Then
            calFechaInicial.SelectedDate = CDate(calFechaFinal.SelectedDate).AddDays(-14)
            If calFechaFinal.SelectedDate > calFechaInicial.SelectedDate Then
                txtDiasLaborados.Text = DateDiff(DateInterval.Day, CDate(calFechaInicial.SelectedDate), CDate(calFechaFinal.SelectedDate))
            Else
                txtDiasLaborados.Text = "0"
            End If
        ElseIf ddlPeriodoPago.SelectedItem.Text = "Quincenal" Then
            calFechaInicial.SelectedDate = CDate(calFechaFinal.SelectedDate).AddDays(-15)
            If calFechaFinal.SelectedDate > calFechaInicial.SelectedDate Then
                txtDiasLaborados.Text = DateDiff(DateInterval.Day, CDate(calFechaInicial.SelectedDate), CDate(calFechaFinal.SelectedDate))
            Else
                txtDiasLaborados.Text = "0"
            End If
        ElseIf ddlPeriodoPago.SelectedItem.Text = "Mensual" Then
            calFechaInicial.SelectedDate = CDate(calFechaFinal.SelectedDate).AddDays(-30)
            If calFechaFinal.SelectedDate > calFechaInicial.SelectedDate Then
                txtDiasLaborados.Text = DateDiff(DateInterval.Day, CDate(calFechaInicial.SelectedDate), CDate(calFechaFinal.SelectedDate))
            Else
                txtDiasLaborados.Text = "0"
            End If
        End If
        ''
    End Sub

    Private Sub btnTinbrarNomina_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTinbrarNomina.Click

        'Dim listErrores As New List(Of String)

        If calFechaInicial.SelectedDate > calFechaFinal.SelectedDate Then
            lblMensaje.Text = "*La fecha inicial no puede ser mayor a la fecha final"
            Return
        End If

        lblMensaje.Text = ""

        For Each dataItem As Telerik.Web.UI.GridDataItem In employeeslist.MasterTableView.Items
            Dim chkid As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkid"), System.Web.UI.WebControls.CheckBox)
            If chkid.Checked = True Then

                Dim empleadoid As String = dataItem.GetDataKeyValue("id").ToString()
                Dim nombre_empleado As String = dataItem.Item("nombre").Text.ToString
                Dim numero_empleado As String = dataItem.Item("numero_empleado").Text.ToString
                Dim rfc As String = dataItem.Item("rfc").Text.ToString

                If numero_empleado = "&nbsp;" Or numero_empleado = "" Then
                    listErrores.Add("- El empleado " & nombre_empleado & " no cuenta con número de empleado.")
                    chkid.Checked = False
                End If
                If rfc = "&nbsp;" Or rfc = "" Then
                    listErrores.Add("- El empleado " & nombre_empleado & " no se le capturó RFC.")
                    chkid.Checked = False
                End If

            End If
        Next

        For Each dataItem As Telerik.Web.UI.GridDataItem In employeeslist.MasterTableView.Items

            Dim chkid As System.Web.UI.WebControls.CheckBox = DirectCast(dataItem.FindControl("chkid"), System.Web.UI.WebControls.CheckBox)

            If chkid.Checked = True Then
                If corridaId.Value = 0 Then
                    listErrores.Add("- No se generó un id corrida.")
                    windowDeducciones.VisibleOnPageLoad = False
                    windowPercepciones.VisibleOnPageLoad = False
                    windowIncapacidades.VisibleOnPageLoad = False
                    windowHorasExtra.VisibleOnPageLoad = False
                    RadWindow1.VisibleOnPageLoad = True
                    Return
                End If

                Dim empleadoid As String = dataItem.GetDataKeyValue("id").ToString()
                Dim nombre_empleado As String = dataItem.Item("nombre").Text.ToString

                Dim ObjData As New DataControl
                Dim totalPercepciones As Decimal = 0
                Dim totalDeducciones As Decimal = 0
                Dim totalISR As Decimal = 0
                Dim totalIncapacidades As Decimal = 0
                Dim totalDescuentos As Decimal = 0
                Dim totalHorasExtra As Decimal = 0

                totalPercepciones = ObjData.RunSQLScalarQueryDecimal("select isnull(sum(monto),0) as subtotal from tblPercepcionEmpleadoCorrida where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")
                totalDeducciones = ObjData.RunSQLScalarQueryDecimal("select isnull(sum(monto),0) as subtotal from tblDeduccionEmpleadoCorrida where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")
                totalISR = ObjData.RunSQLScalarQueryDecimal("select isnull(sum(monto),0) as subtotal from tblDeduccionEmpleadoCorrida a inner join tblDeduccion b on a.deduccionid=b.id where b.clave='002' and corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")
                totalIncapacidades = ObjData.RunSQLScalarQueryDecimal("select isnull(sum(monto),0) as subtotal from tblIncapacidadEmpleadoCorrida where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")
                totalHorasExtra = ObjData.RunSQLScalarQueryDecimal("select isnull(sum(monto),0) as subtotal from tblHorasExtraEmpleadoCorrida where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")
                totalDescuentos = (totalDeducciones - totalISR + totalIncapacidades)

                If totalPercepciones > 0 Then
                    'If totalISR > 0 Then
                    Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                    Dim cmd As New SqlCommand("EXEC pEmpleados @cmd=2, @empleadoid='" & empleadoid.ToString.Trim & "'", conn)
                    Dim con As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                    Dim comando As New SqlCommand("EXEC pCliente @cmd=3", con)

                    Try

                        conn.Open()
                        con.Open()

                        Dim rs, reader As SqlDataReader

                        rs = cmd.ExecuteReader()
                        reader = comando.ExecuteReader()


                        If rs.Read() And reader.Read() Then
                            '
                            '   Rutina de generación de XML CFDI Versión 3.2
                            '
                            ComprobanteXML = New Comprobante()
                            ComprobanteXML.version = "3.2"
                            ComprobanteXML.fecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"))
                            ComprobanteXML.formaDePago = "PAGO EN UNA SOLA EXHIBICION"

                            ComprobanteXML.subTotal = totalPercepciones
                            ComprobanteXML.total = (totalPercepciones - totalDescuentos - totalISR + totalHorasExtra)
                            If totalDescuentos > 0 Then
                                ComprobanteXML.motivoDescuento = "Deducciones nómina"
                                ComprobanteXML.descuento = totalDescuentos
                                ComprobanteXML.descuentoSpecified = True
                            End If
                            '
                            '   Si es factura en dólares cambia la moneda y tipo de cambio
                            '
                            'If (ddlMoneda.SelectedValue = 1) Then
                            ComprobanteXML.Moneda = "MXN"
                            'ElseIf ddlMoneda.SelectedValue = 2 Then
                            'ComprobanteXML.Moneda = "USD"
                            'ComprobanteXML.TipoCambio = txtTipoCambio.Text
                            'End If
                            '
                            ComprobanteXML.tipoDeComprobante = ComprobanteTipoDeComprobante.egreso
                            ComprobanteXML.metodoDePago = rs("metodopago")
                            ComprobanteXML.LugarExpedicion = CargaLugarExpedicion()
                            '
                            '   Agrega los datos del emisor
                            '
                            Call ConfiguraEmisor()
                            '
                            '
                            '   Asigna los datos del receptor
                            '
                            Call ConfiguraReceptor(empleadoid)
                            '
                            '   Agrega los conceptos de la factura
                            '
                            Dim ds As New DataSet()
                            Dim Concepto As ComprobanteConcepto
                            Dim lstConceptos As New List(Of ComprobanteConcepto)

                            ds = ObjData.FillDataSet("select a.percepcionid,isnull(a.monto,0) as monto,b.clave,b.descripcion from tblPercepcionEmpleadoCorrida a inner join tblPercepcion b on a.percepcionid=b.id where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

                            Dim j As Integer = 0
                            If ds.Tables(0).Rows.Count > 0 Then
                                For j = 0 To ds.Tables(0).Rows.Count - 1
                                    If Convert.ToDecimal(ds.Tables(0).Rows.Item(j)("monto").ToString) > 0 Then
                                        Concepto = New ComprobanteConcepto
                                        Concepto.cantidad = "1"
                                        Concepto.unidad = "Servicio"
                                        Concepto.descripcion = ds.Tables(0).Rows.Item(j)("descripcion").ToString
                                        Concepto.importe = ds.Tables(0).Rows.Item(j)("monto").ToString
                                        Concepto.valorUnitario = ds.Tables(0).Rows.Item(j)("monto").ToString
                                        lstConceptos.Add(Concepto)
                                    End If
                                Next
                                '
                                If lstConceptos.Count > 0 Then
                                    ComprobanteXML.Conceptos = lstConceptos.ToArray()
                                End If
                                '
                            Else
                                listErrores.Add("- Error inesperado al agregar Conceptos a documento XML.")
                            End If
                            '
                            '   Asigna Serie y Folio
                            '
                            Call AsignaSerieFolio(empleadoid.ToString.Trim)
                            '
                            '
                            '   Agrega impuestos
                            '
                            '
                            Dim ImpuestoRetenidoISR As New ComprobanteImpuestosRetencion()
                            ImpuestoRetenidoISR.importe = totalISR
                            ImpuestoRetenidoISR.impuesto = ComprobanteImpuestosRetencionImpuesto.ISR

                            Dim impuestos As New ComprobanteImpuestos()
                            impuestos.Retenciones = New ComprobanteImpuestosRetencion() {ImpuestoRetenidoISR}
                            impuestos.totalImpuestosRetenidos = totalISR
                            impuestos.totalImpuestosRetenidosSpecified = True
                            ComprobanteXML.Impuestos = impuestos
                            '
                            archivoSys = Server.MapPath("cfd_storage\nomina\nomina_") & serie.Value.ToString & folio.Value.ToString & ".xml"
                            '
                            '   Crea Complemento Nómina
                            '
                            Call ConfiguraComplemento(empleadoid)
                            '
                            '   Crear cadena original
                            '
                            Dim otrasRutinas As New RutinasNominaCFDI32
                            Dim cadenaOriginal As String = otrasRutinas.GenerarCadenaV3(ComprobanteXML)
                            '
                            '   Generar Sello digital
                            '
                            '   Obtiene llave y contraseña
                            '
                            Call obtienellave()
                            '
                            '
                            '   Lectura del certificado de sello digital
                            '
                            Dim cCert As New X509Certificate()
                            Dim strSerial As String = String.Empty
                            cCert = X509Certificate.CreateFromCertFile(archivoCertificado)
                            '
                            Dim i As Integer
                            Dim sn As String = cCert.GetSerialNumberString()
                            For i = 0 To sn.Length - 1
                                If i Mod 2 <> 0 Then
                                    strSerial = strSerial & sn.Substring(i, 1)
                                End If
                            Next i

                            ComprobanteXML.noCertificado = strSerial
                            ComprobanteXML.certificado = Convert.ToBase64String(cCert.GetRawCertData())
                            ComprobanteXML.sello = otrasRutinas.GenerarSelloDigital(archivoLlavePrivada, contrasenaLlavePrivada, cadenaOriginal)
                            '
                            '   Guarda XML sin timbrar
                            '
                            ComprobanteXML.SaveToFile(archivoSys, System.Text.Encoding.UTF8)
                            '
                            '   Realiza Timbrado
                            '
                            If TimbradoFactureHoy(empleadoid.ToString.Trim) = True Then
                                '
                                total_timbrados = total_timbrados + 1
                                '
                            Else
                                '
                                '   Marca el cfd como no timbrado
                                '
                                ObjData.RunSQLQuery("update tblMisFolios set utilizado=null, fecha_utilizacion=null where serie='" & serie.Value.ToString & "' and folio='" & folio.Value.ToString & "' and utilizado=1")
                                ObjData.RunSQLQuery("update tblNominaEmpleados set serie=null, folio=null where serie='" & serie.Value.ToString & "' and folio='" & folio.Value.ToString & "'")
                                '
                            End If
                        End If

                        rs.Close()
                        reader.Close()
                        ObjData = Nothing

                    Catch ex As Exception
                        Response.Write(ex.Message.ToString)
                    Finally
                        conn.Close()
                        con.Close()
                        conn.Dispose()
                        con.Dispose()
                    End Try
                    'Else
                    '    listErrores.Add("- El empleado " & nombre_empleado & " no se le capturó impuestos retenidos.")
                    'End If
                Else
                    listErrores.Add("- El empleado " & nombre_empleado & " no se le capturó percepciones.")
                End If
            End If
        Next

        '
        listErrores.Add("TOTAL DE DOCUMENTOS TIMBRADOS: " & total_timbrados)
        Dim mensaje = String.Join(Environment.NewLine, listErrores.ToArray())
        txtErrores.Text = mensaje
        RadWindow1.VisibleOnPageLoad = True
        '
        Call GetEmployeesTimbrados()
        '
        windowIncapacidades.VisibleOnPageLoad = False
        windowHorasExtra.VisibleOnPageLoad = False
        windowDeducciones.VisibleOnPageLoad = False
        windowPercepciones.VisibleOnPageLoad = False

    End Sub

    Private Function GetCFD(ByVal serie As String, ByVal folio As String) As Long

        Dim cfdId As Long = 0

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("select isnull(id,0) as cfdid from tblNominaEmpleados where serie='" & serie.ToString & "' and folio='" & folio.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                cfdId = rs("cfdid")

            End If

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            '

        Finally

            conn.Close()
            conn.Dispose()

        End Try

        Return cfdId

    End Function

    Private Sub generacbb(ByVal serie As String, ByVal folio As String)
        Dim cadena As String = ""
        Dim UUID As String = ""
        Dim rfcE As String = ""
        Dim rfcR As String = ""
        Dim total As String = ""

        '
        '   Obtiene datos del cfdi para construir string del CBB
        '

        '
        rfcE = GetXmlAttribute(Server.MapPath("cfd_storage\nomina") & "\" & "nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "rfc", "cfdi:Emisor")
        rfcR = GetXmlAttribute(Server.MapPath("cfd_storage\nomina") & "\" & "nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "rfc", "cfdi:Receptor")
        total = GetXmlAttribute(Server.MapPath("cfd_storage\nomina") & "\" & "nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "total", "cfdi:Comprobante")
        UUID = GetXmlAttribute(Server.MapPath("cfd_storage\nomina") & "\" & "nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        '
        Dim fmt As String = "0000000000.000000"
        Dim totalDec As Decimal = CType(total, Decimal)
        total = totalDec.ToString(fmt)
        '
        cadena = "?re=" & rfcE.ToString & "&rr=" & rfcR.ToString & "&tt=" & total.ToString & "&id=" & UUID.ToString
        '
        'Response.Write(cadena)
        '   Genera gráfico
        '
        Dim qrCodeEncoder As New QRCodeEncoder
        qrCodeEncoder.QRCodeEncodeMode = qrCodeEncoder.ENCODE_MODE.BYTE
        qrCodeEncoder.QRCodeScale = 4
        qrCodeEncoder.QRCodeVersion = 8
        qrCodeEncoder.QRCodeErrorCorrect = qrCodeEncoder.ERROR_CORRECTION.Q
        Dim image As Drawing.Image

        image = qrCodeEncoder.Encode(cadena)
        image.Save(Server.MapPath("~/portalCFD/cbb_nomina") & "\" & serie.ToString & folio.ToString & ".png", System.Drawing.Imaging.ImageFormat.Png)
        ''
    End Sub

    Private Function Num2Text(ByVal value As Decimal) As String
        Select Case value
            Case 0 : Num2Text = "CERO"
            Case 1 : Num2Text = "UN"
            Case 2 : Num2Text = "DOS"
            Case 3 : Num2Text = "TRES"
            Case 4 : Num2Text = "CUATRO"
            Case 5 : Num2Text = "CINCO"
            Case 6 : Num2Text = "SEIS"
            Case 7 : Num2Text = "SIETE"
            Case 8 : Num2Text = "OCHO"
            Case 9 : Num2Text = "NUEVE"
            Case 10 : Num2Text = "DIEZ"
            Case 11 : Num2Text = "ONCE"
            Case 12 : Num2Text = "DOCE"
            Case 13 : Num2Text = "TRECE"
            Case 14 : Num2Text = "CATORCE"
            Case 15 : Num2Text = "QUINCE"
            Case Is < 20 : Num2Text = "DIECI" & Num2Text(value - 10)
            Case 20 : Num2Text = "VEINTE"
            Case Is < 30 : Num2Text = "VEINTI" & Num2Text(value - 20)
            Case 30 : Num2Text = "TREINTA"
            Case 40 : Num2Text = "CUARENTA"
            Case 50 : Num2Text = "CINCUENTA"
            Case 60 : Num2Text = "SESENTA"
            Case 70 : Num2Text = "SETENTA"
            Case 80 : Num2Text = "OCHENTA"
            Case 90 : Num2Text = "NOVENTA"
            Case Is < 100 : Num2Text = Num2Text(Int(value \ 10) * 10) & " Y " & Num2Text(value Mod 10)
            Case 100 : Num2Text = "CIEN"
            Case Is < 200 : Num2Text = "CIENTO " & Num2Text(value - 100)
            Case 200, 300, 400, 600, 800 : Num2Text = Num2Text(Int(value \ 100)) & "CIENTOS"
            Case 500 : Num2Text = "QUINIENTOS"
            Case 700 : Num2Text = "SETECIENTOS"
            Case 900 : Num2Text = "NOVECIENTOS"
            Case Is < 1000 : Num2Text = Num2Text(Int(value \ 100) * 100) & " " & Num2Text(value Mod 100)
            Case 1000 : Num2Text = "MIL"
            Case Is < 2000 : Num2Text = "MIL " & Num2Text(value Mod 1000)
            Case Is < 1000000 : Num2Text = Num2Text(Int(value \ 1000)) & " MIL"
                If value Mod 1000 Then Num2Text = Num2Text & " " & Num2Text(value Mod 1000)
            Case 1000000 : Num2Text = "UN MILLON"
            Case Is < 2000000 : Num2Text = "UN MILLON " & Num2Text(value Mod 1000000)
            Case Is < 1000000000000.0# : Num2Text = Num2Text(Int(value / 1000000)) & " MILLONES "
                If (value - Int(value / 1000000) * 1000000) Then Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000) * 1000000)
            Case 1000000000000.0# : Num2Text = "UN BILLON"
            Case Is < 2000000000000.0# : Num2Text = "UN BILLON " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
            Case Else : Num2Text = Num2Text(Int(value / 1000000000000.0#)) & " BILLONES"
                If (value - Int(value / 1000000000000.0#) * 1000000000000.0#) Then Num2Text = Num2Text & " " & Num2Text(value - Int(value / 1000000000000.0#) * 1000000000000.0#)
        End Select
    End Function

    Private Function CadenaOriginalComplemento(ByVal serie As String, ByVal folio As Long) As String

        '
        '   Obtiene los valores del timbre de respuesta
        '
        Dim selloSAT As String = ""
        Dim noCertificadoSAT As String = ""
        Dim selloCFD As String = ""
        Dim fechaTimbrado As String = ""
        Dim UUID As String = ""
        Dim Version As String = ""
        '
        '
        Dim s_RutaRespuestaPAC As String = Server.MapPath("cfd_storage") & "\" & "link_timbre_nomina_" & serie.ToString & folio.ToString & ".xml"
        Dim respuestaPAC As New Timbrado()
        Dim objStreamReader As New StreamReader(s_RutaRespuestaPAC)
        Dim Xml As New XmlSerializer(respuestaPAC.[GetType]())
        respuestaPAC = DirectCast(Xml.Deserialize(objStreamReader), Timbrado)
        objStreamReader.Close()

        '
        'Crear el objeto timbre para asignar los valores de la respuesta PAC
        fechaTimbrado = respuestaPAC.Items(0).Informacion(0).Timbre(0).FechaTimbrado
        noCertificadoSAT = respuestaPAC.Items(0).Informacion(0).Timbre(0).noCertificadoSAT.ToString
        selloCFD = respuestaPAC.Items(0).Informacion(0).Timbre(0).selloCFD.ToString
        selloSAT = respuestaPAC.Items(0).Informacion(0).Timbre(0).selloSAT.ToString
        UUID = respuestaPAC.Items(0).Informacion(0).Timbre(0).UUID.ToString
        Version = respuestaPAC.Items(0).Informacion(0).Timbre(0).version.ToString
        '
        Dim cadena As String = ""
        cadena = "||" & Version & "|" & UUID & "|" & fechaTimbrado & "|" & selloCFD & "|" & noCertificadoSAT & "||"
        Return cadena
        ''
    End Function

    Private Sub AsignaSerieFolio(ByVal empleadoid As String)
        '
        '   Obtiene serie y folio
        '
        Dim aprobacion As String = ""
        Dim annioaprobacion As String = ""

        Dim SQLUpdate As String = ""

        SQLUpdate = "exec pCorridasNomina @cmd=4, @empleadoid='" & empleadoid.ToString & "', @tipodocumentoid=15, @corridaid='" & corridaId.Value.ToString & "'"

        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand(SQLUpdate, connF)
        Try

            connF.Open()

            Dim rs As SqlDataReader
            rs = cmdF.ExecuteReader()

            If rs.Read Then
                serie.Value = rs("serie").ToString
                folio.Value = rs("folio").ToString
            End If
        Catch ex As Exception
            '
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        ComprobanteXML.folio = folio.Value
        '
        ''
    End Sub

    Private Sub obtienellave()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pCFD @cmd=19, @clienteid='" & Session("clienteid").ToString & "'", conn)
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                archivoLlavePrivada = Server.MapPath("~/portalcfd/llave") & "\" & rs("archivo_llave_privada")
                contrasenaLlavePrivada = rs("contrasena_llave_privada")
                archivoCertificado = Server.MapPath("~/portalcfd/certificados") & "\" & rs("archivo_certificado")
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing

        End Try
    End Sub

    Private Sub ConfiguraEmisor()
        '
        '   Obtiene datos del emisor
        '
        '
        '   Datos del Emisor
        '
        Dim Emisor As New ComprobanteEmisor()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pCliente @cmd=5", conn)
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                Emisor.rfc = rs("fac_rfc")
                Emisor.nombre = rs("razonsocial")

                'Domicilio Fiscal
                Dim domicilioEmisor As New t_UbicacionFiscal()
                domicilioEmisor.calle = rs("fac_calle")
                domicilioEmisor.codigoPostal = rs("fac_cp")
                domicilioEmisor.colonia = rs("fac_colonia")
                domicilioEmisor.estado = rs("fac_estado")
                domicilioEmisor.localidad = rs("fac_municipio")
                domicilioEmisor.municipio = rs("fac_municipio")
                domicilioEmisor.noExterior = rs("fac_num_ext")
                If rs("fac_num_int").ToString.Length > 0 Then
                    domicilioEmisor.noInterior = rs("fac_num_int")
                End If
                domicilioEmisor.pais = "México"

                'Expedido En (Aplica cuando se trata de una sucursal)
                'Dim expedidoEn As New t_Ubicacion()
                'expedidoEn.calle = "Jacinto Lopez"
                'expedidoEn.codigoPostal = "85000"
                'expedidoEn.colonia = "Cortinas"
                'expedidoEn.estado = "Sonora"
                'expedidoEn.localidad = "Obregon"
                'expedidoEn.municipio = "Cajeme"
                'expedidoEn.noExterior = "100"
                'expedidoEn.noInterior = "A"
                'expedidoEn.pais = "México"
                'Asignar el expedidoEn
                'Emisor.ExpedidoEn = expedidoEn
                'Asignar el domicilio al emisor
                Emisor.DomicilioFiscal = domicilioEmisor
                '
                '
                '   Régimen fiscal. Es obligatorio y debe tener al menos 1
                '
                Dim regimenFiscal1 As New ComprobanteEmisorRegimenFiscal()
                regimenFiscal1.Regimen = rs("regimen")
                '
                '   Asignar el regimen fiscal dentro del emisor
                Emisor.RegimenFiscal = New ComprobanteEmisorRegimenFiscal() {regimenFiscal1}
                '
                '   Asignar el emisor al CFDI
                '
                ComprobanteXML.Emisor = Emisor
                '
                '
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
        End Try
        '
        ''
    End Sub

    Private Sub ConfiguraReceptor(ByVal empleadoid As String)
        '
        '
        '
        '   Obtiene datos del receptor
        '
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pEmpleados @cmd=2, @empleadoid='" & empleadoid.ToString & "'", conn)
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                Dim Receptor As New ComprobanteReceptor()
                Receptor.rfc = rs("rfc")
                Receptor.nombre = rs("nombre") & " " & rs("apellido_paterno") & " " & rs("apellido_materno")

                'Crear domicilio del receptor
                Dim domicilioReceptor As New t_Ubicacion()
                domicilioReceptor.calle = rs("calle")
                domicilioReceptor.codigoPostal = rs("codigo_postal")
                domicilioReceptor.colonia = rs("colonia")
                domicilioReceptor.estado = rs("estado")
                domicilioReceptor.localidad = rs("municipio")
                domicilioReceptor.municipio = rs("municipio")
                domicilioReceptor.noExterior = rs("num_ext")
                If rs("num_int").ToString.Length > 0 Then
                    domicilioReceptor.noInterior = rs("num_int")
                End If
                domicilioReceptor.pais = "México"
                '
                '   Asignar el domiclio al receptor
                '
                Receptor.Domicilio = domicilioReceptor
                '
                '   Asignar el Receptor al CFD
                '
                ComprobanteXML.Receptor = Receptor
                '
                'tipocontribuyenteid = rs("tipocontribuyenteid")
                '
            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
        '
        ''
    End Sub

    Private Sub ConfiguraComplemento(ByVal empleadoid As String)

        Dim conX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("EXEC pEmpleados @cmd=2, @empleadoid='" & empleadoid.ToString.Trim & "'", conX)

        'Try
        conX.Open()

        Dim dreader As SqlDataReader

        dreader = cmdX.ExecuteReader()

        Dim Complemento As New ComprobanteComplemento()

        Dim nodoNomina As New complementoNomina.Nomina
        If dreader.Read() Then

            If dreader("metodopagoid").ToString = "2" Or dreader("metodopagoid").ToString = "4" Then
                nodoNomina.Banco = dreader("bancoid").ToString
                nodoNomina.BancoSpecified = True
            ElseIf dreader("metodopagoid").ToString = "3" Then
                nodoNomina.CLABE = dreader("clabe").ToString
                nodoNomina.Banco = dreader("bancoid").ToString
                nodoNomina.BancoSpecified = True
            End If

            nodoNomina.SalarioDiarioIntegrado = dreader("salario_diario_integrado")
            nodoNomina.SalarioDiarioIntegradoSpecified = True
            nodoNomina.SalarioBaseCotApor = dreader("salario_base")
            nodoNomina.SalarioBaseCotAporSpecified = True
            nodoNomina.PeriodicidadPago = ddlPeriodoPago.SelectedItem.Text
            nodoNomina.TipoJornada = dreader("tipo_jornada")
            If dreader("antiguedad").ToString <> "" And dreader("antiguedad").ToString <> "0" Then
                nodoNomina.Antiguedad = Convert.ToInt32(dreader("antiguedad").ToString())
            End If
            If dreader("riesgopuestoid").ToString <> "" And dreader("riesgopuestoid").ToString <> "0" Then
                nodoNomina.RiesgoPuestoSpecified = True
                nodoNomina.RiesgoPuesto = dreader("riesgopuestoid").ToString
            End If
            If dreader("registro_patronal").ToString <> "" And dreader("registro_patronal").ToString <> "0" Then
                nodoNomina.RegistroPatronal = dreader("registro_patronal")
            End If
            nodoNomina.FechaInicioRelLaboral = Convert.ToDateTime(dreader("fecha_ingreso")).ToString("yyyy-MM-dd")
            nodoNomina.NumDiasPagados = txtDiasLaborados.Text
            nodoNomina.FechaInicialPago = Convert.ToDateTime(calFechaInicial.SelectedDate).ToString("yyyy-MM-dd")
            nodoNomina.FechaFinalPago = Convert.ToDateTime(calFechaFinal.SelectedDate).ToString("yyyy-MM-dd")
            nodoNomina.FechaPago = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
            If dreader("numero_seguro_social") <> "" Then
                nodoNomina.NumSeguridadSocial = dreader("numero_seguro_social")
            End If
            nodoNomina.TipoRegimen = dreader("regimencontratacionid")
            nodoNomina.CURP = dreader("curp")
            nodoNomina.NumEmpleado = dreader("numero_empleado")
            nodoNomina.Version = "1.1"

            Dim ds As New DataSet()
            Dim datos As New DataSet()
            Dim ObjData As New DataControl
            ds = ObjData.FillDataSet("select a.percepcionid,isnull(a.monto,0) as monto,c.tipo_percepcion,b.clave,b.descripcion,isnull(b.exentoBit,0) as exentoBit from tblPercepcionEmpleadoCorrida a inner join tblPercepcion b on a.percepcionid=b.id inner join tblTipoPercepcion c on c.id=b.tipopercepcionid where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

            Dim total_percepciones As Decimal = 0
            Dim total_exento As Decimal = 0
            If ds.Tables(0).Rows.Count > 0 Then

                Dim nodoPercepcion As New NominaPercepciones()
                Dim nodoPercepcionItemGral As New List(Of NominaPercepcionesPercepcion)

                For i = 0 To ds.Tables(0).Rows.Count - 1
                    If Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
                        total_percepciones += Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
                        Dim nodoPercepcionItem As New NominaPercepcionesPercepcion
                        If ds.Tables(0).Rows.Item(i)("exentoBit").ToString = "True" Then
                            total_exento += Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
                            nodoPercepcionItem.ImporteExento = Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
                        Else
                            nodoPercepcionItem.ImporteGravado = Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
                        End If
                        nodoPercepcionItem.Concepto = ds.Tables(0).Rows.Item(i)("descripcion").ToString
                        nodoPercepcionItem.Clave = ds.Tables(0).Rows.Item(i)("clave").ToString
                        nodoPercepcionItem.TipoPercepcion = ds.Tables(0).Rows.Item(i)("tipo_percepcion").ToString
                        nodoPercepcionItemGral.Add(nodoPercepcionItem)
                    End If
                Next

                nodoPercepcion.TotalExento = total_exento
                nodoPercepcion.TotalGravado = total_percepciones
                nodoPercepcion.Percepcion = nodoPercepcionItemGral.ToArray()

                If nodoPercepcionItemGral.Count > 0 Then
                    nodoNomina.Percepciones = New NominaPercepciones() {nodoPercepcion}
                End If

            End If


            datos = ObjData.FillDataSet("select a.deduccionid,isnull(a.monto,0) as monto,c.tipo_deduccion,b.clave,b.descripcion,isnull(b.exentoBit,0) as exentoBit from tblDeduccionEmpleadoCorrida a inner join tblDeduccion b on a.deduccionid=b.id inner join tblTipoDeduccion c on c.id=b.tipodeduccionid where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

            Dim totalDeducciones As Decimal = 0
            total_exento = 0
            If datos.Tables(0).Rows.Count > 0 Then

                Dim nodoDeduccion As New NominaDeducciones()
                Dim nodoDeduccionItemGral As New List(Of NominaDeduccionesDeduccion)

                For i = 0 To datos.Tables(0).Rows.Count - 1
                    If Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
                        totalDeducciones += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                        Dim nodoDeduccionItem = New NominaDeduccionesDeduccion
                        If datos.Tables(0).Rows.Item(i)("exentoBit").ToString = "True" Then
                            total_exento += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                            nodoDeduccionItem.ImporteExento = Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                        Else
                            nodoDeduccionItem.ImporteGravado = Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                        End If
                        nodoDeduccionItem.Concepto = datos.Tables(0).Rows.Item(i)("descripcion").ToString
                        nodoDeduccionItem.Clave = datos.Tables(0).Rows.Item(i)("clave").ToString
                        nodoDeduccionItem.TipoDeduccion = datos.Tables(0).Rows.Item(i)("tipo_deduccion").ToString
                        nodoDeduccionItemGral.Add(nodoDeduccionItem)
                    End If
                Next

                nodoDeduccion.TotalExento = total_exento
                nodoDeduccion.TotalGravado = totalDeducciones
                nodoDeduccion.Deduccion = nodoDeduccionItemGral.ToArray()

                If nodoDeduccionItemGral.Count > 0 Then
                    nodoNomina.Deducciones = New NominaDeducciones() {nodoDeduccion}
                End If

            End If

            Dim dsIncapacidades As New DataSet()

            dsIncapacidades = ObjData.FillDataSet("select a.tipoincapacidad, isnull(a.monto,0) as monto, isnull(a.dias,0) as dias, b.nombre from tblIncapacidadEmpleadoCorrida a inner join tblTipoIncapacidad b on b.id=a.tipoincapacidad where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

            Dim nodoIncapacidadItemGral As New List(Of NominaIncapacidadesIncapacidad)

            If dsIncapacidades.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsIncapacidades.Tables(0).Rows.Count - 1
                    If Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
                        Dim nodoIncapacidadItem = New NominaIncapacidadesIncapacidad
                        nodoIncapacidadItem.DiasIncapacidad = Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("dias").ToString)
                        nodoIncapacidadItem.TipoIncapacidad = Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("tipoincapacidad").ToString)
                        nodoIncapacidadItem.Descuento = Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("monto").ToString)
                        nodoIncapacidadItemGral.Add(nodoIncapacidadItem)
                    End If
                Next
            End If

            If nodoIncapacidadItemGral.Count > 0 Then
                nodoNomina.Incapacidades = nodoIncapacidadItemGral.ToArray()
            End If

            Dim dsHorasExtra As New DataSet()

            dsHorasExtra = ObjData.FillDataSet("select a.tipohorasextra, isnull(a.monto,0) as monto, isnull(a.dias,0) as dias, isnull(a.horas,0) as horas, b.nombre from tblHorasExtraEmpleadoCorrida a inner join tblTipoHorasExtra b on b.id=a.tipohorasextra where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")
            ObjData = Nothing
            Dim nodoHorasExtraItemGral As New List(Of NominaHorasExtrasHorasExtra)

            If dsHorasExtra.Tables(0).Rows.Count > 0 Then
                For i = 0 To dsHorasExtra.Tables(0).Rows.Count - 1
                    If Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
                        Dim nodoHorasExtraItem = New NominaHorasExtrasHorasExtra
                        nodoHorasExtraItem.Dias = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("dias").ToString)
                        nodoHorasExtraItem.HorasExtra = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("horas").ToString)
                        nodoHorasExtraItem.TipoHoras = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("tipohorasextra").ToString)
                        nodoHorasExtraItem.ImportePagado = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("monto").ToString)
                        nodoHorasExtraItemGral.Add(nodoHorasExtraItem)
                    End If
                Next
            End If

            If nodoHorasExtraItemGral.Count > 0 Then
                nodoNomina.HorasExtras = nodoHorasExtraItemGral.ToArray()
            End If
            '
            'Convertir el objeto nodoNomina a un nodo
            Dim stream As New System.IO.MemoryStream()
            Dim xmlNameSpace As New XmlSerializerNamespaces()
            xmlNameSpace.Add("nomina", "http://www.sat.gob.mx/nomina")
            '
            Dim xmlTextWriter As New XmlTextWriter(stream, Encoding.UTF8)
            xmlTextWriter.Formatting = Formatting.None
            Dim xs As New XmlSerializer(GetType(complementoNomina.Nomina))
            xs.Serialize(xmlTextWriter, nodoNomina, xmlNameSpace)
            '
            Dim doc As New System.Xml.XmlDocument()
            stream.Position = 0
            doc.Load(stream)
            '
            Dim schemaLocation As XmlAttribute = doc.CreateAttribute("xsi", "schemaLocation", "")
            schemaLocation.Value = ""
            doc.DocumentElement.SetAttributeNode(schemaLocation)
            '
            Dim elemento As XmlElement() = {doc.DocumentElement}
            Complemento.Any = elemento
            ComprobanteXML.Complemento = Complemento
            '
        End If
    End Sub

    'Private Function ConfiguraComplemento(ByVal empleadoid As String) As XmlElement

    '    Dim conX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
    '    Dim cmdX As New SqlCommand("EXEC pEmpleados @cmd=2, @empleadoid='" & empleadoid.ToString.Trim & "'", conX)

    '    'Try
    '    conX.Open()

    '    Dim dreader As SqlDataReader

    '    dreader = cmdX.ExecuteReader()

    '    Dim Complemento As New ComprobanteComplemento()

    '    Dim nodoNomina As New complementoNomina.Nomina
    '    If dreader.Read() Then

    '        If dreader("metodopagoid").ToString = "2" Or dreader("metodopagoid").ToString = "4" Then
    '            nodoNomina.Banco = dreader("bancoid").ToString
    '            nodoNomina.BancoSpecified = True
    '        ElseIf dreader("metodopagoid").ToString = "3" Then
    '            nodoNomina.CLABE = dreader("clabe").ToString
    '            nodoNomina.Banco = dreader("bancoid").ToString
    '            nodoNomina.BancoSpecified = True
    '        End If

    '        nodoNomina.SalarioDiarioIntegrado = dreader("salario_diario_integrado")
    '        nodoNomina.SalarioDiarioIntegradoSpecified = True
    '        nodoNomina.SalarioBaseCotApor = dreader("salario_base")
    '        nodoNomina.SalarioBaseCotAporSpecified = True
    '        nodoNomina.PeriodicidadPago = ddlPeriodoPago.SelectedItem.Text
    '        nodoNomina.TipoJornada = dreader("tipo_jornada")
    '        If dreader("antiguedad").ToString <> "" And dreader("antiguedad").ToString <> "0" Then
    '            nodoNomina.Antiguedad = Convert.ToInt32(dreader("antiguedad").ToString())
    '        End If
    '        If dreader("riesgopuestoid").ToString <> "" And dreader("riesgopuestoid").ToString <> "0" Then
    '            nodoNomina.RiesgoPuestoSpecified = True
    '            nodoNomina.RiesgoPuesto = dreader("riesgopuestoid").ToString
    '        End If
    '        If dreader("registro_patronal").ToString <> "" And dreader("registro_patronal").ToString <> "0" Then
    '            nodoNomina.RegistroPatronal = dreader("registro_patronal")
    '        End If
    '        nodoNomina.FechaInicioRelLaboral = Convert.ToDateTime(dreader("fecha_ingreso")).ToString("yyyy-MM-dd")
    '        nodoNomina.NumDiasPagados = txtDiasLaborados.Text
    '        nodoNomina.FechaInicialPago = Convert.ToDateTime(calFechaInicial.SelectedDate).ToString("yyyy-MM-dd")
    '        nodoNomina.FechaFinalPago = Convert.ToDateTime(calFechaFinal.SelectedDate).ToString("yyyy-MM-dd")
    '        nodoNomina.FechaPago = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"))
    '        If dreader("numero_seguro_social") <> "" Then
    '            nodoNomina.NumSeguridadSocial = dreader("numero_seguro_social")
    '        End If
    '        nodoNomina.TipoRegimen = dreader("regimencontratacionid")
    '        nodoNomina.CURP = dreader("curp")
    '        nodoNomina.NumEmpleado = dreader("numero_empleado")
    '        nodoNomina.Version = "1.1"

    '        Dim ds As New DataSet()
    '        Dim datos As New DataSet()
    '        Dim ObjData As New DataControl
    '        ds = ObjData.FillDataSet("select a.percepcionid,isnull(a.monto,0) as monto,c.tipo_percepcion,b.clave,b.descripcion,isnull(b.exentoBit,0) as exentoBit from tblPercepcionEmpleadoCorrida a inner join tblPercepcion b on a.percepcionid=b.id inner join tblTipoPercepcion c on c.id=b.tipopercepcionid where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

    '        Dim nodoPercepcion As New NominaPercepciones()
    '        Dim nodoPercepcionItemGral As New List(Of NominaPercepcionesPercepcion)

    '        Dim total_percepciones As Decimal = 0
    '        Dim total_exento As Decimal = 0
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            For i = 0 To ds.Tables(0).Rows.Count - 1
    '                If Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
    '                    total_percepciones += Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
    '                    Dim nodoPercepcionItem As New NominaPercepcionesPercepcion
    '                    If ds.Tables(0).Rows.Item(i)("exentoBit").ToString = "True" Then
    '                        total_exento += Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
    '                        nodoPercepcionItem.ImporteExento = Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
    '                    Else
    '                        nodoPercepcionItem.ImporteGravado = Convert.ToDecimal(ds.Tables(0).Rows.Item(i)("monto").ToString)
    '                    End If
    '                    nodoPercepcionItem.Concepto = ds.Tables(0).Rows.Item(i)("descripcion").ToString
    '                    nodoPercepcionItem.Clave = ds.Tables(0).Rows.Item(i)("clave").ToString
    '                    nodoPercepcionItem.TipoPercepcion = ds.Tables(0).Rows.Item(i)("tipo_percepcion").ToString
    '                    nodoPercepcionItemGral.Add(nodoPercepcionItem)
    '                End If
    '            Next
    '        End If
    '        nodoPercepcion.TotalExento = total_exento
    '        nodoPercepcion.TotalGravado = total_percepciones
    '        nodoPercepcion.Percepcion = nodoPercepcionItemGral.ToArray()

    '        datos = ObjData.FillDataSet("select a.deduccionid,isnull(a.monto,0) as monto,c.tipo_deduccion,b.clave,b.descripcion,isnull(b.exentoBit,0) as exentoBit from tblDeduccionEmpleadoCorrida a inner join tblDeduccion b on a.deduccionid=b.id inner join tblTipoDeduccion c on c.id=b.tipodeduccionid where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

    '        Dim nodoDeduccion As New NominaDeducciones()
    '        Dim nodoDeduccionItemGral As New List(Of NominaDeduccionesDeduccion)

    '        Dim totalDeducciones As Decimal = 0
    '        total_exento = 0
    '        If datos.Tables(0).Rows.Count > 0 Then
    '            For i = 0 To datos.Tables(0).Rows.Count - 1
    '                If Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
    '                    totalDeducciones += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
    '                    Dim nodoDeduccionItem = New NominaDeduccionesDeduccion
    '                    If datos.Tables(0).Rows.Item(i)("exentoBit").ToString = "True" Then
    '                        total_exento += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
    '                        nodoDeduccionItem.ImporteExento = Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
    '                    Else
    '                        nodoDeduccionItem.ImporteGravado = Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
    '                    End If
    '                    nodoDeduccionItem.Concepto = datos.Tables(0).Rows.Item(i)("descripcion").ToString
    '                    nodoDeduccionItem.Clave = datos.Tables(0).Rows.Item(i)("clave").ToString
    '                    nodoDeduccionItem.TipoDeduccion = datos.Tables(0).Rows.Item(i)("tipo_deduccion").ToString
    '                    nodoDeduccionItemGral.Add(nodoDeduccionItem)
    '                End If
    '            Next
    '        End If

    '        nodoDeduccion.TotalExento = total_exento
    '        nodoDeduccion.TotalGravado = totalDeducciones

    '        nodoDeduccion.Deduccion = nodoDeduccionItemGral.ToArray()

    '        nodoNomina.Percepciones = New NominaPercepciones() {nodoPercepcion}
    '        nodoNomina.Deducciones = New NominaDeducciones() {nodoDeduccion}

    '        Dim dsIncapacidades As New DataSet()

    '        dsIncapacidades = ObjData.FillDataSet("select a.tipoincapacidad, isnull(a.monto,0) as monto, isnull(a.dias,0) as dias, b.nombre from tblIncapacidadEmpleadoCorrida a inner join tblTipoIncapacidad b on b.id=a.tipoincapacidad where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

    '        Dim nodoIncapacidadItemGral As New List(Of NominaIncapacidadesIncapacidad)

    '        If dsIncapacidades.Tables(0).Rows.Count > 0 Then
    '            For i = 0 To dsIncapacidades.Tables(0).Rows.Count - 1
    '                If Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
    '                    Dim nodoIncapacidadItem = New NominaIncapacidadesIncapacidad
    '                    nodoIncapacidadItem.DiasIncapacidad = Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("dias").ToString)
    '                    nodoIncapacidadItem.TipoIncapacidad = Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("tipoincapacidad").ToString)
    '                    nodoIncapacidadItem.Descuento = Convert.ToDecimal(dsIncapacidades.Tables(0).Rows.Item(i)("monto").ToString)
    '                    nodoIncapacidadItemGral.Add(nodoIncapacidadItem)
    '                End If
    '            Next
    '        End If

    '        If nodoIncapacidadItemGral.Count > 0 Then
    '            nodoNomina.Incapacidades = nodoIncapacidadItemGral.ToArray()
    '        End If

    '        Dim dsHorasExtra As New DataSet()

    '        dsHorasExtra = ObjData.FillDataSet("select a.tipohorasextra, isnull(a.monto,0) as monto, isnull(a.dias,0) as dias, isnull(a.horas,0) as horas, b.nombre from tblHorasExtraEmpleadoCorrida a inner join tblTipoHorasExtra b on b.id=a.tipohorasextra where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

    '        Dim nodoHorasExtraItemGral As New List(Of NominaHorasExtrasHorasExtra)

    '        If dsHorasExtra.Tables(0).Rows.Count > 0 Then
    '            For i = 0 To dsHorasExtra.Tables(0).Rows.Count - 1
    '                If Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("monto").ToString) > 0 Then
    '                    Dim nodoHorasExtraItem = New NominaHorasExtrasHorasExtra
    '                    nodoHorasExtraItem.Dias = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("dias").ToString)
    '                    nodoHorasExtraItem.HorasExtra = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("horas").ToString)
    '                    nodoHorasExtraItem.TipoHoras = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("tipohorasextra").ToString)
    '                    nodoHorasExtraItem.ImportePagado = Convert.ToDecimal(dsHorasExtra.Tables(0).Rows.Item(i)("monto").ToString)
    '                    nodoHorasExtraItemGral.Add(nodoHorasExtraItem)
    '                End If
    '            Next
    '        End If

    '        If nodoHorasExtraItemGral.Count > 0 Then
    '            nodoNomina.HorasExtras = nodoHorasExtraItemGral.ToArray()
    '        End If

    '        Dim resultadoComplemento As String = nodoNomina.Serialize()

    '        Dim xmlDoc As System.Xml.XmlDocument = New XmlDocument()

    '        xmlDoc.LoadXml(resultadoComplemento)

    '        Dim schemaLocation As XmlAttribute = xmlDoc.CreateAttribute("xsi", "schemaLocation", "http://www.sat.gob.mx/nomina")
    '        schemaLocation.Value = "http://www.sat.gob.mx/nomina http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd"
    '        xmlDoc.DocumentElement.SetAttributeNode(schemaLocation)
    '        Dim elementoComplemento As XmlElement = xmlDoc.DocumentElement

    '        ObjData = Nothing

    '        Return elementoComplemento

    '    End If


    '    'Catch ex As Exception
    '    '    Response.Write(ex.Message.ToString)
    '    'Finally
    '    '    conX.Close()
    '    '    conX.Dispose()
    '    'End Try

    'End Function

    Private Function generarXmlDoc() As XmlDocument
        Try
            Dim stream As New System.IO.MemoryStream()
            Dim xmlNameSpace As New XmlSerializerNamespaces()
            xmlNameSpace.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance")
            xmlNameSpace.Add("cfdi", "http://www.sat.gob.mx/cfd/3")
            Dim xmlTextWriter As New XmlTextWriter(stream, Encoding.UTF8)
            xmlTextWriter.Formatting = Formatting.Indented
            Dim xs As New XmlSerializer(GetType(Comprobante))
            xs.Serialize(xmlTextWriter, ComprobanteXML, xmlNameSpace)

            Dim doc As New System.Xml.XmlDocument()
            stream.Position = 0
            'stream.Seek(0, SeekOrigin.Begin)
            doc.Load(stream)

            Dim schemaLocation As XmlAttribute = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance")
            schemaLocation.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd"
            doc.DocumentElement.SetAttributeNode(schemaLocation)

            Dim schemaLocation2 As XmlAttribute = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance")
            schemaLocation2.Value = "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd"
            doc.DocumentElement.SetAttributeNode(schemaLocation2)

            xmlTextWriter.Close()

            Return doc
        Catch ex As Exception
            Throw New Exception("Error al generar XmlDocument, Error: " & ex.Message)
        End Try
    End Function

    Private Function Parametros(ByVal codigoUsuarioProveedor As String, ByVal codigoUsuario As String, ByVal idSucursal As Integer, ByVal textoXml As String) As String
        Dim root As XmlNode
        Dim xmlParametros As New XmlDocument()

        If xmlParametros.ChildNodes.Count = 0 Then
            Dim declarationNode As XmlNode = xmlParametros.CreateXmlDeclaration("1.0", "UTF-8", String.Empty)

            xmlParametros.AppendChild(declarationNode)

            root = xmlParametros.CreateElement("Parametros")
            xmlParametros.AppendChild(root)
        Else
            root = xmlParametros.DocumentElement
            root.RemoveAll()
        End If

        Dim attribute As XmlAttribute = root.OwnerDocument.CreateAttribute("Version")
        attribute.Value = "1.0"
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("CodigoUsuarioProveedor")
        attribute.Value = codigoUsuarioProveedor
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("CodigoUsuario")
        attribute.Value = codigoUsuario
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("IdSucursal")
        attribute.Value = idSucursal.ToString()
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("TextoXml")
        attribute.Value = textoXml
        root.Attributes.Append(attribute)

        Return xmlParametros.InnerXml
    End Function

    Private Function Parametros(ByVal codigoUsuarioProveedor As String, ByVal codigoUsuario As String, ByVal RFCEmisor As String, ByVal UUID As String) As String
        Dim root As XmlNode
        Dim folios As XmlNode
        Dim xmlParametros As New XmlDocument()
        Dim xmlFolios As New XmlDocument()

        If xmlParametros.ChildNodes.Count = 0 Then
            Dim declarationNode As XmlNode = xmlParametros.CreateXmlDeclaration("1.0", "UTF-8", String.Empty)
            xmlParametros.AppendChild(declarationNode)
            root = xmlParametros.CreateElement("Parametros")
            xmlParametros.AppendChild(root)

            folios = xmlParametros.CreateElement("Folios")
            root.AppendChild(folios)
        Else
            root = xmlParametros.DocumentElement
            root.RemoveAll()
        End If

        Dim attribute As XmlAttribute = root.OwnerDocument.CreateAttribute("CodigoUsuarioProveedor")
        attribute.Value = codigoUsuarioProveedor
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("CodigoUsuario")
        attribute.Value = codigoUsuario
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("RFCEmisor")
        attribute.Value = RFCEmisor
        root.Attributes.Append(attribute)

        Dim xmlElmUUID As XmlElement = root.OwnerDocument.CreateElement("UUID")
        folios.AppendChild(xmlElmUUID)
        xmlElmUUID.InnerText = UUID

        Return xmlParametros.InnerXml
    End Function

    Private Function ParametrosAcuseCancelacion(ByVal codigoUsuarioProveedor As String, ByVal codigoUsuario As String, ByVal idSucursal As Integer, ByVal uuid As String) As String
        Dim root As XmlNode
        Dim xmlParametros As New XmlDocument()

        If xmlParametros.ChildNodes.Count = 0 Then
            Dim declarationNode As XmlNode = xmlParametros.CreateXmlDeclaration("1.0", "UTF-8", String.Empty)

            xmlParametros.AppendChild(declarationNode)

            root = xmlParametros.CreateElement("Parametros")
            xmlParametros.AppendChild(root)
        Else
            root = xmlParametros.DocumentElement
            root.RemoveAll()
        End If

        Dim attribute As XmlAttribute = root.OwnerDocument.CreateAttribute("Version")
        attribute.Value = "1.0"
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("CodigoUsuarioProveedor")
        attribute.Value = codigoUsuarioProveedor
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("CodigoUsuario")
        attribute.Value = codigoUsuario
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("IdSucursal")
        attribute.Value = idSucursal.ToString()
        root.Attributes.Append(attribute)

        attribute = root.OwnerDocument.CreateAttribute("UUID")
        attribute.Value = uuid
        root.Attributes.Append(attribute)

        Return xmlParametros.InnerXml
    End Function

    'Private Function TimbradoFacturaxion(ByVal empleadoid As String) As Boolean
    '    '
    '    '
    '    Dim timbradoExitoso As Boolean = False
    '    '
    '    '
    '    Try
    '        '
    '        '   Convierte a texto el XML
    '        '
    '        Dim sw As New StringWriter
    '        Dim xw As New XmlTextWriter(sw)
    '        generarXmlDoc.WriteTo(xw)
    '        '
    '        '   Invoca al webservice de Facturaxion
    '        '
    '        Dim ServicioFX As New WSFX.TimbreFiscalDigitalSoapClient
    '        Dim params As String = ""
    '        Dim codigoUsuarioProveedor As String
    '        Dim codigoUsuario As String
    '        Dim idSucursal As Integer

    '        If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
    '            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor")
    '            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario")
    '            idSucursal = System.Configuration.ConfigurationManager.AppSettings("fx_idSucursal")
    '        Else
    '            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor_prod")
    '            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario_prod")
    '            idSucursal = System.Configuration.ConfigurationManager.AppSettings("fx_idSucursal_prod")
    '        End If

    '        params = Parametros(codigoUsuarioProveedor, codigoUsuario, idSucursal, sw.ToString)

    '        Dim FILENAME As String = Server.MapPath("~/portalcfd/cfd_storage" & "\Output.txt")

    '        'Get a StreamWriter class that can be used to write to the file
    '        Dim objStreamWriter As StreamWriter
    '        objStreamWriter = File.AppendText(FILENAME)

    '        'Append the the end of the string, "A user viewed this demo at: "
    '        'followed by the current date and time
    '        objStreamWriter.WriteLine(params)

    '        'Close the stream
    '        objStreamWriter.Close()

    '        '
    '        '   Timbra y obtiene resultado
    '        '
    '        'Dim resultado As String = ""
    '        Dim resultadoAcuse As String = ""
    '        Dim DocXMLTimbrado As New XmlDocument
    '        Dim DocXMLTimbradoAcuseSAT As New XmlDocument

    '        If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
    '            timbradoExitoso = ServicioFX.GenerarTimbrePrueba(params, resultado)
    '        Else
    '            timbradoExitoso = ServicioFX.GenerarTimbre(params, resultado)
    '            'Dim FILENAME As String = Server.MapPath("~/portalcfd/cfd_storage" & "\Output.txt")

    '            ''Get a StreamWriter class that can be used to write to the file
    '            'Dim objStreamWriter As StreamWriter
    '            'objStreamWriter = File.AppendText(FILENAME)

    '            ''Append the the end of the string, "A user viewed this demo at: "
    '            ''followed by the current date and time
    '            'objStreamWriter.WriteLine("se usó timbrado normal." & DateTime.Now.ToString())

    '            ''Close the stream
    '            'objStreamWriter.Close()

    '        End If


    '        Dim DocParams As New XmlDocument
    '        DocParams.LoadXml(params)
    '        DocParams.Save(Server.MapPath("cfd_storage") & "\" & "link_params_nomina_" & serie.Value.ToString & folio.Value.ToString & ".xml")

    '        DocXMLTimbrado.LoadXml(resultado)
    '        '
    '        '   Guarda XML del Timbre
    '        '
    '        DocXMLTimbrado.Save(Server.MapPath("cfd_storage") & "\" & "link_timbre_nomina_" & serie.Value.ToString & folio.Value.ToString & ".xml")
    '        '
    '    Catch exT As Exception
    '        Response.Write(exT.ToString)
    '        Response.End()
    '    End Try
    '    '
    '    '
    '    If timbradoExitoso Then
    '        '
    '        '   Obtiene los valores del timbre de respuesta
    '        '
    '        Dim selloSAT As String = ""
    '        Dim noCertificadoSAT As String = ""
    '        Dim selloCFD As String = ""
    '        Dim fechaTimbrado As String = ""
    '        Dim UUID As String = ""
    '        Dim Version As String = ""
    '        '
    '        '
    '        Dim s_RutaRespuestaPAC As String = Server.MapPath("cfd_storage") & "\" & "link_timbre_nomina_" & serie.Value.ToString & folio.Value.ToString & ".xml"
    '        Dim respuestaPAC As New Timbrado()
    '        Dim objStreamReader As New StreamReader(s_RutaRespuestaPAC)
    '        Dim Xml As New XmlSerializer(respuestaPAC.[GetType]())
    '        respuestaPAC = DirectCast(Xml.Deserialize(objStreamReader), Timbrado)
    '        objStreamReader.Close()

    '        '
    '        'Crear el objeto timbre para asignar los valores de la respuesta PAC
    '        Dim timbre As New uCFDsLib.TimbreFiscalDigital
    '        timbre.FechaTimbrado = Convert.ToDateTime(respuestaPAC.Items(0).Informacion(0).Timbre(0).FechaTimbrado)
    '        timbre.noCertificadoSAT = respuestaPAC.Items(0).Informacion(0).Timbre(0).noCertificadoSAT
    '        timbre.selloCFD = respuestaPAC.Items(0).Informacion(0).Timbre(0).selloCFD
    '        timbre.selloSAT = respuestaPAC.Items(0).Informacion(0).Timbre(0).selloSAT
    '        timbre.UUID = respuestaPAC.Items(0).Informacion(0).Timbre(0).UUID
    '        timbre.version = respuestaPAC.Items(0).Informacion(0).Timbre(0).version
    '        '
    '        '
    '        'Convertir el objeto TimbreFiscal a un nodo
    '        Dim stream As New System.IO.MemoryStream()
    '        Dim xmlNameSpace As New XmlSerializerNamespaces()
    '        xmlNameSpace.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")

    '        Dim xmlTextWriter As New XmlTextWriter(stream, Encoding.UTF8)
    '        xmlTextWriter.Formatting = Formatting.None
    '        Dim xs As New XmlSerializer(GetType(uCFDsLib.TimbreFiscalDigital))
    '        xs.Serialize(xmlTextWriter, timbre, xmlNameSpace)
    '        Dim doc As New System.Xml.XmlDocument()
    '        stream.Position = 0
    '        doc.Load(stream)

    '        Dim schemaLocation As XmlAttribute = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance")
    '        schemaLocation.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd"
    '        doc.DocumentElement.SetAttributeNode(schemaLocation)

    '        xmlTextWriter.Close()
    '        doc.PreserveWhitespace = True
    '        '
    '        '
    '        '   De un objeto Comprobante ya existente, asignar el timbre fiscal y complemento Nomina
    '        '
    '        Dim elemento As XmlElement() = {ConfiguraComplemento(empleadoid), doc.DocumentElement}
    '        Dim complemento As New ComprobanteComplemento()
    '        complemento.Any = elemento
    '        ComprobanteXML.Complemento = complemento
    '        '
    '        '
    '    Else
    '        '
    '        '   Obtiene y guarda el mensaje de error
    '        '
    '        listErrores.Add("Error inesperado al timbrar documento: " & resultado)
    '        '   
    '        '
    '        '
    '    End If
    '    '
    '    Return timbradoExitoso
    '    ''
    'End Function

    'Private Function TimbradoFacturaxion(ByVal empleadoid As String) As Boolean
    '    '
    '    '
    '    Dim timbradoExitoso As Boolean = False
    '    '
    '    '
    '    Try
    '        '
    '        '   Convierte a texto el XML
    '        '
    '        Dim sw As New StringWriter
    '        Dim xw As New XmlTextWriter(sw)
    '        generarXmlDoc.WriteTo(xw)
    '        '
    '        '   Invoca al webservice de Facturaxion
    '        '
    '        Dim ServicioFX As New WSFX.TimbreFiscalDigitalSoapClient
    '        Dim params As String = ""
    '        Dim codigoUsuarioProveedor As String
    '        Dim codigoUsuario As String
    '        Dim idSucursal As Integer

    '        If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
    '            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor")
    '            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario")
    '            idSucursal = System.Configuration.ConfigurationManager.AppSettings("fx_idSucursal")
    '        Else
    '            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor_prod")
    '            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario_prod")
    '            idSucursal = System.Configuration.ConfigurationManager.AppSettings("fx_idSucursal_prod")
    '        End If

    '        Dim xmlCfdi As New System.Xml.XmlDocument

    '        Dim orden_correcto1 As String = "<cfdi:Comprobante xsi:schemaLocation=" & Chr(34) & "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd http://www.sat.gob.mx/nomina http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd " & Chr(34)
    '        Dim orden_correcto2 As String = "xmlns:nomina=" & Chr(34) & "http://www.sat.gob.mx/nomina" & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " xmlns:cfdi=" & Chr(34) & "http://www.sat.gob.mx/cfd/3" & Chr(34) & "><cfdi:Emisor"
    '        '
    '        xmlCfdi.Load(Server.MapPath("cfd_storage\nomina\nomina_") & serie.Value.ToString & folio.Value.ToString & ".xml")
    '        docXML = xmlCfdi.InnerXml.ToString
    '        docXML = (Replace(docXML, "xmlns:cfdi=" & Chr(34) & "http://www.sat.gob.mx/cfd/3" & Chr(34), "", , , CompareMethod.Text))
    '        docXML = (Replace(docXML, "xmlns:cfdi=" & Chr(34) & "http://www.sat.gob.mx/cfd/3" & Chr(34), "", , , CompareMethod.Text))
    '        docXML = (Replace(docXML, "xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34), "", , , CompareMethod.Text))
    '        docXML = (Replace(docXML, "xsi:schemaLocation=" & Chr(34) & "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd" & Chr(34), "", , , CompareMethod.Text))
    '        '
    '        docXML = (Replace(docXML, "<cfdi:Comprobante", orden_correcto1, , , CompareMethod.Text))
    '        docXML = (Replace(docXML, "><cfdi:Emisor", orden_correcto2, , , CompareMethod.Text))
    '        '
    '        docXML = (Replace(docXML, "<cfdi:Complemento", "<cfdi:Complemento xsi:schemaLocation=" & Chr(34) & Chr(34), , , CompareMethod.Text))
    '        docXML = (Replace(docXML, "xmlns:nomina=" & Chr(34) & "http://www.sat.gob.mx/nomina" & Chr(34) & " schemaLocation=" & Chr(34) & Chr(34), "", , , CompareMethod.Text))
    '        '
    '        archivoSys = Server.MapPath("cfd_storage\nomina\") & serie.Value.ToString & folio.Value.ToString & ".xml"
    '        '
    '        Dim objWriter As New System.IO.StreamWriter(archivoSys)
    '        objWriter.Write(docXML.ToString)
    '        objWriter.Close()
    '        '
    '        '   Define parametros para webservice de Facturaxion
    '        '
    '        params = Parametros(codigoUsuarioProveedor, codigoUsuario, idSucursal, docXML.ToString)

    '        Dim FILENAME As String = Server.MapPath("~/portalcfd/cfd_storage" & "\Output.txt")

    '        'Get a StreamWriter class that can be used to write to the file
    '        Dim objStreamWriter As StreamWriter
    '        objStreamWriter = File.AppendText(FILENAME)

    '        'Append the the end of the string, "A user viewed this demo at: "
    '        'followed by the current date and time
    '        objStreamWriter.WriteLine(params)

    '        'Close the stream
    '        objStreamWriter.Close()
    '        '
    '        '   Se invoca al webservice para el timbrado y obtiene el resultado
    '        '
    '        timbradoExitoso = ServicioFX.GenerarTimbre(params, resultado)
    '        '
    '        Dim DocXMLTimbrado As New XmlDocument
    '        DocXMLTimbrado.LoadXml(resultado)
    '        '
    '        Dim DocParams As New XmlDocument
    '        DocParams.LoadXml(params)
    '        '
    '        '   Guarda XML de Parámetros
    '        '
    '        archivoSys = Server.MapPath("cfd_storage") & "\" & "link_params_nomina_" & serie.Value.ToString & folio.Value.ToString & ".xml"
    '        DocParams.Save(archivoSys)
    '        '
    '        '   Guarda XML del Timbre
    '        '
    '        archivoSys = Server.MapPath("cfd_storage") & "\" & "link_timbre_nomina_" & serie.Value.ToString & folio.Value.ToString & ".xml"
    '        DocXMLTimbrado.Save(archivoSys)
    '        '
    '        '
    '        '
    '    Catch exT As Exception
    '        Response.Write(exT.ToString)
    '        Response.End()
    '    End Try
    '    '
    '    '
    '    If timbradoExitoso Then
    '        '
    '        '   Obtiene los valores del timbre de respuesta
    '        '
    '        Dim selloSAT As String = ""
    '        Dim noCertificadoSAT As String = ""
    '        Dim selloCFD As String = ""
    '        Dim fechaTimbrado As String = ""
    '        Dim UUID As String = ""
    '        Dim Version As String = ""
    '        '
    '        '
    '        Dim s_RutaRespuestaPAC As String = Server.MapPath("cfd_storage") & "\" & "link_timbre_nomina_" & serie.Value.ToString & folio.Value.ToString & ".xml"
    '        Dim respuestaPAC As New Timbrado()
    '        Dim objStreamReader As New StreamReader(s_RutaRespuestaPAC)
    '        Dim Xml As New XmlSerializer(respuestaPAC.[GetType]())
    '        respuestaPAC = DirectCast(Xml.Deserialize(objStreamReader), Timbrado)
    '        objStreamReader.Close()
    '        '
    '        '
    '        'Crear el objeto timbre para asignar los valores de la respuesta PAC
    '        Dim timbre As New uCFDsLib.TimbreFiscalDigital
    '        timbre.FechaTimbrado = Convert.ToDateTime(respuestaPAC.Items(0).Informacion(0).Timbre(0).FechaTimbrado)
    '        timbre.noCertificadoSAT = respuestaPAC.Items(0).Informacion(0).Timbre(0).noCertificadoSAT
    '        timbre.selloCFD = respuestaPAC.Items(0).Informacion(0).Timbre(0).selloCFD
    '        timbre.selloSAT = respuestaPAC.Items(0).Informacion(0).Timbre(0).selloSAT
    '        timbre.UUID = respuestaPAC.Items(0).Informacion(0).Timbre(0).UUID
    '        timbre.version = respuestaPAC.Items(0).Informacion(0).Timbre(0).version
    '        '
    '        '
    '        'Convertir el objeto TimbreFiscal a un nodo
    '        Dim stream As New System.IO.MemoryStream()
    '        Dim xmlNameSpace As New XmlSerializerNamespaces()
    '        xmlNameSpace.Add("tfd", "http://www.sat.gob.mx/TimbreFiscalDigital")

    '        Dim xmlTextWriter As New XmlTextWriter(stream, Encoding.UTF8)
    '        xmlTextWriter.Formatting = Formatting.None
    '        Dim xs As New XmlSerializer(GetType(uCFDsLib.TimbreFiscalDigital))
    '        xs.Serialize(xmlTextWriter, timbre, xmlNameSpace)
    '        Dim doc As New System.Xml.XmlDocument()
    '        stream.Position = 0
    '        doc.Load(stream)

    '        Dim schemaLocation As XmlAttribute = doc.CreateAttribute("xsi", "schemaLocation", "http://www.w3.org/2001/XMLSchema-instance")
    '        schemaLocation.Value = "http://www.sat.gob.mx/TimbreFiscalDigital http://www.sat.gob.mx/TimbreFiscalDigital/TimbreFiscalDigital.xsd"
    '        doc.DocumentElement.SetAttributeNode(schemaLocation)

    '        xmlTextWriter.Close()
    '        doc.PreserveWhitespace = True
    '        '
    '        '
    '        '   Asignar el timbre fiscal y complemento Nomina
    '        '
    '        docXML = (Replace(docXML, "</nomina:Nomina>", "</nomina:Nomina>" & doc.DocumentElement.OuterXml.ToString, , , CompareMethod.Text))
    '        '
    '        '
    '    Else
    '        '
    '        '   Obtiene y guarda el mensaje de error
    '        '
    '        listErrores.Add("No se ha logrado timbrar el documento para el empleado : " & CargaNombreEmpleado(empleadoid) & ", favor de validar su información.")
    '        '   
    '        '
    '        '
    '    End If
    '    '
    '    Return timbradoExitoso
    '    ''
    'End Function

    Private Function TimbradoFactureHoy(ByVal empleadoid As String) As Boolean
        '
        '   Convierte a texto el XML
        '
        '
        Dim timbradoExitoso As Boolean = False
        Dim sw As New StringWriter
        Dim xw As New XmlTextWriter(sw)
        generarXmlDoc.WriteTo(xw)

        Dim xmlCfdi As New System.Xml.XmlDocument

        archivoSys = Server.MapPath("cfd_storage\nomina\nomina_") & serie.Value.ToString & folio.Value.ToString & ".xml"

        Dim orden_correcto1 As String = "<cfdi:Comprobante xsi:schemaLocation=" & Chr(34) & "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd http://www.sat.gob.mx/nomina http://www.sat.gob.mx/sitio_internet/cfd/nomina/nomina11.xsd " & Chr(34)
        Dim orden_correcto2 As String = "xmlns:nomina=" & Chr(34) & "http://www.sat.gob.mx/nomina" & Chr(34) & " xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34) & " xmlns:cfdi=" & Chr(34) & "http://www.sat.gob.mx/cfd/3" & Chr(34) & "><cfdi:Emisor"
        '
        xmlCfdi.Load(archivoSys)
        docXML = xmlCfdi.InnerXml.ToString
        docXML = (Replace(docXML, "xmlns:cfdi=" & Chr(34) & "http://www.sat.gob.mx/cfd/3" & Chr(34), "", , , CompareMethod.Text))
        docXML = (Replace(docXML, "xmlns:cfdi=" & Chr(34) & "http://www.sat.gob.mx/cfd/3" & Chr(34), "", , , CompareMethod.Text))
        docXML = (Replace(docXML, "xmlns:xsi=" & Chr(34) & "http://www.w3.org/2001/XMLSchema-instance" & Chr(34), "", , , CompareMethod.Text))
        docXML = (Replace(docXML, "xsi:schemaLocation=" & Chr(34) & "http://www.sat.gob.mx/cfd/3 http://www.sat.gob.mx/sitio_internet/cfd/3/cfdv32.xsd" & Chr(34), "", , , CompareMethod.Text))
        '
        docXML = (Replace(docXML, "<cfdi:Comprobante", orden_correcto1, , , CompareMethod.Text))
        docXML = (Replace(docXML, "><cfdi:Emisor", orden_correcto2, , , CompareMethod.Text))
        '
        docXML = (Replace(docXML, "<cfdi:Complemento", "<cfdi:Complemento xsi:schemaLocation=" & Chr(34) & Chr(34), , , CompareMethod.Text))
        docXML = (Replace(docXML, "xmlns:nomina=" & Chr(34) & "http://www.sat.gob.mx/nomina" & Chr(34) & " schemaLocation=" & Chr(34) & Chr(34), "", , , CompareMethod.Text))
        '
        archivoSys = Server.MapPath("cfd_storage\nomina\") & serie.Value.ToString & folio.Value.ToString & ".xml"
        '
        Dim objWriter As New System.IO.StreamWriter(archivoSys)
        objWriter.Write(docXML.ToString)
        objWriter.Close()
        '
        '   Timbrado FactureHoy
        '
        Dim queusuariocertus As String = System.Configuration.ConfigurationManager.AppSettings("FactureHoyUsuario") 'usuario facture hoy '"AAA010101AAA.Certus.Facturehoy.Sistemas"
        Dim quepasscertus As String = System.Configuration.ConfigurationManager.AppSettings("FactureHoyContrasena") 'contraseña '"Acceso$23"
        Dim queproceso As Integer = System.Configuration.ConfigurationManager.AppSettings("FactureHoyProceso")      'Id de Servicio '41692353

        Dim MemStream As System.IO.MemoryStream = FileToMemory(archivoSys)
        Dim archivo As Byte() = MemStream.ToArray()
        Dim service As New FactureHoyNT.WsEmisionTimbradoService()
        Dim puerto = service.EmitirTimbrar(queusuariocertus, quepasscertus, queproceso, archivo)

        If puerto.isError Then
            listErrores.Add("No se ha logrado timbrar el documento para el empleado : " & CargaNombreEmpleado(empleadoid) & ", Error: " & puerto.message.ToString)
        Else
            timbradoExitoso = True
            archivoSys = Server.MapPath("cfd_storage\nomina") & "\" & "nomina_" & serie.Value.ToString & folio.Value.ToString & "_timbrado.xml"
            File.WriteAllBytes(archivoSys, puerto.XML)
        End If

        Return timbradoExitoso

    End Function

    Private Function FileToMemory(ByVal Filename As String) As MemoryStream
        Dim FS As New System.IO.FileStream(Filename, FileMode.Open)
        Dim MS As New System.IO.MemoryStream
        Dim BA(FS.Length - 1) As Byte
        FS.Read(BA, 0, BA.Length)
        FS.Close()
        MS.Write(BA, 0, BA.Length)
        Return MS
    End Function

    Private Function CargaNombreEmpleado(ByVal id As Integer) As String

        Dim nombre_empleado As String = ""
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pEmpleados @cmd=2, @empleadoid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                nombre_empleado = rs("nombre") & " " & rs("apellido_paterno") & " " & rs("apellido_materno")

            End If

        Catch ex As Exception
            Return nombre_empleado
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return nombre_empleado
    End Function

    Private Sub btnGeneraCorrida_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGeneraCorrida.Click

        panelCorridas.Visible = False
        panelGenerarCorrida.Visible = True
        btnGeneraCorrida.Enabled = False

        Dim ObjData As New DataControl
        Dim fechaInicial As DateTime
        Dim fechaFinal As DateTime
        fechaInicial = calFechaInicial.SelectedDate
        fechaFinal = calFechaFinal.SelectedDate

        If txtDiasLaborados.Text = "" Then
            txtDiasLaborados.Text = "0"
        End If

        corridaId.Value = ObjData.RunSQLScalarQuery("EXEC pCorridasNomina @cmd=1, @periodo_pagoid='" & ddlPeriodoPago.SelectedValue.ToString & "', @fecha_inicial='" & fechaInicial.ToString("yyyyMMdd") & "', @fecha_final='" & fechaFinal.ToString("yyyyMMdd") & "', @dias_laborados='" & txtDiasLaborados.Text.ToString & "'")

        ObjData.RunSQLQuery("EXEC pCorridasNomina @cmd=2, @id='" & corridaId.Value.ToString & "', @periodo_pagoid='" & ddlPeriodoPago.SelectedValue.ToString & "'")

        ObjData.RunSQLQuery("EXEC pCorridasNomina @cmd=3, @id='" & corridaId.Value.ToString & "', @periodo_pagoid='" & ddlPeriodoPago.SelectedValue.ToString & "'")

        calFechaInicial.Enabled = False
        calFechaFinal.Enabled = False
        ddlPeriodoPago.Enabled = False
        txtDiasLaborados.Enabled = False

        employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        employeeslist.DataSource = GetEmployees()
        employeeslist.DataBind()
        btnTinbrarNomina.Visible = True
    End Sub

    Private Sub ObtenerPercepciones(ByVal empleadoid As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pListarPercepciones @cmd=3,@empleadoid='" & empleadoid & "', @corridaid='" & corridaId.Value.ToString & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        grdPercepciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdPercepciones.DataSource = ds
        grdPercepciones.DataBind()

    End Sub

    Private Sub ObtenerDeducciones(ByVal empleadoid As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pListarDeducciones @cmd=3,@empleadoid='" & empleadoid & "', @corridaid='" & corridaId.Value.ToString & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        grdDeducciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdDeducciones.DataSource = ds
        grdDeducciones.DataBind()

    End Sub

    Private Sub ObtenerIncapacidades(ByVal empleadoid As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pIncapacidades @cmd=2,@empleadoid='" & empleadoid & "', @corridaid='" & corridaId.Value.ToString & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        grdIncapacidades.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdIncapacidades.DataSource = ds
        grdIncapacidades.DataBind()

    End Sub

    Private Sub ObtenerHorasExtra(ByVal empleadoid As String)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pHorasExtra @cmd=1,@empleadoid='" & empleadoid & "', @corridaid='" & corridaId.Value.ToString & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        grdHorasExtra.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdHorasExtra.DataSource = ds
        grdHorasExtra.DataBind()

    End Sub

    Private Sub employeeslist_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles employeeslist.ItemCommand
        Select Case e.CommandName
            Case "cmdCancel"
                Call CancelaCFDI(e.CommandArgument)
                Call GetEmployeesTimbrados()
            Case "cmdVerPercepciones"
                EmployeeID.Value = e.CommandArgument
                ObtenerPercepciones(e.CommandArgument)
                RadWindow1.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = False
                windowHorasExtra.VisibleOnPageLoad = False
                windowPercepciones.VisibleOnPageLoad = True
            Case "cmdVerDeducciones"
                EmployeeID.Value = e.CommandArgument
                ObtenerDeducciones(e.CommandArgument)
                RadWindow1.VisibleOnPageLoad = False
                windowPercepciones.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = False
                windowHorasExtra.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = True
            Case "cmdVerIncapacidades"
                EmployeeID.Value = e.CommandArgument
                ObtenerIncapacidades(e.CommandArgument)
                windowPercepciones.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = False
                RadWindow1.VisibleOnPageLoad = False
                windowHorasExtra.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = True
            Case "cmdVerHorasExtra"
                EmployeeID.Value = e.CommandArgument
                ObtenerHorasExtra(e.CommandArgument)
                windowPercepciones.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = False
                RadWindow1.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = False
                windowHorasextra.VisibleOnPageLoad = True
            Case "cmdXML"
                Call DownloadXML(e.CommandArgument)
                RadWindow1.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = False
                windowHorasExtra.VisibleOnPageLoad = False
                windowPercepciones.VisibleOnPageLoad = False
            Case "cmdPDF"
                Call DownloadPDF(e.CommandArgument)
                RadWindow1.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = False
                windowPercepciones.VisibleOnPageLoad = False
                windowHorasExtra.VisibleOnPageLoad = False
        End Select
    End Sub

    Private Sub CancelaCFDI(ByVal cfdi As Long)
        '
        '   Obtiene serie y folio y construye nombre del XML
        '
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim archivoTimbrado As String = ""
        Dim connX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("select isnull(serie,'') as serie, isnull(folio,0) as folio from tblNominaEmpleados where id='" & cfdi.ToString & "'", connX)
        Try

            connX.Open()

            Dim rs As SqlDataReader
            rs = cmdX.ExecuteReader()

            If rs.Read Then
                serie = rs("serie")
                folio = rs("folio")
            End If

        Catch ex As Exception
            '
            Response.Write(ex.ToString)
            Response.End()
        Finally
            connX.Close()
            connX.Dispose()
            connX = Nothing
        End Try
        '

        '
        '   Lee XML timbrado e invoca al webservice y Marca como cancelado en la base de datos
        '
        If CancelacionFacturaxion(serie, folio) = True Then
            Dim ObjData As New DataControl
            ObjData.RunSQLQuery("exec pCorridasNomina @cmd=7, @id='" & cfdi.ToString & "'")
            ObjData = Nothing
        Else
            Dim ObjData As New DataControl
            ObjData.RunSQLQuery("exec pCorridasNomina @cmd=7, @id='" & cfdi.ToString & "'")
            ObjData = Nothing
        End If
        '
        ''
    End Sub

    Private Function CancelacionFacturaxion(ByVal serie As String, ByVal folio As Long) As Boolean
        '
        '
        Dim cancelacionExitosa As Boolean = False
        '
        '   Invoca al webservice de Facturaxion
        '
        Dim ServicioFX As New WSFX.TimbreFiscalDigitalSoapClient
        Dim params As String = ""

        Dim codigoUsuarioProveedor As String
        Dim codigoUsuario As String

        If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor")
            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario")
        Else
            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor_prod")
            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario_prod")
        End If

        Dim RFCEmisor As String = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage/nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "rfc", "cfdi:Emisor")
        Dim UUID As String = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage/nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        params = Parametros(codigoUsuarioProveedor, codigoUsuario, RFCEmisor, UUID)


        'Solicita la cancelación al webservice

        Dim resultado As String = ""
        Dim DocXMLCancelado As New XmlDocument
        Try
            If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
                cancelacionExitosa = ServicioFX.ReportarCancelacionPrueba(params, resultado)
            Else
                cancelacionExitosa = ServicioFX.ReportarCancelacion(params, resultado)
            End If
        Catch ex As Exception
            Try
                If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
                    cancelacionExitosa = ServicioFX.ReportarCancelacionPrueba(params, resultado)
                Else
                    cancelacionExitosa = ServicioFX.ReportarCancelacion(params, resultado)
                End If
            Catch ex1 As Exception
                '
            End Try
        End Try
        '
        '
        '   Si el webservice regresa algún error guarda el XML
        '
        If Not cancelacionExitosa Then
            DocXMLCancelado.LoadXml(resultado)
            '
            DocXMLCancelado.Save(Server.MapPath("~/portalcfd/cfd_storage/nomina") & "\nomina_" & serie.ToString & folio.ToString & "_cancelacion_errores.xml")
            '
        Else
            '
            'Si la cancelación fue exitosa, generamos el acuse de cancelación y lo guarda en XML
            Call AcuseCancelacion(UUID, serie, folio)
        End If

        Return cancelacionExitosa
        ''
    End Function

    Private Sub AcuseCancelacion(ByVal uuid As String, ByVal serie As String, ByVal folio As String)

        Dim ServicioFX As New WSFX.TimbreFiscalDigitalSoapClient

        Dim codigoUsuarioProveedor As String
        Dim codigoUsuario As String
        Dim idSucursal As Integer
        Dim params As String = ""

        If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor")
            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario")
            idSucursal = System.Configuration.ConfigurationManager.AppSettings("fx_idSucursal")
        Else
            codigoUsuarioProveedor = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuarioproveedor_prod")
            codigoUsuario = System.Configuration.ConfigurationManager.AppSettings("fx_codigousuario_prod")
            idSucursal = System.Configuration.ConfigurationManager.AppSettings("fx_idSucursal_prod")
        End If

        params = ParametrosAcuseCancelacion(codigoUsuarioProveedor, codigoUsuario, idSucursal, uuid)

        'Solicita acuse de cancelación al webservice

        Dim acuseExitoso As Boolean = False
        Dim resultado As String = ""
        Try
            If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
                acuseExitoso = ServicioFX.GetAcuseCancelacion(params, resultado)
            Else
                acuseExitoso = ServicioFX.GetAcuseCancelacion(params, resultado)
            End If
        Catch ex As Exception
            Try
                If System.Configuration.ConfigurationManager.AppSettings("fx_pruebas") = 1 Then
                    acuseExitoso = ServicioFX.GetAcuseCancelacion(params, resultado)
                Else
                    acuseExitoso = ServicioFX.GetAcuseCancelacion(params, resultado)
                End If
            Catch ex1 As Exception
                '
            End Try
        End Try

        Dim DocXMLAcuseCancelado As New XmlDocument
        DocXMLAcuseCancelado.LoadXml(resultado)
        DocXMLAcuseCancelado.Save(Server.MapPath("~/portalcfd/cfd_storage/nomina") & "\nomina_" & serie.ToString & folio.ToString & "_acuse_cancelacion.xml")

    End Sub

    Private Sub DownloadXML(ByVal empleadoid As Long)
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("select isnull(serie,'') as serie,isnull(folio,0) as folio from tblNominaEmpleados where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString & "'", connF)
        Try

            connF.Open()

            Dim rs As SqlDataReader
            rs = cmdF.ExecuteReader()

            If rs.Read Then
                serie = rs("serie").ToString
                folio = rs("folio").ToString
            End If
        Catch ex As Exception
            '
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        Dim FilePath = Server.MapPath("~/portalcfd/cfd_storage/nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml"
        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        End If
        ''
    End Sub

    Private Sub DownloadPDF(ByVal empleadoid As Long)
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("select isnull(serie,'') as serie,isnull(folio,0) as folio from tblNominaEmpleados where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString & "'", connF)
        Try

            connF.Open()

            Dim rs As SqlDataReader
            rs = cmdF.ExecuteReader()

            If rs.Read Then
                serie = rs("serie").ToString
                folio = rs("folio").ToString
            End If
        Catch ex As Exception
            '
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        Dim FilePath = Server.MapPath("~/portalcfd/pdf_nomina") & "\nomina_" & serie.ToString & folio.ToString & ".pdf"
        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        Else

            Dim cfdId As Long = GetCFD(serie, folio)
            '
            GuardaPDF(GeneraPDF(cfdId.ToString, serie, folio), Server.MapPath("~/portalcfd/pdf_nomina") & "\nomina_" & serie.ToString & folio.ToString & ".pdf")
            '
            If File.Exists(FilePath) Then
                Dim FileName As String = Path.GetFileName(FilePath)
                Response.Clear()
                Response.ContentType = "application/octet-stream"
                Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
                Response.Flush()
                Response.WriteFile(FilePath)
                Response.End()
            End If
            '
        End If
        '
    End Sub

    Protected Sub btnGuardarPercepciones_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("EXEC pPercepciones @cmd=8, @empleadoid='" & EmployeeID.Value.ToString & "', @corridaid='" & corridaId.Value.ToString & "'")
        'Actualizamos en tabla tblPercepcionEmpleadoCorrida los montos de las percepciones al empleado
        For Each dataItem As Telerik.Web.UI.GridDataItem In grdPercepciones.MasterTableView.Items
            Dim percepcionId As String = dataItem.GetDataKeyValue("id").ToString
            Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)

            If txtImporte.Text = "" Then
                txtImporte.Text = "0.00"
            End If
            ObjData.RunSQLQuery("EXEC pPercepciones @cmd=9, @empleadoid='" & EmployeeID.Value.ToString & "', @percepcionid='" & percepcionId.ToString & "', @corridaid='" & corridaId.Value.ToString & "', @monto='" & txtImporte.Text.ToString & "'")
            'ObjData.RunSQLQuery("INSERT INTO tblPercepcionEmpleadoCorrida (empleadoid,percepcionid,corridaid,monto) VALUES('" & EmployeeID.Value.ToString & "', '" & percepcionId.ToString & "', '" & corridaId.Value.ToString & "', '" & txtImporte.Text.ToString & "')")
        Next
        ObjData = Nothing

        employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        employeeslist.DataSource = GetEmployees()
        employeeslist.DataBind()

        RadWindow1.VisibleOnPageLoad = False
        windowDeducciones.VisibleOnPageLoad = False
        windowIncapacidades.VisibleOnPageLoad = False
        windowPercepciones.VisibleOnPageLoad = False
        windowHorasExtra.VisibleOnPageLoad = False
    End Sub

    Protected Sub btnGuardarDeducciones_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("EXEC pDeducciones @cmd=8, @empleadoid='" & EmployeeID.Value.ToString & "', @corridaid='" & corridaId.Value & "'")
        'Actualizamos en tabla tblDeduccionEmpleadoCorrida los montos de las deducciones al empleado
        For Each dataItem As Telerik.Web.UI.GridDataItem In grdDeducciones.MasterTableView.Items
            Dim deduccionId As String = dataItem.GetDataKeyValue("id").ToString
            Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)

            If txtImporte.Text = "" Then
                txtImporte.Text = "0.00"
            End If
            ObjData.RunSQLQuery("EXEC pDeducciones @cmd=9, @empleadoid='" & EmployeeID.Value.ToString & "', @deduccionid='" & deduccionId.ToString & "', @corridaid='" & corridaId.Value.ToString & "', @monto='" & txtImporte.Text.ToString & "'")
            'ObjData.RunSQLQuery("INSERT INTO tblDeduccionEmpleadoCorrida (empleadoid,deduccionid,corridaid,monto) VALUES('" & EmployeeID.Value.ToString & "', '" & deduccionId.ToString & "', '" & corridaId.Value.ToString & "', '" & txtImporte.Text.ToString & "')")
        Next
        ObjData = Nothing

        employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        employeeslist.DataSource = GetEmployees()
        employeeslist.DataBind()

        RadWindow1.VisibleOnPageLoad = False
        windowDeducciones.VisibleOnPageLoad = False
        windowIncapacidades.VisibleOnPageLoad = False
        windowPercepciones.VisibleOnPageLoad = False
        windowHorasExtra.VisibleOnPageLoad = False
    End Sub

    Protected Sub btnGuardarIncapacidades_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardarIncapacidades.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("DELETE FROM tblIncapacidadEmpleadoCorrida WHERE empleadoid='" & EmployeeID.Value.ToString & "' and corridaId='" & corridaId.Value & "'")
        'Agregamos en tabla tblIncapacidadEmpleadoCorrida los montos de las incapacidades al empleado
        For Each dataItem As Telerik.Web.UI.GridDataItem In grdIncapacidades.MasterTableView.Items
            Dim tipoincapacidad As String = dataItem.GetDataKeyValue("id").ToString
            Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)
            Dim txtDias As RadNumericTextBox = DirectCast(dataItem.FindControl("txtDias"), RadNumericTextBox)

            If txtImporte.Text = "" Then
                txtImporte.Text = "0.00"
            End If
            If txtDias.Text = "" Then
                txtDias.Text = "0"
            End If
            ObjData.RunSQLQuery("INSERT INTO tblIncapacidadEmpleadoCorrida (empleadoid,tipoincapacidad,corridaid,monto,dias) VALUES('" & EmployeeID.Value.ToString & "', '" & tipoincapacidad.ToString & "', '" & corridaId.Value.ToString & "', '" & txtImporte.Text.ToString & "', '" & txtDias.Text.ToString & "')")
        Next
        ObjData = Nothing
        RadWindow1.VisibleOnPageLoad = False
        windowDeducciones.VisibleOnPageLoad = False
        windowIncapacidades.VisibleOnPageLoad = False
        windowPercepciones.VisibleOnPageLoad = False
        windowHorasExtra.VisibleOnPageLoad = False
    End Sub

    Protected Sub btnGuardarHorasExtra_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardarHorasExtra.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("DELETE FROM tblHorasExtraEmpleadoCorrida WHERE empleadoid='" & EmployeeID.Value.ToString & "' and corridaId='" & corridaId.Value & "'")
        'Actualizamos en tabla tblHorasExtraEmpleadoCorrida los montos de las horas extra al empleado
        For Each dataItem As Telerik.Web.UI.GridDataItem In grdHorasExtra.MasterTableView.Items
            Dim horasExtraId As String = dataItem.GetDataKeyValue("id").ToString
            Dim txtImporte As RadNumericTextBox = DirectCast(dataItem.FindControl("txtImporte"), RadNumericTextBox)
            Dim txtDias As RadNumericTextBox = DirectCast(dataItem.FindControl("txtDias"), RadNumericTextBox)
            Dim txtHoras As RadNumericTextBox = DirectCast(dataItem.FindControl("txtHoras"), RadNumericTextBox)

            If txtImporte.Text = "" Then
                txtImporte.Text = "0.00"
            End If

            ObjData.RunSQLQuery("INSERT INTO tblHorasExtraEmpleadoCorrida (empleadoid,corridaid,monto,tipohorasextra,dias,horas) VALUES('" & EmployeeID.Value.ToString & "', '" & corridaId.Value.ToString & "', '" & txtImporte.Text.ToString & "', '" & horasExtraId.ToString & "', '" & txtDias.Text.ToString & "', '" & txtHoras.Text.ToString & "')")
        Next
        ObjData = Nothing
        RadWindow1.VisibleOnPageLoad = False
        windowDeducciones.VisibleOnPageLoad = False
        windowIncapacidades.VisibleOnPageLoad = False
        windowPercepciones.VisibleOnPageLoad = False
        windowHorasextra.VisibleOnPageLoad = False
    End Sub

    Private Sub employeeslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles employeeslist.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim lnkVerPercepciones As LinkButton = CType(e.Item.FindControl("lnkVerPercepciones"), LinkButton)
                Dim lnkVerDeducciones As LinkButton = CType(e.Item.FindControl("lnkVerDeducciones"), LinkButton)
                Dim lnkVerIncapacidades As LinkButton = CType(e.Item.FindControl("lnkVerIncapacidades"), LinkButton)
                Dim lnkVerHorasExtra As LinkButton = CType(e.Item.FindControl("lnkVerHorasExtra"), LinkButton)
                Dim lnkXML As LinkButton = CType(e.Item.FindControl("lnkXML"), LinkButton)
                Dim lnkPDF As LinkButton = CType(e.Item.FindControl("lnkPDF"), LinkButton)
                Dim lnkCancelar As LinkButton = CType(e.Item.FindControl("lnkCancelar"), LinkButton)
                Dim chkid As System.Web.UI.WebControls.CheckBox = CType(e.Item.FindControl("chkid"), System.Web.UI.WebControls.CheckBox)
                Dim lblSerie As Label = CType(e.Item.FindControl("lblSerie"), Label)
                Dim lblFolio As Label = CType(e.Item.FindControl("lblFolio"), Label)
                Dim lblEstatus As Label = CType(e.Item.FindControl("lblEstatus"), Label)
                Dim lblCancelado As Label = CType(e.Item.FindControl("lblCancelado"), Label)
                '
                Dim lblTotalPercepciones As Label = CType(e.Item.FindControl("lblTotalPercepciones"), Label)
                Dim lblTotalDeducciones As Label = CType(e.Item.FindControl("lblTotalDeducciones"), Label)
                '
                lnkVerPercepciones.Text = lblTotalPercepciones.Text
                lnkVerDeducciones.Text = lblTotalDeducciones.Text
                '
                lnkCancelar.Attributes.Add("onclick", "return confirm ('¿Está seguro que desea cancelar este documento?');")
                '
                If lblSerie.Text <> "" And lblFolio.Text <> "" Then
                    lnkCancelar.Visible = True
                    lnkVerPercepciones.Visible = False
                    lnkVerDeducciones.Visible = False
                    lnkVerIncapacidades.Visible = False
                    lnkVerHorasExtra.Visible = False
                    lnkXML.Visible = True
                    lnkPDF.Visible = True
                    chkid.Checked = False
                    chkid.Visible = False
                    lblTotalPercepciones.Visible = True
                    lblTotalDeducciones.Visible = True
                End If
                '
                If lblEstatus.Text = "3" Then
                    chkid.Checked = False
                    lnkXML.Visible = False
                    lnkCancelar.Visible = False
                    lblCancelado.Visible = True
                    lblTotalPercepciones.Visible = True
                    lblTotalDeducciones.Visible = True
                End If
                '
        End Select
    End Sub

    Private Sub corridasList_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles corridasList.ItemCommand
        Select Case e.CommandName
            Case "cmdVer"
                corridaId.Value = e.CommandArgument.ToString
                Call MuestraDatosCorrida()
                Call GetEmployeesTimbrados()
                panelCorridas.Visible = False
                panelGenerarCorrida.Visible = True
                btnTinbrarNomina.Visible = True
                '
                RadWindow1.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = False
                windowPercepciones.VisibleOnPageLoad = False
                windowHorasExtra.VisibleOnPageLoad = False
                '
            Case "cmdDelete"
                Call EliminaCorrida(e.CommandArgument.ToString)
                Call MuestraCorridas()
                '
                RadWindow1.VisibleOnPageLoad = False
                windowDeducciones.VisibleOnPageLoad = False
                windowIncapacidades.VisibleOnPageLoad = False
                windowPercepciones.VisibleOnPageLoad = False
                windowHorasExtra.VisibleOnPageLoad = False
                '
        End Select
    End Sub

    Private Sub EliminaCorrida(ByVal id As String)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pCorridasNomina @cmd=8, @id='" & id.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click

        Call MuestraCorridas()

        corridaId.Value = 0
        ddlPeriodoPago.SelectedValue = 0
        ddlPeriodoPago.Enabled = True
        calFechaInicial.Enabled = True
        calFechaFinal.Enabled = True
        txtDiasLaborados.Enabled = True
        btnGeneraCorrida.Enabled = True
        panelCorridas.Visible = True
        panelGenerarCorrida.Visible = False
        '
        RadWindow1.VisibleOnPageLoad = False
        windowDeducciones.VisibleOnPageLoad = False
        windowIncapacidades.VisibleOnPageLoad = False
        windowPercepciones.VisibleOnPageLoad = False
        windowHorasExtra.VisibleOnPageLoad = False
        '
    End Sub

    Protected Sub employeeslist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles employeeslist.NeedDataSource
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pEmpleados  @cmd=7, @periodopagoid='" & ddlPeriodoPago.SelectedValue.ToString & "', @corridaid='" & corridaId.Value.ToString & "'", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        employeeslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        employeeslist.DataSource = ds
    End Sub

    Protected Sub corridasList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles corridasList.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                '
                btnDelete.Attributes.Add("onclick", "return confirm ('¿Está seguro que desea eliminar esta corrida?');")
                '
        End Select
    End Sub

    Protected Sub corridasList_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles corridasList.NeedDataSource
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCorridasNomina @cmd=5", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        corridasList.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        corridasList.DataSource = ds
    End Sub

    Private Function MostrarInformacion(ByVal resultado As String) As String
        Dim w As New MemoryStream()
        Dim writer As New XmlTextWriter(w, Encoding.Unicode)

        Dim document As New XmlDocument()
        document.LoadXml(resultado)
        writer.Formatting = Formatting.Indented
        document.WriteContentTo(writer)

        writer.Flush()
        w.Seek(0L, SeekOrigin.Begin)

        Dim reader As New StreamReader(w)
        Return reader.ReadToEnd()
    End Function

#Region "Manejo de PDF"

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub

    Private Function GeneraPDF(ByVal cfdid As Long, ByVal serie As String, ByVal folio As Long) As Telerik.Reporting.Report

        Dim reporte As New Formatos.formato_nomina

        If cfdid.ToString <> "0" Or cfdid.ToString <> "" Then


            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

            Dim numero_empleado As String = ""
            Dim periodo_pago As String = ""
            Dim fecha_inicial As String = ""
            Dim fecha_final As String = ""
            Dim regimen As String = ""
            Dim metodo_pago As String = ""

            Dim emp_nombre As String = ""
            Dim emp_direccion As String = ""
            Dim emp_num_exterior As String = ""
            Dim emp_num_interior As String = ""
            Dim emp_colonia As String = ""
            Dim emp_codigo_postal As String = ""
            Dim emp_municipio As String = ""
            Dim emp_estado As String = ""
            Dim emp_pais As String = ""
            Dim emp_fecha_ingreso As String = ""
            Dim emp_antiguedad As String = ""
            Dim emp_rfc As String = ""
            Dim emp_curp As String = ""
            Dim emp_numero_seguro_social As String = ""
            Dim emp_regimen_contratacion As String = ""
            Dim emp_registro_patronal As String = ""
            Dim emp_riesgo_puesto As String = ""
            Dim emp_salario_base As String = ""
            Dim emp_salario_diario_integrado As String = ""
            Dim emp_horas_extra_dobles As String = ""
            Dim emp_horas_extra_triples As String = ""
            Dim emp_tipo_jornada As String = ""
            Dim emp_departamento As String = ""
            Dim emp_puesto As String = ""
            Dim emp_dias_laborados As String = ""
            Dim emp_banco As String = ""
            Dim emp_clabe As String = ""
            Dim plantillaid As Integer = 1
            Dim empleadoid As String = "0"
            Dim CantidadTexto As String = ""
            Dim lugar_expedicion1 As String = ""
            Dim lugar_expedicion2 As String = ""
            Dim lugar_expedicion3 As String = ""

            Dim total_percepciones As Decimal = 0
            Dim total_deducciones As Decimal = 0
            Dim total As Decimal = 0


            Dim ds As DataSet = New DataSet()

            Try

                Dim cmd As New SqlCommand("EXEC pNomina @cmd=1, @id='" & cfdid.ToString & "'", conn)
                conn.Open()
                Dim rs As SqlDataReader
                rs = cmd.ExecuteReader()

                If rs.Read Then
                    numero_empleado = rs("numero_empleado")
                    periodo_pago = rs("periodo_pago")
                    fecha_inicial = rs("fecha_inicial")
                    fecha_final = rs("fecha_final")
                    regimen = rs("regimen")
                    metodo_pago = rs("metodo_pago")
                    lugar_expedicion1 = rs("lugar_expedicion1")
                    lugar_expedicion2 = rs("lugar_expedicion2")
                    lugar_expedicion3 = rs("lugar_expedicion3")

                    emp_nombre = rs("emp_nombre")
                    emp_direccion = rs("emp_direccion")
                    emp_num_exterior = rs("emp_num_exterior")
                    emp_num_interior = rs("emp_num_interior")
                    emp_colonia = rs("emp_colonia")
                    emp_codigo_postal = rs("emp_codigo_postal")
                    emp_municipio = rs("emp_municipio")
                    emp_estado = rs("emp_estado")
                    emp_pais = rs("emp_pais")
                    emp_fecha_ingreso = rs("emp_fecha_ingreso")
                    emp_antiguedad = rs("emp_antiguedad")
                    emp_rfc = rs("emp_rfc")
                    emp_curp = rs("emp_curp")
                    emp_numero_seguro_social = rs("emp_numero_seguro_social")
                    emp_regimen_contratacion = rs("emp_regimen_contratacion")
                    emp_registro_patronal = rs("emp_registro_patronal")
                    emp_riesgo_puesto = rs("emp_riesgo_puesto")
                    emp_salario_base = rs("emp_salario_base")
                    emp_salario_diario_integrado = rs("emp_salario_diario_integrado")
                    emp_horas_extra_dobles = rs("emp_horas_extra_dobles")
                    emp_horas_extra_triples = rs("emp_horas_extra_triples")
                    emp_tipo_jornada = rs("emp_tipo_jornada")
                    emp_departamento = rs("emp_departamento")
                    emp_puesto = rs("emp_puesto")
                    emp_dias_laborados = rs("emp_dias_laborados")
                    emp_banco = rs("emp_banco")
                    emp_clabe = rs("emp_clabe")
                    plantillaid = rs("plantillaid")
                    empleadoid = rs("empleadoid")
                    'corridaId = rs("corridaid")

                    total_percepciones = rs("total_percepciones")
                    total_deducciones = rs("total_deducciones")
                    total = rs("total")

                    Dim largo = Len(CStr(Format(CDbl(total), "#,###.00")))
                    Dim decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)

                    reporte.ReportParameters("plantillaId").Value = plantillaid.ToString
                    reporte.ReportParameters("cfdiId").Value = cfdid.ToString
                    reporte.ReportParameters("empleadoid").Value = empleadoid.ToString
                    reporte.ReportParameters("corridaId").Value = corridaId.Value.ToString

                    CantidadTexto = "( " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"

                    reporte.ReportParameters("txtNoNomina").Value = "Nómina No. " & folio.ToString
                    reporte.ReportParameters("txtLugarExpedicion1").Value = lugar_expedicion1.ToString
                    reporte.ReportParameters("txtLugarExpedicion2").Value = lugar_expedicion2.ToString
                    reporte.ReportParameters("txtLugarExpedicion3").Value = lugar_expedicion3.ToString

                    reporte.ReportParameters("txtUUID").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage\nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
                    reporte.ReportParameters("txtSerieEmisor").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage\nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "noCertificado", "cfdi:Comprobante")
                    reporte.ReportParameters("txtSerieCertificadoSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage\nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "noCertificadoSAT", "tfd:TimbreFiscalDigital")
                    reporte.ReportParameters("txtRegimenEmisor").Value = regimen.ToString
                    reporte.ReportParameters("txtRegistroPatronal").Value = emp_registro_patronal.ToString

                    reporte.ReportParameters("txtTipoComprobante").Value = "EGRESO"
                    reporte.ReportParameters("txtFechaEmision").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage\nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "fecha", "cfdi:Comprobante")
                    reporte.ReportParameters("txtFechaCertificacion").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage\nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "FechaTimbrado", "tfd:TimbreFiscalDigital")
                    reporte.ReportParameters("txtFormaPago").Value = "Pago en una sola exhibición"

                    reporte.ReportParameters("txtEmpleadoDiasLaborados").Value = emp_dias_laborados.ToString
                    reporte.ReportParameters("txtEmpleadoHorasDobles").Value = emp_horas_extra_dobles.ToString
                    reporte.ReportParameters("txtEmpleadoHorasTriples").Value = emp_horas_extra_triples.ToString

                    reporte.ReportParameters("txtEmpleadoNo").Value = numero_empleado.ToString
                    reporte.ReportParameters("txtEmpleadoNombre").Value = emp_nombre.ToString
                    reporte.ReportParameters("txtEmpleadoDireccion").Value = emp_direccion.ToString
                    reporte.ReportParameters("txtEmpleadoNumExterior").Value = emp_num_exterior.ToString
                    reporte.ReportParameters("txtEmpleadoNumInterior").Value = emp_num_interior.ToString
                    reporte.ReportParameters("txtEmpleadoColonia").Value = emp_colonia.ToString
                    reporte.ReportParameters("txtEmpleadoCodigoPostal").Value = emp_codigo_postal.ToString
                    reporte.ReportParameters("txtEmpleadoMunicipio").Value = emp_municipio.ToString
                    reporte.ReportParameters("txtEmpleadoEstado").Value = emp_estado.ToString
                    reporte.ReportParameters("txtEmpleadoPais").Value = emp_pais.ToString
                    reporte.ReportParameters("txtEmpleadoFechaIngreso").Value = emp_fecha_ingreso.ToString
                    reporte.ReportParameters("txtEmpleadoAntiguedad").Value = emp_antiguedad.ToString
                    reporte.ReportParameters("txtEmpleadoRFC").Value = emp_rfc.ToString
                    reporte.ReportParameters("txtEmpleadoCURP").Value = emp_curp.ToString
                    reporte.ReportParameters("txtEmpleadoNoSeguroSocial").Value = emp_numero_seguro_social.ToString

                    reporte.ReportParameters("txtEmpleadoRegimen").Value = emp_regimen_contratacion.ToString
                    reporte.ReportParameters("txtEmpleadoTipoRiesgo").Value = emp_riesgo_puesto.ToString
                    reporte.ReportParameters("txtEmpleadoDepartamento").Value = emp_departamento.ToString
                    reporte.ReportParameters("txtEmpleadoPuesto").Value = emp_puesto.ToString
                    reporte.ReportParameters("txtEmpleadoSalarioDiario").Value = FormatCurrency(emp_salario_base, 2).ToString
                    reporte.ReportParameters("txtEmpleadoSalarioDiarioIntegrado").Value = FormatCurrency(emp_salario_diario_integrado, 2).ToString
                    reporte.ReportParameters("txtTipoJornada").Value = emp_tipo_jornada.ToString
                    reporte.ReportParameters("txtDiasPagados").Value = emp_dias_laborados.ToString

                    reporte.ReportParameters("txtPeriocidadPago").Value = periodo_pago.ToString
                    reporte.ReportParameters("txtFechaInicial").Value = fecha_inicial.ToString
                    reporte.ReportParameters("txtFechaFinal").Value = fecha_final.ToString
                    reporte.ReportParameters("txtMetodoPago").Value = metodo_pago.ToString
                    reporte.ReportParameters("txtBanco").Value = emp_banco.ToString
                    reporte.ReportParameters("txtClabe").Value = emp_clabe.ToString

                    reporte.ReportParameters("txtTotalPercepciones").Value = FormatCurrency(total_percepciones, 2).ToString
                    reporte.ReportParameters("txtTotalDeducciones").Value = FormatCurrency(total_deducciones, 2).ToString
                    reporte.ReportParameters("txtTotal").Value = FormatCurrency(total, 2).ToString

                    Dim FilePath = Server.MapPath("~/portalcfd/cbb_nomina/" & serie.ToString & folio.ToString & ".png")
                    If Not File.Exists(FilePath) Then
                        generacbb(serie.ToString, folio.ToString)
                    End If

                    reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb_nomina/" & serie.ToString & folio.ToString & ".png")
                    reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))

                    reporte.ReportParameters("txtSelloDigitalCFDI").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage\nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "sello", "cfdi:Comprobante")
                    reporte.ReportParameters("txtSelloDigitalSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage\nomina") & "\nomina_" & serie.ToString & folio.ToString & "_timbrado.xml", "selloSAT", "tfd:TimbreFiscalDigital")

                    reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto.ToString
                    reporte.ReportParameters("txtCadenaOriginal").Value = CadenaOriginalComplemento(serie.ToString, folio.ToString)

                    Dim datos As New DataSet()
                    Dim ObjData As New DataControl()
                    datos = ObjData.FillDataSet("select a.percepcionid,isnull(a.monto,0) as monto,c.tipo_percepcion,b.clave,b.descripcion,isnull(b.exentoBit,0) as exentoBit from tblPercepcionEmpleadoCorrida a inner join tblPercepcion b on a.percepcionid=b.id inner join tblTipoPercepcion c on c.id=b.tipopercepcionid where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

                    Dim total_exento As Decimal = 0
                    Dim total_gravado As Decimal = 0
                    If datos.Tables(0).Rows.Count > 0 Then
                        For i = 0 To datos.Tables(0).Rows.Count - 1
                            If Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString) > 0 And datos.Tables(0).Rows.Item(i)("exentoBit").ToString = "True" Then
                                total_exento += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                            Else
                                total_gravado += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                            End If
                        Next
                    End If

                    reporte.ReportParameters("txtExentoPercepciones").Value = FormatCurrency(total_exento, 2).ToString
                    reporte.ReportParameters("txtGravadoPercepciones").Value = FormatCurrency(total_gravado, 2).ToString

                    datos = Nothing

                    datos = ObjData.FillDataSet("select a.deduccionid,isnull(a.monto,0) as monto,c.tipo_deduccion,b.clave,b.descripcion,isnull(b.exentoBit,0) as exentoBit from tblDeduccionEmpleadoCorrida a inner join tblDeduccion b on a.deduccionid=b.id inner join tblTipoDeduccion c on c.id=b.tipodeduccionid where corridaid='" & corridaId.Value.ToString & "' and empleadoid='" & empleadoid.ToString.Trim & "'")

                    total_exento = 0
                    total_gravado = 0
                    If datos.Tables(0).Rows.Count > 0 Then
                        For i = 0 To datos.Tables(0).Rows.Count - 1
                            If Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString) > 0 And datos.Tables(0).Rows.Item(i)("exentoBit").ToString = "True" Then
                                total_exento += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                            Else
                                total_gravado += Convert.ToDecimal(datos.Tables(0).Rows.Item(i)("monto").ToString)
                            End If
                        Next
                    End If

                    reporte.ReportParameters("txtExentoDeducciones").Value = FormatCurrency(total_exento, 2).ToString
                    reporte.ReportParameters("txtGravadoDeducciones").Value = FormatCurrency(total_gravado, 2).ToString

                End If
                rs.Close()
                '
            Catch ex As Exception
                '
                Response.Write(ex.ToString)
            Finally

                conn.Close()
                conn.Dispose()
                conn = Nothing
            End Try
        Else
            listErrores.Add("- El PDF para el documento con serie: " & serie.ToString & " y folio: " & folio.ToString & " no se generó.")
        End If

        Return reporte

    End Function

#End Region


End Class