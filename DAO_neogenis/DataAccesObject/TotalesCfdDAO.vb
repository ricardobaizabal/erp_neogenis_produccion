Imports System.Data.SqlClient

Public Class TotalesCfdDAO

    Public Sub Main()

    End Sub
    Public Function getById(ByVal cfdid As Integer) As TotalesCfdDTO
        Dim totalDto As New TotalesCfdDTO
        Dim conn As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmd As New SqlCommand("exec pCFD @cmd=16, @cfdid='" & cfdid & "'", conn)
        Try
            conn.Open()
            Dim rs As SqlDataReader
            rs = cmd.ExecuteReader()
            If rs.Read Then
                totalDto.TieneIva16 = CBool(rs("tieneIva16"))
                totalDto.TieneIvaTasaCero = rs("tieneIvaTasaCero")
                totalDto.Subtotal = rs("importe")
                totalDto.Iva = rs("iva")
                totalDto.Descuento = rs("totaldescuento")
                totalDto.Total = rs("total")
            End If

        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            conn.Close()
            conn.Dispose()
            conn = Nothing
        End Try
        Return totalDto
    End Function

End Class
