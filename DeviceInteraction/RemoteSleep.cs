using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS3RemoteManager
{
    public class RemoteSleep
    {
        private DebugLog log = null;

        public RemoteSleep(DebugLog log)
        {
            this.log = log;
        }
        // hibernate remote.
        public void HibernateRemoteWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            log.Write(e.UserState.ToString());
        }

        public void HibernateRemoteWorker(object sender, DoWorkEventArgs e)
        {

            BackgroundWorker worker = sender as BackgroundWorker;

            string deviceConnectSoundFile = "";
            string deviceDisconnectSoundFile = "";
            try
            {
                worker.ReportProgress(0, "Attempting to hibernate remote...");
                worker.ReportProgress(0, "    - Creating Bluetooth client");
                InTheHand.Net.Sockets.BluetoothClient cli = new InTheHand.Net.Sockets.BluetoothClient(); //Create a bluetooth client for the enquiry
                worker.ReportProgress(0, "    - Discovering devices");
                cli.InquiryLength = new TimeSpan(0, 0, 1); //Discover new devices for 1sec, actually we only really need to get the remembered/paired device.
                // slow
                InTheHand.Net.Sockets.BluetoothDeviceInfo[] devs = cli.DiscoverDevices();

                worker.ReportProgress(0, "    - Finding 'BD Remote Control'");
                InTheHand.Net.Sockets.BluetoothDeviceInfo remoteInfo = devs.First(dev => dev.DeviceName == "BD Remote Control"); //Find the remote control

                //The next line will cause a SocketException if the device is already in sleep mode
                worker.ReportProgress(0, "    - checking for sleep state, enumerating services");
                InTheHand.Net.Bluetooth.ServiceRecord[] services = remoteInfo.GetServiceRecords(InTheHand.Net.Bluetooth.BluetoothService.HumanInterfaceDevice);

                if (remoteInfo != null)
                {
                    worker.ReportProgress(0, "    - Remote found with address: " + remoteInfo.DeviceAddress.ToString());
                    //remoteInfo.DeviceAddress containts the address of the remote, could possibly store and reuse this later.

                    //Store the user hardware sounds and blank them
                    worker.ReportProgress(0, "    - Attempting to disable hardware sounds...");
                    worker.ReportProgress(0, "        - Get current hardware sounds");
                    deviceConnectSoundFile = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Current", "", "");
                    deviceDisconnectSoundFile = (string)Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\.Current", "", "");
                    try
                    {
                        worker.ReportProgress(0, "        - Disable hardware sounds");
                        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Current", "", "");
                        Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\.Current", "", "");

                        //Switch off the HID service
                        worker.ReportProgress(0, "    - Attempting to disable HID service...");
                        //slow
                        remoteInfo.SetServiceState(InTheHand.Net.Bluetooth.BluetoothService.HumanInterfaceDevice, false);
                        worker.ReportProgress(0, "        - HID service disabled successfully!");
                        //Re-enable off the HID service
                        worker.ReportProgress(0, "    - Attempting to restart HID service...");
                        // slow
                        remoteInfo.SetServiceState(InTheHand.Net.Bluetooth.BluetoothService.HumanInterfaceDevice, true);
                        worker.ReportProgress(0, "        - HID service enabled successfully!");
                        worker.ReportProgress(0, "Remote Successfully Hibernated!");
                    }
                    finally
                    {
                        //Restore the user hardware sounds 
                        try
                        {
                            worker.ReportProgress(0, "Attempting to restore hardware sounds...");
                            System.Threading.Thread.Sleep(1000);
                            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\DeviceConnect\.Current", "", deviceConnectSoundFile);
                            Microsoft.Win32.Registry.SetValue(@"HKEY_CURRENT_USER\AppEvents\Schemes\Apps\.Default\DeviceDisconnect\.Current", "", deviceDisconnectSoundFile);
                            worker.ReportProgress(0, "Hardware sounds restored!");
                        }
                        catch
                        {
                            worker.ReportProgress(0, "Restoring hardware sounds failed!");
                        }
                    }
                }
                else
                {
                    worker.ReportProgress(0, "Hibernation Failed: PS3 remote could not be found.");
                }
            }
            catch (System.Net.Sockets.SocketException)
            {
                worker.ReportProgress(0, "Hibernation Failed: device is already in a sleep state.");
            }
            catch (Exception ex)
            {
                worker.ReportProgress(0, "Exception during hibernation: " + ex.Message);
            }
        }
    }
}
