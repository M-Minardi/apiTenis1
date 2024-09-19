using apiTenis.Entities;

namespace apiTenis.Validations
{
    public static class JugadorValidacion
    {
        public static bool IsValid(Jugador jugador, ref string mensajeError)
        {
            mensajeError = string.Empty;

            if (jugador.Genero == "M") // Solo para género masculino
            {
                if (!jugador.Fuerza.HasValue || jugador.Fuerza <= 0)
                {
                    mensajeError = "La fuerza es obligatoria, debe estar entre 1 y 100 para los jugadores masculinos.";
                    return false;
                }

                if (!jugador.Velocidad.HasValue || jugador.Velocidad <= 0)
                {
                    mensajeError = "La velocidad es obligatoria, debe estar entre 1 y 100 para los jugadores masculinos.";
                    return false;
                }
            }else if(jugador.Genero == "F")
            {
                if (!jugador.TiempoReaccion.HasValue || jugador.TiempoReaccion <= 0)
                {
                    mensajeError = "El tiempo de reacción es obligatorio, debe estar entre 1 y 100 para las jugadoras femeninas.";
                    return false;
                }
            }
            return true;
        }
    }
}
