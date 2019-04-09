using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Weather
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string historyPath = Path.GetFullPath(@"..\..\") + @"resources\history.txt";
        private readonly string bookmarkPath = Path.GetFullPath(@"..\..\") + @"resources\bookmark.txt";
        public List<string> nameList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            string dateNow = DateTime.Now.ToString("HH");
            int hour = Int32.Parse(dateNow);
            if(hour > 18 || hour < 7)
            {
                ImageBrush myBrush = new ImageBrush();
                myBrush.ImageSource = GetImage("resources/nightSky.jpg");
                window.Background = myBrush;
            }
            
            AppInfo.InitializeClient();
            if (AppInfo.Current_city.Equals(""))
            {
                GetLocationProperty();
            }

            loadAPI(AppInfo.Current_city);

            
        }

        private  void Search_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key != Key.Enter)
                return;

            string searchInput = search.Text.Trim().ToLower();

            loadAPI(searchInput);

            //History
            e.Handled = true;
            History history = new History();

            foreach (HistoryData item in history.getHistory())
            {
                if (item.CityName.Trim().ToLower().Equals(search.Text.Trim().ToLower()))
                    break;
            }

            checkFavourite(search.Text);
            File.AppendAllText(historyPath, search.Text.Trim() + "\n");
            string s = DateTime.Now.ToString();
            File.AppendAllText(historyPath, s.Trim() + "\n");


        }

        private void SearchClickFun(object sender, RoutedEventArgs e)
        {
            string searchInput = search.Text.Trim().ToLower();

            loadAPI(searchInput);
            //History
            e.Handled = true;
            History history = new History();

            foreach (HistoryData item in history.getHistory())
            {
                if (item.CityName.Trim().ToLower().Equals(search.Text.Trim().ToLower()))
                    break;
            }

            checkFavourite(search.Text);
            File.AppendAllText(historyPath, search.Text.Trim() + "\n");
            string s = DateTime.Now.ToString();
            File.AppendAllText(historyPath, s.Trim() + "\n");

        }

        public void GetLocationProperty()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            watcher.PositionChanged += watcher_PositionChanged;

            watcher.Start();

        }

        private void watcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            AppInfo.city_lat = e.Position.Location.Latitude;
            AppInfo.city_lon = e.Position.Location.Longitude;
        }

        public async void loadAPI(string searchInput)
        {

            checkFavourite(searchInput);

            BasicWeatherRecord bwr = null;
            BasicWeatherRecord bwrT = null;
            DetailWeatherRecord dwr = null;

            if (searchInput.Equals(""))
            {
                bwrT = await WeatherProcess.LoadBasicWeatherRecordCoord();
                bwr = await WeatherProcess.LoadBasicWeatherRecordCoord();
                dwr = await WeatherProcess.LoadDetailWeatherRecordCoord();
            }
            else
            {
                bwr = await WeatherProcess.LoadBasicWeatherRecord(searchInput);
                dwr = await WeatherProcess.LoadDetailWeatherRecord(searchInput);
            }

            

            if(bwr == null || dwr == null)
            {
                return;
            }

            cityName.Content = dwr.city.Name + ", " + dwr.city.Country;
            currentForecastTemperature.Content = Convert.ToInt32(bwr.Main.Temp) + "°C";
            currentForecastStatus.Content = bwr.Weathers[0].Description;
            currentForecastInfo.Content = "Last update at " + DateTime.Now.ToString("HH:mm:ss");

            currentForecastLogo.Source = getImage(bwr.Weathers[0].Icon);

            List<PerHourRecord> recordsDay2 = new List<PerHourRecord>();
            List<PerHourRecord> recordsDay3 = new List<PerHourRecord>();
            List<PerHourRecord> recordsDay4 = new List<PerHourRecord>();
            List<PerHourRecord> recordsDay5 = new List<PerHourRecord>();

            string dateDay2 = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            string dateDay3 = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");
            string dateDay4 = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");
            string dateDay5 = DateTime.Now.AddDays(4).ToString("yyyy-MM-dd");


            foreach (PerHourRecord phr in dwr.Records){
                if (phr.time.StartsWith(dateDay2)){
                    recordsDay2.Add(phr);
                }else if (phr.time.StartsWith(dateDay3)){
                    recordsDay3.Add(phr);
                }else if (phr.time.StartsWith(dateDay4)){
                    recordsDay4.Add(phr);
                }else if(phr.time.StartsWith(dateDay5)){
                    recordsDay5.Add(phr);
                }
            }
            

            Day1Date.Content = DateTime.Today.ToString("dddd").Substring(0, 3) + " " + DateTime.Today.ToString("dd");
            Day1Logo.Source = getImage(bwr.Weathers[0].Icon);
            Day1Max.Content = Convert.ToInt32(bwr.Main.Temp_max) + "°C";
            Day1Min.Content = Convert.ToInt32(bwr.Main.Temp_min) + "°C";
            Day1Description.Content = bwr.Weathers[0].Main;

            List<string> res2 = getMinAndMaxTemp(recordsDay2);
            Day2Date.Content = DateTime.Today.AddDays(1).ToString("dddd").Substring(0, 3) + " " + DateTime.Today.AddDays(1).ToString("dd");
            Day2Max.Content = Convert.ToInt32(res2[1]) + "°C";
            Day2Min.Content = Convert.ToInt32(res2[0]) + "°C";
            Day2Description.Content = res2[2];
            Day2Logo.Source = getImage(res2[3]); 


            List<string> res3 = getMinAndMaxTemp(recordsDay3);
            Day3Date.Content = DateTime.Today.AddDays(2).ToString("dddd").Substring(0, 3) + " " + DateTime.Today.AddDays(2).ToString("dd");
            Day3Max.Content = Convert.ToInt32(res3[1]) + "°C";
            Day3Min.Content = Convert.ToInt32(res3[0]) + "°C";
            Day3Description.Content = res3[2];
            Day3Logo.Source = getImage(res3[3]);


            List<string> res4 = getMinAndMaxTemp(recordsDay4);
            Day4Date.Content = DateTime.Today.AddDays(3).ToString("dddd").Substring(0, 3) + " " + DateTime.Today.AddDays(3).ToString("dd");
            Day4Max.Content = Convert.ToInt32(res4[1]) + "°C";
            Day4Min.Content = Convert.ToInt32(res4[0]) + "°C";
            Day4Description.Content = res4[2];
            Day4Logo.Source = getImage(res4[3]);


            List<string> res5 = getMinAndMaxTemp(recordsDay5);
            Day5Date.Content = DateTime.Today.AddDays(4).ToString("dddd").Substring(0, 3) + " " + DateTime.Today.AddDays(4).ToString("dd");
            Day5Max.Content = Convert.ToInt32(res5[1]) + "°C";
            Day5Min.Content = Convert.ToInt32(res5[0]) + "°C";
            Day5Description.Content = res5[2];
            Day5Logo.Source = getImage(res5[3]);




            Detail1Temp.Content = Convert.ToInt32(dwr.Records[0].Main.Temp) + "°C";
            Detail1Description.Content = dwr.Records[0].Weathers[0].Main;
            Detail1Time.Content = dwr.Records[0].time.Substring(dwr.Records[0].time.Length - 8, 5);
            Detail1Logo.Source = getImage(dwr.Records[0].Weathers[0].Icon);


            Detail2Temp.Content = Convert.ToInt32(dwr.Records[1].Main.Temp) + "°C";
            Detail2Description.Content = dwr.Records[1].Weathers[0].Main;
            Detail2Time.Content = dwr.Records[1].time.Substring(dwr.Records[1].time.Length - 8, 5);
            Detail2Logo.Source = getImage(dwr.Records[1].Weathers[0].Icon);


            Detail3Temp.Content = Convert.ToInt32(dwr.Records[2].Main.Temp) + "°C";
            Detail3Description.Content = dwr.Records[2].Weathers[0].Main;
            Detail3Time.Content = dwr.Records[2].time.Substring(dwr.Records[2].time.Length - 8, 5);
            Detail3Logo.Source = getImage(dwr.Records[2].Weathers[0].Icon);


            Detail4Temp.Content = Convert.ToInt32(dwr.Records[3].Main.Temp) + "°C";
            Detail4Description.Content = dwr.Records[3].Weathers[0].Main;
            Detail4Time.Content = dwr.Records[3].time.Substring(dwr.Records[3].time.Length - 8, 5);
            Detail4Logo.Source = getImage(dwr.Records[3].Weathers[0].Icon);


            Detail5Temp.Content = Convert.ToInt32(dwr.Records[4].Main.Temp) + "°C";
            Detail5Description.Content = dwr.Records[4].Weathers[0].Main;
            Detail5Time.Content = dwr.Records[4].time.Substring(dwr.Records[4].time.Length - 8, 5);
            Detail5Logo.Source = getImage(dwr.Records[4].Weathers[0].Icon);


            Detail6Temp.Content = Convert.ToInt32(dwr.Records[5].Main.Temp) + "°C";
            Detail6Description.Content = dwr.Records[5].Weathers[0].Main;
            Detail6Time.Content = dwr.Records[5].time.Substring(dwr.Records[5].time.Length - 8, 5);
            Detail6Logo.Source = getImage(dwr.Records[5].Weathers[0].Icon);


            Detail7Temp.Content = Convert.ToInt32(dwr.Records[6].Main.Temp) + "°C";
            Detail7Description.Content = dwr.Records[6].Weathers[0].Main;
            Detail7Time.Content = dwr.Records[6].time.Substring(dwr.Records[6].time.Length - 8, 5);
            Detail7Logo.Source = getImage(dwr.Records[6].Weathers[0].Icon);


            Detail8Temp.Content = Convert.ToInt32(dwr.Records[7].Main.Temp) + "°C";
            Detail8Description.Content = dwr.Records[7].Weathers[0].Main;
            Detail8Time.Content = dwr.Records[7].time.Substring(dwr.Records[7].time.Length - 8, 5);
            Detail8Logo.Source = getImage(dwr.Records[7].Weathers[0].Icon);

            if (searchInput.Equals(""))
            {
                string currentLocation = cityName.Content.ToString();
                List<string> currentCityList = currentLocation.Split(',').ToList<string>();
                string currentCity = currentCityList[0];
                checkFavourite(currentCity);

            }

        }

        public List<string> getMinAndMaxTemp(List<PerHourRecord> list)
        {
            List<int> MinAndMax = new List<int>() { 9999, -9999 };
            string description = "Sunny";
            string icon = "01d";
            foreach(PerHourRecord phr in list)
            {
                if(phr.Main.Temp_min < MinAndMax[0])
                {
                    MinAndMax[0] = Convert.ToInt32(phr.Main.Temp_min);
                }

                if(phr.Main.Temp_max > MinAndMax[1])
                {
                    MinAndMax[1] = Convert.ToInt32(phr.Main.Temp_max);
                    description = phr.Weathers[0].Main;
                    icon = phr.Weathers[0].Icon;
                }
            }

            return new List<string> { MinAndMax[0].ToString(), MinAndMax[1].ToString(), description, icon};
        }

        public BitmapImage getImage(string logo)
        {
            List<string> resourceImages = new List<string>() {"01d","01n","02d","03d","03n","04d","04n","09d","09n","10d","10n","13d","13n"};
            Boolean check = false;
            BitmapImage image = new BitmapImage(new Uri(AppInfo.getIconURL(logo)));
            foreach (string res in resourceImages)
            {
                if (res.Equals(logo))
                {
                    check = true;
                    image = new BitmapImage(new Uri(string.Format(@"\Resources\{0}.png",logo), UriKind.Relative));
                    break;
                }
            }

            if (!check)
            {
                image = new BitmapImage(new Uri(AppInfo.getIconURL(logo)));

            }

            return image;
        }

        public async void loadTemperaturePerHour(string searchInput, string date, bool middle)
        {

            DetailWeatherRecord dwr = await WeatherProcess.LoadDetailWeatherRecord(searchInput);

            List<PerHourRecord> records = new List<PerHourRecord>();

            string today = DateTime.Now.ToString("yyyy-MM-dd");
            string lastDay = DateTime.Now.AddDays(4).ToString("yyyy-MM-dd");
            if (today == date) {
                for (int i = 0; i < 12; i++)
                {
                    records.Add(dwr.Records[i]);
                }
            } else if (lastDay == date) {

                int lasti = dwr.Records.Count - 8;
                for(int i= lasti; i<dwr.Records.Count; i++)
                {
                    records.Add(dwr.Records[i]);
                }

            } else {
                foreach (PerHourRecord phr in dwr.Records)
                {
                    if (phr.time.StartsWith(date))
                    {
                        records.Add(phr);
                    }
                }
            }
            
            

            Detail1Temp.Content = Convert.ToInt32(records[0].Main.Temp) + "°C";
            Detail1Description.Content = records[0].Weathers[0].Main;
            Detail1Time.Content = records[0].time.Substring(records[0].time.Length - 8, 5);
            Detail1Logo.Source = getImage(records[0].Weathers[0].Icon);

           
            Detail2Temp.Content = Convert.ToInt32(records[1].Main.Temp) + "°C";
            Detail2Description.Content = records[1].Weathers[0].Main;
            Detail2Time.Content = records[1].time.Substring(records[1].time.Length - 8, 5);
            Detail2Logo.Source = getImage(records[1].Weathers[0].Icon);


            Detail3Temp.Content = Convert.ToInt32(records[2].Main.Temp) + "°C";
            Detail3Description.Content = records[2].Weathers[0].Main;
            Detail3Time.Content = records[2].time.Substring(records[2].time.Length - 8, 5);
            Detail3Logo.Source = getImage(records[2].Weathers[0].Icon);


            Detail4Temp.Content = Convert.ToInt32(records[3].Main.Temp) + "°C";
            Detail4Description.Content = records[3].Weathers[0].Main;
            Detail4Time.Content = records[3].time.Substring(records[3].time.Length - 8, 5);
            Detail4Logo.Source = getImage(records[3].Weathers[0].Icon);


            Detail5Temp.Content = Convert.ToInt32(records[4].Main.Temp) + "°C";
            Detail5Description.Content = records[4].Weathers[0].Main;
            Detail5Time.Content = records[4].time.Substring(records[4].time.Length - 8, 5);
            Detail5Logo.Source = getImage(records[4].Weathers[0].Icon);


            Detail6Temp.Content = Convert.ToInt32(records[5].Main.Temp) + "°C";
            Detail6Description.Content = records[5].Weathers[0].Main;
            Detail6Time.Content = records[5].time.Substring(records[5].time.Length - 8, 5);
            Detail6Logo.Source = getImage(records[5].Weathers[0].Icon);


            Detail7Temp.Content = Convert.ToInt32(records[6].Main.Temp) + "°C";
            Detail7Description.Content = records[6].Weathers[0].Main;
            Detail7Time.Content = records[6].time.Substring(records[6].time.Length - 8, 5);
            Detail7Logo.Source = getImage(records[6].Weathers[0].Icon);


            Detail8Temp.Content = Convert.ToInt32(records[7].Main.Temp) + "°C";
            Detail8Description.Content = records[7].Weathers[0].Main;
            Detail8Time.Content = records[7].time.Substring(records[7].time.Length - 8, 5);
            Detail8Logo.Source = getImage(records[7].Weathers[0].Icon);
            

        }

        public SolidColorBrush getDarkDailyColor()
        {
             return (SolidColorBrush)Resources["dailyDarkColor"];
        }

        public SolidColorBrush getLightDailyColor()
        {
            return  (SolidColorBrush)Resources["dailyLightColor"];
            
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            string currentCity = currentCityList[0];

            grid1.Background = getDarkDailyColor();
            grid2.Background = getLightDailyColor();
            grid3.Background = getLightDailyColor();
            grid4.Background = getLightDailyColor();
            grid5.Background = getLightDailyColor();

            loadAPI(currentCity);
        }
        

        private void Grid1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            grid1.Background = getDarkDailyColor();
            grid2.Background = getLightDailyColor();
            grid3.Background = getLightDailyColor();
            grid4.Background = getLightDailyColor();
            grid5.Background = getLightDailyColor();

            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            string currentCity = currentCityList[0];

            string date = DateTime.Now.ToString("yyyy-MM-dd");

            loadTemperaturePerHour(currentCity, date,false);
        }

        private void Grid2_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            grid1.Background = getLightDailyColor();
            grid2.Background = getDarkDailyColor();
            grid3.Background = getLightDailyColor();
            grid4.Background = getLightDailyColor();
            grid5.Background = getLightDailyColor();

            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            string currentCity = currentCityList[0];

            string date = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            loadTemperaturePerHour(currentCity, date,true);
        }

        private void Grid3_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            grid1.Background = getLightDailyColor();
            grid2.Background = getLightDailyColor();
            grid3.Background = getDarkDailyColor();
            grid4.Background = getLightDailyColor();
            grid5.Background = getLightDailyColor();

            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            string currentCity = currentCityList[0];

            string date = DateTime.Now.AddDays(2).ToString("yyyy-MM-dd");

            loadTemperaturePerHour(currentCity, date,true);
        }

        private void Grid4_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            grid1.Background = getLightDailyColor();
            grid2.Background = getLightDailyColor();
            grid3.Background = getLightDailyColor();
            grid4.Background = getDarkDailyColor();
            grid5.Background = getLightDailyColor();

            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            string currentCity = currentCityList[0];

            string date = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd");

            loadTemperaturePerHour(currentCity, date, true);
        }

        private void Grid5_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            grid1.Background = getLightDailyColor();
            grid2.Background = getLightDailyColor();
            grid3.Background = getLightDailyColor();
            grid4.Background = getLightDailyColor();
            grid5.Background = getDarkDailyColor();

            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            string currentCity = currentCityList[0];

            string date = DateTime.Now.AddDays(4).ToString("yyyy-MM-dd");

            loadTemperaturePerHour(currentCity, date, false);
        }

        

        private static BitmapImage GetImage(string imageUri)
        {
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri("pack://application:,,,/" + imageUri);
            bitmapImage.EndInit();
            return bitmapImage;
        }

        private void Bookmark_Click_1(object sender, RoutedEventArgs e)
        {

            string bookmarkCity = cityName.Content.ToString();
            string[] lista = bookmarkCity.Split(',');
            Bookmark bookmark = new Bookmark();
            HashSet<BookmarkData> bookmarkList = bookmark.getHistory();

            if (bookmarkImage.Source.ToString() == "pack://application:,,,/resources/bookmark.png")
            {
                bookmarkImage.Source = GetImage("resources/bookmarked.png");
            }
            else
            {
                bookmarkImage.Source = GetImage("resources/bookmark.png");
            }
            Boolean check = false;
            foreach (BookmarkData item in bookmarkList)
            {
                if (item.CityName.Trim().ToLower().Equals(lista[0].Trim().ToLower()))
                {
                    check = true;
                    bookmarkList.Remove(item);
                    break;

                }
            }
            if (!check)
            {
                File.AppendAllText(bookmarkPath, lista[0].Trim() + "\n");
            }
            else
            {

                File.WriteAllText(bookmarkPath, String.Empty);
                foreach (BookmarkData item in bookmarkList)
                {
                    File.AppendAllText(bookmarkPath, item.CityName + "\n");
                }

            }
        }

        private void AllBookmarks_Click(object sender, RoutedEventArgs e)
        {
            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            AppInfo.Current_city = currentCityList[0];
            BookmarkWindow win2 = new BookmarkWindow();
            win2.Show();
            this.Close();
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            string currentLocation = cityName.Content.ToString();
            List<string> currentCityList = currentLocation.Split(',').ToList<string>();
            AppInfo.Current_city = currentCityList[0];
            HistoryWindow win2 = new HistoryWindow();
            win2.Show();
            this.Close();
        }

        public bool checkFavourite(string city)
        {
            Bookmark bookmark = new Bookmark();
            foreach (BookmarkData item in bookmark.getHistory())
            {
                if (item.CityName.Trim().ToLower().Equals(city.Trim().ToLower()))
                {
                    bookmarkImage.Source = GetImage("resources/bookmarked.png");
                    return true;
                }
            }
            bookmarkImage.Source = GetImage("resources/bookmark.png");
            return false;
        }

    }
}
