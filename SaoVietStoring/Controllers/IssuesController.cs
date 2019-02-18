using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaoVietStoring.Models;
using SaoVietStoring.Entites;
using System.Data.SqlClient;

namespace SaoVietStoring.Controllers
{
    class IssuesController
    {
        public static List<IssuesModel> Select()
        {
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<IssuesModel>("EXEC spm_SelectIssues").ToList();
        }
    }
}
