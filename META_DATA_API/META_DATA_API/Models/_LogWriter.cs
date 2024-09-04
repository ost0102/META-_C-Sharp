using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace META_DATA_API.Models
{
    public class _LogWriter
    {
        public static void Writer(string message)
        {
            string path = "";
            path = System.Web.HttpContext.Current.Server.MapPath("~/EndLog/") + DateTime.Now.ToString("yyyyMM") + "/";
            DateTime dateTime = DateTime.Now;
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
            using (StreamWriter sw = new StreamWriter(path + dateTime.ToLongDateString() + ".txt", true, System.Text.Encoding.Default))
            {
                //string logdata = string.Format("{0:HH:mm:ss}({1}) - {2} ///END///!", dateTime, "", message);
                sw.WriteLine(message);
            }
        }

        public static void Auth_Writer(string message)
        {
            string path = "";
            path = System.Web.HttpContext.Current.Server.MapPath("~/AuthLog/") + DateTime.Now.ToString("yyyyMM") + "/";
            DateTime dateTime = DateTime.Now;
            if (Directory.Exists(path) == false) Directory.CreateDirectory(path);
            using (StreamWriter sw = new StreamWriter(path + dateTime.ToLongDateString() + ".txt", true, System.Text.Encoding.Default))
            {
                //string logdata = string.Format("{0:HH:mm:ss}({1}) - {2} ///END///!", dateTime, "", message);
                sw.WriteLine(message);
            }
        }
    }
}