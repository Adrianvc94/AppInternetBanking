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
using System.Text;
using System.Threading.Tasks;

namespace AppWebInternetBanking.Views
{
    public partial class frmPermiso : System.Web.UI.Page
    {

        IEnumerable<Permiso> permisos = new ObservableCollection<Permiso>();
        PermisoManager permisoManager = new PermisoManager();
        IEnumerable<Usuario> usuarios = new ObservableCollection<Usuario>();
        UsuarioManager usuarioManager = new UsuarioManager();
        static string _codigo = string.Empty;

        public string labelsGrafico = string.Empty;
        public string backgroundcolorsGrafico = string.Empty;
        public string dataGrafico = string.Empty;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    permisos = await permisoManager.ObtenerPermisos2();
                    InicializarControles();
                    ObtenerDatosgrafico();
                }

            }

        }

        private void ObtenerDatosgrafico()
        {
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColors = new StringBuilder();

            var random = new Random();

            foreach (var error in permisos.GroupBy(e => e.TipoPermiso).
                Select(group => new
                { TipoPermiso = group.Key, Cantidad = group.Count()}).OrderBy(x => x.TipoPermiso))
            {
                string color = String.Format("#{0:X6}", random.Next(0x1000000));
                labels.Append(string.Format("'{0}',", error.TipoPermiso));
                data.Append(string.Format("'{0}',", error.Cantidad));
                backgroundColors.Append(string.Format("'{0}',", color));

                labelsGrafico = labels.ToString().Substring(0, labels.Length - 1);
                dataGrafico = data.ToString().Substring(0, data.Length - 1);
                backgroundcolorsGrafico = backgroundColors.ToString().Substring(backgroundColors.Length - 1);
            }

        }

        private bool ValidarCampos()
        {
            try
            {
                DateTime.ParseExact(txtFechaE.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime.ParseExact(txtFechaV.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                lblResultado.Text = "Debe ingresar todos las fechas requeridas";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon;
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

                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                DD_USU_CODIGO.DataTextField = "Username";
                DD_USU_CODIGO.DataValueField = "Codigo";
                DD_USU_CODIGO.DataSource = usuarios.ToList();
                DD_USU_CODIGO.DataBind();
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
                DateTime theDateE = DateTime.ParseExact(txtFechaE.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsertE = theDateE.ToString("dd-MM-yyyy");
                DateTime theDateV = DateTime.ParseExact(txtFechaV.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsertV = theDateE.ToString("dd-MM-yyyy");
                Permiso permiso = new Permiso()
                {
                    CodUsuario = Convert.ToInt32(DD_USU_CODIGO.SelectedValue.ToString()),
                    TipoPermiso = ddlTipoLicencia.SelectedValue.ToString(),
                    FechaEmision = Convert.ToDateTime(theDateE),
                    FechaVencimiento = Convert.ToDateTime(theDateV)
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

            else if (ValidarCampos() && !string.IsNullOrEmpty(txtCodigoMant.Text))
            {
                DateTime theDateE = DateTime.ParseExact(txtFechaE.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsertE = theDateE.ToString("dd-MM-yyyy");
                DateTime theDateV = DateTime.ParseExact(txtFechaV.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsertV = theDateE.ToString("dd-MM-yyyy");
                Permiso permiso = new Permiso() //modificar
                {
                    CodPermiso = Convert.ToInt32(txtCodigoMant.Text),
                    CodUsuario = Convert.ToInt32(DD_USU_CODIGO.SelectedValue.ToString()),
                    TipoPermiso = ddlTipoLicencia.SelectedValue.ToString(),
                    FechaEmision = Convert.ToDateTime(theDateE),
                    FechaVencimiento = Convert.ToDateTime(theDateV)
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
            btnAceptarModal.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            ltrCodigoUsu.Visible = true;
            //txtCodigoUsu.Visible = true;
            ltrTipoPermiso.Visible = true;
            //txtTipoPermiso.Visible = true;
            ltrFechaE.Visible = true;
            txtFechaE.Visible = true;
            ltrFechaV.Visible = true;
            txtFechaV.Visible = true;
            txtCodigoMant.Text = string.Empty;
            //txtTipoPermiso.Text = string.Empty;
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
                    txtFechaE.Text = row.Cells[3].Text.Trim();
                    txtFechaV.Text = row.Cells[4].Text.Trim();

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