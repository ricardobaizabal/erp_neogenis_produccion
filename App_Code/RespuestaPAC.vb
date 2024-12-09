Imports System.Xml.Serialization

''' <remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")> _
<System.SerializableAttribute()> _
<System.Diagnostics.DebuggerStepThroughAttribute()> _
<System.ComponentModel.DesignerCategoryAttribute("code")> _
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)> _
<System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
Partial Public Class Timbrado

    Private itemsField As TimbradoResultado()

    ''' <remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Resultado", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> _
    Public Property Items() As TimbradoResultado()
        Get
            Return Me.itemsField
        End Get
        Set(ByVal value As TimbradoResultado())
            Me.itemsField = value
        End Set
    End Property
End Class

''' <remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")> _
<System.SerializableAttribute()> _
<System.Diagnostics.DebuggerStepThroughAttribute()> _
<System.ComponentModel.DesignerCategoryAttribute("code")> _
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)> _
Partial Public Class TimbradoResultado

    Private informacionField As TimbradoResultadoInformacion()

    ''' <remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Informacion", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> _
    Public Property Informacion() As TimbradoResultadoInformacion()
        Get
            Return Me.informacionField
        End Get
        Set(ByVal value As TimbradoResultadoInformacion())
            Me.informacionField = value
        End Set
    End Property
End Class

''' <remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")> _
<System.SerializableAttribute()> _
<System.Diagnostics.DebuggerStepThroughAttribute()> _
<System.ComponentModel.DesignerCategoryAttribute("code")> _
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)> _
Partial Public Class TimbradoResultadoInformacion

    Private documentoField As TimbradoResultadoInformacionDocumento()

    Private timbreField As TimbradoResultadoInformacionTimbre()

    ''' <remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Documento", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> _
    Public Property Documento() As TimbradoResultadoInformacionDocumento()
        Get
            Return Me.documentoField
        End Get
        Set(ByVal value As TimbradoResultadoInformacionDocumento())
            Me.documentoField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Timbre", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> _
    Public Property Timbre() As TimbradoResultadoInformacionTimbre()
        Get
            Return Me.timbreField
        End Get
        Set(ByVal value As TimbradoResultadoInformacionTimbre())
            Me.timbreField = value
        End Set
    End Property
End Class

''' <remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")> _
<System.SerializableAttribute()> _
<System.Diagnostics.DebuggerStepThroughAttribute()> _
<System.ComponentModel.DesignerCategoryAttribute("code")> _
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)> _
Partial Public Class TimbradoResultadoInformacionDocumento

    Private serieField As String

    Private folioField As String

    Private fechaField As String

    Private rFCEmisorField As String

    Private rFCReceptorField As String

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Serie() As String
        Get
            Return Me.serieField
        End Get
        Set(ByVal value As String)
            Me.serieField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Folio() As String
        Get
            Return Me.folioField
        End Get
        Set(ByVal value As String)
            Me.folioField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property Fecha() As String
        Get
            Return Me.fechaField
        End Get
        Set(ByVal value As String)
            Me.fechaField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property RFCEmisor() As String
        Get
            Return Me.rFCEmisorField
        End Get
        Set(ByVal value As String)
            Me.rFCEmisorField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property RFCReceptor() As String
        Get
            Return Me.rFCReceptorField
        End Get
        Set(ByVal value As String)
            Me.rFCReceptorField = value
        End Set
    End Property
End Class

''' <remarks/>
<System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.1")> _
<System.SerializableAttribute()> _
<System.Diagnostics.DebuggerStepThroughAttribute()> _
<System.ComponentModel.DesignerCategoryAttribute("code")> _
<System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)> _
Partial Public Class TimbradoResultadoInformacionTimbre

    Private versionField As String

    Private uUIDField As String

    Private fechaTimbradoField As String

    Private selloCFDField As String

    Private noCertificadoSATField As String

    Private selloSATField As String

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property version() As String
        Get
            Return Me.versionField
        End Get
        Set(ByVal value As String)
            Me.versionField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property UUID() As String
        Get
            Return Me.uUIDField
        End Get
        Set(ByVal value As String)
            Me.uUIDField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property FechaTimbrado() As String
        Get
            Return Me.fechaTimbradoField
        End Get
        Set(ByVal value As String)
            Me.fechaTimbradoField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property selloCFD() As String
        Get
            Return Me.selloCFDField
        End Get
        Set(ByVal value As String)
            Me.selloCFDField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property noCertificadoSAT() As String
        Get
            Return Me.noCertificadoSATField
        End Get
        Set(ByVal value As String)
            Me.noCertificadoSATField = value
        End Set
    End Property

    ''' <remarks/>
    <System.Xml.Serialization.XmlAttributeAttribute()> _
    Public Property selloSAT() As String
        Get
            Return Me.selloSATField
        End Get
        Set(ByVal value As String)
            Me.selloSATField = value
        End Set
    End Property
End Class
