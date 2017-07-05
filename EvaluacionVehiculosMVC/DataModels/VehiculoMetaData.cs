using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvaluacionVehiculosMVC.DataModels
{
    [MetadataType(typeof(VehiculoMetaData))]
    public partial class Vehiculo 
    {

    }

    
    public class VehiculoMetaData
    {
        public int IdVehiculo { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Debe ingresar {0} Ingresar un máximo de {1} caracteres y un mínimo de {2}")]
        [RegularExpression(@"^([a-zA-Z]{2})-?([a-zA-Z]{2})-?(\d{2})$", ErrorMessage = "Ha ingresado una patente erronea")]
        [ValidacionPatente]
        public string Patente { get; set; }
        public int IdDueno { get; set; }
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$", ErrorMessage = "No se admiten números ni caracteres raros")]
        public string Marca { get; set; }
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9_\-\.\s]+$")]
        public string Modelo { get; set; }
        [Display(Name = "Año")]
        //[StringLength(4)]
        [Range(1900,2017, ErrorMessage = "La fecha debe tener 4 números entre los años 1900 y el 2017")]
        [RegularExpression(@"^[0-9]+$")]
        public Nullable<int> Anno { get; set; }
        [Required]
        [Range(0, 99.99)]
        [RegularExpression(@"\d+(\,\d{0,2})?$")]
        public decimal PrecioEnUF { get; set; }
    }
}