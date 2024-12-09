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


Partial Public Class Facturar_Extended40

    '''<summary>
    '''Control panelClients.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelClients As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control imgPanel1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents imgPanel1 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lblClientsSelectionLegend.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblClientsSelectionLegend As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control valClienteID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valClienteID As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control ValAlmacen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ValAlmacen As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valProyecto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valProyecto As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cmbCliente.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbCliente As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbAlmacen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbAlmacen As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbSucursal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbSucursal As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbProyecto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbProyecto As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control valTipoDocumento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valTipoDocumento As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valMetodoPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valMetodoPago As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valMoneda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valMoneda As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valTipoCambio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valTipoCambio As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cmbTipoDocumento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbTipoDocumento As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbMetodoPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbMetodoPago As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbMoneda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbMoneda As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control txtTipoCambio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtTipoCambio As Global.Telerik.Web.UI.RadNumericTextBox

    '''<summary>
    '''Control txtOrdenCompra.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtOrdenCompra As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control valExportacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valExportacion As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cmbExportacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbExportacion As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblTipoRelacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTipoRelacion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblUUID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblUUID As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbTipoRelacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbTipoRelacion As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbUUID.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbUUID As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control btnAddUiid.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAddUiid As Global.Telerik.Web.UI.RadButton

    '''<summary>
    '''Control valTipoRelecion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valTipoRelecion As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valFolioFiscal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valFolioFiscal As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control tblRelacionados.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents tblRelacionados As Global.Telerik.Web.UI.RadGrid

    '''<summary>
    '''Control panelSpecificClient.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelSpecificClient As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Image1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image1 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lblClientData.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblClientData As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblSocialReason.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSocialReason As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblContact.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblContact As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblCondiciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCondiciones As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblSocialReasonValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSocialReasonValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblContactValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblContactValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbCondiciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbCondiciones As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblContactPhone.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblContactPhone As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblRFC.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRFC As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblTipoPrecio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTipoPrecio As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblContactPhoneValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblContactPhoneValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblRFCValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRFCValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblTipoPrecioValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTipoPrecioValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblUsoCFDI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblUsoCFDI As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFormaPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFormaPago As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbUsoCFD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbUsoCFD As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbFormaPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbFormaPago As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control valUsoCFDI.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valUsoCFDI As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valFormaPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valFormaPago As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control lblObservaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblObservaciones As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblNumCtaPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNumCtaPago As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtObservaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtObservaciones As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control txtNumCtaPago.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumCtaPago As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control chkAduana.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents chkAduana As Global.System.Web.UI.WebControls.CheckBox

    '''<summary>
    '''Control lblComplementoFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblComplementoFactura As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbComplementoFactura.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbComplementoFactura As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control panelInformacionAduanera.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelInformacionAduanera As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control valNombreAduana.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valNombreAduana As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control nombreaduana.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents nombreaduana As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control valFechaPedimento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valFechaPedimento As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control fechapedimento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents fechapedimento As Global.Telerik.Web.UI.RadDatePicker

    '''<summary>
    '''Control valNumeroPedimento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valNumeroPedimento As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control numeropedimento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents numeropedimento As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control panelFacturaGlobal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelFacturaGlobal As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Image6.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image6 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control Label2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label2 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control valcmbPeriodicidad.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valcmbPeriodicidad As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valcmbMes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valcmbMes As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control valtxtAnio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents valtxtAnio As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control cmbPeriodicidad.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbPeriodicidad As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control cmbMes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbMes As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control txtAnio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtAnio As Global.Telerik.Web.UI.RadNumericTextBox

    '''<summary>
    '''Control panelComplementoDetallista.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelComplementoDetallista As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Image5.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image5 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control Label1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label1 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDatosGeneralesComplementos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDatosGeneralesComplementos As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDatosCompradorVendedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDatosCompradorVendedor As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDatosDelComprador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDatosDelComprador As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFechaOrdenCompra.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaOrdenCompra As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control calFechaOrdenCompra.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents calFechaOrdenCompra As Global.Telerik.Web.UI.RadDatePicker

    '''<summary>
    '''Control lblGLNComprador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblGLNComprador As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtGLNComprador.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtGLNComprador As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblNumeroReferenciaAdicional.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNumeroReferenciaAdicional As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroDeReferenciaAdicional.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroDeReferenciaAdicional As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblNumeroDepartamento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNumeroDepartamento As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroDepartamento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroDepartamento As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblNumeroContraRecibo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNumeroContraRecibo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroDeContraRecibo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroDeContraRecibo As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblDatosDelVendedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDatosDelVendedor As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFechaContraRecibo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaContraRecibo As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control calFechaContraRecibo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents calFechaContraRecibo As Global.Telerik.Web.UI.RadDatePicker

    '''<summary>
    '''Control lblGLNVendedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblGLNVendedor As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtGLNVendedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtGLNVendedor As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblNumeroEmisor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNumeroEmisor As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroEmisor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroEmisor As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control panelComercioExterior.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelComercioExterior As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Image8.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image8 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control Label10.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label10 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblTipoOperacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTipoOperacion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbTipoOperacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbTipoOperacion As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblClavePedimento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblClavePedimento As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbClavePedimento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbClavePedimento As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblCertificadoOrigen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCertificadoOrigen As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbCertificadoOrigen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbCertificadoOrigen As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblNumeroCertificadoOrigen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNumeroCertificadoOrigen As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroCertificadoOrigen.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroCertificadoOrigen As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblNumeroExportadorConfiable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNumeroExportadorConfiable As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroExportadorConfiable.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroExportadorConfiable As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblClaveINCOTERM.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblClaveINCOTERM As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control cmbClaveIncoterm.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents cmbClaveIncoterm As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control lblTipoCambioAddenda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTipoCambioAddenda As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtTipoCambioAddenda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtTipoCambioAddenda As Global.Telerik.Web.UI.RadNumericTextBox

    '''<summary>
    '''Control lblTotalUSD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTotalUSD As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtTotalUSD.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtTotalUSD As Global.Telerik.Web.UI.RadNumericTextBox

    '''<summary>
    '''Control panelAddendaCoppel.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelAddendaCoppel As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Image7.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image7 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control Label3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label3 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control Label4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label4 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control Label5.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label5 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control Label6.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label6 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblFechaPromesaEntrega.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblFechaPromesaEntrega As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control calFechaPromesaEntrega.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents calFechaPromesaEntrega As Global.Telerik.Web.UI.RadDatePicker

    '''<summary>
    '''Control Label7.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label7 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control calFechaOrdenCompraDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents calFechaOrdenCompraDS As Global.Telerik.Web.UI.RadDatePicker

    '''<summary>
    '''Control Label8.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label8 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtGLNCompradorDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtGLNCompradorDS As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control Label9.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label9 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroDeReferenciaAdicionalDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroDeReferenciaAdicionalDS As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblCantidadLotes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblCantidadLotes As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtCantidadLotes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtCantidadLotes As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control Label11.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label11 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblBodegaDestino.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblBodegaDestino As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtBodegaDestino.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtBodegaDestino As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control Label13.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label13 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtGLNVendedorDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtGLNVendedorDS As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control lblBodegaReceptora.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblBodegaReceptora As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtBodegaReceptora.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtBodegaReceptora As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control Label14.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Label14 As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtNumeroEmisorDS.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroEmisorDS As Global.Telerik.Web.UI.RadTextBox

    '''<summary>
    '''Control btnAddenda.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAddenda As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control panelItemsRegistration.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelItemsRegistration As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control productoid.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents productoid As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control Image2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image2 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lblClientItems.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblClientItems As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtSearchItem.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtSearchItem As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control gridResults.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gridResults As Global.Telerik.Web.UI.RadGrid

    '''<summary>
    '''Control lblMensaje.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblMensaje As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control btnCancelSearch.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancelSearch As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control btnAgregaConceptos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAgregaConceptos As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control itemsList.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents itemsList As Global.Telerik.Web.UI.RadGrid

    '''<summary>
    '''Control panelDescuento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelDescuento As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Image4.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image4 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lblDescDescuento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDescDescuento As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDescuentoGeneral.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDescuentoGeneral As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control txtDescuento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtDescuento As Global.Telerik.Web.UI.RadNumericTextBox

    '''<summary>
    '''Control btnAplicarDescuento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAplicarDescuento As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control RequiredFieldValidator1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RequiredFieldValidator1 As Global.System.Web.UI.WebControls.RequiredFieldValidator

    '''<summary>
    '''Control panelResume.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelResume As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control Image3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents Image3 As Global.System.Web.UI.WebControls.Image

    '''<summary>
    '''Control lblResume.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblResume As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblSubTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSubTotal As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblSubTotalValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblSubTotalValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblImporteTasaCero.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblImporteTasaCero As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblImporteTasaCeroValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblImporteTasaCeroValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDescuento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDescuento As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblDescuentoValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblDescuentoValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblIVA.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblIVA As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblIVAValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblIVAValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control panelRetencion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents panelRetencion As Global.System.Web.UI.WebControls.Panel

    '''<summary>
    '''Control lblRet.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRet As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblRetValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblRetValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblTotal.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTotal As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblTotalValue.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTotalValue As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control btnCreateInvoice.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCreateInvoice As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control btnCancelInvoice.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnCancelInvoice As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control RadWindow1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents RadWindow1 As Global.Telerik.Web.UI.RadWindow

    '''<summary>
    '''Control txtErrores.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtErrores As Global.System.Web.UI.WebControls.TextBox

    '''<summary>
    '''Control btnAceptar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAceptar As Global.System.Web.UI.WebControls.Button
End Class
