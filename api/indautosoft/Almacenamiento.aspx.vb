Imports Newtonsoft.Json
Imports System.Data
Imports System.Collections.Generic
Imports Telerik.Web.UI.Common
Imports System.Net
Imports System.IO
Imports Newtonsoft.Json.Linq
Imports System.Threading.Tasks

Public Class Almacenamiento
    Inherits System.Web.UI.Page

    Public Property detail As List(Of OrderDetail)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim ordencompraid As Integer
        Dim fecha_act As String

        If Request("ordencompraid") <> Nothing Then
            ordencompraid = Request("ordencompraid")
        Else
            ordencompraid = 0
        End If

        If Request("fecha") <> Nothing Then
            fecha_act = Request("fecha")
        Else
            fecha_act = 0
        End If

        Dim etiquetatotal As DataSet = OrdenCompraParcialByOrdenparcial(ordencompraid, fecha_act)
        Dim JDatosOC As String = ""
        Dim filas As Integer = 0
        Dim contador As Integer = 0

        filas = etiquetatotal.Tables(0).Rows.Count

        Response.Write("[")
        For Each rowtotal As DataRow In etiquetatotal.Tables(0).Rows

            contador += 1
            Dim etiqueta As DataSet = AlmacenadoListado(rowtotal("orderId"))
            Dim ds As New DataSet()
            Dim Order As New Order()
            For Each row As DataRow In etiqueta.Tables(0).Rows

                Order.orderId = row("orderId")
                Order.OrderCode = row("orderCode")
                Order.orderDate = row("orderDate").ToString.Replace("/", "-")
                Order.receivedDate = row("receivedDate").ToString.Replace("/", "-")
                If Order.receivedDate = "01-01-1900 12:00:00 a. m." Then
                    Order.receivedDate = Nothing
                End If
                Order.supplierId = row("proveedorid")

                Dim conceptos As DataSet = AlmacenadoDetalles(Order.orderId)
                If conceptos.Tables(0).Rows.Count > 0 Then
                    Dim detail As New List(Of OrderDetail)
                    For Each r As DataRow In conceptos.Tables(0).Rows
                        Dim OrderDetail As New OrderDetail()
                        OrderDetail.orderId = r("orderId")
                        OrderDetail.quantityOrder = r("quantityOrder")
                        OrderDetail.productId = r("productId")
                        detail.Add(OrderDetail)
                    Next
                    Order.orderDetail = detail
                End If
            Next
            JDatosOC = JsonConvert.SerializeObject(Order).ToString.Replace("\", "")

            If JDatosOC.ToString.Contains(":null}") Then

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

    Public Function OrdenCompraParcialByOrdenparcial(ByVal ordencompraid As Integer, fecha_act As String) As DataSet
        Dim ordencompradata As New DataSet
        Dim obj As New DataControl
        ordencompradata = obj.FillDataSet("EXEC pStockLocation @cmd=6, @ordencompraid=" & ordencompraid & ",@fecha_act=" & fecha_act)
        Return ordencompradata
    End Function

    Public Function AlmacenadoListado(ByVal ordenId As Integer) As DataSet
        Dim almacenado As New DataSet
        Dim obj As New DataControl
        almacenado = obj.FillDataSet("EXEC pStockLocation @cmd=8, @orderId=" & ordenId)
        Return almacenado
    End Function

    Public Function AlmacenadoDetalles(ByVal ordenId As Integer) As DataSet
        Dim almacenadodetalle As New DataSet
        Dim obj As New DataControl
        almacenadodetalle = obj.FillDataSet("EXEC pOrdenCompra @cmd=9, @ordenId='" & ordenId & "'")
        Return almacenadodetalle
    End Function

    Public Class Order

        Private _orderId As String = ""
        Private _orderCode As String = ""
        Private _orderDate As String = ""
        Private _receivedDate As String = ""
        Private _supplierId As String = ""
        Private _orderDetail As List(Of OrderDetail)

        Public Property orderId() As String
            Get
                Return Me._orderId
            End Get
            Set(ByVal value As String)
                Me._orderId = value
            End Set
        End Property

        Public Property OrderCode() As String
            Get
                Return Me._orderCode
            End Get
            Set(value As String)
                Me._orderCode = value
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

        Public Property supplierId() As String
            Get
                Return Me._supplierId
            End Get
            Set(ByVal value As String)
                Me._supplierId = value
            End Set
        End Property

        Public Property orderDetail() As List(Of OrderDetail)
            Get
                Return Me._orderDetail
            End Get
            Set(ByVal value As List(Of OrderDetail))
                Me._orderDetail = value
            End Set
        End Property
    End Class

    Public Class OrderDetail

        Private _orderId As String = ""
        Private _quantityOrder As String = ""
        Private _productId As String = ""

        Public Property orderId() As String
            Get
                Return Me._orderId
            End Get
            Set(ByVal value As String)
                Me._orderId = value
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

        Public Property productId() As String
            Get
                Return Me._productId
            End Get
            Set(ByVal value As String)
                Me._productId = value
            End Set
        End Property

    End Class

End Class