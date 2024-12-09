Imports Newtonsoft.Json
Imports Newtonsoft.Json.Converters
Imports Newtonsoft.Json.Linq
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common

Public Class ObtenerPedidos
    Inherits System.Web.UI.Page

    Public Property detail As List(Of PedidoDetail)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim pedidoid As Integer
        Dim fecha_act As String

        If Request("pedidoid") <> Nothing Then
            pedidoid = Request("pedidoid")
        Else
            pedidoid = 0
        End If

        If Request("fecha") <> Nothing Then
            fecha_act = Request("fecha")
        Else
            fecha_act = 0
        End If

        Dim etiquetatotal As DataSet = PedidoByPedidoId(pedidoid, fecha_act)
        Dim JDatosOC As String = ""
        Dim filas As Integer = 0
        Dim contador As Integer = 0

        filas = etiquetatotal.Tables(0).Rows.Count

        Response.Write("[")
        For Each rowtotal As DataRow In etiquetatotal.Tables(0).Rows

            contador += 1
            Dim etiqueta As DataSet = PedidolListado(rowtotal("custOrderId"))
            Dim ds As New DataSet()
            Dim Pedido As New Pedido()
            For Each row As DataRow In etiqueta.Tables(0).Rows

                Pedido.custOrderId = row("custOrderId")
                Pedido.customerId = row("customerId")
                Pedido.custbranchOfficeId = row("custbranchOfficeId")
                Pedido.custOrderCode = row("custOrderCode")
                Pedido.guia = row("guia")
                Pedido.clicAndCollect = row("clicAndCollect")
                Pedido.bodega = row("bodega")
                Pedido.orderDate = row("orderDate").ToString.Replace("/", "-")
                Pedido.receivedDate = row("receivedDate").ToString.Replace("/", "-")
                If Pedido.receivedDate = "01-01-1900 12:00:00 a. m." Then
                    Pedido.receivedDate = Nothing
                End If
                Pedido.brand = row("brand")
                Pedido.isActive = row("isActive")
                Pedido.removed = row("removed")

                If Pedido.removed = True Then
                    'filas -= 1
                End If

                Dim conceptos As DataSet = PedidosConceptos(Pedido.custOrderId)
                If conceptos.Tables(0).Rows.Count > 0 Then
                    Dim detail As New List(Of PedidoDetail)
                    For Each r As DataRow In conceptos.Tables(0).Rows
                        Dim PedidoDetail As New PedidoDetail()
                        PedidoDetail.conceptId = r("conceptId")
                        PedidoDetail.productId = r("productId")
                        PedidoDetail.quantityOrder = r("quantityOrder")
                        'PedidoDetail.isActive = r("isActive")
                        PedidoDetail.removed = r("removed")
                        detail.Add(PedidoDetail)
                    Next
                    Pedido.orderDetail = detail
                End If
            Next
            JDatosOC = JsonConvert.SerializeObject(Pedido).ToString.Replace("\", "")

            If JDatosOC.ToString.Contains(":null}") Then
                'filas -= 1
            Else
                Response.ClearHeaders()
                Response.ContentType = "text/plain"
                Response.Write(JDatosOC)
                If contador < filas Then
                    Response.Write(",")

                End If
                ' JDatosOC.ToString.Replace(",]", "]")
            End If
        Next

        Response.Write("]")

    End Sub

    Public Function PedidoByPedidoId(ByVal pedidoid As Integer, fecha_act As String) As DataSet
        Dim pedidodata As New DataSet
        Dim obj As New DataControl
        pedidodata = obj.FillDataSet("EXEC pStockLocation @cmd=6, @pedidoid=" & pedidoid & ",@fecha_act=" & fecha_act)
        Return pedidodata
    End Function

    Public Function PedidolListado(ByVal pedidoid As Integer) As DataSet
        Dim pedidodata As New DataSet
        Dim obj As New DataControl
        pedidodata = obj.FillDataSet("EXEC pStockLocation @cmd=6, @pedidoid=" & pedidoid)
        Return pedidodata
    End Function

    Public Function PedidosConceptos(ByVal pedidoid As Integer) As DataSet
        Dim data As New DataSet
        Dim obj As New DataControl
        data = obj.FillDataSet("EXEC pPedidosConceptos @cmd=5, @pedidoid='" & pedidoid & "'")
        Return data
    End Function

    Public Class Pedido

        Private _custOrderId As String = ""
        Private _customerId As String = ""
        Private _custbranchOfficeId As String = ""
        Private _custOrderCode As String = ""
        Private _guia As String = ""
        Private _clicAndCollect As String = ""
        Private _bodega As String = ""
        Private _orderDate As String = ""
        Private _receivedDate As String = ""
        Private _brand As String = ""
        Private _isActive As Boolean = False
        Private _removed As Boolean = False
        Private _pedidoDetail As List(Of PedidoDetail)

        Public Property custOrderId() As String
            Get
                Return Me._custOrderId
            End Get
            Set(ByVal value As String)
                Me._custOrderId = value
            End Set
        End Property

        Public Property customerId() As String
            Get
                Return Me._customerId
            End Get
            Set(value As String)
                Me._customerId = value
            End Set
        End Property

        Public Property custbranchOfficeId() As String
            Get
                Return Me._custbranchOfficeId
            End Get
            Set(ByVal value As String)
                Me._custbranchOfficeId = value
            End Set
        End Property
        Public Property custOrderCode() As String
            Get
                Return Me._custOrderCode
            End Get
            Set(ByVal value As String)
                Me._custOrderCode = value
            End Set
        End Property
        Public Property guia() As String
            Get
                Return Me._guia
            End Get
            Set(ByVal value As String)
                Me._guia = value
            End Set
        End Property
        Public Property clicAndCollect() As String
            Get
                Return Me._clicAndCollect
            End Get
            Set(ByVal value As String)
                Me._clicAndCollect = value
            End Set
        End Property
        Public Property bodega() As String
            Get
                Return Me._bodega
            End Get
            Set(ByVal value As String)
                Me._bodega = value
            End Set
        End Property

        Public Property orderDate() As String
            Get
                Return Me._orderDate
            End Get
            Set(ByVal value As String)
                Me._orderDate = value
            End Set
        End Property

        Public Property receivedDate() As String
            Get
                Return Me._receivedDate
            End Get
            Set(ByVal value As String)
                Me._receivedDate = value
            End Set
        End Property
        Public Property brand() As String
            Get
                Return Me._brand
            End Get
            Set(ByVal value As String)
                Me._brand = value
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
        Public Property removed() As Boolean
            Get
                Return Me._removed
            End Get
            Set(ByVal value As Boolean)
                Me._removed = value
            End Set
        End Property

        Public Property orderDetail() As List(Of PedidoDetail)
            Get
                Return Me._pedidoDetail
            End Get
            Set(ByVal value As List(Of PedidoDetail))
                Me._pedidoDetail = value
            End Set
        End Property
    End Class

    Public Class PedidoDetail

        Private _conceptId As String = ""
        Private _productId As String = ""
        Private _quantityOrder As String = ""
        'Private _isActive As Boolean = False
        Private _removed As Boolean = False

        Public Property conceptId() As String
            Get
                Return Me._conceptId
            End Get
            Set(ByVal value As String)
                Me._conceptId = value
            End Set
        End Property

        Public Property productId() As String
            Get
                Return Me._productId
            End Get
            Set(ByVal value As String)
                Me._productId = value
            End Set
        End Property

        Public Property quantityOrder() As String
            Get
                Return Me._quantityOrder
            End Get
            Set(ByVal value As String)
                Me._quantityOrder = value
            End Set
        End Property

        'Public Property isActive() As Boolean
        '    Get
        '        Return Me._isActive
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Me._isActive = value
        '    End Set
        'End Property

        Public Property removed() As Boolean
            Get
                Return Me._removed
            End Get
            Set(ByVal value As Boolean)
                Me._removed = value
            End Set
        End Property
    End Class
End Class