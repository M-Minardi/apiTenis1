
using apiTenis.Entities;
using apiTenis.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiTenis.Interfaces
{
    public interface ITorneoSimulador
    {
        public Jugador SimularTorneo(List<Jugador> jugadores);         
    }
}
