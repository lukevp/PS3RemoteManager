﻿using Hardcodet.Wpf.TaskbarNotification;
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
        public SettingsModel SettingsModel;

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
            Log.Write(new LogMessage("Searching for PS3 Bluetooth Remote", DebugLevel.BALLOON));
            Log.Write(new LogMessage("PS3 Bluetooth Remote Status: Connected", DebugLevel.BALLOON));
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
                    SettingsModel = JsonConvert.DeserializeObject<SettingsModel>(sr.ReadToEnd());
                }
            }
            catch
            {
                SettingsModel = new SettingsModel();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NotifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
