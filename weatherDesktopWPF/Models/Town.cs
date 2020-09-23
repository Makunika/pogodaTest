using System.Collections.Generic;

namespace weatherDesktopWPF.Models
{
    public class Town
    {
        public int TownId { get; set; }
        
        public string Name { get; set; }
        
        public List<Weather> Weathers { get; set; }
    }
}