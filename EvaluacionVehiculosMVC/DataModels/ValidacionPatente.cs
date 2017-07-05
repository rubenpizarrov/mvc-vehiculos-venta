using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EvaluacionVehiculosMVC.DataModels
{
    sealed public class ValidacionPatente : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var textpatente = value.ToString();
                string nuevaPatente = string.Empty;
                
                Regex expresion = new Regex(@"^([a-zA-Z]{2})-?([a-zA-Z]{2})-?(\d{2})$");
                
                if (textpatente.Length > 8 || textpatente.Length < 6 || textpatente.Length == 7)
                {
                    return new ValidationResult("El largo de la patente debe ser máximo de 8 caracteres");
                }
                else if (!expresion.IsMatch(textpatente))
                {
                    return new ValidationResult("La Patente no está en formato correcto");
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            return ValidationResult.Success;

        }
    }
}