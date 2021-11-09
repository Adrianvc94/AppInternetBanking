<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPrestamo.aspx.cs" Inherits="AppWebInternetBanking.Views.frmPrestamo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript">

        function openModal() {
            $('#myModal').modal('show'); //ventana de mensajes
        }

        function openModalMantenimiento() {
            $('#myModalMantenimiento').modal('show'); //ventana de mantenimiento
        }

        function CloseModal() {
            $('#myModal').modal('hide');//cierra ventana de mensajes
        }

        function CloseMantenimiento() {
            $('#myModalMantenimiento').modal('hide'); //cierra ventana de mantenimiento
        }

        $(document).ready(function () { //filtrar el datagridview
            $("#myInput").on("keyup", function () {
                var value = $(this).val().toLowerCase();
                $("#MainContent_gvSobres tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>

    <h1>Mantenimiento Prestamos</h1>
    <input id="MyInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvPrestamos" runat="server" AutoGenerateColumns="false"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#1B1A1A" HeaderStyle-BorderStyle="None" BorderStyle="None" HeaderStyle-ForeColor="White"
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvPrestamos_RowCommand">

        <Columns>
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="CodPrestamo" DataField="CodPrestamo" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="CodUsuario" DataField="CodUsuario" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="MontoPrestamo" DataField="MontoPrestamo" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="FechaSolicitud" DataField="FechaSolicitud" DataFormatString="{0:M/dd/yyyy}" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="FechaLimite" DataField="FechaLimite" DataFormatString="{0:M/dd/yyyy}" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="TasaInteres" DataField="TasaInteres" />
            <asp:ButtonField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Modificar" CommandName="Modificar"
                ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Eliminar" CommandName="Eliminar"
                ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
        Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />


    <!-- VENTANA DE MANTENIMIENTO -->
    <div id="myModalMantenimiento" class="modal fade" role="dialog">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">
                        <asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
                </div>
                <div class="modal-body">
                    <table style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodPrestamoMant" Text="Codigo Prestamo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodPrestamoMant" runat="server" Enabled="false" CssClass="form-control" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodUsuarioMant" Text="Codigo Usuario" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodUsuarioMant" runat="server" Enabled="true" CssClass="form-control" />
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrMontoPrestamoMant" Text="Monto Prestamo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtMontoPrestamoMant" runat="server" Enabled="true" CssClass="form-control" />
                                
                            </td>
                        </tr>
<tr>
                            <td>
                                <asp:Literal ID="ltrFechaSolicitudMant" Text="Fecha de solicitud" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaSolicitudMant" runat="server" CssClass="form-control" textmode="Date"/>
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaLimiteMant" Text="Fecha Limite" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaLimiteMant" Placeholder="Ingrese la fecha limite" CssClass="form-control" runat="server" textmode="Date"/>
                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrTasaInteresMant" Text="Tasa Interes" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlTasaInteres" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="10">10%</asp:ListItem>
                                    <asp:ListItem Value="15">15%</asp:ListItem>
                                    <asp:ListItem Value="20">20%</asp:ListItem>
                                    <asp:ListItem Value="25">25%</asp:ListItem>
                                    <asp:ListItem Value="30">30%</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="true" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" OnClick="btnAceptarMant_Click" CssClass="btn btn-success" ID="btnAceptarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" OnClick="btnCancelarMant_Click" CssClass="btn btn-danger" ID="btnCancelarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>

    <!-- VENTANA MODAL -->
    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Mantenimiento de Prestamos</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="ltrModalMensaje" runat="server" />
                    </p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>


</asp:Content>
