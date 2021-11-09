﻿<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmMarchamo.aspx.cs" Inherits="AppWebInternetBanking.Views.frmMarchamo" %>

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
                $("#MainContent_gvMarchamos tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>

    <h1>Mantenimiento de Marchamos</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <br />
    <asp:GridView ID="gvMarchamos" runat="server" AutoGenerateColumns="false"
        CssClass="table table-sm text-center" HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#1B1A1A" HeaderStyle-BorderStyle="None" BorderStyle="None"  HeaderStyle-ForeColor="White"
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvMarchamos_RowCommand">
        <Columns>

            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Codigo" DataField="CodMarchamo" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Placa" DataField="Placa"  />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Usuario" DataField="CodUsuario" />
            <asp:BoundField HeaderText="Monto" DataField="Monto" Visible="false" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Valor Vehiculo" DataField="ValorVehiculo" />
            <asp:BoundField HeaderText="Tasa" DataField="Tasa" Visible="false" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Total a Pagar" DataField="TotalPagar" />

            <asp:ButtonField HeaderStyle-CssClass="text-center" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" HeaderStyle-BorderStyle="None"  ItemStyle-BorderStyle="None" HeaderText="Modificar" CommandName="Modificar"
                ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderStyle-CssClass="text-center" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" HeaderStyle-BorderStyle="None" ItemStyle-BorderStyle="None"  HeaderText="Eliminar" CommandName="Eliminar"
                ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
        Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo" />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />




    <!--VENTANA DE MANTENIMIENTO -->
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
                                <asp:Literal ID="ltrCodigoMarchamo" Text="Codigo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoMarchamo" runat="server" Enabled="false" CssClass="form-control"/></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrPlaca" Text="Placa" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtPlaca" runat="server" CssClass="form-control" MaxLength="7"/></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodUsuario" Text="Usuario" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddUSU_CODIGO" CssClass="form-control" runat="server"></asp:DropDownList></td>
                        </tr>
                        <!-- 
              <tr>
                 <td><asp:Literal ID="ltrMonto" Text="Monto" runat="server" /></td>
                 <td><asp:TextBox ID="txtMonto" TextMode="Number" runat="server" CssClass="form-control" /></td>
              </tr>
              -->
                        <tr>
                            <td>
                                <asp:Literal ID="ltrValorVehiculo" Text="Valor Fiscal vehiculo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtValorVehiculo" TextMode="Number" runat="server" CssClass="form-control" /></td>
                        </tr>
                        <!-- 
              <tr>
                 <td><asp:Literal ID="ltrTasa" Text="Tasa" runat="server" /></td>
                 <td><asp:TextBox ID="txtTasa" TextMode="Number" runat="server" CssClass="form-control" /></td>
              </tr>
                    -->
                        <!-- 
              <tr>
                 <td><asp:Literal ID="ltrTotalPagar" Text="Total a pagar" runat="server" /></td>
                 <td><asp:TextBox ID="txtTotalPagar" TextMode="Number" runat="server" CssClass="form-control" /></td>
              </tr>
          -->

                    </table>
                    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
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
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Mantenimiento de Marchamos</h4>
                </div>
                <div class="modal-body">
                    <p>
                        <asp:Literal ID="ltrModalMensaje" runat="server" /></p>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>

</asp:Content>
