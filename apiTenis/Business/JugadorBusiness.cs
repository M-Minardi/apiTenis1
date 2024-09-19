
using apiTenis.Entities;
using apiTenis.Helpers;
using apiTenis.Interfaces;
using apiTenis.Models;
using apiTenis.Validations;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Linq.Dynamic.Core;

namespace apiTenis.Business
{
    public class JugadorBusiness : IJugadorBusiness
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public JugadorBusiness(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<JugadorDTO>> Get()
        {
            var entidades = await _context.Jugador.ToListAsync();
            return _mapper.Map<List<JugadorDTO>>(entidades);
        }
        public async Task<List<JugadorDTO>> GetPaginacion(PaginacionDTO paginacionDTO)
        {
            var queryable = _context.Jugador.AsQueryable();
            var entidades = await queryable.Paginar(paginacionDTO).ToListAsync();
            return _mapper.Map<List<JugadorDTO>>(entidades);
        }
        public async Task<JugadorDTO?> Get(int id)
        {
            var entidad = await _context.Jugador.FirstOrDefaultAsync(x => x.Id == id);
            if (entidad == null) return null;
            return _mapper.Map<JugadorDTO>(entidad);
        }
        public async Task<List<JugadorDTO>?> GetFiltro(JugadorFiltroDTO dto)
        {
            var queryable = _context.Jugador.AsQueryable();
            if (!string.IsNullOrEmpty(dto.Nombre))
                queryable = queryable.Where(x => x.Nombre.Contains(dto.Nombre));
            if (!string.IsNullOrEmpty(dto.Apellido))
                queryable = queryable.Where(x => x.Apellido.Contains(dto.Apellido));            
            if (dto.NivelMinimo > 0)
                queryable = queryable.Where(x => x.Nivel > dto.NivelMinimo);

            //System.Linq.Dynamic.Core
            if (!string.IsNullOrEmpty(dto.OrdenarCampo))
            {
                var tipoOrden = (bool)dto.OrdenarAscendente ? "ascending" : "descending";
                try
                {
                    queryable = queryable.OrderBy($"{dto.OrdenarCampo} {tipoOrden}");
                }
                catch
                {
                    return null;
                }
            }

            var entidades = await queryable.Paginar(dto.Paginacion).ToListAsync();

            return _mapper.Map<List<JugadorDTO>>(entidades);
        }
        public async Task<JugadorDTO> Create(JugadorDTO dto)
        {
            var entidad = _mapper.Map<Jugador>(dto);
            
            string mensajeError = string.Empty;
            if (!JugadorValidacion.IsValid(entidad, ref mensajeError))
                throw new ValidationException(mensajeError);
            
            _context.Jugador.Add(entidad);
            await _context.SaveChangesAsync();
            return _mapper.Map<JugadorDTO>(entidad);
        }
        public async Task<bool> Update(int id, JugadorDTO dto)
        {
            if (id != dto.Id) return false;

            var entidad = await _context.Jugador.FindAsync(id);
            if (entidad == null) return false;

            _mapper.Map(dto, entidad);
            
            string mensajeError = string.Empty;
            if (!JugadorValidacion.IsValid(entidad, ref mensajeError))
                throw new ValidationException(mensajeError);
            

            _context.Entry(entidad).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Jugador.AnyAsync(x => x.Id == id)) return false;
                throw;
            }
        }
        public async Task<bool> Delete(int id)
        {
            var entidad = await _context.Jugador.FindAsync(id);
            if (entidad == null) return false;

            _context.Jugador.Remove(entidad);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
