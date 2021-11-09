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
    public partial class frmPermiso : System.Web.UI.Page
    {

        IEnumerable<Permiso> permisos = new ObservableCollection<Permiso>();
        PermisoManager permisoManager = new PermisoManager();
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

        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtCodigoUsu.Text))
            {
                lblResultado.Text = "Debe ingresar un codigo de usuario";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtTipoPermiso.Text))
            {
                lblResultado.Text = "Debe ingresar un tipo de Permiso";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtFechaE.Text))
            {
                lblResultado.Text = "Debe ingresar un fecha";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtFechaV.Text))
            {
                lblResultado.Text = "Debe ingresar un fecha";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            return true;
        }

        private async void InicializarControles()
        {
            try
            {
                permisos = await permisoManager.ObtenerPermisos(Session["Token"].ToString());
                gvPermisos.DataSource = permisos.ToList();
                gvPermisos.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de permisos";
                lblStatus.Visible = true;
            }
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {

            if (ValidarCampos() && string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Permiso permiso = new Permiso()
                {
                    CodUsuario = Convert.ToInt32(txtCodigoUsu.Text),
                    TipoPermiso = txtTipoPermiso.Text,
                    FechaEmision = Convert.ToDateTime(txtFechaE.Text),
                    FechaVencimiento = Convert.ToDateTime(txtFechaV.Text)
                };

                Permiso permisoIngresada = await permisoManager.Ingresar(permiso, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(permisoIngresada.TipoPermiso))
                {
                    lblResultado.Text = "Permiso ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

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

            else if (ValidarCampos() && string.IsNullOrEmpty(txtCodigoMant.Text))
            {
                Permiso permiso = new Permiso() //modificar
                {
                    CodPermiso = Convert.ToInt32(txtCodigoMant.Text),
                    CodUsuario = Convert.ToInt32(txtCodigoUsu.Text),
                    TipoPermiso = txtTipoPermiso.Text,
                    FechaEmision = Convert.ToDateTime(txtFechaE.Text),
                    FechaVencimiento = Convert.ToDateTime(txtFechaV.Text)
                };

                Permiso permisoActualizado = await permisoManager.Actualizar(permiso, Session["Token"].ToString()); 

                if (!string.IsNullOrEmpty(permisoActualizado.TipoPermiso))
                {
                    lblResultado.Text = "Permiso actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

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
                Permiso permiso = await permisoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(permiso.TipoPermiso))
                {
                    ltrModalMensaje.Text = "Permiso eliminado";
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
                    Vista = "frmPermiso.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult,
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }

        protected void btnCancelarModal_Click1(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nueva licencia";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            ltrCodigoUsu.Visible = true;
            txtCodigoUsu.Visible = true;
            ltrTipoPermiso.Visible = true;
            txtTipoPermiso.Visible = true;
            ltrFechaE.Visible = true;
            txtFechaE.Visible = true;
            ltrFechaV.Visible = true;
            txtFechaV.Visible = true;
            txtCodigoMant.Text = string.Empty;
            txtTipoPermiso.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

        }

        protected void gvPermisos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPermisos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar Licencia";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                   
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