<%@ Page Async="true" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="frmDepositoPlazo.aspx.cs" Inherits="AppWebInternetBanking.Views.frmDepositoPlazo" %>
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
                $("#MainContent_gvServicios tr").filter(function () {
                    $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1)
                });
            });
        });
     </script> 

    <h1>Mantenimiento de Deposito a Plazo</h1>
    <input id="myInput" placeholder="Buscar" class="form-control" type="text" />
    <br />
    <asp:GridView ID="gvDepositoPlazo" runat="server" AutoGenerateColumns="false"
      CssClass="table table-sm text-center" HeaderStyle-CssClass="thead-dark" 
        HeaderStyle-BackColor="#243054" HeaderStyle-ForeColor="White" 
        AlternatingRowStyle-BackColor="LightBlue" OnRowCommand="gvDepositoPlazo_RowCommand" >
        <Columns>

            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderText="Codigo" DataField="CodPlazo" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderText="Usuario" DataField="CodUsuario" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderText="Cantidad de Plazos" DataField="CantidadPlazos" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderText="Total a pagar" DataField="TotalPagar" />
            <asp:BoundField HeaderStyle-CssClass="text-center" HeaderText="Moneda" DataField="CodMoneda" />

            <asp:ButtonField HeaderText="Modificar" CommandName="Modificar" HeaderStyle-CssClass="text-center"
                ControlStyle-CssClass="btn btn-primary" ButtonType="Button" Text="Modificar" />
            <asp:ButtonField HeaderText="Eliminar" CommandName="Eliminar" HeaderStyle-CssClass="text-center"
                ControlStyle-CssClass="btn btn-danger" ButtonType="Button" Text="Eliminar" />
        </Columns>
    </asp:GridView>
    <asp:LinkButton type="Button" CssClass="btn btn-success" ID="btnNuevo" runat="server" OnClick="btnNuevo_Click"
      Text="<span aria-hidden='true' class='glyphicon glyphicon-floppy-disk'></span> Nuevo"    />
    <br />
    <asp:Label ID="lblStatus" ForeColor="Maroon" runat="server" Visible="false" />




     <!--VENTANA DE MANTENIMIENTO -->
  <div id="myModalMantenimiento" class="modal fade" role="dialog">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title"><asp:Literal ID="ltrTituloMantenimiento" runat="server"></asp:Literal></h4>
      </div>
      <div class="modal-body">
          <table style="width: 100%;">
              <tr>
                  <td><asp:Literal ID="ltrCodigoPlazo" Text="Codigo" runat="server" /></td>
                  <td><asp:TextBox ID="txtCodigoPlazo" runat="server" Enabled="false" CssClass="form-control" /></td>
              </tr>
              <tr>
                 <td><asp:Literal ID="ltrCodUsuario" Text="Usuario" runat="server" /></td>
                 <td><asp:DropDownList ID="ddUSU_CODIGO" CssClass="form-control" runat="server"></asp:DropDownList></td>
              </tr>

              <tr>
                 <td><asp:Literal ID="ltrCantidadPlazos" Text="Cantidad de Plazos" runat="server" /></td>
                 <td><asp:TextBox ID="txtCantidadPlazos" TextMode="Number" runat="server" CssClass="form-control" /></td>
              </tr>
              
              <tr>
                 <td><asp:Literal ID="ltrTotalPagar" Text="Total a pagar" runat="server" /></td>
                 <td><asp:TextBox ID="txtTotalPagar" TextMode="Number" runat="server" CssClass="form-control" /></td>
              </tr>

              <tr>
                 <td><asp:Literal ID="ltrCodMoneda" Text="Moneda" runat="server" /></td>
                 <td><asp:DropDownList ID="ddUSU_MONEDA" CssClass="form-control" runat="server"></asp:DropDownList></td>
              </tr>
        

          </table>
          <asp:Label ID="lblResultado" ForeColor="Maroon" Visible="False" runat="server" />
      </div>
      <div class="modal-footer">
        <asp:LinkButton type="button" OnClick="btnAceptarMant_Click" CssClass="btn btn-success" ID="btnAceptarMant" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
         <asp:LinkButton type="button" OnClick="btnCancelarMant_Click"  CssClass="btn btn-danger" ID="btnCancelarMant"  runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
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
        <h4 class="modal-title">Mantenimiento de Deposito a Plazos</h4>
      </div>
      <div class="modal-body">
        <p><asp:Literal ID="ltrModalMensaje" runat="server" /></p>
      </div>
      <div class="modal-footer">
         <asp:LinkButton type="button" CssClass="btn btn-success" ID="btnAceptarModal" OnClick="btnAceptarModal_Click"  runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-ok'></span> Aceptar" />
         <asp:LinkButton type="button"  CssClass="btn btn-danger" ID="btnCancelarModal" OnClick="btnCancelarModal_Click" runat="server" Text="<span aria-hidden='true' class='glyphicon glyphicon-remove'></span> Cerrar" />
      </div>
    </div>
  </div>
</div>


</asp:Content>
