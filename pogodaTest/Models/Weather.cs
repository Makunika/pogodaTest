using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pogodaTest.Models
{
    public class Weather
    {
        [Key]
        public int WeatherId { get; set; }
        
        [Column("WeatherDateTime", TypeName = "DATETIME")]
        public DateTime DateTime { get; set; }
        
        [Column("Degree")]
        public int Degrees { get; set; }

        public string About { get; set; }
        
        public int TownId { get; set; }
    }
}