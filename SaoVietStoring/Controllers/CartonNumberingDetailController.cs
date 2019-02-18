using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaoVietStoring.Models;
using SaoVietStoring.Entites;
using System.Data.SqlClient;


namespace SaoVietStoring.Controllers
{
    public class CartonNumberingDetailController
    {
        private static StoringSystemEntities db = new StoringSystemEntities();
        public static List<CartonNumberingDetailModel> Select(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<CartonNumberingDetailModel>("EXEC spm_SelectCartonNumberingDetailByProductNo @ProductNo", @ProductNo).ToList();
        }

        public static List<CartonNumberingDetailModel> SelectPOLoaded(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            return db.ExecuteStoreQuery<CartonNumberingDetailModel>("EXEC spm_SelectCartonNumberingDetailLoadedByPO @ProductNo", @ProductNo).ToList();
        }
        
    }
}
