Imports System.Data
Imports System.Threading
Imports System.Globalization

Public Class historicoDiv
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte histórico"
        If Not IsPostBack Then
            fechaini.SelectedDate = DateAdd(DateInterval.Day, -7, Now)
            fechafin.SelectedDate = Now
            Dim Objdata As New DataControl
            Objdata.Catalogo(clienteid, "exec pCatalogos @cmd=2", 0)
            Objdata.Catalogo(tipoid, "exec pCatalogos @cmd=5", 1)
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 and perfilid=3 or perfilid=5 order by nombre", 0, True)
            Objdata.Catalogo(sucursalid, "select id, sucursal from tblSucursalCliente where clienteId = '" & clienteid.SelectedValue & "' and isnull(borradobit,0) = 0", 0, True)
            If Session("perfilid") = 3 Then
                vendedorid.SelectedValue = Session("userid")
                vendedorid.Enabled = False
            End If
            Objdata = Nothing
            Call MuestraReporte()
        End If
    End Sub

    Private Sub MuestraReporte()
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=20, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @criterio='" & txtCriterio.Text & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        ''
    End Sub

    Private Sub MuestraSucursal()
        Dim Objdata As New DataControl
        Objdata.Catalogo(sucursalid, "select id,sucursal from tblSucursalCliente where clienteId = '" & clienteid.SelectedValue & "' and isnull(borradobit,0) = 0", 0, True)
        Objdata = Nothing
    End Sub

    Private Sub FiltraVendedor()
        Dim Objdata As New DataControl
        If Session("perfilid") = 2 Then
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where u.perfilid=3 and u.id='" & Session("userid") & "' and s.id= '" & sucursalid.SelectedValue & "' order by nombre", 0, True)
        Else
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where u.perfilid=3 and s.id='" & sucursalid.SelectedValue & "' order by nombre", 0, True)
        End If

        Objdata = Nothing
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Call MuestraReporte()
    End Sub

    Protected Sub reporteGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles reporteGrid.NeedDataSource
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=20, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @criterio='" & txtCriterio.Text & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        ''
    End Sub

    Private Sub clienteid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles clienteid.SelectedIndexChanged
        Call MuestraSucursal()
        Call MuestraReporte()
    End Sub

    Private Sub sucursalid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles sucursalid.SelectedIndexChanged

        If sucursalid.SelectedValue = 0 Then
            Dim Objdata As New DataControl
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 order by nombre", 0, True)
            Objdata = Nothing
        Else
            Call FiltraVendedor()
        End If

        Call MuestraReporte()
    End Sub

    Private Sub tipoid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tipoid.SelectedIndexChanged
        Call MuestraReporte()
    End Sub

End Class