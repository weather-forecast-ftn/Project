using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{
    public class WeatherProcess
    {
        public static async Task<BasicWeatherRecord> LoadBasicWeatherRecord(string cityName = "Novi Sad")
        {
            string url = AppInfo.getBasicCityURL(cityName);

            Console.WriteLine("URL: ", url);
            HttpResponseMessage response = await AppInfo.ApiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                BasicWeatherRecord basicWeatherRecord = await response.Content.ReadAsAsync<BasicWeatherRecord>();
                return basicWeatherRecord;
            }
            else
            {
                return null;
            }

        }
        

        public static async Task<DetailWeatherRecord> LoadDetailWeatherRecord(string cityName = "Novi Sad")
        {
            string url = AppInfo.getDetailCityURL(cityName);

            Console.WriteLine("URL: ", url);
            HttpResponseMessage response = await AppInfo.ApiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                DetailWeatherRecord detailWeatherRecord = await response.Content.ReadAsAsync<DetailWeatherRecord>();
                return detailWeatherRecord;
            }
            else
            {
                return null;
            }

        }



        public static async Task<BasicWeatherRecord> LoadBasicWeatherRecordCoord()
        {
            string url = AppInfo.getBasicCityCoordURL(AppInfo.city_lon, AppInfo.city_lat);

            Console.WriteLine("URL: ", url);
            HttpResponseMessage response = await AppInfo.ApiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                BasicWeatherRecord basicWeatherRecord = await response.Content.ReadAsAsync<BasicWeatherRecord>();
                return basicWeatherRecord;
            }
            else
            {
                return null;
            }

        }


        public static async Task<DetailWeatherRecord> LoadDetailWeatherRecordCoord()
        {
            string url = AppInfo.getDetailCityCoordURL(AppInfo.city_lon, AppInfo.city_lat);

            Console.WriteLine("URL: ", url);
            HttpResponseMessage response = await AppInfo.ApiClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                DetailWeatherRecord detailWeatherRecord = await response.Content.ReadAsAsync<DetailWeatherRecord>();
                return detailWeatherRecord;
            }
            else
            {
                return null;
            }

        }

    }
}
