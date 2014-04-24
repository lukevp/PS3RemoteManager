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

namespace PS3RemoteManager.UI
{
    /// <summary>
    /// Interaction logic for CommandConfig.xaml
    /// </summary>
    public partial class CommandConfig : Window
    {
        public PS3Command CommandItem;
        public CommandConfig()
        {
            InitializeComponent();
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            if (this.CommandType.SelectedIndex == 0)
            {
                // None command
                this.CommandItem = new NullCommand(this.CommandItem.ButtonName);
            }
            else if (this.CommandType.SelectedIndex == 1)
            {
                // Keyboard Command
                this.CommandItem = new KeyboardCommand(this.CommandItem.ButtonName, WindowsInput.Native.VirtualKeyCode.ADD);
            }
            this.DialogResult = true;

            //this
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (CommandItem is KeyboardCommand)
            {
                this.CommandType.SelectedIndex = 1;
            }
            else
            {
                this.CommandType.SelectedIndex = 0;
            }
        }
    }
}
