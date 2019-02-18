using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;

using SaoVietStoring.Models;
using SaoVietStoring.Controllers;

namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for OuputReportWindow.xaml
    /// </summary>
    public partial class OuputReportWindow : Window
    {
        List<OutputtingModel> outputList;
        List<OutputDetail> outputDetailList;
        List<OutputDetail> notOutputDetailList;
        List<OutputDetail> allOutputDetailList;

        OrdersModel orderSearchByProductNo;
        public OuputReportWindow()
        {
            outputList = new List<OutputtingModel>();
            outputDetailList = new List<OutputDetail>();
            notOutputDetailList = new List<OutputDetail>();
            allOutputDetailList = new List<OutputDetail>();
            orderSearchByProductNo = new OrdersModel();

            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string productNo = "";
            productNo = txtProductNo.Text;
            orderSearchByProductNo = OrdersController.SelectByProductNo(productNo);
            if (orderSearchByProductNo == null)
            {
                MessageBox.Show("Mã Đơn Hàng Không Đúng.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            outputList = OutputtingController.SelectByProductNo(productNo);

            List<CartonNumberingDetailModel> totalCartonList = new List<CartonNumberingDetailModel>();
            totalCartonList = CartonNumberingDetailController.SelectByProductNo(productNo);

            tblTotalOutpput.Text = outputList.Count().ToString();
            tblTotalCarton.Text = totalCartonList.Count().ToString();
            tblPass.Text = outputList.Where(w => w.DifferencePercent >= -1 && w.DifferencePercent <= 1).Count().ToString();
            tblHi.Text = outputList.Where(w => w.DifferencePercent > 1).Count().ToString();
            tblLow.Text = outputList.Where(w => w.DifferencePercent < -1).Count().ToString();

            // Add Output
            foreach (OutputtingModel model in outputList)
            {
                string status = "";
                if (model.DifferencePercent >= -1 && model.DifferencePercent <= 1)
                {
                    status = "Pass";
                }
                if (model.DifferencePercent < -1)
                {
                    status = "Low";
                }
                if (model.DifferencePercent > 1)
                {
                    status = "Hi";
                }

                OutputDetail outputDetail = new OutputDetail()
                {
                    ProductNo = model.ProductNo,
                    Barcode = model.Barcode,
                    CartonNo = model.CartonNo,
                    SizeNo = model.SizeNo,
                    GrossWeight = model.GrossWeight,
                    ActualWeight = model.ActualWeight,
                    DifferencePercent = model.DifferencePercent,
                    CreatedAccount = model.CreatedAccount,
                    Status = status,
                    OutputDate = model.CreatedTime.ToShortDateString(),
                };
                outputDetailList.Add(outputDetail);
                allOutputDetailList.Add(outputDetail);
            }

            // Add not output
            List<CartonNumberingDetailModel> cartonListNotOutput = new List<CartonNumberingDetailModel>();
            cartonListNotOutput = CartonNumberingDetailController.SelectCartonNoNotOutput(productNo);
            foreach (CartonNumberingDetailModel model in cartonListNotOutput)
            {
                OutputDetail outputDetail = new OutputDetail()
                {
                    ProductNo = model.ProductNo,
                    CartonNo = model.CartonNo,
                    SizeNo = model.SizeNo,
                };
                notOutputDetailList.Add(outputDetail);
                allOutputDetailList.Add(outputDetail);
            }

            dgOutput.ItemsSource = allOutputDetailList;
        }

        private void radShoeOuput_Checked(object sender, RoutedEventArgs e)
        {
            dgOutput.ItemsSource = null;
            dgOutput.ItemsSource = outputDetailList;
            dgOutput.Items.Refresh();
        }

        private void radShowNotOutput_Checked(object sender, RoutedEventArgs e)
        {
            dgOutput.ItemsSource = null;
            dgOutput.ItemsSource = notOutputDetailList;
            dgOutput.Items.Refresh();
        }

        private void radShowAll_Checked(object sender, RoutedEventArgs e)
        {
            dgOutput.ItemsSource = null;
            dgOutput.ItemsSource = allOutputDetailList;
            dgOutput.Items.Refresh();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProductNo.Focus();
        }
    }
    public class OutputDetail
    {
        public string ProductNo { get; set; }
        public string Barcode { get; set; }
        public int CartonNo { get; set; }
        public string SizeNo { get; set; }
        public double GrossWeight { get; set; }
        public double ActualWeight { get; set; }
        public double DifferencePercent { get; set; }
        public string CreatedAccount { get; set; }
        public string Status { get; set; }
        public string OutputDate { get; set; }
    }
}
