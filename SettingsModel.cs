using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3RemoteManager
{
    public class SettingsModel
    {
        public bool StartWithWindows { get; set; }
        public int MinsBeforeHibernate { get; set; }
        public int RemoteSearchInterval { get; set; }
        
        public SettingsModel()
        {
            StartWithWindows = false;
            MinsBeforeHibernate = 10;
            RemoteSearchInterval = 500;
        }


    }
}
