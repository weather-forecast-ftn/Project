using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : Window
    {
        public ObservableCollection<HistoryData> HistoryList
        {
            get;
            set;
        }
        public HistoryWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            History history = new History();
            HistoryList = new ObservableCollection<HistoryData>();
            foreach (HistoryData hd in history.getHistory().Reverse())
            {
                HistoryList.Add(hd);
            }

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win2 = new MainWindow();
            win2.Show();
            this.Close();
        }
    }
}
