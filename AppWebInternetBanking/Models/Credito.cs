using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWebInternetBanking.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Credito
    {
        public int CodCredito { get; set; }
        public int CodUsuario { get; set; }
        public decimal MontoCredito { get; set; }
        public string Descripcion { get; set; }
        public System.DateTime FechaSolicitud { get; set; }

        public virtual Usuario Usuario { get; set; }
    }
}
