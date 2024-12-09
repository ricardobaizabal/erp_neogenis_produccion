Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://erp.natural.gs/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Etiquetas
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function GetEtiquetaDeCajasByPedidoid(ByVal pedidoid As Integer) As DataSet
        Dim pedidodata As New DataSet
        Dim obj As New DataControl
        pedidodata = obj.FillDataSet("EXEC pPedidos @cmd=30, @pedidoid=" & pedidoid)
        Return pedidodata
    End Function

End Class