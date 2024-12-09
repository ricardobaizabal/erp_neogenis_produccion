Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters
Imports Newtonsoft.Json.Linq
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class Cliente
    Inherits System.Web.UI.Page

    Public Property detail As List(Of ClienteDetail)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim clienteid As Integer
        Dim fecha_act As String

        If Request("clienteid") <> Nothing Then
            clienteid = Request("clienteid")
        Else
            clienteid = 0
        End If

        If Request("fecha") <> Nothing Then
            fecha_act = Request("fecha")
        Else
            fecha_act = 0
        End If

        Dim etiquetatotal As DataSet = ClienteByClienteId(clienteid, fecha_act)
        Dim JDatosOC As String = ""
        Dim filas As Integer = 0
        Dim contador As Integer = 0

        filas = etiquetatotal.Tables(0).Rows.Count

        Response.Write("[")
        For Each rowtotal As DataRow In etiquetatotal.Tables(0).Rows

            contador += 1
            Dim etiqueta As DataSet = ClientelListado(rowtotal("customerId"))
            Dim ds As New DataSet()
            Dim Cliente As New Cliente()
            For Each row As DataRow In etiqueta.Tables(0).Rows

                Cliente.customerId = row("customerId")
                Cliente.name = row("name")
                Cliente.isActive = row("isActive")
                'Cliente.isActiveScanner = row("isActiveScanner")

                Dim conceptos As DataSet = ClienteDetalle(Cliente.customerId)
                If conceptos.Tables(0).Rows.Count > 0 Then
                    Dim detail As New List(Of ClienteDetail)
                    For Each r As DataRow In conceptos.Tables(0).Rows
                        Dim ClienteDetail As New ClienteDetail()
                        ClienteDetail.custbranchOfficeId = r("custbranchOfficeId")
                        detail.Add(ClienteDetail)
                    Next
                    Cliente.custbranchOffice = detail
                Else
                    Dim detail As New List(Of ClienteDetail)
                    Dim ClienteDetail As New ClienteDetail()
                    ClienteDetail.custbranchOfficeId = "N/A"
                    detail.Add(ClienteDetail)

                        Cliente.custbranchOffice = detail
                End If
            Next
            JDatosOC = JsonConvert.SerializeObject(Cliente).ToString.Replace("\", "")

            If JDatosOC.ToString.Contains("N/A") Then

            Else
                Response.ClearHeaders()
                Response.ContentType = "text/plain"
                Response.Write(JDatosOC)
                If contador < filas Then
                    Response.Write(",")
                End If
            End If
        Next
        Response.Write("]")

    End Sub

    Public Function ClienteByClienteId(ByVal clienteId As Integer, fecha_act As String) As DataSet
        Dim clientedata As New DataSet
        Dim obj As New DataControl
        clientedata = obj.FillDataSet("EXEC pStockLocation @cmd=7, @clienteid=" & clienteId & ",@fecha_act=" & fecha_act)
        Return clientedata
    End Function

    Public Function ClientelListado(ByVal clienteId As Integer) As DataSet
        Dim clientedata As New DataSet
        Dim obj As New DataControl
        clientedata = obj.FillDataSet("EXEC pStockLocation @cmd=7, @clienteid=" & clienteId)
        Return clientedata
    End Function

    Public Function ClienteDetalle(ByVal clienteId As Integer) As DataSet
        Dim data As New DataSet
        Dim obj As New DataControl
        data = obj.FillDataSet("EXEC pMisClientes @cmd=8, @clienteId='" & clienteId & "'")
        Return data
    End Function

    Public Class Cliente
        Private _customerId As String = ""
        Private _name As String = ""
        Private _isActive As Boolean = False
        Private _clienteDetail As List(Of ClienteDetail)

        Public Property customerId() As String
            Get
                Return Me._customerId
            End Get
            Set(value As String)
                Me._customerId = value
            End Set
        End Property

        Public Property name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal value As String)
                Me._name = value
            End Set
        End Property

        Public Property isActive() As Boolean
            Get
                Return Me._isActive
            End Get
            Set(ByVal value As Boolean)
                Me._isActive = value
            End Set
        End Property

        Public Property custbranchOffice() As List(Of ClienteDetail)
            Get
                Return Me._clienteDetail
            End Get
            Set(ByVal value As List(Of ClienteDetail))
                Me._clienteDetail = value
            End Set
        End Property
    End Class

    Public Class ClienteDetail

        Private _custbranchOfficeId As String = ""

        Public Property custbranchOfficeId() As String
            Get
                Return Me._custbranchOfficeId
            End Get
            Set(ByVal value As String)
                Me._custbranchOfficeId = value
            End Set
        End Property

    End Class

End Class