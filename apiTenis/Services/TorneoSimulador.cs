using apiTenis.Entities;
using apiTenis.Interfaces;
using apiTenis.Models;
using AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Services
{
    public class TorneoSimulador : ITorneoSimulador
    {
        private readonly IMapper _mapper;
        public TorneoSimulador(IMapper mapper)
        {
            _mapper = mapper;
        }
        public Jugador SimularTorneo(List<Jugador> jugadores)
        {           
            if (jugadores.Count < 2 || jugadores.Count % 2 != 0)
            {
                throw new ValidationException("La cantidad de jugadores debe ser un número par y mayor o igual a 2.");
            }

            while (jugadores.Count > 1)
            {
                var rondaGanadores = new List<Jugador>();

                for (int i = 0; i < jugadores.Count; i += 2)
                {
                    var jugador1 = jugadores[i];
                    var jugador2 = jugadores[i + 1];
                    var ganador = SimularRonda(jugador1, jugador2);
                    rondaGanadores.Add(ganador);
                }
                jugadores = rondaGanadores;
            }

            return jugadores.First();
        }

        private Jugador SimularRonda(Jugador jugador1, Jugador jugador2)
        {
            int puntuacionJugador1 = SimularPuntuacion(jugador1);
            int puntuacionJugador2 = SimularPuntuacion(jugador2);

            //Ganador en base a puntuación
            if (puntuacionJugador1 > puntuacionJugador2)
                return jugador1;
            else if (puntuacionJugador2 > puntuacionJugador1)
                return jugador2;
            else
            {
                //en caso de empate, ganador aleatorio
                return new Random().Next(0, 2) == 0 ? jugador1 : jugador2;
            }
        }

        //Con Patron de diseño: Strategy (con Factory)
        private int SimularPuntuacion(Jugador jugador)
        {
            var estrategia = PuntuacionStrategyFactory.Obtener(jugador.Genero);
            var simulador = new SimuladorPuntuacion(estrategia);
            return simulador.SimularPuntuacion(jugador);
        }

        /* //Sin Patron de diseño: Strategy
        private int SimularPuntuacion(Jugador jugador)
        {
            int puntuacion = jugador.Nivel;

            // Agregar Suerte
            int suerte = new Random().Next(0, 50);
            puntuacion += suerte;

            // Calcular la puntuación según el género
            if (jugador.Genero == "M")  // Masculino
            {
                return puntuacion + jugador.Fuerza + jugador.Velocidad;
            }
            else if (jugador.Genero == "F")  // Femenino
            {
                return puntuacion + jugador.TiempoReaccion;
            }
            else
                throw new ArgumentException("Género no valido");
        }
        */
    }
}
