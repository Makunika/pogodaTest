using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using weatherDesktopWPF.Data;
using weatherDesktopWPF.Models;
using weatherDesktopWPF.Requests;
using weatherDesktopWPF.Service;

namespace weatherDesktopWPF
{
    public partial class MainWindow : Window
    {
        private TownWeatherService _townWeatherService;
        
        public MainWindow()
        {
            InitializeComponent();
            //Инициализируем сервис
            _townWeatherService = new TownWeatherService();
            _ = InitAsync();
        }
        
        private void comboBoxTowns_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBoxTowns.SelectedItem != null)
            {
                _ = WeatherUpdateAsync(comboBoxTowns.SelectedItem.ToString());
            }
        }
        
        private void buttonUpdate_Click(object sender, RoutedEventArgs e)
        {
            UpdateAllTowns();
        }
        
        /// <summary>
        /// Удалание и скачиванее заного всех городов и показ погоды для ранее выбранного города
        /// </summary>
        private async void UpdateAllTowns()
        {
            string name = labelTownName.Content.ToString();
            _townWeatherService.ClearAll();
            await InitAsync();
            comboBoxTowns.Text = name;
            await WeatherUpdateAsync(name);

        }

        /// <summary>
        /// Получе всех городов, для которых можно получить погоду. Погода при этом не качается.
        /// </summary>
        private async Task InitAsync()
        {
            //Очищаем комбобокс от прежних городов
            comboBoxTowns.Items.Clear();
            List<Town> towns = new List<Town>();
            string error = "";
            //Получаем все доступные города
            await Task.Run(() =>
            {
                try
                {
                    towns = _townWeatherService.GetAllTown();
                }
                catch (Exception e)
                {
                    error = "Ошибка получения данных";
                }
            });
            //Обработка ошибки
            if (error != "")
            {
                labelTownName.Content = error;
            } 
            else 
            {
                //Заполняем комбобокс
                foreach (var town in towns)
                {
                    comboBoxTowns.Items.Add(town.Name);
                }
            }
        }

        /// <summary>
        /// Получение погоды по названию города
        /// </summary>
        /// <param name="name">Название города</param>
        private async Task WeatherUpdateAsync(string name)
        {
            //Очищаем врап контейнер от прежней погоды
            wrapWeather.Children.Clear();
            Town town = null;
            string error = "";
            //Получаем данные ассинхронно
            await Task.Run(() =>
            {
                try
                {
                    town = _townWeatherService.GetWeatherByNameTown(name);
                }
                catch (Exception e)
                {
                    error = "Ошибка получения данных";
                }
            });
            //Обработка ошибок
            if (error != "" || town == null)
            {
                labelTownName.Content = error == "" ? "Название города неправильное!" : error;
            }
            else
            {
                //Если все верно - добавляем во врап контейнер карточки с погодой.
                labelTownName.Content = town.Name;
                foreach (var weather in town.Weathers.OrderBy(t => t.DateTime))
                {
                    AddWeatherInWrap(weather);
                }
                //Чтобы не перегружалась память - удаляем погоду, чтобы не сохранялась в памяти при смене города.
                town.Weathers.Clear();
            }
        }

        /// <summary>
        /// Добавляем во врап контейер карточку с погодой
        /// </summary>
        /// <param name="weather">погода</param>
        private void AddWeatherInWrap(Weather weather)
        {
            DateTime dateNow = DateTime.Now;
            StackPanel stackPanel = new StackPanel();
            stackPanel.Margin = new Thickness(10);
            stackPanel.Width = 120;
            stackPanel.Height = 110;
            stackPanel.Orientation = Orientation.Vertical;
            stackPanel.Children.Add(new Label
            {
                Content = weather.DateTime.ToShortDateString(),
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Background = null
            });
            stackPanel.Children.Add(new Label
            {
                Content = weather.Degrees > 0 ? $"+{weather.Degrees} C" : $"{weather.Degrees} C",
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Background = null
            });
            stackPanel.Children.Add(new Label
            {
                Content = weather.About,
                Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Background = null
            });

            if (weather.DateTime.DayOfYear == dateNow.DayOfYear)
            {
                Border border = new Border
                {
                    Child = stackPanel,
                    BorderBrush = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    BorderThickness = new Thickness(2)
                };
                wrapWeather.Children.Add(border);
            }
            else
            {
                wrapWeather.Children.Add(stackPanel);
            }
        }
    }
}
