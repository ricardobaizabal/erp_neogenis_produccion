Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Departamentos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call muestraDepartamentos()
        End If
    End Sub

    Private Sub grdDepartamentos_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdDepartamentos.ItemCommand
        Select Case e.CommandName

            Case "cmdEdit"
                EditaDepartamento(e.CommandArgument)

            Case "cmdDelete"
                EliminaDepartamento(e.CommandArgument)

        End Select
    End Sub

    Private Sub grdDepartamentos_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdDepartamentos.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Departamentos" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('¿Está seguro que desea borrar este departamento de la base de datos?');")

            End If

        End If
    End Sub

    Private Sub muestraDepartamentos()
        Dim ds As New DataSet()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("EXEC pDepartamentos @cmd=1")
        grdDepartamentos.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdDepartamentos.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub btnAgregaDepto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregaDepto.Click
        InsertOrUpdate.Value = 0
        txtNombre.Text = ""
        panelRegistroDepto.Visible = True
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        txtNombre.Text = ""
        panelRegistroDepto.Visible = False
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Dim ObjData As New DataControl
        If InsertOrUpdate.Value = 0 Then
            ObjData.RunSQLQuery("EXEC pDepartamentos @cmd=4, @nombre='" & txtNombre.Text.ToString.ToUpper & "'")
        Else
            ObjData.RunSQLQuery("EXEC pDepartamentos @cmd=5, @nombre='" & txtNombre.Text.ToString.ToUpper & "', @departamentoid='" & departamentoID.Value.ToString & "'")
        End If

        ObjData = Nothing
        Response.Redirect("Departamentos.aspx")

    End Sub

    Private Sub EditaDepartamento(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pDepartamentos @cmd=2, @departamentoid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                '
                txtNombre.Text = rs("nombre")
                '
                panelRegistroDepto.Visible = True
                '
                InsertOrUpdate.Value = 1
                departamentoID.Value = rs("id")

            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub EliminaDepartamento(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pDepartamentos @cmd=3, @departamentoid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelRegistroDepto.Visible = False

            Response.Redirect("Departamentos.aspx")

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

End Class