using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using SaoVietStoring.Controllers;
using SaoVietStoring.Models;
using SaoVietStoring.Helpers;
using System.Net.Mail;
using System.Net;
using System.ComponentModel;
using System.Threading;

namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for CheckIssuesWindow.xaml
    /// </summary>
    public partial class CheckIssuesWindow : Window
    {
        List<IssuesModel> issuesList;
        ControlIssuesAccountModel findControlAccountModel;
        StoringModel receiveStoringModel;
        OutputingModel receiveOutputModel;

        public StoringModel storingCurrent;
        public OutputingModel outputingCurrent;

        List<MailAddressReceiveMessageModel> mailAddressReceiveMessageList;
        SmtpClient smtpClient;
        MailMessage mailMessage;
        bool flagSending;
        IssuesType.Issues issuesType;
        ElectricScaleProfile electricScaleProfile;
        BackgroundWorker bwInsertAndSendEmail;
        string factory;

        public CheckIssuesWindow(OutputingModel _receiveOutputModel, StoringModel _receiveStoringModel, IssuesType.Issues _issuesType, string factory)
        {
            this.receiveStoringModel = _receiveStoringModel;
            this.receiveOutputModel = _receiveOutputModel;
            this.issuesType = _issuesType;
            this.factory = factory;
            issuesList = new List<IssuesModel>();
            electricScaleProfile = new ElectricScaleProfile();
            findControlAccountModel = new ControlIssuesAccountModel();

            storingCurrent = new StoringModel();
            outputingCurrent = new OutputingModel();

            mailAddressReceiveMessageList = new List<MailAddressReceiveMessageModel>();
            MailAddress mailAddressSend = new MailAddress("storingsystem.chungphi@gmail.com", factory);
            smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(mailAddressSend.Address, "Happy2018"),
                Timeout = 10 * 1000,
            };
            smtpClient.SendCompleted += new SendCompletedEventHandler(smtpClient_SendCompleted);
            mailMessage = new MailMessage
            {
                From = mailAddressSend,
                IsBodyHtml = true,
                Subject = "Storing-System",
            };
            flagSending = false;

            bwInsertAndSendEmail = new BackgroundWorker();
            bwInsertAndSendEmail.DoWork +=new DoWorkEventHandler(bwInsertAndSendEmail_DoWork);
            bwInsertAndSendEmail.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwInsertAndSendEmail_RunWorkerCompleted);

            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int profileId = 0;
            int.TryParse(AppSettingsHelper.ReadSetting("ElectricScaleProfile"), out profileId);
            electricScaleProfile = ElectricScaleProfileHelper.ElectricScaleProfileList().Where(p => p.ProfileId == profileId).FirstOrDefault();
            //gridIssues.Children.Clear();
            issuesList = IssuesController.Select();
            txtSecurityCode.Focus();
            if (receiveOutputModel != null)
            {
                this.Title = "Storing System - OUTPUT Report";
            }
            if (receiveStoringModel != null)
            {
                if (issuesType == IssuesType.Issues.IssuesCompareWeight)
                {
                    this.Title = "Storing System - INPUT Report - Weight Problem";
                }
                if (issuesType == IssuesType.Issues.IssuesFirstCartonOfSizeOfPO)
                {
                    this.Title = "Storing System - INPUT Report - First Carton Problem";
                }
            }

            StackPanel stack = new StackPanel();
            stack.Orientation = Orientation.Vertical;
            for (int i = 0; i < issuesList.Count; i++)
            {
                RadioButton rad = new RadioButton();
                rad.Margin = new Thickness(0, 15, 0, 0);
                rad.GroupName = "Issues";
                rad.Tag = i + 1;
                rad.Content = issuesList[i].IssuesName.ToString();
                rad.Foreground = Brushes.Red;
                rad.Click += new RoutedEventHandler(rad_Click);
                stack.Children.Add(rad);
            }
            gridIssues.Children.Add(stack);
        }

        int issuesId = 0;
        private void rad_Click(object sender, RoutedEventArgs e)
        {
            RadioButton rad = sender as RadioButton;
            issuesId = (int)rad.Tag;
            if (issuesId == 0)
            {
                MessageBox.Show("Chọn vấn đề của thùng!\nPlease Choose a Reason!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (bwInsertAndSendEmail.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            gridIssues.IsEnabled = false;
            btnUpdateWeight.IsEnabled = false;
            bwInsertAndSendEmail.RunWorkerAsync();
        }

        int mode = 0;
        private void bwInsertAndSendEmail_DoWork(object sender, DoWorkEventArgs e)
        {
            // Send Email
            mailAddressReceiveMessageList = MailAddressReceiveMessageController.Get();
            foreach (MailAddressReceiveMessageModel mailAddressReceiveMessage in mailAddressReceiveMessageList)
            {
                MailAddress mailAddressReceive = new MailAddress(mailAddressReceiveMessage.MailAddress, mailAddressReceiveMessage.Name);
                mailMessage.To.Add(mailAddressReceive);
            }

            string reason = "";
            reason = issuesList.Where(w => w.IssuesId == issuesId).Select(s => s.IssuesName).FirstOrDefault().ToString();
            string mailBody = "";

            // OUTPUT PROMBLEM
            if (receiveOutputModel != null)
            {
                mode = 1;
                receiveOutputModel.IssuesId = issuesId;
                // InsertDB
                if (OutputingController.Insert(receiveOutputModel) == false)
                {
                    MessageBox.Show("Insert Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                //CreatedLog
                string logBodyOldCarton = "Production No.: {0}, Size No.: {1}, Carton No.: {2}, Gross Weight: {3}kg, Actual Weight: {4}kg, Difference Percent: {5}%, Created By: {6}, Reason :{7}, Location {8}";
                LogHelper.CreateOutputLog(string.Format(logBodyOldCarton, receiveOutputModel.ProductNo, receiveOutputModel.SizeNo, receiveOutputModel.CartonNo, receiveOutputModel.GrossWeight, receiveOutputModel.ActualWeight, receiveOutputModel.DifferencePercent, findControlAccountModel.FullName, reason, electricScaleProfile.Location));

                // Created Email
                mailBody = @"<html><table border='1' style='width:100%'>
                                    <tr><td>ProductNo</td><td>SizeNo</td><td>CartonNo</td><td>Gross Weight</td><td>Actual Weight</td><td>Difference Percent</td><td>Check By</td><td>Location</td><td>Reason</td></tr>
                                    <tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3} kg</td><td>{4} kg</td><td>{5} %</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>
                                    </table></html>";
                //string logBody = "Production No.:{0} Size No.:{1} Carton No.:{2} Gross Weight:{3}kg Actual Weight: {4}kg Difference Percent:{5}% Check by:{6} Reason:{7}";
                mailMessage.Subject = string.Format("STORING SYSTEM - OUTPUT PROMBLEM");
                mailMessage.Body = string.Format(mailBody, receiveOutputModel.ProductNo, receiveOutputModel.SizeNo, receiveOutputModel.CartonNo, receiveOutputModel.GrossWeight, receiveOutputModel.ActualWeight, receiveOutputModel.DifferencePercent, findControlAccountModel.FullName, electricScaleProfile.Location, reason);

                outputingCurrent = receiveOutputModel;
            }

            // INPUT PROBLEM
            if (receiveStoringModel != null)
            {
                mode = 2;

                receiveStoringModel.IssuesId = issuesId;
                if (StoringController.Insert(receiveStoringModel) == false)
                {
                    MessageBox.Show("Insert Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (issuesType == IssuesType.Issues.IssuesFirstCartonOfSizeOfPO)
                {
                    mailBody = @"<html><table border='1' style='width:100%'>
                                    <tr><td>ProductNo</td><td>SizeNo</td><td>CartonNo</td><td>Check By</td><td>Location</td><td>Reason</td></tr>
                                    <tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>
                                    </table></html>";
                    mailMessage.Body = string.Format(mailBody, receiveStoringModel.ProductNo,  receiveStoringModel.SizeNo, receiveStoringModel.CartonNo, findControlAccountModel.FullName, electricScaleProfile.Location ,reason);
                    //string logBody = "Production No.:{0} Size No.:{1} Carton No.:{2} Gross Weight:{3}kg Actual Weight: {4}kg Difference Percent:{5}% Check by:{6} Reason:{7}";
                    mailMessage.Subject = string.Format("STORING SYSTEM - INPUT PROBLEM - FIRST CARTON");
                }

                if (issuesType == IssuesType.Issues.IssuesCompareWeight)
                {
                    mailBody = @"<html><table border='1' style='width:100%'>
                                    <tr><td>ProductNo</td><td>SizeNo</td><td>CartonNo</td><td>Gross Weight</td><td>Actual Weight</td><td>Difference Percent</td><td>Check By</td><td>Location</td><td>Reason</td></tr>
                                    <tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3} kg</td><td>{4} kg</td><td>{5} %</td><td>{6}</td><td>{7}</td><td>{8}</td></tr>
                                    </table></html>";
                    //string logBody = "Production No.:{0} Size No.:{1} Carton No.:{2} Gross Weight:{3}kg Actual Weight: {4}kg Difference Percent:{5}% Check by:{6} Reason:{7}";
                    mailMessage.Subject = string.Format("STORING SYSTEM - INPUT PROMBLEM - WEIGHT");
                    mailMessage.Body = string.Format(mailBody, receiveStoringModel.ProductNo, receiveStoringModel.SizeNo, receiveStoringModel.CartonNo, receiveStoringModel.GrossWeight, receiveStoringModel.ActualWeight, receiveStoringModel.DifferencePercent, findControlAccountModel.FullName, electricScaleProfile.Location ,reason);

                    string logBodyOldCarton = "Production No.: {0}, Size No.: {1}, Carton No.: {2}, Gross Weight: {3}kg, Actual Weight: {4}kg, Difference Percent: {5}%, Created By: {6}, Reason :{7}, Location {8}";
                    LogHelper.CreateIssuesLog(string.Format(logBodyOldCarton, receiveStoringModel.ProductNo, receiveStoringModel.SizeNo, receiveStoringModel.CartonNo, receiveStoringModel.GrossWeight, receiveStoringModel.ActualWeight, receiveStoringModel.DifferencePercent, findControlAccountModel.FullName, reason, electricScaleProfile.Location));
                }
                storingCurrent = receiveStoringModel;
            }

            if (flagSending == false && mailMessage.To.Count > 0)
            {
                if (CheckInternetConnection.CheckConnection() == false)
                {
                    MessageBox.Show("Không có kết nối Internet!\nNo Internet Connection!", "Info", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                smtpClient.SendAsync(mailMessage, mailMessage);
                flagSending = true;
                MessageBox.Show("Đã gửi Email!\nSent Email!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void bwInsertAndSendEmail_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show("Error occurred!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            if (issuesType == IssuesType.Issues.IssuesCompareWeight)
            {
                btnUpdateWeight.IsEnabled = true;
            }
            this.Cursor = null;
            gridIssues.IsEnabled = true;
        }

        private void btnAccecpt_Click(object sender, RoutedEventArgs e)
        {
            int securityCode = 0;
            int.TryParse(txtSecurityCode.Password, out securityCode);
            findControlAccountModel = ControlIssuesAccountController.FindSecurityCode(securityCode);
            if (findControlAccountModel == null)
            {
                MessageBox.Show("Wrong SecurityCode !", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSecurityCode.Focus();
                txtSecurityCode.SelectAll();
                return;
            }
            tblStatus.Text = "Welcome  " + findControlAccountModel.FullName + "!!";
            if (issuesType == IssuesType.Issues.IssuesCompareWeight)
            {
                btnUpdateWeight.IsEnabled = true;
            }
            groupChooseReason.Visibility = Visibility.Visible;
            gridIssues.Visibility = Visibility.Visible;
        }

        private void btnUpdateWeight_Click(object sender, RoutedEventArgs e)
        {
            if (mode == 1)
            {
                txtPO.Text = receiveOutputModel.ProductNo;
                txtBarcode.Text = receiveOutputModel.Barcode;
                txtCartonNo.Text = receiveOutputModel.CartonNo.ToString();
                txtSizeNo.Text = receiveOutputModel.SizeNo;
                txtActualWeight.Text = receiveOutputModel.ActualWeight.ToString();
            }
            if (mode == 2)
            {
                txtPO.Text = receiveStoringModel.ProductNo;
                txtBarcode.Text = receiveStoringModel.Barcode;
                txtCartonNo.Text = receiveStoringModel.CartonNo.ToString();
                txtSizeNo.Text = receiveStoringModel.SizeNo;
                txtActualWeight.Text = receiveStoringModel.ActualWeight.ToString();
            }

            groupUpdateCarton.Visibility = Visibility.Visible;
            groupUpdateCarton.IsEnabled = true;
            btnSave.IsEnabled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            // Input
            double actualWeight = 0;
            int cartonNo = 0;
            Double.TryParse(txtActualWeight.Text.ToString(), out actualWeight);
            Int32.TryParse(txtCartonNo.Text.ToString(), out cartonNo);
            string sizeNo = txtSizeNo.Text.ToString();

            // Output

            if (mode == 1)
            {
                var overrideOutputCarton = receiveOutputModel;
                overrideOutputCarton.ActualWeight = actualWeight;
                overrideOutputCarton.CartonNo = cartonNo;
                overrideOutputCarton.SizeNo = sizeNo;
                overrideOutputCarton.IsPass = true;
                overrideOutputCarton.GrossWeight = 0;
                overrideOutputCarton.DifferencePercent = 0;
                overrideOutputCarton.IssuesId = 0;
                if (OutputingController.Insert(overrideOutputCarton) == false)
                {
                    MessageBox.Show("Error occurred!", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                outputingCurrent = overrideOutputCarton;
            }

            if (mode == 2)
            {
                var overrideStoringCarton = receiveStoringModel;

                overrideStoringCarton.ActualWeight = actualWeight;
                overrideStoringCarton.CartonNo = cartonNo;
                overrideStoringCarton.SizeNo = sizeNo;
                overrideStoringCarton.IsPass = true;
                overrideStoringCarton.GrossWeight = 0;
                overrideStoringCarton.DifferencePercent = 0;
                overrideStoringCarton.IssuesId = 0;
                if (StoringController.Insert(overrideStoringCarton) == false)
                {
                    MessageBox.Show("Error occurred!", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                storingCurrent = overrideStoringCarton;
            }
            MessageBox.Show("Thùng Đã Được Pass!\nPASSED!", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
            btnSave.IsEnabled = false;
            groupUpdateCarton.IsEnabled = false;

            Thread.Sleep(2000);
            this.Close();
        }

        private void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            flagSending = false;
        }
    }
}
