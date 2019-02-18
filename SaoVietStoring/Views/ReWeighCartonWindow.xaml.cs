using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;

using SaoVietStoring.Models;
using SaoVietStoring.Controllers;
using SaoVietStoring.Helpers;
using System.IO.Ports;
using System.Threading;

namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for ReWeighCartonWindow.xaml
    /// </summary>
    public partial class ReWeighCartonWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwSave;
        StoringModel storingPerBarcode;
        string portReceive;
        SerialPort serialPortReceive;
        ElectricScaleProfile electricScaleProfile;
        ControlIssuesAccountModel controlAccount;
        public ReWeighCartonWindow()
        {
            storingPerBarcode = new StoringModel();
            controlAccount = new ControlIssuesAccountModel();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwSave = new BackgroundWorker();
            bwSave.DoWork += new DoWorkEventHandler(bwSave_DoWork);
            bwSave.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwSave_RunWorkerCompleted);

            electricScaleProfile = new ElectricScaleProfile();

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int profileId = 0;
            int.TryParse(AppSettingsHelper.ReadSetting("ElectricScaleProfile"), out profileId);
            electricScaleProfile = ElectricScaleProfileHelper.ElectricScaleProfileList().Where(p => p.ProfileId == profileId).FirstOrDefault();
            if (electricScaleProfile == null)
            {
                this.Close();
            }
            serialPortReceive = new SerialPort();
            serialPortReceive.BaudRate = electricScaleProfile.BaudRate;
            serialPortReceive.DataReceived += new SerialDataReceivedEventHandler(serialPortReceive_DataReceived);
            portReceive = AppSettingsHelper.ReadSetting("ReceivePort");

            string[] portList = SerialPort.GetPortNames();
            if (portList.Count() > 0)
            {
                if (string.IsNullOrEmpty(portReceive) == true || portList.Contains(portReceive) == false)
                {
                    portReceive = portList[0];
                }
                serialPortReceive.PortName = portReceive;
            }

            txtBarcode.Focus();
        }


        string barcode = "";
        private void btnBarcode_Click(object sender, RoutedEventArgs e)
        {
            barcode = txtBarcode.Text;

            txtProductNo.Text = "";
            txtSizeNo.Text = "";
            txtCartonNo.Text = "";

            txtNewWeight.Text = "0";
            txtOldWeight.Text = "0";

            if (barcode == "")
            {
                txtBarcode.BorderBrush = Brushes.Red;
                txtBarcode.BorderThickness = new Thickness(2, 2, 2, 2);
                return;
            }
            txtBarcode.BorderBrush = Brushes.Black;
            txtBarcode.BorderThickness = new Thickness(1, 1, 1, 1);

            if (bwLoad.IsBusy == true)
            {
                return;
            }

            this.Cursor = Cursors.Wait;
            bwLoad.RunWorkerAsync();
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            storingPerBarcode = StoringController.SelectByBarcode(barcode);
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (storingPerBarcode == null)
            {
                MessageBox.Show("Not Found !", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                txtBarcode.Focus();
                txtBarcode.SelectAll();
                return;
            }

            txtProductNo.Text = String.Format("ProductNo: {0}", storingPerBarcode.ProductNo);
            txtSizeNo.Text = String.Format("SizeNo: {0}", storingPerBarcode.SizeNo);
            txtCartonNo.Text = String.Format("CartonNo: {0}", storingPerBarcode.CartonNo);
            txtOldWeight.Text = storingPerBarcode.ActualWeight.ToString();

            stkConfirm.Visibility = Visibility.Collapsed;
            btnWeight.IsEnabled = true;
            btnSave.IsEnabled = false;
        }

        private void btnWeight_Click(object sender, RoutedEventArgs e)
        {
            stkConfirm.Visibility = Visibility.Collapsed;

            if (serialPortReceive.IsOpen == true)
            {
                serialPortReceive.Close();
                txtNewWeight.Text = "0";
                txtNewWeight.Tag = 0;
            }
            try
            {
                serialPortReceive.Open();
                btnWeight.IsEnabled = false;
                txtPassword.Focus();
                btnSave.IsEnabled = true;
            }
            catch
            {
                MessageBox.Show(string.Format("Có Lỗi Xảy Ra Khi Lấy Cân Nặng."), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                serialPortReceive.Close();
                txtNewWeight.Text = "0";
                txtNewWeight.Tag = 0;
            }
        }

        private void serialPortReceive_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(100);
            if (serialPortReceive.IsOpen == true)
            {
                string dataReceived = ElectricScaleProfileHelper.ConvertWeight(serialPortReceive.ReadExisting(), electricScaleProfile);
                if (string.IsNullOrEmpty(dataReceived) == false)
                {
                    double actualWeight = 0;
                    if (double.TryParse(dataReceived, out actualWeight) == false)
                    {
                        //Alert Here.
                    }
                    if (actualWeight > 0)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            txtNewWeight.Text = string.Format("{0}", actualWeight);
                            txtNewWeight.Tag = actualWeight;

                            btnWeight.IsEnabled = true;
                            stkConfirm.Visibility = Visibility.Visible;
                        }));
                        serialPortReceive.Close();
                    }
                }
            }
        }

        int passWord = 0;
        double newWeight = 0;
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(txtPassword.Password.ToString(), out passWord);
            Double.TryParse(txtNewWeight.Tag.ToString(), out newWeight);
            if (bwSave.IsBusy == true || passWord == 0)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            btnSave.IsEnabled = false;
            bwSave.RunWorkerAsync();
        }

        private void bwSave_DoWork(object sender, DoWorkEventArgs e)
        {
            controlAccount = ControlIssuesAccountController.FindSecurityCode(passWord);
        }

        private void bwSave_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (controlAccount == null)
            {
                MessageBox.Show(string.Format("Wrong Security Code !"), "Infor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPassword.Focus();
                txtPassword.SelectAll();
                btnSave.IsEnabled = true;

                return;
            }

            //Create Log
            string logBodyOldCarton = "Production No.: {0}, Size No.: {1}, Carton No.: {2}, Gross Weight: {3}kg, Actual Weight: {4}kg, Difference Percent: {5}%, Created By: {6}";
            LogHelper.CreateLog(string.Format(logBodyOldCarton, storingPerBarcode.ProductNo, storingPerBarcode.SizeNo, storingPerBarcode.CartonNo, storingPerBarcode.GrossWeight, storingPerBarcode.ActualWeight, storingPerBarcode.DifferencePercent, controlAccount.FullName));

            // Update DB
            StoringModel reWeighModel = new StoringModel();
            reWeighModel = storingPerBarcode;
            reWeighModel.ActualWeight = newWeight;

            if (StoringController.Insert(reWeighModel) == true)
            {
                Thread.Sleep(1500);
                MessageBox.Show(string.Format("Saved !"), "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(string.Format("An Error Occur!"), "Infor", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            this.Cursor = null;
            btnSave.IsEnabled = true;

            txtProductNo.Text = "";
            txtSizeNo.Text = "";
            txtCartonNo.Text = "";

            txtNewWeight.Text = "0";
            txtOldWeight.Text = "0";

            txtBarcode.Focus();
            txtBarcode.SelectAll();
            btnBarcode.IsEnabled = true;
            btnBarcode.IsDefault = true;

            txtPassword.Password = "";
            stkConfirm.Visibility = Visibility.Collapsed;

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            serialPortReceive.Close();
        }
    }
}
