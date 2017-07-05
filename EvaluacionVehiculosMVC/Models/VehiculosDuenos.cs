using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvaluacionVehiculosMVC.Models

{
    public class VehiculosDuenos : DataModels.VehiculoMetaData
    {
       
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        [Display(Name = "Nombre Dueño")]
        [Required]
        public string NombreCompleto { get { return Nombre + " " + Apellido; } }
        

    }
}