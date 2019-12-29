using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Starling.Stealth
{
    /// <summary>
    /// Provides functions to make the PS3 invisible on the PSN (Syscall Settings)
    /// </summary>
    public class StealthPSN
    {
        private static void HandleRequest(string command)
        {

            WebRequest request = WebRequest.Create("http://" + Webman.ip + "/" + command);
            WebResponse response = request.GetResponse();

            using (Stream dataStream = response.GetResponseStream())
            {
                StreamReader read = new StreamReader(dataStream);
                read.ReadToEnd();
            }
            response.Close();
        }

        public enum SyscallsToDisable
        {
            /// <summary>
            /// Disables all CFW Syscalls
            /// </summary>
            All,

            /// <summary>
            /// Disables all Syscalls, except the CCAPI Syscalls.
            /// </summary>
            KeepCCAPISyscalls
        }

        public enum SyscallMode
        {
            /// <summary>
            /// The Syscall 8 is fully enabled.
            /// </summary>
            FullyEnabled,

            /// <summary>
            /// Disables all features, except Cobra/Mamba and PS3MAPI features.
            /// </summary>
            PartialDisabled_KeepCobraMambaPS3MAPIFeatures,

            /// <summary>
            /// Disables all features, except PS3MAPI features.
            /// </summary>
            PartialDisabled_KeepPS3MAPIFeatures,

            /// <summary>
            /// Disables all features, except the LV1 Peek function. Syscall 8 can be fully enabled later.
            /// </summary>
            KeepLV1Peek_Only,

            /// <summary>
            /// Tricks the system into thinking that it's disabled while it's still enabled. Can be fully enabled later.
            /// </summary>
            FakeDisabled,

            /// <summary>
            /// Disables all Syscall 8 Functions, the Syscall cannot be re-enabled later, only when the system is restarted.
            /// </summary>
            FullyDisabled
        }

        public enum Data
        {
            IDPS,
            PSID,
            AllInOne
        }

        /// <summary>
        /// Disables CFW Syscalls for safe PSN login.
        /// </summary>
        public void DisableCFWSyscalls(SyscallsToDisable SC)
        {
            if (SC == SyscallsToDisable.All)
                HandleRequest("browser.ps3$disable_syscalls");
            else if (SC == SyscallsToDisable.KeepCCAPISyscalls)
                HandleRequest("browser.ps3$disable_syscalls?ccapi");
        }
        

        public void Dump(Data data)
        {
            if (data == Data.IDPS)
                HandleRequest("idps.ps3");
            else if (data == Data.PSID)
                HandleRequest("psid.ps3");
            else if (data == Data.AllInOne)
                HandleRequest("consoleid.ps3");
        }

        public void Dump(Data data, string path)
        {
            if (data == Data.IDPS)
                HandleRequest("idps.ps3?" + path);
            else if (data == Data.PSID)
                HandleRequest("psid.ps3?" + path);
            else if (data == Data.AllInOne)
                HandleRequest("consoleid.ps3");
        }
    }
}
