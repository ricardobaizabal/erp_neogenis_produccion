Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Prioridad
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        panelRegistration.Visible = True
        InsertOrUpdate.Value = 0
        txtPorcentaje.Text = ""
        btnGuardar.Text = "Guardar"
    End Sub

    Private Sub PrioridadesList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles PrioridadesList.ItemCommand
        Select Case e.CommandName
            Case "cmdEdit"
                GetEditPrioridad(e.CommandArgument)
            Case "cmdDelete"
                DeleteUnidad(e.CommandArgument)
        End Select
    End Sub

    Private Sub PrioridadesList_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles PrioridadesList.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then
            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Marcas" Then
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('Va a borrar este Registro. ¿Desea continuar?');")
            End If
        End If
    End Sub

    Private Sub PrioridadesList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles PrioridadesList.NeedDataSource
        PrioridadesList.MasterTableView.NoMasterRecordsText = " No se han agregado prioridades"
        PrioridadesList_NeedData("off")
    End Sub
    Public Sub PrioridadesList_NeedData(ByVal state As String)
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pPrioridades @cmd=2", conn)

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
        PrioridadesList.DataSource = ds
        If state = "on" Then
            PrioridadesList.DataBind()
        End If
    End Sub
    Private Sub GetEditPrioridad(ByVal id As Integer)
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try
            Dim cmd As New SqlCommand("EXEC pPrioridades @cmd=3, @id=" & id, conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()
            If rs.Read Then
                txtPrioridad.Text = rs("nombre")
                txtPorcentaje.Text = rs("porcentaje")
                panelRegistration.Visible = True
                btnGuardar.Text = "Actualizar"
                InsertOrUpdate.Value = 1
                PrioridadID.Value = id
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub
    Private Sub DeleteUnidad(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try
            Dim cmd As New SqlCommand("EXEC pPrioridades @cmd=5, @id=" & id.ToString, conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()
            rs.Close()
            conn.Close()
            panelRegistration.Visible = False
            Call PrioridadesList_NeedData("on")
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try
            Dim ncmd As Integer = 0
            If InsertOrUpdate.Value = 0 Then
                ncmd = 1  ' cmd insert
            Else
                ncmd = 4 ' cmd update
            End If

            Dim cmd As New SqlCommand("EXEC pPrioridades @cmd=" & ncmd & ", @nombre='" & txtPrioridad.Text & "', @porcentaje='" & txtPorcentaje.Text & "', @id=" & PrioridadID.Value.ToString, conn)
            conn.Open()
            cmd.ExecuteReader()
            panelRegistration.Visible = False
            PrioridadesList.MasterTableView.NoMasterRecordsText = "No se han agregado prioridades"
            txtPrioridad.Text = ""
            txtPorcentaje.Text = ""
            Call PrioridadesList_NeedData("on")
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
    End Sub

    Private Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        btnGuardar.Text = "btnGuardar"
        txtPrioridad.Text = ""
        txtPorcentaje.Text = ""
        panelRegistration.Visible = False
    End Sub

    Private Sub PrioridadesList_Load(sender As Object, e As EventArgs) Handles PrioridadesList.Load

    End Sub
End Class