using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public partial class Tarjeta
    {
        public int CodTarjeta { get; set; }
        public int CodUsuario { get; set; }
        public string Tipo { get; set; }
        public string Emisor { get; set; }
        public System.DateTime FechaEmision { get; set; }
        public System.DateTime FechaVencimiento { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
