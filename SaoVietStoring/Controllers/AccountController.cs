using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaoVietStoring.Models;
using SaoVietStoring.Entites;
using System.Data.SqlClient;

namespace SaoVietStoring.Controllers
{
    public class AccountController
    {
        public static AccountModel Select(string userName, string password)
        {
            var @UserName = new SqlParameter("@UserName", userName);
            var @Password = new SqlParameter("@Password", password);
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<AccountModel>("EXEC spm_CheckAccount @UserName,@Password", @UserName, @Password).FirstOrDefault();
        }

        public static List<AccountModel> Get()
        {
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<AccountModel>("EXEC spm_GetAccount").ToList();
        }
    }
}
