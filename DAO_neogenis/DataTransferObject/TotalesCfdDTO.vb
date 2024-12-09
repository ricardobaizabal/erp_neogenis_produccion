Public Class TotalesCfdDTO
    Private _cfdid As Integer
    Private _tieneIva16 As Boolean
    Private _tieneIvaTasaCero As Boolean
    Private _subtotal As Decimal
    Private _retencion As Decimal
    Private _iva As Decimal
    Private _descuento As Decimal
    Private _total As Decimal

    Public Sub Main()
        _cfdid = 0
        _tieneIva16 = False
        _tieneIvaTasaCero = False
        _subtotal = 0
        _iva = 0
        _retencion = 0
        _descuento = 0
        _total = 0
    End Sub

    Public Property Cfdid As Integer
        Get
            Return _cfdid
        End Get
        Set(value As Integer)
            _cfdid = value
        End Set
    End Property

    Public Property TieneIva16 As Boolean
        Get
            Return _tieneIva16
        End Get
        Set(value As Boolean)
            _tieneIva16 = value
        End Set
    End Property

    Public Property TieneIvaTasaCero As Boolean
        Get
            Return _tieneIvaTasaCero
        End Get
        Set(value As Boolean)
            _tieneIvaTasaCero = value
        End Set
    End Property

    Public Property Subtotal As Decimal
        Get
            Return _subtotal
        End Get
        Set(value As Decimal)
            _subtotal = value
        End Set
    End Property
    Public Property Retencion As Decimal
        Get
            Return _retencion
        End Get
        Set(value As Decimal)
            _retencion = value
        End Set
    End Property
    Public Property Iva As Decimal
        Get
            Return _iva
        End Get
        Set(value As Decimal)
            _iva = value
        End Set
    End Property

    Public Property Descuento As Decimal
        Get
            Return _descuento
        End Get
        Set(value As Decimal)
            _descuento = value
        End Set
    End Property

    Public Property Total As Decimal
        Get
            Return _total
        End Get
        Set(value As Decimal)
            _total = value
        End Set
    End Property


End Class
