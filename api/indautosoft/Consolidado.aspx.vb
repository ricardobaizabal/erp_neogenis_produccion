Imports Newtonsoft.Json
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports System.Threading.Tasks

Public Class Consolidado
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim dt As New DataTable
        Dim obj As New DataControl
        Dim celda As String = ""
        Dim request As HttpWebRequest
        Dim response1 As HttpWebResponse = Nothing
        Dim reader As StreamReader

        If Not Me.IsPostBack Then
            ServicePointManager.Expect100Continue = True
            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
            '(New WebClient).DownloadString("")
            request = DirectCast(WebRequest.Create("https://raw.githubusercontent.com/aspsnippets/test/master/Customers.json"), HttpWebRequest)
            'Dim tester As DataTable = CType(JsonConvert.DeserializeObject(json.ToString(), (GetType(DataTable))), DataTable)
            response1 = DirectCast(request.GetResponse(), HttpWebResponse)
            reader = New StreamReader(response1.GetResponseStream())

            Dim rawresp As String
            rawresp = reader.ReadToEnd()

            Dim array As JArray = JArray.Parse(rawresp)

            For Each item As JObject In array
                Dim id As String = If(item("id") Is Nothing, "", item("id").ToString())
                Dim name As String = If(item("Name") Is Nothing, "", item("Name").ToString())
                Dim email As String = If(item("Country") Is Nothing, "", item("Country").ToString())
                Console.WriteLine("Id: " & id)
                Console.WriteLine("Name: " & name)
                Console.WriteLine("Country: " & email)

                Response.ClearHeaders()
                Response.ContentType = "text/plain"
                Response.Write(id)
                Response.Write(name)
                Response.Write(email)
                Response.Write(",")

            Next
            '    Response.ClearHeaders()
            'Response.ContentType = "text/plain"
            'Response.Write(request)
        End If

    End Sub

    'Friend Shared Async Function CambioDolarEuro_Tradicional() As Task(Of Double)



    '    If True Then
    '        Dim apiRes As String = Await HttpResponse.Content.ReadAsStringAsync()
    '        Dim infoCambio = JsonSerializer.Deserialize(Of InfoCambioMoneda)(apiRes)
    '        Return infoCambio.rates("USD")
    '    End If
    'End Function


    'Friend Shared Function CambioDolarEuro_Extensores() As Double
    '    HttpClient httpClient = New();
    '   Dim infoCambio = Await httpClient.GetFromJsonAsync(Of InfoCambioMoneda)(apiCambioMonedaUrl)
    'Return infoCambio.rates("USD")

    'End Function


    Friend Class InfoCambioMoneda
        Public Property amount As Double
        Public Property base As String
        Public Property datetime As DateTime
        Public Property rates As Dictionary(Of String, Double)
    End Class
End Class