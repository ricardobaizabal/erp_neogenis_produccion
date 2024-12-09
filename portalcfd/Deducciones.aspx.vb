Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Deducciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call muestraDeducciones()
            Dim ObjData As New DataControl
            ObjData.Catalogo(ddlTipoDeduccion, "select id,descripcion from tblTipoDeduccion order by id", 0)
            ObjData = Nothing
        End If
    End Sub

    Private Sub muestraDeducciones()
        Dim ds As New DataSet()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("EXEC pDeducciones @cmd=1")
        grdDeducciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdDeducciones.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        txtClave.Text = ""
        txtDescripcion.Text = ""
        panelRegistroDeduccion.Visible = False
    End Sub

    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGuardar.Click

        Dim exento As Integer

        If exentoBit.Checked Then
            exento = 1
        Else
            exento = 0
        End If

        Dim ObjData As New DataControl
        If InsertOrUpdate.Value = 0 Then
            ObjData.RunSQLQuery("EXEC pDeducciones @cmd=4, @tipodeduccionid='" & ddlTipoDeduccion.SelectedValue & "', @clave='" & txtClave.Text.ToString.ToUpper & "', @descripcion='" & txtDescripcion.Text.ToString & "', @exentoBit='" & exento.ToString & "'")
        Else
            ObjData.RunSQLQuery("EXEC pDeducciones @cmd=5, @tipodeduccionid='" & ddlTipoDeduccion.SelectedValue & "', @clave='" & txtClave.Text.ToString.ToUpper & "', @descripcion='" & txtDescripcion.Text.ToString & "', @deduccionid='" & deduccionID.Value.ToString & "', @exentoBit='" & exento.ToString & "'")
        End If

        ObjData = Nothing
        Response.Redirect("Deducciones.aspx")
    End Sub

    Private Sub EditaDeduccion(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pDeducciones @cmd=2, @deduccionid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            Dim exento As String = "False"

            If rs.Read Then
                '
                ddlTipoDeduccion.SelectedValue = rs("tipodeduccionid")
                txtClave.Text = rs("clave")
                txtDescripcion.Text = rs("descripcion")
                exento = rs("exentoBit")
                '
                panelRegistroDeduccion.Visible = True
                '
                InsertOrUpdate.Value = 1
                deduccionID.Value = rs("id")

                If exento = "True" Then
                    exentoBit.Checked = True
                Else
                    exentoBit.Checked = False
                End If


            End If

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub EliminaDeduccion(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pDeducciones @cmd=3, @deduccionid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelRegistroDeduccion.Visible = False

            Response.Redirect("Deducciones.aspx")

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Private Sub grdDeducciones_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdDeducciones.ItemCommand
        Select Case e.CommandName

            Case "cmdEdit"
                EditaDeduccion(e.CommandArgument)

            Case "cmdDelete"
                EliminaDeduccion(e.CommandArgument)

        End Select
    End Sub

    Private Sub grdDeducciones_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdDeducciones.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Deducciones" Then

                Dim exentoBit As CheckBox = CType(dataItem("Exento").FindControl("exentoBit"), CheckBox)
                Dim lblExento As Label = CType(dataItem("Exento").FindControl("lblExento"), Label)
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('¿Está seguro que desea borrar esta deducción de la base de datos?');")

                If lblExento.Text.ToString = "True" Then
                    exentoBit.Checked = True
                End If

            End If

        End If
    End Sub

    Private Sub btnAgregaDeduccion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregaDeduccion.Click
        InsertOrUpdate.Value = 0
        txtClave.Text = ""
        txtDescripcion.Text = ""
        panelRegistroDeduccion.Visible = True
    End Sub

    Protected Sub grdDeducciones_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdDeducciones.NeedDataSource
        Call muestraDeducciones()
    End Sub

End Class