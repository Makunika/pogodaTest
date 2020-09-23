using System;

namespace weatherDesktopWPF.Models
{
    public class Weather
    {
        public int WeatherId { get; set; }
        
        public DateTime DateTime { get; set; }
        
        public int Degrees { get; set; }

        public string About { get; set; }
        
        public int TownId { get; set; }
    }
}