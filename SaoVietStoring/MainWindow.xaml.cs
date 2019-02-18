using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using SaoVietStoring.Controllers;
using SaoVietStoring.Helpers;
using SaoVietStoring.Models;
using SaoVietStoring.Views;


namespace SaoVietStoring
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker threadLogin;
        BackgroundWorker bwLoad;
        AccountModel accountTranfer;
        AccountModel account;
        string factory = "SAO VIET";

        ElectricScaleProfile electricScaleProfile;
        public MainWindow()
        {
            accountTranfer = new AccountModel();
            threadLogin = new BackgroundWorker();
            account = new AccountModel();
            threadLogin.DoWork += threadLogin_DoWork;
            threadLogin.RunWorkerCompleted += threadLogin_RunWorkerCompleted;

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            electricScaleProfile = new ElectricScaleProfile();

            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            imgBackground.Visibility = Visibility.Collapsed;
            gridLogin.Visibility = Visibility.Visible;
            if (bwLoad.IsBusy == true)
            {
                return;
            }

            txtUserName.Focus();
            this.Cursor = Cursors.Wait;
            bwLoad.RunWorkerAsync();

            int profileId = 0;
            int.TryParse(AppSettingsHelper.ReadSetting("ElectricScaleProfile"), out profileId);

            if (String.IsNullOrEmpty(AppSettingsHelper.ReadSetting("Factory").ToUpper()) == false)
            {
                factory = AppSettingsHelper.ReadSetting("Factory").ToUpper();
            }

            this.Title = String.Format("{0} - Storing System", factory);
            electricScaleProfile = ElectricScaleProfileHelper.ElectricScaleProfileList().Where(p => p.ProfileId == profileId).FirstOrDefault();
            if (electricScaleProfile == null)
            {
                this.Close();
            }
        }

        bool connectionStatus = false;
        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Helpers.CheckConnection.Exist() == true)
            {
                connectionStatus = true;
            }
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (connectionStatus == true)
            {
                txtUserName.IsEnabled = true;
                txtPassword.IsEnabled = true;
                txtUserName.Focus();
            }
            else
            {
                tblConnectionStatus.Text = "Not Connected !";
                tblConnectionStatus.Foreground = Brushes.White;
                backgroundStatusConnection.Background = Brushes.Red;
                return;
            }
        }

        private void miInput_Click(object sender, RoutedEventArgs e)
        {
            InputWindow window = new InputWindow(account, electricScaleProfile, factory);
            window.Show();
        }
        private void miOutput_Click(object sender, RoutedEventArgs e)
        {
            OutputWindow window = new OutputWindow(account, electricScaleProfile, factory);
            window.Show();
        }

        private void btnOKLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUserName.Text;
            if (string.IsNullOrEmpty(username) == true)
            {
                return;
            }
            string password = txtPassword.Password;
            if (string.IsNullOrEmpty(password) == true)
            {
                return;
            }
            if (threadLogin.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            btnOKLogin.IsEnabled = false;
            threadLogin.RunWorkerAsync(new object[] { username, password });
        }
        private void threadLogin_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] arguments = e.Argument as object[];
            string username = arguments[0] as string;
            string password = arguments[1] as string;
            e.Result = AccountController.Select(username, password);
        }
        private void threadLogin_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnOKLogin.IsEnabled = true;
            if (e.Cancelled == true || e.Error != null)
            {
                MessageBox.Show("Unknow Error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            account = e.Result as AccountModel;

            if (account == null)
            {
                MessageBox.Show("Đăng Nhập Thất Bại !!!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (account.ElectricScaleId != electricScaleProfile.ProfileId)
            {
                MessageBox.Show("Đăng Nhập Thất Bại !!!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            accountTranfer = account;
            txtPassword.Password = "";
            MessageBox.Show(String.Format("Welcome , {0}!!!", account.FullName), "Welcome", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Title = String.Format("{0} Storing System - User: {1} - Version: {2}", factory, account.FullName, AssemblyHelper.Version());
            miInput.IsEnabled = true;
            miOutput.IsEnabled = true;

            miImportPORepacking.IsEnabled = true;
            miImportPORepacking.Foreground = Brushes.Red;
            miReWeighCarton.IsEnabled = true;
            miReWeighCarton.Foreground = Brushes.Blue;
            gridLogin.Visibility = Visibility.Collapsed;
            imgBackground.Visibility = Visibility.Visible;

        }

        private void btnCloseLogin_Click(object sender, RoutedEventArgs e)
        {
            imgBackground.Visibility = Visibility.Visible;
            gridLogin.Visibility = Visibility.Collapsed;
            btnReLogin.Visibility = Visibility.Visible;
            btnReLogin.IsEnabled = true;
        }

        private void miAbout_Click(object sender, RoutedEventArgs e)
        {
            string about = "Project: Storing System\n"
                + string.Format("Current Version: {0}\n", AssemblyHelper.Version())
                + "Created By: It SaoViet\n"
                + "Contact:  Phone: 0988 471 934";
            MessageBox.Show(about, "About Me", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void miVersion_Click(object sender, RoutedEventArgs e)
        {
            string updateHistory = "Software Update History\n" +
                "- 1.0.0.0:" + " Primitive software\n" +
                "- 1.1.1.5:" + " Update: Override weight\n" +
                "- 1.1.1.6:" + " Update: New db\n" +
                "- 1.1.1.7:" + " Update: Input SubPO\n" +
                "- 1.1.1.8:" + " Update: Search Carton Detail Information\n" +
                "- 1.1.1.9:" + " Update: Detail Report\n" +
                "- 2.0.0.2:" + " Revise: Input Barcode\n" +
                "- 2.0.0.3:" + " Update: Import PO Repacking\n" +
                "- 2.0.0.5:" + " Update: Reweigh Carton\n" +
                "- 2.0.0.8:" + " Update: Output Carton\n" +
                "- 2.0.0.9:" + " Revise: PO Repacking\n" +
                "- 2.0.1.0:" + " Update: PrintBarcode, Scan BarcodeTemp.\n" +
                "- 2.0.1.1:" + " Update: Factory Email.";

            MessageBox.Show(updateHistory, string.Format("Current Version: {0}", AssemblyHelper.Version()), MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void miInputReport_Click(object sender, RoutedEventArgs e)
        {
            StoringReportWindow window = new StoringReportWindow();
            window.Show();
        }

        private void miSearch_Click(object sender, RoutedEventArgs e)
        {
            //CartonInformationDetailWindow window = new CartonInformationDetailWindow();
            //window.Show();
        }

        private void btnReLogin_Click(object sender, RoutedEventArgs e)
        {
            gridLogin.Visibility = Visibility.Visible;
            imgBackground.Visibility = Visibility.Collapsed;
            txtUserName.Focus();
            btnOKLogin.IsEnabled = true;
            btnOKLogin.IsDefault = true;
            btnReLogin.Visibility = Visibility.Collapsed;
        }

        private void miDetailReport_Click(object sender, RoutedEventArgs e)
        {
            InputDetailReportWindow window = new InputDetailReportWindow();
            window.Show();
        }

        private void miImportPORepacking_Click(object sender, RoutedEventArgs e)
        {
            ImportPORepackingWindow window = new ImportPORepackingWindow();
            window.Show();
        }

        private void miReWeighCarton_Click(object sender, RoutedEventArgs e)
        {
            ReWeighCartonWindow window = new ReWeighCartonWindow();
            window.Show();
        }

        private void miPrintBarcode_Click(object sender, RoutedEventArgs e)
        {
            PrintBarcodeWindow window = new PrintBarcodeWindow();
            window.ShowDialog();
        }
    }
}
