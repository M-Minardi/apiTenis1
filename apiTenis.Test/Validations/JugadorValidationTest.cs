using apiTenis.Controllers;
using apiTenis.Entities;
using apiTenis.Interfaces;
using apiTenis.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apiTenis.Test.Validations
{
    public class JugadorValidationTest
    {
        private readonly Mock<IJugadorBusiness> _mockJugadorBusiness;
        private readonly JugadorController _controller;

        public JugadorValidationTest()
        {
            _mockJugadorBusiness = new Mock<IJugadorBusiness>();
            _controller = new JugadorController(_mockJugadorBusiness.Object);
        }
        [Fact]
        public void ValidarJugadorMasculino_OK()
        {
            var jugador = new Jugador
            {
                Nombre = "Jugador1",
                Apellido = "Apellido1",
                Genero = "M",
                Nivel = 50,
                Fuerza = 10,
                Velocidad = 20
            };

            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(jugador);
            bool esValido = Validator.TryValidateObject(jugador, contexto, resultados, true);

            Assert.True(esValido);
        }
        [Fact]
        public void ValidarJugadorFemenino_OK()
        {
            var jugador = new Jugador
            {
                Nombre = "Jugador1",
                Apellido = "Apellido1",
                Genero = "F",
                Nivel = 50,
                //Fuerza = 10,
                //Velocidad = 20,
                TiempoReaccion = 30
            };

            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(jugador);
            bool esValido = Validator.TryValidateObject(jugador, contexto, resultados, true);

            Assert.True(esValido);
        }
        [Fact]
        public void ValidarJugadorFemenino_SinTiempoReaccion()
        {
            var jugador = new JugadorDTO
            {
                Nombre = "Jugador1",
                Apellido = "Apellido1",
                Genero = "F",
                Nivel = 50,
                //Fuerza = 10,
                //Velocidad = 20,
                //TiempoReaccion = 30
            };

            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(jugador);
            bool esValido = Validator.TryValidateObject(jugador, contexto, resultados, true);

            Assert.False(esValido);
            Assert.Contains(resultados, r => r.ErrorMessage == "El tiempo de reacción es obligatorio, debe estar entre 1 y 100 para las jugadoras femeninas.");
        }

        [Fact]
        public void ValidarJugadorMasculino_SinFuerza()
        {
            var jugador = new JugadorDTO
            {
                Nombre = "Jugador1",
                Apellido = "Apellido1",
                Genero = "M",
                Nivel = 50,
                //Fuerza = 0,
                Velocidad = 20
            };

            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(jugador);
            bool esValido = Validator.TryValidateObject(jugador, contexto, resultados, true);

            Assert.False(esValido);
            Assert.Contains(resultados, r => r.ErrorMessage == "La fuerza es obligatoria, debe estar entre 1 y 100 para los jugadores masculinos.");
        }

        [Fact]
        public void ValidarJugadorMasculino_SinVelocidad()
        {
            var jugador = new JugadorDTO
            {
                Nombre = "Jugador1",
                Apellido = "Apellido1",
                Genero = "M",
                Nivel = 50,
                Fuerza = 10,
                //Velocidad = 0
            };

            var resultados = new List<ValidationResult>();
            var contexto = new ValidationContext(jugador);
            bool esValido = Validator.TryValidateObject(jugador, contexto, resultados, true);

            Assert.False(esValido);
            Assert.Contains(resultados, r => r.ErrorMessage == "La velocidad es obligatoria, debe estar entre 1 y 100 para los jugadores masculinos.");
        }
    }
}
