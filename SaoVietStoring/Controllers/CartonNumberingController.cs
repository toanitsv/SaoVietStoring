using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaoVietStoring.Models;
using SaoVietStoring.Entites;
using System.Data.SqlClient;

namespace SaoVietStoring.Controllers
{
    public class CartonNumberingController
    {
        private static StoringSystemEntities db = new StoringSystemEntities();
        public static List<CartonNumberingModel> Get(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<CartonNumberingModel>("spm_SelectCartonNumberingByProductNo @ProductNo", @ProductNo).ToList();
        }
    }
}
