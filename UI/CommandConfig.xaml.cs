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
using WindowsInput.Native;

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
                this.CommandItem = new NullCommand();
            }
            else if (this.CommandType.SelectedIndex == 1)
            {
                // Keyboard Command
                // TODO: change keyboard command to whatever the user chooses.
                VirtualKeyCode enumVal = (VirtualKeyCode)Enum.Parse(typeof(VirtualKeyCode), KeyComboBox.SelectedValue.ToString());
                this.CommandItem = new KeyboardCommand(enumVal);
            }
            this.DialogResult = true;

        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.KeyComboBox.ItemsSource = Enum.GetValues(typeof(WindowsInput.Native.VirtualKeyCode)).Cast<WindowsInput.Native.VirtualKeyCode>();
            if (CommandItem is KeyboardCommand)
            {
                this.CommandType.SelectedIndex = 1;
                this.KeyComboBox.SelectedValue = (this.CommandItem as KeyboardCommand).KeyCode;
            }
            else
            {
                this.CommandType.SelectedIndex = 0;
            }
        }
    }
}
