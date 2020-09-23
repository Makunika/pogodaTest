using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using pogodaTest.Data;
using pogodaTest.Models;

namespace pogodaTest.Modules
{
    /// <summary>
    /// Сервис по получению погоды с яндекс погоды раз в час.
    /// </summary>
    public class WeatherUpdate : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger<WeatherUpdate> _logger;
        private const string Domen = "https://yandex.ru";
        private const string Region = "/pogoda/region/10650?via=brd";

        public WeatherUpdate(ILogger<WeatherUpdate> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //Создание таймера
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1));
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Парсит html страницы яндекс погоды
        /// </summary>
        private async void DoWork(object obj)
        {
            _logger.LogInformation("Получение всех городов.");
            TownWeatherContext townWeatherContext = new TownWeatherContext();
            WebRequest webRequest = WebRequest.Create(Domen + Region);
            webRequest.Method = "GET";
            WebResponse response = await webRequest.GetResponseAsync();
            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                //Парсим html
                string html = reader.ReadToEnd();
                HtmlParser parser = new HtmlParser();
                IHtmlDocument document = parser.ParseDocument(html);
                foreach (IElement element in document.QuerySelectorAll(".place-list__item-name"))
                {
                    string townName = element.InnerHtml;
                    //Если город уже есть в бд, то не добавляем его, а обновляем ему погоду.
                    Town town = townWeatherContext
                        .Towns
                        .Where(t => t.Name == townName)
                        .Include(t => t.Weathers)
                        .FirstOrDefault();
                        
                    if (town == null)
                    {
                        town = new Town
                        {
                            Name = townName, 
                            Weathers = new List<Weather>()
                        };
                        SetWeathers(element.GetAttribute("href"), town);
                        townWeatherContext.Towns.Add(town);
                    }
                    else
                    {
                        town.Weathers.Clear();
                        townWeatherContext.SaveChanges();
                        SetWeathers(element.GetAttribute("href"), town);
                    }
                    townWeatherContext.SaveChanges();
                }
            }
            response.Close();

        }

        /// <summary>
        /// Обновляем погоду у заданного города
        /// </summary>
        private void SetWeathers(string href, Town town)
        {
            _logger.LogInformation("Получение погоды " + town.Name);
            
            
            WebRequest webRequest = WebRequest.Create(Domen + href);
            webRequest.Method = "GET";
            
            WebResponse response = webRequest.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                string html = reader.ReadToEnd();
                HtmlParser parser = new HtmlParser();
                IHtmlDocument document = parser.ParseDocument(html);
                foreach (IElement element in document.QuerySelectorAll(".forecast-briefly__day"))
                {
                    Weather weather = new Weather();
                    weather.Degrees = int.Parse(element
                        .QuerySelector(".forecast-briefly__temp_day")
                        .QuerySelector(".temp__value")
                        .InnerHtml);
                    weather.DateTime = DateTime.Parse(element.QuerySelector("time").GetAttribute("datetime"));
                    weather.About = element.QuerySelector(".forecast-briefly__condition").InnerHtml;
                    town.Weathers.Add(weather);
                }
            }
            response.Close();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}