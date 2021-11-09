
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
    public partial class frmPrestamo : System.Web.UI.Page
    {
        IEnumerable<Prestamo> prestamos = new ObservableCollection<Prestamo>();
        PrestamoManager prestamoManager = new PrestamoManager();
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
                prestamos = await prestamoManager.ObtenerPrestamos(Session["Token"].ToString());
                gvPrestamos.DataSource = prestamos.ToList();
                gvPrestamos.DataBind();

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


        protected void gvPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvPrestamos.Rows[index];

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar servicio";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";
                    txtCodPrestamoMant.Text = row.Cells[0].Text.Trim();
                    ddUSU_CODIGO.SelectedValue = row.Cells[1].Text.Trim();
                    txtMontoPrestamoMant.Text = row.Cells[2].Text.Trim();
                    txtFechaSolicitudMant.Text = row.Cells[3].Text.Trim();
                    txtFechaLimiteMant.Text = row.Cells[4].Text.Trim();
                    ddlTasaInteres.SelectedValue = row.Cells[4].Text.Trim();
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

        protected void btnNuevo_Click(object sender, EventArgs e)
        {
            ltrTituloMantenimiento.Text = "Nuevo Sobre";
            btnAceptarMant.ControlStyle.CssClass = "btn btn-success";
            btnAceptarMant.Visible = true;
            ltrCodPrestamoMant.Visible = true;
            txtCodPrestamoMant.Visible = true;
            ltrCodUsuarioMant.Visible = true;
            ddUSU_CODIGO.Visible = true;
            ltrMontoPrestamoMant.Visible = true;
            txtMontoPrestamoMant.Visible = true;
            ltrFechaSolicitudMant.Visible = true;
            txtFechaSolicitudMant.Visible = true;
            ltrFechaLimiteMant.Visible = true;
            txtFechaLimiteMant.Visible = true;
            ltrTasaInteresMant.Visible = true;
            ddlTasaInteres.Visible = true;
            ScriptManager.RegisterStartupScript(this,
    this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }

        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCodPrestamoMant.Text) && validar() && validarCampos())  //insertar
            {
                DateTime theDate = DateTime.ParseExact(txtFechaSolicitudMant.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsert = theDate.ToString("dd-MM-yyyy");
                DateTime theDate2 = DateTime.ParseExact(txtFechaLimiteMant.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsert2 = theDate2.ToString("dd-MM-yyyy");
                Prestamo prestamo = new Prestamo()
                {

                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedValue.ToString()),
                    FechaSolicitud = Convert.ToDateTime(theDate),
                    FechaLimite = Convert.ToDateTime(theDate2),
                    MontoPrestamo = Convert.ToDecimal(txtMontoPrestamoMant.Text),
                    TasaInteres = Convert.ToDecimal(ddlTasaInteres.SelectedValue)

                };

                Prestamo prestamoIngresar = await prestamoManager.Ingresar(prestamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(prestamoIngresar.CodPrestamo)))
                {
                    lblResultado.Text = "Prestamo ingresado con exito";
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
            else if (validarCampos() && validar() && !string.IsNullOrEmpty(txtCodPrestamoMant.Text)) //modificar
            {
                DateTime theDate = DateTime.ParseExact(txtFechaSolicitudMant.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsert = theDate.ToString("dd-MM-yyyy");
                DateTime theDate2 = DateTime.ParseExact(txtFechaLimiteMant.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                //string dateToInsert2 = theDate2.ToString("dd-MM-yyyy");
                Prestamo prestamo = new Prestamo()
                {
                    CodPrestamo = Convert.ToInt32(txtCodPrestamoMant.Text),
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedValue.ToString()),
                    FechaSolicitud = Convert.ToDateTime(theDate),
                    FechaLimite = Convert.ToDateTime(theDate2),
                    MontoPrestamo = Convert.ToDecimal(txtMontoPrestamoMant.Text),
                    TasaInteres = Convert.ToDecimal(ddlTasaInteres.SelectedValue)
                };

                Prestamo prestamoActualizado = await prestamoManager.Actualizar(prestamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(Convert.ToString(prestamoActualizado.CodPrestamo)))
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

        protected void btnCancelarMant_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "LaunchServerSide", "$(function() { CloseModal(); });", true);
        }

        protected async void btnAceptarModal_Click(object sender, EventArgs e)
        {
            try
            {
                Prestamo prestamo = await prestamoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(Convert.ToString(prestamo.CodPrestamo)))
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
                    Vista = "frmPrestamo.aspx",
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
        }


        private bool validar()
        {
            try
            {
                DateTime.ParseExact(txtFechaSolicitudMant.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                DateTime.ParseExact(txtFechaLimiteMant.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
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

        private bool validarCampos()
        {
            if (string.IsNullOrEmpty(txtMontoPrestamoMant.Text))
            {
                lblResultado.Text = "Debe ingresar el monto del prestamo";
                lblResultado.Visible = true;
                lblResultado.ForeColor = Color.Maroon;
                return false;
            }
            return true;
        }
    }


}