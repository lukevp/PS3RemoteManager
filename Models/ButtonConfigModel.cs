using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace PS3RemoteManager
{

    public abstract class PS3Command
    {
        public abstract void ButtonPress(App app, string buttonName);
        public abstract string Description { get; }
    }

    public class KeyboardCommand : PS3Command
    {
        public VirtualKeyCode KeyCode;
        public int? KeyRepeat;
        public int InitialWait;

        public KeyboardCommand(VirtualKeyCode code, int? keyRepeat = null, int initialWait = 500)
        {
            this.KeyCode = code;
            this.InitialWait = initialWait;
            this.KeyRepeat = keyRepeat;
        }
        public override void ButtonPress(App app, string buttonName)
        {
            app.vKeyboard.Keyboard.KeyPress(this.KeyCode);
        }
        public override string Description
        {
            get
            {
                return String.Format("Key: {0}", this.KeyCode);
            }
        }
    }
    public class NullCommand : PS3Command
    {
        public override void ButtonPress(App app, string buttonName)
        {
            app.Log.Write("Button Pressed with no Config Setting: " + buttonName);
        }
        public override string Description
        {
            get { return ""; }
        }
    }
    public class CommandDescriptor
    {
        public string Description {get; set; }
        public string Name {get; set; }
        public CommandDescriptor(string name, string desc)
        {
            this.Name = name;
            this.Description = desc;
        }
    }

    public class ButtonConfigModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void RefreshView()
        {
            OnPropertyChanged("ButtonInfo");
        }

        public Dictionary<string, PS3Command> Commands = new Dictionary<string, PS3Command>();
        public List<string> ValidButtons = new List<string>() { "Num_1", "Num_2", "Num_3", "Num_4", "Num_5", "Num_6", "Num_7", "Num_8", "Num_9", "Num_0", "Dash_Slash_Dash_Dash", "Return", "Clear", "Channel_Up", "Channel_Down", "Eject", "Top_Menu", "Time", "Prev", "Next", "Play", "Scan_Back", "Scan_Forward", "Stop", "Pause", "PopUp_Menu", "Step_Back", "Step_Forward", "Subtitle", "Audio", "Angle", "Display", "Instant_Forward", "Instant_Back", "Blue", "Red", "Green", "Yellow", "Playstation", "Enter", "L2", "R2", "L1", "R1", "Triangle", "Circle", "Cross", "Square", "Select", "L3", "R3", "Start", "Arrow_Up", "Arrow_Right", "Arrow_Down", "Arrow_Left" };

        public List<CommandDescriptor> ButtonInfo
        {
            get
            {
                List<CommandDescriptor> temp = new List<CommandDescriptor>();
                foreach (var button in ValidButtons)
                {
                    temp.Add(new CommandDescriptor(button, Commands[button].Description));
                }
                return temp;
            }
        }

        public ButtonConfigModel()
        {
            Commands["Num_1"] = new KeyboardCommand(VirtualKeyCode.NUMPAD1);
            Commands["Num_2"] = new KeyboardCommand(VirtualKeyCode.NUMPAD2);
            Commands["Num_3"] = new KeyboardCommand(VirtualKeyCode.NUMPAD3);
            Commands["Num_4"] = new KeyboardCommand(VirtualKeyCode.NUMPAD4);
            Commands["Num_5"] = new KeyboardCommand(VirtualKeyCode.NUMPAD5);
            Commands["Num_6"] = new KeyboardCommand(VirtualKeyCode.NUMPAD6);
            Commands["Num_7"] = new KeyboardCommand(VirtualKeyCode.NUMPAD7);
            Commands["Num_8"] = new KeyboardCommand(VirtualKeyCode.NUMPAD8);
            Commands["Num_9"] = new KeyboardCommand(VirtualKeyCode.NUMPAD9);
            Commands["Num_0"] = new KeyboardCommand(VirtualKeyCode.NUMPAD0);
            Commands["Dash_Slash_Dash_Dash"] = new KeyboardCommand(VirtualKeyCode.VOLUME_MUTE);
            Commands["Return"] = new KeyboardCommand(VirtualKeyCode.ESCAPE);
            Commands["Clear"] = new KeyboardCommand(VirtualKeyCode.DELETE);
            Commands["Channel_Up"] = new KeyboardCommand(VirtualKeyCode.VOLUME_UP, 50);
            Commands["Channel_Down"] = new KeyboardCommand(VirtualKeyCode.VOLUME_DOWN, 50);
            Commands["Eject"] = new KeyboardCommand(VirtualKeyCode.VK_E);
            Commands["Top_Menu"] = new KeyboardCommand(VirtualKeyCode.VK_S);
            Commands["Time"] = new KeyboardCommand(VirtualKeyCode.OEM_3);
            Commands["Prev"] = new KeyboardCommand(VirtualKeyCode.PRIOR);
            Commands["Next"] = new KeyboardCommand(VirtualKeyCode.NEXT);
            Commands["Play"] = new KeyboardCommand(VirtualKeyCode.PLAY);
            Commands["Scan_Back"] = new KeyboardCommand(VirtualKeyCode.VK_R);
            Commands["Scan_Forward"] = new KeyboardCommand(VirtualKeyCode.VK_F);
            Commands["Stop"] = new KeyboardCommand(VirtualKeyCode.MEDIA_STOP);
            Commands["Pause"] = new KeyboardCommand(VirtualKeyCode.PAUSE);
            Commands["PopUp_Menu"] = new KeyboardCommand(VirtualKeyCode.VK_M);
            Commands["Step_Back"] = new KeyboardCommand(VirtualKeyCode.OEM_COMMA);
            Commands["Step_Forward"] = new KeyboardCommand(VirtualKeyCode.OEM_PERIOD);
            Commands["Subtitle"] = new KeyboardCommand(VirtualKeyCode.VK_T);
            Commands["Audio"] = new KeyboardCommand(VirtualKeyCode.VK_A);
            Commands["Angle"] = new KeyboardCommand(VirtualKeyCode.VK_Z);
	        Commands["Display"] = new KeyboardCommand(VirtualKeyCode.VK_I);
            Commands["Instant_Forward"] = new NullCommand();
            Commands["Instant_Back"] = new NullCommand();
            Commands["Blue"] = new KeyboardCommand(VirtualKeyCode.F9);
            Commands["Red"] = new KeyboardCommand(VirtualKeyCode.F7);
            Commands["Green"] = new KeyboardCommand(VirtualKeyCode.F8);
            Commands["Yellow"] = new KeyboardCommand(VirtualKeyCode.F10);
            Commands["Playstation"] = new NullCommand();
            Commands["Enter"] = new KeyboardCommand(VirtualKeyCode.RETURN);
            Commands["L2"] = new KeyboardCommand(VirtualKeyCode.F2);
            Commands["R2"] = new KeyboardCommand(VirtualKeyCode.F5);
            Commands["L1"] = new KeyboardCommand(VirtualKeyCode.F1);
            Commands["R1"] = new KeyboardCommand(VirtualKeyCode.F4);
            Commands["Triangle"] = new KeyboardCommand(VirtualKeyCode.VK_C);
            Commands["Circle"] = new KeyboardCommand(VirtualKeyCode.BACK);
            Commands["Cross"] = new KeyboardCommand(VirtualKeyCode.SPACE);
            Commands["Square"] = new KeyboardCommand(VirtualKeyCode.TAB);
            Commands["Select"] = new KeyboardCommand(VirtualKeyCode.INSERT);
            Commands["L3"] = new KeyboardCommand(VirtualKeyCode.F3);
            Commands["R3"] = new KeyboardCommand(VirtualKeyCode.F6);
            Commands["Start"] = new KeyboardCommand(VirtualKeyCode.HOME);
            Commands["Arrow_Up"] = new KeyboardCommand(VirtualKeyCode.UP, 100, 1000);
            Commands["Arrow_Right"] = new KeyboardCommand(VirtualKeyCode.RIGHT, 100, 1000);
            Commands["Arrow_Down"] = new KeyboardCommand(VirtualKeyCode.DOWN, 100, 1000);
            Commands["Arrow_Left"] = new KeyboardCommand(VirtualKeyCode.LEFT, 100, 1000);
        }
            
    }
}
