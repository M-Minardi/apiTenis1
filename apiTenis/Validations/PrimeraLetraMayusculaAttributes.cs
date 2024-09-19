using System.ComponentModel.DataAnnotations;

namespace apiTenis.Validations
{
    public class PrimeraLetraMayusculaAttributes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var stringValue = value as string;

            // Verificar si el valor es null o vacío
            if (string.IsNullOrEmpty(stringValue))
            {
#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
                return ValidationResult.Success;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
            }

            // Verificar la primera letra
            if (!char.IsUpper(stringValue[0]))
            {
                return new ValidationResult("La primera letra debe ser mayúscula");
            }

#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return ValidationResult.Success;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
    }
}
