using Hardcodet.Wpf.TaskbarNotification;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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

        // for keyrepeat
        public DispatcherTimer timerRepeat = new DispatcherTimer();

        // The tray icon
        public TaskbarIcon NotifyIcon;

        // Class for hibernating remote
        public RemoteSleep RemoteSleep;

        // Logging to file and to the UI
        public DebugLog Log { get; set; }

        public SettingsModel SettingsVM { get; set; }


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
                Log.Write(new LogMessage(String.Format("PS3 Bluetooth Remote Battery is currently at {0}%.", batteryLife), DebugLevel.BALLOON));
            }
        }
        private PS3Command heldButton = null;
        private string heldButtonName = null;

        private Icon iconDisconnected = new Icon("Resources/Icon Disconnected.ico");
        private Icon iconConnected = new Icon("Resources/Icon Connected.ico");

        private void remoteConnected()
        {
            Log.Write(new LogMessage("PS3 Bluetooth Remote Connected!", DebugLevel.BALLOON));
            NotifyIcon.Icon = iconConnected;
            NotifyIcon.ToolTipText = "Remote Connected";
        }
        private void remoteDisconnected()
        {
            Log.Write(new LogMessage("PS3 Bluetooth Remote Disconnected!", DebugLevel.BALLOON));
            NotifyIcon.Icon = iconDisconnected;
            NotifyIcon.ToolTipText = "Remote Disconnected";
        }
        private void buttonDown(PS3Remote.Button b)
        {
            // todo: guard against issues w/ this
            PS3Command command = this.SettingsVM.ActiveConfig.GetCommand(b.Name);
            if (command != null)
            {
                command.ButtonPress(this, b.Name);
                if (command.Type == CmdType.KEYBOARD)
                {
                    if (command.KeyRepeat != null)
                    {
                        heldButton = command;
                        heldButtonName = b.Name;
                        timerRepeat.Interval = TimeSpan.FromMilliseconds(command.InitialWait); // gated repeat - first repeat is minimum of 750 ms to prevent unnecessary repeats.
                        timerRepeat.Start();
                    }
                }
            }
        }
        private void buttonUp(PS3Remote.Button b)
        {
            heldButton = null;
            timerRepeat.Stop();
        }


        
        private void TimerRepeat_Elapsed(object sender, EventArgs e)
        {
            heldButton.ButtonPress(this, heldButtonName);
            timerRepeat.Interval = TimeSpan.FromMilliseconds(Convert.ToDouble(heldButton.KeyRepeat));
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
            // setup repeat timer
            timerRepeat.Tick += new EventHandler(TimerRepeat_Elapsed);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml)
            NotifyIcon = (TaskbarIcon)FindResource("NotifyIcon");

            NotifyIcon.Icon = iconDisconnected;
            NotifyIcon.ToolTipText = "Searching for Remote...";

            Log = new DebugLog();
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

            /*
            using (StreamReader sr = new StreamReader("settings.json"))
            {
                SettingsVM = JsonConvert.DeserializeObject<SettingsModel>(sr.ReadToEnd());
            }
            */
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
            Utils.SaveSettings(this);
            base.OnExit(e);
        }

    }
}
