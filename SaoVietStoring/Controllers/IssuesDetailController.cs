using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SaoVietStoring.Models;
using SaoVietStoring.Entites;
using System.Data.SqlClient;

namespace SaoVietStoring.Controllers
{
    public class IssuesDetailController
    {
        public static bool Insert(IssuesDetailModel model)
        {
            var @ProductNo = new SqlParameter("@ProductNo", model.ProductNo);
            var @Barcode = new SqlParameter("@Barcode", model.Barcode);
            var @CartonNo = new SqlParameter("@CartonNo", model.CartonNo);
            var @SizeNo = new SqlParameter("@SizeNo", model.SizeNo);
            var @IssuesId = new SqlParameter("@IssuesId", model.IssuesId);
            var @GrossWeight = new SqlParameter("@GrossWeight", model.GrossWeight);
            var @ActualWeight = new SqlParameter("@ActualWeight", model.ActualWeight);
            var @DifferencePercent = new SqlParameter("@DifferencePercent", model.DifferencePercent);
            var @Process = new SqlParameter("@Process", model.Process);
            var @CheckBy = new SqlParameter("@CheckBy", model.CheckBy);
            var @Location = new SqlParameter("@Location", model.Location);
            StoringSystemEntities db = new StoringSystemEntities();
            if (db.ExecuteStoreCommand("EXEC spm_InsertIssuesDetail @ProductNo, @Barcode, @CartonNo, @SizeNo, @IssuesId, @GrossWeight, @ActualWeight, @DifferencePercent, @Process, @CheckBy,@Location", @ProductNo, @Barcode, @CartonNo, @SizeNo, @IssuesId, @GrossWeight, @ActualWeight, @DifferencePercent, @Process, @CheckBy, @Location) >= 1)
            {
                return true;
            }
            else
                return false;
        }

        public static List<IssuesDetailModel> Select()
        {
            StoringSystemEntities db = new StoringSystemEntities();
            return db.ExecuteStoreQuery<IssuesDetailModel>("EXEC spm_SelectIssuesDetail").ToList();
        }
    }
}
