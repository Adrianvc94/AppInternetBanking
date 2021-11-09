<%@ Page Title="" Async="true" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmCredito.aspx.cs" Inherits="AppWebInternetBanking.Views.frmCredito" %>
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
                $("#MainContent_gvCredito tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>
    <h1>Mantenimiento de créditos</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvCredito" runat="server" AutoGenerateColumns="false"
        CssClass="table table-sm" HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#1B1A1A" HeaderStyle-ForeColor="White"
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvCredito_RowCommand"
        HeaderStyle-BorderStyle="None" BorderStyle="None">
        <Columns>
            <asp:BoundField HeaderText="Código" DataField="CodCredito" HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None"/>
            <asp:BoundField HeaderText="Código de Usuario" DataField="CodUsuario" HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None"/>
            <asp:BoundField HeaderText="Monto Crédito" DataField="MontoCredito" HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None"/>
            <asp:BoundField HeaderText="Descripcion" DataField="Descripcion" HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None"/>
            <asp:BoundField HeaderText="Fecha de Solicitud" DataField="FechaSolicitud" HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" DataFormatString="{0:M/dd/yyyy}" />
            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar"
                ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None"/>
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar"
                ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None"/>
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
                                <asp:Literal ID="ltrCodigoMant" Text="Codigo" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtCodigoMant" runat="server" Enabled="false" CssClass="form-control" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrCodUsuario" Text="Usuario" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddUSU_CODIGO" CssClass="form-control" runat="server"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrMontoCredito" Text="Monto de Crédito" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtMontoCredito" runat="server" CssClass="form-control" textmode="Number" MaxLength="18"/></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrDescripcion" Text="Descripción" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" MaxLength="100"/></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaSolicitud" Text="Fecha de solicitud" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaSolicitud" runat="server" CssClass="form-control" textmode="Date"/></td>
                        </tr>
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
                    <h4 class="modal-title">Mantenimiento de Créditos</h4>
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
