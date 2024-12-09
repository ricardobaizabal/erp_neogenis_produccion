Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class Stocklocation
    Inherits System.Web.UI.Page
    Private ds As DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Not IsPostBack Then

            Dim objCat As New DataControl
            objCat.Catalogo(filtromarcaid, "select id, nombre from tblProyecto order by nombre", 0)
            objCat.Catalogo(filtrocoleccionid, "select id, isnull(codigo,'') + ' - ' + isnull(nombre,'') as nombre from tblColeccion where isnull(borradoBit,0)=0 order by nombre", 0)

            objCat = Nothing
        End If
    End Sub

#Region "Load List Of Products"

    Function GetProducts() As DataSet
        ds = New DataSet
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlDataAdapter("EXEC pMisProductos @cmd=17, @txtSearch='" & txtSearch.Text & "', @proyectoid='" & filtromarcaid.SelectedValue.ToString & "', @coleccionid='" & filtrocoleccionid.SelectedValue.ToString & "', @upcSearch='" & upcSearch.Text & "', @clienteid='" & Session("clienteid") & "', @asignados='" & ckfiltroAsignado.Checked & "'", conn)
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        productslist.MasterTableView.NoMasterRecordsText = "No existen productos asignados."
        ds = GetProducts()
        productslist.DataSource = ds
        productslist.DataBind()
    End Sub

    Protected Sub btnAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAll.Click
        txtSearch.Text = ""
        filtrocoleccionid.SelectedValue = 0
        filtromarcaid.SelectedValue = 0
        productslist.MasterTableView.NoMasterRecordsText = "No existen productos asignados."
        ds = GetProducts()
        productslist.DataSource = ds
        productslist.DataBind()
    End Sub


#Region "Telerik Grid Products Loading Events"

    Protected Sub productslist_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles productslist.NeedDataSource

        If Not e.IsFromDetailTable Then
            productslist.MasterTableView.NoMasterRecordsText = "No existen productos asignados."

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

End Class