
using apiTenis.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace apiTenis
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Postgre acepte la fecha
            modelBuilder.Entity<Torneo>()
                .Property(a => a.Fecha)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v,DateTimeKind.Utc));

            base.OnModelCreating(modelBuilder);            
        }

        public DbSet<Jugador> Jugador { get; set; }
        public DbSet<Torneo> Torneo { get; set; }               
    }
}
