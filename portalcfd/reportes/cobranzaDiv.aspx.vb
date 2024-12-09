Imports System.Data
Imports System.Threading
Imports System.Globalization

Public Class cobranzaDiv
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Title = Resources.Resource.WindowsTitle
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte de cobranza"
        If Not IsPostBack Then
            fechaini.SelectedDate = DateAdd(DateInterval.Day, -7, Now)
            fechafin.SelectedDate = Now
            Dim Objdata As New DataControl
            Objdata.Catalogo(cmbCuentaConcentradora, "select id, nombre  from tblCuentasConcentradoras where isnull(borradorbit,0)=0 order by nombre", 0, True)
            Objdata.Catalogo(clienteid, "exec pCatalogos @cmd=2", 0, True)
            Objdata.Catalogo(tipoid, "exec pCatalogos @cmd=5", 1)
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 order by nombre", 0, True)
            'Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 and perfilid=3 or perfilid=5 order by nombre", 0, True)
            Objdata.Catalogo(sucursalid, "select id, sucursal from tblSucursalCliente where clienteId = '" & clienteid.SelectedValue & "' and isnull(borradobit,0) = 0", 0, True)
            Objdata.Catalogo(proyectoid, "select id, nombre from tblProyecto order by id", 0)
            If Session("perfilid") = 3 Then
                vendedorid.SelectedValue = Session("userid")
                vendedorid.Enabled = False
            End If
            Objdata = Nothing
        End If
    End Sub

    Private Sub MuestraReporte()
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=19, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds
        reporteGrid.DataBind()
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
    End Sub

    Private Sub MuestraSucursal()
        Dim Objdata As New DataControl
        Objdata.Catalogo(sucursalid, "select id,sucursal from tblSucursalCliente where clienteId = '" & clienteid.SelectedValue & "' and isnull(borradobit,0) = 0", 0, True)
        Objdata = Nothing
    End Sub

    Private Sub FiltraVendedor()
        Dim Objdata As New DataControl
        If Session("perfilid") = 3 Then
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where isnull(u.borradoBit,0)=0 and u.id='" & Session("userid") & "' and s.id= '" & sucursalid.SelectedValue & "' order by nombre", 0, True)
        Else
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where s.id='" & sucursalid.SelectedValue & "' and isnull(u.borradobit,0)=0 order by nombre", 0, True)
        End If

        Objdata = Nothing
    End Sub

    Private Sub FiltraVendedorCliente()
        Dim Objdata As New DataControl
        If clienteid.SelectedValue = 0 Then
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 order by nombre", 0, True)
        Else
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id inner join tblMisClientes c on c.id=s.clienteId where u.perfilid=3 and c.id = '" & clienteid.SelectedValue.ToString & "' and isnull(u.borradobit,0)=0 order by nombre", 0, True)
        End If
        Objdata = Nothing
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Call MuestraReporte()
    End Sub

    Protected Sub reporteGrid_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles reporteGrid.ItemCommand
        Select Case e.CommandName
            Case "cmdFolio"
                Response.Redirect("~/portalcfd/CFD_Detalle.aspx?id=" & e.CommandArgument.ToString)
        End Select
    End Sub

    Protected Sub reporteGrid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles reporteGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim lnkFolio As LinkButton = CType(e.Item.FindControl("lnkFolio"), LinkButton)
                If Session("perfilid") = 3 Then
                    lnkFolio.Enabled = False
                End If
                If e.Item.DataItem("estatus_cobranza") = "Pendiente" Then
                    e.Item.Cells(13).ForeColor = Drawing.Color.DarkRed
                Else
                    e.Item.Cells(13).ForeColor = Drawing.Color.Green
                End If
                e.Item.Cells(13).Font.Bold = True
            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    e.Item.Cells(10).Text = FormatCurrency(ds.Tables(0).Compute("sum(importe)", ""), 2).ToString
                    e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(10).Font.Bold = True
                    '
                    e.Item.Cells(11).Text = FormatCurrency(ds.Tables(0).Compute("sum(descuento)", ""), 2).ToString
                    e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(11).Font.Bold = True
                    '
                    e.Item.Cells(12).Text = FormatCurrency(ds.Tables(0).Compute("sum(iva)", ""), 2).ToString
                    e.Item.Cells(12).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(12).Font.Bold = True
                    '
                    e.Item.Cells(13).Text = FormatCurrency(ds.Tables(0).Compute("sum(total)", ""), 2).ToString
                    e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
                    e.Item.Cells(13).Font.Bold = True
                End If
        End Select
    End Sub

    Protected Sub reporteGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles reporteGrid.NeedDataSource
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=19, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        ''
    End Sub

    Private Sub sucursalid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles sucursalid.SelectedIndexChanged
        If sucursalid.SelectedValue = 0 Then
            Call FiltraVendedorCliente()
        Else
            Call FiltraVendedor()
        End If
        If clienteid.SelectedValue = 0 And sucursalid.SelectedValue = 0 Then
            Dim Objdata As New DataControl
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 order by nombre", 0, True)
            Objdata = Nothing
        End If
        If Session("perfilid") = 3 Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 and id='" & Session("userid") & "' order by nombre", Session("userid"), True)
            vendedorid.Enabled = False
        End If
    End Sub

    Private Sub clienteid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles clienteid.SelectedIndexChanged
        Call FiltraVendedorCliente()
        If Session("perfilid") = 3 Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 and id='" & Session("userid") & "' order by nombre", Session("userid"), True)
            vendedorid.Enabled = False
        End If
        Call MuestraSucursal()
    End Sub
End Class