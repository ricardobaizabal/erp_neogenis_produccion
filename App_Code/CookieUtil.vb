Public Class CookieUtil

    'SET COOKIE FUNCTIONS *****************************************************

    'SetTripleDESEncryptedCookie - key & value only
    Public Shared Sub SetTripleDESEncryptedCookie(ByVal key As String, _
            ByVal value As String)
        'Convert parts
        key = CryptoUtil.EncryptTripleDES(key)
        value = CryptoUtil.EncryptTripleDES(value)

        SetCookie(key, value)
    End Sub

    'SetTripleDESEncryptedCookie - overloaded method with expires parameter
    Public Shared Sub SetTripleDESEncryptedCookie(ByVal key As String, _
            ByVal value As String, ByVal expires As Date)
        'Convert parts
        key = CryptoUtil.EncryptTripleDES(key)
        value = CryptoUtil.EncryptTripleDES(value)

        SetCookie(key, value, expires)
    End Sub


    'SetEncryptedCookie - key & value only
    Public Shared Sub SetEncryptedCookie(ByVal key As String, _
            ByVal value As String)
        'Convert parts
        key = CryptoUtil.Encrypt(key)
        value = CryptoUtil.Encrypt(value)

        SetCookie(key, value)
    End Sub

    'SetEncryptedCookie - overloaded method with expires parameter
    Public Shared Sub SetEncryptedCookie(ByVal key As String, _
            ByVal value As String, ByVal expires As Date)
        'Convert parts
        key = CryptoUtil.Encrypt(key)
        value = CryptoUtil.Encrypt(value)

        SetCookie(key, value, expires)
    End Sub


    'SetCookie - key & value only
    Public Shared Sub SetCookie(ByVal key As String, ByVal value As String)
        'Encode Part
        key = HttpContext.Current.Server.UrlEncode(key)
        value = HttpContext.Current.Server.UrlEncode(value)

        Dim cookie As HttpCookie
        cookie = New HttpCookie(key, value)
        SetCookie(cookie)
    End Sub

    'SetCookie - overloaded with expires parameter
    Public Shared Sub SetCookie(ByVal key As String, _
            ByVal value As String, ByVal expires As Date)
        'Encode Parts
        key = HttpContext.Current.Server.UrlEncode(key)
        value = HttpContext.Current.Server.UrlEncode(value)

        Dim cookie As HttpCookie
        cookie = New HttpCookie(key, value)
        cookie.Expires = expires
        SetCookie(cookie)
    End Sub

    'SetCookie - HttpCookie only
    'final step to set the cookie to the response clause
    Public Shared Sub SetCookie(ByVal cookie As HttpCookie)
        HttpContext.Current.Response.Cookies.Set(cookie)
    End Sub

    'GET COOKIE FUNCTIONS *****************************************************

    Public Shared Function GetTripleDESEncryptedCookieValue(ByVal key As String) As String
        'encrypt key only - encoding done in GetCookieValue
        key = CryptoUtil.EncryptTripleDES(key)

        'get value 
        Dim value As String
        value = GetCookieValue(key)
        'decrypt value
        value = CryptoUtil.DecryptTripleDES(value)
        Return value
    End Function

    Public Shared Function GetEncryptedCookieValue(ByVal key As String) As String
        'encrypt key only - encoding done in GetCookieValue
        key = CryptoUtil.Encrypt(key)

        'get value 
        Dim value As String
        value = GetCookieValue(key)
        'decrypt value
        value = CryptoUtil.Decrypt(value)
        Return value
    End Function
    Public Shared Function GetCookie(ByVal key As String) As HttpCookie
        'encode key for retrieval
        key = HttpContext.Current.Server.UrlEncode(key)
        Return HttpContext.Current.Request.Cookies.Get(key)
    End Function
    Public Shared Function GetCookieValue(ByVal key As String) As String
        Dim value As String = ""
        Try
            'don't encode key for retrieval here
            'done in the GetCookie function
            value = GetCookie(key).Value
            'decode stored value
            value = HttpContext.Current.Server.UrlDecode(value)
        Catch
            '
        End Try
        Return value
    End Function

End Class
