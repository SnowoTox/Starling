using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Starling.InGame
{
    public class InGame
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

        public enum VideoFormat
        {
            MP4 = 1,
            JPEG = 2,
            PSP = 3,
            HD = 4,
            AVC = 5,
            AAC = 6,
            PCM = 7
        }

        public enum AudioFormat
        {
            _64 = 64,
            _128 = 128,
            _384 = 384,
            _512 = 512,
            _768 = 768,
            _1024 = 1024,
            _1536 = 1536,
            _2048 = 2048
        }

        /// <summary>
        /// Will record the gameplay, use the VideoFormat structure for video formats.
        /// </summary>
        /// <param name="FormatType"></param>
        public void VideoRecorder(string FormatType)
        {
            HandleRequest("videorec.ps3?" + FormatType);
        }


        /// <summary>
        /// Will record the gameplay, use the AudioFormat enumeration for audio formats.
        /// </summary>
        /// <param name="FormatType"></param>
        public void VideoRecorder(AudioFormat FormatType)
        {
            HandleRequest("videorec.ps3?" + FormatType);        
        }

        /// <summary>
        /// Will record the gameplay, custom AudioFormat inputs are allowed. 
        /// </summary>
        /// <param name="AudioFormat"></param>
        public void VideoRecorder(int AudioFormat)
        {
            HandleRequest("videorec.ps3?" + AudioFormat);
        }


        /// <summary>
        /// Will record the gameplay
        /// </summary>
        public void VideoRecorder()
        {
            HandleRequest("videorec.ps3");
        }
    }
}
