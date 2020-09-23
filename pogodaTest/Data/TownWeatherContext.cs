using Microsoft.EntityFrameworkCore;
using pogodaTest.Models;

namespace pogodaTest.Data
{
    public class TownWeatherContext : DbContext
    {

        public DbSet<Town> Towns { get; set; }
        public DbSet<Weather> Weathers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("server=localhost;UserId=root;Password=12345;database=weathertest;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Town>()
                .HasMany(t => t.Weathers)
                .WithOne();
        }
    }
}