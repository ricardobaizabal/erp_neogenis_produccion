Public Class AddendaCoppelDTO
    Private _cfdid As Integer
    Private _fechaPromesaEntrega As DateTime
    Private _fechaOrdenCompra As DateTime
    Private _numReferenciaAdicional As String
    Private _fleteCaja As String
    Private _bodegaDestino As String
    Private _bodegaReceptora As String
    Private _glnComprador As String
    Private _glnVendedor As String
    Private _numProveedor As String
    Private _serie As String
    Private _folio As String
    Private _ordenCompra As String
    Private _tipoAddendaid As Integer
    Private _cadenaOriginal As String

    Public Property Cfdid As Integer
        Get
            Return _cfdid
        End Get
        Set(value As Integer)
            _cfdid = value
        End Set
    End Property

    Public Property FechaPromesaEntrega As Date
        Get
            Return _fechaPromesaEntrega
        End Get
        Set(value As Date)
            _fechaPromesaEntrega = value
        End Set
    End Property

    Public Property FechaOrdenCompra As Date
        Get
            Return _fechaOrdenCompra
        End Get
        Set(value As Date)
            _fechaOrdenCompra = value
        End Set
    End Property

    Public Property NumReferenciaAdicional As String
        Get
            Return _numReferenciaAdicional
        End Get
        Set(value As String)
            _numReferenciaAdicional = value
        End Set
    End Property

    Public Property FleteCaja As String
        Get
            Return _fleteCaja
        End Get
        Set(value As String)
            _fleteCaja = value
        End Set
    End Property

    Public Property BodegaDestino As String
        Get
            Return _bodegaDestino
        End Get
        Set(value As String)
            _bodegaDestino = value
        End Set
    End Property

    Public Property BodegaReceptora As String
        Get
            Return _bodegaReceptora
        End Get
        Set(value As String)
            _bodegaReceptora = value
        End Set
    End Property

    Public Property GlnComprador As String
        Get
            Return _glnComprador
        End Get
        Set(value As String)
            _glnComprador = value
        End Set
    End Property

    Public Property GlnVendedor As String
        Get
            Return _glnVendedor
        End Get
        Set(value As String)
            _glnVendedor = value
        End Set
    End Property

    Public Property NumProveedor As String
        Get
            Return _numProveedor
        End Get
        Set(value As String)
            _numProveedor = value
        End Set
    End Property

    Public Property Serie As String
        Get
            Return _serie
        End Get
        Set(value As String)
            _serie = value
        End Set
    End Property

    Public Property Folio As String
        Get
            Return _folio
        End Get
        Set(value As String)
            _folio = value
        End Set
    End Property

    Public Property OrdenCompra As String
        Get
            Return _ordenCompra
        End Get
        Set(value As String)
            _ordenCompra = value
        End Set
    End Property

    Public Property TipoAddendaid As Integer
        Get
            Return _tipoAddendaid
        End Get
        Set(value As Integer)
            _tipoAddendaid = value
        End Set
    End Property

    Public Property CadenaOriginal As String
        Get
            Return _cadenaOriginal
        End Get
        Set(value As String)
            _cadenaOriginal = value
        End Set
    End Property
End Class
