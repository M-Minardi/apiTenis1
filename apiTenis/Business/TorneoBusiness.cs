
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
    public class TorneoBusiness : ITorneoBusiness
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ITorneoSimulador _torneoSimulador;
        public TorneoBusiness(ApplicationDbContext context, IMapper mapper, ITorneoSimulador torneoSimulador)
        {
            _context = context;
            _mapper = mapper;
            _torneoSimulador = torneoSimulador;
        }

        public async Task<List<TorneoDTO>> Get()
        {
            var entidades = await _context.Torneo.Include(x => x.JugadorGanador).ToListAsync();
            return _mapper.Map<List<TorneoDTO>>(entidades);
        }
        public async Task<List<TorneoDTO>?> GetFiltro(TorneoFiltroDTO dto)
        {
            var queryable = _context.Torneo.Include(x => x.JugadorGanador).AsQueryable();
            // Filtro por nombre
            if (!string.IsNullOrEmpty(dto.Nombre))
                queryable = queryable.Where(x => x.Nombre.Contains(dto.Nombre));
            // Filtro por Genero
            if (!string.IsNullOrEmpty(dto.Genero))
                queryable = queryable.Where(x => x.Genero == dto.Genero);
            // Filtro por fechas (desde y hasta)
            if (dto.FechaDesde.HasValue)            
                queryable = queryable.Where(x => x.Fecha >= dto.FechaDesde.Value);            
            if (dto.FechaHasta.HasValue)            
                queryable = queryable.Where(x => x.Fecha <= dto.FechaHasta.Value);            
            // Filtro por JugadorGanadorId
            if (dto.JugadorGanadorId.HasValue)            
                queryable = queryable.Where(x => x.JugadorGanadorId == dto.JugadorGanadorId.Value);
            //Ordenar //System.Linq.Dynamic.Core
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
            // Aplicar paginación
            var entidades = await queryable.Paginar(dto.Paginacion).ToListAsync();

            return _mapper.Map<List<TorneoDTO>>(entidades);
        }
        public async Task<String> CrearTorneoConSimulacion(TorneoCrearDTO dto)
        {
            var torneo = _mapper.Map<Torneo>(dto);

            //validar cantidad jugadores
            if (dto.JugadoresIds == null || !dto.JugadoresIds.Any())            
                throw new ValidationException("Debe haber al menos un jugador para simular el torneo.");

            //Buscar jugadores en la base de datos
            var jugadores = await _context.Jugador.Where(j => dto.JugadoresIds.Contains(j.Id)).ToListAsync();
            //Validar que existan todos los jugadores
            if (jugadores.Count != dto.JugadoresIds.Count)
                throw new ValidationException("Algunos jugadores no existen en la base de datos.");

            // Validar genero del torneo con jugadores
            var generosJugadores = jugadores.Select(j => j.Genero).Distinct().ToList();
            if (generosJugadores.Count > 1 || generosJugadores.First() != torneo.Genero)
            {
                throw new ValidationException("El género del torneo debe coincidir con todos los géneros de los jugadores.");
            }

            var jugadorGanador = _torneoSimulador.SimularTorneo(jugadores);
            
            torneo.JugadorGanadorId = jugadorGanador.Id;

            _context.Torneo.Add(torneo);
            await _context.SaveChangesAsync();

            return $"El jugador ganador es: {jugadorGanador.Apellido} {jugadorGanador.Nombre}";            
        }
        

    }
}
