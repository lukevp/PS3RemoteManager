using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PS3RemoteManager
{


    public class LogMessage
    {
        public DebugLevel LogLevel;

        public string NiceTime
        {
            get
            {
                return _time.ToString("T", CultureInfo.CreateSpecificCulture("en-us"));
            }
        }
        private DateTime _time;
        public DateTime Time { get { return _time; } }
        public string Message { get; set;}

        public LogMessage(string message, DebugLevel loglevel=DebugLevel.LOG)
        {
            this.Message = message;
            this.LogLevel = loglevel;
            this._time = DateTime.Now;
        }
    }

    public enum DebugLevel
    {
        LOG,
        BALLOON
    }

    public class DebugLog
    {
        private App currentApp = null;

        private ObservableCollection<LogMessage> _logMessages = new ObservableCollection<LogMessage>();
        public ObservableCollection<LogMessage> LogMessages { get { return _logMessages; } }
        

        public void Write(string message, DebugLevel level = DebugLevel.LOG)
        {
            this.Write(new LogMessage(message, level));
        }
        public void Write(LogMessage message)
        {
            if (message.LogLevel == DebugLevel.BALLOON)
            {
                this.currentApp = App.Current as App;
                if (currentApp != null)
                    currentApp.NotifyIcon.ShowBalloonTip("PS3RemoteManager", message.Message, BalloonIcon.Info);
            }
            this.LogMessages.Add(message);
        }

        /*

        public static void write(LogMessage message)
        {

            _log.Add("[" + DateTime.Now.ToString() + "] :  " + line + ".");
            outputToFile("Log.txt");
        }

        public static void outputToFile(string filename)
        {
            StreamWriter file = null;

            try
            {
                file = new StreamWriter(filename, true, Encoding.UTF8);

                foreach (string line in _log)
                {
                    file.WriteLine(line);
                }
            }
            catch
            { }
            finally
            {
                if (file != null) file.Close();
            }
        }*/

    }
}
