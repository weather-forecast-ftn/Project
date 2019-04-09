using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;

namespace Weather
{

    public static class AppInfo
    {

        public static string API_KEY = "5eb55c79c0b75bc5e36aa494769f0266";

        public static string Current_city = "";

        public static double city_lat = 0;

        public static double city_lon = 0;

        public static string citiesPath = Path.GetFullPath(@"..\..\") + @"resources\cities.json";

        public static string getDetailCityURL(string cityName)
        {
            return string.Format("http://api.openweathermap.org/data/2.5/forecast?q={0}&appid={1}&units=metric", cityName, API_KEY);
        }

        public static string getBasicCityURL(string cityName)
        {
            return string.Format("http://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&mode=json&units=metric", cityName, API_KEY);
        }

        public static string getBasicCityCoordURL(double lon, double lat)
        {
            return string.Format("http://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units=metric",lat,lon,API_KEY);
        }

        public static string getDetailCityCoordURL(double lon, double lat)
        {
            return string.Format("http://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid={2}&units=metric", lat, lon, API_KEY);
        }


        public static List<string> GetCities()
        {
            List<City> cities = new List<City>();
            if (File.Exists(citiesPath))
            {
                string citiesJSON = File.ReadAllText(citiesPath);
                cities = JsonConvert.DeserializeObject<List<City>>(citiesJSON);

            }
            List<string> allCities = new List<string>();
            foreach (City item in cities)
            {
                if (item.Name.Equals("North America") || item.Name.Equals("South America"))
                {
                    continue;
                }
                allCities.Add(item.Name);
            }
            return allCities;
        }

        public static string getIconURL(string name)
        {
            return string.Format("http://openweathermap.org/img/w/{0}.png", name);
        }

        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }


    public class DetailWeatherRecord
    {
        [JsonProperty("city")]
        public CityRecord city { get; set; }

        [JsonProperty("list")]
        public List<PerHourRecord> Records { get; set; }

    }
    
    public class CityRecord
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("coord")]
        public CoordRecord coord { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }
    }
    
    public class PerHourRecord
    {

        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("weather")]
        public List<WeatherElement> Weathers { get; set; }

        [JsonProperty("dt_txt")]
        public string time { get; set; }

    }

    public class Main
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("temp_min")]
        public double Temp_min { get; set; }

        [JsonProperty("temp_max")]
        public double Temp_max { get; set; }

        [JsonProperty("pressure")]
        public double Pressure { get; set; }
    }

    public class WeatherElement
    {
        [JsonProperty("main")]
        public string Main { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

    }

    public class CoordRecord
    {
        [JsonProperty("lat")]
        public double lat { get; set; }

        [JsonProperty("lon")]
        public double lon { get; set; }
    }



    public class BasicWeatherRecord
    {
        [JsonProperty("weather")]
        public List<WeatherElement> Weathers { get; set; }

        [JsonProperty("main")]
        public Main Main { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("sys")]
        public CountrySys CountrySys { get; set; }

    }

    public class CountrySys
    {
        [JsonProperty("country")]
        public string Country { get; set; }
    }


    public class City
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("coord")]
        public CoordRecord coord { get; set; }

    }





}
