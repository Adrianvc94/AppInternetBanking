using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public class Prestamo
    {
        public int CodPrestamo { get; set; }
        public int CodUsuario { get; set; }
        public decimal MontoPrestamo { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime FechaLimite { get; set; }
        public decimal TasaInteres { get; set; }
    }
}