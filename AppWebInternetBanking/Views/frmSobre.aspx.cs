using AppWebInternetBanking.Controllers;
using AppWebInternetBanking.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppWebInternetBanking.Views
{
    public partial class frmSobre : System.Web.UI.Page
    {
        IEnumerable<Sobre> sobres = new ObservableCollection<Sobre>();
        SobreManager sobreManager = new SobreManager();
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
                    InicializarControles();
                ObtenerDatosgrafico();
            }
        }

        private async void InicializarControles()
        {
            try
            {
                sobres = await sobreManager.ObtenerSobres(Session["Token"].ToString());
                gvSobres.DataSource = sobres.ToList();
                gvSobres.DataBind();

                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                ddUSU_CODIGO.DataTextField = "Username";
                ddUSU_CODIGO.DataValueField = "Codigo";
                ddUSU_CODIGO.DataSource = usuarios.ToList();
                ddUSU_CODIGO.DataBind();
            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de servicios";
                lblStatus.Visible = true;
            }
        }


        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (ValidarCampo() == true && string.IsNullOrEmpty(txtCodSobreMant.Text))  //insertar
            {
                Sobre sobre = new Sobre()
                {
                    CodCuenta = Convert.ToInt32(ddUSU_CODIGO.SelectedValue.ToString()),
                    Saldo = Convert.ToDecimal(txtSaldoMant.Text),
                    Descripcion = txtDescripcionMant.Text,
                    CodMoneda = Convert.ToInt32(ddlCodMoneda.SelectedValue)
                };

                Sobre sobreIngresado = await sobreManager.Ingresar(sobre, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(sobreIngresado.Descripcion))
                {
                    lblResultado.Text = "Sobre ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                }
                else
                {
                    lblResultado.Text = "Hubo un error al efectuar la operacion";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Maroon;
                }
            }
            else if (ValidarCampo() == true && !string.IsNullOrEmpty(txtCodSobreMant.Text))//modificar
            {
                Sobre sobre = new Sobre()
                {
                    CodSobre = Convert.ToInt32(txtCodSobreMant.Text),
                    CodCuenta = Convert.ToInt32(ddUSU_CODIGO.SelectedValue.ToString()),
                    Saldo = Convert.ToDecimal(txtSaldoMant.Text),
                    Descripcion = txtDescripcionMant.Text,
                    CodMoneda = Convert.ToInt32(ddlCodMoneda.SelectedValue)
                };

                Sobre sobreActualizado = await sobreManager.Actualizar(sobre, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(sobreActualizado.Descripcion))
                {
                    lblResultado.Text = "Sobre actualizado con exito";
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

        protected void btnCancelarModal_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {
                Sobre sobre = await sobreManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(sobre.Descripcion))
                {
                    ltrModalMensaje.Text = "Sobre eliminado";
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
                    Convert.ToInt32(Session["CodigoUsuaio"].ToString()),
                    FechaHora = DateTime.Now,
                    Vista = "frmSobre.aspx",
                    Accion = "btnAceptarModal_Click",
                    Fuente = ex.Source,
                    Numero = ex.HResult,
                    Descripcion = ex.Message
                };
                Error errorIngresado = await errorManager.Ingresar(error);
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo Sobre";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodSobreMant.Visible = true;
            txtCodSobreMant.Visible = true;
            ltrCodCuentaMant.Visible = true;
            //txtCodCuentaMant.Visible = true;
            ltrSaldoMant.Visible = true;
            txtSaldoMant.Visible = true;
            ltrDescripcionMant.Visible = true;
            txtDescripcionMant.Visible = true;
            ddlCodMoneda.Visible = true;
            txtCodSobreMant.Text = string.Empty;
            txtDescripcionMant.Text = string.Empty;
            ScriptManager.RegisterStartupScript(this,
    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);

        }

        protected void gvSobres_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvSobres.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodSobreMant.Text = row.Cells[0].Text.Trim();
                    ddUSU_CODIGO.SelectedValue = row.Cells[1].Text.Trim();
                    txtSaldoMant.Text = row.Cells[2].Text.Trim();
                    txtDescripcionMant.Text = row.Cells[3].Text.Trim();
                    btnAceptarMant.Visible = true;
                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
                    _codigo = row.Cells[0].Text.Trim();
                    ltrModalMensaje.Text = "Esta seguro que desea eliminar el servicio?";
                    ScriptManager.RegisterStartupScript(this,
               this.GetType(), "LaunchServerSide", "$(function() {openModal(); } );", true);
                    break;
                default:
                    break;
            }
        }

        private bool ValidarCampo()
        {
            if (string.IsNullOrEmpty(txtDescripcionMant.Text))
            {
                lblResultado.Text = "Ingrese una descripcion";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            if (string.IsNullOrEmpty(txtSaldoMant.Text) || (Convert.ToDecimal(txtSaldoMant.Text) <= 0))
            {
                lblResultado.Text = "Ingrese un valor valido de saldo";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            return true;
        }

        private async void ObtenerDatosgrafico()
        {

            if (sobres.Count() == 0)
            {
                sobres = await sobreManager.ObtenerSobres(Session["Token"].ToString());
            }

            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColors = new StringBuilder();

            var random = new Random();

            foreach (var sobre in sobres.OrderBy(x => x.Descripcion).Take(10))
            {
                string color = String.Format("#{0:X6}", random.Next(0x1000000));
                labels.Append(string.Format("'{0} {1}',", "descripcion de sobre:", sobre.Descripcion));
                data.Append(string.Format("'{0}',", sobre.Saldo));
                backgroundColors.Append(string.Format("'{0}',", color));

                labelsGrafico = labels.ToString().Substring(0, labels.Length - 1);
                dataGrafico = data.ToString().Substring(0, data.Length - 1);
                backgroundcolorsGrafico = backgroundColors.ToString().Remove(backgroundColors.Length - 1, 1);
            }

        }


    }
}