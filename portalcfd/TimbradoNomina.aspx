<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="TimbradoNomina.aspx.vb" Inherits="erp_neogenis.TimbradoNomina" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
 <script type="text/javascript">
     function OnRequestStart(target, arguments) {
         if ((arguments.get_eventTarget().indexOf("lnkPDF") > -1) || (arguments.get_eventTarget().indexOf("lnkXML") > -1) || (arguments.get_eventTarget().indexOf("btnDelete") > -1) || (arguments.get_eventTarget().indexOf("corridasList") > -1)) {
             arguments.set_enableAjax(false);
         }
     }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" HorizontalAlign="NotSet" LoadingPanelID="RadAjaxLoadingPanel1" ClientEvents-OnRequestStart="OnRequestStart">
    <table width="100%" border="0">
        <tr>
            <td style="width:20%">
                <asp:Label ID="lblPeriodoPago" runat="server" CssClass="item" Font-Bold="True" Text="Seleccione el periodo de:"></asp:Label>
            </td>
            <td style="width:20%">
                <asp:Label ID="lblFechaInicial" runat="server" CssClass="item" Font-Bold="True" Text="Fecha Inicial:"></asp:Label>
            </td>
            <td style="width:20%">
                <asp:Label ID="lblFechaFinal" runat="server" CssClass="item" Font-Bold="True" Text="Fecha Final:"></asp:Label>
            </td>
            <td style="width:20%">
                <asp:Label ID="lblDiasLaborados" runat="server" CssClass="item" Font-Bold="True" Text="Días Laborados:"></asp:Label>
            </td>
            <td style="width:20%">&nbsp;</td>
        </tr>
        <tr>
            <td style="width:20%">
                <asp:DropDownList ID="ddlPeriodoPago" runat="server" Width="200px" AutoPostBack="true"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPeriodoPago" ValidationGroup="gpoGeneraCorrida" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
            </td>
            <td style="width:20%">
                <telerik:RadDatePicker ID="calFechaInicial" CultureInfo="Español (México)" DateInput-DateFormat="dd/MM/yyyy" runat="server"></telerik:RadDatePicker>
            </td>
            <td style="width:20%">
                <telerik:RadDatePicker ID="calFechaFinal" CultureInfo="Español (México)" DateInput-DateFormat="dd/MM/yyyy" runat="server"></telerik:RadDatePicker>
            </td>
            <td style="width:20%">
                <telerik:RadNumericTextBox ID="txtDiasLaborados" runat="server" Value="0" MinValue="0" NumberFormat-DecimalDigits="0" MaxLength="2"></telerik:RadNumericTextBox>
            </td>
            <td style="width:20%" align="right">
                <asp:Button ID="btnGeneraCorrida" runat="server" ValidationGroup="gpoGeneraCorrida" Text="Generar Nueva Corrida" CausesValidation="True" CssClass="item" />
            </td>
        </tr>
        <tr>
            <td colspan="5" style="height:2px">&nbsp;</td>
        </tr>
    </table>
    
    <asp:Panel ID="panelCorridas" runat="server" Visible="True">
        <fieldset>
            <legend style="padding-right:6px; color:Black">
                <asp:Label ID="Label1" runat="server" Text="Lista de Corridas" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%" border="0">
                <tr>
                    <td style="height:2px">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0">
                            <tr>
                                <td>
                                    <telerik:RadGrid ID="corridasList" runat="server" AllowPaging="True" 
                                        AutoGenerateColumns="False" GridLines="None" AllowSorting="true" 
                                        PageSize="20" ShowStatusBar="True" 
                                        Skin="Simple" Width="100%">
                                        <ClientSettings> 
                                            <Selecting AllowRowSelect="True" /> 
                                          </ClientSettings>
                                        <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Employees" Width="100%" EnableHeaderContextMenu="true">
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="id" HeaderText="Id" UniqueName="id"></telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Período de pago" UniqueName="">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkVer" runat="server" Text='<%# Eval("periodo_pago") %>' CommandArgument='<%# Eval("id") %>' CommandName="cmdVer"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Left" />
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridBoundColumn DataField="fecha_inicial" HeaderText="Fecha inicial" UniqueName="fecha_inicial"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="fecha_final" HeaderText="Fecha final" UniqueName="fecha_final"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="dias_laborados" HeaderText="Días laborados" UniqueName="dias_laborados"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="total" HeaderText="Total empleados" UniqueName="total"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="total_timbrados" HeaderText="Total empleados timbrados" UniqueName="total_timbrados"></telerik:GridBoundColumn>
                                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Delete">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </fieldset>
    </asp:Panel>
    
    <asp:Panel ID="panelGenerarCorrida" runat="server" Visible="False">
    <fieldset>
        <legend style="padding-right:6px; color:Black">
            <asp:Label ID="lblEmployeeListLegend" runat="server" Text="Lista de Empleados" Font-Bold="true" CssClass="item"></asp:Label>
        </legend>
        <table width="100%" border="0">
            <tr>
                <td style="height:2px">&nbsp;</td>
            </tr>
            <%--<tr>
                <td>
                    <table width="100%" border="0">
                        <tr>
                            <td style="width:40%">
                                <asp:Label ID="lblLugarExpedicion" runat="server" CssClass="item" Font-Bold="True" Text="Lugar de expedición:"></asp:Label>
                            </td>
                            <td style="width:20%">
                                <asp:Label ID="lblMoneda" runat="server" CssClass="item" Font-Bold="True" Text="Moneda:" Visible="false"></asp:Label>
                            </td>
                            <td style="width:20%">
                                <asp:Label ID="lblTipoCambio" runat="server" CssClass="item" Font-Bold="True" Text="Tipo de Cambio:" Visible="false"></asp:Label>
                            </td>
                            <td style="width:20%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width:40%">
                                <asp:TextBox ID="txtLugarExpedicion" CssClass="item" Width="95%" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLugarExpedicion" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width:20%">
                                <asp:DropDownList ID="ddlMoneda" runat="server" Visible="false"></asp:DropDownList><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlMoneda" InitialValue="0" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width:20%" class="item">
                                <telerik:RadNumericTextBox ID="txtTipoCambio" runat="server" NumberFormat-DecimalDigits="2" Value="0" AutoPostBack="true" Visible="false"></telerik:RadNumericTextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtTipoCambio" CssClass="item" ForeColor="Red" ErrorMessage=" *" SetFocusOnError="true"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width:20%">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <tr>
                <td style="height:2px">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    <telerik:RadGrid ID="employeeslist" runat="server" AllowPaging="False" 
                        AutoGenerateColumns="False" GridLines="None" AllowSorting="true" 
                        ShowStatusBar="True" Skin="Simple" Width="100%">
                        <ClientSettings> 
                            <Selecting AllowRowSelect="True" /> 
                          </ClientSettings>
                        <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Employees" Width="100%" EnableHeaderContextMenu="true">
                            <Columns>
                                <telerik:GridBoundColumn DataField="nominaid" HeaderText="nominaid" UniqueName="nominaid" Visible="false"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstatus" runat="server" Text='<%# Eval("estatus") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkid" runat="server" Checked="true" CssClass="item" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Serie" UniqueName="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSerie" runat="server" Text='<%# Eval("serie") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="Folio" UniqueName="">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFolio" runat="server" Text='<%# Eval("folio") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridBoundColumn DataField="numero_empleado" HeaderText="No. Empleado" UniqueName="numero_empleado"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="nombre" HeaderText="Nombre" UniqueName="nombre"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="rfc" HeaderText="RFC" UniqueName="rfc"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="numero_seguro_social" HeaderText="No. Seguro Social" UniqueName="numero_seguro_social"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="departamento" HeaderText="Departamento" UniqueName="departamento"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Percepciones" UniqueName="Percepciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVerPercepciones" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdVerPercepciones" Text="Ver" CausesValidation="False"></asp:LinkButton>
                                        <asp:Label ID="lblTotalPercepciones" runat="server" Text='<%# Eval("total_percepciones") %>' CssClass="item" Visible="false" ></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Deducciones" UniqueName="Deducciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVerDeducciones" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdVerDeducciones" Text="Ver" CausesValidation="false"></asp:LinkButton>
                                        <asp:Label ID="lblTotalDeducciones" runat="server" Text='<%# Eval("total_deducciones") %>' CssClass="item" Visible="False" ></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Total" UniqueName="Total">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Eval("total") %>' CssClass="item" ></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Incapacidades" UniqueName="Incapacidades">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVerIncapacidades" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdVerIncapacidades" Text="Ver" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Horas Extra" UniqueName="HorasExtra">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkVerHorasExtra" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdVerHorasExtra" Text="Ver" CausesValidation="false"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkXML" runat="server" Text="xml" CommandArgument='<%# Eval("id") %>' Visible="false" CommandName="cmdXML"></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="false" HeaderText="" UniqueName="">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPDF" runat="server" Text="pdf" CommandArgument='<%# Eval("id") %>' Visible="false" CommandName="cmdPDF"></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Cancelar">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCancelar" runat="server" Text="Cancelar" CommandArgument='<%# Eval("nominaid") %>' Visible="false" CausesValidation="false" CommandName="cmdCancel"></asp:LinkButton>
                                        <asp:Label ID="lblCancelado" runat="server" Text="Cancelado" CssClass="item" ForeColor="Red" Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                </td>
            </tr>
            <tr>
                <td style="height:2px" align="left">
                    <asp:Label ID="lblMensaje" runat="server" ForeColor="Red" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height:2px">
                    <asp:HiddenField id="EmployeeID" runat="server" Value="0"/>
                    <asp:HiddenField id="cfdId" runat="server" Value="0"/>
                    <asp:HiddenField id="corridaId" runat="server" Value="0"/>
                    <asp:HiddenField id="serie" runat="server" Value=""/>
                    <asp:HiddenField id="folio" runat="server" Value="0"/>
                </td>
            </tr>
            <tr>
                <td align="right">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CausesValidation="False" CssClass="item" />&nbsp;&nbsp;<asp:Button ID="btnTinbrarNomina" runat="server" Text="Timbrar nómina" CausesValidation="True" CssClass="item" />
                </td>
            </tr>
        </table>
    </fieldset>
    </asp:Panel>
    
    <telerik:RadWindow ID="windowPercepciones" runat="server" Modal="true" CenterIfModal="true" VisibleOnPageLoad="false" AutoSize="false" Behaviors="Close" Width="700" Height="600">
        <ContentTemplate>
            <br />
            <fieldset style="width:94%; height:auto;">
                <legend style="padding-right:6px; color:Black">
                    <asp:Label ID="lblPercepcionesTitle" runat="server" Text="Percepciones" Font-Bold="true" CssClass="item"></asp:Label>
                </legend>
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadGrid ID="grdPercepciones" runat="server" AllowPaging="True" 
                                AutoGenerateColumns="False" GridLines="None" AllowSorting="true" 
                                PageSize="15" ShowStatusBar="True" 
                                Skin="Simple" Width="100%">
                                <PagerStyle Mode="NextPrevAndNumeric" />
                                <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Percepciones" Width="100%">
                                    <Columns>
                                        <telerik:GridBoundColumn DataField="clave" HeaderText="Clave" UniqueName="clave" ItemStyle-Width="20%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="50%">
                                        </telerik:GridBoundColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Importe" UniqueName="importe" ItemStyle-Width="30%">
                                            <ItemTemplate>
                                                <telerik:RadNumericTextBox ID="txtImporte" runat="server" Text='<%#Eval("importe")%>' Value="0" MinValue="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <br />
                            <asp:Button ID="btnGuardarPercepciones" runat="server" Text="Guardar" CausesValidation="False" CssClass="item" OnClick="btnGuardarPercepciones_Click" />
                        </td>
                    </tr>
                </table>  
            </fieldset>
            <br />
        </ContentTemplate>
    </telerik:RadWindow>
    
    <telerik:RadWindow ID="windowDeducciones" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" VisibleOnPageLoad="false" Behaviors="Close" Width="700" Height="600">
        <ContentTemplate>
            <br />
            <fieldset style="width:94%; height:auto;">
            <legend style="padding-right:6px; color:Black">
                <asp:Label ID="lblDeduccionesTitle" runat="server" Text="Deducciones" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="grdDeducciones" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" GridLines="None" AllowSorting="true" 
                            PageSize="15" ShowStatusBar="True" 
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NextPrevAndNumeric" />
                            <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Deducciones" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="clave" HeaderText="Clave" UniqueName="clave" ItemStyle-Width="20%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="50%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Importe" UniqueName="importe" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtImporte" runat="server" Text='<%#Eval("importe")%>' Value="0" MinValue="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <br />
                        <asp:Button ID="btnGuardarDeducciones" runat="server" Text="Guardar" CausesValidation="False" CssClass="item" OnClick="btnGuardarDeducciones_Click" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
   </telerik:RadWindow>
    
    <telerik:RadWindow ID="windowIncapacidades" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" VisibleOnPageLoad="false" Behaviors="Close" Width="700" Height="600">
        <ContentTemplate>
            <br />
            <fieldset style="width:94%; height:auto;">
            <legend style="padding-right:6px; color:Black">
                <asp:Label ID="lblIncapacidadesTitle" runat="server" Text="Incapacidades" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="grdIncapacidades" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" GridLines="None" AllowSorting="true" 
                            PageSize="15" ShowStatusBar="True" 
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NextPrevAndNumeric" />
                            <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="Deducciones" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Clave" UniqueName="clave" ItemStyle-Width="20%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="50%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Días" UniqueName="importe" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtDias" Width="80px" runat="server" Text='<%#Eval("dias")%>' Value="0" MinValue="0" NumberFormat-GroupSeparator="" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Importe" UniqueName="importe" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtImporte" runat="server" Text='<%#Eval("importe")%>' Value="0" MinValue="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <br />
                        <asp:Button ID="btnGuardarIncapacidades" runat="server" Text="Guardar" CausesValidation="False" CssClass="item" />
                    </td>
                </tr>
            </table>
            </fieldset>
        </ContentTemplate>
    </telerik:RadWindow>
    
    <telerik:RadWindow ID="windowHorasExtra" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" VisibleOnPageLoad="false" Behaviors="Close" Width="700" Height="600">
        <ContentTemplate>
            <br />
            <fieldset style="width:94%; height:auto;">
            <legend style="padding-right:6px; color:Black">
                <asp:Label ID="Label2" runat="server" Text="Horas Extra" Font-Bold="true" CssClass="item"></asp:Label>
            </legend>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="grdHorasExtra" runat="server" AllowPaging="True" 
                            AutoGenerateColumns="False" GridLines="None" AllowSorting="true" 
                            PageSize="15" ShowStatusBar="True" 
                            Skin="Simple" Width="100%">
                            <PagerStyle Mode="NextPrevAndNumeric" />
                            <MasterTableView AllowMultiColumnSorting="true" AllowSorting="true" DataKeyNames="id" Name="HorasExtra" Width="100%">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="id" HeaderText="Clave" UniqueName="clave" ItemStyle-Width="20%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="nombre" HeaderText="Descripción" UniqueName="descripcion" ItemStyle-Width="50%">
                                    </telerik:GridBoundColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Días" UniqueName="dias" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtDias" Width="80px" runat="server" Text='<%#Eval("dias")%>' Value="0" MinValue="0" NumberFormat-GroupSeparator="" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Horas" UniqueName="horas" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtHoras" Width="80px" runat="server" Text='<%#Eval("horas")%>' Value="0" MinValue="0" NumberFormat-GroupSeparator="" NumberFormat-DecimalDigits="0"></telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn AllowFiltering="False" HeaderText="Importe" UniqueName="importe" ItemStyle-Width="30%">
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtImporte" runat="server" Text='<%#Eval("importe")%>' Value="0" MinValue="0" NumberFormat-DecimalDigits="2"></telerik:RadNumericTextBox>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" />
                                        <ItemStyle HorizontalAlign="Left" />
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr> 
                <tr>
                    <td align="right">
                        <br />
                        <asp:Button ID="btnGuardarHorasExtra" runat="server" Text="Guardar" CausesValidation="False" CssClass="item" />
                    </td>
                </tr>
            </table>
            </fieldset>
        </ContentTemplate>
    </telerik:RadWindow>
    
    <telerik:RadWindow ID="RadWindow1" runat="server" Modal="true" CenterIfModal="true" AutoSize="false" VisibleOnPageLoad="false" Behaviors="Close" Width="700" Height="600">
        <ContentTemplate>
        <br />
        <table align="center" width="90%">
            <tr>
                <td>
                    <asp:TextBox ID="txtErrores" TextMode="MultiLine" Width="100%" Rows="35" ReadOnly="true" CssClass="item" runat="server"></asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        </ContentTemplate>
    </telerik:RadWindow>
    </telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>
</asp:Content>