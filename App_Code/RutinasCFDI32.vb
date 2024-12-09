Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Xml
Imports System.Xml.Xsl
Imports System.IO
Imports Org.BouncyCastle.OpenSsl
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Security

Namespace Cfdi32
    Public Class RutinasCFDI32

        Public Function GenerarCadenaV3(ByVal cfdi As Comprobante) As String
            Dim esquema As String = HttpContext.Current.Server.MapPath("~/portalcfd/SAT/cadenaoriginal_3_2.xslt")
            Dim sXML As String = cfdi.Serialize(System.Text.Encoding.UTF8)
            Dim xmlDoc As New XmlDocument()
            xmlDoc.LoadXml(sXML)

            Dim transformador As New XslCompiledTransform()
            transformador.Load(esquema)
            Dim CadenaOriginal As New StringWriter()
            transformador.Transform(xmlDoc.CreateNavigator(), Nothing, CadenaOriginal)

            Return CadenaOriginal.ToString()
        End Function

        Public Function GenerarSelloDigital(ByVal rutaKey As String, ByVal password As String, ByVal cadenaOriginal As String) As String
            Dim dataKey() As Byte = File.ReadAllBytes(rutaKey)
            Dim asp As Org.BouncyCastle.Crypto.AsymmetricKeyParameter = Org.BouncyCastle.Security.PrivateKeyFactory.DecryptKey(password.ToCharArray(), dataKey)
            Dim ms As New MemoryStream()
            Dim writer As TextWriter = New StreamWriter(ms)
            Dim stWrite As New System.IO.StringWriter()
            Dim pmw As Org.BouncyCastle.OpenSsl.PemWriter = New PemWriter(stWrite)
            pmw.WriteObject(asp)
            stWrite.Close()

            Dim sig As ISigner = SignerUtilities.GetSigner("SHA1WithRSA")

            '' Convertir a UTF8
            Dim plaintext() As Byte = Encoding.UTF8.GetBytes(cadenaOriginal)

            '' SELLAR
            sig.Init(True, asp)
            sig.BlockUpdate(plaintext, 0, plaintext.Length)
            Dim signature() As Byte = sig.GenerateSignature()

            Dim signatureHeader As Object = Convert.ToBase64String(signature)

            'Asignar sello
            Return signatureHeader.ToString()
        End Function
    End Class
End Namespace
