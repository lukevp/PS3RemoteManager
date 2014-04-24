using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ButtonConfigModel
    {

        private Dictionary<string, PS3Command> commandLookup = new Dictionary<string, PS3Command>();

        public void SetCommand(string name, PS3Command command)
        {
            this.commandLookup[name] = command;
        }

        public ButtonConfigModel()
        {
            SetCommand("Num_1", new KeyboardCommand(VirtualKeyCode.NUMPAD1));
            SetCommand("Num_2", new KeyboardCommand(VirtualKeyCode.NUMPAD2));
            SetCommand("Num_3", new KeyboardCommand(VirtualKeyCode.NUMPAD3));
            SetCommand("Num_4", new KeyboardCommand(VirtualKeyCode.NUMPAD4));
            SetCommand("Num_5", new KeyboardCommand(VirtualKeyCode.NUMPAD5));
            SetCommand("Num_6", new KeyboardCommand(VirtualKeyCode.NUMPAD6));
            SetCommand("Num_7", new KeyboardCommand(VirtualKeyCode.NUMPAD7));
            SetCommand("Num_8", new KeyboardCommand(VirtualKeyCode.NUMPAD8));
            SetCommand("Num_9", new KeyboardCommand(VirtualKeyCode.NUMPAD9));
            SetCommand("Num_0", new KeyboardCommand(VirtualKeyCode.NUMPAD0));
            SetCommand("Dash_Slash_Dash_Dash", new KeyboardCommand(VirtualKeyCode.VOLUME_MUTE));
            SetCommand("Return", new KeyboardCommand(VirtualKeyCode.ESCAPE));
            SetCommand("Clear", new KeyboardCommand(VirtualKeyCode.DELETE));
            SetCommand("Channel_Up", new KeyboardCommand(VirtualKeyCode.VOLUME_UP, 50));
            SetCommand("Channel_Down", new KeyboardCommand(VirtualKeyCode.VOLUME_DOWN, 50));
            SetCommand("Eject", new KeyboardCommand(VirtualKeyCode.VK_E));
            SetCommand("Top_Menu", new KeyboardCommand(VirtualKeyCode.VK_S));
            SetCommand("Time", new KeyboardCommand(VirtualKeyCode.OEM_3));
            SetCommand("Prev", new KeyboardCommand(VirtualKeyCode.PRIOR));
            SetCommand("Next", new KeyboardCommand(VirtualKeyCode.NEXT));
            SetCommand("Play", new KeyboardCommand(VirtualKeyCode.PLAY));
            SetCommand("Scan_Back", new KeyboardCommand(VirtualKeyCode.VK_R));
            SetCommand("Scan_Forward", new KeyboardCommand(VirtualKeyCode.VK_F));
            SetCommand("Stop", new KeyboardCommand(VirtualKeyCode.MEDIA_STOP));
            SetCommand("Pause", new KeyboardCommand(VirtualKeyCode.PAUSE));
            SetCommand("PopUp_Menu", new KeyboardCommand(VirtualKeyCode.VK_M));
            SetCommand("Step_Back", new KeyboardCommand(VirtualKeyCode.OEM_COMMA));
            SetCommand("Step_Forward", new KeyboardCommand(VirtualKeyCode.OEM_PERIOD));
            SetCommand("Subtitle", new KeyboardCommand(VirtualKeyCode.VK_T));
            SetCommand("Audio", new KeyboardCommand(VirtualKeyCode.VK_A));
            SetCommand("Angle", new KeyboardCommand(VirtualKeyCode.VK_Z));
	        SetCommand("Display", new KeyboardCommand(VirtualKeyCode.VK_I));
            SetCommand("Instant_Forward", new NullCommand());
            SetCommand("Instant_Back", new NullCommand());
            SetCommand("Blue", new KeyboardCommand(VirtualKeyCode.F9));
            SetCommand("Red", new KeyboardCommand(VirtualKeyCode.F7));
            SetCommand("Green", new KeyboardCommand(VirtualKeyCode.F8));
            SetCommand("Yellow", new KeyboardCommand(VirtualKeyCode.F10));
            SetCommand("Playstation", new NullCommand());
            SetCommand("Enter", new KeyboardCommand(VirtualKeyCode.RETURN));
            SetCommand("L2", new KeyboardCommand(VirtualKeyCode.F2));
            SetCommand("R2", new KeyboardCommand(VirtualKeyCode.F5));
            SetCommand("L1", new KeyboardCommand(VirtualKeyCode.F1));
            SetCommand("R1", new KeyboardCommand(VirtualKeyCode.F4));
            SetCommand("Triangle", new KeyboardCommand(VirtualKeyCode.VK_C));
            SetCommand("Circle", new KeyboardCommand(VirtualKeyCode.BACK));
            SetCommand("Cross", new KeyboardCommand(VirtualKeyCode.SPACE));
            SetCommand("Square", new KeyboardCommand(VirtualKeyCode.TAB));
            SetCommand("Select", new KeyboardCommand(VirtualKeyCode.INSERT));
            SetCommand("L3", new KeyboardCommand(VirtualKeyCode.F3));
            SetCommand("R3", new KeyboardCommand(VirtualKeyCode.F6));
            SetCommand("Start", new KeyboardCommand(VirtualKeyCode.HOME));
            SetCommand("Arrow_Up", new KeyboardCommand(VirtualKeyCode.UP, 100, 1000));
            SetCommand("Arrow_Right", new KeyboardCommand(VirtualKeyCode.RIGHT, 100, 1000));
            SetCommand("Arrow_Down", new KeyboardCommand(VirtualKeyCode.DOWN, 100, 1000));
            SetCommand("Arrow_Left", new KeyboardCommand(VirtualKeyCode.LEFT, 100, 1000));
        }
            
    }
}
