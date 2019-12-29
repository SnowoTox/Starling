using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Starling.FanController
{
    /// <summary>
    /// Provides functions to control the CPU, GPU and Fan
    /// </summary>
    public class FanControllerFunctions
    {
        public enum FanMode
        {
            Manual,
            Dynamic
        }

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

        public void PauseRSX()
        {
            HandleRequest("browser.ps3$rsx_pause");
        }

        /// <summary>
        /// Will continue any processes on the RSX.
        /// </summary>
        public void ContinueRSXProcesses()
        {
            HandleRequest("browser.ps3$rsx_continue");
        }


        /// <summary>
        /// Set the fanspeed. Fanspeed can be between 20% and 95%. Do not write the % symbol in the parameter.
        /// </summary>
        /// <param name="FanSpeed"></param>
        public void SetFanSpeed(int FanSpeed)
        {
            HandleRequest("cpursx.ps3?fan=" + FanSpeed);
        }

        /// <summary>
        /// Increases the fanspeed.
        /// </summary>
        public void IncreaseFanSpeed()
        {
            HandleRequest("cpursx.ps3?up");
        }

        /// <summary>
        /// Decreases the fanspeed.
        /// </summary>
        public void DecreaseFanSpeed()
        {
            HandleRequest("cpursx.ps3?dn");
        }

        /// <summary>
        /// Sets the fan controller to manual or dynamic.
        /// Manual = Fixed Fan Speed
        /// Dynamic = Dynamic Fan Speed, not fixed.
        /// </summary>
        /// <param name="mode"></param>
        public void SetFanControllerMode(FanMode mode)
        {
            if (mode == FanMode.Manual)
                HandleRequest("cpursx.ps3?man");
            else if (mode == FanMode.Dynamic)
                HandleRequest("cpursx.ps3?dyn");
        }
    }
}
