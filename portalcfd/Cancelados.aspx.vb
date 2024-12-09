Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.Threading
Imports System.Xml
Imports System.Net.Mail
Imports System.Xml.Serialization
Imports uCFDsLib
Imports uCFDsLib.v3
Imports ThoughtWorks.QRCode.Codec
Imports ThoughtWorks.QRCode.Codec.Util
Imports Telerik.Reporting.Processing
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Drawing
Imports Ionic.Zip
Imports System.Web.Services.Protocols
Imports System.Net.Security
Imports FirmaSAT.Sat

Public Class Cancelados
    Inherits System.Web.UI.Page

    Private RFCEmisor As String = ""
    Private uuids As New List(Of String)()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        If Not IsPostBack Then
            cmbAnio.SelectedValue = 2019
        End If
    End Sub

    Private Sub btnView_Click(sender As Object, e As EventArgs) Handles btnView.Click
        Call MuestraLista()
    End Sub

    Private Sub cfdlist_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles cfdlist.ItemCommand
        Select Case e.CommandName
            Case "cmdCancel"
                Call CancelaCFDI33(e.CommandArgument)
                Call MuestraLista()
            Case "cmdAcuse"
                Call VerAcuse(e.CommandArgument)
        End Select
    End Sub

    Private Sub CancelaCFDI33(ByVal cfdi As Long)
        '
        '   Obtiene serie y folio y construye nombre del XML
        '
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


        Dim FlujoReader As XmlTextReader = Nothing
        Dim i As Integer
        FlujoReader = New XmlTextReader(Server.MapPath("~/portalcfd/cfd_storage/") & "ng_" & serie.ToString & folio.ToString & "_timbrado.xml")
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
            'Dim TimbreSifeiVersion33 As New CancelacionPruebasSIFEI.Cancelacion()

            'Produccion
            Dim TimbreSifeiVersion33 As New CancelacionSIFEI.Cancelacion()

            Dim result = TimbreSifeiVersion33.cancelaCFDI(SIFEIUsuario, SIFEIContrasena, RFCEmisor, PfxBytes, ContrasenaPfx(), uuids.ToArray())

            Dim xml As New XmlDocument()
            xml.LoadXml(result)
            xml.Save(Server.MapPath("~/portalcfd/cfd_storage/") & "acuse_" & uuids(0).ToString & ".xml")

            Dim EstatusUUID As String = ""
            Dim DescricionCodigo As String = ""
            EstatusUUID = GetXMLValue(Server.MapPath("~/portalcfd/cfd_storage/") & "acuse_" & uuids(0).ToString & ".xml", "EstatusUUID")

            If EstatusUUID = "201" Then

                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("update tblCFD set estatuscancelado=3 where id='" & cfdi.ToString & "'")
                ObjData = Nothing

            ElseIf EstatusUUID = "202" Then
                DescricionCodigo = "UUID Previamente cancelado"

                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("update tblCFD set estatuscancelado=3 where id='" & cfdi.ToString & "'")
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
                MessageBox(DescricionCodigo)
            End If

        Catch ex As SoapException
            MessageBox(ex.Detail.InnerText.ToString.Replace("'", "").Replace(vbCr, "").Replace(vbLf, ""))
        End Try

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
        rfcE = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Emisor")
        rfcR = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Rfc", "cfdi:Receptor")
        total = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "Total", "cfdi:Comprobante")
        UUID = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "UUID", "tfd:TimbreFiscalDigital")
        sello = GetXmlAttribute(Server.MapPath("cfd_storage") & "\ng_" & serie.ToString & folio.ToString & "_timbrado.xml", "SelloCFD", "tfd:TimbreFiscalDigital")
        FinalSelloDigitalEmisor = Mid(sello, (Len(sello) - 7))
        '
        Dim totalDec As Decimal = CType(total, Decimal)
        '
        UrlSAT = "https://verificacfdi.facturaelectronica.sat.gob.mx/default.aspx" & "?id=" & UUID & "&re=" & rfcE & "&rr=" & rfcR & "&tt=" & totalDec.ToString & "&fe=" & FinalSelloDigitalEmisor

        Dim script As String = "function f(){openRadWindow('" & UrlSAT & "'); Sys.Application.remove_load(f);}Sys.Application.add_load(f);"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, True)

    End Sub

    Private Function ContrasenaPfx() As String
        Dim contrasena_llave_privada As String = ""
        Dim ObjData As New DataControl
        contrasena_llave_privada = ObjData.RunSQLScalarQueryString("select top 1 isnull(contrasena_llave_privada,'') as contrasena_llave_privada from tblCliente")
        ObjData = Nothing
        Return contrasena_llave_privada
    End Function

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

    Private Sub cfdlist_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles cfdlist.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim lnkCancelar As LinkButton = CType(e.Item.FindControl("lnkCancelar"), LinkButton)
                Dim lnkAcuse As LinkButton = CType(e.Item.FindControl("lnkAcuse"), LinkButton)
                If e.Item.DataItem("estatus").ToString = "Cancelado" Then
                    lnkAcuse.Visible = True
                    lnkCancelar.Visible = False
                    e.Item.Cells(6).ForeColor = Drawing.Color.Red
                    e.Item.Cells(6).Font.Bold = True
                Else
                    lnkAcuse.Visible = False
                End If
        End Select
    End Sub

    Private Sub cfdlist_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles cfdlist.NeedDataSource
        Dim ObjData As New DataControl
        cfdlist.DataSource = ObjData.FillDataSet("exec pCFD_Cancelados @anio='" & cmbAnio.SelectedValue.ToString & "', @serie='" & txtSerie.Text.ToString & "', @folio='" & txtFolio.Text.ToString & "'")
        ObjData = Nothing
    End Sub

    Protected Sub cfdlist_PageIndexChanged(ByVal source As Object, ByVal e As Telerik.Web.UI.GridPageChangedEventArgs) Handles cfdlist.PageIndexChanged
        cfdlist.CurrentPageIndex = e.NewPageIndex
        Call MuestraLista()
    End Sub

    Private Sub MuestraLista()
        Dim ObjData As New DataControl
        cfdlist.DataSource = ObjData.FillDataSet("exec pCFD_Cancelados @anio='" & cmbAnio.SelectedValue.ToString & "', @serie='" & txtSerie.Text.ToString & "', @folio='" & txtFolio.Text.ToString & "'")
        cfdlist.DataBind()
        ObjData = Nothing
    End Sub

End Class