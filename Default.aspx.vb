Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        '
        Dim SQL As String = ""
        If System.Configuration.ConfigurationManager.AppSettings("usuarios") = 0 Then
            SQL = "exec pLogin @email='" + email.Text + "', @contrasena='" + contrasena.Text + "'"
        Else
            SQL = "exec pUsuarios @cmd=1, @email='" + email.Text + "', @contrasena='" + contrasena.Text + "'"
        End If
        '

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand(SQL, conn)
        Dim ClienteValido As Boolean = False
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                If rs("error") = 1 Then
                    lblMensaje.Text = rs("mensaje")
                    ClienteValido = False
                Else
                    Session("admin") = rs("admin")
                    Session("clienteId") = rs("clienteId")
                    Session("razonsocial") = rs("razonsocial")
                    Session("contacto") = rs("contacto")
                    Session("logo") = rs("logo")
                    Session("logo_formato") = rs("logo_formato")

                    ClienteValido = True
                    If chkRemember.Checked = True Then
                        CookieUtil.SetTripleDESEncryptedCookie("email", email.Text, Now.AddDays(365))
                        CookieUtil.SetTripleDESEncryptedCookie("contrasena", contrasena.Text, Now.AddDays(365))
                    Else
                        CookieUtil.SetTripleDESEncryptedCookie("email", "", Now.AddDays(-1))
                        CookieUtil.SetTripleDESEncryptedCookie("contrasena", "", Now.AddDays(-1))
                    End If
                    '
                    If System.Configuration.ConfigurationManager.AppSettings("usuarios") = 1 And Session("admin") = 0 Then
                        Session("userid") = rs("id")
                        Session("nombre") = rs("nombre")
                        Session("perfilid") = rs("perfilid")
                        Session("sucursalid") = rs("sucursalid")
                    Else
                        Session("userid") = 0
                        Session("nombre") = ""
                        Session("perfilid") = 0
                        Session("sucursalid") = 0
                    End If
                    '
                End If

            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally

            conn.Close()

        End Try
        '
        If ClienteValido Then
            Response.Redirect("~/portalcfd/Home.aspx")
        End If
        '
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        Me.Title = Resources.Resource.WindowsTitle
        '
        'Call CargaLogo()
        '
        '   Remember Function
        '
        If Not IsPostBack Then
            If Not CookieUtil.GetTripleDESEncryptedCookieValue("email") Is Nothing Then
                chkRemember.Checked = True
                email.Text = CookieUtil.GetTripleDESEncryptedCookieValue("email")
                contrasena.Attributes.Add("value", CookieUtil.GetTripleDESEncryptedCookieValue("contrasena"))
            End If
        End If
        '
    End Sub


    Private Sub CargaLogo()
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("select top 1 logo from tblCliente", conn)
        Dim ClienteValido As Boolean = False
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                imgBanner.ImageUrl = "~/portalcfd/logos/" & rs("logo")
            End If

        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally

            conn.Close()

        End Try
    End Sub

End Class
