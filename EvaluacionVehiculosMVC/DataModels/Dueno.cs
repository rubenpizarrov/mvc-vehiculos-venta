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
    
    public partial class Dueno
    {
        public Dueno()
        {
            this.Vehiculo = new HashSet<Vehiculo>();
        }
    
        public int IdDueno { get; set; }
        public string RUT { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public System.DateTime FechaRegistro { get; set; }
    
        public virtual ICollection<Vehiculo> Vehiculo { get; set; }
    }
}
