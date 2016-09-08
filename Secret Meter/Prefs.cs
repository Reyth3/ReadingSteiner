using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Secret_Meter
{
    [Serializable]
    public class Prefs
    {
        static XmlSerializer xs = new XmlSerializer(typeof(Prefs));
        public Prefs() { }
        public bool AlwaysOnTop { get; set; }
        public double PosX { get; set; }
        public double PosY { get; set; }
        public bool MuteSound { get; set; }
        public TimeRange Range { get; set; }


        public static Prefs LoadPrefs()
        {
            var prefs = new Prefs();
            if (File.Exists(Directory.GetCurrentDirectory() + @"\config.xml"))
            {
                var xml = File.ReadAllBytes(Directory.GetCurrentDirectory() + @"\config.xml");
                var str = new MemoryStream();
                str.Write(xml, 0, xml.Length);
                str.Seek(0, SeekOrigin.Begin);
                prefs = xs.Deserialize(str) as Prefs;
            }
            else prefs = NewPrefs();
            return prefs;
        }

        private static Prefs NewPrefs()
        {
            MainWindow w = MainWindow.Current;
            Screen scr = Screen.AllScreens[0];
            var prefs = new Prefs();
            prefs.AlwaysOnTop = true;
            prefs.PosX = scr.Bounds.Width - w.Width;
            prefs.PosY = scr.Bounds.Height - w.Height - 64;
            prefs.MuteSound = false;
            prefs.Range = new TimeRange();
            prefs.Range.endDate = new DateTime(2010, 8, 13, 19, 0, 0);
            var xml = new MemoryStream();
            xs.Serialize(xml, prefs);
            File.WriteAllBytes(Directory.GetCurrentDirectory() + @"\config.xml", xml.ToArray());
            return prefs;
        }

        public void SavePrefs()
        {
            var w = MainWindow.Current;
            PosX = w.Left;
            PosY = w.Top;
            AlwaysOnTop = w.Topmost;
            var xml = new MemoryStream();
            xs.Serialize(xml, this);
            File.WriteAllBytes(Directory.GetCurrentDirectory() + @"\config.xml", xml.ToArray());
        }
    }

    [Serializable]
    public class TimeRange
    {
        [XmlAttribute()]
        public DateTime startDate { get; set; }
        [XmlAttribute()]
        public DateTime endDate { get; set; }
    }
}
