Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class CuentasConcentradoras
    Inherits System.Web.UI.Page

    Private _cuentaConcentradoras As New CuentaConcentradora
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then

            lblNombre.Text = "Nombre:"
            btnCancelar.Text = Resources.Resource.btnCancel
        End If
    End Sub
    Private Sub btnClientAutorizadoSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        _cuentaConcentradoras.id = cuentaconcentradoraid.Value
        _cuentaConcentradoras.nombre = txtNombre.Text
        _cuentaConcentradoras.Save()
        cleanInputClientAutorizado()
        ClientAutorizadoList_NeedData()
    End Sub
    Sub cleanInputClientAutorizado()
        cuentaconcentradoraid.Value = 0
        btnSave.Text = "Agregar"
        btnCancelar.Visible = False
        txtNombre.Text = ""
    End Sub
    Private Sub ClientAutorizadoEdit(id As Integer)
        _cuentaConcentradoras.Find(id)
        cuentaconcentradoraid.Value = id
        txtNombre.Text = _cuentaConcentradoras.nombre
        btnSave.Text = "Actualizar"
        btnCancelar.Visible = True
    End Sub
    Private Sub ClientAutorizadoList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles cuentasList.NeedDataSource
        ClientAutorizadoList_NeedData("off")
    End Sub
    Private Sub ClientAutorizadoList_NeedData(Optional state As String = "on")
        cuentasList.DataSource = _cuentaConcentradoras.GetAll()
        If state = "on" Then
            cuentasList.DataBind()
        End If

    End Sub
    Private Sub ClientAutorizadoList_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles cuentasList.ItemCommand
        Select Case e.CommandName
            Case "cmdEditar"
                ClientAutorizadoEdit(e.CommandArgument)
            Case "cmdEliminar"
                _cuentaConcentradoras.Remove(e.CommandArgument)
                ClientAutorizadoList_NeedData()
        End Select
    End Sub

End Class