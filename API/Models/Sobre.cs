//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace API.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Sobre
    {
        public int CodSobre { get; set; }
        public int CodCuenta { get; set; }
        public decimal Saldo { get; set; }
        public string Descripcion { get; set; }
        public int CodMoneda { get; set; }
    
        public virtual Cuenta Cuenta { get; set; }
    }
}
