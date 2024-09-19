using System.ComponentModel.DataAnnotations;

namespace apiTenis.Models
{
    public class JugadorFiltroDTO
    {
        public int Pagina { get; set; } = 1;

        public int CantidadRegistrosPorPagina = 10;
        public PaginacionDTO Paginacion
        {
            get { return new PaginacionDTO() { Pagina = Pagina, CantidadRegistrosPorPagina = CantidadRegistrosPorPagina }; }
        }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public int? NivelMinimo { get; set; }   
        public string? OrdenarCampo { get; set; }
        public bool OrdenarAscendente { get; set; } = false;

    }
}
