using PropertyChanged;
using test;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Secret_Meter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Current { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Current = this;
            prefs = Prefs.LoadPrefs();
        }

        Divergence div;
        public Prefs prefs;
        UserActivityHook uah;
        Random r = new Random();
        double realignChance = 0.0125574;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Topmost = prefs.AlwaysOnTop;
            Top = prefs.PosY;
            Left = prefs.PosX;
            div = new Divergence();
            var d = dsds();

            uah = new UserActivityHook();
            uah.Start();
            uah.KeyDown += KeyboardActivity;
            uah.OnMouseActivity += MouseActivity;
            uah.KeyDown += KeyboardActivity;
        }

        private void MouseActivity(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Clicks >= 1 && r.NextDouble() < realignChance)
                realignTrigger = true;
        }

        private void KeyboardActivity(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter && r.NextDouble() < realignChance) || (e.Alt && e.KeyCode == Keys.F4 && r.NextDouble() < 0.2))
                realignTrigger = true;
        }

        bool realignTrigger = false;
        async Task dsds()
        {
            Random r = new Random();
            await Realign();
            for (;;)
            {
                numbers.ItemsSource = div.ToArray();
                await Task.Delay(150);
                if (realignTrigger)
                    await Realign();
            }
        }

        async Task Realign()
        {
            realignTrigger = true;
            var start = 9.9975f;
            var time = 25d;
            int it = 0;
            var mp = new MediaPlayer();
            if (!prefs.MuteSound)
            {
                mp.Open(new Uri(Directory.GetCurrentDirectory() + @"\realign.wav"));
                mp.Play();
            }
            await Task.Delay(500);
            for (;;)
            {
                var res = string.Format(Divergence._FORMAT, start * r.NextDouble()).ToCharArray();
                numbers.ItemsSource = res;
                await Task.Delay((int)time);
                time += r.NextDouble();
                if (it > r.Next(58, 86))
                    break;
                it++;
            }
            realignTrigger = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            uah.Stop();
            prefs.SavePrefs();
        }

        private void Window_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }

    public class Divergence
    {
        public Divergence()
        {
        }

        public static string _FORMAT = "{0:0.0000000}";
        public TimeSpan CurrentOffset { get { return (DateTime.Now - MainWindow.Current.prefs.Range.startDate); } }
        public float Value { get { return (float)CurrentOffset.TotalDays / (float)(MainWindow.Current.prefs.Range.endDate - MainWindow.Current.prefs.Range.startDate).TotalDays; } }
        public string FormattedValue { get { return string.Format(_FORMAT, Value); } }

        public char[] ToArray()
        {
            return FormattedValue.ToCharArray();
        }
    }
}
