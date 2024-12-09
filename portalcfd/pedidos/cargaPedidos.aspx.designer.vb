'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class cargaPedidos

    '''<summary>
    '''Control tblProductos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tblProductos As Global.System.Web.UI.HtmlControls.HtmlTable

    '''<summary>
    '''Control lblPedidoscsv.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPedidoscsv As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control fileUploadPedido.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fileUploadPedido As Global.System.Web.UI.WebControls.FileUpload

    '''<summary>
    '''Control btnCargaPedidosCsv.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCargaPedidosCsv As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control imgdownloadF.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgdownloadF As Global.System.Web.UI.WebControls.ImageButton

    '''<summary>
    '''Control lblMensaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblMensaje As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cargaidHidden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cargaidHidden As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control panelCSVPedidos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelCSVPedidos As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lblConceptosCargaPedidos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblConceptosCargaPedidos As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control resultslistCSVPedidos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents resultslistCSVPedidos As Global.Telerik.Web.UI.RadGrid

    '''<summary>
    '''Control panelErroresPedidos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelErroresPedidos As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Label2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control erroresListPedidos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents erroresListPedidos As Global.Telerik.Web.UI.RadGrid

    '''<summary>
    '''Control cargaidHidden2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cargaidHidden2 As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control panel1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panel1 As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lblProdsTitulo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblProdsTitulo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control productosList.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents productosList As Global.Telerik.Web.UI.RadGrid

    '''<summary>
    '''Control btnAgregaConceptos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAgregaConceptos As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control lblPedidoError.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblPedidoError As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control RadWindowManager1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RadWindowManager1 As Global.Telerik.Web.UI.RadWindowManager

    '''<summary>
    '''Control HiddenButtonOk.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents HiddenButtonOk As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control HiddenButtonCancel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents HiddenButtonCancel As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control rwAlerta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents rwAlerta As Global.Telerik.Web.UI.RadWindowManager
End Class
