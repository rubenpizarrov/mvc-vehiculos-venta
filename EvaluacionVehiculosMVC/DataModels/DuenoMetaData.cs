using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EvaluacionVehiculosMVC.DataModels
{
    [MetadataType(typeof(DuenoMetaData))]
    public partial class Dueno
    {
     
    }

    public class DuenoMetaData
    {
        [Required]
        [ValidacionRut]
        [StringLength(9, MinimumLength=8, ErrorMessage="El RUT ingresado debe tener un formato 11222333K")]
        [RegularExpression(@"^([1-9]{1,2})([0-9]{3})([0-9]{3})([Kk0-9]{1})$", ErrorMessage="El RUT ingresado debe tener un formato 11222333K")]
        public string RUT { get; set; }
        [Required]
        [RegularExpression("^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$", ErrorMessage = "No se admiten números ni símbolos" )]
        [StringLength(50, ErrorMessage = "Ha escrito un Nombre demasiado Largo")]
        public string Nombre { get; set; }
        [Required]
        [RegularExpression("^[a-zA-ZñÑáéíóúÁÉÍÓÚ ]*$", ErrorMessage = "No se admiten números ni símbolos")]
        [StringLength(50, ErrorMessage = "Ha escrito un Apellido demasiado Largo")]
        public string Apellido { get; set; }
        [Required]
        [DisplayName("Teléfono")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "No se admiten letras")]
        [StringLength(15)]
        public string Telefono { get; set; }
        [Required]
        [DisplayName("Fecha de Registro")]
        [DisplayFormat(DataFormatString="{0:dd-MM-yyyy}", ApplyFormatInEditMode=true)]
        public System.DateTime FechaRegistro { get; set; }
    }


}