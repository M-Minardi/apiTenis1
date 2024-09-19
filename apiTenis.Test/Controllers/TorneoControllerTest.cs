using Moq;
using apiTenis.Controllers;
using Microsoft.AspNetCore.Mvc;
using apiTenis.Models;
using apiTenis.Interfaces;
using apiTenis.Entities;
using apiTenis.Test;

namespace apiTenis.Tests.Controllers
{
    public class TorneoControllerTest : BasePruebas
    {
        private readonly Mock<ITorneoBusiness> _mockTorneoBusiness;
        private readonly Mock<IJugadorBusiness> _mockJugadorBusiness;
        private readonly TorneoController _controller;

        public TorneoControllerTest()
        {
            _mockTorneoBusiness = new Mock<ITorneoBusiness>();
            _mockJugadorBusiness = new Mock<IJugadorBusiness>();
            _controller = new TorneoController(_mockTorneoBusiness.Object);
        }

        [Fact]
        public async Task Get_ReturnListTorneos()
        {
            // Arrange
            var torneos = new List<TorneoDTO>
            {
                new TorneoDTO { Id = 1, Nombre = "Torneo1", Fecha = new DateTime(2024,9,19), Genero = "M" },
                new TorneoDTO { Id = 2, Nombre = "Torneo2", Fecha = new DateTime(2024,9,19), Genero = "F" }
            };
            _mockTorneoBusiness.Setup(b => b.Get()).ReturnsAsync(torneos);

            // Act
            var actionResult = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<TorneoDTO>>(okResult.Value);

            Assert.Equal(2, returnValue.Count());
            Assert.Equal(torneos, returnValue);
        }
        [Fact]
        public async Task GetFiltroGenero_ReturnListTorneos()
        {
            // Arrange
            var filtroDTO = new TorneoFiltroDTO { Genero = "F" };
            var torneos = new List<Torneo>
            {
                new Torneo { Id = 1, Nombre = "Torneo1", Fecha = new DateTime(2024,9,18), Genero = "M", JugadorGanadorId = 1 },
                new Torneo { Id = 2, Nombre = "Torneo2", Fecha = new DateTime(2024,9,21), Genero = "F", JugadorGanadorId = 1 }
            };

            var nombreDB = Guid.NewGuid().ToString();
            var _context = ConstruirContext(nombreDB);
            await _context.Torneo.AddRangeAsync(torneos);
            await _context.SaveChangesAsync();

            _mockTorneoBusiness.Setup(b => b.GetFiltro(It.IsAny<TorneoFiltroDTO>()))
                               .ReturnsAsync(_context.Torneo
                               .Where(t => t.Genero == filtroDTO.Genero)
                               .Select(t => new TorneoDTO
                               {
                                   Id = t.Id,
                                   Nombre = t.Nombre,
                                   Fecha = t.Fecha,
                                   Genero = t.Genero
                               })
                                .ToList());

            // Act
            var result = await _controller.GetFiltro(filtroDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<TorneoDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<TorneoDTO>>(okResult.Value);

            Assert.Single(returnValue);
            Assert.Equal("Torneo2", returnValue?.FirstOrDefault()?.Nombre);
        }

        [Fact]
        public async Task GetFiltroFechaDesde_ReturnListTorneos()
        {
            // Arrange
            var filtroDTO = new TorneoFiltroDTO { FechaDesde = new DateTime(2024, 9, 19) };
            var torneos = new List<Torneo>
            {
                new Torneo { Id = 1, Nombre = "Torneo1", Fecha = new DateTime(2024,9,18), Genero = "M", JugadorGanadorId = 1 },
                new Torneo { Id = 2, Nombre = "Torneo2", Fecha = new DateTime(2024,9,21), Genero = "F", JugadorGanadorId = 1 }
            };

            var nombreDB = Guid.NewGuid().ToString();
            var _context = ConstruirContext(nombreDB);
            await _context.Torneo.AddRangeAsync(torneos);
            await _context.SaveChangesAsync();

            _mockTorneoBusiness.Setup(b => b.GetFiltro(It.IsAny<TorneoFiltroDTO>()))
                               .ReturnsAsync(_context.Torneo
                               .Where(t => t.Fecha >= filtroDTO.FechaDesde)
                               .Select(t => new TorneoDTO
                                {
                                    Id = t.Id,
                                    Nombre = t.Nombre,
                                    Fecha = t.Fecha,
                                    Genero = t.Genero
                                })
                                .ToList());

            // Act
            var result = await _controller.GetFiltro(filtroDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<TorneoDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<TorneoDTO>>(okResult.Value);

            Assert.Single(returnValue);
            Assert.Equal("Torneo2", returnValue?.FirstOrDefault()?.Nombre);
        }

        [Fact]
        public async Task CreateTorneoM_ReturnsOk()
        {
            // Arrange
            var jugadores = new List<Jugador>
            {
                    new Jugador {
                        Id = 1, Nombre = "Jugador1", Apellido="Player1", Genero = "M",
                        Nivel=70, Fuerza=70, Velocidad=70 },
                    new Jugador {
                        Id = 2, Nombre = "Jugador2", Apellido="Player2", Genero = "M",
                        Nivel=7, Fuerza=7, Velocidad=7 },
                    new Jugador {
                        Id = 3, Nombre = "Jugador3", Apellido="Player3", Genero = "M",
                        Nivel=7, Fuerza=7, Velocidad=7 },
                    new Jugador {
                        Id = 4, Nombre = "Jugador4", Apellido="Player4", Genero = "M",
                        Nivel=7, Fuerza=7, Velocidad=7 },
            };
            var jugadoresDTO = jugadores.Select(j => new JugadorDTO
            {
                Id = j.Id,
                Nombre = j.Nombre,
                Apellido = j.Apellido,
                Genero = j.Genero,
                Nivel = j.Nivel,
                Fuerza = j.Fuerza,
                Velocidad = j.Velocidad
            }).ToList();
            foreach (var jugadorDto in jugadoresDTO)
            {
                _mockJugadorBusiness.Setup(j => j.Get(jugadorDto.Id)).ReturnsAsync(jugadorDto);
            }

            var dto = new TorneoCrearDTO
            {
                Nombre = "Torneo1",
                Genero = "M",
                Fecha = DateTime.Now,
                JugadoresIds = jugadores.Select(j => (long)j.Id).ToList()
            };

            var jugadorGanador = jugadores.First(); // Simular que gana el primero

            _mockTorneoBusiness.Setup(b => b.CrearTorneoConSimulacion(dto))
                               .ReturnsAsync($"El jugador ganador es: {jugadorGanador.Apellido} {jugadorGanador.Nombre}");

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<string>(okResult.Value);
            Assert.Equal($"El jugador ganador es: {jugadorGanador.Apellido} {jugadorGanador.Nombre}", returnValue);

        }


    }
}
