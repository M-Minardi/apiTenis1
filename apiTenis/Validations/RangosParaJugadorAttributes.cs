using apiTenis.Entities;
using apiTenis.Models;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Validations
{
    public class RangosParaJugadorAttributes : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var jugador = (JugadorDTO)validationContext.ObjectInstance;

            if (jugador.Genero == "M") // Solo para género masculino
            {
                if (!jugador.Fuerza.HasValue || jugador.Fuerza <= 0)
                {
                    return new ValidationResult("La fuerza es obligatoria, debe estar entre 1 y 100 para los jugadores masculinos.");
                }

                if (!jugador.Velocidad.HasValue || jugador.Velocidad <= 0)
                {
                    return new ValidationResult("La velocidad es obligatoria, debe estar entre 1 y 100 para los jugadores masculinos.");
                }
            }
            else if (jugador.Genero == "F") // Solo para género femenino
            {
                if (!jugador.TiempoReaccion.HasValue || jugador.TiempoReaccion <= 0)
                {
                    return new ValidationResult("El tiempo de reacción es obligatorio, debe estar entre 1 y 100 para las jugadoras femeninas.");
                }
            }

#pragma warning disable CS8603 // Posible tipo de valor devuelto de referencia nulo
                return ValidationResult.Success;
#pragma warning restore CS8603 // Posible tipo de valor devuelto de referencia nulo
        }
    }
}
