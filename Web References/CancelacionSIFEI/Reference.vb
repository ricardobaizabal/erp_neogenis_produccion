﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.42000.
'
Namespace CancelacionSIFEI
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="CancelacionPortBinding", [Namespace]:="http://service.sifei.cancelacion/")>  _
    Partial Public Class Cancelacion
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private peticionesPendientesOperationCompleted As System.Threading.SendOrPostCallback
        
        Private consultaSATCFDIOperationCompleted As System.Threading.SendOrPostCallback
        
        Private cancelaCFDIOperationCompleted As System.Threading.SendOrPostCallback
        
        Private procesarRespuestaOperationCompleted As System.Threading.SendOrPostCallback
        
        Private cfdiRelacionadoOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.erp_neogenis.My.MySettings.Default.erp_neogenis_CancelacionSIFEI_Cancelacion
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event peticionesPendientesCompleted As peticionesPendientesCompletedEventHandler
        
        '''<remarks/>
        Public Event consultaSATCFDICompleted As consultaSATCFDICompletedEventHandler
        
        '''<remarks/>
        Public Event cancelaCFDICompleted As cancelaCFDICompletedEventHandler
        
        '''<remarks/>
        Public Event procesarRespuestaCompleted As procesarRespuestaCompletedEventHandler
        
        '''<remarks/>
        Public Event cfdiRelacionadoCompleted As cfdiRelacionadoCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://service.sifei.cancelacion/", ResponseNamespace:="http://service.sifei.cancelacion/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function peticionesPendientes(<System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal usuarioSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal rfcReceptor As String) As <System.Xml.Serialization.XmlElementAttribute("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> String
            Dim results() As Object = Me.Invoke("peticionesPendientes", New Object() {usuarioSIFEI, passwordSIFEI, rfcReceptor})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub peticionesPendientesAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal rfcReceptor As String)
            Me.peticionesPendientesAsync(usuarioSIFEI, passwordSIFEI, rfcReceptor, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub peticionesPendientesAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal rfcReceptor As String, ByVal userState As Object)
            If (Me.peticionesPendientesOperationCompleted Is Nothing) Then
                Me.peticionesPendientesOperationCompleted = AddressOf Me.OnpeticionesPendientesOperationCompleted
            End If
            Me.InvokeAsync("peticionesPendientes", New Object() {usuarioSIFEI, passwordSIFEI, rfcReceptor}, Me.peticionesPendientesOperationCompleted, userState)
        End Sub
        
        Private Sub OnpeticionesPendientesOperationCompleted(ByVal arg As Object)
            If (Not (Me.peticionesPendientesCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent peticionesPendientesCompleted(Me, New peticionesPendientesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://service.sifei.cancelacion/", ResponseNamespace:="http://service.sifei.cancelacion/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function consultaSATCFDI(<System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal usuarioSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal id As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal re As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal rr As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal tt As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal fe As String) As <System.Xml.Serialization.XmlElementAttribute("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> String
            Dim results() As Object = Me.Invoke("consultaSATCFDI", New Object() {usuarioSIFEI, passwordSIFEI, id, re, rr, tt, fe})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub consultaSATCFDIAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal id As String, ByVal re As String, ByVal rr As String, ByVal tt As String, ByVal fe As String)
            Me.consultaSATCFDIAsync(usuarioSIFEI, passwordSIFEI, id, re, rr, tt, fe, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub consultaSATCFDIAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal id As String, ByVal re As String, ByVal rr As String, ByVal tt As String, ByVal fe As String, ByVal userState As Object)
            If (Me.consultaSATCFDIOperationCompleted Is Nothing) Then
                Me.consultaSATCFDIOperationCompleted = AddressOf Me.OnconsultaSATCFDIOperationCompleted
            End If
            Me.InvokeAsync("consultaSATCFDI", New Object() {usuarioSIFEI, passwordSIFEI, id, re, rr, tt, fe}, Me.consultaSATCFDIOperationCompleted, userState)
        End Sub
        
        Private Sub OnconsultaSATCFDIOperationCompleted(ByVal arg As Object)
            If (Not (Me.consultaSATCFDICompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent consultaSATCFDICompleted(Me, New consultaSATCFDICompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://service.sifei.cancelacion/", ResponseNamespace:="http://service.sifei.cancelacion/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function cancelaCFDI(<System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal usuarioSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordSifei As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal rfcEmisor As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="base64Binary", IsNullable:=true)> ByVal pfx() As Byte, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordPfx As String, <System.Xml.Serialization.XmlElementAttribute("uuids", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=true)> ByVal uuids() As String) As <System.Xml.Serialization.XmlElementAttribute("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> String
            Dim results() As Object = Me.Invoke("cancelaCFDI", New Object() {usuarioSIFEI, passwordSifei, rfcEmisor, pfx, passwordPfx, uuids})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub cancelaCFDIAsync(ByVal usuarioSIFEI As String, ByVal passwordSifei As String, ByVal rfcEmisor As String, ByVal pfx() As Byte, ByVal passwordPfx As String, ByVal uuids() As String)
            Me.cancelaCFDIAsync(usuarioSIFEI, passwordSifei, rfcEmisor, pfx, passwordPfx, uuids, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub cancelaCFDIAsync(ByVal usuarioSIFEI As String, ByVal passwordSifei As String, ByVal rfcEmisor As String, ByVal pfx() As Byte, ByVal passwordPfx As String, ByVal uuids() As String, ByVal userState As Object)
            If (Me.cancelaCFDIOperationCompleted Is Nothing) Then
                Me.cancelaCFDIOperationCompleted = AddressOf Me.OncancelaCFDIOperationCompleted
            End If
            Me.InvokeAsync("cancelaCFDI", New Object() {usuarioSIFEI, passwordSifei, rfcEmisor, pfx, passwordPfx, uuids}, Me.cancelaCFDIOperationCompleted, userState)
        End Sub
        
        Private Sub OncancelaCFDIOperationCompleted(ByVal arg As Object)
            If (Not (Me.cancelaCFDICompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent cancelaCFDICompleted(Me, New cancelaCFDICompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://service.sifei.cancelacion/", ResponseNamespace:="http://service.sifei.cancelacion/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function procesarRespuesta(<System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal usuarioSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal rfcReceptor As String, <System.Xml.Serialization.XmlElementAttribute("folios", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, IsNullable:=true)> ByVal folios() As folios, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="base64Binary", IsNullable:=true)> ByVal pfx() As Byte, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordPfx As String) As <System.Xml.Serialization.XmlElementAttribute("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> String
            Dim results() As Object = Me.Invoke("procesarRespuesta", New Object() {usuarioSIFEI, passwordSIFEI, rfcReceptor, folios, pfx, passwordPfx})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub procesarRespuestaAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal rfcReceptor As String, ByVal folios() As folios, ByVal pfx() As Byte, ByVal passwordPfx As String)
            Me.procesarRespuestaAsync(usuarioSIFEI, passwordSIFEI, rfcReceptor, folios, pfx, passwordPfx, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub procesarRespuestaAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal rfcReceptor As String, ByVal folios() As folios, ByVal pfx() As Byte, ByVal passwordPfx As String, ByVal userState As Object)
            If (Me.procesarRespuestaOperationCompleted Is Nothing) Then
                Me.procesarRespuestaOperationCompleted = AddressOf Me.OnprocesarRespuestaOperationCompleted
            End If
            Me.InvokeAsync("procesarRespuesta", New Object() {usuarioSIFEI, passwordSIFEI, rfcReceptor, folios, pfx, passwordPfx}, Me.procesarRespuestaOperationCompleted, userState)
        End Sub
        
        Private Sub OnprocesarRespuestaOperationCompleted(ByVal arg As Object)
            If (Not (Me.procesarRespuestaCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent procesarRespuestaCompleted(Me, New procesarRespuestaCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace:="http://service.sifei.cancelacion/", ResponseNamespace:="http://service.sifei.cancelacion/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function cfdiRelacionado(<System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal usuarioSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordSIFEI As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal rfcReceptor As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal uuid As String, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified, DataType:="base64Binary", IsNullable:=true)> ByVal pfx() As Byte, <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> ByVal passwordPfx As String) As <System.Xml.Serialization.XmlElementAttribute("return", Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)> String
            Dim results() As Object = Me.Invoke("cfdiRelacionado", New Object() {usuarioSIFEI, passwordSIFEI, rfcReceptor, uuid, pfx, passwordPfx})
            Return CType(results(0),String)
        End Function
        
        '''<remarks/>
        Public Overloads Sub cfdiRelacionadoAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal rfcReceptor As String, ByVal uuid As String, ByVal pfx() As Byte, ByVal passwordPfx As String)
            Me.cfdiRelacionadoAsync(usuarioSIFEI, passwordSIFEI, rfcReceptor, uuid, pfx, passwordPfx, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub cfdiRelacionadoAsync(ByVal usuarioSIFEI As String, ByVal passwordSIFEI As String, ByVal rfcReceptor As String, ByVal uuid As String, ByVal pfx() As Byte, ByVal passwordPfx As String, ByVal userState As Object)
            If (Me.cfdiRelacionadoOperationCompleted Is Nothing) Then
                Me.cfdiRelacionadoOperationCompleted = AddressOf Me.OncfdiRelacionadoOperationCompleted
            End If
            Me.InvokeAsync("cfdiRelacionado", New Object() {usuarioSIFEI, passwordSIFEI, rfcReceptor, uuid, pfx, passwordPfx}, Me.cfdiRelacionadoOperationCompleted, userState)
        End Sub
        
        Private Sub OncfdiRelacionadoOperationCompleted(ByVal arg As Object)
            If (Not (Me.cfdiRelacionadoCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent cfdiRelacionadoCompleted(Me, New cfdiRelacionadoCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024)  _
                        AndAlso (String.Compare(wsUri.Host, "localHost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return true
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://service.sifei.cancelacion/")>  _
    Partial Public Class folios
        
        Private respuestaField As respuesta
        
        Private respuestaFieldSpecified As Boolean
        
        Private uuidField As String
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property respuesta() As respuesta
            Get
                Return Me.respuestaField
            End Get
            Set
                Me.respuestaField = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>  _
        Public Property respuestaSpecified() As Boolean
            Get
                Return Me.respuestaFieldSpecified
            End Get
            Set
                Me.respuestaFieldSpecified = value
            End Set
        End Property
        
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(Form:=System.Xml.Schema.XmlSchemaForm.Unqualified)>  _
        Public Property uuid() As String
            Get
                Return Me.uuidField
            End Get
            Set
                Me.uuidField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.6.1590.0"),  _
     System.SerializableAttribute(),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://service.sifei.cancelacion/")>  _
    Public Enum respuesta
        
        '''<remarks/>
        Aceptacion
        
        '''<remarks/>
        Rechazo
    End Enum
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub peticionesPendientesCompletedEventHandler(ByVal sender As Object, ByVal e As peticionesPendientesCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class peticionesPendientesCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub consultaSATCFDICompletedEventHandler(ByVal sender As Object, ByVal e As consultaSATCFDICompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class consultaSATCFDICompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub cancelaCFDICompletedEventHandler(ByVal sender As Object, ByVal e As cancelaCFDICompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class cancelaCFDICompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub procesarRespuestaCompletedEventHandler(ByVal sender As Object, ByVal e As procesarRespuestaCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class procesarRespuestaCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0")>  _
    Public Delegate Sub cfdiRelacionadoCompletedEventHandler(ByVal sender As Object, ByVal e As cfdiRelacionadoCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.6.1590.0"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class cfdiRelacionadoCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As String
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),String)
            End Get
        End Property
    End Class
End Namespace