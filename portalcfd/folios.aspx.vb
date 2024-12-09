Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Globalization
Partial Class portalcfd_folios
    Inherits System.Web.UI.Page

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pMisFolios @cmd=1, @serie='" & txtSerie.Text & "', @folioInicial='" & txtFolioInicial.Text & "', @folioFinal='" & txtFolioFinal.Text & "', @aprobacion='" & txtAprobacion.Text & "', @annio_solicitud='" & txtAnnioSolicitud.Text & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @fecha_emision='" & fecha_emision.SelectedDate.Value.ToShortDateString & "'", conn)
        Try

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                If rs("error") = 1 Then
                    lblMensaje.Text = "La información de sus folios no es correcta, revise el consecutivo."
                Else
                    lblMensaje.Text = "Sus folios han sido agregados correctamente."
                End If


            End If

        Catch ex As Exception
            lblMensaje.Text = ex.ToString
        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
        '
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
        Call MuestraPaquetes()
        '
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaCatalogos()
            Call MuestraPaquetes()
        End If
        '
        If Session("admin") <> 1 Then
            btnSave.Enabled = False
        End If
        '
    End Sub
    Private Sub CargaCatalogos()
        Dim ObjCat As New DataControl
        ObjCat.Catalogo(tipoid, "select id, nombre from tblTipoDocumento order by nombre", 0)
        ObjCat = Nothing
    End Sub
    Private Sub MuestraPaquetes()
        Dim ObjData As New DataControl
        FoliosGrid.DataSource = ObjData.FillDataSet("exec pMisFolios @cmd=2")
        FoliosGrid.DataBind()
        ObjData = Nothing
    End Sub

   
    Protected Sub FoliosGrid_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles FoliosGrid.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Dim commandArgs() As String = e.CommandArgument.Split(",")
                'lblMensaje.Text = "exec pMisFolios @cmd=3, @tipoid='" & commandArgs(2).ToString & "', @aprobacion='" & commandArgs(0).ToString & "', @annio_solicitud='" & commandArgs(1).ToString & "'"
                Dim ObjData As New DataControl
                ObjData.RunSQLQuery("exec pMisFolios @cmd=3, @tipoid='" & commandArgs(2).ToString & "', @aprobacion='" & commandArgs(0).ToString & "', @annio_solicitud='" & commandArgs(1).ToString & "'")
                ObjData = Nothing
                Call MuestraPaquetes()
        End Select
    End Sub

    Protected Sub FoliosGrid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles FoliosGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim btnDelete As System.Web.UI.WebControls.ImageButton = CType(e.Item.FindControl("btnDelete"), System.Web.UI.WebControls.ImageButton)
                If e.Item.DataItem("foliosUtilizados") > 0 Then
                    btnDelete.Visible = False
                Else
                    btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a borrar un lote de folios, ¿Desea continuar?');")
                End If
        End Select
    End Sub

    Protected Sub FoliosGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles FoliosGrid.NeedDataSource
        Dim ObjData As New DataControl
        FoliosGrid.DataSource = ObjData.FillDataSet("exec pMisFolios @cmd=2")
        ObjData = Nothing
    End Sub
End Class
