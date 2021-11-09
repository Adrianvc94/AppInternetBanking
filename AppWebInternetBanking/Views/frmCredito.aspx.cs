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
    public partial class frmCredito : System.Web.UI.Page
    {
        IEnumerable<Credito> credito = new ObservableCollection<Credito>();
        CreditoManager creditoManager = new CreditoManager();
        IEnumerable<Usuario> usuarios = new ObservableCollection<Usuario>();
        UsuarioManager usuarioManager = new UsuarioManager();
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
                credito = await creditoManager.ObtenerCredito(Session["Token"].ToString());
                gvCredito.DataSource = credito.ToList();
                gvCredito.DataBind();

                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                ddUSU_CODIGO.DataTextField = "Username";
                ddUSU_CODIGO.DataValueField = "Codigo";
                ddUSU_CODIGO.DataSource = usuarios.ToList();
                ddUSU_CODIGO.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de créditos";
                lblStatus.Visible = true;
            }
        }

        protected void gvCredito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvCredito.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar crédito";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    //txtCodUsuario.Text = row.Cells[1].Text.Trim();
                    txtMontoCredito.Text = row.Cells[2].Text.Trim();
                    txtDescripcion.Text = row.Cells[3].Text.Trim();
                    txtFechaSolicitud.Text = row.Cells[4].Text.Trim();
                    btnAceptarMant.Visible = true;

                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    btnAceptarModal.Visible = true;
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el crédito?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }

        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {

            ltrTituloMantenimiento.Text = "Nuevo credito";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodUsuario.Visible = true;
            //txtCodUsuario.Visible = true;

            ltrMontoCredito.Visible = true;
            txtMontoCredito.Visible = true;

            txtDescripcion.Visible = true;
            ltrDescripcion.Visible = true;

            ltrFechaSolicitud.Visible = true;
            txtFechaSolicitud.Visible = true;



            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodigoMant.Text) && validar()) //insertar
            {
                DateTime theDate = DateTime.ParseExact(txtFechaSolicitud.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string dateToInsert = theDate.ToString("dd-MM-yyyy");

                Credito credito = new Credito()
                {
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    MontoCredito = Convert.ToDecimal(txtMontoCredito.Text),
                    Descripcion = txtDescripcion.Text,
                    FechaSolicitud = Convert.ToDateTime(dateToInsert)
                };

                Credito creditoIngresado = await creditoManager.Ingresar(credito, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(creditoIngresado.Descripcion))
                {
                    lblResultado.Text = "Crédito ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo crédito incluido", creditoIngresado.Descripcion, "svillagra07@gmail.com",
                        Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;

                    ScriptManager.RegisterStartupScript(this,
                       this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }

            }

            else if (!string.IsNullOrEmpty(txtCodigoMant.Text) && validar()) // modificar
            {

                DateTime theDate = DateTime.ParseExact(txtFechaSolicitud.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string dateToInsert = theDate.ToString("dd-MM-yyyy");

                Credito credito = new Credito()
                {
                    CodCredito = Convert.ToInt32(txtCodigoMant.Text),
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    MontoCredito = Convert.ToDecimal(txtMontoCredito.Text),
                    Descripcion = txtDescripcion.Text,
                    FechaSolicitud = Convert.ToDateTime(dateToInsert)
                };

                Credito creditoActualizado = await creditoManager.Actualizar(credito, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(creditoActualizado.Descripcion))
                {
                    lblResultado.Text = "Crédito actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Crédito actualizado con exito", creditoActualizado.Descripcion, "svillagra07@gmail.com",
                                    Convert.ToInt32(Session["CodigoUsuario"].ToString()));

                    ScriptManager.RegisterStartupScript(this,
                        this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

                    txtCodigoMant.Text = null;
                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;

                    ScriptManager.RegisterStartupScript(this,
                       this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

                    txtCodigoMant.Text = null;
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

                Credito credito = await creditoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(credito.Descripcion))
                {
                    ltrModalMensaje.Text = "Credito eliminado";
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
                    Vista = "frmCredito.aspx",
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

        private bool validar()
        {
            try
            {
                DateTime.ParseExact(txtFechaSolicitud.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                lblResultado.Text = "Por favor rellene todos los espacios";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon;
                return false;
            }
            if (string.IsNullOrEmpty(txtDescripcion.Text) || string.IsNullOrEmpty(Convert.ToString(txtMontoCredito.Text))) 
            {
                lblResultado.Text = "Por favor rellene todos los espacios";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon;
                return false;
            }
            return true;
        }
    }
}