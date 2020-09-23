using Microsoft.EntityFrameworkCore;
using pogodaTest.Models;

namespace pogodaTest.Data
{
    public class TownWeatherContext : DbContext
    {
        /* SQL скрипт для развертывания
         
            CREATE TABLE Towns
            (
                TownId INT AUTO_INCREMENT Primary KEY,
                TownName VARCHAR(100) NOT NULL
            );

            CREATE TABLE Weathers
            (
                WeatherId INT AUTO_INCREMENT Primary KEY,
                TownId INT NOT NULL,
                Degree INT NOT NULL,
                About VARCHAR(100) NOT NULL,
                WeatherDateTime DATETIME,
                CONSTRAINT tw_fk FOREIGN KEY (TownId) REFERENCES Towns(TownId)
            );
         */

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