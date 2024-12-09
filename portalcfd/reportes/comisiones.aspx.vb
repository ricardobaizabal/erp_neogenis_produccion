Imports System.Data
Imports System.Threading
Imports System.Globalization

Public Class comisiones
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.Title = Resources.Resource.WindowsTitle
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte de Comisiones"
        If Not IsPostBack Then
            fechaini.SelectedDate = DateAdd(DateInterval.Day, -7, Now)
            fechafin.SelectedDate = Now
            Dim Objdata As New DataControl
            Objdata.Catalogo(clienteid, "exec pCatalogos @cmd=2", 0, True)
            'Objdata.Catalogo(tipoid, "select id, nombre from tblTipoDocumento where id in (1,2) order by nombre", 1)
            'Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 order by nombre", 0, True)
            'Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 and perfilid=3 or perfilid=5 order by nombre", 0, True)
            'Objdata.Catalogo(sucursalid, "select id, sucursal from tblSucursalCliente where clienteId = '" & clienteid.SelectedValue & "' and isnull(borradobit,0) = 0", 0, True)
            'Objdata.Catalogo(proyectoid, "select id, nombre from tblProyecto order by id", 0)
            'If Session("perfilid") = 3 Then
            '    vendedorid.SelectedValue = Session("userid")
            '    vendedorid.Enabled = False
            'End If
            Objdata = Nothing
        End If
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Call MuestraReporte()
    End Sub

    Private Sub MuestraReporte()
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=28, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
    End Sub

    Protected Sub reporteGrid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles reporteGrid.ItemDataBound
        Select Case e.Item.ItemType

            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    e.Item.Cells(10).Text = FormatCurrency(ds.Tables(0).Compute("sum(ComisionEjecutivo)", ""), 2).ToString
                    e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(10).Font.Bold = True
                    '
                    e.Item.Cells(13).Text = FormatCurrency(ds.Tables(0).Compute("sum(ComisionCoordinador)", ""), 2).ToString
                    e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(13).Font.Bold = True
                    '
                End If
        End Select
    End Sub

End Class