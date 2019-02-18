using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaoVietStoring.Entites;
using SaoVietStoring.Models;
using System.Data.SqlClient;
namespace SaoVietStoring.Controllers
{
    public class OutputingController
    {
        public static List<OutputingModel> SelectIsNotLoad()
        {
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<OutputingModel>("EXEC spm_SelectOutputingIsNotLoad").ToList();
        }

        public static List<OutputingModel> SelectPerPO(string productNo)
        {
            var @ProductNo = new SqlParameter("@ProductNo", productNo);
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<OutputingModel>("EXEC spm_SelectOutputingPerPO @ProductNo", @ProductNo).ToList();
        }
        public static bool Insert(OutputingModel model)
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

            StoringSystemEntities db = new StoringSystemEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertOutputing @ProductNo, @Barcode, @SizeNo, @CartonNo, @GrossWeight, @ActualWeight, @DifferencePercent, @IsPass, @WorkerId, @IssuesId",
                                                                 @ProductNo, @Barcode, @SizeNo, @CartonNo, @GrossWeight, @ActualWeight, @DifferencePercent, @IsPass, @WorkerId, @IssuesId) >= 1)
            {
                return true;
            }
            else
                return false;
        }
    }
}
