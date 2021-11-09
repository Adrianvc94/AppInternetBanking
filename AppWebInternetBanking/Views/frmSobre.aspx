<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmSobre.aspx.cs" Inherits="AppWebInternetBanking.Views.frmSobre" %>

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
    <h1>Mantenimiento Sobres</h1>
    <input id="MyInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvSobres" runat="server" AutoGenerateColumns="false"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#1B1A1A" HeaderStyle-BorderStyle="None" BorderStyle="None" HeaderStyle-ForeColor="White"
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvSobres_RowCommand">

        <Columns>
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="CodSobre" DataField="CodSobre" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="CodCuenta" DataField="CodCuenta" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Saldo" DataField="Saldo" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Descripcion" DataField="Descripcion" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="CodMoneda" DataField="CodMoneda" />
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
                                <asp:Literal ID="ltrCodSobreMant" Text="Codigo Sobre" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodSobreMant" runat="server" Enabled="false" CssClass="form-control" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodCuentaMant" Text="Codigo Cuenta" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodCuentaMant" runat="server" Enabled="true" CssClass="form-control" />
                                
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrSaldoMant" Text="Saldo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtSaldoMant" runat="server" Enabled="true" CssClass="form-control" />
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrDescripcionMant" Text="Descripcion" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtDescripcionMant" runat="server" Enabled="true" CssClass="form-control" />
                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodigoMonedaMant" Text="Codigo Moneda" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlCodMoneda" CssClass="form-control" runat="server">
                                    <asp:ListItem Value="1">Dolares</asp:ListItem>
                                    <asp:ListItem Value="2">Colones</asp:ListItem>
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
                    <h4 class="modal-title">Mantenimiento de sobres</h4>
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
