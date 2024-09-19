namespace apiTenis.Models
{
    public class PaginacionDTO
    {
        public int Pagina { get; set; } = 1;
        private int cantidadRegistrosPorPagina = 10;
        private readonly int cantidadMaximaRecordsPorPagina = 50;
        public int CantidadRegistrosPorPagina{
            get => cantidadRegistrosPorPagina;
            set
            {
                cantidadRegistrosPorPagina = (value > cantidadMaximaRecordsPorPagina) ? cantidadMaximaRecordsPorPagina : value;
            }
        }
    }
}
