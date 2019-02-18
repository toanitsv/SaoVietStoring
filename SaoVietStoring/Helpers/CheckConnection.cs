using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaoVietStoring.Entites;

namespace SaoVietStoring.Helpers
{
    class CheckConnection
    {
        public static bool Exist()
        {
            StoringSystemEntities db = new StoringSystemEntities();
            if (db.DatabaseExists() == true)
            {
                return true;
            }
            return false;
        }
    }
}
