using System;
using System.Collections.Generic;
using System.Linq;
using weatherDesktopWPF.Data;
using weatherDesktopWPF.Models;
using weatherDesktopWPF.Requests;

namespace weatherDesktopWPF.Service
{
    public class TownWeatherService
    {
        /// <summary>
        /// Наш локальный репозиторий городов
        /// </summary>
        private TownsSingelton _townsSingelton;

        public TownWeatherService()
        {
            _townsSingelton = TownsSingelton.Instance;
        }

        /// <summary>
        /// Получает список всех городов без погоды
        /// </summary>
        /// <returns></returns>
        public List<Town> GetAllTown()
        {
            _townsSingelton.Towns = GetTowns.GetAllTowns();
            return _townsSingelton.Towns;
        }

        /// <summary>
        /// Получает погоду по названию города
        /// </summary>
        /// <param name="name">Название города</param>
        public Town GetWeatherByNameTown(string name)
        {
            Town town = _townsSingelton.Towns.Where(t => t.Name == name).FirstOrDefault();
            if (town == null)
            {
                return null;
            }
            
            return GetTowns.GetTownByTown(town);
        }

        /// <summary>
        /// Очищает локальный репозиторий
        /// </summary>
        public void ClearAll()
        {
            _townsSingelton.Towns.Clear();
        }
    }
}