using AirCinelMVC.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AirCinelMVC.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Manufacturer> Manufacturers { get; set; }

        public DbSet<Model> Models { get; set; }

        public DbSet<Airplane> Airplanes { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.Name)
            .IsUnique();

            modelBuilder.Entity<City>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<Airport>()
                .HasIndex(a => a.Name)
                .IsUnique();

            modelBuilder.Entity<Manufacturer>()
                .HasIndex(m => m.Name)
                .IsUnique();

            modelBuilder.Entity<Model>()
                .HasIndex(m => m.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }


    }
}
