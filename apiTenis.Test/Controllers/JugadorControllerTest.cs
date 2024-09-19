using Moq;
using apiTenis.Controllers;
using Microsoft.AspNetCore.Mvc;
using apiTenis.Models;
using apiTenis.Interfaces;
using apiTenis.Entities;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Tests.Controllers
{
    public class JugadorControllerTest
    {
        private readonly Mock<IJugadorBusiness> _mockJugadorBusiness;
        private readonly JugadorController _controller;

        public JugadorControllerTest()
        {
            _mockJugadorBusiness = new Mock<IJugadorBusiness>();            
            _controller = new JugadorController(_mockJugadorBusiness.Object);
        }

        [Fact]
        public async Task Get_ReturnListJugadores()
        {
            // Arrange
            var jugadores = new List<JugadorDTO>
            {
                new JugadorDTO { Id = 1, Nombre = "Jugador1", Apellido = "1", Genero = "M", Nivel = 70 },
                new JugadorDTO { Id = 2, Nombre = "Jugador2", Apellido = "2", Genero = "M", Nivel = 75 }
            };
            _mockJugadorBusiness.Setup(b => b.Get()).ReturnsAsync(jugadores);

            // Act
            var actionResult = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<JugadorDTO>>(okResult.Value);

            Assert.Equal(2, returnValue.Count());
            Assert.Equal(jugadores, returnValue);
        }        
        [Fact]
        public async Task Get_ReturnNotFound()
        {
            // Arrange
            int invalidId = 999;
            _mockJugadorBusiness.Setup(b => b.Get(invalidId)).ReturnsAsync((JugadorDTO)null);

            // Act
            var result = await _controller.Get(invalidId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetFiltro_ReturnListJugadores()
        {
            // Arrange
            var filtroDTO = new JugadorFiltroDTO { Nombre = "Jugador1" };
            var jugadores = new List<JugadorDTO>
            {
                new JugadorDTO { Id = 1, Nombre = "Jugador1", Apellido = "1", Genero = "M", Nivel = 70 },
                new JugadorDTO { Id = 2, Nombre = "Jugador2", Apellido = "2", Genero = "M", Nivel = 75 }
            };

            _mockJugadorBusiness.Setup(b => b.GetFiltro(filtroDTO)).ReturnsAsync(jugadores);

            // Act
            var result = await _controller.GetFiltro(filtroDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<JugadorDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<JugadorDTO>>(okResult.Value);
            Assert.Equal("Jugador1", returnValue?.FirstOrDefault()?.Nombre);
        }
        [Fact]
        public async Task GetPaginar_ReturnListJugadores()
        {
            // Arrange
            var paginacionDTO = new PaginacionDTO { Pagina = 1, CantidadRegistrosPorPagina = 10 };
            var jugadores = new List<JugadorDTO>
            {
                new JugadorDTO { Id = 1, Nombre = "Jugador1", Apellido = "1", Genero = "M", Nivel = 70 },
                new JugadorDTO { Id = 2, Nombre = "Jugador2", Apellido = "2", Genero = "M", Nivel = 75 }
            };
            _mockJugadorBusiness.Setup(b => b.GetPaginacion(paginacionDTO)).ReturnsAsync(jugadores);

            // Act
            var result = await _controller.GetPaginar(paginacionDTO);

            // Assert
            var actionResult = Assert.IsType<ActionResult<List<JugadorDTO>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<JugadorDTO>>(okResult.Value);
            Assert.Equal(2, returnValue.Count());
            Assert.Equal(jugadores, returnValue);
        }
        [Fact]
        public async Task Create()
        {
            // Arrange
            var dto = new JugadorDTO { Id = 1, Nombre = "Nuevo Jugador" };
            _mockJugadorBusiness.Setup(b => b.Create(dto)).ReturnsAsync(dto);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<JugadorDTO>(createdAtRouteResult.Value);
            Assert.Equal("Nuevo Jugador", returnValue.Nombre);
        }

        [Fact]
        public async Task Update()
        {
            // Arrange
            int id = 1;
            var dto = new JugadorDTO { Id = 2 }; 

            // Act
            var result = await _controller.Update(id, dto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            int id = 1;
            _mockJugadorBusiness.Setup(b => b.Delete(id)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnNotFound()
        {
            // Arrange
            int id = 1;
            _mockJugadorBusiness.Setup(b => b.Delete(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }        
    }
}
