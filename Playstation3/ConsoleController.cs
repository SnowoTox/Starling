using System;

using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Microsoft.Win32;
using System.Linq;
using System.Text;


namespace Starling
{

    /// <summary>
    /// Provides functions to access the Playstation 3 through the Webman Mod Software.
    /// </summary>
    public class Webman
    {
        
        /// <summary>
        /// Provides the ProcessList and ProcessID variable. ProcessID = Current attached Process.
        /// </summary>
        public struct PROCESS
        {
            public static uint[] ProcessList;
            public static uint ProcessID;
        }


        /// <summary>
        /// This variable stores the IP Address of the current target, when disconnecting, the IP Address data will be erased and will be rewritten when connecting again.
        /// </summary>
        public static IPAddress ip;
        private string ipSTR;

        private Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public enum LedColor
        {
            Red = 0,
            Green = 1,
            Yellow = 2
        }

        public enum LedMode
        {
            Off = 0,
            On = 1,
            BlinkFast = 2,
            BlinkSlow = 3
        }

        public enum BuzzerMode
        {
            Simple = 1,
            Double = 2,
            Triple = 3
        }

        /// <summary>
        /// Provides functions for the in-game experience.
        /// </summary>
        public InGame.InGame InGame
        {
            get { return new InGame.InGame(); }
        }

        /// <summary>
        /// Provides functions to control the fan of the system.
        /// </summary>
        public FanController.FanControllerFunctions FanController
        {
            get { return new FanController.FanControllerFunctions(); }
        }

        /// <summary>
        /// Provides stealth functions to minimize a chance for a console or account ban.
        /// </summary>
        public Stealth.StealthPSN Stealth
        {
            get { return new Stealth.StealthPSN(); }
        }

        /// <summary>
        /// Provides some different function such as database rebuilding, virtual controller, menu switcher (CEX QA/DEX) etc.
        /// </summary>
        public Misc.Misc Misc
        {
            get { return new Misc.Misc(); }
        }

        /// <summary>
        /// Provides functions for CCAPI 2.70 or higher.
        /// </summary>
        public CCAPI.CCAPI CCAPI
        {
            get { return new CCAPI.CCAPI(); }
        }

        /// <summary>
        /// Provides functions for CCAPI 2.60 or lower.
        /// </summary>
        public CCAPI.CCAPI_Legacy CCAPI_Legacy
        {
            get { return new CCAPI.CCAPI_Legacy(); }
        }

        /// <summary>
        /// Provides functions for TMAPI
        /// </summary>
        public TMAPI.TMAPI TMAPI
        {
            get { return new Starling.TMAPI.TMAPI(); }
        }

        /// <summary>
        /// Provides function for file management on the system
        /// </summary>
        public IO.Files Files
        {
            get { return new IO.Files(); }
        }

        /// <summary>
        /// Provides functions for PS3MAPI.
        /// </summary>
        public PS3MAPI.PS3MAPI PS3MAPI
        {
            get { return new Starling.PS3MAPI.PS3MAPI(); }
        }

        public enum PowerFlags
        {
            SoftReboot,
            HardReboot,
            QuickReboot,
            Shutdown
        }

        /// <summary>
        /// Quite obvious. Connects to the specified target.
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public bool Connect(string ipAddress)
        {
            bool result = false;
            result = IPAddress.TryParse(ipAddress, out ip);
            if (result)
            {
                s.Connect(ip, 7887);
                ipSTR = ipAddress;
            }

            if (s.Connected)
                return true;
            else return false;
        }

        public void SetMemory(uint processid, ulong offset, byte[] value)
        {
            HandleRequest("setmem.ps3mapi?proc=" + processid + "&addr=" + offset + "&&val=");
        }

        /// <summary>
        /// Disconnects from the target.
        /// </summary>
        public void Disconnect()
        {
            ip = null;
            s.Disconnect(false);
        }


        private static void HandleRequest(string command)
        {
            WebRequest request = WebRequest.Create("http://" + ip + "/" + command);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader read = new StreamReader(dataStream);
                read.ReadToEnd();
            }
            response.Close();
        }

        /// <summary>
        /// Uses the notify function from PS3MAPI to display notifications on the top right corner of the screen.
        /// </summary>
        /// <param name="Message"></param>
        public void Notify(string Message)
        {
            HandleRequest("notify.ps3mapi?msg=" + Message);
        }

        /// <summary>
        /// Triggers the buzzer function to create a beep sound. Use the BuzzerMode structure for the pre-defined modes.
        /// </summary>
        /// <param name="mode"></param>
        public void RingBuzzer(BuzzerMode mode)
        {
            HandleRequest("buzzer.ps3mapi?mode=" + mode);
        }

        /// <summary>
        /// Do not use this, if you don't know any modes. Use the BuzzerMode structure instead.
        /// </summary>
        /// <param name="mode"></param>
        public void RingBuzzer(string mode)
        {
            HandleRequest("buzzer.ps3mapi?mode=" + mode);
        }
        /// <summary>
        /// Sets the console led and mode.
        /// </summary>
        /// <param name="color"></param>
        /// <param name="mode"></param>
        public void SetConsoleLed(LedColor color, LedMode mode)
        {
            HandleRequest("led.ps3mapi?color=" + color + "&mode=" + mode);
        }

        public void SetConsoleLed(string color, string mode)
        {
            HandleRequest("led.ps3mapi?color=" + color + "&mode=" + mode);
        }

        public void SetIDPS(string IDPS, string PSID)
        {
            if (IDPS.Length < 32 || IDPS.Length > 32 && PSID.Length < 32 || PSID.Length > 32)
                MessageBox.Show("Error. Invalid Length of IDPS or PSID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                IDPS.Substring(15, 16);
            }


            HandleRequest("setidps.ps3mapi?idps1= " + IDPS + "&idps2= ");
        }

        public enum UnloadSlot
        {
            Slot1 = 1,
            Slot2 = 2,
            Slot3 = 3,
            Slot4,
            Slot5,
            Slot6,
            Slot7,
            Slot8,
            Slot9,
            Slot10,
            Slot11,
            Slot12,
            Slot13,
            Slot14,
            Slot15,
            Slot16,
            Slot17,
            Slot18,
            Slot19,
            Slot20,
            Slot21,
            Slot22,
            Slot23,
            Slot24,
            Slot25,
            Slot26,
            Slot27,
            Slot28,
            Slot29,
            Slot30,
            Slot31,
            Slot32,
            Slot33,
            Slot34,
            Slot35,
            Slot36,
            Slot37,
            Slot38,
            Slot39,
            Slot40,
            Slot41,
            Slot42,
            Slot43,
            Slot44,
            Slot45,
            Slot46,
            Slot47,
            Slot48,
            Slot49,
            Slot50,
            Slot51,
            Slot52,
            Slot53,
            Slot54,
            Slot55,
            Slot56,
            Slot57,
            Slot58,
            Slot59,
            Slot60,
            AllSlots

        }


        public void GamePlugins(UnloadSlot slot)
        {
            if (slot == UnloadSlot.AllSlots)
                for (int i = 0; i < 60; i++)
                    HandleRequest("gameplugin.ps3mapi?proc=16777984&prx=&unload_slot=" + i);
            else HandleRequest("gameplugin.ps3mapi?proc=16777984&prx=&load_slot=" + slot);

        }



        /// <summary>
        /// Commands to power off the target.
        /// </summary>
        /// <param name="flag"></param>
        public void Power(PowerFlags flag)
        {
            if (flag == PowerFlags.QuickReboot)
                HandleRequest("reboot.ps3?quick");
            else if (flag == PowerFlags.SoftReboot)
                HandleRequest("reboot.ps3?soft");
            else if (flag == PowerFlags.HardReboot)
                HandleRequest("reboot.ps3?hard");
            else if (flag == PowerFlags.Shutdown)
                HandleRequest("shutdown.ps3");
        }

        /// <summary>
        /// Class that provides functions about the network status of several PS3 services
        /// </summary>
        public class NetworkStatus
        {
            public void ShowNetworkStatus()
            {
                HandleRequest("netstatus.ps3?");
            }

            public void ShowFTPServerStatus()
            {
                HandleRequest("netstatus.ps3?ftp");
            }

            public void ShowPS3MAPIStatus()
            {
                HandleRequest("netstatus.ps3?ps3mapi");
            }

            public void ShowNetworkStatusServer()
            {
                HandleRequest("netstatus.ps3?netsrv");
            }

            public void StopFTPServer()
            {
                HandleRequest("netstatus.ps3?stop-ftp");
            }

            public void StopNetworkServer()
            {
                HandleRequest("netstatus.ps3?stop-netsrv");
            }

            public void StopPS3MAPIServer()
            {
                HandleRequest("netstatus.ps3?stop-ps3mapi");
            }

            public void StopAllServers()
            {
                HandleRequest("netstatus.ps3?stop");
            }
        }
    
    }
}