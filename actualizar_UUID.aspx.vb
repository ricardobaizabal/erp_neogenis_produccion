Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports Telerik.Reporting.Processing
Imports System.IO
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.Xsl
Imports System.Xml.Schema
Imports System.Xml.XPath.XPathItem
Imports System.Xml.XPath.XPathNavigator
Imports System.Xml.Serialization
Imports System.Collections
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.OpenSsl
Imports Org.BouncyCastle.Security
Imports ThoughtWorks.QRCode
Imports ThoughtWorks.QRCode.Codec
Imports ThoughtWorks.QRCode.Codec.Data
Imports System.Security.Cryptography.X509Certificates
Imports System.Threading
Imports System.Globalization
Imports System.Security.Cryptography
Imports System.Web.Services.Protocols

Partial Public Class actualizar_UUID
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ds As DataSet
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("select serie,folio,id from tblcfd where isnull(timbrado,0) =1 and isnull(estatus,0) <>3 and uuid is  null and metodopagoid='PPD' order by folio")
        ObjData = Nothing

        For Each row As DataRow In ds.Tables(0).Rows

            Dim serie As String = row("serie")
            Dim folio As String = row("folio")
            Dim cfdid As String = row("id")



            '   Obtiene el UUID
            '
            Dim filePath As String = Server.MapPath("~/portalcfd/cfd_storage/ng_") & "" & serie & folio & "_timbrado.xml"
            Dim UUID() As String
            ReDim UUID(0)
            '
            Dim FlujoReader As XmlTextReader = Nothing
            Dim j As Integer
            FlujoReader = New XmlTextReader(filePath)
            FlujoReader.WhitespaceHandling = WhitespaceHandling.None
            While FlujoReader.Read()
                Select Case FlujoReader.NodeType
                    Case XmlNodeType.Element
                        If FlujoReader.Name = "tfd:TimbreFiscalDigital" Then
                            For j = 0 To FlujoReader.AttributeCount - 1
                                FlujoReader.MoveToAttribute(j)
                                If FlujoReader.Name = "UUID" Then
                                    UUID(0) = FlujoReader.Value.ToString
                                End If
                            Next
                        End If
                End Select
            End While
            '
            '   Marca el cfd como timbrado
            '
            cfdtimbrado(UUID(0).ToString, cfdid.ToString)
        Next

        'Call cfdtimbrado(UUID(0))
    End Sub

    Private Sub cfdtimbrado(ByVal uuid As String, ByVal cfdid As String)
        Dim Objdata As New DataControl
        Objdata.RunSQLQuery("update tblcfd set uuid = '" + uuid.ToString + "' where id = " + cfdid.ToString + " ")
        Objdata = Nothing
    End Sub


End Class