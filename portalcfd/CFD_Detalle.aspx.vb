Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Globalization
Imports FirmaSAT.Sat
Imports System.IO
Imports System.Xml.Serialization
Imports Telerik.Web.UI

Public Class portalcfd_CFD_Detalle
    Inherits System.Web.UI.Page
    Private serie As String = ""
    Private folio As Long = 0
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call CargaDetalleCobranzaCFD()
            Dim ObjData As New DataControl
            ObjData.Catalogo(tipo_pagoid, "exec pMisInformes @cmd=12", 0)
            ObjData = Nothing
        End If
    End Sub

    Private Sub CargaDetalleCobranzaCFD()

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Try


            Dim cmd As New SqlCommand("EXEC pMisInformes @cmd=13, @cfdid='" & Request("id").ToString & "'", conn)
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then
                lblCliente.Text = rs("razonsocial").ToString
                lblDocumento.Text = rs("serie").ToString & rs("folio").ToString
                lblTotalFactura.Text = FormatCurrency(rs("total"), 2).ToString
                'referencia.Text = rs("referencia")
                'If Not IsDBNull(rs("fecha_pago")) Then
                '    fechapago.SelectedDate = rs("fecha_pago")
                'End If
            End If
            rs.Close()
            '
        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
        '
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If monto.Text = "" Then
            monto.Text = 0
        End If
        If Convert.ToDecimal(monto.Text) <= 0 Then
            lblMensaje.Text = "Proporciona un monto mayor a $0.00"
        Else
            '
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
            Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
            '
            Dim ObjData As New DataControl
            If Not fechapago.SelectedDate Is Nothing Then
                ObjData.RunSQLQuery("exec pMisInformes @cmd=14, @parcialidades=1, @tipo_pagoId='" & tipo_pagoid.SelectedValue.ToString & "', @monto='" & monto.Text & "', @referencia='" & referencia.Text & "', @cfdid='" & Request("id").ToString & "', @fecha_pago='" & fechapago.SelectedDate.Value.ToShortDateString & "', @userid='" & Session("userid").ToString & "'")
            Else
                ObjData.RunSQLQuery("exec pMisInformes @cmd=14, @parcialidades=1, @tipo_pagoId='" & tipo_pagoid.SelectedValue.ToString & "', @monto='" & monto.Text & "', @referencia='" & referencia.Text & "', @cfdid='" & Request("id").ToString & "', @userid='" & Session("userid").ToString & "'")
            End If
            ObjData = Nothing
            '
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
            Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
            '
            Response.Redirect("~/portalcfd/reportes/ingresosDiv.aspx")
            '
        End If
        If System.Configuration.ConfigurationManager.AppSettings("usuarios") = 1 Then
            Call ActualizaCFDUsuario(Request("id"))
        End If
        '
    End Sub

    Private Sub ActualizaCFDUsuario(ByVal cfdid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pUsuarios @cmd=8, @userid='" & Session("userid").ToString & "', @cfdid='" & cfdid.ToString & "'")
        ObjData = Nothing
    End Sub

    Function GetList() As DataSet
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMisInformes @cmd=25, @cfdid='" & Request("id").ToString & "'", conn)
        Dim ds As DataSet = New DataSet
        Try
            conn.Open()
            cmd.Fill(ds)
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return ds
    End Function

    Private Sub cfdiListPagos_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles cfdiListPagos.ItemDataBound
        Select Case e.Item.ItemType
            Case Telerik.Web.UI.GridItemType.Footer
                If ds.Tables(0).Rows.Count > 0 Then
                    If Not IsDBNull(ds.Tables(0).Compute("sum(monto)", "")) Then
                        e.Item.Cells(5).Text = FormatCurrency(ds.Tables(0).Compute("sum(monto)", ""), 2).ToString
                        e.Item.Cells(5).HorizontalAlign = HorizontalAlign.Right
                        e.Item.Cells(5).Font.Bold = True
                    End If
                End If
            Case Telerik.Web.UI.GridItemType.Item, Telerik.Web.UI.GridItemType.AlternatingItem
                Dim lnkBorrar As LinkButton = CType(e.Item.FindControl("lnkBorrar"), LinkButton)
                lnkBorrar.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar este pago. ¿Desea continuar?');")
        End Select
    End Sub

    Private Sub cfdiListPagos_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles cfdiListPagos.NeedDataSource
        cfdiListPagos_NeedData()
    End Sub
    Private Sub cfdiListPagos_NeedData(Optional ByVal state As String = "")
        ds = GetList()
        cfdiListPagos.MasterTableView.NoMasterRecordsText = "No se encontraron registros."
        cfdiListPagos.DataSource = ds
        If state = "on" Then
            cfdiListPagos.DataBind()
        End If
    End Sub


    Private Sub cfdiListPagos_ItemCommand(sender As Object, e As GridCommandEventArgs) Handles cfdiListPagos.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Dim obj As New DataControl
                Dim cmd As String = $"EXEC pCDFPagos @cmd=1, @userid='{ Session("userid").ToString }', @pagoid='{e.CommandArgument}'"
                obj.RunSQLQuery(cmd)
        End Select
        cfdiListPagos_NeedData("on")
    End Sub
End Class
