using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EvaluacionVehiculosMVC.Models
{
    public class User
    {
        public string UserAlumno { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string IsValudOrMessage { get; set; }
        [Required(ErrorMessage = "Campo Usuario Requerido")]
        public string Usuario { get; set; }
        [Required(ErrorMessage="Campo Password Requerido")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}