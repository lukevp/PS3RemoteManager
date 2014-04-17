using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace PS3RemoteManager
{
    public enum CommandType { KEYBOARD, MOUSE, PROGRAM, NULLCOMMAND }

    public abstract class PS3Command
    {
        public abstract bool Exec(App app);
        public CommandType Type;
        public string ButtonName {get; set; }
    }

    public class KeyboardCommand : PS3Command
    {
        public VirtualKeyCode KeyCode;
        public KeyboardCommand(string buttonName, VirtualKeyCode code)
        {
            this.ButtonName = buttonName;
            this.Type = CommandType.KEYBOARD;
            this.KeyCode = code;
        }
        public override bool Exec(App app)
        {
            app.vKeyboard.Keyboard.KeyPress(this.KeyCode);
            return true;
        }
    }
    public class NullCommand : PS3Command
    {
        public override bool Exec(App app)
        {
            app.Log.Write("Button Pressed with no Config Setting: " + this.ButtonName);
            return true;
        }
        public NullCommand(string buttonName)
        {
            this.ButtonName = buttonName;
            this.Type = CommandType.NULLCOMMAND;
        }
    }
    public class ButtonConfigModel
    {
        public ObservableCollection<PS3Command> Commands { get; set; }
            
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
            Commands.Add(new KeyboardCommand("Channel_Up", VirtualKeyCode.VOLUME_UP));
            Commands.Add(new KeyboardCommand("Channel_Down", VirtualKeyCode.VOLUME_DOWN));
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
            Commands.Add(new KeyboardCommand("Arrow_Up", VirtualKeyCode.UP));
            Commands.Add(new KeyboardCommand("Arrow_Right", VirtualKeyCode.RIGHT));
            Commands.Add(new KeyboardCommand("Arrow_Down", VirtualKeyCode.DOWN));
            Commands.Add(new KeyboardCommand("Arrow_Left", VirtualKeyCode.LEFT));
        }
            
    }
}
