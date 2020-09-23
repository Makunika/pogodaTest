using System.Collections.Generic;
using weatherDesktopWPF.Models;

namespace weatherDesktopWPF.Data
{
    public class TownsSingelton
    {
        private TownsSingelton()
        {
            
        }

        public static TownsSingelton Instance { get; } = new TownsSingelton
        {
            Towns = new List<Town>()
        };
        
        public List<Town> Towns { get; set; }
    }
}