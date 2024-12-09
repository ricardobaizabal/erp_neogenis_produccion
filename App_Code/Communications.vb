Imports Microsoft.VisualBasic
Imports System.Net.Mail
Public Class Communications
    Private _EmailTo As String = ""
    Private _EmailCc As String = ""
    Private _EmailBcc As String = ""
    Private _EmailFrom As String = ""
    Private _EmailSubject As String = ""
    Private _EmailBody As String = ""
    Private _EmailHost As String = ""
    Private _EmailIsAuth As Boolean = False
    Private _EmailUsername As String = ""
    Private _EmailPassword As String = ""
    WriteOnly Property EmailTo() As String
        Set(ByVal value As String)
            _EmailTo = value
        End Set
    End Property
    WriteOnly Property EmailCc() As String
        Set(ByVal value As String)
            _EmailCc = value
        End Set
    End Property
    WriteOnly Property EmailBcc() As String
        Set(ByVal value As String)
            _EmailBcc = value
        End Set
    End Property
    WriteOnly Property EmailFrom() As String
        Set(ByVal value As String)
            _EmailFrom = value
        End Set
    End Property
    WriteOnly Property EmailSubject() As String
        Set(ByVal value As String)
            _EmailSubject = value
        End Set
    End Property
    WriteOnly Property EmailBody() As String
        Set(ByVal value As String)
            _EmailBody = value
        End Set
    End Property
    WriteOnly Property EmailHost() As String
        Set(ByVal value As String)
            _EmailHost = value
        End Set
    End Property
    WriteOnly Property EmailIsAuth() As Boolean
        Set(ByVal value As Boolean)
            _EmailIsAuth = value
        End Set
    End Property
    WriteOnly Property EmailUsername() As String
        Set(ByVal value As String)
            _EmailUsername = value
        End Set
    End Property
    WriteOnly Property EmailPassword() As String
        Set(ByVal value As String)
            _EmailPassword = value
        End Set
    End Property
    Public Sub EmailSend()
        Dim mm As New MailMessage(_EmailFrom, _EmailTo)
        If _EmailBcc.Length > 0 Then
            Dim mailBcc As New MailAddress(_EmailBcc)
            mm.Bcc.Add(mailBcc)
        End If
        mm.Subject = _EmailSubject
        mm.Body = Replace(_EmailBody, vbCrLf, "<br />")
        mm.IsBodyHtml = True
        '
        Dim SmtpMail As New SmtpClient
        Try
            Dim SmtpUser As New Net.NetworkCredential
            SmtpUser.UserName = "enviosweb@linkium.mx"
            SmtpUser.Password = "Link2020"
            'SmtpUser.Domain = "globaltime.mx"
            SmtpMail.UseDefaultCredentials = False
            SmtpMail.Credentials = SmtpUser
            SmtpMail.Host = "smtp.linkium.mx"
            SmtpMail.DeliveryMethod = SmtpDeliveryMethod.Network
            SmtpMail.Send(mm)
        Catch ex As Exception
            '
            '
        Finally
            SmtpMail = Nothing
        End Try
    End Sub
End Class
