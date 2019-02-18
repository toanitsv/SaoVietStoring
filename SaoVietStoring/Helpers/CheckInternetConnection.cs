using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace SaoVietStoring.Helpers
{
    public class CheckInternetConnection
    {
        /// <summary>
        /// Check connect to goolgle.com
        /// </summary>
        /// <returns></returns>
        public static bool CheckConnection()
        {
            WebClient client = new WebClient();
            try
            {
                using (client.OpenRead("http://www.google.com"))
                {
                }
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }
    }
}
