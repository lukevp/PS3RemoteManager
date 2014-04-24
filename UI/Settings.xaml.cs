using Newtonsoft.Json;
using PS3RemoteManager.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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

        private void Configuration_DoubleClick(object sender,
                                  System.Windows.Input.MouseButtonEventArgs e)
        {
            IInputElement element = e.MouseDevice.DirectlyOver;
            if (element != null && element is FrameworkElement)
            {
                if (((FrameworkElement)element).Parent is DataGridCell)
                {
                    var grid = sender as DataGrid;
                    if (grid != null && grid.SelectedItems != null
                        && grid.SelectedItems.Count == 1)
                    {
                        var rowItem = grid.SelectedItem as CommandDescriptor;
                        CommandConfig newWin = new CommandConfig();
                        newWin.CommandItem = currentApp.SettingsVM.ActiveConfig.Commands[rowItem.Name];
                        var result = newWin.ShowDialog();
                        if (result.HasValue && result.Value)
                        {
                            currentApp.SettingsVM.ActiveConfig.Commands[(grid.SelectedItem as CommandDescriptor).Name] = newWin.CommandItem;
                            currentApp.SettingsVM.ActiveConfig.RefreshView();
                        }
                    }
                }
            }
        }
    }

}
