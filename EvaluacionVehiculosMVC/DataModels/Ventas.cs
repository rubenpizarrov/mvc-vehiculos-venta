//------------------------------------------------------------------------------
// <auto-generated>
//    Este código se generó a partir de una plantilla.
//
//    Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//    Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EvaluacionVehiculosMVC.DataModels
{
    using System;
    using System.Collections.Generic;
    
    public partial class Ventas
    {
        public int IdVenta { get; set; }
        public System.DateTime FechaVenta { get; set; }
        public string Patente { get; set; }
        public decimal TotalVenta { get; set; }
        public decimal IVA { get; set; }
        public decimal TotalBruto { get; set; }
        public string RUTDueno { get; set; }
    }
}
