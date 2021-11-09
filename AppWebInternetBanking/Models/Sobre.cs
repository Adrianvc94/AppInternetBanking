using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    public class Sobre
    {
        public int CodSobre { get; set; }
        public int CodCuenta { get; set; }
        public decimal Saldo { get; set; }
        public string Descripcion { get; set; }
        public int CodMoneda { get; set; }
    }
}