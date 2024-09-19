using apiTenis.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace apiTenis.Test
{
    public class BasePruebas
    {
        protected ApplicationDbContext ConstruirContext(string nombreDB)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(nombreDB)
                .Options;
            var dbContext = new ApplicationDbContext(options);
            return dbContext;
        }

        protected IMapper ConfigurarAutoMapper()
        {
            var config = new MapperConfiguration(opc =>
            {
                opc.AddProfile(new AutoMapperProfiles());
            });
            return config.CreateMapper();
        }
    }
}
