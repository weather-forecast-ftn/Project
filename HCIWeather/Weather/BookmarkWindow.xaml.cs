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
using System.Windows.Shapes;

namespace Weather
{
    /// <summary>
    /// Interaction logic for BookmarkWindow.xaml
    /// </summary>
    /// 
    public class MyGrid : Grid
    {
        public string CityName { get; set; }
        public MyGrid() { }

    }
    public partial class BookmarkWindow : Window
    {
        public BookmarkWindow()
        {
            InitializeComponent();
            loadBookmarkAPI();

        }
        public async void loadBookmarkAPI()
        {
            Bookmark bookmark = new Bookmark();



            foreach (BookmarkData bd in bookmark.getHistory())
            {
                MyGrid LabelGrid = new MyGrid();
                LabelGrid.CityName = bd.CityName;
                LabelGrid.Background = getLightDailyColor();
                LabelGrid.PreviewMouseDown += LabelGrid_PreviewMouseDown;
                LabelGrid.MouseEnter += LabelGrid_MouseEnter;
                LabelGrid.MouseLeave += LabelGrid_MouseLeave;
                LabelGrid.Margin = new Thickness(20, 20, 20, 20);
                LabelGrid.Height = 130;
                LabelGrid.Width = 200;
                LabelGrid.ColumnDefinitions.Clear();
                LabelGrid.RowDefinitions.Clear();

                LabelGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(2, GridUnitType.Star) });
                LabelGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(4, GridUnitType.Star) });
                LabelGrid.RowDefinitions.Clear();
                BasicWeatherRecord bwr = await WeatherProcess.LoadBasicWeatherRecord(bd.CityName);
                LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });
                LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(4, GridUnitType.Star) });
                LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2.5, GridUnitType.Star) });
                LabelGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(2.5, GridUnitType.Star) });


                Label cityName = new Label();
                cityName.Content = bd.CityName;
                cityName.FontSize = 22;
                cityName.HorizontalAlignment = HorizontalAlignment.Center;
                cityName.Foreground = new SolidColorBrush(Colors.White);
                MyGrid.SetRow(cityName, 0);
                MyGrid.SetColumnSpan(cityName, 2);
                LabelGrid.Children.Add(cityName);


                Label currTemp = new Label();
                currTemp.Content = Convert.ToInt32(bwr.Main.Temp) + "°C";
                currTemp.FontSize = 22;
                currTemp.Foreground = new SolidColorBrush(Colors.White);
                MyGrid.SetRow(currTemp, 1);
                MyGrid.SetColumnSpan(currTemp, 2);
                currTemp.HorizontalAlignment = HorizontalAlignment.Center;
                LabelGrid.Children.Add(currTemp);

                Label maxTemp = new Label();
                maxTemp.Content = "Max: " + Convert.ToInt32(bwr.Main.Temp_max) + "°C";
                //maxTemp.FontSize = 15;
                maxTemp.Foreground = new SolidColorBrush(Colors.White);
                MyGrid.SetRow(maxTemp, 2);
                MyGrid.SetColumn(maxTemp, 1);
                maxTemp.HorizontalAlignment = HorizontalAlignment.Right;
                LabelGrid.Children.Add(maxTemp);

                Label minTemp = new Label();
                minTemp.Content = "Min: " + Convert.ToInt32(bwr.Main.Temp_min) + "°C";
                //minTemp.FontSize = 15;
                minTemp.HorizontalAlignment = HorizontalAlignment.Right;
                minTemp.Foreground = new SolidColorBrush(Colors.White);
                MyGrid.SetRow(minTemp, 3);
                MyGrid.SetColumn(minTemp, 1);
                LabelGrid.Children.Add(minTemp);

                Label description = new Label();
                description.Content = bwr.Weathers[0].Main;
                description.Foreground = new SolidColorBrush(Colors.White);
                MyGrid.SetRow(description, 3);
                MyGrid.SetColumn(description, 0);
                LabelGrid.Children.Add(description);

                Image image = new Image();
                image.Source = getImage(bwr.Weathers[0].Icon);
                image.HorizontalAlignment = HorizontalAlignment.Left;
                MyGrid.SetColumn(image, 0);
                MyGrid.SetRow(image, 2);
                LabelGrid.Children.Add(image);
                //I want to creatre the Seperate coloum and row to  display KEY
                // VALUE Pair distinctly
                bookmarkList.Children.Add(LabelGrid);
            }
        }

        private void LabelGrid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            MyGrid mg = sender as MyGrid;
            mg.Background = getDarkDailyColor();
            MainWindow mw = new MainWindow();
            mw.Show();
            mw.loadAPI(mg.CityName);
            this.Close();
        }

        public SolidColorBrush getDarkDailyColor()
        {
            return (SolidColorBrush)Resources["dailyDarkColor"];
        }

        public SolidColorBrush getLightDailyColor()
        {
            return (SolidColorBrush)Resources["dailyLightColor"];

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            win2.Show();
            this.Close();
        }

        public BitmapImage getImage(string logo)
        {
            List<string> resourceImages = new List<string>() { "01d", "01n", "02d", "03d", "03n", "04d", "04n", "09d", "09n", "10d", "10n", "13d", "13n" };
            Boolean check = false;
            BitmapImage image = new BitmapImage(new Uri(AppInfo.getIconURL(logo)));
            foreach (string res in resourceImages)
            {
                if (res.Equals(logo))
                {
                    check = true;
                    image = new BitmapImage(new Uri(string.Format(@"\Resources\{0}.png", logo), UriKind.Relative));
                    break;
                }
            }

            if (!check)
            {
                image = new BitmapImage(new Uri(AppInfo.getIconURL(logo)));

            }

            return image;
        }

        private void LabelGrid_MouseLeave(object sender, MouseEventArgs e)
        {
            MyGrid mg = sender as MyGrid;
            mg.Background = getLightDailyColor();
        }

        private void LabelGrid_MouseEnter(object sender, MouseEventArgs e)
        {
            MyGrid mg = sender as MyGrid;
            mg.Background = getDarkDailyColor();
        }
    }
}