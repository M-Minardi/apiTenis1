using System.ComponentModel.DataAnnotations;

namespace apiTenis.Models
{
    public class TorneoFiltroDTO
    {
        public int Pagina { get; set; } = 1;

        public int CantidadRegistrosPorPagina = 10;
        public PaginacionDTO Paginacion
        {
            get { return new PaginacionDTO() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistrosPorPagina }; }
        }
        public string? Nombre { get; set; }
        public string? Genero { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public long? JugadorGanadorId { get; set; }   
        public string? OrdenarCampo { get; set; }
        public bool OrdenarAscendente { get; set; } = false;

    }
}
