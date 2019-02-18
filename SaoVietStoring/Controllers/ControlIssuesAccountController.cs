using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaoVietStoring.Models;
using SaoVietStoring.Entites;
using System.Data.SqlClient;

namespace SaoVietStoring.Controllers
{
    public class ControlIssuesAccountController
    {
        public static ControlIssuesAccountModel FindSecurityCode(int securityCode)
        {
            var @SecurityCode = new SqlParameter("@SecurityCode", securityCode);
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<ControlIssuesAccountModel>("EXEC spm_CheckControlIssuesAccount @SecurityCode", @SecurityCode).FirstOrDefault();
        }
    }
}
