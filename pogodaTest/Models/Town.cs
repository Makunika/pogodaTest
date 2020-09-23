using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pogodaTest.Models
{
    public class Town
    {
        [Key]
        public int TownId { get; set; }
        
        [Column("TownName")]
        public string Name { get; set; }
        
        public List<Weather> Weathers { get; set; }
    }
}