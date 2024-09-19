
using apiTenis.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiTenis.Interfaces
{
    public interface ITorneoBusiness
    {
        Task<List<TorneoDTO>> Get();       
        Task<List<TorneoDTO>?> GetFiltro(TorneoFiltroDTO dto);
        Task<String> CrearTorneoConSimulacion(TorneoCrearDTO dto);
    }
}
