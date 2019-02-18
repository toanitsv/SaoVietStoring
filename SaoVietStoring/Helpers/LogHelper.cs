using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SaoVietStoring.Helpers
{
    class LogHelper
    {
        public static void CreateLog(string log)
        {
            log = string.Format("{0:yyyy-MM-dd hh:mm:ss} {1}{2}", DateTime.Now, log, Environment.NewLine);
            File.AppendAllText(@"Log.txt", log, Encoding.UTF8);
        }
        public static void CreateIssuesLog(string log)
        {
            log = string.Format("{0:yyyy-MM-dd hh:mm:ss} {1}{2}", DateTime.Now, log, Environment.NewLine);
            File.AppendAllText(@"IssuesLog.txt", log, Encoding.UTF8);
        }
        public static void CreateOutputLog(string log)
        {
            log = string.Format("{0:yyyy-MM-dd hh:mm:ss} {1}{2}", DateTime.Now, log, Environment.NewLine);
            File.AppendAllText(@"IssuesOutputLog.txt", log, Encoding.UTF8);
        }
    }
}
