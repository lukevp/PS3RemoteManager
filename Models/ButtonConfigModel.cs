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
    public enum CmdType { None, Keyboard, Program };

    public class PS3Command : INotifyPropertyChanged
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
        public string ButtonName { get; set; }
        //public string 
        // TODO: maybe check the CmdType enumerator instead of checking this.
        public bool IsKeyCommand
        {
            get
            { return Type == CmdType.Keyboard; }
        } 
        public bool IsProgramCommand
        {
            get
            { return Type == CmdType.Program; }
        }

        private CmdType _type;
        public CmdType Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
                OnPropertyChanged("Type");
                OnPropertyChanged("Description");
                OnPropertyChanged("IsKeyCommand");
                OnPropertyChanged("IsProgramCommand");
            }
        }
        

        /*keyboard commands */
        private VirtualKeyCode _keyCode;
        public VirtualKeyCode KeyCode { 
            get
            {
                return _keyCode;
            }
            set
            {
                _keyCode = value;
                OnPropertyChanged("KeyCode");
                OnPropertyChanged("Description");
            }
        }
        public int? KeyRepeat;
        public int InitialWait;

        public string ProgramPath { get; set; }

        public PS3Command(string buttonName, VirtualKeyCode code, int? keyRepeat = null, int initialWait=500) // keyboard command
        {
            this.Type = CmdType.Keyboard;
            this.ButtonName = buttonName;
            this.KeyCode = code;
            this.InitialWait = initialWait;
            this.KeyRepeat = keyRepeat;
        }

        public PS3Command(string buttonName) // nothing specified so make a null type
        {
            this.Type = CmdType.None;
            this.ButtonName = buttonName;
        }

        //public PS3Command(string buttonName, string programPath, string args)

        public void ButtonPress(App app, string buttonName)
        {
            if (this.Type == CmdType.Keyboard)
            {
                app.vKeyboard.Keyboard.KeyPress(this.KeyCode);
            }
            else if (this.Type == CmdType.None)
            {
                app.Log.Write("Button Pressed with no Config Setting: " + buttonName);
            }
        }

        public string Description
        {
            get
            {
                if (this.Type == CmdType.Keyboard)
                    return String.Format("Key: {0}", this.KeyCode);
                return "";
            }
        }
    }

    /*
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
    }*/


    public class ButtonConfigModel
    {

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
        /*
        public void SaveConfigModel()
        {
            // TODO: ask where to save.
            File.WriteAllText("ButtonConfig.json", JsonConvert.SerializeObject(this));
        }*/

        public ButtonConfigModel()
        {
            Commands = new ObservableCollection<PS3Command>();
            Commands.Add(new PS3Command("Num_1", VirtualKeyCode.NUMPAD1));
            Commands.Add(new PS3Command("Num_2", VirtualKeyCode.NUMPAD2));
            Commands.Add(new PS3Command("Num_3", VirtualKeyCode.NUMPAD3));
            Commands.Add(new PS3Command("Num_4", VirtualKeyCode.NUMPAD4));
            Commands.Add(new PS3Command("Num_5", VirtualKeyCode.NUMPAD5));
            Commands.Add(new PS3Command("Num_6", VirtualKeyCode.NUMPAD6));
            Commands.Add(new PS3Command("Num_7", VirtualKeyCode.NUMPAD7));
            Commands.Add(new PS3Command("Num_8", VirtualKeyCode.NUMPAD8));
            Commands.Add(new PS3Command("Num_9", VirtualKeyCode.NUMPAD9));
            Commands.Add(new PS3Command("Num_0", VirtualKeyCode.NUMPAD0));
            Commands.Add(new PS3Command("Dash_Slash_Dash_Dash", VirtualKeyCode.VOLUME_MUTE));
            Commands.Add(new PS3Command("Return", VirtualKeyCode.ESCAPE));
            Commands.Add(new PS3Command("Clear", VirtualKeyCode.DELETE));
            Commands.Add(new PS3Command("Channel_Up", VirtualKeyCode.VOLUME_UP, 50));
            Commands.Add(new PS3Command("Channel_Down", VirtualKeyCode.VOLUME_DOWN, 50));
            Commands.Add(new PS3Command("Eject", VirtualKeyCode.VK_E));
            Commands.Add(new PS3Command("Top_Menu", VirtualKeyCode.VK_S));
            Commands.Add(new PS3Command("Time", VirtualKeyCode.OEM_3));
            Commands.Add(new PS3Command("Prev", VirtualKeyCode.PRIOR));
            Commands.Add(new PS3Command("Next", VirtualKeyCode.NEXT));
            Commands.Add(new PS3Command("Play", VirtualKeyCode.PLAY));
            Commands.Add(new PS3Command("Scan_Back", VirtualKeyCode.VK_R));
            Commands.Add(new PS3Command("Scan_Forward", VirtualKeyCode.VK_F));
            Commands.Add(new PS3Command("Stop", VirtualKeyCode.MEDIA_STOP));
            Commands.Add(new PS3Command("Pause", VirtualKeyCode.PAUSE));
            Commands.Add(new PS3Command("PopUp_Menu", VirtualKeyCode.VK_M));
            Commands.Add(new PS3Command("Step_Back", VirtualKeyCode.OEM_COMMA));
            Commands.Add(new PS3Command("Step_Forward", VirtualKeyCode.OEM_PERIOD));
            Commands.Add(new PS3Command("Subtitle", VirtualKeyCode.VK_T));
            Commands.Add(new PS3Command("Audio", VirtualKeyCode.VK_A));
            Commands.Add(new PS3Command("Angle", VirtualKeyCode.VK_Z));
	        Commands.Add(new PS3Command("Display", VirtualKeyCode.VK_I));
            Commands.Add(new PS3Command("Instant_Forward"));
            Commands.Add(new PS3Command("Instant_Back"));
            Commands.Add(new PS3Command("Blue", VirtualKeyCode.F9));
            Commands.Add(new PS3Command("Red", VirtualKeyCode.F7));
            Commands.Add(new PS3Command("Green", VirtualKeyCode.F8));
            Commands.Add(new PS3Command("Yellow", VirtualKeyCode.F10));
            Commands.Add(new PS3Command("Playstation"));
            Commands.Add(new PS3Command("Enter", VirtualKeyCode.RETURN));
            Commands.Add(new PS3Command("L2", VirtualKeyCode.F2));
            Commands.Add(new PS3Command("R2", VirtualKeyCode.F5));
            Commands.Add(new PS3Command("L1", VirtualKeyCode.F1));
            Commands.Add(new PS3Command("R1", VirtualKeyCode.F4));
            Commands.Add(new PS3Command("Triangle", VirtualKeyCode.VK_C));
            Commands.Add(new PS3Command("Circle", VirtualKeyCode.BACK));
            Commands.Add(new PS3Command("Cross", VirtualKeyCode.SPACE));
            Commands.Add(new PS3Command("Square", VirtualKeyCode.TAB));
            Commands.Add(new PS3Command("Select", VirtualKeyCode.INSERT));
            Commands.Add(new PS3Command("L3", VirtualKeyCode.F3));
            Commands.Add(new PS3Command("R3", VirtualKeyCode.F6));
            Commands.Add(new PS3Command("Start", VirtualKeyCode.HOME));
            Commands.Add(new PS3Command("Arrow_Up", VirtualKeyCode.UP, 100, 1000));
            Commands.Add(new PS3Command("Arrow_Right", VirtualKeyCode.RIGHT, 100, 1000));
            Commands.Add(new PS3Command("Arrow_Down", VirtualKeyCode.DOWN, 100, 1000));
            Commands.Add(new PS3Command("Arrow_Left", VirtualKeyCode.LEFT, 100, 1000));
        }
            
    }
}
