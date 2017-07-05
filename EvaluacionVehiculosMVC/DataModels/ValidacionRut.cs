using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EvaluacionVehiculosMVC.DataModels
{
    public class ValidacionRut : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string rut = value.ToString();
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");
                string patron = "^([1-9]{1,2})([0-9]{3})([0-9]{3})([Kk0-9]{1})$";
                Regex expresion = new Regex(patron);
                if (rut.Length > 9)
                {
                    char[] charsrut= rut.ToCharArray(0,9);
                    rut = new string(charsrut);
                }
                
                if (!expresion.IsMatch(rut))
                {
                    return new ValidationResult("El Rut Ingresado no tiene el formato correcto");
                }
                else
                {
                    return ValidationResult.Success;
                }

            }
            return null;

        }
    }
}