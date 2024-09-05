using AirCinelMVC.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AirCinelMVC.Data
{
    public class DataContext : DbContext
    {
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
            base.OnModelCreating(modelBuilder);

            // Configuração da relação Country - City (um-para-muitos)
            modelBuilder.Entity<City>()
                .HasOne(c => c.Country)
                .WithMany(cn => cn.Cities)
                .HasForeignKey(c => c.CountryID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação City - Airport (um-para-muitos)
            modelBuilder.Entity<Airport>()
                .HasOne(a => a.City)
                .WithMany(c => c.Airports)
                .HasForeignKey(a => a.CityID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação Flight - Airplane (um-para-muitos)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.Airplane)
                .WithMany()
                .HasForeignKey(f => f.AirplaneID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação Flight - DepartureAirport (um-para-muitos)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DepartureAirport)
                .WithMany()
                .HasForeignKey(f => f.DepartureAirportID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação Flight - ArrivalAirport (um-para-muitos)
            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ArrivalAirport)
                .WithMany()
                .HasForeignKey(f => f.ArrivalAirportID)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação Ticket - Flight (um-para-muitos)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Flight)
                .WithMany(f => f.Tickets)
                .HasForeignKey(t => t.FlightId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação Ticket - User (um-para-muitos)
            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configuração da relação User - City (um-para-muitos)
            modelBuilder.Entity<User>()
                .HasOne(u => u.City)
                .WithMany()
                .HasForeignKey(u => u.CityId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
