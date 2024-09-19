using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Validations
{
    public class CantidadParAttributes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var lista = value as ICollection;
            if (lista == null || lista.Count % 2 != 0)
            {
                return new ValidationResult("La cantidad de registros en la lista debe ser un número par.");
            }

#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
            return ValidationResult.Success;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
    }
}
