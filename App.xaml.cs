using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WindowsInput;
using WindowsInput.Native;

namespace PS3RemoteManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // Used to prevent multiple starts
        Mutex m;

        // Used for the hibernate functionality
        public BackgroundWorker bw = new BackgroundWorker();

        // The tray icon
        public TaskbarIcon NotifyIcon;

        // Class for hibernating remote
        public RemoteSleep RemoteSleep;

        // Logging to file and to the UI
        private DebugLog _log = new DebugLog();
        public DebugLog Log { get { return _log; } set { _log = value; } }

        // Settings for the application
        private SettingsModel _settingsVM;
        public SettingsModel SettingsVM { get { return _settingsVM; } set { _settingsVM = value; } }


        public InputSimulator vKeyboard = new InputSimulator();

        //Remote Listener
        public PS3Remote Remote;
        private void batteryLifeChanged(int batteryLife)
        {
            if (batteryLife <= 40 && batteryLife > 20)
            {
                Log.Write(new LogMessage(String.Format("Battery Low.  Consider replacing batteries soon. battery is currently at {0}%.", batteryLife), DebugLevel.BALLOON));
            }
            else if (batteryLife <= 20)
            {
                Log.Write(new LogMessage(String.Format("BATTERY CRITICALLY LOW!!! Consider replacing batteries soon. battery is currently at {0}%.", batteryLife), DebugLevel.BALLOON));
            }
            else
            {
                Log.Write(new LogMessage(String.Format("PS3 Bluetooth Remote Battery is now at {0}%.", batteryLife), DebugLevel.BALLOON));
            }
        }

        private void remoteConnected()
        {
            Log.Write(new LogMessage("PS3 Bluetooth Remote Connected!", DebugLevel.BALLOON));
        }
        private void remoteDisconnected()
        {
            Log.Write(new LogMessage("PS3 Bluetooth Remote Disconnected!", DebugLevel.BALLOON));
        }
        private void buttonDown(PS3Remote.Button b)
        {
            Log.Write(new LogMessage("Button Pressed: "+ b.Name));
            if (this.SettingsVM.Commands.ContainsKey(b.Name))
            {
                var command = this.SettingsVM.Commands[b.Name];
                if (command.Type == CommandType.KEYBOARD)
                {
                    command.Exec(this);
                }
            }

        }
        private void buttonUp(PS3Remote.Button b)
        {
            Log.Write(new LogMessage("Button Released: " + b.Name));
        }

        protected override void OnStartup(StartupEventArgs e)
        {

            base.OnStartup(e);
            // Check if it is a new instance or not
            bool isnew;
            // use a global mutex with a GUID to identify our app against other processes.
            m = new Mutex(true, "6e9f1aff-f53e-4ee0-ad7b-24a35b3def60", out isnew);
            if (!isnew)
            {
                MessageBox.Show("The application is already running. You cannot run more than one instance.",
                             "Message", MessageBoxButton.OK, MessageBoxImage.Warning);

                Environment.Exit(0);
            }
            
            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
            NotifyIcon = (TaskbarIcon)FindResource("NotifyIcon");


            Log.Write(new LogMessage("Searching for PS3 Bluetooth Remote...", DebugLevel.BALLOON));
            Remote = new PS3Remote();
            Remote.BatteryLifeChanged = new PS3Remote.batteryDelegate(this.batteryLifeChanged);
            Remote.Connected = new PS3Remote.eventDelegate(this.remoteConnected);
            Remote.Disconnected = new PS3Remote.eventDelegate(this.remoteDisconnected);
            Remote.ButtonDown = new PS3Remote.buttonDelegate(this.buttonDown);
            Remote.ButtonUp = new PS3Remote.buttonDelegate(this.buttonUp);

            Remote.Connect();
            RemoteSleep = new RemoteSleep(this.Log);

            // setup hibernate backgroundworker
            bw.WorkerSupportsCancellation = false;
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(RemoteSleep.HibernateRemoteWorker);
            bw.ProgressChanged += new ProgressChangedEventHandler(RemoteSleep.HibernateRemoteWorkerProgressChanged);

            try
            {
                using (StreamReader sr = new StreamReader("settings.json"))
                {
                    SettingsVM = JsonConvert.DeserializeObject<SettingsModel>(sr.ReadToEnd());
                }
            }
            catch
            {
                SettingsVM = new SettingsModel();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NotifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
