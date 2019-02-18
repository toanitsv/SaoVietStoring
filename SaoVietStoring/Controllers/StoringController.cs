using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaoVietStoring.Entites;
using SaoVietStoring.Models;
using System.Data.SqlClient;

namespace SaoVietStoring.Controllers
{
    public class StoringController
    {
        public static List<StoringModel> Select()
        {
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<StoringModel>("EXEC spm_SelectStoring").ToList();
        }

        public static List<StoringModel> SelectIsNotLoad()
        {
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<StoringModel>("EXEC spm_SelectStoringIsNotLoad").ToList();
        }
        
        public static StoringModel SelectByBarcode(string barcode)
        {
            var @Barcode = new SqlParameter("@Barcode", barcode);
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<StoringModel>("EXEC spm_SelectStoringPerBarcode @Barcode", @Barcode).FirstOrDefault();
        }

        public static List<StoringModel> SelectPerPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<StoringModel>("EXEC spm_SelectStoringByPO @ProductNo", @ProductNo).ToList();
        }

        public static List<StoringModel> SelectByDate(DateTime today, string workerId)
        {
            var @Day = new SqlParameter("@Day", today.Day);
            var @Month = new SqlParameter("@Month", today.Month);
            var @Year = new SqlParameter("@Year", today.Year);
            var @WorkerId = new SqlParameter("@WorkerId", workerId);
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<StoringModel>("EXEC spm_SelectStoringByDate @Day, @Month, @Year, @WorkerID", @Day, @Month, @Year, @WorkerId).ToList();
        }

        public static bool Insert(StoringModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Barcode = new SqlParameter("@Barcode", model.Barcode);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @CartonNo = new SqlParameter("@CartonNo", model.CartonNo);
            var @GrossWeight = new SqlParameter("@GrossWeight", model.GrossWeight);
            var @ActualWeight = new SqlParameter("@ActualWeight", model.ActualWeight);
            var @DifferencePercent = new SqlParameter("@DifferencePercent", model.DifferencePercent);
            var @IsPass = new SqlParameter("@IsPass", model.IsPass);
            var @WorkerId = new SqlParameter("@WorkerId", model.WorkerId);
            var @IssuesId = new SqlParameter("@IssuesId", model.IssuesId);
            var @IsComplete = new SqlParameter("@IsComplete", model.IsComplete);
            
            StoringSystemEntities db = new StoringSystemEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertStoring_3 @ProductNo, @Barcode, @SizeNo, @CartonNo, @GrossWeight, @ActualWeight, @DifferencePercent, @IsPass, @WorkerId, @IssuesId, @IsComplete",
                                                                 @ProductNo, @Barcode, @SizeNo, @CartonNo, @GrossWeight, @ActualWeight, @DifferencePercent, @IsPass, @WorkerId, @IssuesId, @IsComplete) >= 1)
            {
                return true;
            }
            else
                return false;
        }
    }
}
