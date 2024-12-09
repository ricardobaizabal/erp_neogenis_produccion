Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI
Imports System.Globalization
Imports System.Threading
Imports System.IO
Imports System.Linq
Public Class agregarorden
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaProveedores()
        End If
    End Sub

    Private Sub CargaProveedores()
        Dim ObjData As New DataControl
        ObjData.Catalogo(proveedorid, "select id, razonsocial as nombre from tblMisProveedores order by razonsocial", 0)
        ObjData.Catalogo(proyectoid, "select id, nombre from tblProyecto order by nombre", 0)
        ObjData = Nothing
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        Response.Redirect("~/portalcfd/proveedores/ordenes_compra.aspx")
    End Sub

    Private Sub btnAddorder_Click(sender As Object, e As System.EventArgs) Handles btnAddorder.Click

        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '

        Dim ordenId As Long = 0
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("exec pOrdenCompra @cmd=2, @proveedorId='" & proveedorid.SelectedValue & "', 
                                                                    @userid='" & Session("userid").ToString & "', 
                                                                    @comentarios='" & txtComentarios.Text & "',
                                                                    @fechaEstEnt='" & fechaEstEnt.SelectedDate.Value.ToShortDateString & "', 
                                                                    @marcaid='" & proyectoid.SelectedValue.ToString & "', 
                                                                    @claveEdit='" & txtclaveEdit.Text & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                ordenId = rs("ordenId")
            End If

        Catch ex As Exception

        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing

        End Try
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Response.Redirect("~/portalcfd/proveedores/editarorden.aspx?id=" & ordenId.ToString)
    End Sub

End Class