<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/portalcfd/MasterPage_PortalCFD.master" CodeBehind="recibirorden.aspx.vb" Inherits="erp_neogenis.recibirorden" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script language="javascript" type="text/javascript">

        function OnClientClose(sender, args) {
            document.location.href = document.location.href;
        }


        function openRadWindow(id) {
            var oWnd = radopen("recibir.aspx?id=" + id, "ReceiveWindow");
            oWnd.center();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Width="100%" LoadingPanelID="RadAjaxLoadingPanel1">--%>
    <br />
    <fieldset>
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblEditorOrdenes" runat="server" Font-Bold="true" CssClass="item" Text="Recibir Orden de Compra"></asp:Label>
        </legend>
        <br />
        
        <table width="100%" cellspacing="2" cellpadding="2" align="center" style="line-height:25px;">
            <tr>
                <td class="item">
                    <strong>Clave: </strong>
                    <asp:Label ID="lblClaveEdit" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Proveedor: </strong><br />
                    <asp:DropDownList ID="proveedorid" runat="server" CssClass="item"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Marca: </strong><br />
                    <asp:DropDownList ID="proyectoid" runat="server" CssClass="item"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Fecha Estimada de Entrega: </strong>
                    <asp:Label ID="lblFechaEstEnt" runat="server" Font-Bold="true" CssClass="item"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="item">
                    <strong>Comentarios: </strong><br />
                    <telerik:RadTextBox id="txtComentarios" runat="server" TextMode="MultiLine" Width="600px" Height="90px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
        
    </fieldset>

    <br />

    <%--<fieldset class="item">
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblOrdenParcial" runat="server" Font-Bold="true" CssClass="item" Text="Orden Parcial"></asp:Label>
        </legend>

        <telerik:RadGrid ID="OrdenParcialList" runat="server" Width="70%" ShowStatusBar="True"
                AutoGenerateColumns="False" AllowPaging="True" PageSize="8" GridLines="None"
                Skin="Simple" ShowFooter="true">
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView Width="100%" DataKeyNames="id" Name="Products" AllowMultiColumnSorting="False">
                    <Columns>
                        <telerik:GridBoundColumn DataField="ordenParcialId" HeaderText="Orden Parcial" UniqueName="orden_parcial">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="ordenId" HeaderText="Orden Id" UniqueName="orden_id">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="fecha_alta" HeaderText="Fecha Alta" UniqueName="fecha_alta" >
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="fecha_recepcion" HeaderText="Fecha Recepción" UniqueName="aecha_recepción" >
                        </telerik:GridBoundColumn>                    
                        <telerik:GridTemplateColumn AllowFiltering="False" HeaderStyle-HorizontalAlign="Center"
                            UniqueName="Del">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnDelete" runat="server" CommandArgument='<%# Eval("id") %>'
                                    CommandName="cmdDelete" ImageUrl="~/images/action_delete.gif" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
     </fieldset>--%>

     <br />

    <fieldset class="item">
        <legend style="padding-right: 6px; color: Black">
            <asp:Label ID="lblPartidas" runat="server" Font-Bold="true" CssClass="item" Text="Conceptos"></asp:Label>
        </legend>
        
        <br />

        <telerik:RadGrid ID="conceptosList" runat="server" Width="100%" ShowStatusBar="True"
                AutoGenerateColumns="False" AllowPaging="False" GridLines="None"
                Skin="Simple" ShowFooter="true">
                <PagerStyle Mode="NumericPages"></PagerStyle>
                <MasterTableView Width="100%" DataKeyNames="id, cantidad, codigo" Name="Products" AllowMultiColumnSorting="False">
                    <Columns>
                        <telerik:GridBoundColumn DataField="codigo" HeaderText="Código" UniqueName="codigo">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="descripcion" HeaderText="Descripción" UniqueName="descripcion">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="cantidad" HeaderText="Cant. Pedida" UniqueName="cantidad" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="cantidad_recibida" HeaderText="Cantidad Recibida" UniqueName="cantidad_recibida" ItemStyle-HorizontalAlign="Center">
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="costo" HeaderText="Costo (MXN)" UniqueName="costo" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>
                        <%--<telerik:GridBoundColumn DataField="costo_variable" HeaderText="Costo Variable Prom. (MXN)" UniqueName="costo_variable" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>--%>
                        <%--<telerik:GridBoundColumn DataField="total" HeaderText="Total (MXN)" UniqueName="total" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right">
                        </telerik:GridBoundColumn>--%>
                        <telerik:GridTemplateColumn>
                            <HeaderTemplate>Cant. Recibir</HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadNumericTextBox ID="txtCantidad" runat="server" Skin="Default" Width="50px" MinValue="0" Value='0'>
                                    <NumberFormat DecimalDigits="2" GroupSeparator="" />
                                </telerik:RadNumericTextBox>&nbsp;&nbsp;
                                <span style="color:red;">*</span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderTemplate>Costo Variable</HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadNumericTextBox ID="txtCostoVariable" runat="server" Skin="Default" Width="50px" MinValue="0" Value='0'>
                                    <NumberFormat DecimalDigits="2" GroupSeparator="" />
                                </telerik:RadNumericTextBox>&nbsp;&nbsp;
                                <span style="color:red;">*</span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderTemplate>Almacén</HeaderTemplate>
                            <ItemTemplate>
                                <asp:DropDownList ID="almacenid" runat="server" CssClass="item"></asp:DropDownList>&nbsp;&nbsp;
                                <span style="color:red;">*</span>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn Visible="False" AllowFiltering="False" HeaderStyle-HorizontalAlign="Center" UniqueName="Add">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnAdd" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="cmdAdd" ImageUrl="~/portalcfd/images/action_add.gif" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" />
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
            </telerik:RadGrid>
            <br />
            <span style="color:Red;">* Campos requeridos</span>
            <br /><br />
            <asp:Button ID="btnProcesar" runat="server" Text="Recibir" />&nbsp;&nbsp;
            <asp:Button ID="btnProcessCancel" runat="server" Text="Cancelar Órden" BackColor="Red" ForeColor="White" CausesValidation="false" />&nbsp;&nbsp;
            <asp:Button ID="btnCancelar" runat="server" Text="Regresar al listado" CausesValidation="false" />
            <br /><br />
            <asp:Label ID="lblMensaje" runat="server" Font-Bold="true"></asp:Label>
            <br /><br />
    </fieldset>
    
    <br /><br /><br />

    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" Skin="MetroTouch" VisibleStatusbar="False" Behavior="Default" Height="80px" InitialBehavior="None" Left="" Top="">
        <Windows> 
            <telerik:RadWindow ID="ReceiveWindow" runat="server" ShowContentDuringLoad="False" Modal="True" ReloadOnShow="True" Skin="MetroTouch" VisibleStatusbar="False" Width="990px" Height="450px" Behavior="Close" BackColor="Gray" style="display:none; z-index:1000;" Behaviors="Close" InitialBehavior="None" OnClientClose="OnClientClose">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>

    <%--</telerik:RadAjaxPanel>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Bootstrap" Width="100%">
    </telerik:RadAjaxLoadingPanel>--%>
</asp:Content>