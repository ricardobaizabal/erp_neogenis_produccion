Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class CuentasBeneficiario
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            btnGuardar.Text = Resources.Resource.btnSave
            btnCancelar.Text = Resources.Resource.btnCancel
        End If
    End Sub

    Private Sub EditCuenta(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCatalogoCuentasBeneficiario @cmd=2, @id=" & id, conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                txtBanco.Text = rs("banco")
                txtCuenta.Text = rs("numctapago")
                txtRFC.Text = rs("rfc")
                chkPredeterminado.Checked = rs("predeterminadoBit")

                panelRegistration.Visible = True
                btnGuardar.Text = "Actualizar"
                InsertOrUpdate.Value = 1
                CuentaID.Value = id

            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub DeleteCuenta(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pCatalogoCuentasBeneficiario @cmd=3, @id=" & id.ToString, conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelRegistration.Visible = False

            Cuentaslist.DataSource = GetCuenta()
            Cuentaslist.DataBind()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
        LimpiarCampos()
        InsertOrUpdate.Value = 0
        CuentaID.Value = 0
        btnGuardar.Text = "Guardar"
        panelRegistration.Visible = True
        txtBanco.Focus()
    End Sub

    Function GetCuenta() As DataSet

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pCatalogoCuentasBeneficiario @cmd=5", conn)

        Dim ds As DataSet = New DataSet

        Try

            conn.Open()

            cmd.Fill(ds)

            conn.Close()
            conn.Dispose()

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

        Return ds

    End Function

    Protected Sub Cuentaslist_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles Cuentaslist.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                EditCuenta(e.CommandArgument)

            Case "cmdDelete"
                DeleteCuenta(e.CommandArgument)

        End Select
    End Sub

    Protected Sub Cuentaslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles Cuentaslist.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Cuenta" Then
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('Va a borrar este Registro. ¿Desea continuar?');")
            End If
        End If
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim imgPredeterminado As System.Web.UI.WebControls.Image = CType(e.Item.FindControl("imgPredeterminado"), System.Web.UI.WebControls.Image)
                imgPredeterminado.Visible = e.Item.DataItem("predeterminadoBit")
        End Select
    End Sub

    Protected Sub Cuentaslist_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles Cuentaslist.NeedDataSource
        If Not e.IsFromDetailTable Then
            Cuentaslist.MasterTableView.NoMasterRecordsText = "No se encontraron registros"
            Cuentaslist.DataSource = GetCuenta()
        End If
    End Sub

    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        btnGuardar.Text = Resources.Resource.btnSave
        LimpiarCampos()
        panelRegistration.Visible = False
    End Sub

    Private Sub LimpiarCampos()
        txtBanco.Text = ""
        txtRFC.Text = ""
        txtCuenta.Text = ""
        chkPredeterminado.Checked = 0
    End Sub

    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        If Page.IsValid Then

            Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

            Try
                If InsertOrUpdate.Value = 0 Then

                    Dim cmd As New SqlCommand("EXEC pCatalogoCuentasBeneficiario @cmd=1, @banco='" & txtBanco.Text & "', @rfc='" & txtRFC.Text & "', @numctapago='" & txtCuenta.Text & "', @predeterminadoBit='" & chkPredeterminado.Checked & "'", conn)
                    conn.Open()

                    cmd.ExecuteReader()

                    panelRegistration.Visible = False

                    Cuentaslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                    Cuentaslist.DataSource = GetCuenta()
                    Cuentaslist.DataBind()

                    conn.Close()
                    conn.Dispose()

                Else
                    Dim cmd As New SqlCommand("EXEC pCatalogoCuentasBeneficiario @cmd=4, @banco='" & txtBanco.Text & "', @rfc='" & txtRFC.Text & "', @numctapago='" & txtCuenta.Text & "', @predeterminadoBit='" & chkPredeterminado.Checked & "', @id=" & CuentaID.Value.ToString, conn)

                    conn.Open()

                    cmd.ExecuteReader()

                    panelRegistration.Visible = False

                    Cuentaslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                    Cuentaslist.DataSource = GetCuenta()
                    Cuentaslist.DataBind()

                    conn.Close()
                    conn.Dispose()

                End If

            Catch ex As Exception
                Response.Write(ex.Message)
                Response.End()
            Finally
                conn.Close()
                conn.Dispose()
            End Try
        End If
    End Sub

End Class