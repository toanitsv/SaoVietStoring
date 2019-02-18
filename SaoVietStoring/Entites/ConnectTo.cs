using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaoVietStoring.Entites;

namespace SaoVietStoring.Entites
{
    class ConnectTo
    {
        public static StoringSystemEntities StoringSystemEntities()
        {
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["StoringSystemEntities"].ConnectionString;
            return new StoringSystemEntities(string.Format(connectionString, "sa", "sa@123456"));
        }
    }
}
