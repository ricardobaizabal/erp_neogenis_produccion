Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Globalization
Imports Telerik.Web.UI
Imports System.IO
Imports System.Linq


Partial Class portalcfd_Productos
    Inherits System.Web.UI.Page
    Private ds As DataSet
    Public firstOpen As Boolean

#Region "Load Initial Values"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ''''''''''''''
        'Window Title'
        ''''''''''''''

        Me.Title = Resources.Resource.WindowsTitle

        If Not IsPostBack Then

            '''''''''''''''''''
            'Fieldsets Legends'
            '''''''''''''''''''

            lblProductsListLegend.Text = Resources.Resource.lblProductsListLegend
            lblProductEditLegend.Text = Resources.Resource.lblProductEditLegend

            ''''''''''''''
            'Label Titles'
            ''''''''''''''

            lblCode.Text = Resources.Resource.lblCode
            lblUnit.Text = Resources.Resource.lblUnit
            lblUnitaryPrice.Text = "Precio Unit. 1"
            lblUnitaryPrice2.Text = "Precio Unit. 2"
            lblUnitaryPrice3.Text = "Precio Unit. 3"
            lblUnitaryPrice4.Text = "Precio Unit. 4"

            lblDescription.Text = Resources.Resource.lblDescription

            '''''''''''''''''''
            'Validators Titles'
            '''''''''''''''''''

            RequiredFieldValidator1.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator2.Text = Resources.Resource.validatorMessage
            RequiredFieldValidator3.Text = Resources.Resource.validatorMessage

            ''''''''''''''''
            'Buttons Titles'
            ''''''''''''''''

            btnAddProduct.Text = Resources.Resource.btnAddProduct
            btnSaveProduct.Text = Resources.Resource.btnSave
            btnCancel.Text = Resources.Resource.btnCancel
            '
            Dim objCat As New DataControl
            objCat.Catalogo(tasaid, "select id, nombre from tblTasa order by id", 0)
            objCat.Catalogo(monedaid, "select id, nombre from tblMoneda order by id", 0)
            objCat.Catalogo(proveedorid, "select id, razonsocial from tblMisProveedores order by razonsocial", 0)
            objCat.Catalogo(coleccionid, "select id, isnull(codigo,'') + ' - ' + isnull(nombre,'') as nombre from tblColeccion where isnull(borradoBit,0)=0 order by nombre", 0)
            objCat.Catalogo(filtrocoleccionid, "select id, isnull(codigo,'') + ' - ' + isnull(nombre,'') as nombre from tblColeccion where isnull(borradoBit,0)=0 order by nombre", 0)
            objCat.Catalogo(filtromarcaid, "select id, nombre from tblProyecto order by nombre", 0)
            objCat.Catalogo(proyectoid, "select id, nombre from tblProyecto order by nombre", 0)
            objCat.Catalogo(cboclaveunidad, "select clave, clave + ' - ' + isnull(nombre,'') as nombre from tblUnidad order by nombre", 0)
            objCat.Catalogo(cboproductoserv, "select clave, clave + ' - ' + isnull(nombre,'') as nombre from tblClaveProducto order by nombre", 0)
            objCat.Catalogo(cmbgeneroid, "select id, isnull(descripcion,'') as descripcion from tblGenero", 0)
            objCat.Catalogo(cbmObjetoImpuesto, "select id, descripcion from tblObjetoImp", 0)
            'objCat.Catalogo(estatusproductoid, "select id, nombre from tblEstatusProducto order by id desc", 0)
            objCat = Nothing
            '
            Dim obj As New DataControl
            ckMarketplaces.DataSource = obj.FillDataSet("EXEC pMarketPlace @cmd=5")
            ckMarketplaces.DataBind()
            '
            Call MuestraCodigosStockLocart()
            '
            chkInventariableBit.Checked = True
            chkPerecederoBit.Checked = True
            '
            If Session("perfilid") = 3 Then
                btnAddProduct.Visible = False
                btnSaveProduct.Enabled = False
            End If
            '
        End If

    End Sub
#End Region

#Region "Load List Of Products"

    Function GetProducts() As DataSet
        ds = New DataSet
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMisProductos @cmd=1, @clienteid='" & Session("clienteid") & "', @txtSearch='" & txtSearch.Text & "', @proyectoid='" & filtromarcaid.SelectedValue.ToString & "', @coleccionid='" & filtrocoleccionid.SelectedValue.ToString & "', @upcSearch='" & upcSearch.Text & "', @conExistencia='" & ckfiltroExistencia.Checked & "'", conn)
        cmd.SelectCommand.CommandTimeout = 180000
        Try
            conn.Open()
            cmd.Fill(ds)
            conn.Close()
            conn.Dispose()
        Catch ex As Exception
            Throw New Exception("Error: " & ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Return ds
    End Function

#End Region

#Region "Telerik Grid Products Loading Events"

    Protected Sub productslist_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productslist.NeedDataSource

        If Not e.IsFromDetailTable Then
            productslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ProductsEmptyGridMessage

            If IsPostBack Then
                ds = GetProducts()
                productslist.DataSource = ds
            Else
                Dim dt As New DataTable
                productslist.DataSource = dt
            End If

        End If

    End Sub

#End Region

#Region "Telerik Grid Language Modification(Spanish)"

    Protected Sub productslist_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles productslist.Init

        productslist.PagerStyle.NextPagesToolTip = "Ver mas"
        productslist.PagerStyle.NextPageToolTip = "Siguiente"
        productslist.PagerStyle.PrevPagesToolTip = "Ver más"
        productslist.PagerStyle.PrevPageToolTip = "Atrás"
        productslist.PagerStyle.LastPageToolTip = "Última Página"
        productslist.PagerStyle.FirstPageToolTip = "Primera Página"
        productslist.PagerStyle.PagerTextFormat = "{4}    Página {0} de {1}, Registros {2} al {3} de {5}"
        productslist.SortingSettings.SortToolTip = "Ordernar"
        productslist.SortingSettings.SortedAscToolTip = "Ordenar Asc"
        productslist.SortingSettings.SortedDescToolTip = "Ordenar Desc"


        Dim menu As Telerik.Web.UI.GridFilterMenu = productslist.FilterMenu
        Dim i As Integer = 0

        While i < menu.Items.Count

            If menu.Items(i).Text = "NoFilter" Or menu.Items(i).Text = "Contains" Then
                i = i + 1
            Else
                menu.Items.RemoveAt(i)
            End If

        End While

        Call ModificaIdiomaGrid()

    End Sub

    Private Sub ModificaIdiomaGrid()

        productslist.GroupingSettings.CaseSensitive = False

        Dim Menu As Telerik.Web.UI.GridFilterMenu = productslist.FilterMenu
        Dim item As Telerik.Web.UI.RadMenuItem

        For Each item In Menu.Items

            ''''''''''''''''''''''''''''''''''''''''''''''
            'Change The Text For The StartsWith Menu Item'
            ''''''''''''''''''''''''''''''''''''''''''''''

            If item.Text = "StartsWith" Then
                item.Text = "Empieza con"
            End If

            If item.Text = "NoFilter" Then
                item.Text = "Sin Filtro"
            End If

            If item.Text = "Contains" Then
                item.Text = "Contiene"
            End If

            If item.Text = "EndsWith" Then
                item.Text = "Termina con"
            End If

        Next

    End Sub

#End Region

#Region "Telerik Grid Products Editing & Deleting Events"

    Protected Sub productslist_ItemDataBound(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles productslist.ItemDataBound

        If TypeOf e.Item Is GridDataItem Then

            Dim dataItem As GridDataItem = CType(e.Item, GridDataItem)

            If e.Item.OwnerTableView.Name = "Products" Then

                Dim lnkdel As ImageButton = CType(dataItem("Delete").FindControl("btnDelete"), ImageButton)
                lnkdel.Attributes.Add("onclick", "return confirm ('" & Resources.Resource.ProductsDeleteConfirmationMessage & "');")
                Dim lnkEdit As LinkButton = CType(dataItem("Codigo").FindControl("lnkEdit"), LinkButton)

                If Session("perfilid") <> 1 Then
                    lnkdel.Enabled = False
                End If

                If Session("perfilid") = 3 Then
                    lnkEdit.Enabled = False
                End If

            End If

        End If
        If IsPostBack Then
            Select Case e.Item.ItemType
                Case Telerik.Web.UI.GridItemType.Footer
                    If ds.Tables(0).Rows.Count > 0 Then
                        'If Not IsDBNull(ds.Tables(0).Compute("sum(mexico)", "")) Then
                        '    e.Item.Cells(13).Text = FormatNumber(ds.Tables(0).Compute("sum(mexico)", ""), 2).ToString
                        '    e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
                        '    e.Item.Cells(13).Font.Bold = True
                        'End If
                        'If Not IsDBNull(ds.Tables(0).Compute("sum(monterrey)", "")) Then
                        '    e.Item.Cells(10).Text = FormatNumber(ds.Tables(0).Compute("sum(monterrey)", ""), 2).ToString
                        '    e.Item.Cells(10).HorizontalAlign = HorizontalAlign.Right
                        '    e.Item.Cells(10).Font.Bold = True
                        'End If
                        'If Not IsDBNull(ds.Tables(0).Compute("sum(guadalajara)", "")) Then
                        '    e.Item.Cells(14).Text = FormatNumber(ds.Tables(0).Compute("sum(guadalajara)", ""), 2).ToString
                        '    e.Item.Cells(14).HorizontalAlign = HorizontalAlign.Right
                        '    e.Item.Cells(14).Font.Bold = True
                        'End If
                        'If Not IsDBNull(ds.Tables(0).Compute("sum(matriz)", "")) Then
                        '    e.Item.Cells(11).Text = FormatNumber(ds.Tables(0).Compute("sum(matriz)", ""), 0).ToString
                        '    e.Item.Cells(11).HorizontalAlign = HorizontalAlign.Right
                        '    e.Item.Cells(11).Font.Bold = True
                        'End If
                        'If Not IsDBNull(ds.Tables(0).Compute("sum(mermas)", "")) Then
                        '    e.Item.Cells(13).Text = FormatNumber(ds.Tables(0).Compute("sum(mermas)", ""), 2).ToString
                        '    e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
                        '    e.Item.Cells(13).Font.Bold = True
                        'End If

                        'If Not IsDBNull(ds.Tables(0).Compute("sum(proceso)", "")) Then
                        '    e.Item.Cells(12).Text = FormatNumber(ds.Tables(0).Compute("sum(proceso)", ""), 0).ToString
                        '    e.Item.Cells(12).HorizontalAlign = HorizontalAlign.Right
                        '    e.Item.Cells(12).Font.Bold = True
                        'End If
                        If Not IsDBNull(ds.Tables(0).Compute("sum(disponibles)", "")) Then
                            e.Item.Cells(13).Text = FormatNumber(ds.Tables(0).Compute("sum(consignacion)", ""), 0).ToString
                            e.Item.Cells(13).HorizontalAlign = HorizontalAlign.Right
                            e.Item.Cells(13).Font.Bold = True
                        End If
                        'If Not IsDBNull(ds.Tables(0).Compute("sum(disponibles)", "")) Then
                        '    e.Item.Cells(14).Text = FormatNumber(ds.Tables(0).Compute("sum(disponibles)", ""), 0).ToString
                        '    e.Item.Cells(14).HorizontalAlign = HorizontalAlign.Right
                        '    e.Item.Cells(14).Font.Bold = True
                        'End If


                    End If
            End Select
        End If


    End Sub

    Protected Sub productslist_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles productslist.ItemCommand

        Select Case e.CommandName

            Case "cmdEdit"
                Call EditProduct(e.CommandArgument)
                Call CargaClientes()
                Call MuestraCodigos()
            Case "cmdDelete"
                Call DeleteProduct(e.CommandArgument)

        End Select
    End Sub

    Private Sub DeleteProduct(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pMisProductos  @cmd='2', @productoId ='" & id.ToString & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader

            rs = cmd.ExecuteReader()
            rs.Close()

            conn.Close()

            panelProductList.Visible = True
            panelProductRegistration.Visible = False
            ds = GetProducts()
            productslist.DataSource = ds
            productslist.DataBind()

        Catch ex As Exception


        Finally

            conn.Close()
            conn.Dispose()

        End Try

    End Sub

    Private Sub EditProduct(ByVal id As Integer)

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)

        Try

            Dim cmd As New SqlCommand("EXEC pMisProductos @cmd=4, @productoid='" & id & "'", conn)

            conn.Open()

            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()

            If rs.Read Then

                txtCode.Text = rs("codigo")
                'txtSKU.Text = rs("sku")
                txtUPC.Text = rs("upc")
                txtUnitaryPrice.Text = rs("unitario")
                txtUnitaryPrice2.Text = rs("unitario2")
                txtUnitaryPrice3.Text = rs("unitario3")
                txtUnitaryPrice4.Text = rs("unitario4")
                txtDescription.Text = rs("descripcion")
                txtMinimo.Text = rs("minimo")
                txtMaximo.Text = rs("maximo")
                txtReorden.Text = rs("punto_reorden")
                txtCostoStd.Text = rs("costo_estandar")
                txtCostoProm.Text = rs("costo_promedio")
                txtCompraMinima.Text = rs("compra_min")
                txtTiempoEntrega.Text = rs("tiempo_entrega")
                txtPresentacion.Text = rs("presentacion")
                txtPesoUnitario.Text = rs("peso")

                txtmodeloEstilo.Text = rs("modeloEstilo")
                txtplataforma.Text = rs("plataforma")
                cmbgeneroid.SelectedValue = rs("generoid")
                txttallaUSA.Text = rs("tallaUSA")
                txttallaMX.Text = rs("tallaMX")
                txtcolor.Text = rs("color")
                txtmaterial.Text = rs("material")
                txtpesoKg.Text = rs("pesoKg")
                txtempaqueAlto.Text = rs("empaqueAlto")
                txtempaqueLargo.Text = rs("empaqueLargo")
                txtempaqueAncho.Text = rs("empaqueAncho")
                txtcolorMX.Text = rs("colorMx")
                txtDescriptionCorta.Text = rs("descripcion_corta")


                If rs("foto").ToString.Length > 0 Then
                    imgFoto.Visible = True
                    imgFoto.ImageUrl = "~/portalcfd/images/productos/" & rs("foto")
                Else
                    imgFoto.Visible = False
                End If

                lblImgFoto.Text = rs("foto")

                If rs("inventariableBit") = "1" Then
                    chkInventariableBit.Checked = True
                Else
                    chkInventariableBit.Checked = False
                End If

                If rs("perecederoBit") = "1" Then
                    chkPerecederoBit.Checked = True
                Else
                    chkPerecederoBit.Checked = False
                End If

                panelStockLocator.Visible = True
                panelCodigosAlternos.Visible = True
                panelProductList.Visible = False
                panelProductRegistration.Visible = True

                InsertOrUpdate.Value = 1
                ProductID.Value = id
                Dim objCat As New DataControl
                objCat.Catalogo(tasaid, "select id, nombre from tblTasa order by id", rs("tasaid"))
                objCat.Catalogo(monedaid, "select id, nombre from tblMoneda order by id", rs("monedaid"))
                objCat.Catalogo(proveedorid, "select id, razonsocial from tblMisProveedores order by razonsocial", rs("proveedorid"))
                objCat.Catalogo(coleccionid, "select id, isnull(codigo,'') + ' - ' + isnull(nombre,'') as nombre from tblColeccion where isnull(borradoBit,0)=0 order by nombre", rs("coleccionid"))
                objCat.Catalogo(proyectoid, "select id, nombre from tblProyecto order by id", rs("proyectoid"))
                objCat.Catalogo(cboclaveunidad, "select clave, clave + ' - ' + isnull(nombre,'') as nombre from tblUnidad order by nombre", rs("claveunidad"))
                objCat.Catalogo(cboproductoserv, "select clave, clave + ' - ' + isnull(nombre,'') as nombre from tblClaveProducto order by nombre", rs("claveprodserv"))
                cbmObjetoImpuesto.SelectedValue = rs("objeto_impuestoid")
                objCat = Nothing

                Call MuestraCodigosStockLocart()

            End If

        Catch ex As Exception
            Throw New Exception("Error: " & ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
        End Try
        Call setForUpMarquetplaces()
    End Sub

    Private Sub CargaClientes()
        Dim ObjData As New DataControl
        ObjData.Catalogo(clienteid, "select id, razonsocial from tblMisClientes order by razonsocial", 0)
        ObjData = Nothing
    End Sub

    Private Sub MuestraCodigos()
        Dim ObjData As New DataControl
        ClientCodesList.DataSource = ObjData.FillDataSet("exec pMisProductos @cmd=7, @productoid='" & ProductID.Value.ToString & "'")
        ClientCodesList.DataBind()
        ObjData = Nothing
    End Sub
    Private Sub MuestraCodigosStockLocart()
        Dim ObjData As New DataControl
        ProductCodesList.DataSource = ObjData.FillDataSet("exec pMisProductos @cmd=15, @productoid='" & ProductID.Value.ToString & "'")
        ProductCodesList.DataBind()
        ObjData = Nothing
    End Sub


    Private Sub ProductCodesList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles ProductCodesList.NeedDataSource
        Dim ObjData As New DataControl
        ProductCodesList.DataSource = ObjData.FillDataSet("exec pMisProductos @cmd=15, @productoid='" & ProductID.Value.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub ClientCodesList_ItemCommand(sender As Object, e As Telerik.Web.UI.GridCommandEventArgs) Handles ClientCodesList.ItemCommand
        Select Case e.CommandName
            Case "cmdDelete"
                Call BorraCodigo(e.CommandArgument)
                Call MuestraCodigos()
        End Select
    End Sub

    Private Sub ClientCodesList_ItemDataBound(sender As Object, e As Telerik.Web.UI.GridItemEventArgs) Handles ClientCodesList.ItemDataBound
        Select Case e.Item.ItemType
            Case GridItemType.Item, GridItemType.AlternatingItem
                Dim btnDelete As ImageButton = CType(e.Item.FindControl("btnDelete"), ImageButton)
                btnDelete.Attributes.Add("onclick", "javascript:return confirm('Va a eliminar un código de cliente. ¿Desea contiuar?');")
        End Select
    End Sub

    Private Sub ClientCodesList_NeedDataSource(sender As Object, e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles ClientCodesList.NeedDataSource
        Dim ObjData As New DataControl
        ClientCodesList.DataSource = ObjData.FillDataSet("exec pMisProductos @cmd=7")
        ObjData = Nothing
    End Sub

#End Region

#Region "Telerik Grid Products Column Names (From Resource File)"

    Protected Sub productslist_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles productslist.ItemCreated

        If TypeOf e.Item Is GridHeaderItem Then

            Dim header As GridHeaderItem = CType(e.Item, GridHeaderItem)

            If e.Item.OwnerTableView.Name = "Products" Then

                header("codigo").Text = Resources.Resource.gridColumnNameCode
                'header("unidad").Text = Resources.Resource.gridColumnNameMeasureUnit
                header("descripcion").Text = Resources.Resource.gridColumnNameDescription
                header("unitario").Text = Resources.Resource.gridColumnNameUnitaryPrice
                header("unitario2").Text = Resources.Resource.gridColumnNameUnitaryPrice2
                header("unitario3").Text = Resources.Resource.gridColumnNameUnitaryPrice3
                header("unitario4").Text = Resources.Resource.gridColumnNameUnitaryPrice4
            End If

        End If

    End Sub

#End Region

#Region "Display Product Data Panel"

    Protected Sub btnAddProduct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddProduct.Click

        InsertOrUpdate.Value = 0

        txtCode.Text = ""
        'txtUnit.Text = ""
        'txtUnitaryPrice.Text = ""
        'txtUnitaryPrice2.Text = ""
        'txtUnitaryPrice3.Text = ""
        'txtDescription.Text = ""

        panelProductList.Visible = False
        panelProductRegistration.Visible = True

    End Sub

#End Region

#Region "Save Product"

    Protected Sub btnSaveClient_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveProduct.Click

        Dim inventariableBit As Integer = 0
        Dim perecederoBit As Integer = 0

        If chkInventariableBit.Checked = True Then
            inventariableBit = 1
        Else
            inventariableBit = 0
        End If

        If chkPerecederoBit.Checked = True Then
            perecederoBit = 1
        Else
            perecederoBit = 0
        End If
        '
        '   Guarda imagen
        '
        Dim thumbnailName As String
        If foto.PostedFile.ContentLength > 0 Then
            thumbnailName = foto.PostedFile.FileName.Substring(foto.PostedFile.FileName.LastIndexOf("\") + 1)
            foto.SaveAs(Server.MapPath("..\images\productos\" + thumbnailName.ToString))
        Else
            thumbnailName = lblImgFoto.Text
        End If

        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim numCmd = 3
        Try

            If InsertOrUpdate.Value <> 0 Then
                numCmd = 5
            End If
            Dim DataControl As New DataControl
            Dim textComand As String = "EXEC pMisProductos @cmd=" & numCmd & ", @productoid=" & ProductID.Value & ", @clienteid='" & Session("clienteid").ToString &
                                                                  "', @codigo='" & txtCode.Text & "', @claveprodserv='" & cboproductoserv.SelectedValue.ToString &
                                                                  "', @claveunidad='" & cboclaveunidad.SelectedValue.ToString & "', @unitario='" & txtUnitaryPrice.Text &
                                                                  "', @unitario2='" & txtUnitaryPrice2.Text & "', @unitario3='" & txtUnitaryPrice3.Text &
                                                                  "', @unitario4='" & txtUnitaryPrice4.Text & "', @descripcion='" & txtDescription.Text &
                                                                  "', @tasaid='" & tasaid.SelectedValue.ToString & "', @maximo='" & txtMaximo.Text &
                                                                  "', @minimo='" & txtMinimo.Text & "', @punto_reorden='" & txtReorden.Text &
                                                                  "', @costo_estandar='" & txtCostoStd.Text & "', @costo_promedio='" & txtCostoProm.Text &
                                                                  "', @compra_min='" & txtCompraMinima.Text & "', @tiempo_entrega='" & txtTiempoEntrega.Text &
                                                                  "', @presentacion='" & txtPresentacion.Text & "', @monedaid='" & monedaid.SelectedValue.ToString &
                                                                  "', @peso='" & txtPesoUnitario.Text & "', @proveedorId='" & proveedorid.SelectedValue.ToString &
                                                                  "', @inventariableBit='" & inventariableBit.ToString & "', @foto='" & thumbnailName.ToString &
                                                                  "', @perecederoBit='" & perecederoBit.ToString &
                                                                  "', @coleccionid='" & coleccionid.SelectedValue.ToString & "', @proyectoid='" & proyectoid.SelectedValue.ToString &
                                                                  "', @upc='" & txtUPC.Text.ToString & "', @modeloEstilo ='" & txtmodeloEstilo.Text.ToString &
                                                                  "', @plataforma='" & txtplataforma.Text.ToString & "', @genero='" & cmbgeneroid.SelectedItem.ToString &
                                                                  "', @generoid ='" & cmbgeneroid.SelectedValue & "', @tallaUSA='" & txttallaUSA.Text.ToString &
                                                                  "', @tallaMX='" & txttallaMX.Text.ToString & "', @color='" & txtcolor.Text.ToString &
                                                                  "', @material='" & txtmaterial.Text.ToString & "', @pesoKg='" & txtpesoKg.Text.ToString &
                                                                  "', @empaqueAlto='" & txtempaqueAlto.Text.ToString & "', @empaqueLargo='" & txtempaqueLargo.Text.ToString &
                                                                  "', @empaqueAncho='" & txtempaqueAncho.Text.ToString &
                                                                  "', @objeto_impuestoid = '" & cbmObjetoImpuesto.SelectedValue &
                                                                  "', @colorMX='" & txtcolorMX.Text.ToString & "', @descripcion_corta='" & txtDescriptionCorta.Text.ToString &
                                                                  "'"
            lblMensaje.Text = DataControl.RunSQLScalarQueryString(textComand)
            DataControl = Nothing


            If lblMensaje.Text = "" Then
                panelProductList.Visible = True
                panelProductRegistration.Visible = False
                txtSearch.Text = txtCode.Text
                ds = GetProducts()
                productslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ClientsEmptyGridMessage
                productslist.DataSource = ds
                productslist.DataBind()
            End If
        Catch ex As Exception
            Throw New Exception("Error: " & ex.Message)
        Finally

            conn.Close()
            conn.Dispose()
            conn = Nothing
            Call guardaMarketplaces()
        End Try

    End Sub

#End Region

#Region "Carga de CVS Cambio de Precios"

    Protected Sub btnCargarExcelCP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargarExcelCP.Click
        If fileUpload1.HasFile Then
            If System.IO.Path.GetExtension(fileUpload1.FileName).ToUpper = ".CSV" Then
                panelErroresCP.Visible = False

                Dim nombreArchivo As String = ""
                nombreArchivo = fileUpload1.PostedFile.FileName.ToString

                For i = 1 To 99999
                    If Not File.Exists(Server.MapPath("../almacen/cargaCsvCP/") & nombreArchivo) Then
                        fileUpload1.SaveAs(Server.MapPath("../almacen/cargaCsvCP/") & nombreArchivo)
                        Exit For
                    Else
                        nombreArchivo = i.ToString + "_" + fileUpload1.PostedFile.FileName.ToString
                    End If
                Next

                Dim registros As Integer
                Try
                    registros = System.IO.File.ReadAllLines(Server.MapPath("../almacen/cargaCsvCP/") & nombreArchivo).Count
                Catch ex As Exception
                    registros = 0
                End Try

                If registros > 0 Then
                    registros = registros - 1
                End If

                Dim registros_error As Integer = 0
                Dim registros_correctos As Integer = 0
                Dim msgerror As String = ""

                Dim cargaid As Long = 0
                Dim ObjData As New DataControl
                cargaid = ObjData.RunSQLScalarQuery("exec pCargaCambioPreciosCsv @cmd=1, @userid='" & Session("userid").ToString & "', @archivo='" & nombreArchivo.ToString & "', @registros='" & registros.ToString & "'")
                cargaidHiddenCP.Value = cargaid

                If registros = 0 Then
                    ObjData.RunSQLQuery("exec pCargaCambioPreciosCsv @cmd=9, @cargaid='" & cargaid.ToString & "")
                End If

                Dim progress As RadProgressContext = RadProgressContext.Current
                progress.SecondaryTotal = registros
                progress.Speed = "N/A"

                ds = New DataSet
                ds = ObjData.FillDataSet("exec pCargaCambioPreciosCsv @cmd=2")

                Dim dtConceptosDetalle As New DataTable

                If ds.Tables.Count > 0 Then
                    dtConceptosDetalle = ds.Tables(0)

                End If

                Dim j As Integer = 0
                Dim line As String = ""

                Dim reader As System.IO.StreamReader = New StreamReader(Server.MapPath("../almacen/cargaCsvCP/") & nombreArchivo, System.Text.Encoding.Default)
                Do

                    j += 1

                    line = reader.ReadLine

                    If line = Nothing Then
                        Exit Do
                    End If


                    'Dim cantidad As Integer = 0
                    Dim codigoId As String = ""
                    Dim codigoActivo As Integer = 0

                    Dim codigo As String = ""
                    Dim precioUnit1 As Decimal = 0
                    Dim precioUnit2 As Decimal = 0
                    Dim precioUnit3 As Decimal = 0
                    Dim precioUnit4 As Decimal = 0

                    If j > 1 Then
                        ' CODIGO
                        Try
                            codigo = line.Split(",")(0)
                        Catch ex As Exception
                            codigo = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            precioUnit1 = line.Split(",")(1)
                        Catch ex As Exception
                            precioUnit1 = 0
                        End Try

                        ' CODIGO REORDEN
                        Try
                            precioUnit2 = line.Split(",")(2)
                        Catch ex As Exception
                            precioUnit2 = 0
                        End Try

                        ' CODIGO REORDEN
                        Try
                            precioUnit3 = line.Split(",")(3)
                        Catch ex As Exception
                            precioUnit3 = 0
                        End Try

                        ' CODIGO REORDEN
                        Try
                            precioUnit4 = line.Split(",")(4)
                        Catch ex As Exception
                            precioUnit4 = 0
                        End Try


                        Try
                            Dim rowCodigo() As DataRow = dtConceptosDetalle.Select("codigo = '" & LTrim(RTrim(codigo)).ToString & "'")
                            For Each row As DataRow In rowCodigo
                                codigo = row(0)
                            Next
                        Catch ex As Exception
                            codigo = 0
                        End Try

                        Try
                            Dim rowPrecioUnit1() As DataRow = dtConceptosDetalle.Select("precioUnit1 = '" & LTrim(RTrim(precioUnit1)).ToString & "'")
                            For Each row As DataRow In rowPrecioUnit1
                                rowPrecioUnit1 = row(0)
                            Next
                        Catch ex As Exception
                            precioUnit1 = 0
                        End Try

                        Try
                            Dim rowPrecioUnit2() As DataRow = dtConceptosDetalle.Select("precioUnit2 = '" & LTrim(RTrim(precioUnit2)).ToString & "'")
                            For Each row As DataRow In rowPrecioUnit2
                                rowPrecioUnit2 = row(0)
                            Next
                        Catch ex As Exception
                            precioUnit2 = 0
                        End Try

                        Try
                            Dim rowPrecioUnit3() As DataRow = dtConceptosDetalle.Select("precioUnit3 = '" & LTrim(RTrim(precioUnit3)).ToString & "'")
                            For Each row As DataRow In rowPrecioUnit3
                                rowPrecioUnit3 = row(0)
                            Next
                        Catch ex As Exception
                            precioUnit3 = 0
                        End Try

                        Try
                            Dim rowPrecioUnit4() As DataRow = dtConceptosDetalle.Select("precioUnit4 = '" & LTrim(RTrim(precioUnit4)).ToString & "'")
                            For Each row As DataRow In rowPrecioUnit4
                                rowPrecioUnit4 = row(0)
                            Next
                        Catch ex As Exception
                            precioUnit4 = 0
                        End Try



                        If codigo.Length > 0 Then

                            codigoActivo = ObjData.RunSQLScalarQuery("exec pCargaCambioPreciosCsv @cmd=14, @codigo='" & codigo & "'")

                            If codigoActivo = 0 Then
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@precioUnit1", precioUnit1))
                                p.Add(New SqlParameter("@precioUnit2", precioUnit2))
                                p.Add(New SqlParameter("@precioUnit3", precioUnit3))
                                p.Add(New SqlParameter("@precioUnit4", precioUnit4))
                                p.Add(New SqlParameter("@error", "El código " & codigo & " no está registrado."))
                                ' p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaCambioPreciosCsv", p)
                                registros_error = registros_error + 1

                            Else
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 3))
                                p.Add(New SqlParameter("@cargaid", cargaid))
                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@precioUnit1", precioUnit1))
                                p.Add(New SqlParameter("@precioUnit2", precioUnit2))
                                p.Add(New SqlParameter("@precioUnit3", precioUnit3))
                                p.Add(New SqlParameter("@precioUnit4", precioUnit4))
                                'p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaCambioPreciosCsv", p)
                                registros_correctos = registros_correctos + 1
                            End If
                        Else

                            msgerror = ""
                            If codigo.Length = 0 Then
                                msgerror = msgerror & "Código es requerido."
                            End If

                            If codigoId.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Codigo no registrado."
                            End If
                            Dim p As New ArrayList
                            p.Add(New SqlParameter("@cmd", 7))
                            p.Add(New SqlParameter("@cargaid", cargaid))
                            p.Add(New SqlParameter("@codigo", codigo))
                            p.Add(New SqlParameter("@precioUnit1", precioUnit1))
                            p.Add(New SqlParameter("@precioUnit2", precioUnit2))
                            p.Add(New SqlParameter("@precioUnit3", precioUnit3))
                            p.Add(New SqlParameter("@precioUnit4", precioUnit4))
                            p.Add(New SqlParameter("@error", msgerror))
                            'p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                            ObjData.ExecuteNonQueryWithParams("pCargaCambioPreciosCsv", p)
                            registros_error = registros_error + 1
                        End If

                    End If

                    progress.SecondaryValue = j
                    progress.SecondaryPercent = Math.Round((j * 100 / registros), 0)
                    progress.CurrentOperationText = "Registro " & j.ToString()

                    If Not Response.IsClientConnected Then
                        Exit Do
                    End If

                    progress.TimeEstimated = (registros - j) * 100
                    System.Threading.Thread.Sleep(100)

                Loop Until line Is Nothing

                reader.Close()

                If registros_correctos > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 8))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_correctos", registros_correctos))
                    ObjData.ExecuteNonQueryWithParams("pCargaCambioPreciosCsv", p)
                End If

                If registros_error > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 9))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_error", registros_error))
                    ObjData.ExecuteNonQueryWithParams("pCargaCambioPreciosCsv", p)

                    panelErroresCP.Visible = True
                    erroresListCP.DataSource = ObjData.FillDataSet("exec pCargaCambioPreciosCsv @cmd=10, @cargaid='" & cargaid.ToString & "'")
                    erroresListCP.DataBind()

                End If

                If registros_correctos = 0 Then
                    ObjData.RunSQLQuery("exec pCargaCambioPreciosCsv @cmd=11, @cargaid='" & cargaid.ToString & "'")
                End If

                If registros_correctos > 0 And registros_error > 0 Then
                    rwAlertaCP.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.<br>" & registros_error.ToString & " con error, favor de verificar.", 350, 200, "Alerta", "", "")
                ElseIf registros_correctos > 0 Then
                    rwAlertaCP.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.", 350, 200, "Alerta", "", "")
                ElseIf registros_error > 0 Then
                    rwAlertaCP.RadAlert("Se encontraron " & registros_error.ToString & " productos con error, favor de verificar.", 350, 200, "Alerta", "", "")
                End If

                ObjData = Nothing
            Else
                rwAlertaCP.RadAlert("Formato CSV no válido.", 350, 200, "Alerta", "", "")
            End If
        Else
            rwAlertaCP.RadAlert("Selecciona un archivo en formato CSV.", 350, 200, "Alerta", "", "")
        End If

        panelCSVCP.Visible = True
        'btnAgregaConceptos.Visible = True
        btnCambiarPrecios.Enabled = True
        Dim ObjData2 As New DataControl
        ds = ObjData2.FillDataSet("exec pCargaCambioPreciosCsv @cmd=5, @cargaid='" & cargaidHiddenCP.Value & "'")
        Dim lineas As Integer = ds.Tables(0).Rows.Count
        If lineas = 0 Then
            lineas = 1
        End If
        resultslistCSVCP.PageSize = lineas
        resultslistCSVCP.DataSource = ds
        resultslistCSVCP.DataBind()
        ObjData2.RunSQLQuery("exec pCargaCambioPreciosCsv @cmd=12")

        ObjData2 = Nothing
    End Sub

    Private Sub imgDownloadCP_Click(sender As Object, e As ImageClickEventArgs) Handles imgDownloadCP.Click
        Dim FilePath = Server.MapPath("~/portalcfd/almacen/cargaCsvCP/") & "FORMATOCARGA.csv"

        If File.Exists(FilePath) Then
            Dim FileName As String = Path.GetFileName(FilePath)
            Response.Clear()
            Response.ContentType = "application/octet-stream"
            Response.AddHeader("Content-Disposition", "attachment; filename=""" & FileName & """")
            Response.Flush()
            Response.WriteFile(FilePath)
            Response.End()
        End If
    End Sub
    Private Sub erroresListCP_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles erroresListCP.NeedDataSource
        Dim ObjData As New DataControl
        erroresListCP.DataSource = ObjData.FillDataSet("exec pCargaCambioPreciosCsv @cmd=10, @cargaid='" & cargaidHiddenCP.Value.ToString & "'")
        ObjData = Nothing
    End Sub

#End Region

#Region "Cancel Product (Save/Edit)"

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Response.Redirect("~/portalcfd/almacen/Productos.aspx")

    End Sub

#End Region

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        productslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ProductsEmptyGridMessage
        ds = GetProducts()
        productslist.DataSource = ds
        productslist.DataBind()
    End Sub

    Protected Sub btnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAll.Click
        txtSearch.Text = ""
        filtrocoleccionid.SelectedValue = 0
        filtromarcaid.SelectedValue = 0
        productslist.MasterTableView.NoMasterRecordsText = Resources.Resource.ProductsEmptyGridMessage
        ds = GetProducts()
        productslist.DataSource = ds
        productslist.DataBind()
    End Sub

    Private Sub btnAddCode_Click(sender As Object, e As System.EventArgs) Handles btnAddCode.Click
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pMisProductos @cmd=6, @clienteid='" & clienteid.SelectedValue.ToString & "', @productoid='" & ProductID.Value.ToString & "', @codigo='" & txtClientCode.Text & "'")
        ObjData = Nothing
        clienteid.SelectedIndex = 0
        txtClientCode.Text = ""
        Call MuestraCodigos()
    End Sub

    Private Sub BorraCodigo(ByVal codigoid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pMisProductos @cmd=8, @codigoclienteid='" & codigoid.ToString & "'")
        ObjData = Nothing
    End Sub

    Private Sub BorraCodigoStockLocator(ByVal codigoid As Long)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pMisProductos @cmd=16, @codigoproductoid='" & codigoid.ToString & "'")
        ObjData = Nothing
    End Sub

    Public Sub setForUpMarquetplaces()
        Dim ObjData As New DataControl
        Dim ptable As New DataSet
        ptable = ObjData.FillDataSet("exec pRelacionProductoMarketplaces @cmd=2, @productoid='" & ProductID.Value.ToString & "'")
        For Each item In ptable.Tables(0).Rows
            For Each citem As ListItem In ckMarketplaces.Items
                If citem.Value = item("marketplaces") Then
                    citem.Selected = True
                End If
            Next
        Next
        ObjData = Nothing
    End Sub

    Private Sub guardaMarketplaces()
        For Each li As ListItem In ckMarketplaces.Items
            If li.Selected Then
                CreaRelacionMarketPlace(li.Value)
            Else
                BorraRelacionProductoMarketplaces(li.Value)
            End If
        Next
    End Sub

    Private Sub BorraRelacionProductoMarketplaces(ByVal productoid As Integer)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pRelacionProductoMarketplaces @cmd=3, @marketplaces='" & productoid & "', @productoid='" & ProductID.Value & "'")
        ObjData = Nothing
    End Sub

    Private Sub CreaRelacionMarketPlace(ByVal productoid As Integer)
        Dim ObjData As New DataControl
        ObjData.RunSQLQuery("exec pRelacionProductoMarketplaces @cmd=1, @marketplaces='" & productoid & "', @productoid='" & ProductID.Value & "'")
        ObjData = Nothing
    End Sub

    Private Sub btnDowloadCatalogo_Click(sender As Object, e As EventArgs) Handles btnDowloadCatalogo.Click
        productslist.ExportSettings.FileName = "CatálogoProductos_" & Format(Now(), "ddMMyyHHmmss")

        productslist.MasterTableView.GetColumn("Codigo").HeaderStyle.Width = 120
        productslist.MasterTableView.GetColumn("Codigo").ItemStyle.HorizontalAlign = HorizontalAlign.Center
        productslist.MasterTableView.GetColumn("upc").HeaderStyle.Width = 120
        productslist.MasterTableView.GetColumn("descripcion").HeaderStyle.Width = 250
        productslist.MasterTableView.GetColumn("descripcion").ItemStyle.HorizontalAlign = HorizontalAlign.Left
        productslist.MasterTableView.GetColumn("proyecto").HeaderStyle.Width = 100
        productslist.MasterTableView.GetColumn("coleccion").HeaderStyle.Width = 150
        productslist.MasterTableView.GetColumn("unitario").HeaderStyle.Width = 100
        productslist.MasterTableView.GetColumn("unitario2").HeaderStyle.Width = 100
        productslist.MasterTableView.GetColumn("unitario3").HeaderStyle.Width = 100
        productslist.MasterTableView.GetColumn("unitario4").HeaderStyle.Width = 100

        productslist.MasterTableView.GetColumn("modeloEstilo").Display = True
        productslist.MasterTableView.GetColumn("modeloEstilo").Exportable = True

        productslist.MasterTableView.GetColumn("plataforma").Display = True
        productslist.MasterTableView.GetColumn("plataforma").Exportable = True

        productslist.MasterTableView.GetColumn("genero").Display = True
        productslist.MasterTableView.GetColumn("genero").Exportable = True

        productslist.MasterTableView.GetColumn("tallaUSA").Display = True
        productslist.MasterTableView.GetColumn("tallaUSA").Exportable = True

        productslist.MasterTableView.GetColumn("tallaMX").Display = True
        productslist.MasterTableView.GetColumn("tallaMX").Exportable = True

        productslist.MasterTableView.GetColumn("color").Display = True
        productslist.MasterTableView.GetColumn("color").Exportable = True

        productslist.MasterTableView.GetColumn("material").Display = True
        productslist.MasterTableView.GetColumn("material").Exportable = True

        productslist.MasterTableView.GetColumn("pesoKg").Display = True
        productslist.MasterTableView.GetColumn("pesoKg").Exportable = True

        productslist.MasterTableView.GetColumn("empaqueAlto").Display = True
        productslist.MasterTableView.GetColumn("empaqueAlto").Exportable = True

        productslist.MasterTableView.GetColumn("empaqueLargo").Display = True
        productslist.MasterTableView.GetColumn("empaqueLargo").Exportable = True

        productslist.MasterTableView.GetColumn("empaqueAncho").Display = True
        productslist.MasterTableView.GetColumn("empaqueAncho").Exportable = True
        '
        productslist.MasterTableView.GetColumn("unidad").Display = True
        productslist.MasterTableView.GetColumn("unidad").Exportable = True

        productslist.MasterTableView.GetColumn("claveprodserv").Display = True
        productslist.MasterTableView.GetColumn("claveprodserv").Exportable = True

        productslist.MasterTableView.GetColumn("moneda").Display = True
        productslist.MasterTableView.GetColumn("moneda").Exportable = True

        productslist.MasterTableView.GetColumn("Tasa").Display = True
        productslist.MasterTableView.GetColumn("Tasa").Exportable = True

        productslist.MasterTableView.GetColumn("costo").Display = True
        productslist.MasterTableView.GetColumn("costo").Exportable = True

        productslist.MasterTableView.GetColumn("promedio").Display = True
        productslist.MasterTableView.GetColumn("promedio").Exportable = True

        productslist.MasterTableView.GetColumn("colorMx").Display = True
        productslist.MasterTableView.GetColumn("colorMx").Exportable = True

        productslist.MasterTableView.GetColumn("descripcion_corta").Display = True
        productslist.MasterTableView.GetColumn("descripcion_corta").Exportable = True
        productslist.MasterTableView.GetColumn("descripcion_corta").HeaderStyle.Width = 200
        productslist.MasterTableView.GetColumn("descripcion_corta").ItemStyle.HorizontalAlign = HorizontalAlign.Center

        productslist.MasterTableView.ExportToExcel()
    End Sub

    Private Sub btnCambiarPrecios_Click(sender As Object, e As EventArgs) Handles btnCambiarPrecios.Click

        Try
            Dim ObjData2 As New DataControl

            '
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
            Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
            '
            Dim ObjData As New DataControl
            Dim codigo As String = ""
            Dim precioUnit1 As Double = 0.00
            Dim precioUnit2 As Double = 0.00
            Dim precioUnit3 As Double = 0.00
            Dim precioUnit4 As Double = 0.00


            For Each row As GridDataItem In resultslistCSVCP.MasterTableView.Items
                'conceptoId = row.GetDataKeyValue("id")
                codigo = row.GetDataKeyValue("codigo")
                precioUnit1 = row.GetDataKeyValue("precioUnit1")
                precioUnit2 = row.GetDataKeyValue("precioUnit2")
                precioUnit3 = row.GetDataKeyValue("precioUnit3")
                precioUnit4 = row.GetDataKeyValue("precioUnit4")

                If codigo.Length > 0 Then

                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 20))
                    p.Add(New SqlParameter("@codigo", codigo.ToString))
                    p.Add(New SqlParameter("@unitario", precioUnit1.ToString))
                    p.Add(New SqlParameter("@unitario2", precioUnit2.ToString))
                    p.Add(New SqlParameter("@unitario3", precioUnit3.ToString))
                    p.Add(New SqlParameter("@unitario4", precioUnit4.ToString))
                    ObjData.ExecuteNonQueryWithParams("pMisProductos", p)
                    lblMensajeCSVCP.Text = "Precios Actualizados."
                End If
            Next        '

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
            Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
            '

            '

            'Call CargaConceptos()
            'Call CargaListaOrdenesParciales()
            panelCSVCP.Visible = False
            panelErroresCP.Visible = False

            resultslistCSVCP.DataSource = Nothing
            resultslistCSVCP.DataBind()
            erroresListCP.DataSource = Nothing
            erroresListCP.DataBind()

        Catch ex As Exception
            lblMensajeCSVCP.Text = "Error: " & ex.Message.ToString
        Finally
        End Try
    End Sub
End Class
