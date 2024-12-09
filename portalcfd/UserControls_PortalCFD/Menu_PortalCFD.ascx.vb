
Partial Class portalcfd_usercontrols_portalcfd_Menu_PortalCFD
    Inherits System.Web.UI.UserControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If System.Configuration.ConfigurationManager.AppSettings("usuarios") = 1 And Session("admin") = 0 Then
            lblUsuario.Text = "Usuario en sesión: <strong>" & Session("nombre").ToString & "</strong>"
            '
            '   Permisos para el menu
            '
            Select Case Session("perfilid")
                Case 3, 5
                    '   Nómina
                    RadMenu1.Items(1).Visible = False
                    '   Cuentas por cobrar
                    RadMenu1.Items(2).Items(1).Visible = False

                    '   Proveedores
                    RadMenu1.Items(3).Visible = False

                    '   Inventarios
                    RadMenu1.Items(4).Items(1).Visible = False
                    RadMenu1.Items(4).Items(2).Visible = False
                    RadMenu1.Items(4).Items(3).Visible = False
                    RadMenu1.Items(4).Items(4).Visible = True
                    RadMenu1.Items(4).Items(5).Visible = False
                    RadMenu1.Items(4).Items(6).Visible = False
                    RadMenu1.Items(4).Items(7).Visible = False
                    RadMenu1.Items(4).Items(10).Visible = False
                    RadMenu1.Items(4).Items(11).Visible = False

                    '   Facturación
                    If Session("perfilid") <> 2 Then
                        RadMenu1.Items(5).Items(0).Visible = False
                        RadMenu1.Items(5).Items(2).Visible = False
                        RadMenu1.Items(5).Items(3).Visible = False
                    End If

                    RadMenu1.Items(5).Items(4).Visible = False
                    RadMenu1.Items(5).Items(5).Visible = False

                    '   Reportes
                    'RadMenu1.Items(7).Visible = False
                    '   Configuración
                    RadMenu1.Items(8).Visible = False

                Case 4
                    '   Nómina
                    RadMenu1.Items(1).Visible = False
                    '   Cuentas por cobrar
                    RadMenu1.Items(2).Items(1).Visible = False
                    '   Proveedores
                    If (Session("userid") = 26) Then
                        RadMenu1.Items(3).Items(0).Visible = False
                        RadMenu1.Items(3).Items(1).Visible = True
                    End If
                    If (Session("userid") = 37) Then
                        RadMenu1.Items(3).Items(0).Visible = False
                        RadMenu1.Items(3).Items(1).Visible = True

                    End If

                    If (Session("userid") <> 37 And Session("userid") <> 26) Then
                        RadMenu1.Items(3).Visible = False

                    End If

                    '   Inventarios
                    RadMenu1.Items(4).Items(1).Visible = False
                    RadMenu1.Items(4).Items(2).Visible = False
                    RadMenu1.Items(4).Items(3).Visible = False
                    RadMenu1.Items(4).Items(4).Visible = True
                    RadMenu1.Items(4).Items(5).Visible = False
                    RadMenu1.Items(4).Items(6).Visible = False
                    RadMenu1.Items(4).Items(7).Visible = False
                    '   Facturación
                    If Session("perfilid") <> 2 Then
                        RadMenu1.Items(5).Items(0).Visible = False
                        RadMenu1.Items(5).Items(2).Visible = False
                        RadMenu1.Items(5).Items(3).Visible = False
                    End If
                    RadMenu1.Items(5).Items(4).Visible = False
                    RadMenu1.Items(5).Items(5).Visible = False
                    '   Reportes
                    'RadMenu1.Items(7).Visible = False
                    '   Configuración
                    RadMenu1.Items(8).Visible = False

                Case 2
                    '   Nómina
                    RadMenu1.Items(1).Visible = False
                    '   Cuentas por cobrar
                    RadMenu1.Items(2).Items(1).Visible = False
                    '   Proveedores
                    If (Session("userid") <> 41 Or Session("userid") <> 34) Then

                        RadMenu1.Items(3).Visible = False
                    End If

                    If (Session("userid") = 41) Then
                        RadMenu1.Items(3).Visible = True

                    End If

                    If (Session("userid") = 34) Then
                        RadMenu1.Items(3).Visible = True

                    End If

                    '   Inventarios
                    RadMenu1.Items(4).Items(1).Visible = False
                    RadMenu1.Items(4).Items(2).Visible = False
                    RadMenu1.Items(4).Items(3).Visible = False
                    RadMenu1.Items(4).Items(4).Visible = True
                    RadMenu1.Items(4).Items(5).Visible = False
                    RadMenu1.Items(4).Items(6).Visible = False
                    RadMenu1.Items(4).Items(7).Visible = False
                    '   Facturación
                    If Session("perfilid") <> 2 Then
                        RadMenu1.Items(5).Items(0).Visible = False
                        RadMenu1.Items(5).Items(2).Visible = False
                        RadMenu1.Items(5).Items(3).Visible = False
                    End If
                    RadMenu1.Items(5).Items(4).Visible = False
                    RadMenu1.Items(5).Items(5).Visible = False
                    '   Reportes
                    'RadMenu1.Items(7).Visible = False
                    '   Configuración
                    RadMenu1.Items(8).Visible = False
            End Select
            '
        End If
        '
    End Sub

End Class