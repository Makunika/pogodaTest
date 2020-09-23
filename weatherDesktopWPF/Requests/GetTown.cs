using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using weatherDesktopWPF.Models;

namespace weatherDesktopWPF.Requests
{
    public class GetTowns
    {
        private const string Domen = "https://localhost:5001"; 
        
        /// <summary>
        /// Отправляет REST запрос на все города
        /// </summary>
        /// <returns></returns>
        public static List<Town> GetAllTowns()
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate {return true;};
            List<Town> towns = new List<Town>();
            WebRequest request = WebRequest.Create(Domen + "/api/Towns");
            WebResponse response = request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                
                string resonseText = stream.ReadToEnd();
                towns = JsonConvert.DeserializeObject<List<Town>>(resonseText);
            }

            return towns;
        }

        /// <summary>
        /// Отправет REST запрос погоды по городу
        /// </summary>
        /// <param name="town">Город</param>
        /// <returns></returns>
        public static Town GetTownByTown(Town town)
        {
            ServicePointManager.ServerCertificateValidationCallback += delegate {return true;};
            WebRequest request = WebRequest.Create(Domen + "/api/Towns/" + town.TownId);
            WebResponse response = request.GetResponse();
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                string resonseText = stream.ReadToEnd();
                Town townNew = JsonConvert.DeserializeObject<Town>(resonseText);
                town.Name = townNew.Name;
                town.Weathers = townNew.Weathers;
                town.TownId = townNew.TownId;
            }

            return town;
        }
    }
}