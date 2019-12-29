using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Starling.IO
{
    public class Files
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

        public void Copy(string path)
        {
            HandleRequest("cpy.ps3/" + path);
        }

        public void Move(string path)
        {
            HandleRequest("cut.ps3/" + path);
        }

        public void Paste(string destination)
        {
            HandleRequest("paste.ps3/" + destination);
        }

        /// <summary>
        /// copy file or folder to destination path
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Destination"></param>
        public void Copy(string Path, string Destination, bool RestartPS3)
        {
            if (RestartPS3)
                HandleRequest("copy.ps3/" + Path + "&to=" + Destination + "?restart.ps3");
            else HandleRequest("copy.ps3/" + Path + "&to=" + Destination);
        }

        public enum Type
        {
            HistoryFiles,
            wmconfig,
            wmtmp,
            wmtmp_wmconfig,
            UninstallWebman
        }


        public void Delete(Type type)
        {
            if (type == Type.HistoryFiles)
                HandleRequest("delete.ps3?history");
            else if (type == Type.wmconfig)
                HandleRequest("delete.ps3?wmconfig");
            else if (type == Type.wmtmp)
                HandleRequest("delete.ps3?wmtmp");
            else if (type == Type.wmtmp_wmconfig)
                HandleRequest("delete.ps3?reset");
            else if (type == Type.UninstallWebman)
                HandleRequest("delete.ps3?uninstall");
        }

        public void Delete(string path, bool restartPS3, bool IsRecursive)
        {
            if (restartPS3 && IsRecursive)
                HandleRequest("delete.ps3/" + path + "?restart.ps3");
            else if(restartPS3 && !IsRecursive)
                HandleRequest("delete_ps3/" + path + "?restart.ps3");
            else if(!restartPS3 && IsRecursive)
                HandleRequest("delete.ps3/" + path);
            else if(!restartPS3 && !IsRecursive)
                HandleRequest("delete_ps3/" + path);
        }

        public void EditFile(string path, string Text)
        {
            HandleRequest("edit.ps3?f=" + path + "&t=" + Text);
        }

        public void MakeDirectory(string path)
        {
            HandleRequest("mkdir.ps3/" + path);
        }

        public void AbortCopy()
        {
            HandleRequest("copy.ps3$abort");
        }



    }
}
