using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsInput.Native;

namespace PS3RemoteManager
{

    public class SettingsModel
    {
        public bool StartWithWindows { get; set; }
        public int MinsBeforeHibernate { get; set; }

        private ButtonConfigModel _activeConfig = new ButtonConfigModel();
        public ButtonConfigModel ActiveConfig { get { return _activeConfig; } set { _activeConfig = value; } }

        public SettingsModel()
        {
            StartWithWindows = false;
            MinsBeforeHibernate = 10;
            ActiveConfig = new ButtonConfigModel();
        }


    }
}
