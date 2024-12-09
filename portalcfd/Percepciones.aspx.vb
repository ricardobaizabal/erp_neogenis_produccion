Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Public Class Percepciones
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call muestraPercepciones()
            Dim ObjData As New DataControl
            ObjData.Catalogo(ddlTipoPercepcion, "select id,descripcion from tblTipoPercepcion order by id", 0)
            ObjData = Nothing
        End If
    End Sub

    Private Sub muestraPercepciones()
        Dim ds As New DataSet()
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("EXEC pPercepciones @cmd=1")
        grdPercepciones.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        grdPercepciones.DataSource = ds
        ObjData = Nothing
    End Sub

    Private Sub grdPercepciones_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles grdPercepciones.ItemCommand
        Select Case e.CommandName

            Case "cmdEdit"
                EditaPercepcion(e.CommandArgument)

            Case "cmdDelete"
                EliminaPercepcion(e.CommandArgument)

        End Select
    End Sub

    Private Sub grdPercepciones_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles grdPercepciones.ItemDataBound
        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Percepciones" Then

                Dim exentoBit As CheckBox = CType(dataItem("Exento").FindControl("exentoBit"), CheckBox)
                Dim lblExento As Label = CType(dataItem("Exento").FindControl("lblExento"), Label)
                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('¿Está seguro que desea borrar esta percepción de la base de datos?');")

                If lblExento.Text.ToString = "True" Then
                    exentoBit.Checked = True
                End If

            End If

        End If
    End Sub

    Private Sub btnAgregaPercepcion_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAgregaPercepcion.Click
        InsertOrUpdate.Value = 0
        txtClave.Text = ""
        txtDescripcion.Text = ""
        panelRegistroPercepcion.Visible = True
    End Sub

    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        InsertOrUpdate.Value = 0
        txtClave.Text = ""
        txtDescripcion.Text = ""
        panelRegistroPercepcion.Visible = False
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
            ObjData.RunSQLQuery("EXEC pPercepciones @cmd=4, @tipopercepcionid='" & ddlTipoPercepcion.SelectedValue & "', @clave='" & txtClave.Text.ToString.ToUpper & "', @descripcion='" & txtDescripcion.Text.ToString & "', @exentoBit='" & exento.ToString & "'")
        Else
            ObjData.RunSQLQuery("EXEC pPercepciones @cmd=5, @tipopercepcionid='" & ddlTipoPercepcion.SelectedValue & "', @clave='" & txtClave.Text.ToString.ToUpper & "', @descripcion='" & txtDescripcion.Text.ToString & "', @percepcionid='" & percepcionID.Value.ToString & "', @exentoBit='" & exento.ToString & "'")
        End If

        ObjData = Nothing
        Response.Redirect("Percepciones.aspx")
    End Sub

    Private Sub EditaPercepcion(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pPercepciones @cmd=2, @percepcionid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            Dim exento As String = "False"

            If rs.Read Then
                '
                ddlTipoPercepcion.SelectedValue = rs("tipopercepcionid")
                txtClave.Text = rs("clave")
                txtDescripcion.Text = rs("descripcion")
                exento = rs("exentoBit")
                '
                panelRegistroPercepcion.Visible = True
                '
                InsertOrUpdate.Value = 1
                percepcionID.Value = rs("id")


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

    Private Sub EliminaPercepcion(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pPercepciones @cmd=3, @percepcionid='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelRegistroPercepcion.Visible = False

            Response.Redirect("Percepciones.aspx")

        Catch ex As Exception
            Response.Write(ex.Message.ToString)
        Finally
            conn.Close()
            conn.Dispose()
        End Try

    End Sub

    Protected Sub grdPercepciones_NeedDataSource(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles grdPercepciones.NeedDataSource
        Call muestraPercepciones()
    End Sub

End Class