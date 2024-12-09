Imports System.Data
Imports System.Threading
Imports System.Globalization
Imports Telerik.Web.UI
Imports Microsoft.Office

Public Class ingresosDiv
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '
        Me.Title = Resources.Resource.WindowsTitle
        '
        chkAll.Attributes.Add("onclick", "checkedAll(" & Me.Form.ClientID.ToString & ");")
        '
        lblReportsLegend.Text = Resources.Resource.lblReportsLegend & " - Reporte de facturación"
        If Not IsPostBack Then
            fechaini.SelectedDate = DateAdd(DateInterval.Day, -7, Now)
            fechafin.SelectedDate = Now
            Dim Objdata As New DataControl
            Objdata.Catalogo(cmbCuentaConcentradora, "select id, nombre  from tblCuentasConcentradoras where isnull(borradorbit,0)=0 order by nombre", 0, True)
            Objdata.Catalogo(clienteid, "exec pCatalogos @cmd=2", 0)
            Objdata.Catalogo(tipoid, "exec pCatalogos @cmd=5", 1)
            Objdata.Catalogo(estatus_cobranzaid, "exec pMisInformes @cmd=11", 0)
            Objdata.Catalogo(tipo_pagoid, "exec pMisInformes @cmd=12", 0)
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where isnull(borradoBit,0)=0 and perfilid=3 or perfilid=5 order by nombre", 0, True)
            Objdata.Catalogo(sucursalid, "select id, sucursal from tblSucursalCliente where clienteId = '" & clienteid.SelectedValue & "' and isnull(borradobit,0) = 0", 0, True)
            Objdata.Catalogo(proyectoid, "select id, nombre from tblProyecto order by id", 0)
            Objdata.Catalogo(estatuscobranza, "select id, nombre from tblCFD_EstatusCobranza", 0)
            If Session("perfilid") = 3 Then
                vendedorid.SelectedValue = Session("userid")
                vendedorid.Enabled = False
                btnPayAll.Visible = False
                panelCobrar.Visible = False
                chkAll.Visible = False
                reporteGrid.MasterTableView.GetColumn("chkcfdid").Display = False
            End If
            Objdata = Nothing
        End If
    End Sub

    Private Sub MuestraReporte()
        '
        'reporteGrid.Rebind()
        reporteGrid.CurrentPageIndex = 0
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=18, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue.ToString & "', @estatus_cobranzaId='" & estatuscobranza.SelectedValue & "'")
        reporteGrid.DataSource = ds.Tables(0).DefaultView
        reporteGrid.DataBind()
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
    End Sub

    Private Sub MuestraSucursal()
        Dim Objdata As New DataControl
        Objdata.Catalogo(sucursalid, "select id,sucursal from tblSucursalCliente where clienteId = '" & clienteid.SelectedValue & "' and isnull(borradobit,0)=0 order by sucursal", 0, True)
        Objdata = Nothing
    End Sub

    Private Sub FiltraVendedor()
        Dim Objdata As New DataControl
        If Session("perfilid") = 3 Then
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where u.perfilid=3 and u.id='" & Session("userid") & "' and s.id= '" & sucursalid.SelectedValue & "' order by nombre", 0, True)
        Else
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id where u.perfilid=3 and s.id='" & sucursalid.SelectedValue & "' and isnull(u.borradobit,0)=0 order by nombre", 0, True)
        End If
        Objdata = Nothing
    End Sub

    Private Sub FiltraVendedorCliente()
        Dim Objdata As New DataControl
        If clienteid.SelectedValue = 0 Then
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 order by nombre", 0, True)
        Else
            Objdata.Catalogo(vendedorid, "select distinct u.id, u.nombre from tblUsuario u inner join tblSucursalCliente s on s.vendedorid=u.id inner join tblMisClientes c on c.id=s.clienteId where u.perfilid=3 and c.id = '" & clienteid.SelectedValue.ToString & "' and isnull(u.borradobit,0)=0 order by nombre", 0, True)
        End If
        Objdata = Nothing
    End Sub

    Protected Sub btnGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Call MuestraReporte()
    End Sub

    Protected Sub reporteGrid_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles reporteGrid.ItemCommand
        If e.CommandName = RadGrid.ExportToExcelCommandName Then
            reporteGrid.MasterTableView.GetColumn("chkcfdid").Visible = False
        End If
        Select Case e.CommandName
            Case "cmdFolio"
                Response.Redirect("~/portalcfd/CFD_Detalle.aspx?id=" & e.CommandArgument.ToString)
        End Select
    End Sub

    Protected Sub reporteGrid_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles reporteGrid.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim chkcfdid As CheckBox = CType(e.Item.FindControl("chkcfdid"), CheckBox)
                Dim lnkFolio As LinkButton = CType(e.Item.FindControl("lnkFolio"), LinkButton)

                If Session("perfilid") = 3 Then
                    lnkFolio.Enabled = False
                End If

                If e.Item.DataItem("estatus_cobranzaid") = 2 Or e.Item.DataItem("escancelada") = True Or e.Item.DataItem("pagado") > 0 Then
                    chkcfdid.Visible = False
                End If

                If e.Item.DataItem("estatus_cobranza") = "Pendiente" Then
                    e.Item.Cells(16).ForeColor = Drawing.Color.Blue
                Else
                    e.Item.Cells(16).ForeColor = Drawing.Color.Green
                End If

                If (e.Item.DataItem("estatus") = 3) Or (e.Item.DataItem("escancelada")) Then
                    e.Item.Cells(16).Text = "Cancelada"
                    e.Item.Cells(16).ForeColor = Drawing.Color.Red
                    e.Item.Cells(16).Font.Bold = True
                    lnkFolio.Enabled = False
                End If

                e.Item.Cells(15).Font.Bold = True
                e.Item.Cells(16).Font.Bold = True

            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(ds.Tables(0).Compute("sum(importe)", "estatus<>3")) Then
                        e.Item.Cells(9).Text = FormatCurrency(ds.Tables(0).Compute("sum(importe)", "estatus<>3"), 2).ToString
                        e.Item.Cells(9).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(9).Font.Bold = True
                    End If
                    '
                    If Not IsDBNull(ds.Tables(0).Compute("sum(descuento)", "estatus<>3")) Then
                        e.Item.Cells(10).Text = FormatCurrency(ds.Tables(0).Compute("sum(descuento)", "estatus<>3"), 2).ToString
                        e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(10).Font.Bold = True
                    End If
                    '
                    If Not IsDBNull(ds.Tables(0).Compute("sum(iva)", "estatus<>3")) Then
                        e.Item.Cells(11).Text = FormatCurrency(ds.Tables(0).Compute("sum(iva)", "estatus<>3"), 2).ToString
                        e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(11).Font.Bold = True
                    End If
                    '
                    If Not IsDBNull(ds.Tables(0).Compute("sum(total)", "estatus<>3")) Then
                        e.Item.Cells(12).Text = FormatCurrency(ds.Tables(0).Compute("sum(total)", "estatus<>3"), 2).ToString
                        e.Item.Cells(12).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(12).Font.Bold = True
                    End If
                    '
                    If Not IsDBNull(ds.Tables(0).Compute("sum(pagado)", "estatus<>3")) Then
                        e.Item.Cells(13).Text = FormatCurrency(ds.Tables(0).Compute("sum(pagado)", "estatus<>3"), 2).ToString
                        e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(13).Font.Bold = True
                    End If
                    '
                    If Not IsDBNull(ds.Tables(0).Compute("sum(pagos)", "estatus<>3")) Then
                        e.Item.Cells(14).Text = FormatCurrency(ds.Tables(0).Compute("sum(pagos)", "estatus<>3"), 2).ToString
                        e.Item.Cells(14).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(14).Font.Bold = True
                    End If
                    '
                    If Not IsDBNull(ds.Tables(0).Compute("sum(saldo)", "estatus<>3")) Then
                        e.Item.Cells(15).Text = FormatCurrency(ds.Tables(0).Compute("sum(saldo)", "estatus<>3"), 2).ToString
                        e.Item.Cells(15).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(15).Font.Bold = True
                    End If
                    '
                End If
        End Select
    End Sub

    Protected Sub reporteGrid_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles reporteGrid.NeedDataSource
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=18, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds.Tables(0)
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
    End Sub

    Protected Sub btnPayAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPayAll.Click
        Dim elementos As Integer = 0
        For Each row As GridDataItem In reporteGrid.Items
            Dim chkItem As CheckBox = CType(row.FindControl("chkcfdid"), CheckBox)
            If chkItem.Checked = True Then
                '
                elementos += 1
                '
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
                Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
                '
                Dim ObjData As New DataControl
                If Not fechapago.SelectedDate Is Nothing Then
                    ObjData.RunSQLQuery("exec pMisInformes @cmd=14, @estatus_cobranzaId='" & estatus_cobranzaid.SelectedValue.ToString & "', @tipo_pagoId='" & tipo_pagoid.SelectedValue.ToString & "', @referencia='" & referencia.Text & "', @cfdid='" & row.GetDataKeyValue("id").ToString & "', @fecha_pago='" & fechapago.SelectedDate.Value.ToShortDateString & "'")
                Else
                    ObjData.RunSQLQuery("exec pMisInformes @cmd=14, @estatus_cobranzaId='" & estatus_cobranzaid.SelectedValue.ToString & "', @tipo_pagoId='" & tipo_pagoid.SelectedValue.ToString & "', @referencia='" & referencia.Text & "', @cfdid='" & row.GetDataKeyValue("id").ToString & "'")
                End If
                ObjData = Nothing
                '
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
                Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
                '
                If System.Configuration.ConfigurationManager.AppSettings("usuarios") = 1 Then
                    Call ActualizaCFDUsuario(row.GetDataKeyValue("id"))
                End If
                '
            End If
        Next
        '
        If elementos > 0 Then
            Call ActualizaReporte()
            Call LimpiaControles()
            lblMensajeActualiza.Text = "Los documentos seleccionados han sido actualizados"
        Else
            lblMensajeActualiza.Text = "No se ha seleccionado ningún documento para actualización"
        End If
        '
    End Sub

    Private Sub ActualizaCFDUsuario(ByVal cfdid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pUsuarios @cmd=8, @userid='" & Session("userid").ToString & "', @cfdid='" & cfdid.ToString & "'")
        ObjData = Nothing
    End Sub

    Protected Sub btnGenerate_Click1(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerate.Click
        Call ActualizaReporte()
    End Sub

    Private Sub ActualizaReporte()
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
        '
        Dim ObjData As New DataControl
        ds = ObjData.FillDataSet("exec pMisInformes @cmd=18, @fechaini='" & fechaini.SelectedDate.Value.ToShortDateString & "', @fechafin='" & fechafin.SelectedDate.Value.ToShortDateString & "', @clienteid='" & clienteid.SelectedValue.ToString & "', @cuenta_concentradoraid='" & cmbCuentaConcentradora.SelectedValue.ToString & "', @tipoid='" & tipoid.SelectedValue.ToString & "', @vendedorid='" & vendedorid.SelectedValue.ToString & "', @sucursalid='" & sucursalid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue.ToString & "'")
        reporteGrid.DataSource = ds.Tables(0)
        reporteGrid.DataBind()
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
        '
    End Sub

    Private Sub LimpiaControles()
        estatus_cobranzaid.SelectedIndex = 0
        tipo_pagoid.SelectedIndex = 0
        referencia.Text = ""
        fechapago.SelectedDate = Nothing
    End Sub

    Private Sub clienteid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles clienteid.SelectedIndexChanged
        Call FiltraVendedorCliente()
        If Session("perfilid") = 3 Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 and id='" & Session("userid") & "' order by nombre", Session("userid"), True)
            vendedorid.Enabled = False
        End If
        Call MuestraSucursal()
    End Sub

    Private Sub sucursalid_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles sucursalid.SelectedIndexChanged
        If sucursalid.SelectedValue = 0 Then
            Call FiltraVendedorCliente()
        Else
            Call FiltraVendedor()
        End If
        If clienteid.SelectedValue = 0 And sucursalid.SelectedValue = 0 Then
            Dim Objdata As New DataControl
            Objdata.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 order by nombre", 0, True)
            Objdata = Nothing
        End If
        If Session("perfilid") = 3 Then
            Dim ObjCat As New DataControl
            ObjCat.Catalogo(vendedorid, "select id, nombre from tblUsuario where perfilid=3 and id='" & Session("userid") & "' order by nombre", Session("userid"), True)
            vendedorid.Enabled = False
        End If
    End Sub

End Class