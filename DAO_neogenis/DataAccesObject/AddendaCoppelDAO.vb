Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading
Imports System.Xml
Imports Microsoft.SqlServer

Public Class AddendaCoppelDAO
    Const URI_SAT = "http://www.sat.gob.mx/cfd/3"
    Public Sub Main()

    End Sub
    Public Sub PruebaDeConexion()
        Dim ib As New DataControl
        ib.RunSQLQuery("--hello")
    End Sub
    Public Sub GuardaInformacion()

    End Sub
    Public Sub ObtieneInformacion()

    End Sub

    Public Function GeneraAddendav33(ByVal addendaDto As AddendaCoppelDTO) As XmlElement
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("en-US")

        Dim totalesDao As New TotalesCfdDAO
        Dim totalDto As New TotalesCfdDTO
        totalDto = totalesDao.getById(addendaDto.Cfdid)

        Dim doc As XmlDocument = New XmlDocument()
        Dim requestForPayment As XmlElement
        Dim requestForPaymentIdentification As XmlElement
        Dim entityType As XmlElement
        Dim uniqueCreatorIdentification As XmlElement
        Dim orderIdentification As XmlElement

        Dim referenceIdentification As XmlElement
        Dim ReferenceDate As XmlElement
        Dim FechaPromesaEnt As XmlElement

        Dim seller As XmlElement
        Dim sellergln As XmlElement
        Dim alternatePartyIdentification As XmlElement
        Dim IndentificaTipoProv As XmlElement

        Dim shipTo As XmlElement
        Dim BodegaDestino As XmlElement
        Dim BodegaReceptora As XmlElement
        Dim nameAndAddress As XmlElement
        Dim bodegaEnt As XmlElement

        Dim currency As XmlElement
        Dim currencyFunction As XmlElement
        Dim rateOfChange As XmlElement

        Dim FleteCaja As XmlElement

        Dim allowanceCharge As XmlElement
        Dim specialServicesType1 As XmlElement
        Dim monetaryAmountOrPercentage As XmlElement
        Dim rateAmountOrPercentage As XmlElement
        Dim percentage As XmlElement

        Dim TotalLotes As XmlElement
        Dim cantidadLotes As XmlElement

        Dim totalAmount As XmlElement
        Dim Amount1 As XmlElement

        Dim TotalAllowanceCharge As XmlElement
        Dim specialServicesType2 As XmlElement
        Dim Amount2 As XmlElement

        Dim baseAmount As XmlElement
        Dim Amount3 As XmlElement

        Dim taxVAT As XmlElement
        Dim taxPercentageVAT As XmlElement
        Dim taxAmountVAT As XmlElement
        Dim taxCategoryVAT As XmlElement

        Dim payableAmount As XmlElement
        Dim Amount4 As XmlElement

        Dim cadenaOriginal As XmlElement
        Dim Cadena As XmlElement

        Dim ObjData As New DataControl

        'Dim Addenda As XmlElement
        'Addenda = doc.CreateElement("cfdi:Addenda", URI_SAT)
        requestForPayment = doc.CreateElement("requestForPayment")
        requestForPayment.SetAttribute("type", "SimpleInvoiceType")
        requestForPayment.SetAttribute("contentVersion", "1.0")
        requestForPayment.SetAttribute("documentStructureVersion", "CPLR1.0")
        requestForPayment.SetAttribute("documentStatus", "ORIGINAL")
        requestForPayment.SetAttribute("DeliveryDate", Format(Date.Now(), "yyyy-MM-dd"))
        'requestForPayment.SetAttribute("DeliveryDate", Format(calFechaOrdenCompraDS.SelectedDate, "yyyy-MM-dd"))

        requestForPaymentIdentification = doc.CreateElement("requestForPaymentIdentification")
        entityType = doc.CreateElement("entityType")
        entityType.AppendChild(doc.CreateTextNode("INVOICE"))
        uniqueCreatorIdentification = doc.CreateElement("uniqueCreatorIdentification")
        uniqueCreatorIdentification.AppendChild(doc.CreateTextNode(addendaDto.Serie & addendaDto.Folio))
        requestForPaymentIdentification.AppendChild(entityType)
        requestForPaymentIdentification.AppendChild(uniqueCreatorIdentification)
        requestForPayment.AppendChild(requestForPaymentIdentification)

        orderIdentification = doc.CreateElement("orderIdentification")
        referenceIdentification = doc.CreateElement("referenceIdentification")
        referenceIdentification.SetAttribute("type", "ON")
        referenceIdentification.AppendChild(doc.CreateTextNode(addendaDto.OrdenCompra))
        orderIdentification.AppendChild(referenceIdentification)
        ReferenceDate = doc.CreateElement("ReferenceDate")
        ReferenceDate.AppendChild(doc.CreateTextNode(Format(addendaDto.FechaOrdenCompra, "yyyy-MM-dd")))
        orderIdentification.AppendChild(ReferenceDate)
        FechaPromesaEnt = doc.CreateElement("FechaPromesaEnt")
        FechaPromesaEnt.AppendChild(doc.CreateTextNode(Format(addendaDto.FechaPromesaEntrega, "yyyy-MM-dd")))
        orderIdentification.AppendChild(FechaPromesaEnt)

        requestForPayment.AppendChild(orderIdentification)

        seller = doc.CreateElement("seller")
        sellergln = doc.CreateElement("gln")
        alternatePartyIdentification = doc.CreateElement("alternatePartyIdentification")
        sellergln.AppendChild(doc.CreateTextNode(addendaDto.GlnVendedor))
        alternatePartyIdentification.SetAttribute("type", "SELLER_ASSIGNED_IDENTIFIER_FOR_A_PARTY")
        alternatePartyIdentification.AppendChild(doc.CreateTextNode(addendaDto.NumProveedor.Trim))
        IndentificaTipoProv = doc.CreateElement("IndentificaTipoProv")
        IndentificaTipoProv.AppendChild(doc.CreateTextNode("2"))
        seller.AppendChild(sellergln)
        seller.AppendChild(alternatePartyIdentification)
        seller.AppendChild(IndentificaTipoProv)
        requestForPayment.AppendChild(seller)

        shipTo = doc.CreateElement("shipTo")
        nameAndAddress = doc.CreateElement("nameAndAddress")
        bodegaEnt = doc.CreateElement("bodegaEnt")
        bodegaEnt.InnerText = addendaDto.BodegaDestino

        nameAndAddress.AppendChild(bodegaEnt)
        shipTo.AppendChild(nameAndAddress)

        'BodegaDestino = doc.CreateElement("BodegaDestino")
        'BodegaDestino.AppendChild(doc.CreateTextNode(addendaDto.BodegaDestino))
        'shipTo.AppendChild(BodegaDestino)

        'BodegaReceptora = doc.CreateElement("BodegaReceptora")
        'BodegaReceptora.AppendChild(doc.CreateTextNode(addendaDto.BodegaReceptora))
        'shipTo.AppendChild(BodegaReceptora)

        requestForPayment.AppendChild(shipTo)

        'If cmbSucursal.SelectedValue > 0 Then
        '    Dim ds As New DataSet
        '    ds = ObjData.FillDataSet("EXEC pConsultarSucursal @sucursalid='" & cmbSucursal.SelectedValue.ToString & "'")
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        For Each row As DataRow In ds.Tables(0).Rows
        '            name.AppendChild(doc.CreateTextNode(row("NOMENT")))
        '            streetAddressOne.AppendChild(doc.CreateTextNode(row("CALENT") & " " & row("COLENT")))
        '            city.AppendChild(doc.CreateTextNode(row("LOCENT")))
        '            postalCode.AppendChild(doc.CreateTextNode(row("CODENT")))
        '            bodegaEnt.AppendChild(doc.CreateTextNode(row("bodega")))
        '            nameAndAddress.AppendChild(name)
        '            nameAndAddress.AppendChild(streetAddressOne)
        '            nameAndAddress.AppendChild(city)
        '            nameAndAddress.AppendChild(postalCode)
        '            nameAndAddress.AppendChild(bodegaEnt)
        '            shipTo.AppendChild(nameAndAddress)
        '            requestForPayment.AppendChild(shipTo)
        '        Next
        '    End If
        'End If

        currency = doc.CreateElement("currency")
        currency.SetAttribute("currencyISOCode", "MXN")
        currencyFunction = doc.CreateElement("currencyFunction")
        currencyFunction.AppendChild(doc.CreateTextNode("BILLING_CURRENCY"))
        rateOfChange = doc.CreateElement("rateOfChange")
        rateOfChange.AppendChild(doc.CreateTextNode("1"))
        currency.AppendChild(currencyFunction)
        currency.AppendChild(rateOfChange)
        requestForPayment.AppendChild(currency)

        'FleteCaja = doc.CreateElement("FleteCaja")
        'FleteCaja.SetAttribute("type", "SELLER_PROVIDED")
        'FleteCaja.AppendChild(doc.CreateTextNode(addendaDto.FleteCaja))
        'requestForPayment.AppendChild(FleteCaja)

        'allowanceCharge = doc.CreateElement("allowanceCharge")
        'allowanceCharge.SetAttribute("allowanceChargeType", "ALLOWANCE_GLOBAL")
        'allowanceCharge.SetAttribute("settlementType", "BILL_BACK")

        'specialServicesType1 = doc.CreateElement("specialServicesType")
        'specialServicesType1.AppendChild(doc.CreateTextNode("AA"))
        'allowanceCharge.AppendChild(specialServicesType1)
        monetaryAmountOrPercentage = doc.CreateElement("monetaryAmountOrPercentage")
        rateAmountOrPercentage = doc.CreateElement("rate")
        rateAmountOrPercentage.SetAttribute("base", "INVOICE_VALUE")
        percentage = doc.CreateElement("percentage")
        percentage.AppendChild(doc.CreateTextNode("0.00"))
        rateAmountOrPercentage.AppendChild(percentage)
        monetaryAmountOrPercentage.AppendChild(rateAmountOrPercentage)
        'allowanceCharge.AppendChild(monetaryAmountOrPercentage)
        'requestForPayment.AppendChild(allowanceCharge)

        TotalLotes = doc.CreateElement("TotalLotes")
        cantidadLotes = doc.CreateElement("cantidad")
        cantidadLotes.AppendChild(doc.CreateTextNode(addendaDto.FleteCaja.Trim))
        TotalLotes.AppendChild(cantidadLotes)
        requestForPayment.AppendChild(TotalLotes)
        '
        '   PARTIDAS
        '
        Dim connP As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("conn").ConnectionString)
        Dim cmdP As New SqlCommand("exec pCFD @cmd=13, @cfdId='" & addendaDto.Cfdid & "'", connP)
        Try
            connP.Open()
            '
            Dim rs As SqlDataReader
            rs = cmdP.ExecuteReader()
            '
            Dim partida As Integer = 0
            While rs.Read
                partida = partida + 1

                Dim PrecioBruto As Decimal = 0
                Dim PrecioNeto As Decimal = 0
                Dim ImporteBruto As Decimal = 0
                Dim importeNeto As Decimal = 0
                Dim Cantidad As Decimal = 0


                Cantidad = rs("cantidad")

                PrecioBruto = rs("precio")
                PrecioNeto = PrecioBruto

                ImporteBruto = Cantidad * PrecioBruto ' - Descuento
                importeNeto = Cantidad * PrecioNeto

                Dim lineItem As XmlElement
                lineItem = doc.CreateElement("lineItem")
                lineItem.SetAttribute("type", "SimpleInvoiceLineItemType")
                lineItem.SetAttribute("number", partida)

                Dim tradeItemIdentification As XmlElement
                Dim gtin As XmlElement

                tradeItemIdentification = doc.CreateElement("tradeItemIdentification")
                gtin = doc.CreateElement("gtin")
                gtin.AppendChild(doc.CreateTextNode(rs("codigo")))
                tradeItemIdentification.AppendChild(gtin)

                Dim alternateTradeItemIdentification As XmlElement
                alternateTradeItemIdentification = doc.CreateElement("alternateTradeItemIdentification")
                alternateTradeItemIdentification.SetAttribute("type", "BUYER_ASSIGNED")
                alternateTradeItemIdentification.AppendChild(doc.CreateTextNode(rs("codigo")))

                'codigoTallaInternoCop
                Dim codigoTallaInternoCop As XmlElement
                Dim codigo As XmlElement
                Dim talla As XmlElement

                codigoTallaInternoCop = doc.CreateElement("codigoTallaInternoCop")
                codigo = doc.CreateElement("codigo")
                talla = doc.CreateElement("talla")
                codigo.AppendChild(doc.CreateTextNode(rs("codigo")))
                talla.AppendChild(doc.CreateTextNode(rs("talla")))
                codigoTallaInternoCop.AppendChild(codigo)
                codigoTallaInternoCop.AppendChild(talla)

                Dim tradeItemDescriptionInformation As XmlElement
                tradeItemDescriptionInformation = doc.CreateElement("tradeItemDescriptionInformation")
                tradeItemDescriptionInformation.SetAttribute("language", "ES")

                Dim longTextDescripcion As XmlElement
                longTextDescripcion = doc.CreateElement("longText")
                longTextDescripcion.AppendChild(doc.CreateTextNode(Truncate(rs("descripcion"), 35)))
                tradeItemDescriptionInformation.AppendChild(longTextDescripcion)

                Dim invoicedQuantity As XmlElement
                invoicedQuantity = doc.CreateElement("invoicedQuantity")
                invoicedQuantity.SetAttribute("unitOfMeasure", "PCE")
                invoicedQuantity.AppendChild(doc.CreateTextNode(rs("cantidad")))

                'Dim aditionalQuantity As XmlElement
                'aditionalQuantity = doc.CreateElement("aditionalQuantity")
                'aditionalQuantity.SetAttribute("QuantityType", "NUM_CONSUMER_UNITS")
                'aditionalQuantity.AppendChild(doc.CreateTextNode("50"))

                Dim grossPrice As XmlElement
                Dim lineItemAmount1 As XmlElement
                grossPrice = doc.CreateElement("grossPrice")
                lineItemAmount1 = doc.CreateElement("Amount")
                lineItemAmount1.AppendChild(doc.CreateTextNode(Format(PrecioBruto, "#0.00")))
                grossPrice.AppendChild(lineItemAmount1)

                Dim netPrice As XmlElement
                Dim lineItemAmount2 As XmlElement
                netPrice = doc.CreateElement("netPrice")
                lineItemAmount2 = doc.CreateElement("Amount")
                lineItemAmount2.AppendChild(doc.CreateTextNode(Format(PrecioNeto, "#0.00")))
                netPrice.AppendChild(lineItemAmount2)

                Dim modeloInformation As XmlElement
                modeloInformation = doc.CreateElement("modeloInformation")
                Dim longTextModelo As XmlElement
                longTextModelo = doc.CreateElement("longText")
                longTextModelo.AppendChild(doc.CreateTextNode(rs("codigo")))
                modeloInformation.AppendChild(longTextModelo)

                Dim allowanceChargeItem As XmlElement
                Dim specialServicesTypeItem As XmlElement
                Dim monetaryAmountOrPercentageItem As XmlElement
                Dim ratePerUnit As XmlElement
                Dim percentagePerUnit As XmlElement
                Dim amountPerUnit As XmlElement

                allowanceChargeItem = doc.CreateElement("allowanceCharge")
                allowanceChargeItem.SetAttribute("Type", "ALLOWANCE_GLOBAL")
                specialServicesTypeItem = doc.CreateElement("specialServicesType")
                specialServicesTypeItem.AppendChild(doc.CreateTextNode("PAD"))
                allowanceChargeItem.AppendChild(specialServicesTypeItem)
                monetaryAmountOrPercentageItem = doc.CreateElement("monetaryAmountOrPercentage")
                percentagePerUnit = doc.CreateElement("percentagePerUnit")
                'percentagePerUnit.InnerText = rs("percentagePerUnit")
                percentagePerUnit.InnerText = 0
                ratePerUnit = doc.CreateElement("ratePerUnit")
                amountPerUnit = doc.CreateElement("amountPerUnit")
                amountPerUnit.InnerText = 0
                ratePerUnit.AppendChild(amountPerUnit)
                monetaryAmountOrPercentageItem.AppendChild(percentagePerUnit)
                monetaryAmountOrPercentageItem.AppendChild(ratePerUnit)
                allowanceChargeItem.AppendChild(monetaryAmountOrPercentageItem)

                Dim palletInformation As XmlElement
                Dim palletQuantity As XmlElement
                Dim description As XmlElement
                Dim transport As XmlElement
                Dim methodOfPayment As XmlElement
                Dim prepactCant As XmlElement

                palletInformation = doc.CreateElement("palletInformation")
                palletQuantity = doc.CreateElement("palletQuantity")
                palletQuantity.AppendChild(doc.CreateTextNode(rs("cajas")))
                description = doc.CreateElement("description")
                description.SetAttribute("type", "BOX")
                description.AppendChild(doc.CreateTextNode("EMPAQUETADO"))

                transport = doc.CreateElement("transport")
                methodOfPayment = doc.CreateElement("methodOfPayment")
                methodOfPayment.AppendChild(doc.CreateTextNode("PAID_BY_BUYER"))
                prepactCant = doc.CreateElement("prepactCant")
                prepactCant.AppendChild(doc.CreateTextNode(rs("piezasporcaja")))
                transport.AppendChild(methodOfPayment)
                transport.AppendChild(prepactCant)

                palletInformation.AppendChild(palletQuantity)
                palletInformation.AppendChild(description)
                palletInformation.AppendChild(transport)

                Dim totalLineAmount As XmlElement
                Dim grossAmount As XmlElement
                Dim lineItemAmount3 As XmlElement
                Dim netAmount As XmlElement
                Dim lineItemAmount4 As XmlElement

                totalLineAmount = doc.CreateElement("totalLineAmount")

                grossAmount = doc.CreateElement("grossAmount")
                lineItemAmount3 = doc.CreateElement("Amount")
                lineItemAmount3.AppendChild(doc.CreateTextNode(Format(ImporteBruto, "#0.00")))
                grossAmount.AppendChild(lineItemAmount3)
                totalLineAmount.AppendChild(grossAmount)

                netAmount = doc.CreateElement("netAmount")
                lineItemAmount4 = doc.CreateElement("Amount")
                lineItemAmount4.AppendChild(doc.CreateTextNode(Format(importeNeto, "#0.00")))
                netAmount.AppendChild(lineItemAmount4)
                totalLineAmount.AppendChild(netAmount)

                lineItem.AppendChild(tradeItemIdentification)
                lineItem.AppendChild(alternateTradeItemIdentification)
                lineItem.AppendChild(codigoTallaInternoCop)
                lineItem.AppendChild(tradeItemDescriptionInformation)
                lineItem.AppendChild(invoicedQuantity)
                'lineItem.AppendChild(aditionalQuantity)
                lineItem.AppendChild(grossPrice)
                lineItem.AppendChild(netPrice)
                lineItem.AppendChild(modeloInformation)
                lineItem.AppendChild(allowanceChargeItem)
                lineItem.AppendChild(palletInformation)
                lineItem.AppendChild(totalLineAmount)

                requestForPayment.AppendChild(lineItem)

            End While
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            connP.Close()
            connP.Dispose()
            connP = Nothing
        End Try

        totalAmount = doc.CreateElement("totalAmount")
        Amount1 = doc.CreateElement("Amount")
        Amount1.AppendChild(doc.CreateTextNode(Format(totalDto.Subtotal - totalDto.Descuento, "#0.00")))
        totalAmount.AppendChild(Amount1)
        requestForPayment.AppendChild(totalAmount)

        TotalAllowanceCharge = doc.CreateElement("TotalAllowanceCharge")
        TotalAllowanceCharge.SetAttribute("allowanceOrChargeType", "ALLOWANCE")
        specialServicesType2 = doc.CreateElement("specialServicesType")
        specialServicesType2.AppendChild(doc.CreateTextNode("PAD"))
        Amount2 = doc.CreateElement("Amount")
        'Amount2.AppendChild(doc.CreateTextNode(Format(totalDto.Descuento, "#0.00")))
        Amount2.AppendChild(doc.CreateTextNode(Format(0, "#0.00")))
        TotalAllowanceCharge.AppendChild(specialServicesType2)
        TotalAllowanceCharge.AppendChild(Amount2)
        requestForPayment.AppendChild(TotalAllowanceCharge)

        baseAmount = doc.CreateElement("baseAmount")
        Amount3 = doc.CreateElement("Amount")
        Amount3.AppendChild(doc.CreateTextNode(Format(totalDto.Subtotal - totalDto.Descuento, "#0.00")))
        baseAmount.AppendChild(Amount3)
        requestForPayment.AppendChild(baseAmount)

        taxVAT = doc.CreateElement("tax")
        taxVAT.SetAttribute("type", "VAT")
        taxPercentageVAT = doc.CreateElement("taxPercentage")
        taxPercentageVAT.AppendChild(doc.CreateTextNode("16.00"))
        taxVAT.AppendChild(taxPercentageVAT)

        taxAmountVAT = doc.CreateElement("taxAmount")
        taxAmountVAT.AppendChild(doc.CreateTextNode(Format(totalDto.Iva, "#0.00")))
        taxVAT.AppendChild(taxAmountVAT)

        taxCategoryVAT = doc.CreateElement("taxCategory")
        taxCategoryVAT.AppendChild(doc.CreateTextNode("TRANSFERIDO"))
        taxVAT.AppendChild(taxCategoryVAT)
        requestForPayment.AppendChild(taxVAT)

        payableAmount = doc.CreateElement("payableAmount")
        Amount4 = doc.CreateElement("Amount")
        Amount4.AppendChild(doc.CreateTextNode(Format(totalDto.Total, "#0.00")))
        payableAmount.AppendChild(Amount4)
        requestForPayment.AppendChild(payableAmount)

        cadenaOriginal = doc.CreateElement("cadenaOriginal")
        Cadena = doc.CreateElement("Cadena")
        Cadena.AppendChild(doc.CreateTextNode(addendaDto.CadenaOriginal))
        cadenaOriginal.AppendChild(Cadena)
        requestForPayment.AppendChild(cadenaOriginal)


        ObjData.RunSQLQuery("exec pCFD @cmd=47, @tipo_addenda='" & addendaDto.TipoAddendaid & "', @cfdid='" & addendaDto.Cfdid & "'")
        ObjData = Nothing
        '
        Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es-MX")
        Thread.CurrentThread.CurrentUICulture = New CultureInfo("es-MX")

        Return requestForPayment
    End Function
    Public Sub GeneraAddendav40()

    End Sub
    Public Function Truncate(value As String, length As Integer) As String
        If length > value.Length Then
            Return value
        Else
            Return value.Substring(0, length)
        End If
    End Function

End Class
