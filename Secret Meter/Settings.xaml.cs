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

namespace Secret_Meter
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Prefs Result;

        public Settings(Prefs p)
        {
            InitializeComponent();
            DataContext = p;
            Result = p;
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            Result.AlwaysOnTop = aot.IsChecked.Value;
            Result.MuteSound = ms.IsChecked.Value;
            Result.Range.startDate = from.SelectedDate;
            Result.Range.endDate = to.SelectedDate;
            Close();
        }
    }
}
