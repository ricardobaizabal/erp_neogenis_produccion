Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.IO

Partial Class portalcfd_Configuracion
    Inherits System.Web.UI.Page

#Region "Load Initial Values"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle

        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then

            '''''''''''''''''''
            'Fieldsets Legends'
            '''''''''''''''''''

            lblPrivateKeyLegend.Text = Resources.Resource.lblPrivateKeyLegend
            lblCertificateLegend.Text = Resources.Resource.lblCertificateLegend
            lblCertificatesListLegend.Text = Resources.Resource.lblCertificatesListLegend

            ''''''''''''''
            'Label Titles'
            ''''''''''''''

            lblPrivateKey.Text = Resources.Resource.lblPrivateKey
            lblPrivateKeyDownload.Text = Resources.Resource.lblPrivateKeyDownload
            lblPasword.Text = Resources.Resource.lblPassword
            lblCertificate.Text = Resources.Resource.lblCertificate
            lblActivate.Text = Resources.Resource.lblActivate

            '''''''''''''''''''
            'Validators Titles'
            '''''''''''''''''''
            RequiredFieldValidator1.Text = Resources.Resource.validatorMessage
            CustomValidator.Text = Resources.Resource.validatorMessage
            ValidateExtensions.Text = Resources.Resource.InvalidExtension

            CustomValidator2.Text = Resources.Resource.validatorMessage
            ValidateExtensions2.Text = Resources.Resource.InvalidExtension

            ''''''''''''''''
            'Buttons Titles'
            ''''''''''''''''

            btnSavePrivateKey.Text = Resources.Resource.btnSave
            btnSaveCertificate.Text = Resources.Resource.btnSave

            '''''''''''''''''''''''''''''''
            'Private Key & Password Values'
            '''''''''''''''''''''''''''''''

            DisplayPrivateKeyAndPassword()

            '''''''''''''''''''''''''''
            'Telerik Grid Data Binding'
            '''''''''''''''''''''''''''

            DisplayCertificates()

        End If
        '
        If Session("admin") <> 1 Then
            fileIcon.Enabled = False
            lnkDownloadPrivateKey.Enabled = False
        End If
        ''

        If Session("perfilid") <> 1 Then
            If Session("userid") <> 13 Or Session("userid") <> 18 Or Session("userid") <> 32 Then
                Response.Redirect("~/portalcfd/Home.aspx")
                'ElseIf Session("userid") <> 8 Then
                '    Response.Redirect("~/portalcfd/Home.aspx")

                'ElseIf Session("userid") <> 32 Then
                '    Response.Redirect("~/portalcfd/Home.aspx")
            End If
        End If
    End Sub

#End Region

#Region "Private Key & Password Loading"

    Protected Sub DisplayPrivateKeyAndPassword()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCliente @cmd=2, @clienteid='" & Session("clienteid") & "'", conn)

        conn.Open()

        Dim rs As SqlDataReader
        rs = cmd.ExecuteReader()

        If rs.Read Then

            txtPassword.Text = rs("contrasena_llave_privada")
            'txtPassword.Attributes.Add("value", rs("contrasena_llave_privada"))

            '''''''''''''''''''''''''''''''''''''''''''''''''''
            'Verify If A Private Key & Password Already Exists'
            '''''''''''''''''''''''''''''''''''''''''''''''''''

            If (rs("archivo_llave_privada").ToString.Length > 4) Then

                fileName.Value = rs("archivo_llave_privada")
                lnkDownloadPrivateKey.Text = rs("archivo_llave_privada")

                lblPrivateKeyDownload.Visible = True
                fileIcon.Visible = True
                lnkDownloadPrivateKey.Visible = True

            End If

        Else

            lblPrivateKeyDownload.Visible = False
            fileIcon.Visible = False
            lnkDownloadPrivateKey.Visible = False

        End If

        rs.Close()
        conn.Close()

    End Sub

#End Region

#Region "Telerik Grid Certificates Binding"

    Private Sub DisplayCertificates()

        Dim ObjData As New DataControl

        Try

            certificatesList.MasterTableView.NoMasterRecordsText = Resources.Resource.certificatesEmptyGridMessage
            certificatesList.DataSource = ObjData.FillDataSet("EXEC pCertificados @cmd=5")
            certificatesList.DataBind()

        Catch ex As Exception

        Finally

            ObjData = Nothing

        End Try

    End Sub

    Protected Sub certificatesList_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles certificatesList.NeedDataSource

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCertificados, @cmd=5", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            certificatesList.MasterTableView.NoMasterRecordsText = Resources.Resource.certificatesEmptyGridMessage
            certificatesList.DataSource = ds
            certificatesList.DataBind()

            conn.Close()
            conn.Dispose()

        Catch ex As Exception

        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

#End Region

#Region "Telerik Grid Active Image & Certificate Events (Download & Delete)"

    Protected Sub certificatesList_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles certificatesList.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Certificates" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('" & Resources.Resource.CertificateDeleteConfirmationMessage & "');")
                Dim btnDownload As ImageButton = CType(e.Item.FindControl("btnDownload"), ImageButton)
                If Session("admin") <> 1 Then
                    btnDownload.Enabled = False
                End If

                Dim imgDownload As Image = CType(dataItem("activo").FindControl("imgActive"), Image)

                If e.Item.DataItem("activo") <> "False" Then

                    imgDownload.ImageUrl = "~/images/icons/arrow.gif"

                Else

                    imgDownload.ImageUrl = "~/images/icons/close.gif"

                End If

            End If

        End If

    End Sub

    Protected Sub certificatesList_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles certificatesList.ItemCommand

        Select Case e.CommandName

            Case "cmdDownloadCertificate"
                DownloadCertificate(e.CommandArgument)

            Case "cmdDeleteCertificate"
                DeleteCertificateFromFolder(e.CommandArgument)
                DeleteCertificateFromDB(e.CommandArgument)
                DisplayCertificates()

        End Select

    End Sub

    Private Sub DownloadCertificate(ByVal id As Integer)

        Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCertificados @cmd=6, @certificadoId ='" & id.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                Dim FilePath As String = System.Configuration.ConfigurationManager.AppSettings("CertificatesStoragePath") + rs("certificado")

                Trace.Write(FilePath)

                If File.Exists(FilePath) Then
                    Dim FileName As String = Path.GetFileName(FilePath)
                    Response.Clear()
                    Response.ContentType = "application/octet-stream"
                    Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
                    Response.Flush()
                    Response.WriteFile(FilePath)
                End If

            End If

            rs.Close()
            conn.Close()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub DeleteCertificateFromFolder(ByVal id As Integer)

        Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCertificados @cmd=6, @certificadoId ='" & id.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                File.Delete(System.Configuration.ConfigurationManager.AppSettings("CertificatesStoragePath") + rs("certificado"))

            End If

            rs.Close()
            conn.Close()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub DeleteCertificateFromDB(ByVal id As Integer)

        Dim conn As New SqlConnection(ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("EXEC pCertificados @cmd=4, @certificadoId ='" & id.ToString & "'", conn)

        Try

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

#End Region

#Region "Telerik Grid Items Column Names (From Resource File)"

    Protected Sub certificatesList_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles certificatesList.ItemCreated

        If TypeOf e.Item Is GridHeaderItem Then

            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)

            If e.Item.OwnerTableView.Name = "Certificates" Then

                header("download").Text = Resources.Resource.gridColumnNameCertificate
                header("certificado").Text = Resources.Resource.gridColumnNameCertificate
                header("activo").Text = Resources.Resource.gridColumnNameActive
                header("delete").Text = Resources.Resource.gridColumnNameDelete

            End If

        End If

    End Sub

#End Region

#Region "Private Key Download Events"

    Protected Sub fileIcon_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles fileIcon.Click

        Dim FilePath As String = System.Configuration.ConfigurationManager.AppSettings("PrivateKeyStoragePath") + fileName.Value.ToString

        Trace.Write(FilePath)

        If File.Exists(FilePath) Then

            Dim PrivateKeyFile As String = Path.GetFileName(FilePath)

            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & PrivateKeyFile & """")
            Response.Flush()
            Response.WriteFile(FilePath)

        End If

    End Sub

    Protected Sub lnkDownloadPrivateKey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkDownloadPrivateKey.Click

        Dim FilePath As String = System.Configuration.ConfigurationManager.AppSettings("PrivateKeyStoragePath") + fileName.Value.ToString

        Trace.Write(FilePath)

        If File.Exists(FilePath) Then

            Dim PrivateKeyFile As String = Path.GetFileName(FilePath)

            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & PrivateKeyFile & """")
            Response.Flush()
            Response.WriteFile(FilePath)

        End If

    End Sub

#End Region

#Region "Save Private Key"

    Protected Sub btnSavePrivateKey_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSavePrivateKey.Click

        Try

            If ((RadUpload1.UploadedFiles.Count = 0) And (lnkDownloadPrivateKey.Text <> "")) Then

                Dim newFileName As String = lnkDownloadPrivateKey.Text

                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                Dim cmd As New SqlCommand("EXEC pCliente @cmd=1, @archivo_llave_privada='" & newFileName & "', @contrasena_llave_privada='" & txtPassword.Text & "', @clienteid='" & Session("clienteid") & "'", conn)

                conn.Open()

                Dim rs As SqlDataReader

                rs = cmd.ExecuteReader()
                rs.Close()

                conn.Close()

                Call DisplayPrivateKeyAndPassword()


            Else

                Dim newFileName As String = ""

                For Each validFile As UploadedFile In RadUpload1.UploadedFiles

                    newFileName = validFile.GetName()

                    validFile.SaveAs(System.Configuration.ConfigurationManager.AppSettings("PrivateKeyStoragePath") + newFileName)

                Next

                Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
                Dim cmd As New SqlCommand("EXEC pCliente @cmd=1, @archivo_llave_privada='" & newFileName & "', @contrasena_llave_privada='" & txtPassword.Text & "', @clienteid='" & Session("clienteid") & "'", conn)

                conn.Open()

                Dim rs As SqlDataReader

                rs = cmd.ExecuteReader()
                rs.Close()

                conn.Close()

                Call DisplayPrivateKeyAndPassword()

            End If

        Catch ex As Exception

        End Try

    End Sub

#End Region

#Region "Save Certificate"

    Protected Sub btnSaveCertificate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCertificate.Click

        Try

            Dim newFileName As String = ""

            For Each validFile As UploadedFile In RadUpload2.UploadedFiles

                newFileName = validFile.GetName()

                validFile.SaveAs(System.Configuration.ConfigurationManager.AppSettings("CertificatesStoragePath") + newFileName)

            Next

            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
            Dim cmd As New SqlCommand("EXEC pCertificados @cmd=1, @archivoCertificado='" & newFileName & "', @activo='" & chckActivate.Checked & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            Call DisplayCertificates()

        Catch ex As Exception

        End Try

    End Sub

#End Region

End Class
