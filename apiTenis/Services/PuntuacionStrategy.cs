using apiTenis.Entities;
using apiTenis.Interfaces;

namespace apiTenis.Services
{
    public class PuntuacionMasculinoStrategy : IPuntuacionStrategy
    {
        public int CalcularPuntuacion(Jugador jugador)
        {
            int puntuacion = jugador.Nivel;

            // Agregar Suerte
            int suerte = new Random().Next(0, 50);
            puntuacion += suerte;

            return puntuacion + (jugador.Fuerza ?? 0) + (jugador.Velocidad ?? 0);
        }
    }
    public class PuntuacionFemeninoStrategy : IPuntuacionStrategy
    {
        public int CalcularPuntuacion(Jugador jugador)
        {
            int puntuacion = jugador.Nivel;

            // Agregar Suerte
            int suerte = new Random().Next(0, 50);
            puntuacion += suerte;

            return puntuacion + (jugador.TiempoReaccion ?? 0);
        }
    }

    public class PuntuacionStrategyFactory
    {
        public static IPuntuacionStrategy Obtener(string genero)
        {
            return genero switch
            {
                "M" => new PuntuacionMasculinoStrategy(),
                "F" => new PuntuacionFemeninoStrategy(),
                _ => throw new ArgumentException("Género no válido")
            };
        }
    }

    public class SimuladorPuntuacion
    {
        private readonly IPuntuacionStrategy _estrategia;

        public SimuladorPuntuacion(IPuntuacionStrategy estrategia)
        {
            _estrategia = estrategia;
        }

        public int SimularPuntuacion(Jugador jugador)
        {
            return _estrategia.CalcularPuntuacion(jugador);
        }
    }
}
