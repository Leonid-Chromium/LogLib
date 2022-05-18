using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace LogLib
{
	[Serializable]
	public class Log
	{
		public string level { get; set; }
		public DateTime dateTime { get; set; }
		public string version { get; set; }
		public uint code { get; set; }
		public string user { get; set; }
		public string message { get; set; }

		public Log()
		{

		}

        public static int LogTrace(Log log)
        {
            try
            {
                Trace.WriteLine("Level: \t\t" + log.level);
                Trace.WriteLine("Data-time: \t\t" + log.dateTime);
                Trace.WriteLine("Version: \t\t" + log.version);
                Trace.WriteLine("Code: \t\t" + log.code);
                Trace.WriteLine("User: \t\t" + log.user);
                Trace.WriteLine("Message: \t\t" + log.message);

                return 0;
            }
            catch
            {
                return 1;
            }
        }

        public static string LogString(Log log)
        {
            try
            {
                string logStr = "Level: \t\t" + log.level +
                    "\nData-time: \t\t" + log.dateTime +
                    "\nVersion: \t\t" + log.version +
                    "\nCode: \t\t" + log.code +
                    "\nUser: \t\t" + log.user +
                    "\nMessage: \t\t" + log.message;

                return logStr;
            }
            catch
            {
                return "Ошибка в методе LogString";
            }
        }
    }
}
