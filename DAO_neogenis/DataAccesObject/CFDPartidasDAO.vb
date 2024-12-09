Public Class CFDPartidasDAO
    Private _id As Integer
    Private _datControl As New DataControl
    Public Sub New(ByVal id As Integer)
        _id = id
    End Sub
    Public Sub ActulizaTalla(ByVal talla As Decimal)
        _datControl.RunSQLQuery("EXEC pCFD_Partidas @cmd=6, @id=" & _id & ", @talla=" & talla)
    End Sub
    Public Sub ActulizaCajas(ByVal cajas As Decimal)
        _datControl.RunSQLQuery("EXEC pCFD_Partidas @cmd=7, @id=" & _id & ", @cajas=" & cajas)
    End Sub
    Public Sub ActulizaPiezasPorCaja(ByVal piezasporcaja As Decimal)
        _datControl.RunSQLQuery("EXEC pCFD_Partidas @cmd=8, @id=" & _id & ", @piezasporcaja=" & piezasporcaja)
    End Sub
End Class
