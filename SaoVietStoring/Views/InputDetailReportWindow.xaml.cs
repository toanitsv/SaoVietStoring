using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Data;

using SaoVietStoring.Controllers;
using SaoVietStoring.Models;
using SaoVietStoring.Helpers;
using SaoVietStoring.DataSets;


namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for DetailReportWindow.xaml
    /// </summary>
    public partial class InputDetailReportWindow : Window
    {
        BackgroundWorker bwReport;
        List<StoringModel> storingPerPOList;
        List<OutputingModel> outputingPerPOList;
        List<AccountModel> accountList;
        public InputDetailReportWindow()
        {
            storingPerPOList = new List<StoringModel>();
            outputingPerPOList = new List<OutputingModel>();
            accountList = new List<AccountModel>();

            bwReport = new BackgroundWorker();
            bwReport.DoWork += new DoWorkEventHandler(bwReport_DoWork);
            bwReport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwReport_RunWorkerCompleted);
            InitializeComponent();
        }

        string productNo = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProductNo.Focus();
        }


        bool output = false;
        private void btnReportOutput_Click(object sender, RoutedEventArgs e)
        {
            output = true;
            productNo = txtProductNo.Text.ToUpper();
            if (String.IsNullOrEmpty(productNo) == true)
            {
                return;
            }

            if (bwReport.IsBusy == true)
            {
                return;
            }

            dgDetailReport.ItemsSource = null;
            btnReport.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            bwReport.RunWorkerAsync();
        }
        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            output = false;
            productNo = txtProductNo.Text.ToUpper();
            if (String.IsNullOrEmpty(productNo) == true)
            {
                return;
            }

            if (bwReport.IsBusy == true)
            {
                return;
            }

            dgDetailReport.ItemsSource = null;
            btnReport.IsEnabled = false;
            this.Cursor = Cursors.Wait;
            bwReport.RunWorkerAsync();
        }

        //string gtnPONo = "", totalCartonInputedString = "", totalWeightString = "";
        private void bwReport_DoWork(object sender, DoWorkEventArgs e)
        {
            outputingPerPOList = OutputingController.SelectPerPO(productNo).OrderBy(o => o.CartonNo).ToList();
            storingPerPOList = StoringController.SelectPerPO(productNo).OrderBy(o => o.CartonNo).ToList();
            accountList = AccountController.Get();
        }

        private void bwReport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            this.Cursor = null;
            btnReport.IsEnabled = true;

            DataTable dtStoring = new StoringDataSet().Tables["StoringTable"];

            if (output == false)
            {
                foreach (var storing in storingPerPOList)
                {
                    DataRow dr = dtStoring.NewRow();

                    dr["ProductNo"] = storing.ProductNo;
                    dr["CartonNo"] = storing.CartonNo;
                    dr["Barcode"] = storing.Barcode;
                    dr["SizeNo"] = storing.SizeNo;
                    if (storing.GrossWeight > 0)
                    {
                        dr["GrossWeight"] = storing.GrossWeight;
                    }
                    if (storing.ActualWeight > 0)
                    {
                        dr["ActualWeight"] = storing.ActualWeight;
                    }
                    if (storing.DifferencePercent != 0)
                    {
                        dr["DifferencePercent"] = storing.DifferencePercent;
                    }
                    dr["IsPass"] = storing.IsPass;
                    dr["InputtedTime"] = String.Format("{0:yyyy/MM/dd HH:mm:ss}", storing.CreatedTime);

                    string locationString = "";
                    var account = accountList.Where(w => w.UserName == storing.WorkerId).FirstOrDefault();
                    if (account != null)
                    {
                        var location = ElectricScaleProfileHelper.ElectricScaleProfileList().Where(w => w.ProfileId == account.ElectricScaleId).FirstOrDefault();
                        locationString = location != null ? location.Location : "";
                    }
                    dr["Location"] = locationString;
                    dtStoring.Rows.Add(dr);
                }
            }
            if (output == true)
            {
                foreach (var outputing in outputingPerPOList)
                {
                    DataRow dr = dtStoring.NewRow();

                    dr["ProductNo"] = outputing.ProductNo;
                    dr["CartonNo"] = outputing.CartonNo;
                    dr["Barcode"] = outputing.Barcode;
                    dr["SizeNo"] = outputing.SizeNo;
                    if (outputing.GrossWeight > 0)
                    {
                        dr["GrossWeight"] = outputing.GrossWeight;
                    }
                    if (outputing.ActualWeight > 0)
                    {
                        dr["ActualWeight"] = outputing.ActualWeight;
                    }
                    if (outputing.DifferencePercent != 0)
                    {
                        dr["DifferencePercent"] = outputing.DifferencePercent;
                    }
                    dr["IsPass"] = outputing.IsPass;
                    dr["InputtedTime"] = String.Format("{0:yyyy/MM/dd HH:mm:ss}", outputing.CreatedTime);

                    string locationString = "";
                    var account = accountList.Where(w => w.UserName == outputing.WorkerId).FirstOrDefault();
                    if (account != null)
                    {
                        var location = ElectricScaleProfileHelper.ElectricScaleProfileList().Where(w => w.ProfileId == account.ElectricScaleId).FirstOrDefault();
                        locationString = location != null ? location.Location : "";
                    }
                    dr["Location"] = locationString;
                    dtStoring.Rows.Add(dr);
                }
            }

            dgDetailReport.ItemsSource = dtStoring.AsDataView();
            output = false;
        }

        private void txtProductNo_GotMouseCapture(object sender, MouseEventArgs e)
        {
            txtProductNo.SelectAll();
        }
    }
}
