using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace PS3RemoteManager
{

    public class PS3Command
    {
        // Use an enumerator to determine command type instead of subclasses because subclassing makes deserialization from JSON nasty
        public enum Types { NULL, KEYBOARD, PROGRAM };
        public Types Type;
        public string ButtonName { get; set; }
        public string 

        /*keyboard commands */
        public VirtualKeyCode KeyCode;
        public int? KeyRepeat;
        public int InitialWait;


        public PS3Command(string buttonName, VirtualKeyCode code, int? keyRepeat = null, int initialWait=500) // keyboard command
        {
            this.Type = Types.KEYBOARD;
            this.ButtonName = buttonName;
            this.KeyCode = code;
            this.InitialWait = initialWait;
            this.KeyRepeat = keyRepeat;
        }

        public PS3Command(string buttonName) // nothing specified so make a null type
        {
            this.Type = Types.NULL;
            this.ButtonName = buttonName;
        }

        public PS3Command(string buttonName, string programPath, string args)

        public void ButtonPress(App app, string buttonName)
        {
            if (this.Type == Types.KEYBOARD)
            {
                app.vKeyboard.Keyboard.KeyPress(this.KeyCode);
            }
            else if (this.Type == Types.NULL)
            {
                app.Log.Write("Button Pressed with no Config Setting: " + buttonName);
            }
        }

        public string Description
        {
            get
            {
                if (this.Type == Types.KEYBOARD)
                    return String.Format("Key: {0}", this.KeyCode);
                return "";
            }
        }
    }


    public class ProgramCommand : PS3Command
    {
        public ProgramCommand(string buttonName, string progPath=null)
        {
            this.ButtonName = buttonName;
            this.ProgPath = progPath;
        }
        public string ProgPath { get; set; }

        public override void ButtonPress(App app, string buttonName)
        {
            app.Log.Write("Button Pressed with no Config Setting: " + buttonName);
        }
        public override string Description
        {
            get { return "Program: " + ProgPath; }
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

        [JsonIgnore]
        public List<string> ValidButtons = new List<string>() { "Num_1", "Num_2", "Num_3", "Num_4", "Num_5", "Num_6", "Num_7", "Num_8", "Num_9", "Num_0", "Dash_Slash_Dash_Dash", "Return", "Clear", "Channel_Up", "Channel_Down", "Eject", "Top_Menu", "Time", "Prev", "Next", "Play", "Scan_Back", "Scan_Forward", "Stop", "Pause", "PopUp_Menu", "Step_Back", "Step_Forward", "Subtitle", "Audio", "Angle", "Display", "Instant_Forward", "Instant_Back", "Blue", "Red", "Green", "Yellow", "Playstation", "Enter", "L2", "R2", "L1", "R1", "Triangle", "Circle", "Cross", "Square", "Select", "L3", "R3", "Start", "Arrow_Up", "Arrow_Right", "Arrow_Down", "Arrow_Left" };

        public ObservableCollection<PS3Command> Commands { get; set; }

        public void UpdateCommand(PS3Command command)
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i].ButtonName == command.ButtonName)
                {
                    Commands[i] = command;
                }
            }
        }

        public PS3Command GetCommand(string name)
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                if (Commands[i].ButtonName == name)
                {
                    return Commands[i];
                }
            }
            return null;
        }

        public void SaveConfigModel()
        {
            // TODO: ask where to save.
            File.WriteAllText("ButtonConfig.json", JsonConvert.SerializeObject(this));
        }

        public ButtonConfigModel()
        {
            Commands = new ObservableCollection<PS3Command>();
            Commands.Add(new KeyboardCommand("Num_1", VirtualKeyCode.NUMPAD1));
            Commands.Add(new KeyboardCommand("Num_2", VirtualKeyCode.NUMPAD2));
            Commands.Add(new KeyboardCommand("Num_3", VirtualKeyCode.NUMPAD3));
            Commands.Add(new KeyboardCommand("Num_4", VirtualKeyCode.NUMPAD4));
            Commands.Add(new KeyboardCommand("Num_5", VirtualKeyCode.NUMPAD5));
            Commands.Add(new KeyboardCommand("Num_6", VirtualKeyCode.NUMPAD6));
            Commands.Add(new KeyboardCommand("Num_7", VirtualKeyCode.NUMPAD7));
            Commands.Add(new KeyboardCommand("Num_8", VirtualKeyCode.NUMPAD8));
            Commands.Add(new KeyboardCommand("Num_9", VirtualKeyCode.NUMPAD9));
            Commands.Add(new KeyboardCommand("Num_0", VirtualKeyCode.NUMPAD0));
            Commands.Add(new KeyboardCommand("Dash_Slash_Dash_Dash", VirtualKeyCode.VOLUME_MUTE));
            Commands.Add(new KeyboardCommand("Return", VirtualKeyCode.ESCAPE));
            Commands.Add(new KeyboardCommand("Clear", VirtualKeyCode.DELETE));
            Commands.Add(new KeyboardCommand("Channel_Up", VirtualKeyCode.VOLUME_UP, 50));
            Commands.Add(new KeyboardCommand("Channel_Down", VirtualKeyCode.VOLUME_DOWN, 50));
            Commands.Add(new KeyboardCommand("Eject", VirtualKeyCode.VK_E));
            Commands.Add(new KeyboardCommand("Top_Menu", VirtualKeyCode.VK_S));
            Commands.Add(new KeyboardCommand("Time", VirtualKeyCode.OEM_3));
            Commands.Add(new KeyboardCommand("Prev", VirtualKeyCode.PRIOR));
            Commands.Add(new KeyboardCommand("Next", VirtualKeyCode.NEXT));
            Commands.Add(new KeyboardCommand("Play", VirtualKeyCode.PLAY));
            Commands.Add(new KeyboardCommand("Scan_Back", VirtualKeyCode.VK_R));
            Commands.Add(new KeyboardCommand("Scan_Forward", VirtualKeyCode.VK_F));
            Commands.Add(new KeyboardCommand("Stop", VirtualKeyCode.MEDIA_STOP));
            Commands.Add(new KeyboardCommand("Pause", VirtualKeyCode.PAUSE));
            Commands.Add(new KeyboardCommand("PopUp_Menu", VirtualKeyCode.VK_M));
            Commands.Add(new KeyboardCommand("Step_Back", VirtualKeyCode.OEM_COMMA));
            Commands.Add(new KeyboardCommand("Step_Forward", VirtualKeyCode.OEM_PERIOD));
            Commands.Add(new KeyboardCommand("Subtitle", VirtualKeyCode.VK_T));
            Commands.Add(new KeyboardCommand("Audio", VirtualKeyCode.VK_A));
            Commands.Add(new KeyboardCommand("Angle", VirtualKeyCode.VK_Z));
	        Commands.Add(new KeyboardCommand("Display", VirtualKeyCode.VK_I));
            Commands.Add(new NullCommand("Instant_Forward"));
            Commands.Add(new NullCommand("Instant_Back"));
            Commands.Add(new KeyboardCommand("Blue", VirtualKeyCode.F9));
            Commands.Add(new KeyboardCommand("Red", VirtualKeyCode.F7));
            Commands.Add(new KeyboardCommand("Green", VirtualKeyCode.F8));
            Commands.Add(new KeyboardCommand("Yellow", VirtualKeyCode.F10));
            Commands.Add(new NullCommand("Playstation"));
            Commands.Add(new KeyboardCommand("Enter", VirtualKeyCode.RETURN));
            Commands.Add(new KeyboardCommand("L2", VirtualKeyCode.F2));
            Commands.Add(new KeyboardCommand("R2", VirtualKeyCode.F5));
            Commands.Add(new KeyboardCommand("L1", VirtualKeyCode.F1));
            Commands.Add(new KeyboardCommand("R1", VirtualKeyCode.F4));
            Commands.Add(new KeyboardCommand("Triangle", VirtualKeyCode.VK_C));
            Commands.Add(new KeyboardCommand("Circle", VirtualKeyCode.BACK));
            Commands.Add(new KeyboardCommand("Cross", VirtualKeyCode.SPACE));
            Commands.Add(new KeyboardCommand("Square", VirtualKeyCode.TAB));
            Commands.Add(new KeyboardCommand("Select", VirtualKeyCode.INSERT));
            Commands.Add(new KeyboardCommand("L3", VirtualKeyCode.F3));
            Commands.Add(new KeyboardCommand("R3", VirtualKeyCode.F6));
            Commands.Add(new KeyboardCommand("Start", VirtualKeyCode.HOME));
            Commands.Add(new KeyboardCommand("Arrow_Up", VirtualKeyCode.UP, 100, 1000));
            Commands.Add(new KeyboardCommand("Arrow_Right", VirtualKeyCode.RIGHT, 100, 1000));
            Commands.Add(new KeyboardCommand("Arrow_Down", VirtualKeyCode.DOWN, 100, 1000));
            Commands.Add(new KeyboardCommand("Arrow_Left", VirtualKeyCode.LEFT, 100, 1000));
        }
            
    }
}
