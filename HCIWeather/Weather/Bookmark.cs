using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather
{

    public class BookmarkData
    {
        public string CityName { get; set; }
    }
    class Bookmark
    {
        private HashSet<BookmarkData> bookmarkSet { get; set; }
        private readonly string bookmarkPath = Path.GetFullPath(@"..\..\") + @"resources\bookmark.txt";

        public Bookmark()
        {
            bookmarkSet = new HashSet<BookmarkData>();
        }

        public HashSet<BookmarkData> getHistory()
        {
            bookmarkSet.Clear();

            if (File.Exists(bookmarkPath))
            {
                string[] searchedPlaces = File.ReadAllLines(bookmarkPath);

                foreach (string place in searchedPlaces)
                {
                    if (place != "")
                    {
                        BookmarkData bookmarkData = new BookmarkData();
                        bookmarkData.CityName = place;
                        bookmarkSet.Add(bookmarkData);
                    }
                }
            }

            return bookmarkSet;
        }
    }
}
