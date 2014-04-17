/*
 * 
 * 
Copyright (c) 2014 Luke Paireepinart





Copyright (c) 2011 Ben Barron

Permission is hereby granted, free of charge, to any person obtaining a copy 
of this software and associated documentation files (the "Software"), to deal 
in the Software without restriction, including without limitation the rights 
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
copies of the Software, and to permit persons to whom the Software is furnished 
to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all 
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using HidLibrary;

using System.Windows.Threading;

namespace PS3RemoteManager
{
    public class PS3Remote
    {
        public delegate void buttonDelegate(PS3Remote.Button b);
        public buttonDelegate ButtonDown = null;
        public buttonDelegate ButtonUp = null;

        public delegate void batteryDelegate(int level);
        public batteryDelegate BatteryLifeChanged = null;

        public delegate void eventDelegate();
        public eventDelegate Connected = null;
        public eventDelegate Disconnected = null;



        private HidDevice hidRemote = null;
        private DispatcherTimer timerFindRemote = new DispatcherTimer();
        private DispatcherTimer timerHibernate = new DispatcherTimer();
        public class DeviceInfo {
            public int VendorID;
            public int ProductID;
            public string Description;

            public DeviceInfo(int vid, int pid, string desc=null)
            {
                VendorID = vid;
                ProductID = pid;
                Description = desc;
            }
        }

        public List<DeviceInfo> Devices = new List<DeviceInfo>() 
        {
            new DeviceInfo(0x054c, 0x0306, "Official Sony Remote"),
            new DeviceInfo(0x0609, 0x0306, "SMK Remote"), // SMK
            new DeviceInfo(0x046d, 0x0306, "Logitech Harmony Remote") // SMK
        };
        public DeviceInfo ActiveDevice = null;

        private Button lastButton = null;
        private bool isButtonDown = false;

        private byte _batteryLife = 100;
        private App currentApp;


        #region Buttons Definition
        public class Button
        {
            private byte[] _buttonCodes = new byte[4];
            public string Name;

            public Button(string name, byte a=0, byte b=0, byte c=0, byte d=0)
            {
                Name = name;
                _buttonCodes[0] = a;
                _buttonCodes[1] = b;
                _buttonCodes[2] = c;
                _buttonCodes[3] = d;
            }
            public override bool Equals(object obj)
            {
                Button otherButton = (Button)obj;
                // compare on buttoncode, disregard name.
                return (_buttonCodes[0] == otherButton._buttonCodes[0] && _buttonCodes[1] == otherButton._buttonCodes[1] && _buttonCodes[2] == otherButton._buttonCodes[2] && _buttonCodes[3] == otherButton._buttonCodes[3]);
            }
            public override int GetHashCode()
            {
                return _buttonCodes[0]<<24+_buttonCodes[1]<<16+_buttonCodes[2]<<8+_buttonCodes[3];
            }
        }
        List<Button> Buttons = new List<Button>()
        {
            new Button("Num_1", 0, 0, 0, 0),
            new Button("Num_2", 0, 0, 0, 1),
            new Button("Num_3", 0, 0, 0, 2),
            new Button("Num_4", 0, 0, 0, 3),
            new Button("Num_5", 0, 0, 0, 4),
            new Button("Num_6", 0, 0, 0, 5),
            new Button("Num_7", 0, 0, 0, 6),
            new Button("Num_8", 0, 0, 0, 7),
            new Button("Num_9", 0, 0, 0, 8),
            new Button("Num_0", 0, 0, 0, 9),
            new Button("Dash_Slash_Dash_Dash", 0, 0, 0, 12),
	        new Button("Return", 0, 0, 0, 14),
	        new Button("Clear", 0, 0, 0, 15),
            new Button("Channel_Up", 0, 0, 0, 16),
            new Button("Channel_Down", 0, 0, 0, 17),
            new Button("Eject", 0, 0, 0, 22),
	        new Button("Top_Menu", 0, 0, 0, 26),
	        new Button("Time", 0, 0, 0, 40),
	        new Button("Prev", 0, 0, 0, 48),
	        new Button("Next", 0, 0, 0, 49),
	        new Button("Play", 0, 0, 0, 50),
	        new Button("Scan_Back", 0, 0, 0, 51),
	        new Button("Scan_Forward", 0, 0, 0, 52),
	        new Button("Stop", 0, 0, 0, 56),
	        new Button("Pause", 0, 0, 0, 57),
	        new Button("PopUp_Menu", 0, 0, 0, 64),
	        new Button("Step_Back", 0, 0, 0, 96),
	        new Button("Step_Forward", 0, 0, 0, 97),
	        new Button("Subtitle", 0, 0, 0, 99),
	        new Button("Audio", 0, 0, 0, 100),
	        new Button("Angle", 0, 0, 0, 101),
	        new Button("Display", 0, 0, 0, 112),
            new Button("Instant_Forward", 0, 0, 0, 117),
            new Button("Instant_Back", 0, 0, 0, 118),
	        new Button("Blue", 0, 0, 0, 128),
	        new Button("Red", 0, 0, 0, 129),
	        new Button("Green", 0, 0, 0, 130),
	        new Button("Yellow", 0, 0, 0, 131),
	        new Button("Playstation", 0, 0, 1, 67),
	        new Button("Enter", 0, 0, 8, 11),
	        new Button("L2", 0, 1, 0, 88),
	        new Button("R2", 0, 2, 0, 89),
	        new Button("L1", 0, 4, 0, 90),
	        new Button("R1", 0, 8, 0, 91),
	        new Button("Triangle", 0, 16, 0, 92),
	        new Button("Circle", 0, 32, 0, 93),
	        new Button("Cross", 0, 64, 0, 94),
	        new Button("Square", 0, 128, 0, 95),
	        new Button("Select", 1, 0, 0, 80),
	        new Button("L3", 2, 0, 0, 81),
	        new Button("R3", 4, 0, 0, 82),
	        new Button("Start", 8, 0, 0, 83),
	        new Button("Arrow_Up", 16, 0, 0, 84),
	        new Button("Arrow_Right", 32, 0, 0, 85),
	        new Button("Arrow_Down", 64, 0, 0, 86),
	        new Button("Arrow_Left", 128, 0, 0, 87),
        };
        #endregion

        public PS3Remote()
        {
            this.currentApp = App.Current as App;
            currentApp.Log.Write(new LogMessage("@Remote@ Remote Interface Created"));
            timerFindRemote.Interval = TimeSpan.FromMilliseconds(1000);
            timerFindRemote.Tick += new EventHandler(TimerFindRemote_Elapsed);
            timerHibernate.Interval = TimeSpan.FromMinutes(1);
        }

        public void Connect()
        {
            // call findRemote immediately the first time.
            if (hidRemote == null)
            {
                TimerFindRemote_Elapsed(null, null);
            }
        }

        public byte GetBatteryLife
        {
            get { return _batteryLife; }
        }
        private bool _connected = false;

        public bool IsConnected
        {
            get { return _connected; }
        }

        private void ReadButtonData(HidDeviceData InData)
        {
            if ((InData.Status == HidDeviceData.ReadStatus.Success) && (InData.Data[0] == 1))
            {
                timerHibernate.Stop();
                timerHibernate.Start();

                if ((InData.Data[10] == 0) || (InData.Data[4] == 255)) // button released
                {
                    if (ButtonUp != null && isButtonDown && lastButton != null)
                    {
                        currentApp.Dispatcher.BeginInvoke(ButtonUp, lastButton);
                    }
                }
                else // button pressed
                {
                    var pressedButton = new Button("", InData.Data[1], InData.Data[2], InData.Data[3], InData.Data[4]);
                    foreach (var button in Buttons)
                    {
                        if (button.Equals(pressedButton))
                        {
                            lastButton = button; // use button not pressedButton so we get official name string.
                            isButtonDown = true;
                            if (ButtonDown != null)
                            {
                                currentApp.Dispatcher.BeginInvoke(ButtonDown, lastButton);
                            }
                            break;
                        }
                    }
                }
                byte batteryReading = (byte)(InData.Data[11] * 20);

                if (batteryReading != _batteryLife) //Check battery life reading.
                {
                    _batteryLife = batteryReading;
                    if (BatteryLifeChanged != null)  currentApp.Dispatcher.BeginInvoke(BatteryLifeChanged, batteryReading);
                }

                hidRemote.Read(ReadButtonData); //Read next button pressed.
            }
            else
            {

                if (Disconnected != null) currentApp.Dispatcher.BeginInvoke(Disconnected);
                _connected = false;
                timerHibernate.Stop();
                hidRemote.Dispose(); //Dispose of current remote.
                hidRemote = null;

                timerFindRemote.Start(); //Try to reconnect.
            }
        }
        private int missingCount = 0;

        private void TimerFindRemote_Elapsed(object sender, EventArgs e)
        {
            if (missingCount < 5)
            {
                currentApp.Log.Write(new LogMessage("@Remote@ Searching for Remote..."));
            }
            hidRemote = null;
            foreach (var device in Devices)
            {
                IEnumerator<HidDevice> hidDevices = HidDevices.Enumerate(device.VendorID, device.ProductID).GetEnumerator();
                if (hidDevices.MoveNext())
                {
                    hidRemote = hidDevices.Current;
                    ActiveDevice = device;
                    break;
                }
            }

            if (hidRemote != null)
            {
                missingCount = 0;
                currentApp.Log.Write(new LogMessage(String.Format("@Remote@ Found Remote! VendorID: {0}, Product ID: {1}, Description: {2}", ActiveDevice.VendorID, ActiveDevice.ProductID, ActiveDevice.Description)));
                hidRemote.OpenDevice();

                if (Connected != null) currentApp.Dispatcher.BeginInvoke(Connected);

                hidRemote.Read(ReadButtonData);
                _connected = true;
                timerHibernate.Start();
                timerFindRemote.Stop();
            }

            else
            {
                missingCount += 1;
                if (missingCount == 5)
                {
                    currentApp.Log.Write(new LogMessage("@Remote@ Remote not found.  Squelching further log messages until remote is found."));
                }
                else if (missingCount < 5)
                {
                    currentApp.Log.Write(new LogMessage("@Remote@ Remote not found.  Waiting..."));
                }
                _connected = false;
                timerHibernate.Stop();
                timerFindRemote.Start();
            }
        }
    }
}
