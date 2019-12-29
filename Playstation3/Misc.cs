using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Starling.Misc
{

    public class Misc
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



        public struct ButtonNames
        {
            public static string
            Circle = "circle",
            Triangle = "triangle",
            Square = "square",
            Cross = "cross",
            Up = "up",
            Down = "down",
            Left = "left",
            Right = "right",
            R1 = "r1",
            R2 = "r2",
            L1 = "l1",
            L2 = "l2",
            PSButton = "psbtn",
            SELECT = "select",
            START = "start",
            Hold = "hold",
            Release = "release";
        }


        public enum Mode
        {
            Normal,
            Rebug
        }


        /// <summary>
        /// Select between REBUG and Normal mode. SwapMenu means that it also will swap the debug menu from CEX to DEX and vice versa. (Note that the library doesn't know if it's a CEX or DEX menu, it will just swap it.)
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="SwapMenu"></param>
        public void ToggleMode(Mode mode, bool SwapMenu)
        {
            if (mode == Mode.Rebug && SwapMenu == true)
            {
                HandleRequest("browser.ps3$toggle_debug_menu");
                HandleRequest("browser.ps3$toggle_rebug_mode");
            }
            else HandleRequest("browser.ps3$toggle_rebug_mode");

            if (mode == Mode.Normal && SwapMenu == true)
            {
                HandleRequest("browser.ps3$toggle_debug_menu");
                HandleRequest("browser.ps3$toggle_normal_mode");
            }
            else HandleRequest("browser.ps3$toggle_normal_mode");
        }

        public void PauseRSX(bool Pause)
        {
            if (Pause)
                HandleRequest("browser.ps3$rsx_pause");
            else HandleRequest("/browser.ps3$rsx_continue");
        }

        /// <summary>
        /// Simulates a controller button input.
        /// Custom inputs are allowed, use the ButtonNames structure for the built-in button functions
        /// </summary>
        /// <param name="ButtonName"></param>
        public void PressButton(string names)
        {
            HandleRequest("pad.ps3?" + names);
        }

        public void PressButton(ButtonNames names)
        {
            HandleRequest("pad.ps3?" + names);
        }

        /// <summary>
        /// Custom inputs are allowed, use the ButtonNames structure for the built-in button functions
        /// </summary>
        public void HoldButton(string names)
        {
            HandleRequest("hold.ps3?" + names);
        }

        public void HoldButton(ButtonNames names)
        {
            HandleRequest("hold.ps3?" + names);
        }

        /// <summary>
        /// Shows the minimum downgrade version as a notify message on the PS3
        /// </summary>
        public void ShowMinimumDowngradeVersion()
        {
            HandleRequest("minver.ps3");
        }

        /// <summary>
        /// Rebuilds the PS3 database
        /// </summary>
        public void RebuildDatabase()
        {
            HandleRequest("rebuild.ps3");
        }


    }
}
