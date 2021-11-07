using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking.Views
{
    public partial class frmDepositoPlazo : System.Web.UI.Page
    {
        IEnumerable<DepositoPlazo> depositoPlazos = new ObservableCollection<DepositoPlazo>();
        DepositoPlazoManager depositoManager = new DepositoPlazoManager();

        IEnumerable<Usuario> usuarios = new ObservableCollection<Usuario>();
        UsuarioManager usuarioManager = new UsuarioManager();

        IEnumerable<Moneda> monedas = new ObservableCollection<Moneda>();
        MonedaManager monedaManager = new MonedaManager();
        static string _codigo = string.Empty;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                    InicializarControles();
            }
        }


        private async void InicializarControles()
        {
            try
            {
                depositoPlazos = await depositoManager.ObtenerDepositoPlazos(Session["Token"].ToString());
                gvDepositoPlazo.DataSource = depositoPlazos.ToList();
                gvDepositoPlazo.DataBind();


                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                ddUSU_CODIGO.DataTextField = "Username";
                ddUSU_CODIGO.DataValueField = "Codigo";
                ddUSU_CODIGO.DataSource = usuarios.ToList();
                ddUSU_CODIGO.DataBind();

                monedas = await monedaManager.ObtenerMonedas(Session["Token"].ToString());
                ddUSU_MONEDA.DataTextField = "Descripcion";
                ddUSU_MONEDA.DataValueField = "Codigo";
                ddUSU_MONEDA.DataSource = monedas.ToList();
                ddUSU_MONEDA.DataBind();



            }
            catch (Exception e)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de depositos a plazo";
                lblStatus.Visible = true;
            }
        }


        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtCantidadPlazos.Text) || (Convert.ToInt32(txtCantidadPlazos.Text) <= 0))
            {
                lblResultado.Text = "Debe ingresar una cantidad de plazos valida";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtTotalPagar.Text) || (Convert.ToDecimal(txtTotalPagar.Text) <= 0))
            {
                lblResultado.Text = "Debe ingresar un total a pagar valido";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            return true;
        }



        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {

            if (ValidarCampos() && string.IsNullOrEmpty(txtCodigoPlazo.Text)) //insertar
            {
                DepositoPlazo depositoPlazo = new DepositoPlazo()
                {
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    CantidadPlazos = Convert.ToInt32(txtCantidadPlazos.Text),
                    TotalPagar = Convert.ToDecimal(txtTotalPagar.Text),
                    CodMoneda = Convert.ToInt32(ddUSU_MONEDA.SelectedItem.Value.ToString()),
                };

                DepositoPlazo depositoIngresado = await depositoManager.Ingresar(depositoPlazo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(depositoIngresado.CantidadPlazos)))
                {
                    lblResultado.Text = "Deposito a plazo ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo deposito a plazo incluido", Convert.ToString(depositoIngresado.CantidadPlazos), "svillagra07@gmail.com",
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                    ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
            }
            else if (ValidarCampos() && (!string.IsNullOrEmpty(txtCodigoPlazo.Text)))// modificar
            {

                DepositoPlazo depositoPlazo = new DepositoPlazo()
                {
                    CodPlazo = Convert.ToInt32(txtCodigoPlazo.Text),
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    CantidadPlazos = Convert.ToInt32(txtCantidadPlazos.Text),
                    TotalPagar = Convert.ToDecimal(txtTotalPagar.Text),
                    CodMoneda = Convert.ToInt32(ddUSU_MONEDA.SelectedItem.Value.ToString()),
                };


                DepositoPlazo depositoActualizado = await depositoManager.Actualizar(depositoPlazo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(depositoActualizado.CantidadPlazos)))
                {
                    lblResultado.Text = "Deposito a Plazo actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Deposito a Plazo actualizado con exito", Convert.ToString(depositoActualizado.CantidadPlazos), "svillagra07@gmail.com",
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                    ScriptManager.RegisterStartupScript(this,
                    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this,
                  this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
            }
        }


        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);

        }


        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {
                DepositoPlazo depositoPlazo = await depositoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(Convert.ToString(depositoPlazo.CantidadPlazos)))
                {
                    ltrModalMensaje.Text = "Deposito a Plazo eliminado";
                    btnAceptarModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); });", true);
                    InicializarControles();
                }
            }
            catch (Exception ex)
            {
                ErrorManager errorManager = new ErrorManager();
                Error error = new Error()
                {
                    CodigoUsuario =
                    Convert.ToInt32(Session["CodigoUsuario"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = "frmDepositoPlazo.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult,
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }


        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }


        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo mantenimiento";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoPlazo.Visible = true;
            txtCodigoPlazo.Visible = true;

            ltrCodUsuario.Visible = true;
            ltrCodMoneda.Visible = true;

            ltrCantidadPlazos.Visible = true;
            txtCantidadPlazos.Visible = true;


            ltrTotalPagar.Visible = true;
            txtTotalPagar.Visible = true;



            txtCodigoPlazo.Text = string.Empty;
            txtCantidadPlazos.Text = string.Empty;
            txtTotalPagar.Text = string.Empty;
            lblResultado.Visible = false;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }



        protected void gvDepositoPlazo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvDepositoPlazo.Rows[index];
            lblResultado.Visible = false;

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar Deposito a Plazo";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";


                    txtCodigoPlazo.Text = row.Cells[0].Text.Trim();
                    txtCantidadPlazos.Text = row.Cells[2].Text.Trim();
                    txtTotalPagar.Text = row.Cells[3].Text.Trim();


                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el servicio?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    btnAceptarModal.Visible = true;
                    break;
                default:
                    break;
            }
        }


    }
}