﻿using AppWebInternetBanking.Controllers;
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
    public partial class frmMarchamo : System.Web.UI.Page
    {
        IEnumerable<Marchamo> marchamos = new ObservableCollection<Marchamo>();
        MarchamoManager marchamoManager = new MarchamoManager();
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
                marchamos = await marchamoManager.ObtenerMarchamos(Session["Token"].ToString());
                gvMarchamos.DataSource = marchamos.ToList();
                gvMarchamos.DataBind();


                usuarios = await usuarioManager.ObtenerUsuarios(Session["Token"].ToString());
                ddUSU_CODIGO.DataTextField = "Username";
                ddUSU_CODIGO.DataValueField = "Codigo";
                ddUSU_CODIGO.DataSource = usuarios.ToList();
                ddUSU_CODIGO.DataBind();



            }
            catch (Exception)
            {
                lblStatus.Text = "Hubo un error al cargar la lista de marchamos";
                lblStatus.Visible = true;
            }
        }



        private bool ValidarCampos()
        {
            if (string.IsNullOrEmpty(txtPlaca.Text))
            {
                lblResultado.Text = "Debe ingresar la placa del vehiculo";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            //if (string.IsNullOrEmpty(txtMonto.Text) || (Convert.ToDecimal(txtMonto.Text) <= 0))
            //{
            //    lblResultado.Text = "Debe ingresar un monto valido";
            //    lblResultado.ForeColor = Color.Maroon;
            //    lblResultado.Visible = true;
            //    return false;
            //}
            if (string.IsNullOrEmpty(txtValorVehiculo.Text) || (Convert.ToDecimal(txtValorVehiculo.Text) <= 0))
            {
                lblResultado.Text = "Debe ingresar un valor valido del vehiculo";
                lblResultado.ForeColor = Color.Maroon;
                lblResultado.Visible = true;
                return false;
            }
            //if (string.IsNullOrEmpty(txtTasa.Text) || (Convert.ToDecimal(txtTasa.Text) <= 0))
            //{
            //    lblResultado.Text = "Debe ingresar una tasa valida";
            //    lblResultado.ForeColor = Color.Maroon;
            //    lblResultado.Visible = true;
            //    return false;
            //}
            return true;
        }


        private decimal CalcularMarchamo()
        {
            double valorFiscal = Convert.ToDouble(txtValorVehiculo.Text);

            double TotalMarchamo = ( (valorFiscal * (0.535 / 100)) + (valorFiscal * (0.0695 / 100)) + (valorFiscal * (0.244 / 100)) + (valorFiscal * (2.481 / 100)) + (valorFiscal * (0.00427 / 100)) + (valorFiscal * (0.0238 / 100)) + (valorFiscal * (0.0409 / 100)) );

            return Convert.ToDecimal(TotalMarchamo);
        }


        protected async void btnAceptarMant_Click(object sender, EventArgs e)
        {

            if (ValidarCampos() && string.IsNullOrEmpty(txtCodigoMarchamo.Text)) //insertar
            {
                Marchamo marchamo = new Marchamo()
                {
                    Placa = txtPlaca.Text,
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    Monto = 1,
                    ValorVehiculo = Convert.ToDecimal(txtValorVehiculo.Text),
                    Tasa = 1,
                    TotalPagar = CalcularMarchamo()
                };

                Marchamo marchamoIngresado = await marchamoManager.Ingresar(marchamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(marchamoIngresado.Placa))
                {
                    lblResultado.Text = "Marchamo ingresado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Nuevo marchamo incluido", marchamoIngresado.Placa, "svillagra07@gmail.com",
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
            else if (ValidarCampos() && (!string.IsNullOrEmpty(txtCodigoMarchamo.Text)))// modificar
            {

                Marchamo marchamo = new Marchamo()
                {
                    CodMarchamo = Convert.ToInt32(txtCodigoMarchamo.Text),
                    Placa = txtPlaca.Text,
                    CodUsuario = Convert.ToInt32(ddUSU_CODIGO.SelectedItem.Value.ToString()),
                    Monto = 1,
                    ValorVehiculo = Convert.ToDecimal(txtValorVehiculo.Text),
                    Tasa = 1,
                    TotalPagar = CalcularMarchamo()
                };


                Marchamo marchamoActualizado = await marchamoManager.Actualizar(marchamo, Session["Token"].ToString());

                if (!string.IsNullOrEmpty(marchamoActualizado.Placa))
                {
                    lblResultado.Text = "Marchamo actualizado con exito";
                    lblResultado.Visible = true;
                    lblResultado.ForeColor = Color.Green;
                    btnAceptarMant.Visible = false;
                    InicializarControles();

                    Correo correo = new Correo();
                    correo.Enviar("Marchamo actualizado con exito", marchamoActualizado.Placa, "svillagra07@gmail.com",
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
                Marchamo marchamo = await marchamoManager.Eliminar(_codigo, Session["Token"].ToString());
                if (!string.IsNullOrEmpty(marchamo.Placa))
                {
                    ltrModalMensaje.Text = "Marchamo eliminado";
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
                    Vista = "frmMarchamo.aspx",
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

            ltrCodigoMarchamo.Visible = true;
            txtCodigoMarchamo.Visible = true;

            ltrPlaca.Visible = true;
            txtPlaca.Visible = true;

            ltrCodUsuario.Visible = true;
            //ddUSU_CODIGO.Enabled = false;

            //ltrMonto.Visible = true;
            //txtMonto.Visible = true;

            ltrValorVehiculo.Visible = true;
            txtValorVehiculo.Visible = true;


            //ltrTasa.Visible = true;
            //txtTasa.Visible = true;

            txtCodigoMarchamo.Text = string.Empty;
            txtPlaca.Text = string.Empty;
            //txtMonto.Text = string.Empty;
            txtValorVehiculo.Text = string.Empty;
            //txtTasa.Text = string.Empty;
            lblResultado.Visible = false;

            ScriptManager.RegisterStartupScript(this,
                this.GetType(), "LaunchServerSide", "$(function() {openModalMantenimiento(); } );", true);
        }


        protected void gvMarchamos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvMarchamos.Rows[index];
            lblResultado.Visible = false;

            switch (e.CommandName)
            {
                case "Modificar":
                    ltrTituloMantenimiento.Text = "Modificar marchamo";
                    btnAceptarMant.ControlStyle.CssClass = "btn btn-primary";


                    txtCodigoMarchamo.Text = row.Cells[0].Text.Trim();
                    txtPlaca.Text = row.Cells[1].Text.Trim();
                    //txtCodUsuario.Text = row.Cells[2].Text.Trim();
                    //txtMonto.Text = row.Cells[3].Text.Trim();
                    txtValorVehiculo.Text = row.Cells[3].Text.Trim();
                    //txtTasa.Text = row.Cells[5].Text.Trim();


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