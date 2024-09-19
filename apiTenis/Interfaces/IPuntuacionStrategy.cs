
using apiTenis.Entities;
using apiTenis.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiTenis.Interfaces
{
    public interface IPuntuacionStrategy
    {
        int CalcularPuntuacion(Jugador jugador);
    }
}
