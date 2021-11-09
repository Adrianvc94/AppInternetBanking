using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public class Licencia
    {
        public int CodLicencia { get; set; }
        public int CodUsuario { get; set; }
        public string TipoLicencia { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}