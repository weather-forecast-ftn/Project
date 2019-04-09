using System;
using System.Collections.Generic;
using System.IO;

namespace Weather
{

    public class HistoryData
    {
        public string CityName { get; set; }
        public string SearchTime { get; set; }
    }

    class History
    {
        private HashSet<HistoryData> historySet { get; set; }
        private readonly string historyPath = Path.GetFullPath(@"..\..\") + @"resources\history.txt";

        public History()
        {
            historySet = new HashSet<HistoryData>();
        }

        public HashSet<HistoryData> getHistory()
        {
            historySet.Clear();

            if (File.Exists(historyPath))
            {
                string[] searchedPlaces = File.ReadAllLines(historyPath);
                int i = 0;
                HistoryData histData = new HistoryData();


                foreach (string place in searchedPlaces)
                {
                    i++;
                    if (place != "")
                        if (i % 2 == 1)
                        {
                            histData = new HistoryData();
                            histData.CityName = place;
                        }
                        else
                        {
                            histData.SearchTime = place;
                            historySet.Add(histData);
                        }
                }
            }

            return historySet;
        }
    }
}
