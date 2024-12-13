Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.Threading
Imports System.Xml
Imports System.Net.Mail
Imports ThoughtWorks.QRCode.Codec
Imports Telerik.Reporting.Processing
Imports System.Web.Services.Protocols

Partial Class portalcfd_CFD
    Inherits System.Web.UI.Page
    Private RFCEmisor As String = ""
    Private uuids As New List(Of String)()

#Region "Load Initial Values"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Server.ScriptTimeout = 3600
        Me.Title = Resources.Resource.WindowsTitle
        If Not IsPostBack Then
            fha_ini.SelectedDate = Now()
            fha_fin.SelectedDate = Now()
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(tipoid, "exec pCatalogos @cmd=3", 0)
            ObjCat.Catalogo(clienteid, "exec pMisClientes @cmd=6, @userid='" & Session("userid").ToString & "'", 0)
            ObjCat.Catalogo(cmbMotivoCancela, "select isnull(clave,''), isnull(motivo,'') from tblCFD_MotivoCancelacion ", "02")
            ObjCat = Nothing
            Call MuestraLista()
            Call SetGridFilters()
        End If
    End Sub

    Private Sub MuestraLista()
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        cfdlist.DataSource = ObjData.FillDataSet("exec pCFD @cmd=8, @tipodocumentoid='" & tipoid.SelectedValue.ToString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @fhaini='" & fha_ini.SelectedDate.Value.ToShortDateString & "', @fhafin='" & fha_fin.SelectedDate.Value.ToShortDateString & "', @userid='" & Session("userid").ToString & "'")
        cfdlist.DataBind()
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
    End Sub
#End Region

#Region "Grid Handle"

    Protected Sub cfdlist_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles cfdlist.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                Response.Redirect("~/portalcfd/Facturar_Extended40.aspx?id=" & e.CommandArgument.ToString)
            Case "cmdXML"
                RadWindow1.VisibleOnPageLoad = False
                Call DownloadXML(e.CommandArgument)
            Case "cmdPDF"
                RadWindow1.VisibleOnPageLoad = False
                Call DownloadPDF(e.CommandArgument)
            Case "cmdSend"
                Call DatosEmail(e.CommandArgument)
            Case "cmdDelete"
                RadWindow1.VisibleOnPageLoad = False
                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pCFD @cmd=9, @cfdid='" & e.CommandArgument.ToString & "'")
                ObjData = Nothing
                Call MuestraLista()
            Case "cmdCancel"
                RadWindow1.VisibleOnPageLoad = False
                Call CancelaSIFEI(e.CommandArgument)
            Case "cmdAcuse"
                'RadWindow1.VisibleOnPageLoad = False
                Call VerAcuse(e.CommandArgument)
        End Select
    End Sub

    Protected Sub cfdlist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles cfdlist.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim lnkEdit As LinkButton = CType(e.Item.FindControl("lnkEdit"), LinkButton)
                Dim lnkCancelar As LinkButton = CType(e.Item.FindControl("lnkCancelar"), LinkButton)
                Dim lnkBorrar As LinkButton = CType(e.Item.FindControl("lnkBorrar"), LinkButton)
                Dim lnkXML As LinkButton = CType(e.Item.FindControl("lnkXML"), LinkButton)
                Dim lnkPDF As LinkButton = CType(e.Item.FindControl("lnkPDF"), LinkButton)
                Dim lblTimbrado As Label = CType(e.Item.FindControl("lblTimbrado"), Label)
                Dim lnkAcuse As LinkButton = CType(e.Item.FindControl("lnkAcuse"), LinkButton)
                Dim imgTimbrado As System.Web.UI.WebControls.Image = CType(e.Item.FindControl("imgTimbrado"), System.Web.UI.WebControls.Image)
                Dim imgSend As ImageButton = CType(e.Item.FindControl("imgSend"), ImageButton)
                Dim btnPrint As ImageButton = CType(e.Item.FindControl("btnPrint"), ImageButton)

                imgTimbrado.Visible = e.Item.DataItem("timbrado")

                If Not e.Item.DataItem("timbrado") Then
                    lblTimbrado.Text = " "
                    If e.Item.DataItem("folio") = 0 Then
                        lnkEdit.Enabled = True
                    Else
                        lnkEdit.Enabled = False
                    End If
                End If

                If e.Item.DataItem("enviadoBit") = True Then
                    imgSend.ImageUrl = "~/portalcfd/images/envelopeok.jpg"
                    imgSend.ToolTip = "Enviado el " & e.Item.DataItem("fechaenvio").ToString
                End If

                If e.Item.DataItem("formatoBit") Then
                    lnkEdit.Enabled = Not e.Item.DataItem("formatoBit")
                    lnkXML.Enabled = Not e.Item.DataItem("formatoBit")
                    lnkPDF.Enabled = e.Item.DataItem("formatoBit")
                    imgSend.Enabled = e.Item.DataItem("formatoBit")
                    lnkCancelar.Enabled = e.Item.DataItem("formatoBit")
                    lnkBorrar.Enabled = Not e.Item.DataItem("formatoBit")
                    lnkAcuse.Visible = False
                Else
                    lnkEdit.Enabled = Not e.Item.DataItem("timbrado")
                    lnkXML.Enabled = e.Item.DataItem("timbrado")
                    lnkPDF.Enabled = e.Item.DataItem("timbrado")
                    imgSend.Enabled = e.Item.DataItem("timbrado")
                    lnkCancelar.Enabled = e.Item.DataItem("timbrado")
                    lnkBorrar.Enabled = Not e.Item.DataItem("timbrado")
                    lnkAcuse.Visible = True
                End If

                lnkBorrar.Attributes.Add("onclick", "javascript:return confirm('Va a borrar un folio no timbrado. ¿Desea continuar?');")
                'lnkCancelar.Attributes.Add("onclick", "javascript:return confirm('Va a cancelar un documento. ¿Desea continuar?');")
                'lnkTimbrar.Attributes.Add("onclick", "javascript:return confirm('Por algún motivo no se logró realizar el timbrado para este cfdi. ¿Desea intentar timbrarlo de nuevo?');")
                'imgSend.Attributes.Add("onclick", "javascript:return confirm('Va a enviar por correo los archivos del cfdi. ¿Desea continuar?');")

                If e.Item.DataItem("estatus") = "Aplicado" Or e.Item.DataItem("estatus") = "Cancelado" Then
                    lnkCancelar.Visible = True
                    If e.Item.DataItem("estatus") = "Aplicado" Then
                        e.Item.ForeColor = Drawing.Color.Green
                        lnkAcuse.Visible = False
                    Else
                        lnkAcuse.Visible = True
                    End If
                Else
                    lnkCancelar.Visible = False
                    lnkAcuse.Visible = False
                End If

                If (e.Item.DataItem("estatus") = "Cancelado") Then
                    e.Item.Cells(6).ForeColor = Drawing.Color.Red
                    e.Item.Cells(6).Font.Bold = True
                    lnkCancelar.Visible = False
                    lnkAcuse.Visible = True
                    lnkXML.Enabled = e.Item.DataItem("timbrado")
                    lnkPDF.Enabled = e.Item.DataItem("timbrado")
                    imgSend.Visible = False
                End If

                If Session("perfilid") <> 1 And Session("userid") <> 11 Then

                    lnkCancelar.Visible = False
                    lnkCancelar.ToolTip = "Función no permitida"

                    lnkBorrar.Visible = False
                    lnkBorrar.ToolTip = "Función no permitida"
                    lnkEdit.Visible = False
                    lnkEdit.ToolTip = "Función no permitida"
                End If


                If Session("perfilid") = 1 And Session("userid") <> 11 And Session("userid") <> 13 And Session("userid") <> 18 And Session("userid") <> 32 Then

                    lnkCancelar.Visible = False
                    lnkCancelar.ToolTip = "Función no permitida"

                    lnkBorrar.Visible = False
                    lnkBorrar.ToolTip = "Función no permitida"
                    lnkEdit.Visible = False
                    lnkEdit.ToolTip = "Función no permitida"
                End If


        End Select
    End Sub

    Protected Sub cfdlist_PageIndexChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles cfdlist.PageIndexChanged
        cfdlist.CurrentPageIndex = e.NewPageIndex
        Call MuestraLista()
    End Sub

    Protected Sub cfdlist_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles cfdlist.NeedDataSource
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        cfdlist.DataSource = ObjData.FillDataSet("exec pCFD @cmd=8, @tipodocumentoid='" & tipoid.SelectedValue.ToString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @fhaini='" & fha_ini.SelectedDate.Value.ToShortDateString & "', @fhafin='" & fha_fin.SelectedDate.Value.ToShortDateString & "', @userid='" & Session("userid").ToString & "'")
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        ''
    End Sub

#End Region

#Region "Functions"

    Public Function GetXmlAttribute(ByVal url As String, campo As String, nodo As String) As String
        '
        '   Obtiene datos del cfdi para construir string del CBB
        '
        Dim valor As String = ""
        Dim FlujoReader As XmlTextReader = Nothing
        Dim i As Integer
        '
        '   Leer del fichero e ignorar los nodos vacios
        '
        FlujoReader = New XmlTextReader(url)
        FlujoReader.WhitespaceHandling = WhitespaceHandling.None
        Try
            While FlujoReader.Read()
                Select Case FlujoReader.NodeType
                    Case XmlNodeType.Element
                        If FlujoReader.Name = nodo Then
                            For i = 0 To FlujoReader.AttributeCount - 1
                                FlujoReader.MoveToAttribute(i)
                                If FlujoReader.Name = campo Then
                                    valor = FlujoReader.Value.ToString
                                End If
                            Next
                        End If
                End Select
            End While
        Catch ex As Exception
            valor = ""
        End Try
        Return valor
    End Function

    Private Sub DownloadXML(ByVal cfdid As Long)
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("exec pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", connF)
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
        Dim FilePath = Server.MapPath("~/portalcfd/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml"
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

    Private Sub DownloadPDF(ByVal cfdid As Long)
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("exec pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", connF)
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
        Dim FilePath = Server.MapPath("~/portalcfd/pdf/") & "ng_" & serie.ToString & folio.ToString & ".pdf"
        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        Else
            If serie.ToString = "R" Then
                GuardaPDF(GeneraPDF_Documento(cfdid), FilePath)
            Else
                GuardaPDF(GeneraPDF(cfdid), FilePath)
            End If


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

    Private Sub DownloadPDF_Preimpreso(ByVal cfdid As Long)
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("exec pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", connF)
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
        Dim FilePath = Server.MapPath("~/portalcfd/pdf/") & "ng_" & serie.ToString & folio.ToString & "_preimpreso.pdf"
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

    Private Sub DatosEmail(ByVal cfdid As Long)
        '
        tempcfdid.Value = cfdid
        '
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("exec pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", connF)
        Try

            connF.Open()

            Dim rsF = cmdF.ExecuteReader

            If rsF.Read Then
                serie = rsF("serie").ToString
                folio = rsF("folio").ToString
            End If
        Catch ex As Exception
            '
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        '   Obtiene datos de la persona
        '
        Dim mensaje As String = ""
        Dim razonsocial As String = ""
        Dim correo As String = ""
        Dim correo_cc As String = ""
        Dim correo_cco As String = ""
        Dim email_from As String = ""
        Dim email_smtp_server As String = ""
        Dim email_smtp_username As String = ""
        Dim email_smtp_password As String = ""
        Dim email_smtp_port As String = ""

        '
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        conn.Open()
        Dim SqlCommand As SqlCommand = New SqlCommand("exec pEnviaEmail @cfdid='" & cfdid.ToString & "'", conn)
        Dim rs = SqlCommand.ExecuteReader
        If rs.Read Then
            '
            razonsocial = rs("razonsocial")
            correo = rs("email_to")
            correo_cc = rs("email_cc")
            correo_cco = rs("email_cco")
            email_from = rs("email_from")
            email_smtp_server = rs("email_smtp_server")
            email_smtp_username = rs("email_smtp_username")
            email_smtp_password = rs("email_smtp_password")
            email_smtp_port = rs("email_smtp_port")
            '
        End If
        conn.Close()
        conn.Dispose()
        conn = Nothing
        '
        mensaje = "Estimado(a) Cliente, por este medio se le anexa la factura solicitada, la cual sirve como comprobante fiscal ante Hacienda." & vbCrLf & vbCrLf
        mensaje += "Gracias por su preferencia." & vbCrLf & vbCrLf
        mensaje += "Atentamente." & vbCrLf
        mensaje += razonsocial.ToString & vbCrLf & vbCrLf & vbCrLf & vbCrLf

        lblMensajeEmail.Text = ""
        txtFrom.Text = email_from.ToString
        txtTo.Text = correo.ToString
        txtCC.Text = correo_cc.ToString
        txtCCO.Text = correo_cco.ToString
        txtSubject.Text = razonsocial & " - Comprobante Fiscal Digital"
        txtMenssage.Text = mensaje.ToString

        RadWindow1.VisibleOnPageLoad = True

    End Sub

    Private Sub SendEmail(ByVal cfdid As Long)
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("exec pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", connF)
        Try

            connF.Open()

            Dim rsF = cmdF.ExecuteReader

            If rsF.Read Then
                serie = rsF("serie").ToString
                folio = rsF("folio").ToString
            End If
        Catch ex As Exception
            '
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        '   Obtiene datos de la persona
        '
        Dim mensaje As String = ""
        Dim razonsocial As String = ""
        Dim correo As String = ""
        Dim email_from As String = ""
        Dim email_smtp_server As String = ""
        Dim email_smtp_username As String = ""
        Dim email_smtp_password As String = ""
        Dim email_smtp_port As String = ""

        '
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        conn.Open()
        Dim SqlCommand As SqlCommand = New SqlCommand("exec pEnviaEmail @cfdid='" & cfdid.ToString & "'", conn)
        Dim rs = SqlCommand.ExecuteReader
        If rs.Read Then
            '       
            razonsocial = rs("razonsocial")
            correo = rs("email_to")
            email_from = rs("email_from")
            email_smtp_server = rs("email_smtp_server")
            email_smtp_username = rs("email_smtp_username")
            email_smtp_password = rs("email_smtp_password")
            email_smtp_port = rs("email_smtp_port")
            '
        End If
        conn.Close()
        conn.Dispose()
        conn = Nothing
        '
        '
        '
        mensaje = "<html><head></head><body><br />"
        mensaje += "Estimado(a) Cliente, por este medio se le anexa la factura solicitada, la cual sirve como comprobante fiscal ante Hacienda.<br /><br />Gracias por su preferencia."
        mensaje += "<br /><br />"
        mensaje += "Atentamente.<br /><br />"
        mensaje += "<strong>" & razonsocial.ToString & "</strong><br /><br /><br /><br /><br />"
        mensaje += "<span style='text-aling:justify; font-size:10px; font-family:Arial, Helvetica, sans-serif;font-weight: normal; color:#000; line-height:10px;'>Este correo es de car&aacute;cter informativo, favor de no responderlo. Para dudas o asesor&iacute;a visite nuestro sitio: www.facebook.com/natural.gs o env&iacute;e un correo a su asesor de ventas.</span><br /></body></html>"
        '
        '
        Dim objMM As New MailMessage
        objMM.To.Add(correo)
        objMM.From = New MailAddress(email_from, razonsocial)
        objMM.IsBodyHtml = True
        objMM.Priority = MailPriority.Normal
        objMM.Subject = razonsocial & " - Comprobante Fiscal Digital"
        objMM.Body = mensaje
        '
        '   Agrega anexos
        '
        Dim AttachXML As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("~/portalcfd/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
        Dim AttachPDF As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("~/portalcfd/pdf/") & "ng_" & serie.ToString & folio.ToString & ".pdf")
        '
        objMM.Attachments.Add(AttachXML)
        objMM.Attachments.Add(AttachPDF)
        '
        Dim SmtpMail As New SmtpClient
        Try
            Dim SmtpUser As New Net.NetworkCredential
            SmtpUser.UserName = email_smtp_username
            SmtpUser.Password = email_smtp_password
            SmtpUser.Domain = email_smtp_server
            SmtpMail.UseDefaultCredentials = False
            SmtpMail.Credentials = SmtpUser
            SmtpMail.Host = email_smtp_server
            SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpMail.Send(objMM)
        Catch ex As Exception
            '
            '
        Finally
            SmtpMail = Nothing
        End Try
        objMM = Nothing
    End Sub

    Private Sub CancelaCFDI33(ByVal cfdi As Long)
        '
        '   Obtiene serie y folio y construye nombre del XML
        '
        Dim consignacionid As Long = 0
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim email As String = ""
        Dim archivo_llave_privada As String = ""
        Dim contrasena_llave_privada As String = ""
        Dim archivoCertificado As String = ""

        Dim connX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("exec pCFD_Cancela @cfdid='" & cfdi.ToString & "'", connX)
        Try

            connX.Open()

            Dim rs As SqlDataReader
            rs = cmdX.ExecuteReader()

            If rs.Read Then
                consignacionid = rs("consignacionid")
                serie = rs("serie")
                folio = rs("folio")
                email = rs("email")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            connX.Close()
            connX.Dispose()
            connX = Nothing
        End Try

        Dim connY As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdY As New SqlCommand("EXEC pCFD @cmd=19", connY)
        Try

            connY.Open()

            Dim rsY As SqlDataReader
            rsY = cmdY.ExecuteReader()

            If rsY.Read Then
                archivo_llave_privada = Server.MapPath("~/portalcfd/llave/") & rsY("archivo_llave_privada")
                contrasena_llave_privada = rsY("contrasena_llave_privada")
                archivoCertificado = Server.MapPath("~/portalcfd/certificados/") & rsY("archivo_certificado")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            connY.Close()
            connY.Dispose()
            connY = Nothing
        End Try

        If Not EsDocumento(cfdi) Then
            If serie = "R" And folio > 0 Then
                Dim DataControl As New DataControl
                DataControl.RunSQLQuery("exec pCFD @cmd=21, @cfdid='" & cfdi.ToString & "'")
                DataControl = Nothing
            Else
                Dim MemStreamArch As System.IO.MemoryStream = FileToMemory(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
                Dim Archivo As Byte() = MemStreamArch.ToArray()
                MemStreamArch = Nothing

                Dim MemStream As System.IO.MemoryStream = FileToMemory(archivoCertificado)
                Dim Certificado As Byte() = MemStream.ToArray()

                Dim pkey As New Chilkat.PrivateKey
                pkey.LoadPkcs8EncryptedFile(archivo_llave_privada, contrasena_llave_privada)
                Dim Resultado As Byte() = pkey.GetPkcs8()
                Dim LlavePrivada As String = Convert.ToBase64String(Resultado)

                Dim FactureHoyUsuario As String = System.Configuration.ConfigurationManager.AppSettings("FactureHoyUsuario")
                Dim FactureHoyContrasena As String = System.Configuration.ConfigurationManager.AppSettings("FactureHoyContrasena")
                Dim FactureHoyProceso As String = System.Configuration.ConfigurationManager.AppSettings("FactureHoyProceso")

                Dim CancelaFH As New WsCancelacionFHCFDI33.WsCancelacionCFDI33()
                Dim Respuesta = CancelaFH.CancelarCFDI(FactureHoyUsuario, FactureHoyContrasena, FactureHoyProceso, Archivo, Certificado, LlavePrivada, email)
                If Respuesta.isError = False Then
                    If consignacionid > 0 Then
                        Dim ObjData As New DataControl
                        ObjData.RunSQLQuery("exec pCFD @cmd=36, @cfdid='" & cfdi.ToString & "', @consignacionid='" & consignacionid.ToString & "'")
                        ObjData = Nothing
                    Else
                        Dim ObjData As New DataControl
                        ObjData.RunSQLQuery("exec pCFD @cmd=21, @cfdid='" & cfdi.ToString & "'")
                        ObjData = Nothing
                    End If
                    lblError.Text = Respuesta.mensaje
                    lblError.ForeColor = System.Drawing.Color.Green
                Else
                    lblError.Text = Respuesta.mensaje
                End If
            End If
        Else
            Dim ObjData As New DataControl
            ObjData.RunSQLQuery("exec pCFD @cmd=21, @cfdid='" & cfdi.ToString & "'")
            ObjData = Nothing
        End If
    End Sub

    Private Sub CancelaSIFEI_OLD(ByVal cfdi As Long)
        '
        '   Obtiene serie y folio y construye nombre del XML
        '
        Dim consignacionid As Long = 0
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim email As String = ""
        Dim archivo_llave_privada As String = ""
        Dim contrasena_llave_privada As String = ""
        Dim archivoCertificado As String = ""

        Dim connX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("exec pCFD_Cancela @cfdid='" & cfdi.ToString & "'", connX)
        Try

            connX.Open()

            Dim rs As SqlDataReader
            rs = cmdX.ExecuteReader()

            If rs.Read Then
                consignacionid = rs("consignacionid")
                serie = rs("serie")
                folio = rs("folio")
                email = rs("email")
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
            Response.End()
        Finally
            connX.Close()
            connX.Dispose()
            connX = Nothing
        End Try

        Dim FlujoReader As XmlTextReader = Nothing
        Dim i As Integer
        FlujoReader = New XmlTextReader(Server.MapPath("~/portalcfd/cfd_storage") & "/" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
        FlujoReader.WhitespaceHandling = WhitespaceHandling.None
        While FlujoReader.Read()
            Select Case FlujoReader.NodeType
                Case XmlNodeType.Element
                    If FlujoReader.Name = "tfd:TimbreFiscalDigital" Then
                        For i = 0 To FlujoReader.AttributeCount - 1
                            FlujoReader.MoveToAttribute(i)
                            If FlujoReader.Name = "UUID" Then
                                uuids.Add(FlujoReader.Value)
                            End If
                        Next
                    ElseIf FlujoReader.Name = "cfdi:Emisor" Then
                        For i = 0 To FlujoReader.AttributeCount - 1
                            FlujoReader.MoveToAttribute(i)
                            If FlujoReader.Name = "Rfc" Then
                                RFCEmisor = FlujoReader.Value
                            End If
                        Next
                    End If
            End Select
        End While

        Try
            Dim SIFEIUsuario As String = System.Configuration.ConfigurationManager.AppSettings("SIFEIUsuario")
            Dim SIFEIContrasena As String = System.Configuration.ConfigurationManager.AppSettings("SIFEIContrasena")

            Dim PfxBytes As Byte() = ReadFile(Server.MapPath("~/portalcfd/certificados/") & CertificadoCliente() & ".pfx")

            'Pruebas
            'Dim sifei As New CancelacionPruebasSIFEI.Cancelacion()

            'Produccion
            Dim sifei As New CancelacionSIFEI.Cancelacion()

            Dim result = sifei.cancelaCFDI(SIFEIUsuario, SIFEIContrasena, RFCEmisor, PfxBytes, ContrasenaPfx(), uuids.ToArray())

            Dim xml As New XmlDocument()
            xml.LoadXml(result)
            xml.Save(Server.MapPath("~/portalcfd/cfd_storage/") & "acuse_" & uuids(0).ToString & ".xml")

            Dim EstatusUUID As String = ""
            Dim DescricionCodigo As String = ""
            EstatusUUID = GetXMLValue(Server.MapPath("~/portalcfd/cfd_storage/") & "acuse_" & uuids(0).ToString & ".xml", "EstatusUUID")

            If EstatusUUID = "201" Then

                'DescricionCodigo = "UUID Cancelado exitosamente"
                If consignacionid > 0 Then
                    Dim ObjData As New DataControl
                    ObjData.RunSQLQuery("exec pCFD @cmd=36, @cfdid='" & cfdi.ToString & "', @consignacionid='" & consignacionid.ToString & "'")
                    ObjData = Nothing
                Else
                    Dim ObjData As New DataControl
                    ObjData.RunSQLQuery("exec pCFD @cmd=21, @cfdid='" & cfdi.ToString & "'")
                    ObjData = Nothing
                End If

            ElseIf EstatusUUID = "202" Then
                DescricionCodigo = "UUID Previamente cancelado"

                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pCFD @cmd=21, @cfdid='" & cfdi.ToString & "'")
                ObjData = Nothing

            ElseIf EstatusUUID = "203" Then
                DescricionCodigo = "UUID No corresponde el RFC del Emisor y de quien solicita la cancelación"
            ElseIf EstatusUUID = "205" Then
                DescricionCodigo = "UUID No existe"
            ElseIf EstatusUUID = "300" Then
                DescricionCodigo = "Usuario y contraseña inválidos"
            ElseIf EstatusUUID = "301" Then
                DescricionCodigo = "XML mas formado"
            ElseIf EstatusUUID = "302" Then
                DescricionCodigo = "Sello mal formado o inválido"
            ElseIf EstatusUUID = "303" Then
                DescricionCodigo = "Sello no corresponde a emisor"
            ElseIf EstatusUUID = "304" Then
                DescricionCodigo = "Certificado Revocado o caduco"
            ElseIf EstatusUUID = "305" Then
                DescricionCodigo = "La fecha de emisión no esta dentro de la vigencia del CSD del Emisor"
            ElseIf EstatusUUID = "306" Then
                DescricionCodigo = "El certificado no es de tipo CSD"
            ElseIf EstatusUUID = "307" Then
                DescricionCodigo = "El CFDI contiene un timbre previo"
            ElseIf EstatusUUID = "308" Then
                DescricionCodigo = "Certificado no expedido por el SAT"
            ElseIf EstatusUUID = "401" Then
                DescricionCodigo = "Fecha y hora de generación fuera de rango"
            ElseIf EstatusUUID = "402" Then
                DescricionCodigo = "RFC del emisor no se encuentra en el régimen de contribuyentes"
            ElseIf EstatusUUID = "403" Then
                DescricionCodigo = "La fecha de emisión no es posterior al 01 de enero de 2012"
            ElseIf EstatusUUID = "501" Then
                DescricionCodigo = "Autenticación no válida"
            ElseIf EstatusUUID = "203" Then
                DescricionCodigo = "UUID No corresponde el RFC del Emisor y de quien solicita la cancelación"
            ElseIf EstatusUUID = "703" Then
                DescricionCodigo = "Cuenta suspendida"
            ElseIf EstatusUUID = "704" Then
                DescricionCodigo = "Error con la contraseña de la llave Privada"
            ElseIf EstatusUUID = "705" Then
                DescricionCodigo = "XML estructura inválida"
            ElseIf EstatusUUID = "706" Then
                DescricionCodigo = "Socio Inválido"
            ElseIf EstatusUUID = "707" Then
                DescricionCodigo = "XML ya contiene un nodo TimbreFiscalDigital"
            ElseIf EstatusUUID = "708" Then
                DescricionCodigo = "No se pudo conectar al SAT"
            End If

            If EstatusUUID <> "201" Then
                RadAlert.RadAlert(DescricionCodigo, 330, 180, "Alerta", "", "")
            End If

        Catch ex As SoapException
            Response.Write(ex.Detail.InnerText.ToString)
            Response.End()
        End Try

    End Sub

    Public Function GetXMLValue(ByVal url As String, ByVal nodo As String) As String
        Dim valor As String = ""
        Try
            Dim xmlDoc As New XmlDocument()
            xmlDoc.Load(url)

            Dim parentNode As XmlNodeList = xmlDoc.GetElementsByTagName(nodo)
            For Each childrenNode As XmlNode In parentNode
                valor = childrenNode.InnerText
            Next
        Catch ex As Exception
            valor = ""
        End Try
        Return valor
    End Function

    Private Function ContrasenaPfx() As String
        Dim contrasena_llave_privada As String = ""
        Dim ObjData As New DataControl
        contrasena_llave_privada = ObjData.RunSQLScalarQueryString("select top 1 isnull(contrasena_llave_privada,'') as contrasena_llave_privada from tblCliente")
        ObjData = Nothing
        Return contrasena_llave_privada
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

    Public Function CertificadoCliente() As String
        Dim certificado As String = ""
        Dim ObjData As New DataControl
        certificado = ObjData.RunSQLScalarQueryString("select top 1 isnull(archivoCertificado,'') as archivoCertificado from tblMisCertificados where isnull(activo,0)=1")
        Dim elements() As String = certificado.Split(New Char() {"."c}, StringSplitOptions.RemoveEmptyEntries)
        ObjData = Nothing
        Return elements(0)
    End Function

    Public Function ReadFile(ByVal strArchivo As String) As Byte()
        Dim f As New FileStream(strArchivo, FileMode.Open, System.IO.FileAccess.Read)
        Dim size As Integer = CInt(f.Length)
        Dim data As Byte() = New Byte(size - 1) {}
        size = f.Read(data, 0, size)
        f.Close()
        Return data
    End Function

    Private Sub MessageBox(ByVal strMsg As String)
        Dim lbl As New Label
        lbl.Text = "<script language='javascript'>" & Environment.NewLine & "window.alert(" & "'" & strMsg & "'" & ")</script>"
        Page.Controls.Add(lbl)
    End Sub

    Private Function EsDocumento(ByVal cfdi As Long) As Boolean
        Dim es As Boolean = False
        Dim connX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("select isnull(formatoBit,0) as formatoBit from tblCFD where id='" & cfdi.ToString & "'", connX)
        Try

            connX.Open()

            Dim rs As SqlDataReader
            rs = cmdX.ExecuteReader()

            If rs.Read Then
                If rs("formatoBit") = 1 Then
                    es = True
                End If
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
        Return es
    End Function

    Private Sub cfdnotimbrado()
        Dim Objdata As New DataControl
        Objdata.RunSQLQuery("exec pCFD @cmd=23, @cfdid='" & Session("CFD").ToString & "'")
        Objdata = Nothing
    End Sub

    Private Sub cfdtimbrado()
        Dim Objdata As New DataControl
        Objdata.RunSQLQuery("exec pCFD @cmd=24, @cfdid='" & Session("CFD").ToString & "'")
        Objdata = Nothing
    End Sub

    Private Sub GuardaPDF(ByVal report As Telerik.Reporting.Report, ByVal fileName As String)
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor()
        Dim result As RenderingResult = reportProcessor.RenderReport("PDF", report, Nothing)
        Using fs As New FileStream(fileName, FileMode.Create)
            fs.Write(result.DocumentBytes, 0, result.DocumentBytes.Length)
        End Using
    End Sub

    Private Function GeneraPDF(ByVal cfdid As Long) As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim tipoid As Integer = 0
        Dim numeroaprobacion As String = ""
        Dim anoAprobacion As String = ""
        Dim fechaHora As String = ""
        Dim noCertificado As String = ""
        Dim razonsocial As String = ""
        Dim callenum As String = ""
        Dim colonia As String = ""
        Dim ciudad As String = ""
        Dim rfc As String = ""
        Dim em_razonsocial As String = ""
        Dim em_callenum As String = ""
        Dim em_colonia As String = ""
        Dim em_ciudad As String = ""
        Dim em_rfc As String = ""
        Dim em_regimen As String = ""
        Dim importe As Decimal = 0
        Dim importetasacero As Decimal = 0
        Dim importe_descuento As Decimal = 0
        Dim iva As Decimal = 0
        Dim total As Decimal = 0
        Dim CantidadTexto As String = ""
        Dim condiciones As String = ""
        Dim enviara As String = ""
        Dim instrucciones As String = ""
        Dim pedimento As String = ""
        Dim retencion As Decimal = 0
        Dim monedaid As Integer = 1
        Dim expedicionLinea1 As String = ""
        Dim expedicionLinea2 As String = ""
        Dim expedicionLinea3 As String = ""
        Dim porcentaje As Decimal = 0
        Dim plantillaid As Integer = 1
        Dim metodopago As String = ""
        Dim formapago As String = ""
        Dim numctapago As String = ""
        Dim serie As String = ""
        Dim folio As Integer = 0
        Dim usoCFDI As String = ""
        Dim tipo_comprobante As String = ""
        Dim tiporelacion As String = ""
        Dim uuid_relacionado As String = ""
        Dim tipocontribuyenteid As Integer = 0

        Dim ds As DataSet = New DataSet

        Try
            Dim cmd As New SqlCommand("EXEC pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                tipoid = rs("tipoid")
                serie = rs("serie")
                folio = rs("folio")
                tipoid = rs("tipoid")
                em_razonsocial = rs("em_razonsocial")
                em_callenum = rs("em_callenum")
                em_colonia = rs("em_colonia")
                em_ciudad = rs("em_ciudad")
                em_rfc = rs("em_rfc")
                em_regimen = rs("regimen")
                razonsocial = rs("razonsocial")
                callenum = rs("callenum")
                colonia = rs("colonia")
                ciudad = rs("ciudad")
                rfc = rs("rfc")
                importe = rs("importe")
                importetasacero = rs("importetasacero")
                importe_descuento = rs("importe_descuento")
                iva = rs("iva")
                total = rs("total")
                monedaid = rs("monedaid")
                fechaHora = rs("fecha_factura").ToString
                condiciones = rs("condiciones").ToString
                enviara = rs("enviara").ToString
                instrucciones = rs("instrucciones")
                If rs("aduana") = "" Or rs("numero_pedimento") = "" Then
                    pedimento = ""
                Else
                    pedimento = "Aduana: " & rs("aduana") & vbCrLf & "Fecha: " & rs("fecha_pedimento").ToString & vbCrLf & "Número: " & rs("numero_pedimento").ToString
                End If
                expedicionLinea1 = rs("expedicionLinea1")
                expedicionLinea2 = rs("expedicionLinea2")
                expedicionLinea3 = rs("expedicionLinea3")
                porcentaje = rs("porcentaje")
                plantillaid = rs("plantillaid")
                tipocontribuyenteid = rs("tipocontribuyenteid")
                metodopago = rs("metodopago")
                formapago = rs("formapago")
                numctapago = rs("numctapago")
                usoCFDI = rs("usocfdi")
                tiporelacion = rs("tiporelacion")
                uuid_relacionado = rs("uuid_relacionado")
            End If
            rs.Close()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try

        Dim largo = Len(CStr(Format(CDbl(total), "#,###.00")))
        Dim decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)

        If monedaid = 1 Then
            CantidadTexto = "Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N."
        Else
            CantidadTexto = "Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD"
        End If

        Select Case tipoid
            Case 3, 6, 7      ' honorarios y arrendamiento
                Dim reporte As New Formatos.formato_cfdi_honorarios33
                reporte.ReportParameters("plantillaId").Value = plantillaid
                reporte.ReportParameters("cfdiId").Value = cfdid
                Select Case tipoid
                    Case 3
                        reporte.ReportParameters("txtDocumento").Value = "Arrendamiento No.    " & serie.ToString & folio.ToString
                    Case 6
                        reporte.ReportParameters("txtDocumento").Value = "Honorarios No.    " & serie.ToString & folio.ToString
                    Case 7
                        reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
                End Select
                reporte.ReportParameters("txtCondicionesPago").Value = condiciones
                reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb/" & serie.ToString & folio.ToString & ".png")
                reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
                reporte.ReportParameters("txtFechaEmision").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Fecha", "cfdi:Comprobante")
                reporte.ReportParameters("txtFechaCertificacion").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "FechaTimbrado", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtUUID").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtPACCertifico").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "RfcProvCertif", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtSerieEmisor").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificado", "cfdi:Comprobante")
                reporte.ReportParameters("txtSerieCertificadoSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificadoSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtClienteRazonSocial").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Nombre", "cfdi:Receptor")
                reporte.ReportParameters("txtClienteCalleNum").Value = callenum
                reporte.ReportParameters("txtClienteColonia").Value = colonia
                reporte.ReportParameters("txtClienteCiudadEstado").Value = ciudad
                reporte.ReportParameters("txtClienteRFC").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
                reporte.ReportParameters("txtSelloDigitalCFDI").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Sello", "cfdi:Comprobante")
                reporte.ReportParameters("txtSelloDigitalSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtPedimento").Value = pedimento
                reporte.ReportParameters("txtEnviarA").Value = enviara

                If tipocontribuyenteid = 1 Then
                    reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
                    'reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                    reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                    reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe + iva, 2).ToString
                    reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
                    reporte.ReportParameters("txtRetIva").Value = FormatCurrency(0, 2).ToString
                    reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva), 2).ToString
                    '
                    '   Ajusta cantidad con texto
                    '
                    total = FormatNumber((importe + iva), 2)
                    largo = Len(CStr(Format(CDbl(total), "#,###.00")))
                    decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
                    CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
                    '
                Else
                    If tipoid = 7 Then
                        reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
                        'reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                        reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                        reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe + iva, 2).ToString
                        reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
                        reporte.ReportParameters("txtRetIva").Value = FormatCurrency((iva * 0.1), 2).ToString
                        reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva) - ((iva * 0.1)), 2).ToString
                        '
                        '   Ajusta cantidad con texto
                        '
                        total = FormatNumber((importe + iva) - ((iva * 0.1)), 2)
                        largo = Len(CStr(Format(CDbl(total), "#,###.00")))
                        decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
                        CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
                        '
                    Else
                        reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
                        'reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                        reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                        reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe + iva, 2).ToString
                        reporte.ReportParameters("txtRetISR").Value = FormatCurrency(importe * 0.1, 2).ToString
                        reporte.ReportParameters("txtRetIva").Value = FormatCurrency((iva / 3) * 2, 2).ToString
                        reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva) - (importe * 0.1) - ((iva / 3) * 2), 2).ToString
                        '
                        '   Ajusta cantidad con texto
                        '
                        total = FormatNumber((importe + iva) - (importe * 0.1) - ((iva / 3) * 2), 2)
                        largo = Len(CStr(Format(CDbl(total), "#,###.00")))
                        decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
                        If monedaid = 1 Then
                            CantidadTexto = "Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N."
                        Else
                            CantidadTexto = "Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD"
                        End If
                    End If
                End If
                reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto
                reporte.ReportParameters("txtCadenaOriginal").Value = CadenaOriginalComplemento(serie, folio)
                reporte.ReportParameters("txtEmisorRazonSocial").Value = em_razonsocial
                reporte.ReportParameters("txtLugarExpedicion").Value = expedicionLinea1 & vbCrLf & expedicionLinea2 & vbCrLf & expedicionLinea3
                If porcentaje > 0 Then
                    reporte.ReportParameters("txtInteres").Value = porcentaje.ToString
                End If
                reporte.ReportParameters("txtRegimen").Value = em_regimen.ToString
                reporte.ReportParameters("txtMetodoPago").Value = metodopago.ToString
                reporte.ReportParameters("txtFormaPago").Value = formapago.ToString
                reporte.ReportParameters("txtUsoCFDI").Value = usoCFDI.ToString
                reporte.ReportParameters("txtNumCtaPago").Value = "Núm. cuenta: " & numctapago.ToString
                reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString
                Return reporte
            Case Else
                Dim reporte As New formato_cfdi40_natural
                reporte.ReportParameters("plantillaId").Value = plantillaid
                reporte.ReportParameters("cfdiId").Value = cfdid
                Select Case tipoid
                    Case 1, 4, 7
                        reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
                    Case 2, 8
                        reporte.ReportParameters("txtDocumento").Value = "Nota de Crédito No. " & serie.ToString & folio.ToString
                    Case 5
                        reporte.ReportParameters("txtDocumento").Value = "Carta Porte No.    " & serie.ToString & folio.ToString
                        reporte.ReportParameters("txtLeyenda").Value = "IMPUESTO RETENIDO DE CONFORMIDAD CON LA LEY DEL IMPUESTO AL VALOR AGREGADO     EFECTOS FISCALES AL PAGO"
                    Case 6
                        reporte.ReportParameters("txtDocumento").Value = "Honorarios No.    " & serie.ToString & folio.ToString
                    Case Else
                        reporte.ReportParameters("txtDocumento").Value = "Factura No.    " & serie.ToString & folio.ToString
                End Select
                reporte.ReportParameters("txtCondicionesPago").Value = condiciones
                reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb/" & serie.ToString & folio.ToString & ".png")
                reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
                reporte.ReportParameters("txtFechaEmision").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Fecha", "cfdi:Comprobante")
                reporte.ReportParameters("txtFechaCertificacion").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "FechaTimbrado", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtUUID").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtPACCertifico").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "RfcProvCertif", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtSerieEmisor").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificado", "cfdi:Comprobante")
                reporte.ReportParameters("txtSerieCertificadoSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "NoCertificadoSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtClienteRazonSocial").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Nombre", "cfdi:Receptor")
                reporte.ReportParameters("txtClienteCalleNum").Value = callenum
                reporte.ReportParameters("txtClienteColonia").Value = colonia
                reporte.ReportParameters("txtClienteCiudadEstado").Value = ciudad
                reporte.ReportParameters("txtClienteRFC").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
                reporte.ReportParameters("txtSelloDigitalCFDI").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Sello", "cfdi:Comprobante")
                reporte.ReportParameters("txtSelloDigitalSAT").Value = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloSAT", "tfd:TimbreFiscalDigital")
                reporte.ReportParameters("txtInstrucciones").Value = instrucciones
                reporte.ReportParameters("txtPedimento").Value = pedimento
                reporte.ReportParameters("txtEnviarA").Value = enviara
                reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto
                reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe, 2).ToString
                reporte.ReportParameters("txtTasaCero").Value = FormatCurrency(importetasacero, 2).ToString
                reporte.ReportParameters("txtEtiquetaIVA").Value = "IVA 16%"
                'reporte.ReportParameters("txtEtiquetaRetIVA").Value = "- RET IVA"
                reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
                reporte.ReportParameters("txtDescuento").Value = FormatCurrency(importe_descuento, 2).ToString
                reporte.ReportParameters("txtRetIVA").Value = FormatCurrency(0, 2).ToString
                reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
                reporte.ReportParameters("txtTotal").Value = FormatCurrency(total, 2).ToString
                reporte.ReportParameters("txtCadenaOriginal").Value = CadenaOriginalComplemento(serie, folio)
                reporte.ReportParameters("txtEmisorRazonSocial").Value = em_razonsocial
                reporte.ReportParameters("txtLugarExpedicion").Value = expedicionLinea1 & vbCrLf & expedicionLinea2 & vbCrLf & expedicionLinea3
                tipo_comprobante = GetXmlAttribute(Server.MapPath("~/portalCFD/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "TipoDeComprobante", "cfdi:Comprobante")

                If tipo_comprobante = "I" Then
                    tipo_comprobante = "Ingreso"
                ElseIf tipo_comprobante = "E" Then
                    tipo_comprobante = "Egreso"
                ElseIf tipo_comprobante = "N" Then
                    tipo_comprobante = "Nómina"
                ElseIf tipo_comprobante = "P" Then
                    tipo_comprobante = "Pago"
                ElseIf tipo_comprobante = "T" Then
                    tipo_comprobante = "Traslado"
                End If

                reporte.ReportParameters("txtTipoComprobante").Value = tipo_comprobante

                If tipoid = 2 Then
                    reporte.ReportParameters("txtTipoRelacion").Value = "Tipo Relación: " & tiporelacion
                    'reporte.ReportParameters("txtUUIDRelacionado").Value = "UUID Relacionado: " & uuid_relacionado
                    Dim OBJ As New DataControl
                    Dim dt As DataSet = OBJ.FillDataSet("exec pCFD @cmd=46, @cfdid='" & cfdid.ToString & "'")
                    Dim nodoc As Integer = 0
                    For Each row As DataRow In dt.Tables(0).Rows
                        nodoc += 1
                    Next
                    reporte.ReportParameters("txtUUIDRelacionado").Value = "UUID Relacionado(s): " & nodoc.ToString
                ElseIf tipoid = 5 Then
                    retencion = FormatNumber((importe * 0.04), 2)
                    reporte.ReportParameters("txtRetencion").Value = FormatCurrency(retencion, 2).ToString
                    reporte.ReportParameters("txtTotal").Value = FormatCurrency(total - retencion, 2).ToString
                    largo = Len(CStr(Format(CDbl(total - retencion), "#,###.00")))
                    decimales = Mid(CStr(Format(CDbl(total - retencion), "#,###.00")), largo - 2)
                    If monedaid = 1 Then
                        CantidadTexto = "Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N."
                    Else
                        CantidadTexto = "Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD"
                    End If
                    reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto.ToString
                End If

                reporte.ReportParameters("txtRegimen").Value = em_regimen.ToString
                reporte.ReportParameters("txtMetodoPago").Value = metodopago.ToString
                reporte.ReportParameters("txtFormaPago").Value = formapago.ToString
                If numctapago.Length > 0 Then
                    reporte.ReportParameters("txtNumCtaPago").Value = "Núm. cuenta: " & numctapago.ToString
                End If
                reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString
                reporte.ReportParameters("txtUsoCFDI").Value = "Uso de CFDI: " & usoCFDI

                Return reporte

        End Select

    End Function

    Private Function GeneraPDF_Documento(ByVal cfdid As Long) As Telerik.Reporting.Report
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Dim serie As String = ""
        Dim folio As Long = 0
        Dim numeroaprobacion As String = ""
        Dim anoAprobacion As String = ""
        Dim fechaHora As String = ""
        Dim noCertificado As String = ""
        Dim razonsocial As String = ""
        Dim callenum As String = ""
        Dim colonia As String = ""
        Dim ciudad As String = ""
        Dim rfc As String = ""
        Dim em_razonsocial As String = ""
        Dim em_callenum As String = ""
        Dim em_colonia As String = ""
        Dim em_ciudad As String = ""
        Dim em_rfc As String = ""
        Dim em_regimen As String = ""
        Dim rec_razonsocial As String = ""
        Dim rec_callenum As String = ""
        Dim rec_colonia As String = ""
        Dim rec_ciudad As String = ""
        Dim rec_rfc As String = ""

        Dim folio_aprobacion As String = ""
        Dim folio_emision As String = ""
        Dim folio_vigencia As String = ""
        Dim folio_rango As String = ""

        Dim importe As Decimal = 0
        Dim importetasacero As Decimal = 0
        Dim iva As Decimal = 0
        Dim total As Decimal = 0
        Dim CantidadTexto As String = ""
        Dim condiciones As String = ""
        Dim enviara As String = ""
        Dim instrucciones As String = ""
        Dim pedimento As String = ""
        Dim retencion As Decimal = 0
        Dim tipoid As Integer = 0
        Dim divisaid As Integer = 1
        Dim expedicionLinea1 As String = ""
        Dim expedicionLinea2 As String = ""
        Dim expedicionLinea3 As String = ""
        Dim porcentaje As Decimal = 0
        Dim plantillaid As Integer = 1
        Dim codigo_cbb As String = ""
        Dim tipopago As String = ""
        Dim formapago As String = ""
        Dim numctapago As String = ""
        Dim tipocontribuyenteid As Integer = 0

        Dim ds As DataSet = New DataSet

        Dim cmd As New SqlCommand("EXEC pCFD @cmd=18, @cfdid='" & cfdid.ToString & "'", conn)
        conn.Open()
        Dim rs As SqlDataReader
        rs = cmd.ExecuteReader()

        If rs.Read Then
            serie = rs("serie")
            folio = rs("folio")
            tipoid = rs("tipoid")
            em_razonsocial = rs("em_razonsocial")
            em_callenum = rs("em_callenum")
            em_colonia = rs("em_colonia")
            em_ciudad = rs("em_ciudad")
            em_rfc = rs("em_rfc")
            em_regimen = rs("regimen")
            '
            rec_razonsocial = rs("razonsocial")
            rec_callenum = rs("callenum")
            rec_colonia = rs("colonia")
            rec_ciudad = rs("ciudad")
            rec_rfc = rs("rfc")
            '
            folio_aprobacion = rs("folio_aprobacion")
            folio_emision = rs("folio_emision")
            folio_vigencia = rs("folio_vigencia")
            folio_rango = rs("folio_rango")
            '
            razonsocial = rs("razonsocial")
            callenum = rs("callenum")
            colonia = rs("colonia")
            ciudad = rs("ciudad")
            rfc = rs("rfc")
            importe = rs("importe")
            importetasacero = rs("importetasacero")
            iva = rs("iva")
            total = rs("total")
            divisaid = rs("divisaid")
            fechaHora = rs("fecha_factura").ToString
            condiciones = rs("condiciones").ToString
            enviara = rs("enviara").ToString
            instrucciones = rs("instrucciones")
            If rs("aduana") = "" Or rs("numero_pedimento") = "" Then
                pedimento = ""
            Else
                pedimento = "Aduana: " & rs("aduana") & vbCrLf & "Fecha: " & rs("fecha_pedimento").ToString & vbCrLf & "Número: " & rs("numero_pedimento").ToString
            End If
            expedicionLinea1 = rs("expedicionLinea1")
            expedicionLinea2 = rs("expedicionLinea2")
            expedicionLinea3 = rs("expedicionLinea3")
            porcentaje = rs("porcentaje")
            plantillaid = rs("plantillaid")
            tipocontribuyenteid = rs("tipocontribuyenteid")
            codigo_cbb = rs("codigo_cbb")
            tipopago = rs("tipopago")
            formapago = rs("formapago")
            numctapago = rs("numctapago")
        End If
        rs.Close()

        conn.Close()
        conn.Dispose()
        conn = Nothing

        Dim largo = Len(CStr(Format(CDbl(total), "#,###.00")))
        Dim decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)

        If System.Configuration.ConfigurationManager.AppSettings("divisas") = 1 Then
            If divisaid = 1 Then
                CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
            Else
                CantidadTexto = "( Son " + Num2Text(total - decimales) & " dólares " & Mid(decimales, Len(decimales) - 1) & "/100 USD. )"
            End If
        Else
            CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"
        End If


        Dim reporte As New Formatos.formato_cbb_neogenis


        reporte.ReportParameters("plantillaId").Value = plantillaid
        reporte.ReportParameters("cfdiId").Value = cfdid
        reporte.ReportParameters("txtFechaEmision").Value = Now.ToShortDateString

        Select Case tipoid
            Case 10
                reporte.ReportParameters("txtDocumento").Value = "Remisión No.    " & serie.ToString & folio.ToString
                reporte.ReportParameters("txtObservaciones4").Value = "NOTA: ESTE COMPROBANTE NO TIENE VALOR FISCAL"
        End Select

        reporte.ReportParameters("txtNoAprobacion").Value = "Aprobación No. " & folio_aprobacion.ToString
        reporte.ReportParameters("txtEmision").Value = folio_emision.ToString
        reporte.ReportParameters("txtRango").Value = folio_rango.ToString

        reporte.ReportParameters("txtCondicionesPago").Value = condiciones
        reporte.ReportParameters("paramImgCBB").Value = Server.MapPath("~/portalcfd/cbb/nocbb.png")
        reporte.ReportParameters("txtLeyenda").Value = ""

        reporte.ReportParameters("paramImgBanner").Value = Server.MapPath("~/portalcfd/logos/" & Session("logo_formato"))
        reporte.ReportParameters("txtClienteRazonSocial").Value = rec_razonsocial.ToString
        reporte.ReportParameters("txtClienteCalleNum").Value = rec_callenum.ToString
        reporte.ReportParameters("txtClienteColonia").Value = rec_colonia.ToString
        reporte.ReportParameters("txtClienteCiudadEstado").Value = rec_ciudad.ToString
        reporte.ReportParameters("txtClienteRFC").Value = rec_rfc.ToString
        '
        '
        reporte.ReportParameters("txtInstrucciones").Value = instrucciones
        reporte.ReportParameters("txtPedimento").Value = pedimento
        reporte.ReportParameters("txtEnviarA").Value = enviara

        '
        reporte.ReportParameters("txtImporte").Value = FormatCurrency(importe, 2).ToString
        reporte.ReportParameters("txtIVA").Value = FormatCurrency(iva, 2).ToString
        reporte.ReportParameters("txtSubtotal").Value = FormatCurrency(importe, 2).ToString
        reporte.ReportParameters("txtRetIVA").Value = FormatCurrency(0, 2).ToString
        reporte.ReportParameters("txtRetISR").Value = FormatCurrency(0, 2).ToString
        reporte.ReportParameters("txtTotal").Value = FormatCurrency((importe + iva), 2).ToString
        '
        '   Ajusta cantidad con texto
        '
        total = FormatNumber((importe + iva), 2)
        largo = Len(CStr(Format(CDbl(total), "#,###.00")))
        decimales = Mid(CStr(Format(CDbl(total), "#,###.00")), largo - 2)
        '
        CantidadTexto = "( Son " + Num2Text(total - decimales) & " pesos " & Mid(decimales, Len(decimales) - 1) & "/100 M.N. )"

        reporte.ReportParameters("txtCantidadLetra").Value = CantidadTexto
        '
        '
        reporte.ReportParameters("txtEmisorRazonSocial").Value = em_razonsocial
        reporte.ReportParameters("txtLugarExpedicion").Value = expedicionLinea1 & vbCrLf & expedicionLinea2 & vbCrLf & expedicionLinea3
        If porcentaje > 0 Then
            reporte.ReportParameters("txtInteres").Value = porcentaje.ToString
        End If
        '
        '
        reporte.ReportParameters("txtRegimen").Value = em_regimen.ToString
        reporte.ReportParameters("txtFormaPago").Value = tipopago.ToString
        reporte.ReportParameters("txtMetodoPago").Value = formapago.ToString
        reporte.ReportParameters("txtNumCtaPago").Value = "Núm. cuenta: " & numctapago.ToString
        reporte.ReportParameters("txtInstrucciones").Value = instrucciones.ToString
        reporte.ReportParameters("txtLeyenda").Value = ""

        Dim totalPzas As String
        Dim objData As New DataControl
        totalPzas = objData.RunSQLScalarQuery("exec pCFD @cmd=34, @cfdid='" & cfdid.ToString & "'")
        objData = Nothing

        reporte.ReportParameters("txtTotalPiezas").Value = totalPzas.ToString

        '
        Return reporte
    End Function

    Public Function Num2Text(ByVal nCifra As Object) As String
        ' Defino variables 
        Dim cifra, bloque, decimales, cadena As String
        Dim longituid, posision, unidadmil As Byte

        ' En caso de que unidadmil sea: 
        ' 0 = cientos 
        ' 1 = miles 
        ' 2 = millones 
        ' 3 = miles de millones 
        ' 4 = billones 
        ' 5 = miles de billones 

        ' Reemplazo el símbolo decimal por un punto (.) y luego guardo la parte entera y la decimal por separado 
        ' Es necesario poner el cero a la izquierda del punto así si el valor es de sólo decimales, se lo fuerza 
        ' a colocar el cero para que no genere error 
        cifra = Format(CType(nCifra, Decimal), "###############0.#0")
        decimales = Mid(cifra, Len(cifra) - 1, 2)
        cifra = Left(cifra, Len(cifra) - 3)

        ' Verifico que el valor no sea cero 
        If cifra = "0" Then
            Return IIf(decimales = "00", "cero", "cero con " & decimales & "/100")
        End If

        ' Evaluo su longitud (como mínimo una cadena debe tener 3 dígitos) 
        If Len(cifra) < 3 Then
            cifra = Rellenar(cifra, 3)
        End If

        ' Invierto la cadena 
        cifra = Invertir(cifra)

        ' Inicializo variables 
        posision = 1
        unidadmil = 0
        cadena = ""

        ' Selecciono bloques de a tres cifras empezando desde el final (de la cadena invertida) 
        Do While posision <= Len(cifra)
            ' Selecciono una porción del numero 
            bloque = Mid(cifra, posision, 3)

            ' Transformo el número a cadena 
            cadena = Convertir(bloque, unidadmil) & " " & cadena.Trim

            ' Incremento la cantidad desde donde seleccionar la subcadena 
            posision = posision + 3

            ' Incremento la posisión de la unidad de mil 
            unidadmil = unidadmil + 1
        Loop

        ' Cargo la función 
        Return IIf(decimales = "00", cadena.Trim.ToLower, cadena.Trim.ToLower & " con " & decimales & "/100")
    End Function

    ' Esta función es complemento de la función de conversión. 
    ' En los arrays se agrega una posisión inicial vacía ya que VB.NET empieza de la posisión cero 
    Private Function Convertir(ByVal cadena As String, ByVal unidadmil As Byte) As String
        ' Defino variables 
        Dim centena, decena, unidad As Byte

        ' Invierto la subcadena (la original habia sido invertida en el procedimiento NumeroATexto) 
        cadena = Invertir(cadena)

        ' Determino la longitud de la cadena 
        If Len(cadena) < 3 Then
            cadena = Rellenar(cadena, 3)
        End If

        ' Verifico que la cadena no esté vacía (000) 
        If cadena = "000" Then
            Return ""
        End If

        ' Desarmo el numero (empiezo del dígito cero por el manejo de cadenas de VB.NET) 
        centena = CType(cadena.Substring(0, 1), Byte)
        decena = CType(cadena.Substring(1, 1), Byte)
        unidad = CType(cadena.Substring(2, 1), Byte)
        cadena = ""

        ' Calculo las centenas 
        If centena <> 0 Then
            Dim centenas() As String = {"", IIf(decena = 0 And unidad = 0, "cien", "ciento"), "doscientos", "trescientos", "cuatrocientos", "quinientos", "seiscientos", "setecientos", "ochocientos", "novecientos"}
            cadena = centenas(centena)
        End If

        ' Calculo las decenas 
        If decena <> 0 Then
            Dim decenas() As String = {"", IIf(unidad = 0, "diez", IIf(unidad >= 6, "dieci", IIf(unidad = 1, "once", IIf(unidad = 2, "doce", IIf(unidad = 3, "trece", IIf(unidad = 4, "catorce", "quince")))))), IIf(unidad = 0, "veinte", "venti"), "treinta", "cuarenta", "cincuenta", "sesenta", "setenta", "ochenta", "noventa"}
            cadena = cadena & " " & decenas(decena)
        End If

        ' Calculo las unidades (no pregunten por que este IF es necesario ... simplemente funciona) 
        If decena = 1 And unidad < 6 Then
        Else
            Dim unidades() As String = {"", IIf(decena <> 1, IIf(unidadmil = 1, "un", "uno"), ""), "dos", "tres", "cuatro", "cinco", "seis", "siete", "ocho", "nueve"}
            If decena >= 3 And unidad <> 0 Then
                cadena = cadena.Trim & " y "
            End If

            If decena = 0 Then
                cadena = cadena.Trim & " "
            End If
            cadena = cadena & unidades(unidad)
        End If

        ' Evaluo la posision de miles, millones, etc 
        If unidadmil <> 0 Then
            Dim agregado() As String = {"", "mil", IIf((centena = 0) And (decena = 0) And (unidad = 1), "millón", "millones"), "mil millones", "billones", "mil billones"}
            If (centena = 0) And (decena = 0) And (unidad = 1) And unidadmil = 2 Then
                cadena = "un"
            End If
            cadena = cadena & " " & agregado(unidadmil)
        End If

        ' Cargo la función 
        Return cadena.Trim
    End Function

    ' Esta función recibe una cadena de caracteres y la devuelve "invertida". 
    Public Function Invertir(ByVal cadena As String) As String
        ' Defino variables 
        Dim retornar As String

        ' Inviero la cadena 
        For posision As Short = cadena.Length To 1 Step -1
            retornar = retornar & cadena.Substring(posision - 1, 1)
        Next

        ' Retorno la cadena invertida 
        Return retornar
    End Function

    ' Esta función rellena con ceros a la izquierda un número pasado como parámetro. Con el parámetro "cifras" se especifica la cantidad de dígitos a la izquierda. 
    Public Function Rellenar(ByVal valor As Object, ByVal cifras As Byte) As String
        ' Defino variables 
        Dim cadena As String

        ' Verifico el valor pasado 
        If Not IsNumeric(valor) Then
            valor = 0
        Else
            valor = CType(valor, Integer)
        End If

        ' Cargo la cadena 
        cadena = valor.ToString.Trim

        ' Relleno con los ceros que sean necesarios para llenar los dígitos pedidos 
        For puntero As Byte = (Len(cadena) + 1) To cifras
            cadena = "0" & cadena
        Next puntero

        ' Cargo la función 
        Return cadena
    End Function

    Private Function CadenaOriginalComplemento(ByVal serie As String, ByVal folio As Long) As String
        '
        '   Obtiene los valores del timbre de respuesta
        '
        Dim Version As String = ""
        Dim selloSAT As String = ""
        Dim UUID As String = ""
        Dim noCertificadoSAT As String = ""
        Dim selloCFD As String = ""
        Dim fechaTimbrado As String = ""
        Dim RfcProvCertif As String = ""
        '
        Dim FlujoReader As XmlTextReader = Nothing
        Dim i As Integer
        ' leer del fichero e ignorar los nodos vacios
        FlujoReader = New XmlTextReader(Server.MapPath("~/portalcfd/cfd_storage") & "/" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
        FlujoReader.WhitespaceHandling = WhitespaceHandling.None
        ' analizar el fichero y presentar cada nodo
        While FlujoReader.Read()
            Select Case FlujoReader.NodeType
                Case XmlNodeType.Element
                    If FlujoReader.Name = "tfd:TimbreFiscalDigital" Then
                        For i = 0 To FlujoReader.AttributeCount - 1
                            FlujoReader.MoveToAttribute(i)
                            If FlujoReader.Name = "fechaTimbrado" Or FlujoReader.Name = "FechaTimbrado" Then
                                fechaTimbrado = FlujoReader.Value
                            ElseIf FlujoReader.Name = "UUID" Then
                                UUID = FlujoReader.Value
                            ElseIf FlujoReader.Name = "NoCertificadoSAT" Then
                                noCertificadoSAT = FlujoReader.Value
                            ElseIf FlujoReader.Name = "SelloCFD" Then
                                selloCFD = FlujoReader.Value
                            ElseIf FlujoReader.Name = "SelloSAT" Then
                                selloSAT = FlujoReader.Value
                            ElseIf FlujoReader.Name = "Version" Then
                                Version = FlujoReader.Value
                            ElseIf FlujoReader.Name = "RfcProvCertif" Then
                                RfcProvCertif = FlujoReader.Value
                            End If
                        Next
                    End If
            End Select
        End While
        '
        '
        Dim cadena As String = ""
        cadena = "||" & Version & "|" & UUID & "|" & fechaTimbrado & "|" & RfcProvCertif & "|" & selloCFD & "|" & noCertificadoSAT & "||"
        Return cadena
        '
        '
    End Function

    Private Sub generacbb(ByVal serie As String, ByVal folio As Long)
        Dim cadena As String = ""
        Dim UUID As String = ""
        Dim rfcE As String = ""
        Dim rfcR As String = ""
        Dim total As String = ""

        '
        '   Obtiene datos del cfdi para construir string del CBB
        '

        '
        rfcE = GetXmlAttribute(Server.MapPath("cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "rfc", "cfdi:Emisor")
        rfcR = GetXmlAttribute(Server.MapPath("cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "rfc", "cfdi:Receptor")
        total = GetXmlAttribute(Server.MapPath("cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "total", "cfdi:Comprobante")
        UUID = GetXmlAttribute(Server.MapPath("cfd_storage") & "\" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        '
        Dim fmt As String = "0000000000.000000"
        Dim totalDec As Decimal = CType(total, Decimal)
        total = totalDec.ToString(fmt)
        '
        cadena = "?re=" & rfcE.ToString & "&rr=" & rfcR.ToString & "&tt=" & total.ToString & "&id=" & UUID.ToString
        '
        Response.Write(cadena)
        '   Genera gráfico
        '
        Dim qrCodeEncoder As New QRCodeEncoder
        qrCodeEncoder.QRCodeEncodeMode = qrCodeEncoder.ENCODE_MODE.BYTE
        qrCodeEncoder.QRCodeScale = 4
        qrCodeEncoder.QRCodeVersion = 8
        qrCodeEncoder.QRCodeErrorCorrect = qrCodeEncoder.ERROR_CORRECTION.Q
        Dim image As Drawing.Image

        image = qrCodeEncoder.Encode(cadena)
        image.Save(Server.MapPath("~/portalCFD/cbb") & "\" & serie.ToString & folio.ToString & ".png", System.Drawing.Imaging.ImageFormat.Png)
        ''
    End Sub

    Private Sub VerAcuse(ByVal cfdi As Long)
        '
        '   Obtiene serie y folio y construye nombre del XML
        '
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim archivo_llave_privada As String = ""
        Dim contrasena_llave_privada As String = ""
        Dim archivoCertificado As String = ""
        Dim connX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("select isnull(b.serie,'') as serie, isnull(b.folio,0) as folio from tblCFD b where b.id='" & cfdi.ToString & "'", connX)
        Try

            connX.Open()

            Dim rs As SqlDataReader
            rs = cmdX.ExecuteReader()

            If rs.Read Then
                serie = rs("serie")
                folio = rs("folio")
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
            Response.End()
        Finally
            connX.Close()
            connX.Dispose()
            connX = Nothing
        End Try
        '
        Dim UrlSAT As String = ""
        Dim FinalSelloDigitalEmisor As String = ""

        Dim rfcE As String = ""
        Dim rfcR As String = ""
        Dim total As String = ""
        Dim UUID As String = ""
        Dim sello As String = ""
        '
        '   Obtiene datos del cfdi para construir string del CBB
        '
        rfcE = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Emisor")
        rfcR = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
        total = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Total", "cfdi:Comprobante")
        UUID = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        sello = GetXmlAttribute(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloCFD", "tfd:TimbreFiscalDigital")
        FinalSelloDigitalEmisor = Mid(sello, (Len(sello) - 7))
        '
        Dim totalDec As Decimal = CType(total, Decimal)
        '
        UrlSAT = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx" & "?id=" & UUID & "&re=" & rfcE & "&rr=" & rfcR & "&tt=" & totalDec.ToString & "&fe=" & FinalSelloDigitalEmisor

        RadWindow1.VisibleOnPageLoad = False
        RadWindow2.VisibleOnPageLoad = False
        'RadWindow3.VisibleOnPageLoad = False

        'Dim script As String = "function f(){openRadWindow('" & UrlSAT & "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);"
        'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, True)

        Response.Write("<script>window.open('" + UrlSAT + "','_blank');</script>")

    End Sub

#End Region

#Region "Events"
    Protected Sub btnView_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnView.Click
        '        
        Dim ObjData As New DataControl
        cfdlist.DataSource = ObjData.FillDataSet("exec pCFD @cmd=22, @serie='" & txtSerie.Text & "', @folio='" & txtFolio.Text & "', @userid='" & Session("userid").ToString & "'")
        cfdlist.DataBind()
        ObjData = Nothing
        '
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Call MuestraLista()
    End Sub
#End Region

#Region "Grid Filters"
    Private Sub SetGridFilters()
        cfdlist.GroupingSettings.CaseSensitive = False
        Dim Menu As GridFilterMenu = cfdlist.FilterMenu
        Dim item As RadMenuItem
        For Each item In Menu.Items
            'change the text for the StartsWith menu item
            If item.Text = "StartsWith" Then
                item.Text = "Empieza con"
            End If

            If item.Text = "NoFilter" Then
                item.Text = "Sin Filtro"
            End If

            If item.Text = "EqualTo" Then
                item.Text = "Igual a"
            End If

            If item.Text = "EndsWith" Then
                item.Text = "Termina con"
            End If


        Next

    End Sub

    Private Sub cfdlist_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles cfdlist.Init
        cfdlist.PagerStyle.NextPagesToolTip = "Ver mas"
        cfdlist.PagerStyle.NextPageToolTip = "Siguiente"
        cfdlist.PagerStyle.PrevPagesToolTip = "Ver mas"
        cfdlist.PagerStyle.PrevPageToolTip = "Atras"
        cfdlist.PagerStyle.LastPageToolTip = "Ultima Página"
        cfdlist.PagerStyle.FirstPageToolTip = "Primera Pagina"
        cfdlist.PagerStyle.PagerTextFormat = "{4}    Pagina {0} de {1}, Registros {2} al {3} de {5}"
        cfdlist.SortingSettings.SortToolTip = "Ordernar"
        cfdlist.SortingSettings.SortedAscToolTip = "Ordenar Asc"
        cfdlist.SortingSettings.SortedDescToolTip = "Ordenar Desc"


        Dim menu As GridFilterMenu = cfdlist.FilterMenu
        Dim i As Integer = 0
        While i < menu.Items.Count
            If menu.Items(i).Text = "NoFilter" Or _
               menu.Items(i).Text = "EqualTo" Then
                i = i + 1
            Else
                menu.Items.RemoveAt(i)
            End If
        End While
    End Sub

    Private Sub btnSendEmail_Click(sender As Object, e As EventArgs) Handles btnSendEmail.Click

        Dim serie As String = ""
        Dim folio As Long = 0
        Dim connF As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdF As New SqlCommand("exec pCFD @cmd=18, @cfdid='" & tempcfdid.Value.ToString & "'", connF)
        Try

            connF.Open()

            Dim rsF = cmdF.ExecuteReader

            If rsF.Read Then
                serie = rsF("serie").ToString
                folio = rsF("folio").ToString
            End If
        Catch ex As Exception
            '
        Finally
            connF.Close()
            connF.Dispose()
            connF = Nothing
        End Try
        '
        '   Obtiene datos de la persona
        '
        Dim mensaje As String = ""
        Dim razonsocial As String = ""
        'Dim correo As String = ""
        'Dim correo_cc As String = ""
        'Dim correo_cco As String = ""
        Dim email_from As String = ""
        Dim email_sender As String = ""
        Dim email_smtp_server As String = ""
        Dim email_smtp_username As String = ""
        Dim email_smtp_password As String = ""
        Dim email_smtp_port As String = ""
        '
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        conn.Open()
        Dim SqlCommand As SqlCommand = New SqlCommand("exec pEnviaEmail @cfdid='" & tempcfdid.Value.ToString & "'", conn)
        Dim rs = SqlCommand.ExecuteReader
        If rs.Read Then
            '       
            razonsocial = rs("razonsocial")
            'correo = rs("email_to")
            'correo_cc = rs("email_cc")
            'correo_cco = rs("email_cco")
            email_from = rs("email_from")
            'email_sender = ("naturalgs@linkium.mx")
            email_smtp_server = rs("email_smtp_server")
            email_smtp_username = rs("email_smtp_username")
            email_smtp_password = rs("email_smtp_password")
            email_smtp_port = rs("email_smtp_port")

            '
        End If
        conn.Close()
        conn.Dispose()
        conn = Nothing

        Dim delimit As Char() = New Char() {";"c, ","c}
        Dim novalidos As String = ""

        Dim objMM As New MailMessage

        For Each splitTo As String In txtCC.Text.Trim().Split(delimit)
            If validarEmail(splitTo.Trim()) Then
                objMM.CC.Add(splitTo.Trim())
            Else

                If novalidos = "" Then
                    novalidos += splitTo.Trim()
                Else
                    novalidos += "," & splitTo.Trim()
                End If

            End If
        Next

        If novalidos = "" Then
            objMM.To.Add(txtTo.Text.Trim())
            If txtCC.Text.ToString().Length > 0 Then
                objMM.CC.Add(txtCC.Text.Trim())
            End If
            If txtCCO.Text.ToString().Length > 0 Then
                objMM.CC.Add(txtCCO.Text.Trim())
            End If
            objMM.From = New MailAddress(email_from, razonsocial)
            'objMM.Sender = New MailAddress(email_sender)
            objMM.IsBodyHtml = False
            objMM.Priority = MailPriority.Normal
            objMM.Subject = txtSubject.Text
            objMM.Body = txtMenssage.Text
            '
            '   Agrega anexos
            '
            Dim AttachXML As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("~/portalcfd/cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
            Dim AttachPDF As Net.Mail.Attachment = New Net.Mail.Attachment(Server.MapPath("~/portalcfd/pdf/") & "ng_" & serie.ToString & folio.ToString & ".pdf")
            '
            objMM.Attachments.Add(AttachXML)
            objMM.Attachments.Add(AttachPDF)
            '
            Dim SmtpMail As New SmtpClient
            Try
                'Dim SmtpUser As New Net.NetworkCredential
                'SmtpUser.UserName = email_smtp_username
                'SmtpUser.Password = email_smtp_password
                'SmtpUser.Domain = email_smtp_server
                'SmtpMail.UseDefaultCredentials = False
                'SmtpMail.Port = email_smtp_port
                'SmtpMail.Credentials = SmtpUser
                'SmtpMail.Host = email_smtp_server
                'SmtpMail.EnableSsl = True
                'SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network
                'SmtpMail.Send(objMM)

                Dim SmtpUser As New Net.NetworkCredential
                SmtpUser.UserName = email_smtp_username
                SmtpUser.Password = email_smtp_password
                'SmtpUser.Domain = email_smtp_server
                SmtpMail.EnableSsl = True
                SmtpMail.UseDefaultCredentials = False
                SmtpMail.Credentials = SmtpUser
                SmtpMail.Host = email_smtp_server
                SmtpMail.Port = email_smtp_port
                SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network
                SmtpMail.Send(objMM)

                '
                '   Lo marca como enviado
                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pCFD @cmd=40, @cfdid='" & tempcfdid.Value.ToString & "'")
                ObjData = Nothing
                '
            Catch ex As Exception
                lblMensajeEmail.Text = "Error: " & ex.Message.ToString
            Finally
                SmtpMail = Nothing
            End Try
            objMM = Nothing
            '
            '
            Call MuestraLista()
            RadWindow1.VisibleOnPageLoad = False
            '
        Else
            lblMensajeEmail.Text = "Formato de correo no válido: " & novalidos
        End If
    End Sub

    Public Shared Function validarEmail(ByVal email As String) As Boolean
        Dim expresion As String = "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
        If Regex.IsMatch(email, expresion) Then
            If Regex.Replace(email, expresion, String.Empty).Length = 0 Then
                Return True
            Else
                Return False
            End If

        Else
            Return False
        End If

    End Function
#End Region

#Region "cancela CFD"
    Private Sub CancelaSIFEI(ByVal cfdi As Long)
        CancelarId.Value = cfdi
        WinCancel.VisibleOnPageLoad = True
    End Sub

    Protected Sub cmbMotivoCancela_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMotivoCancela.SelectedIndexChanged
        If cmbMotivoCancela.SelectedValue = "01" Then
            panelFolioSustituye.Visible = True
            WinCancel.Height = 290
        Else
            panelFolioSustituye.Visible = False
            WinCancel.Height = 200
            txtFolioSustituye.Text = ""
        End If
    End Sub

    Protected Sub btnCancelaFactura_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelaFactura.Click
        Dim motivoCancela As String = ""
        Dim folioSustituye As String = ""
        Dim cfdid As Long = 0

        cfdid = CancelarId.Value
        folioSustituye = txtFolioSustituye.Text
        motivoCancela = cmbMotivoCancela.SelectedValue

        CancelaCFDI33_Aplica(CancelarId.Value, motivoCancela, folioSustituye)

        WinCancel.VisibleOnPageLoad = False
        CancelarId.Value = 0
        txtFolioSustituye.Text = ""
        Call MuestraLista()
    End Sub

    Private Sub CancelaCFDI33_Aplica(ByVal cfdi As Long, ByVal motivoCancela As String, ByVal folioSustituye As String)
        '
        '   Obtiene serie y folio y construye nombre del XML
        '
        Dim consignacionid As Long = 0
        Dim serie As String = ""
        Dim folio As Long = 0
        Dim email As String = ""
        Dim archivo_llave_privada As String = ""
        Dim contrasena_llave_privada As String = ""
        Dim archivoCertificado As String = ""

        Dim connX As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdX As New SqlCommand("exec pCFD_Cancela @cfdid='" & cfdi.ToString & "'", connX)
        Try

            connX.Open()

            Dim rs As SqlDataReader
            rs = cmdX.ExecuteReader()

            If rs.Read Then
                consignacionid = rs("consignacionid")
                serie = rs("serie")
                folio = rs("folio")
                email = rs("email")
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
            Response.End()
        Finally
            connX.Close()
            connX.Dispose()
            connX = Nothing
        End Try

        Dim FlujoReader As XmlTextReader = Nothing
        Dim i As Integer
        FlujoReader = New XmlTextReader(Server.MapPath("~/portalcfd/cfd_storage") & "/" & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
        FlujoReader.WhitespaceHandling = WhitespaceHandling.None
        While FlujoReader.Read()
            Select Case FlujoReader.NodeType
                Case XmlNodeType.Element
                    If FlujoReader.Name = "tfd:TimbreFiscalDigital" Then
                        For i = 0 To FlujoReader.AttributeCount - 1
                            FlujoReader.MoveToAttribute(i)
                            If FlujoReader.Name = "UUID" Then
                                uuids.Add(FlujoReader.Value)
                            End If
                        Next
                    ElseIf FlujoReader.Name = "cfdi:Emisor" Then
                        For i = 0 To FlujoReader.AttributeCount - 1
                            FlujoReader.MoveToAttribute(i)
                            If FlujoReader.Name = "Rfc" Then
                                RFCEmisor = FlujoReader.Value
                            End If
                        Next
                    End If
            End Select
        End While

        Try
            Dim SIFEIUsuario As String = System.Configuration.ConfigurationManager.AppSettings("SIFEIUsuario")
            Dim SIFEIContrasena As String = System.Configuration.ConfigurationManager.AppSettings("SIFEIContrasena")

            Dim PfxBytes As Byte() = ReadFile(Server.MapPath("~/portalcfd/certificados/") & CertificadoCliente() & ".pfx")

            'Pruebas
            Dim sifei As New CancelacionPruebasSIFEI.Cancelacion()

            'Produccion
            'Dim sifei As New CancelacionSIFEI.Cancelacion()

            Dim nameAcuse As String = uuids(0).ToString
            Dim uuidscancelar As New List(Of String)
            For Each row In uuids
                uuidscancelar.Add("|" & row & "|" & motivoCancela & "|" & folioSustituye & "|")
            Next

            System.Net.ServicePointManager.SecurityProtocol = DirectCast(3072, System.Net.SecurityProtocolType) Or DirectCast(768, System.Net.SecurityProtocolType) Or DirectCast(192, System.Net.SecurityProtocolType) Or DirectCast(48, System.Net.SecurityProtocolType)

            Dim result = sifei.cancelaCFDI(SIFEIUsuario, SIFEIContrasena, RFCEmisor, PfxBytes, ContrasenaPfx(), uuidscancelar.ToArray())

            Dim xml As New XmlDocument()
            xml.LoadXml(result)
            xml.Save(Server.MapPath("~/portalcfd/cfd_storage/") & "acuse_" & nameAcuse & ".xml")

            Dim EstatusUUID As String = ""
            Dim DescricionCodigo As String = ""
            EstatusUUID = GetXMLValue(Server.MapPath("~/portalcfd/cfd_storage/") & "acuse_" & nameAcuse & ".xml", "EstatusUUID")

            If EstatusUUID = "201" Then
                'DescricionCodigo = "UUID Cancelado exitosamente"
                If consignacionid > 0 Then
                    Dim ObjData As New DataControl
                    ObjData.RunSQLQuery("exec pCFD @cmd=36, @cfdid='" & cfdi.ToString & "', @consignacionid='" & consignacionid.ToString & "', @motivoid='" & motivoCancela & "', @uuid_sustituye='" & folioSustituye & "'")
                    ObjData = Nothing
                Else
                    Dim ObjData As New DataControl
                    ObjData.RunSQLQuery("exec pCFD @cmd=21, @cfdid='" & cfdi.ToString & "', @motivoid='" & motivoCancela & "', @uuid_sustituye='" & folioSustituye & "'")
                    ObjData = Nothing
                End If

            ElseIf EstatusUUID = "205" Then
                DescricionCodigo = "UUID Previamente cancelado"

                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pCFD @cmd=21, @cfdid='" & cfdi.ToString & "', @motivoid='" & motivoCancela & "', @uuid_sustituye='" & folioSustituye & "'")
                ObjData = Nothing

            ElseIf EstatusUUID = "202" Then
                DescricionCodigo = "Folio Fiscal Previamente Cancelado"
            ElseIf EstatusUUID = "203" Then
                DescricionCodigo = "UUID No corresponde el RFC del Emisor y de quien solicita la cancelación"
            ElseIf EstatusUUID = "204" Then
                DescricionCodigo = "Folio Fiscal No Aplicable a Cancelación."
            ElseIf EstatusUUID = "205" Then
                DescricionCodigo = "UUID No existe"
            ElseIf EstatusUUID = "206" Then
                DescricionCodigo = "UUID no corresponde a un CFDI del Sector Primario."
            ElseIf EstatusUUID = "207" Then
                DescricionCodigo = "Folio sustitución Inválido"
            ElseIf EstatusUUID = "208" Then
                DescricionCodigo = "La Fecha de Solicitud de Cancelación es mayor a la fecha de declaración."
            ElseIf EstatusUUID = "209" Then
                DescricionCodigo = "La Fecha de Solicitud de Cancelación límite para factura global."
            ElseIf EstatusUUID = "300" Then
                DescricionCodigo = "Usuario y contraseña inválidos"
            ElseIf EstatusUUID = "301" Then
                DescricionCodigo = "XML mas formado"
            ElseIf EstatusUUID = "302" Then
                DescricionCodigo = "Sello mal formado o inválido"
            ElseIf EstatusUUID = "303" Then
                DescricionCodigo = "Sello no corresponde a emisor"
            ElseIf EstatusUUID = "304" Then
                DescricionCodigo = "Certificado Revocado o caduco"
            ElseIf EstatusUUID = "305" Then
                DescricionCodigo = "La fecha de emisión no esta dentro de la vigencia del CSD del Emisor"
            ElseIf EstatusUUID = "306" Then
                DescricionCodigo = "El certificado no es de tipo CSD"
            ElseIf EstatusUUID = "307" Then
                DescricionCodigo = "El CFDI contiene un timbre previo"
            ElseIf EstatusUUID = "308" Then
                DescricionCodigo = "Certificado no expedido por el SAT"
            ElseIf EstatusUUID = "310" Then
                DescricionCodigo = "CSD Inválido."
            ElseIf EstatusUUID = "311" Then
                DescricionCodigo = "Clave de motivo de cancelación no válida."
            ElseIf EstatusUUID = "312" Then
                DescricionCodigo = "UUID no relacionado de acuerdo a la clave de motivo de cancelación."
            ElseIf EstatusUUID = "401" Then
                DescricionCodigo = "Fecha y hora de generación fuera de rango"
            ElseIf EstatusUUID = "402" Then
                DescricionCodigo = "RFC del emisor no se encuentra en el régimen de contribuyentes"
            ElseIf EstatusUUID = "403" Then
                DescricionCodigo = "La fecha de emisión no es posterior al 01 de enero de 2012"
            ElseIf EstatusUUID = "501" Then
                DescricionCodigo = "Autenticación no válida"
            ElseIf EstatusUUID = "203" Then
                DescricionCodigo = "UUID No corresponde el RFC del Emisor y de quien solicita la cancelación"
            ElseIf EstatusUUID = "703" Then
                DescricionCodigo = "Cuenta suspendida"
            ElseIf EstatusUUID = "704" Then
                DescricionCodigo = "Error con la contraseña de la llave Privada"
            ElseIf EstatusUUID = "705" Then
                DescricionCodigo = "XML estructura inválida"
            ElseIf EstatusUUID = "706" Then
                DescricionCodigo = "Socio Inválido"
            ElseIf EstatusUUID = "707" Then
                DescricionCodigo = "XML ya contiene un nodo TimbreFiscalDigital"
            ElseIf EstatusUUID = "708" Then
                DescricionCodigo = "No se pudo conectar al SAT"
            End If

            If EstatusUUID <> "201" Then
                RadAlert.RadAlert(DescricionCodigo, 330, 180, "Alerta", "", "")
            End If

        Catch ex As SoapException
            Response.Write(ex.Detail.InnerText.ToString)
            Response.End()
        End Try

    End Sub
#End Region
End Class