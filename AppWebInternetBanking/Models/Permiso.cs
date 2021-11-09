
namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;

    public class Permiso
    {
        public int CodPermiso { get; set; }
        public int CodUsuario { get; set; }
        public string TipoPermiso { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateTime FechaVencimiento { get; set; }
    }
}