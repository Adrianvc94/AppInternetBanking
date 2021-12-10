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
    public partial class frmLicencia : System.Web.UI.Page
    {
        IEnumerable<Licencia> licencias = new ObservableCollection<Licencia>();
        LicenciaManager licenciaManager = new LicenciaManager();
        IEnumerable<Usuario> usuarios = new ObservableCollection<Usuario>();
        UsuarioManager usuarioManager = new UsuarioManager();
        static string _codigo = string.Empty;

        public string labelsGrafico = string.Empty;
        public string backgroundcolorsGrafico = string.Empty;
        public string dataGrafico = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["CodigoUsuario"] == null)
                    Response.Redirect("~/Login.aspx");
                else
                {
                    InicializarControles();
                    ObtenerDatosgrafico();
                }

            }
        }

        private async void ObtenerDatosgrafico()
        {
            if (licencias.Count() == 0)
            {
                licencias = await licenciaManager.ObtenerLicencias2();
            }
            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColors = new StringBuilder();

            var random = new Random();

            foreach (var error in licencias.GroupBy(e => e.TipoLicencia).
                Select(group => new
                { TipoLicencia = group.Key, Cantidad = group.Count() }).OrderBy(x => x.TipoLicencia))
            {
                string color = String.Format("#{0:X6}", random.Next(0x1000000));
                labels.Append(string.Format("'{0}',", error.TipoLicencia));
                data.Append(string.Format("'{0}',", error.Cantidad));
                backgroundColors.Append(string.Format("'{0}',", color));

                labelsGrafico = labels.ToString().Substring(0, labels.Length - 1);
                dataGrafico = data.ToString().Substring(0, data.Length - 1);
                backgroundcolorsGrafico = backgroundColors.ToString().Substring(backgroundColors.Length - 1);
            }

        }


        private async void InicializarControles()
        {
            try
            {
                licencias = await licenciaManager.ObtenerLicencias(Session["Token"].ToString());
                gvLicencias.DataSource = licencias.ToList();
                gvLicencias.DataBind();

                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                DD_USU_CODIGO.DataTextField = "Username";
                DD_USU_CODIGO.DataValueField = "Codigo";
                DD_USU_CODIGO.DataSource = usuarios.ToList();
                DD_USU_CODIGO.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de licencias";
                lblStatus.Visible = true;
            }
        }



        private bool ValidarCampos()
        {

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

        protected async void btnAceptarMant_Click1(object sender, EventArgs e)
        {
            if (ValidarCampos() && string.IsNullOrEmpty(txtCodigoMant.Text)) //insertar
            {
                Licencia licencia = new Licencia()
                {
                    CodUsuario = Convert.ToInt32(DD_USU_CODIGO.SelectedValue),
                    TipoLicencia = ddlTipoLicencia.SelectedValue.ToString(),
                    FechaEmision = Convert.ToDateTime(txtFechaE.Text),
                    FechaVencimiento = Convert.ToDateTime(txtFechaV.Text)
                };

                Licencia licenciaIngresada = await licenciaManager.Ingresar(licencia, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(licenciaIngresada.TipoLicencia))
                {
                    lblResultado.Text = "Licencia ingresada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                    ObtenerDatosgrafico();

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
                Licencia licencia = new Licencia()
                {
                    CodLicencia = Convert.ToInt32(txtCodigoMant.Text),
                    CodUsuario = Convert.ToInt32(DD_USU_CODIGO.SelectedValue),
                    TipoLicencia = ddlTipoLicencia.SelectedValue.ToString(),
                    FechaEmision = Convert.ToDateTime(txtFechaE.Text),
                    FechaVencimiento = Convert.ToDateTime(txtFechaV.Text)
                };

                Licencia licenciaActualizado = await licenciaManager.Actualizar(licencia, Session["Token"].ToString()); //error

                if (!string.IsNullOrEmpty(licenciaActualizado.TipoLicencia))
                {
                    lblResultado.Text = "Licencia actualizada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                    ObtenerDatosgrafico();

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
            ObtenerDatosgrafico();
        }

        protected async void btnAceptarModal_Click1(object sender, EventArgs e)
        {
            try
            {
                Licencia licencia = await licenciaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(licencia.TipoLicencia))
                {
                    ltrModalMensaje.Text = "Licencia eliminado";
                    btnAceptarModal.Visible = false;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { openModal(); });", true);
                    InicializarControles();
                    ObtenerDatosgrafico();
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
                    Vista = "frmServicio.aspx",
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            ObtenerDatosgrafico();
        }

        protected void btnNuevo_Click1(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nueva licencia";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;
            ltrCodigoUsu.Visible = true;
            //txtCodigoUsu.Visible = true;
            ltrTipoLicencia.Visible = true;
            //ddlTipoLicencia.Visible = true;
            ltrFechaE.Visible = true;
            txtFechaE.Visible = true;
            ltrFechaV.Visible = true;
            txtFechaV.Visible = true;
            txtCodigoMant.Text = string.Empty;
            //txtTipoLicencia.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

        }

        protected void gvLicencias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvLicencias.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar Licencia";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    ///txtTipoLicencia.Text = row.Cells[1].Text.Trim();
                    //
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