using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PS3RemoteManager
{
    /// <summary>
    /// Interaction logic for Settings
    public partial class Settings : Window
    {
        private App currentApp = null;

        public Settings()
        {
            currentApp = App.Current as App;
            this.DataContext = currentApp;
            currentApp.Log.LogChanged = logChanged;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.SettingsTabs.SelectedItem = this.TabStatus;
            currentApp.bw.RunWorkerAsync();
        }

        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            currentApp.Log.Write("Serializing settings to JSON file...");
            try
            {
                using (StreamWriter sw = new StreamWriter("settings.json"))
                {
                    sw.Write(JsonConvert.SerializeObject(currentApp.SettingsVM));
                }
            }
            catch
            {
                currentApp.Log.Write("Unable to serialize settings!");
                return;
            }
            currentApp.Log.Write("Settings saved to settings.json!");
        }


        private void logChanged()
        {
            if (this.StatusLog.Items.Count > 0)
            {
                if (VisualTreeHelper.GetChildrenCount(this.StatusLog) > 0)
                {
                    var border = VisualTreeHelper.GetChild(this.StatusLog, 0) as Decorator;
                    if (border != null)
                    {
                        var scroll = border.Child as ScrollViewer;
                        if (scroll != null) scroll.ScrollToEnd();
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            currentApp.Log.LogChanged = null;
        }
    }

}
