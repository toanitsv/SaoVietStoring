using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaoVietStoring.Models;
using SaoVietStoring.Entites;
using System.Data.SqlClient;

namespace SaoVietStoring.Controllers
{
    public class PORepackingController
    {
        private static StoringSystemEntities db = new StoringSystemEntities();

        public static List<PORepackingModel> GetAll()
        {
            return db.ExecuteStoreQuery<PORepackingModel>("EXEC spm_SelectPORepacking").ToList();
        }

        public static bool Insert(PORepackingModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @CreatedTime = new SqlParameter("@CreatedTime", model.CreatedTime);

            if (db.ExecuteStoreCommand("EXEC spm_InsertPORepacking @ProductNo, @CreatedTime",
                                                                   @ProductNo, @CreatedTime) >= 1)
            {
                return true;
            }
            else
                return false;
        }

        public static bool Update(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);

            if (db.ExecuteStoreCommand("EXEC spm_UpdatePORepacking @ProductNo",
                                                                   @ProductNo) >= 1)
            {
                return true;
            }
            else
                return false;
        }
    }
}
