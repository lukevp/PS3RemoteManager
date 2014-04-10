using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace PS3RemoteManager
{
    public enum CommandType { KEYBOARD, MOUSE, PROGRAM }

    public abstract class PS3Command
    {
        public abstract bool Exec(App app);
        public CommandType Type;
    }

    public class KeyboardCommand : PS3Command
    {
        public VirtualKeyCode KeyCode;
        public KeyboardCommand(CommandType type, VirtualKeyCode code)
        {
            this.Type = type;
            this.KeyCode = code;
        }
        public override bool Exec(App app)
        {
            app.vKeyboard.Keyboard.KeyPress(this.KeyCode);
            return true;
        }

    }

    public class SettingsModel
    {
        public bool StartWithWindows { get; set; }
        public int MinsBeforeHibernate { get; set; }
        public Dictionary<string, PS3Command> Commands = new Dictionary<string, PS3Command>();
        
        public SettingsModel()
        {
            StartWithWindows = false;
            MinsBeforeHibernate = 10;
            Commands.Add("Arrow_Up", new KeyboardCommand(CommandType.KEYBOARD, VirtualKeyCode.UP));
            Commands.Add("Arrow_Right", new KeyboardCommand(CommandType.KEYBOARD, VirtualKeyCode.RIGHT));
            Commands.Add("Arrow_Down", new KeyboardCommand(CommandType.KEYBOARD, VirtualKeyCode.DOWN));
            Commands.Add("Arrow_Left", new KeyboardCommand(CommandType.KEYBOARD, VirtualKeyCode.LEFT));
        }


    }
}
