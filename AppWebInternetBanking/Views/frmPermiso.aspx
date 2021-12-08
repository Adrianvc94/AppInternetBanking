<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmPermiso.aspx.cs" Inherits="AppWebInternetBanking.Views.frmPermiso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" >

        <script type="text/javascript src=" https://cdnjs.cloudflare.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/1.10.12/css/jquery.dataTables.min.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.2.2/css/buttons.dataTables.min.css" />
    <script type="text/javascript" src="https://cdn.datatables.net/1.10.12/js/jquery.dataTables.min.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/dataTables.buttons.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jszip/2.5.0/jszip.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/pdfmake.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.2/vfs_fonts.js"></script>
    <script type="text/javascript" src="https://cdn.datatables.net/buttons/1.2.2/js/buttons.html5.min.js"></script>
    <script type="text/javascript">
            $(document).ready(function () {
                $('[id*=gvPermisos]').prepend($("<thead></thead>").append($(this).find("tr:first"))).DataTable({
                    dom: 'Bfrtip',
                    'aoColumnDefs': [{ 'bSortable': true, 'aTargets': [0] }],
                    'iDisplayLength': 20,
                    buttons: [
                        { extend: 'copy', text: 'Copy to clipboard', className: 'exportExcel', exportOptions: { modifier: { page: 'all' } } },
                        { extend: 'excel', text: 'Export to Excel', className: 'exportExcel', filename: 'Perimsos_Excel', exportOptions: { modifier: { page: 'all' } } },
                        { extend: 'csv', text: 'Export to CSV', className: 'exportExcel', filename: 'Permisos_Csv', exportOptions: { modifier: { page: 'all' } } },
                        { extend: 'pdf', text: 'Export to PDF', className: 'exportExcel', filename: 'Perimsos_Pdf', orientation: 'landscape', pageSize: 'LEGAL', exportOptions: { modifier: { page: 'all' }, columns: ':visible' } }
                    ]
                });
            });

        function openModal() {
            $('#myModal').modal('show');
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
                $("#MainContent_gvLicencias tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
    </script>


    <h1>Mantenimiento de Permisos</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <asp:GridView ID="gvPermisos" runat="server" AutoGenerateColumns="false"
       CssClass="table table-sm" HeaderStyle-CssClass="thead-dark"
        HeaderStyle-BackColor="#1B1A1A" HeaderStyle-BorderStyle="None" BorderStyle="None" HeaderStyle-ForeColor="White"
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvPermisos_RowCommand">
        <Columns>
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Codigo" DataField="CodPermiso" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="ID Usuario" DataField="CodUsuario" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Tipo Permiso" DataField="TipoPermiso" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Emision" DataField="FechaEmision"  DataFormatString="{0:M/dd/yyyy}"/>
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderStyle-BorderStyle="None" ItemStyle-BackColor="#DEDAD4" ItemStyle-ForeColor="#1B1A1A" ItemStyle-BorderStyle="None" HeaderText="Vencimiento" DataField="FechaVencimiento" DataFormatString="{0:M/dd/yyyy}"/>

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
                                <asp:Literal ID="ltrCodigoUsu" Text="Usuario" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="DD_USU_CODIGO" runat="server" Enabled="true" CssClass="form-control" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrTipoPermiso" Text="Licencia" runat="server" /></td>
                            <td>
                                <asp:DropDownList ID="ddlTipoLicencia" runat="server" Enabled="true" CssClass="form-control">
                                    <asp:ListItem Value="A1">A1</asp:ListItem>
                                    <asp:ListItem Value="A2">A2</asp:ListItem>
                                    <asp:ListItem Value="A3">A3</asp:ListItem>
                                    <asp:ListItem Value="B1">B1</asp:ListItem>
                                    <asp:ListItem Value="B2">B2</asp:ListItem>
                                    <asp:ListItem Value="B3">B3</asp:ListItem>
                                    <asp:ListItem Value="C1">C1</asp:ListItem>
                                    <asp:ListItem Value="C2">C2</asp:ListItem>
                                    <asp:ListItem Value="C3">C3</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaE" Text="Fecha Emision" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaE" runat="server" Enabled="true" CssClass="form-control" TextMode="Date" /></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="ltrFechaV" Text="Fecha Vencimiento" runat="server" /></td>
                            <td>
                                <asp:TextBox ID="txtFechaV" runat="server" Enabled="true" CssClass="form-control" TextMode="Date"/></td>
                        </tr>
                    </table>
                    <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="false" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:LinkButton type="button" OnClick="btnAceptarMant_Click" CssClass="btn btn-success" ID="btnAceptarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                    <asp:LinkButton type="button" OnClick="btnCancelarMant_Click" CssClass="btn btn-danger" ID="btnCancelarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                </div>
            </div>
        </div>
    </div>

    <div id="myModal" class="modal fade" role="dialog">
        <div class="modal-dialog modal-sm">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Mantenimiento de licencias</h4>
                    </div>
                    <div class="modal-footer">
                        <p>
                            <asp:Literal ID="ltrModalMensaje" runat="server" /></p>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
                        <asp:LinkButton type="button" CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click1" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
                    </div>
                </div>
            </div>
        </div>
        </div>

    <div class="row">
            <div class="col-sm">
     <div id="canvas-holder" style="width:40%">
		            <canvas id="vistas-chart"></canvas>
	            </div>
              <script >
                  new Chart(document.getElementById("vistas-chart"), {
                      type: 'pie',
                      data: {
                          labels: [<%= this.labelsGrafico %>],
                          datasets: [{
                              label: "Total de lIncenias por tipo",
                              backgroundColor: [
                                  "#1e81b0",
                                  "#e28743",
                                  "#eab676",
                                  "#21130d",
                                  "#873e23",
                                  "#abdbe3",
                                  "#154c79",
                                  "#e4512f",
                                  "#c3a46e"
                              ],
                              data: [<%= this.dataGrafico %>]
                          }]
                      },
                      options: {
                          title: {
                              display: true,
                              text: 'Total de lIncenias por tipo'
                          }
                      }
                  });
              </script>
                </div>
            </div>
    

    

</asp:Content>