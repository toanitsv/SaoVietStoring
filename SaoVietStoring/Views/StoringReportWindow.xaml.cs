using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using System.ComponentModel;

using SaoVietStoring.DataSets;
using SaoVietStoring.Models;
using SaoVietStoring.Controllers;
using Microsoft.Reporting.WinForms;

namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for StoringReportWindow.xaml
    /// </summary>
    public partial class StoringReportWindow : Window
    {
        BackgroundWorker bwReport;
        public StoringReportWindow()
        {
            InitializeComponent();

            bwReport = new BackgroundWorker();
            bwReport.DoWork += new DoWorkEventHandler(bwReport_DoWork);
            bwReport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwReport_RunWorkerCompleted);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProductNo.Focus();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            string productNo = txtProductNo.Text.ToUpper();
            if (string.IsNullOrEmpty(productNo) == true)
            {
                return;
            }

            if (bwReport.IsBusy == true)
            {
                return;
            }

            btnReport.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            bwReport.RunWorkerAsync(productNo);
        }

        private void bwReport_DoWork(object sender, DoWorkEventArgs e)
        {
            string productNo = e.Argument.ToString();
            List<CartonNumberingModel> cartonNumberingList = CartonNumberingController.Get(productNo).ToList();
            List<CartonNumberingDetailModel> cartonNumberingDetailList = CartonNumberingDetailController.Select(productNo);
            List<StoringModel> storingList = StoringController.SelectPerPO(productNo);

            DataTable dtCartonNumbering = new CartonNumberingDataSet().Tables[0];
            DataTable dtStoring = new StoringReportDataSet().Tables[0];

            foreach (CartonNumberingModel cartonNumbering in cartonNumberingList)
            {
                DataRow drCartonNumbering = dtCartonNumbering.NewRow();
                drCartonNumbering["ProductNo"] = cartonNumbering.ProductNo;
                drCartonNumbering["SizeNo"] = cartonNumbering.SizeNo;
                drCartonNumbering["Quantity"] = cartonNumbering.Quantity;
                StoringModel storingPerSize = storingList.Where(p => p.SizeNo == cartonNumbering.SizeNo).FirstOrDefault();
                if (storingPerSize != null)
                {
                    drCartonNumbering["GrossWeight"] = storingPerSize.ActualWeight;
                    drCartonNumbering["FirstCartonOfSize"] = storingPerSize.CartonNo;
                }
                
                dtCartonNumbering.Rows.Add(drCartonNumbering);

                List<CartonNumberingDetailModel> cartonNumberingDetail_D1 = cartonNumberingDetailList.Where(c => c.SizeNo == cartonNumbering.SizeNo).ToList();
                for (int i = 1; i <= cartonNumberingDetail_D1.Count(); i++)
                {
                    CartonNumberingDetailModel cartonNumberingDetail = cartonNumberingDetail_D1[i - 1];
                    StoringModel storing = storingList.Where(p => p.CartonNo == cartonNumberingDetail.CartonNo).FirstOrDefault();

                    DataRow drStoring = dtStoring.NewRow();
                    drStoring["ProductNo"] = cartonNumberingDetail.CartonNo;
                    drStoring["ProductNo"] = cartonNumbering.ProductNo;
                    drStoring["SizeNo"] = cartonNumbering.SizeNo;
                    drStoring["NumberOf"] = i;
                    drStoring["StoringValue"] = string.Format("{0} |", cartonNumberingDetail.CartonNo);
                    if (storing != null)
                    {
                        drStoring["StoringValue"] = string.Format("{0} | {1}", cartonNumberingDetail.CartonNo, storing.ActualWeight);
                        
                        drStoring["IsPass"] = storing.IsPass;
                    }
                    dtStoring.Rows.Add(drStoring);
                }
            }
            string storingDate = "";
            string quantityCarton = "";
            if (storingList.Count() > 0 && cartonNumberingDetailList.Count() > 0)
            {
                storingDate = string.Format("{0:dd/MM/yyyy}", storingList.FirstOrDefault().CreatedTime);
                quantityCarton = string.Format("{0} of {1}", storingList.Count(), cartonNumberingDetailList.Select(s => s.CartonNo).Max());
            }
            double totalWeight = storingList.Sum(p => p.ActualWeight);
            object[] results = { productNo, storingDate, dtCartonNumbering, dtStoring, totalWeight, quantityCarton };
            e.Result = results;
        }

        private void bwReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnReport.IsEnabled = true;

            object[] results = e.Result as object[];
            string productNo = results[0] as string;
            string storingDate = results[1] as string;
            double totalWeight = (results[4] as double?).Value;
            string quantityCarton = results[5] as string;
            DataTable dtCartonNumbering = results[2] as DataTable;
            DataTable dtStoring = results[3] as DataTable;

            ReportParameter rpProductNo = new ReportParameter("ProductNo", productNo);
            ReportParameter rpStoringDate = new ReportParameter("StoringDate", storingDate);
            ReportParameter rpTotalWeight = new ReportParameter("TotalWeight", totalWeight.ToString());
            ReportParameter rpQuantityCarton = new ReportParameter("TotalCarton", quantityCarton);

            ReportDataSource rds = new ReportDataSource();
            rds.Name = "CartonNumberingDetail";
            rds.Value = dtCartonNumbering;
            ReportDataSource rds1 = new ReportDataSource();
            rds1.Name = "StoringReportDetail";
            rds1.Value = dtStoring;
            //Debug
            //reportViewer.LocalReport.ReportPath = @"E:\SV PROJECT\Storing\Storing2.1_BigChange\SaoVietStoring\Reports\StoringReport.rdlc";
            //Release
            reportViewer.LocalReport.ReportPath = @"Reports\StoringReport.rdlc";
            reportViewer.LocalReport.SetParameters(new ReportParameter[] { rpProductNo, rpStoringDate, rpTotalWeight, rpQuantityCarton });
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(rds);
            reportViewer.LocalReport.DataSources.Add(rds1);
            reportViewer.RefreshReport();
        }

        private void txtProductNo_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txtProductNo.SelectAll();
        }
    }
}
