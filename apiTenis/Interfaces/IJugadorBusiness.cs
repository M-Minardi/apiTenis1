
using apiTenis.Models;
using Microsoft.AspNetCore.Mvc;

namespace apiTenis.Interfaces
{
    public interface IJugadorBusiness
    {
         Task<List<JugadorDTO>> Get();
         Task<List<JugadorDTO>> GetPaginacion(PaginacionDTO paginacionDTO);
         Task<JugadorDTO?> Get(int id);        
        Task<List<JugadorDTO>?> GetFiltro(JugadorFiltroDTO dto);
        Task<JugadorDTO> Create(JugadorDTO dto);
         Task<bool> Update(int id, JugadorDTO dto);
        Task<bool> Delete(int id);
    }
}
