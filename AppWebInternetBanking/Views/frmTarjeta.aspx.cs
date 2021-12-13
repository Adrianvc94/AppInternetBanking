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
    public partial class frmTarjeta : System.Web.UI.Page
    {
        IEnumerable<Tarjeta> tarjeta = new ObservableCollection<Tarjeta>();
        TarjetaManager tarjetaManager = new TarjetaManager();
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

        private async void ObtenerDatosgrafico()
        {
            if (tarjeta.Count() == 0)
            {
                tarjeta = await tarjetaManager.ObtenerTarjeta(Session["Token"].ToString());
            }

            StringBuilder labels = new StringBuilder();
            StringBuilder data = new StringBuilder();
            StringBuilder backgroundColors = new StringBuilder();

            var random = new Random();

            foreach (var t in tarjeta.GroupBy(e => e.Emisor).
                Select(group => new
                { Emisor = group.Key, Cantidad = group.Count() }).OrderBy(x => x.Emisor))
            {
                string color = String.Format("#{0:X6}", random.Next(0x1000000));
                labels.Append(string.Format("'{0}',", t.Emisor));
                data.Append(string.Format("'{0}',", t.Cantidad));
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
                tarjeta = await tarjetaManager.ObtenerTarjeta(Session["Token"].ToString());
                gvTarjeta.DataSource = tarjeta.ToList();
                gvTarjeta.DataBind();

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

        protected void gvTarjeta_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvTarjeta.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar crédito";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodigoMant.Text = row.Cells[0].Text.Trim();
                    //txtCodUsuario.Text = row.Cells[1].Text.Trim();
                    //txtTipo.Text = row.Cells[2].Text.Trim();
                    //txtEmisor.Text = row.Cells[3].Text.Trim();
                    txtFechaEmision.Text = row.Cells[4].Text.Trim();
                    txtFechaVencimiento.Text = row.Cells[5].Text.Trim();
                    btnAceptarMant.Visible = true;

                    ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
                    break;
                case "Eliminar":
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

            ltrTituloMantenimiento.Text = "Nueva Tarjeta";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;

            ltrCodigoMant.Visible = true;
            txtCodigoMant.Visible = true;

            ltrCodUsuario.Visible = true;

            ltrTipo.Visible = true;

            ltrEmisor.Visible = true;

            ltrFechaEmision.Visible = true;
            txtFechaEmision.Visible = true;

            ltrFechaVencimiento.Visible = true;
            txtFechaVencimiento.Visible = true;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtCodigoMant.Text) && valFecha()) //insertar
            {
                DateTime theDateE = DateTime.ParseExact(txtFechaEmision.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsertE = theDateE.ToString("MM-dd-yyyy");

                DateTime theDateV = DateTime.ParseExact(txtFechaVencimiento.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsertV = theDateV.ToString("MM-dd-yyyy");

                Tarjeta tarjeta = new Tarjeta()
                {
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    Tipo = ddTIPO.SelectedItem.ToString(),
                    Emisor = ddEMISOR.SelectedItem.ToString(),
                    FechaEmision = Convert.ToDateTime(theDateE),
                    FechaVencimiento = Convert.ToDateTime(theDateV)
                };

                Tarjeta tarjetaIngresada = await tarjetaManager.Ingresar(tarjeta, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(tarjetaIngresada.Emisor))
                {
                    lblResultado.Text = "Crédito ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                    ObtenerDatosgrafico();

                    Correo correo = new Correo();
                    correo.Enviar("Nueva tarjeta incluida", tarjetaIngresada.Emisor, "svillagra07@gmail.com",
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

            else if (!string.IsNullOrEmpty(txtCodigoMant.Text) && valFecha()) // modificar
            {
                DateTime theDateE = DateTime.ParseExact(txtFechaEmision.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string dateToInsertE = theDateE.ToString("dd-MM-yyyy");

                DateTime theDateV = DateTime.ParseExact(txtFechaVencimiento.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                string dateToInsertV = theDateV.ToString("dd-MM-yyyy");

                Tarjeta tarjeta = new Tarjeta()
                {
                    CodTarjeta = Convert.ToInt32(txtCodigoMant.Text),
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    Tipo = ddTIPO.SelectedItem.ToString(),
                    Emisor = ddEMISOR.SelectedItem.ToString(),
                    FechaEmision = Convert.ToDateTime(dateToInsertE),
                    FechaVencimiento = Convert.ToDateTime(dateToInsertV)
                };

                Tarjeta tarjetaActualizada = await tarjetaManager.Actualizar(tarjeta, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(tarjetaActualizada.Emisor) && !string.IsNullOrEmpty(tarjetaActualizada.Tipo))
                {
                    lblResultado.Text = "Tarjeta actualizada con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();
                    ObtenerDatosgrafico();

                    Correo correo = new Correo();
                    correo.Enviar("Tarjeta actualizada con exito", tarjetaActualizada.Emisor, "svillagra07@gmail.com",
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
                txtCodigoMant.Text = null;
            }
        }

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseMantenimiento(); });", true);
            ObtenerDatosgrafico();
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {
                Tarjeta tarjeta = await tarjetaManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(tarjeta.Emisor))
                {
                    ltrModalMensaje.Text = "Tarjeta eliminado";
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
            ObtenerDatosgrafico();
        }

        private bool valFecha()
        {
            try
            {
                DateTime.ParseExact(txtFechaEmision.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime.ParseExact(txtFechaVencimiento.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                lblResultado.Text = "Por favor ingrese las fechas";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon;
                return false;
            }
            return true;
        }
    }
}