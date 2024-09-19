using apiTenis.Validations;
using System.ComponentModel.DataAnnotations;

namespace apiTenis.Test.Validations
{
    public class PrimeraLetraMayusculaAttributeTest
    {
        [Fact]
        public void PrimeraLetraMayuscula_DevuelveError()
        {
            // preparacion
            var attribute = new PrimeraLetraMayusculaAttributes();
            var valor = "hola";
            var valContext = new ValidationContext(new { Name = valor });
            // ejecucion
            var result = attribute.GetValidationResult(valor, valContext);
            // verificacion
            Assert.Equal("La primera letra debe ser mayúscula", result.ErrorMessage);
        }

        [Fact]
        public void ValorNulo_NoDevuelveError()
        {
            // preparacion
            var attribute = new PrimeraLetraMayusculaAttributes();
            string valor = null;
            var valContext = new ValidationContext(new { Name = valor });
            // ejecucion
            var result = attribute.GetValidationResult(valor, valContext);
            // verificacion
            Assert.Null(result);
        }

        [Fact]
        public void ValorConPrimerLetraMayuscula_NoDevuelveError()
        {
            // preparacion
            var attribute = new PrimeraLetraMayusculaAttributes();
            string valor = "Matias";
            var valContext = new ValidationContext(new { Name = valor });
            // ejecucion
            var result = attribute.GetValidationResult(valor, valContext);
            // verificacion
            Assert.Null(result);
        }
    }
}
