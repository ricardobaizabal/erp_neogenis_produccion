Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

#Region "Class Specifications"

'''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''This Class Fill A RadComboBox From Telerik With A Specific Data Source''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
''The Method Receives: '''''''''''''''''''''''''''''''''''''''''''''''''''
''The Name Of The Telerik RadComboBox & The SQL Command To Execute''''''''
''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

#End Region

Public Class FillRadComboBox

    Private connection_str As String = System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString

    Public Sub FillRadComboBox(ByVal RadComboBox As RadComboBox, ByVal SQLCommand As String)

        Dim conn As New SqlConnection(connection_str)
        Dim cmd As SqlDataAdapter = New SqlDataAdapter(SQLCommand, connection_str)

        conn.Open()

        Dim ds As New DataSet
        cmd.Fill(ds)

        RadComboBox.DataSource = ds.Tables(0)
        RadComboBox.DataTextField = ds.Tables(0).Columns(1).ColumnName.ToString()
        RadComboBox.DataValueField = ds.Tables(0).Columns(0).ColumnName.ToString()
        RadComboBox.DataBind()

        conn.Close()
        conn.Dispose()
        conn = Nothing

    End Sub

End Class
