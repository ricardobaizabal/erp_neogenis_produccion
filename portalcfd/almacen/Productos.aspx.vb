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

#Region "Carga de CVS"
    Protected Sub btnCargarExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCargarExcel.Click
        If fileUploadExcel1.HasFile Then
            If System.IO.Path.GetExtension(fileUploadExcel1.FileName).ToUpper = ".CSV" Then
                panelErrores.Visible = False

                Dim nombreArchivo As String = ""
                nombreArchivo = fileUploadExcel1.PostedFile.FileName.ToString

                ' Si no existe archivo entonces lo creo
                ' Si ya existe entonces lo agrego consecutivo al principio
                For i = 1 To 99999
                    If Not File.Exists(Server.MapPath("../almacen/cargaCsv/") & nombreArchivo) Then
                        fileUploadExcel1.SaveAs(Server.MapPath("../almacen/cargaCsv/") & nombreArchivo)
                        Exit For
                    Else
                        nombreArchivo = i.ToString + "_" + fileUploadExcel1.PostedFile.FileName.ToString
                    End If
                Next

                'Leo todas las líneas del csv
                Dim registros As Integer
                Try
                    registros = System.IO.File.ReadAllLines(Server.MapPath("../almacen/cargaCsv/") & nombreArchivo).Count
                Catch ex As Exception
                    registros = 0
                End Try

                If registros > 0 Then
                    registros = registros - 1
                End If

                Dim registros_error As Integer = 0
                Dim registros_correctos As Integer = 0
                Dim msgerror As String = ""

                'Ejecuto store procedure de carga de productos, generación de encabezado 
                Dim cargaid As Long = 0
                Dim ObjData As New DataControl
                cargaid = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=1, @userid='" & Session("userid").ToString & "', @archivo='" & nombreArchivo.ToString & "', @registros='" & registros.ToString & "'")
                cargaidHidden.Value = cargaid

                'actualiza registros erroneos
                If registros = 0 Then
                    ObjData.RunSQLQuery("exec pCargaProductosCsv @cmd=9, @cargaid='" & cargaid.ToString & "")
                End If

                Dim progress As RadProgressContext = RadProgressContext.Current
                progress.SecondaryTotal = registros
                progress.Speed = "N/A"

                'consulto registros de detalle, si es que hay
                ds = New DataSet
                ds = ObjData.FillDataSet("exec pCargaProductosCsv @cmd=2")

                Dim dtConceptosDetalle As New DataTable

                If ds.Tables.Count > 0 Then
                    dtConceptosDetalle = ds.Tables(0)

                End If

                Dim j As Integer = 0
                Dim line As String = ""
                'Itero líneas de csv hasta EOF
                Dim reader As System.IO.StreamReader = New StreamReader(Server.MapPath("../almacen/cargaCsv/") & nombreArchivo, System.Text.Encoding.Default)
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
                    Dim upc As String = ""
                    Dim claveSat As String = ""
                    Dim unidad As String = ""
                    Dim descripcion As String = ""
                    Dim descripcion_corta As String = ""
                    Dim marca As String = 0
                    Dim temporada As String = ""
                    Dim unitario1 As Decimal = 0
                    Dim unitario2 As Decimal = 0
                    Dim unitario3 As Decimal = 0
                    Dim unitario4 As Decimal = 0
                    Dim modelo_estilo As String = ""
                    Dim plataforma As String = ""
                    Dim genero As String = ""
                    Dim tallaUSA As String = ""
                    Dim tallaMX As String = ""
                    Dim color As String = ""
                    Dim colorMX As String = ""
                    Dim material As String = ""
                    Dim peso As String = ""
                    Dim empaque_alto As String = ""
                    Dim empaque_largo As String = ""
                    Dim empaque_ancho As String = ""
                    Dim unidadMedida As String = ""
                    Dim moneda As String = ""
                    Dim claveProdServ As String = ""
                    Dim tasa As String = ""

                    'Marketplace
                    Dim marketPlaceLiverpool As String = ""
                    Dim marketPlaceShopify As String = ""
                    Dim marketPlaceAcctivity As String = ""

                    'se usa cmd para obtener estos valores
                    Dim coleccionId As Integer = 0
                    Dim proyectoId As Integer = 0
                    Dim generoId As Integer = 0
                    Dim marcaId As Integer = 0
                    Dim tasaId As Integer = 0
                    Dim monedaId As Integer = 0
                    Dim objImpId As Integer = 2



                    'ID

                    'skip encabezado
                    If j > 1 Then
                        ' CODIGO
                        Try
                            codigo = line.Split(",")(0)
                        Catch ex As Exception
                            codigo = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            upc = line.Split(",")(1)
                        Catch ex As Exception
                            upc = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            descripcion = line.Split(",")(2)
                        Catch ex As Exception
                            descripcion = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            descripcion_corta = line.Split(",")(3)
                        Catch ex As Exception
                            descripcion_corta = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            marca = line.Split(",")(4)
                        Catch ex As Exception
                            marca = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            temporada = line.Split(",")(5)
                        Catch ex As Exception
                            temporada = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            unitario1 = line.Split(",")(6)
                        Catch ex As Exception
                            unitario1 = 0
                        End Try

                        ' CODIGO REORDEN
                        Try
                            unitario2 = line.Split(",")(7)
                        Catch ex As Exception
                            unitario2 = 0
                        End Try

                        ' CODIGO REORDEN
                        Try
                            unitario3 = line.Split(",")(8)
                        Catch ex As Exception
                            unitario3 = 0
                        End Try

                        ' CODIGO REORDEN
                        Try
                            unitario4 = line.Split(",")(9)
                        Catch ex As Exception
                            unitario4 = 0
                        End Try

                        ' CODIGO REORDEN
                        Try
                            modelo_estilo = line.Split(",")(10)
                        Catch ex As Exception
                            modelo_estilo = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            plataforma = line.Split(",")(11)
                        Catch ex As Exception
                            plataforma = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            genero = line.Split(",")(12)
                        Catch ex As Exception
                            genero = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            tallaUSA = line.Split(",")(13)
                        Catch ex As Exception
                            tallaUSA = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            tallaMX = line.Split(",")(14)
                        Catch ex As Exception
                            tallaMX = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            color = line.Split(",")(15)
                        Catch ex As Exception
                            color = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            colorMX = line.Split(",")(16)
                        Catch ex As Exception
                            colorMX = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            material = line.Split(",")(17)
                        Catch ex As Exception
                            material = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            peso = line.Split(",")(18)
                        Catch ex As Exception
                            peso = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            empaque_alto = line.Split(",")(19)
                        Catch ex As Exception
                            empaque_alto = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            empaque_largo = line.Split(",")(20)
                        Catch ex As Exception
                            empaque_largo = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            empaque_ancho = line.Split(",")(21)
                        Catch ex As Exception
                            empaque_ancho = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            unidadMedida = line.Split(",")(22)
                        Catch ex As Exception
                            unidadMedida = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            claveProdServ = line.Split(",")(23)
                        Catch ex As Exception
                            claveProdServ = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            moneda = line.Split(",")(24)
                        Catch ex As Exception
                            moneda = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            tasa = line.Split(",")(25)
                        Catch ex As Exception
                            tasa = ""
                        End Try

                        claveSat = claveProdServ

                        ' CODIGO REORDEN
                        Try
                            marketPlaceLiverpool = line.Split(",")(26)
                        Catch ex As Exception
                            marketPlaceLiverpool = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            marketPlaceShopify = line.Split(",")(27)
                        Catch ex As Exception
                            marketPlaceShopify = ""
                        End Try

                        ' CODIGO REORDEN
                        Try
                            marketPlaceAcctivity = line.Split(",")(28)
                        Catch ex As Exception
                            marketPlaceAcctivity = ""
                        End Try

                        '???
                        Try
                            Dim rowCodigo() As DataRow = dtConceptosDetalle.Select("codigo = '" & LTrim(RTrim(codigo)).ToString & "'")
                            For Each row As DataRow In rowCodigo
                                codigo = row(0)
                            Next
                        Catch ex As Exception
                            codigo = 0
                        End Try

                        Try
                            Dim rowUpc() As DataRow = dtConceptosDetalle.Select("upc = '" & LTrim(RTrim(upc)).ToString & "'")
                            For Each row As DataRow In rowUpc
                                upc = row(0)
                            Next
                        Catch ex As Exception
                            upc = 0
                        End Try

                        Try
                            Dim rowClaveSat() As DataRow = dtConceptosDetalle.Select("claveSat = '" & LTrim(RTrim(claveSat)).ToString & "'")
                            For Each row As DataRow In rowClaveSat
                                claveSat = row(0)
                            Next
                        Catch ex As Exception
                            claveSat = 0
                        End Try

                        Try
                            Dim rowUnidad() As DataRow = dtConceptosDetalle.Select("unidad = '" & LTrim(RTrim(unidad)).ToString & "'")
                            For Each row As DataRow In rowUnidad
                                unidad = row(0)
                            Next
                        Catch ex As Exception
                            unidad = 0
                        End Try

                        Try
                            Dim rowDescripcion() As DataRow = dtConceptosDetalle.Select("descripcion = '" & LTrim(RTrim(descripcion)).ToString & "'")
                            For Each row As DataRow In rowDescripcion
                                descripcion = row(0)
                            Next
                        Catch ex As Exception
                            descripcion = 0
                        End Try

                        Try
                            Dim rowDescripcion_corta() As DataRow = dtConceptosDetalle.Select("descripcion_corta = '" & LTrim(RTrim(descripcion_corta)).ToString & "'")
                            For Each row As DataRow In rowDescripcion_corta
                                descripcion_corta = row(0)
                            Next
                        Catch ex As Exception
                            descripcion_corta = 0
                        End Try

                        Try
                            Dim rowMarca() As DataRow = dtConceptosDetalle.Select("marca = '" & LTrim(RTrim(marca)).ToString & "'")
                            For Each row As DataRow In rowMarca
                                marca = row(0)
                            Next
                        Catch ex As Exception
                            marca = 0
                        End Try

                        Try
                            Dim rowTemporada() As DataRow = dtConceptosDetalle.Select("temporada = '" & LTrim(RTrim(temporada)).ToString & "'")
                            For Each row As DataRow In rowTemporada
                                temporada = row(0)
                            Next
                        Catch ex As Exception
                            temporada = 0
                        End Try

                        Try
                            Dim rowUnitario1() As DataRow = dtConceptosDetalle.Select("unitario1 = '" & LTrim(RTrim(unitario1)).ToString & "'")
                            For Each row As DataRow In rowUnitario1
                                unitario1 = row(0)
                            Next
                        Catch ex As Exception
                            unitario1 = 0
                        End Try

                        Try
                            Dim rowUnitario2() As DataRow = dtConceptosDetalle.Select("unitario2 = '" & LTrim(RTrim(unitario2)).ToString & "'")
                            For Each row As DataRow In rowUnitario2
                                unitario2 = row(0)
                            Next
                        Catch ex As Exception
                            unitario2 = 0
                        End Try

                        Try
                            Dim rowUnitario3() As DataRow = dtConceptosDetalle.Select("unitario3 = '" & LTrim(RTrim(unitario3)).ToString & "'")
                            For Each row As DataRow In rowUnitario3
                                unitario3 = row(0)
                            Next
                        Catch ex As Exception
                            unitario3 = 0
                        End Try

                        Try
                            Dim rowUnitario4() As DataRow = dtConceptosDetalle.Select("unitario4 = '" & LTrim(RTrim(unitario4)).ToString & "'")
                            For Each row As DataRow In rowUnitario4
                                unitario4 = row(0)
                            Next
                        Catch ex As Exception
                            unitario4 = 0
                        End Try

                        Try
                            Dim rowModelo_estilo() As DataRow = dtConceptosDetalle.Select("modelo_estilo = '" & LTrim(RTrim(modelo_estilo)).ToString & "'")
                            For Each row As DataRow In rowModelo_estilo
                                modelo_estilo = row(0)
                            Next
                        Catch ex As Exception
                            modelo_estilo = 0
                        End Try

                        Try
                            Dim rowPlataforma() As DataRow = dtConceptosDetalle.Select("plataforma = '" & LTrim(RTrim(plataforma)).ToString & "'")
                            For Each row As DataRow In rowPlataforma
                                plataforma = row(0)
                            Next
                        Catch ex As Exception
                            plataforma = 0
                        End Try

                        Try
                            Dim rowGenero() As DataRow = dtConceptosDetalle.Select("genero = '" & LTrim(RTrim(genero)).ToString & "'")
                            For Each row As DataRow In rowGenero
                                genero = row(0)
                            Next
                        Catch ex As Exception
                            genero = 0
                        End Try

                        Try
                            Dim rowTallaUSA() As DataRow = dtConceptosDetalle.Select("tallaUSA = '" & LTrim(RTrim(tallaUSA)).ToString & "'")
                            For Each row As DataRow In rowTallaUSA
                                tallaUSA = row(0)
                            Next
                        Catch ex As Exception
                            tallaUSA = 0
                        End Try

                        Try
                            Dim rowTallaMX() As DataRow = dtConceptosDetalle.Select("tallaMX = '" & LTrim(RTrim(tallaMX)).ToString & "'")
                            For Each row As DataRow In rowTallaMX
                                tallaMX = row(0)
                            Next
                        Catch ex As Exception
                            tallaMX = 0
                        End Try

                        Try
                            Dim rowColor() As DataRow = dtConceptosDetalle.Select("color = '" & LTrim(RTrim(color)).ToString & "'")
                            For Each row As DataRow In rowColor
                                color = row(0)
                            Next
                        Catch ex As Exception
                            color = 0
                        End Try

                        Try
                            Dim rowColorMX() As DataRow = dtConceptosDetalle.Select("colorMX = '" & LTrim(RTrim(colorMX)).ToString & "'")
                            For Each row As DataRow In rowColorMX
                                colorMX = row(0)
                            Next
                        Catch ex As Exception
                            colorMX = 0
                        End Try

                        Try
                            Dim rowMaterial() As DataRow = dtConceptosDetalle.Select("material = '" & LTrim(RTrim(material)).ToString & "'")
                            For Each row As DataRow In rowMaterial
                                material = row(0)
                            Next
                        Catch ex As Exception
                            material = 0
                        End Try

                        Try
                            Dim rowPeso() As DataRow = dtConceptosDetalle.Select("peso = '" & LTrim(RTrim(peso)).ToString & "'")
                            For Each row As DataRow In rowPeso
                                peso = row(0)
                            Next
                        Catch ex As Exception
                            peso = 0
                        End Try

                        Try
                            Dim rowEmpaque_alto() As DataRow = dtConceptosDetalle.Select("empaque_alto = '" & LTrim(RTrim(empaque_alto)).ToString & "'")
                            For Each row As DataRow In rowEmpaque_alto
                                empaque_alto = row(0)
                            Next
                        Catch ex As Exception
                            empaque_alto = 0
                        End Try

                        Try
                            Dim rowEmpaque_largo() As DataRow = dtConceptosDetalle.Select("empaque_largo = '" & LTrim(RTrim(empaque_largo)).ToString & "'")
                            For Each row As DataRow In rowEmpaque_largo
                                empaque_largo = row(0)
                            Next
                        Catch ex As Exception
                            empaque_largo = 0
                        End Try

                        Try
                            Dim rowEmpaque_ancho() As DataRow = dtConceptosDetalle.Select("empaque_ancho = '" & LTrim(RTrim(empaque_ancho)).ToString & "'")
                            For Each row As DataRow In rowEmpaque_ancho
                                empaque_ancho = row(0)
                            Next
                        Catch ex As Exception
                            empaque_ancho = 0
                        End Try

                        Try
                            Dim rowUnidadMedida() As DataRow = dtConceptosDetalle.Select("unidadMedida = '" & LTrim(RTrim(unidadMedida)).ToString & "'")
                            For Each row As DataRow In rowUnidadMedida
                                unidadMedida = row(0)
                            Next
                        Catch ex As Exception
                            unidadMedida = 0
                        End Try

                        Try
                            Dim rowMoneda() As DataRow = dtConceptosDetalle.Select("moneda = '" & LTrim(RTrim(moneda)).ToString & "'")
                            For Each row As DataRow In rowMoneda
                                moneda = row(0)
                            Next
                        Catch ex As Exception
                            moneda = 0
                        End Try

                        Try
                            Dim rowClaveProdServ() As DataRow = dtConceptosDetalle.Select("claveProdServ = '" & LTrim(RTrim(claveProdServ)).ToString & "'")
                            For Each row As DataRow In rowClaveProdServ
                                claveProdServ = row(0)
                            Next
                        Catch ex As Exception
                            claveProdServ = 0
                        End Try

                        Try
                            Dim rowTasa() As DataRow = dtConceptosDetalle.Select("tasa = '" & LTrim(RTrim(tasa)).ToString & "'")
                            For Each row As DataRow In rowTasa
                                tasa = row(0)
                            Next
                        Catch ex As Exception
                            tasa = 0
                        End Try

                        Try
                            Dim rowMarketPlaceLiverpool() As DataRow = dtConceptosDetalle.Select("marketPlaceLiverpool = '" & LTrim(RTrim(marketPlaceLiverpool)).ToString & "'")
                            For Each row As DataRow In rowMarketPlaceLiverpool
                                marketPlaceLiverpool = row(0)
                            Next
                        Catch ex As Exception
                            marketPlaceLiverpool = 0
                        End Try

                        Try
                            Dim rowMarketPlaceShopify() As DataRow = dtConceptosDetalle.Select("marketPlaceShopify = '" & LTrim(RTrim(marketPlaceShopify)).ToString & "'")
                            For Each row As DataRow In rowMarketPlaceShopify
                                marketPlaceShopify = row(0)
                            Next
                        Catch ex As Exception
                            marketPlaceShopify = 0
                        End Try

                        Try
                            Dim rowMarketPlaceAcctivity() As DataRow = dtConceptosDetalle.Select("marketPlaceAcctivity = '" & LTrim(RTrim(marketPlaceAcctivity)).ToString & "'")
                            For Each row As DataRow In rowMarketPlaceAcctivity
                                marketPlaceAcctivity = row(0)
                            Next
                        Catch ex As Exception
                            marketPlaceAcctivity = 0
                        End Try

                        'validaciones
                        proyectoId = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=16, @marca='" & marca & "'")
                        coleccionId = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=17, @temporada='" & temporada & "'")
                        monedaId = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=18, @moneda='" & moneda & "'")
                        generoId = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=19, @genero='" & genero & "'")
                        tasaId = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=20, @tasa='" & tasa & "'")


                        If codigo.Length > 0 Then
                            ' Dim productoid As Long

                            ' Consulto código ya activo
                            codigoActivo = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=14, @codigo='" & codigo & "'")

                            'If codigoNoActivo > 0 Then
                            '    ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=15, @id='" & codigoNoActivo & "'")
                            'End If

                            'productoid = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=4, @codigo='" & codigo & "', @cargaid='" & cargaid.ToString & "'")
                            'codigoActivo = ObjData.RunSQLScalarQuery("exec pCargaProductosCsv @cmd=13, @codigo='" & codigo & "'")

                            'If productoid > 0 Then
                            '    Dim p As New ArrayList
                            '    p.Add(New SqlParameter("@cmd", 7))
                            '    p.Add(New SqlParameter("@cargaid", cargaid))
                            '    p.Add(New SqlParameter("@codigo", codigo))
                            '    p.Add(New SqlParameter("@cantidad", cantidad))
                            '    p.Add(New SqlParameter("@error", "El código " & codigo & " ya se encuentra registrado."))
                            '    p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                            '    ObjData.ExecuteNonQueryWithParams("pCargaProductosCsv", p)
                            '    registros_error = registros_error + 1

                            'ElseIf codigoNoActivo = 0 Then
                            '    Dim p As New ArrayList
                            '    p.Add(New SqlParameter("@cmd", 7))
                            '    p.Add(New SqlParameter("@cargaid", cargaid))
                            '    p.Add(New SqlParameter("@codigo", codigo))
                            '    p.Add(New SqlParameter("@cantidad", cantidad))
                            '    p.Add(New SqlParameter("@error", "El código " & codigo & "Producto no activo."))
                            '    ObjData.ExecuteNonQueryWithParams("pCargaProductosCsv", p)
                            '    registros_error = registros_error + 1

                            If codigoActivo >= 1 Then
                                ' Marco error
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 7))
                                p.Add(New SqlParameter("@cargaid", cargaid))

                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@upc", upc))
                                p.Add(New SqlParameter("@unidad", unidad))
                                p.Add(New SqlParameter("@descripcion", descripcion))
                                p.Add(New SqlParameter("@descripcion_corta", descripcion_corta))
                                p.Add(New SqlParameter("@marca", marca))
                                p.Add(New SqlParameter("@temporada", temporada))
                                p.Add(New SqlParameter("@coleccionId", coleccionId))
                                p.Add(New SqlParameter("@unitario1", unitario1))
                                p.Add(New SqlParameter("@unitario2", unitario2))
                                p.Add(New SqlParameter("@unitario3", unitario3))
                                p.Add(New SqlParameter("@unitario4", unitario4))
                                p.Add(New SqlParameter("@modelo_estilo", modelo_estilo))
                                p.Add(New SqlParameter("@plataforma", plataforma))
                                p.Add(New SqlParameter("@genero", genero))
                                p.Add(New SqlParameter("@tallaUSA", tallaUSA))
                                p.Add(New SqlParameter("@tallaMX", tallaMX))
                                p.Add(New SqlParameter("@color", color))
                                p.Add(New SqlParameter("@colorMX", colorMX))
                                p.Add(New SqlParameter("@material", material))
                                p.Add(New SqlParameter("@peso", peso))
                                p.Add(New SqlParameter("@empaque_alto", empaque_alto))
                                p.Add(New SqlParameter("@empaque_largo", empaque_largo))
                                p.Add(New SqlParameter("@empaque_ancho", empaque_ancho))
                                p.Add(New SqlParameter("@unidadMedida", unidadMedida))
                                p.Add(New SqlParameter("@claveProdServ", claveProdServ))
                                p.Add(New SqlParameter("@moneda", moneda))
                                p.Add(New SqlParameter("@tasa", tasa))
                                p.Add(New SqlParameter("@claveSat", claveSat))


                                p.Add(New SqlParameter("@marketPlaceLiverpool", Trim(marketPlaceLiverpool)))
                                p.Add(New SqlParameter("@marketPlaceShopify", Trim(marketPlaceShopify)))
                                p.Add(New SqlParameter("@marketPlaceAcctivity", Trim(marketPlaceAcctivity)))


                                p.Add(New SqlParameter("@error", "El código " & codigo & " ya está registrado."))
                                ' p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaProductosCsv", p)
                                registros_error = registros_error + 1

                            Else
                                ' inserción
                                Dim p As New ArrayList
                                p.Add(New SqlParameter("@cmd", 3))
                                p.Add(New SqlParameter("@cargaid", cargaid))

                                p.Add(New SqlParameter("@codigo", codigo))
                                p.Add(New SqlParameter("@upc", upc))
                                p.Add(New SqlParameter("@unidad", unidad))
                                p.Add(New SqlParameter("@descripcion", descripcion))
                                p.Add(New SqlParameter("@descripcion_corta", descripcion_corta))
                                p.Add(New SqlParameter("@marca", marca))
                                p.Add(New SqlParameter("@temporada", temporada))
                                p.Add(New SqlParameter("@coleccionId", coleccionId))
                                p.Add(New SqlParameter("@unitario1", unitario1))
                                p.Add(New SqlParameter("@unitario2", unitario2))
                                p.Add(New SqlParameter("@unitario3", unitario3))
                                p.Add(New SqlParameter("@unitario4", unitario4))
                                p.Add(New SqlParameter("@modelo_estilo", modelo_estilo))
                                p.Add(New SqlParameter("@plataforma", plataforma))
                                p.Add(New SqlParameter("@genero", genero))
                                p.Add(New SqlParameter("@tallaUSA", tallaUSA))
                                p.Add(New SqlParameter("@tallaMX", tallaMX))
                                p.Add(New SqlParameter("@color", color))
                                p.Add(New SqlParameter("@colorMX", colorMX))
                                p.Add(New SqlParameter("@material", material))
                                p.Add(New SqlParameter("@peso", peso))
                                p.Add(New SqlParameter("@empaque_alto", empaque_alto))
                                p.Add(New SqlParameter("@empaque_largo", empaque_largo))
                                p.Add(New SqlParameter("@empaque_ancho", empaque_ancho))
                                p.Add(New SqlParameter("@unidadMedida", unidadMedida))
                                p.Add(New SqlParameter("@claveProdServ", claveProdServ))
                                p.Add(New SqlParameter("@moneda", moneda))
                                p.Add(New SqlParameter("@tasa", tasa))
                                p.Add(New SqlParameter("@claveSat", claveSat))

                                p.Add(New SqlParameter("@marketPlaceLiverpool", Trim(marketPlaceLiverpool)))
                                p.Add(New SqlParameter("@marketPlaceShopify", Trim(marketPlaceShopify)))
                                p.Add(New SqlParameter("@marketPlaceAcctivity", Trim(marketPlaceAcctivity)))

                                'p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                                ObjData.ExecuteNonQueryWithParams("pCargaProductosCsv", p)
                                registros_correctos = registros_correctos + 1
                            End If
                        Else

                            'Agrego error de código es requerido 
                            msgerror = ""
                            If codigo.Length = 0 Then
                                msgerror = msgerror & "Código es requerido."
                            End If

                            'If cantidad = 0 Then
                            '    msgerror = msgerror & vbCrLf & "Cantidad es requerida."
                            'End If

                            If codigoId.Length = 0 Then
                                msgerror = msgerror & vbCrLf & "Codigo no registrado."
                            End If

                            'If codigoNoActivo = 0 Then
                            '    msgerror = msgerror & vbCrLf & "Producto no activo."
                            'End If

                            'If unitario = 0 Then
                            '    msgerror = msgerror & vbCrLf & "Precio unitario es requerido."
                            'End If
                            Dim p As New ArrayList
                            p.Add(New SqlParameter("@cmd", 7))
                            p.Add(New SqlParameter("@cargaid", cargaid))

                            p.Add(New SqlParameter("@codigo", codigo))
                            p.Add(New SqlParameter("@upc", upc))
                            p.Add(New SqlParameter("@unidad", unidad))
                            p.Add(New SqlParameter("@descripcion", descripcion))
                            p.Add(New SqlParameter("@descripcion_corta", descripcion_corta))
                            p.Add(New SqlParameter("@marca", marca))
                            p.Add(New SqlParameter("@temporada", temporada))
                            p.Add(New SqlParameter("@coleccionId", coleccionId))
                            p.Add(New SqlParameter("@unitario1", unitario1))
                            p.Add(New SqlParameter("@unitario2", unitario2))
                            p.Add(New SqlParameter("@unitario3", unitario3))
                            p.Add(New SqlParameter("@unitario4", unitario4))
                            p.Add(New SqlParameter("@modelo_estilo", modelo_estilo))
                            p.Add(New SqlParameter("@plataforma", plataforma))
                            p.Add(New SqlParameter("@genero", genero))
                            p.Add(New SqlParameter("@tallaUSA", tallaUSA))
                            p.Add(New SqlParameter("@tallaMX", tallaMX))
                            p.Add(New SqlParameter("@color", color))
                            p.Add(New SqlParameter("@colorMX", colorMX))
                            p.Add(New SqlParameter("@material", material))
                            p.Add(New SqlParameter("@peso", peso))
                            p.Add(New SqlParameter("@empaque_alto", empaque_alto))
                            p.Add(New SqlParameter("@empaque_largo", empaque_largo))
                            p.Add(New SqlParameter("@empaque_ancho", empaque_ancho))
                            p.Add(New SqlParameter("@unidadMedida", unidadMedida))
                            p.Add(New SqlParameter("@claveProdServ", claveProdServ))
                            p.Add(New SqlParameter("@moneda", moneda))
                            p.Add(New SqlParameter("@tasa", tasa))
                            p.Add(New SqlParameter("@claveSat", claveSat))

                            p.Add(New SqlParameter("@marketPlaceLiverpool", Trim(marketPlaceLiverpool)))
                            p.Add(New SqlParameter("@marketPlaceShopify", Trim(marketPlaceShopify)))
                            p.Add(New SqlParameter("@marketPlaceAcctivity", Trim(marketPlaceAcctivity)))

                            p.Add(New SqlParameter("@error", msgerror))
                            'p.Add(New SqlParameter("@ordenId", Request("id").ToString))
                            ObjData.ExecuteNonQueryWithParams("pCargaProductosCsv", p)
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
                    ObjData.ExecuteNonQueryWithParams("pCargaProductosCsv", p)
                End If

                If registros_error > 0 Then
                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 9))
                    p.Add(New SqlParameter("@cargaid", cargaid))
                    p.Add(New SqlParameter("@registros_error", registros_error))
                    ObjData.ExecuteNonQueryWithParams("pCargaProductosCsv", p)

                    panelErrores.Visible = True
                    erroresList.DataSource = ObjData.FillDataSet("exec pCargaProductosCsv @cmd=10, @cargaid='" & cargaid.ToString & "'")
                    erroresList.DataBind()

                End If

                If registros_correctos = 0 Then
                    ObjData.RunSQLQuery("exec pCargaProductosCsv @cmd=11, @cargaid='" & cargaid.ToString & "'")
                End If

                If registros_correctos > 0 And registros_error > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.<br>" & registros_error.ToString & " con error, favor de verificar.", 350, 200, "Alerta", "", "")
                ElseIf registros_correctos > 0 Then
                    rwAlerta.RadAlert("Se cargaron " & registros_correctos.ToString & " productos con éxito.", 350, 200, "Alerta", "", "")
                ElseIf registros_error > 0 Then
                    rwAlerta.RadAlert("Se encontraron " & registros_error.ToString & " productos con error, favor de verificar.", 350, 200, "Alerta", "", "")
                End If

                ObjData = Nothing
            Else
                rwAlerta.RadAlert("Formato CSV no válido.", 350, 200, "Alerta", "", "")
            End If
        Else
            rwAlerta.RadAlert("Selecciona un archivo en formato CSV.", 350, 200, "Alerta", "", "")
        End If

        panelCSV.Visible = True
        'btnAgregaConceptos.Visible = True
        btnAddorderParcial.Enabled = True
        Dim ObjData2 As New DataControl
        ds = ObjData2.FillDataSet("exec pCargaProductosCsv @cmd=5, @cargaid='" & cargaidHidden.Value & "'")
        Dim lineas As Integer = ds.Tables(0).Rows.Count
        If lineas = 0 Then
            lineas = 1
        End If
        resultslistCSV.PageSize = lineas
        resultslistCSV.DataSource = ds
        resultslistCSV.DataBind()
        ObjData2.RunSQLQuery("exec pCargaProductosCsv @cmd=12")

        ObjData2 = Nothing
    End Sub

    Private Sub imgDownload_Click(sender As Object, e As ImageClickEventArgs) Handles imgDownload.Click
        Dim FilePath = Server.MapPath("~/portalcfd/almacen/cargaCsv/") & "FORMATOCARGA.csv"

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
    Private Sub erroresList_NeedDataSource(sender As Object, e As GridNeedDataSourceEventArgs) Handles erroresList.NeedDataSource
        Dim ObjData As New DataControl
        erroresList.DataSource = ObjData.FillDataSet("exec pCargaProductosCsv @cmd=10, @cargaid='" & cargaidHidden.Value.ToString & "'")
        ObjData = Nothing
    End Sub
    Protected Sub btnAddorderParcial_Click(sender As Object, e As EventArgs) Handles btnAddorderParcial.Click

        Try
            Dim ObjData2 As New DataControl

            '
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
            Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")
            '


            Dim ObjData As New DataControl
            Dim codigo As String = ""
            Dim upc As String = ""
            Dim descripcion As String = ""
            Dim descripcion_corta As String = ""
            Dim marca As String = ""
            Dim temporada As String = ""
            Dim unitario1 As Double = 0.00
            Dim unitario2 As Double = 0.00
            Dim unitario3 As Double = 0.00
            Dim unitario4 As Double = 0.00
            Dim modelo_estilo As String = ""
            Dim plataforma As String = ""
            Dim genero As String = ""
            Dim tallaUSA As String = ""
            Dim tallaMX As String = ""
            Dim color As String = ""
            Dim colorMX As String = ""
            Dim material As String = ""
            Dim peso As String = ""
            Dim empaque_alto As String = ""
            Dim empaque_largo As String = ""
            Dim empaque_ancho As String = ""
            Dim unidadMedida As String = ""
            Dim claveProdServ As String = ""
            Dim moneda As String = ""
            Dim tasa As String = ""
            Dim claveSat As String = ""
            Dim proyectoId As Integer = 0
            Dim coleccionId As Integer = 0
            Dim monedaId As Integer = 0
            Dim tasaId As Integer = 0
            Dim generoId As Integer = 0
            Dim objImpId As Integer = 0
            Dim marketPlaceLiverpool As String = ""
            Dim marketPlaceShopify As String = ""
            Dim marketPlaceAcctivity As String = ""

            For Each row As GridDataItem In resultslistCSV.MasterTableView.Items

                'conceptoId = row.GetDataKeyValue("id")
                codigo = row.GetDataKeyValue("codigo")
                upc = row.GetDataKeyValue("upc")
                descripcion = row.GetDataKeyValue("descripcion")
                descripcion_corta = row.GetDataKeyValue("descripcion_corta")
                marca = row.GetDataKeyValue("marca")
                temporada = row.GetDataKeyValue("temporada")
                unitario1 = row.GetDataKeyValue("unitario1")
                unitario2 = row.GetDataKeyValue("unitario2")
                unitario3 = row.GetDataKeyValue("unitario3")
                unitario4 = row.GetDataKeyValue("unitario4")
                modelo_estilo = row.GetDataKeyValue("modelo_estilo")
                plataforma = row.GetDataKeyValue("plataforma")
                genero = row.GetDataKeyValue("genero")
                tallaUSA = row.GetDataKeyValue("tallaUSA")
                tallaMX = row.GetDataKeyValue("tallaMX")
                color = row.GetDataKeyValue("color")
                colorMX = row.GetDataKeyValue("colorMX")
                material = row.GetDataKeyValue("material")
                peso = row.GetDataKeyValue("peso")
                empaque_alto = row.GetDataKeyValue("empaque_alto")
                empaque_largo = row.GetDataKeyValue("empaque_largo")
                empaque_ancho = row.GetDataKeyValue("empaque_ancho")
                unidadMedida = row.GetDataKeyValue("unidadMedida")
                claveProdServ = row.GetDataKeyValue("claveProdServ")
                moneda = row.GetDataKeyValue("moneda")
                tasa = row.GetDataKeyValue("tasa")
                claveSat = row.GetDataKeyValue("claveSat")
                proyectoId = row.GetDataKeyValue("proyectoId")
                coleccionId = row.GetDataKeyValue("coleccionId")
                monedaId = row.GetDataKeyValue("monedaId")
                tasaId = row.GetDataKeyValue("tasaId")
                generoId = row.GetDataKeyValue("generoId")
                objImpId = row.GetDataKeyValue("objImpId")
                marketPlaceLiverpool = row.GetDataKeyValue("marketPlaceLiverpool")
                marketPlaceShopify = row.GetDataKeyValue("marketPlaceShopify")
                marketPlaceAcctivity = row.GetDataKeyValue("marketPlaceAcctivity")

                'Filtro unidad quito guiòn solo tomo primeras letras
                unidadMedida = Split(unidadMedida, "-")(0)
                unidadMedida = Trim(unidadMedida)

                If codigo.Length > 0 Then

                    Dim p As New ArrayList
                    p.Add(New SqlParameter("@cmd", 19))
                    p.Add(New SqlParameter("@codigo", codigo.ToString))
                    p.Add(New SqlParameter("@upc", upc.ToString))
                    p.Add(New SqlParameter("@descripcion", descripcion.ToString))
                    p.Add(New SqlParameter("@descripcion_corta", descripcion_corta.ToString))
                    p.Add(New SqlParameter("@claveunidad", unidadMedida.ToString))

                    p.Add(New SqlParameter("@genero", genero.ToString))
                    p.Add(New SqlParameter("@generoId", generoId.ToString))

                    p.Add(New SqlParameter("@unitario", unitario1.ToString))
                    p.Add(New SqlParameter("@unitario2", unitario2.ToString))
                    p.Add(New SqlParameter("@unitario3", unitario3.ToString))
                    p.Add(New SqlParameter("@unitario4", unitario4.ToString))

                    p.Add(New SqlParameter("@tasaId", tasaId.ToString))
                    p.Add(New SqlParameter("@monedaId", monedaId.ToString))

                    p.Add(New SqlParameter("@coleccionId", coleccionId.ToString))
                    p.Add(New SqlParameter("@proyectoId", proyectoId.ToString))

                    p.Add(New SqlParameter("@claveprodserv", claveProdServ.ToString))


                    p.Add(New SqlParameter("@objImpId", objImpId.ToString))

                    p.Add(New SqlParameter("@modeloEstilo", modelo_estilo.ToString))

                    p.Add(New SqlParameter("@plataforma", plataforma.ToString))

                    'p.Add(New SqlParameter("@temporada", temporada.ToString))
                    p.Add(New SqlParameter("@tallaUSA", tallaUSA.ToString))
                    p.Add(New SqlParameter("@tallaMX", tallaMX.ToString))
                    p.Add(New SqlParameter("@color", color.ToString))
                    p.Add(New SqlParameter("@colorMX", colorMX.ToString))
                    p.Add(New SqlParameter("@material", material.ToString))
                    p.Add(New SqlParameter("@peso", peso.ToString))
                    p.Add(New SqlParameter("@empaqueAlto", empaque_alto.ToString))
                    p.Add(New SqlParameter("@empaqueLargo", empaque_largo.ToString))
                    p.Add(New SqlParameter("@empaqueAncho", empaque_ancho.ToString))

                    'p.Add(New SqlParameter("@unidadMedida", unidadMedida.ToString))
                    'p.Add(New SqlParameter("@moneda", moneda.ToString))
                    'p.Add(New SqlParameter("@tasa", tasa.ToString))
                    'p.Add(New SqlParameter("@claveSat", claveSat.ToString))

                    p.Add(New SqlParameter("@marketPlaceLiverpool", marketPlaceLiverpool.ToString))
                    p.Add(New SqlParameter("@marketPlaceShopify", marketPlaceShopify.ToString))
                    p.Add(New SqlParameter("@marketPlaceAcctivity", marketPlaceAcctivity.ToString))

                    ObjData.ExecuteNonQueryWithParams("pMisProductos", p)
                    lblMensajeCSV.Text = "Poductos Guardados."
                End If

            Next        '

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
            Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")
            '

            '

            'Call CargaConceptos()
            'Call CargaListaOrdenesParciales()
            panelCSV.Visible = False
            panelErrores.Visible = False

            resultslistCSV.DataSource = Nothing
            resultslistCSV.DataBind()
            erroresList.DataSource = Nothing
            erroresList.DataBind()

        Catch ex As Exception
            lblMensajeCSV.Text = "Error: " & ex.Message.ToString
        Finally
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
