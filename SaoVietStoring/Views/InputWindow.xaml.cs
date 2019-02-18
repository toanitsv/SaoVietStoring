using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

using SaoVietStoring.Controllers;
using SaoVietStoring.Helpers;
using SaoVietStoring.Models;

namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>
    public partial class InputWindow : Window
    {
        AccountModel account;
        DispatcherTimer dispatcherTimer;
        DispatcherTimer dispatcherTimerSendReport;

        string portReceive;
        SerialPort serialPortReceive;
        string portWrite;
        ElectricScaleProfile electricScaleProfile;

        string barcodeValidateString = "";
        string factory;

        // DEFAULT VALUE
        int MAX_LENGHT = 20, FIRST_SERIAL = 10, SERIAL_LENGHT = 9;
        double LIMITED_MIN = 0.95, LIMITED_MAX = 1.05;
        int createReportHour = 17, createReportMinute = 30, createReportSecond = 0;

        List<StoringCurrent> storingCurrentList;
        List<StoringModel> storingWeighedList;
        List<StoringModel> storingWeighedToDayList;
        List<OutputingModel> outputtedList;
        List<StoringTemp> storingTempList;

        BackgroundWorker bwLoad;

        List<MailAddressReceiveMessageModel> mailAddressReceiveMessageList;
        SmtpClient smtpClient;
        MailMessage mailMessage;
        bool flagSending;

        public InputWindow(AccountModel _account, ElectricScaleProfile electricScaleProfile, string factory)
        {
            this.account = _account;
            this.electricScaleProfile = electricScaleProfile;
            this.factory = factory;
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimerSendReport = new DispatcherTimer();

            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();

            storingCurrentList = new List<StoringCurrent>();
            storingWeighedList = new List<StoringModel>();
            storingWeighedToDayList = new List<StoringModel>();
            outputtedList = new List<OutputingModel>();
            storingTempList = new List<StoringTemp>();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);


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
                Subject = "STORING SYSTEM - DAILY REPORT",
            };
            flagSending = false;

            InitializeComponent();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            tblStatusTime.Text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);
        }

        string fullTitle = "", tempTitle = "";
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fullTitle = string.Format("{0} {1} - Version: {2} / User: {3} / Location: {4}", factory, this.Title, AssemblyHelper.Version(), account.FullName, electricScaleProfile.Location);
            this.Title = fullTitle;
            brComplete.Visibility = Visibility.Visible;

            serialPortReceive = new SerialPort();
            serialPortReceive.BaudRate = electricScaleProfile.BaudRate;
            serialPortReceive.DataReceived += new SerialDataReceivedEventHandler(serialPortReceive_DataReceived);

            portReceive = AppSettingsHelper.ReadSetting("ReceivePort");
            portWrite = AppSettingsHelper.ReadSetting("WritePort");

            barcodeValidateString = AppSettingsHelper.ReadSetting("BarcodeValidate");
            if (String.IsNullOrEmpty(barcodeValidateString) == false)
            {
                string[] barcodeValidateSplit = barcodeValidateString.Split(',');
                if (barcodeValidateSplit.Count() == 3)
                {
                    Int32.TryParse(barcodeValidateSplit[0].ToString(), out MAX_LENGHT);
                    Int32.TryParse(barcodeValidateSplit[1].ToString(), out FIRST_SERIAL);
                    Int32.TryParse(barcodeValidateSplit[2].ToString(), out SERIAL_LENGHT);
                }
            }

            string limitString = AppSettingsHelper.ReadSetting("LimitedValue");
            if (String.IsNullOrEmpty(limitString) == false)
            {
                string[] limitSplit = limitString.Split(',');
                if (limitSplit.Count() == 2)
                {
                    Double.TryParse(limitSplit[0], out LIMITED_MIN);
                    Double.TryParse(limitSplit[1], out LIMITED_MAX);
                }
            }

            string createdReportTimeString = AppSettingsHelper.ReadSetting("CreateReportTime");
            if (String.IsNullOrEmpty(createdReportTimeString) == false)
            {
                string[] createdReportTimeSplit = createdReportTimeString.Split(',');
                if (createdReportTimeSplit.Count() == 3)
                {
                    Int32.TryParse(createdReportTimeSplit[0].ToString(), out createReportHour);
                    Int32.TryParse(createdReportTimeSplit[1].ToString(), out createReportMinute);
                    Int32.TryParse(createdReportTimeSplit[2].ToString(), out createReportSecond);
                }
            }

            string[] portList = SerialPort.GetPortNames();
            if (portList.Count() > 0)
            {
                if (string.IsNullOrEmpty(portReceive) == true || portList.Contains(portReceive) == false)
                {
                    portReceive = portList[0];
                }
                if (string.IsNullOrEmpty(portWrite) == true || portList.Contains(portWrite) == false)
                {
                    portWrite = portList[0];
                }
                serialPortReceive.PortName = portReceive;
            }

            if (bwLoad.IsBusy == false)
            {
                this.Cursor = Cursors.Wait;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            storingWeighedList = StoringController.SelectIsNotLoad();
            outputtedList = OutputingController.SelectIsNotLoad();

            int storingIndex = 1;
            foreach (StoringModel storing in storingWeighedList)
            {
                var current = new StoringCurrent() {
                    StoringModel = storing,
                };
                storingCurrentList.Add(current);

                Dispatcher.Invoke(new Action(() =>
                {
                    tempTitle = string.Format("Downloading Carton: {0} / {1}", storingIndex, storingWeighedList.Count);
                    this.Title = tempTitle;
                }));

                storingIndex++;
            }

            // If this carton was outputted, allow reweigh
            if (outputtedList.Count > 0)
            {
                var barcodeOutputedList = outputtedList.Select(s => s.Barcode).Distinct().ToList();
                storingCurrentList.RemoveAll(r => barcodeOutputedList.Contains(r.StoringModel.Barcode));
            }

            var POScannedList = storingWeighedList.Where(w => w.Barcode.Contains(",") == false).Select(s => s.ProductNo).Distinct().ToList();

            // Create barcode temp
            int poCreatedIndex = 1;
            foreach (var PO in POScannedList)
            {
                var cartonNumberingDetail_POScannedList = CartonNumberingDetailController.Select(PO);
                // If total carton current in po equals total carton in cartonnumberingdetail. PO will be ignore.
                if (storingWeighedList.Where(w => w.ProductNo == PO).Count() == cartonNumberingDetail_POList.Count())
                {
                    poCreatedIndex++;
                    continue;
                }
                var firstStoringWeighedList = storingWeighedList.Where(w => w.ProductNo == PO).FirstOrDefault();
                int serialCarton_0 = ConvertBarcodeToSerial(firstStoringWeighedList.Barcode) - firstStoringWeighedList.CartonNo;
                int cartonIndex = 1;
                foreach (var cartonDetail in cartonNumberingDetail_POScannedList)
                {
                    var storingTemp = new StoringTemp()
                    {
                        SizeNo = cartonDetail.SizeNo,
                        ProductNo = cartonDetail.ProductNo,
                        CartonNo = cartonDetail.CartonNo,
                        SerialNo = serialCarton_0 + cartonDetail.CartonNo,
                    };
                    storingTempList.Add(storingTemp);

                    Dispatcher.Invoke(new Action(() =>
                    {
                        tempTitle = String.Format("Creating Barcode: {0} of {1} for {2} / {3} PO#",
                                                    cartonIndex,
                                                    cartonNumberingDetail_POScannedList.Select(s => s.CartonNo).Max(),
                                                    poCreatedIndex,
                                                    POScannedList.Count);
                        this.Title = tempTitle;
                    }));

                    cartonIndex++;
                }
                poCreatedIndex++;
            }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            this.Title = fullTitle;
            DefaultStatus();

            dispatcherTimerSendReport.Tick += new EventHandler(dispatcherTimerSendReport_Tick);
            dispatcherTimerSendReport.Start();
        }

        private void dispatcherTimerSendReport_Tick(object sender, EventArgs e)
        {
            var currentTime = DateTime.Now;
            if (currentTime.Hour == createReportHour && currentTime.Minute == createReportMinute && currentTime.Second == createReportSecond)
            {
                var storingReportPerDateList = new List<StoringModel>();
                storingReportPerDateList = StoringController.SelectByDate(DateTime.Now, account.UserName);
                if (storingReportPerDateList.Count > 0)
                {
                    // Send Email
                    mailAddressReceiveMessageList = MailAddressReceiveMessageController.Get();
                    foreach (MailAddressReceiveMessageModel mailAddressReceiveMessage in mailAddressReceiveMessageList)
                    {
                        MailAddress mailAddressReceive = new MailAddress(mailAddressReceiveMessage.MailAddress, mailAddressReceiveMessage.Name);
                        mailMessage.To.Add(mailAddressReceive);
                    }

                    string mailHeader = @"<html><table border='1' style='width:75%'>
                                    <tr><td style='width:20%'>ProductNo</td><td style='width:15%'>Total</td><td style='width:15%'>Inputted</td><td style='width:15%'>Total Inputted</td><td style='width:15%'>Balance</td><td style='width:20%'>Location</td></tr>";
                    string mailBody = "";
                    foreach (var productNo in storingReportPerDateList.Select(s => s.ProductNo).Distinct().ToList())
                    {
                        //int total = storingTempList.Where(w => w.ProductNo == productNo).Select(s => s.CartonNo).Max();
                        int total = CartonNumberingDetailController.Select(productNo).Select(s => s.CartonNo).Max();
                        int inputted = storingReportPerDateList.Where(w => w.ProductNo == productNo && w.IsPass == true && w.IsComplete == true).Count();
                        //int totalInputed = storingCurrentList.Where(w => w.StoringModel.ProductNo == productNo && w.StoringModel.IsPass == true && w.StoringModel.IsComplete == true).Count();
                        int totalInputted = StoringController.SelectPerPO(productNo).Where(w => w.IsPass == true && w.IsComplete == true).Count();
                        int balance = total - totalInputted;
                        mailBody += string.Format("<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>",
                                                    productNo, total, inputted, totalInputted, balance, electricScaleProfile.Location);
                    }
                    mailMessage.Body = mailHeader + mailBody + "</table></html>";

                    if (flagSending == false && mailMessage.To.Count > 0 && CheckInternetConnection.CheckConnection() == true)
                    {
                        smtpClient.SendAsync(mailMessage, mailMessage);
                        flagSending = true;
                        MessageBox.Show(String.Format("Đã gửi report ngày: {0:dd/MM/yyyy HH:mm:ss}", DateTime.Now), "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        string barcode = "";
        string productNo = "";
        int cartonNo = 0;
        string sizeNo = "";
        private void btnBarcodeComplete_Click(object sender, RoutedEventArgs e)
        {
            ClearValue();
            barcode = txtBarCodeComplete.Text.Trim();

            // Check Label is temporary label or OCL Barcode Label
            if (barcode.Length != MAX_LENGHT)
            {
                if (barcode.Contains(","))
                {
                    productNo = barcode.Split(',')[0].Trim().ToString();
                    var cartonNumberingDetailOnLabelList = CartonNumberingDetailController.Select(productNo).ToList();
                    Int32.TryParse(barcode.Split(',')[1].Trim().ToString(), out cartonNo);
                    var sizeNoOnLabel = cartonNumberingDetailOnLabelList.Where(w => w.CartonNo == cartonNo).FirstOrDefault();
                    if (cartonNumberingDetailOnLabelList.Count() == 0 || cartonNo == 0 || sizeNoOnLabel == null)
                    {
                        MessageBox.Show("Kiểm Tra Lại Barocde !", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        HighLightTextblock(txtBarCodeComplete, btnBarcodeComplete);
                        return;
                    }
                    sizeNo = sizeNoOnLabel.SizeNo;
                }
                else
                {
                    MessageBox.Show("Barocde ( Mã Thùng ) Không Hợp Lệ !", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    HighLightTextblock(txtBarCodeComplete, btnBarcodeComplete);
                    return;
                }
            }
            else
            {
                var serialDigitList = storingTempList.Select(s => s.SerialNo).Distinct().ToList();
                var serialNo = ConvertBarcodeToSerial(barcode);
                if (serialDigitList.Contains(serialNo) == false)
                {
                    InputPOStatus();
                    return;
                }
                else
                {
                    var storingTemp_SerialNo = storingTempList.Where(w => w.SerialNo == serialNo).FirstOrDefault();
                    cartonNo = storingTemp_SerialNo.CartonNo;
                    productNo = storingTemp_SerialNo.ProductNo;
                    sizeNo = storingTemp_SerialNo.SizeNo;
                }
            }
            NoneHighLightTextblock(txtBarCodeComplete);

            // Check Inputted Or Not
            var storingByBarcode = storingCurrentList.Where(w => (w.StoringModel.Barcode == barcode
                                                                || (w.StoringModel.ProductNo == productNo && w.StoringModel.CartonNo == cartonNo))
                                                                && w.StoringModel.IsComplete == completeCarton)
                                                     .Select(s => s.StoringModel).FirstOrDefault();
            if (storingByBarcode != null && storingByBarcode.IsPass == true)
            {
                MessageBox.Show(string.Format("Thùng {0}, PO {1} Đã Được Cân !", storingByBarcode.CartonNo, storingByBarcode.ProductNo), "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                tblProductNo.Text = storingByBarcode.ProductNo;
                tblSizeItemQuantityCartonNo.Text = String.Format("Size: {0}\nCartonNo: {1} of {2}", storingByBarcode.SizeNo, storingByBarcode.CartonNo, GetMaxCarton(storingByBarcode.ProductNo));

                txtBarCodeComplete.Focus();
                txtBarCodeComplete.SelectAll();
                btnBarcodeComplete.IsEnabled = true;
                btnBarcodeComplete.IsDefault = true;

                return;
            }
            BarcodeProcess();
        }

        List<CartonNumberingDetailModel> cartonNumberingDetail_POList = new List<CartonNumberingDetailModel>();
        private void btnProductNo_Click(object sender, RoutedEventArgs e)
        {
            txtProductNo.Text = txtProductNo.Text.ToUpper().Trim();

            productNo = txtProductNo.Text.ToUpper().Trim();
            if (productNo == "")
            {
                MessageBox.Show("Nhập PO# !", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                HighLightTextblock(txtProductNo, btnProductNo);
                return;
            }

            // Reload StoreProcedure
            cartonNumberingDetail_POList = CartonNumberingDetailController.Select(productNo);

            if (cartonNumberingDetail_POList.Count() == 0)
            {
                MessageBox.Show("Mã Đơn Hàng Không Đúng !", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                HighLightTextblock(txtProductNo, btnProductNo);
                return;
            }

            // If ProductNo has been inputted and PO is correctly.
            //var storingCurrent_POList = storingCurrentList.Where(w => w.StoringModel.ProductNo == productNo).OrderBy(o => o.StoringModel.CartonNo).ToList();
            //if (storingCurrent_POList.Count > 2)
            //{
            //    var firstModel = storingCurrent_POList.FirstOrDefault();
            //    var lastModel = storingCurrent_POList.LastOrDefault();

            //    int cartonSum = lastModel.StoringModel.CartonNo - firstModel.StoringModel.CartonNo;
            //    int digitSum = ConvertBarcodeToSerial(lastModel.StoringModel.Barcode) - ConvertBarcodeToSerial(firstModel.StoringModel.Barcode);

            //    if (cartonSum == digitSum)
            //    {
            //        MessageBox.Show(String.Format("Đơn: {0} Đã Được Nhập !", productNo), "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //        HighLightTextblock(txtProductNo, btnProductNo);
            //        return;
            //    }
            //}

            // Checking ProductNo was loaded or not
            var cartonNumberingDetailLoadedPOList = CartonNumberingDetailController.SelectPOLoaded(productNo).ToList();
            if (cartonNumberingDetailLoadedPOList.Count > 0)
            {
                MessageBox.Show(String.Format("Đơn: {0} Đã Được Xuất Hàng !", productNo), "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                HighLightTextblock(txtProductNo, btnProductNo);
                return;
            }

            NoneHighLightTextblock(txtProductNo);

            btnProductNo.IsEnabled = false;
            txtProductNo.IsEnabled = false;

            txtCartonNo.IsEnabled = true;
            txtCartonNo.Focus();
            txtCartonNo.SelectAll();

            btnCartonNo.IsEnabled = true;
            btnCartonNo.IsDefault = true;
        }

        private void btnCartonNo_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(txtCartonNo.Text.Trim().ToString(), out cartonNo);
            if (cartonNumberingDetail_POList.Where(w => w.CartonNo == cartonNo).Count() == 0)
            {
                MessageBox.Show(String.Format("Kiểm Tra Lại PO: {0} , Thùng {1} !", productNo, cartonNo), "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                HighLightTextblock(txtCartonNo, btnCartonNo);
                return;
            }
            
            var poAndCartonScanned = storingCurrentList.Where(w => w.StoringModel.ProductNo == productNo && w.StoringModel.CartonNo == cartonNo).FirstOrDefault();
            if (poAndCartonScanned != null)
            {
                MessageBox.Show(String.Format("Thùng: {0}, PO: {1} Đã Được Nhập !", poAndCartonScanned.StoringModel.CartonNo, poAndCartonScanned.StoringModel.ProductNo),
                                "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);

                HighLightTextblock(txtProductNo, btnProductNo);
                txtCartonNo.IsEnabled = false;
                btnCartonNo.IsEnabled = false;
                return;
            }
            NoneHighLightTextblock(txtProductNo);

            // Add barcode range of PO to storingTempList.
            // If PO upload cartonnumberingdetail already, but not enough cartonno or wrong something. Remove POstoringtemplist. Then add PO to storingtemplist again.
            int serialCartonNewPO_0 = ConvertBarcodeToSerial(barcode) - cartonNo;
            foreach (var p in cartonNumberingDetail_POList)
            {
                storingTempList.RemoveAll(r => r.ProductNo == productNo && r.CartonNo == p.CartonNo);

                StoringTemp storingTemp = new StoringTemp()
                {
                    SizeNo = p.SizeNo,
                    ProductNo = productNo,
                    CartonNo = p.CartonNo,
                    SerialNo = serialCartonNewPO_0 + p.CartonNo,
                };

                storingTempList.Add(storingTemp);
            }
            sizeNo = storingTempList.Where(w => w.ProductNo == productNo && w.CartonNo == cartonNo).FirstOrDefault().SizeNo;

            txtCartonNo.IsEnabled = false;
            btnCartonNo.IsEnabled = false;
            popInputSubPO.IsOpen = false;

            BarcodeProcess();
        }

        double minActualWeight = 0;
        double maxActualWeight = 0;
        StoringModel firstCarton_Size_PO;
        bool insertFirstCarton_Size_PO = false;
        bool insertIncompleteCarton = false;
        bool compareCartonSameSize = false;
        private void BarcodeProcess()
        {
            serialPortReceive.Close();
            firstCarton_Size_PO = new StoringModel();
            ComPortHelper.WriteToPort(portWrite, "DIO[0]:VALUE=0\r\n");
            ComPortHelper.WriteToPort(portWrite, "DIO[3]:VALUE=0\r\n");
            if (double.TryParse(txtMinActualWeight.Text, out minActualWeight) == false)
            {
                txtMinActualWeight.BorderBrush = Brushes.Red;
            }
            else
            {
                txtMinActualWeight.ClearValue(TextBox.BorderBrushProperty);
            }

            if (double.TryParse(txtMaxActualWeight.Text, out maxActualWeight) == false)
            {
                txtMaxActualWeight.BorderBrush = Brushes.Red;
            }

            else
            {
                txtMaxActualWeight.ClearValue(TextBox.BorderBrushProperty);
            }

            tblProductNo.Text = productNo;
            tblSizeItemQuantityCartonNo.Text = String.Format("Size: {0}\nCartonNo: {1} of {2}", sizeNo, cartonNo, GetMaxCarton(productNo));
            // check first carton of size of productno
            if (completeCarton == true)
            {
                var storingCurrent_POList = storingCurrentList.Where(w => w.StoringModel.ProductNo == productNo).Select(s => s.StoringModel).ToList();
                firstCarton_Size_PO = storingCurrent_POList.Where(w => w.SizeNo == sizeNo && w.IsPass == true && w.IsComplete == true).FirstOrDefault();
                var firstCartonProblem = new StoringModel()
                   {
                       ProductNo = productNo,
                       Barcode = barcode,
                       SizeNo = sizeNo,
                       CartonNo = cartonNo,
                       WorkerId = account.UserName,
                       ActualWeight = 0,
                       GrossWeight = 0,
                       DifferencePercent = 0,
                       IsComplete = true
                   };
                if (firstCarton_Size_PO == null)
                {
                    MessageBoxResult result = MessageBox.Show(string.Format("Đây Là Thùng Đầu Tiên Của Size: {0} ; PO#: {1}\n\nMở Thùng Và Kiểm Tra\n- SỐ LƯỢNG GIÀY\n- SIZE.\n\nClick OK để CÂN và tiếp tục, Nếu có vấn đề Click Cancel! ", sizeNo, productNo),
                        "Kiểm tra thùng đầu tiên của Size",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {
                        serialPortReceive.Open();
                        insertFirstCarton_Size_PO = true;
                    }
                    if (result == MessageBoxResult.Cancel)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            CheckIssuesWindow window = new CheckIssuesWindow(null, firstCartonProblem, IssuesType.Issues.IssuesFirstCartonOfSizeOfPO, factory);
                            window.ShowDialog();
                            StoringModel storingRecieve = window.storingCurrent;
                            StoringCurrent storingCurrent = new StoringCurrent();
                            storingCurrent.StoringModel = storingRecieve;
                            storingCurrentList.Add(storingCurrent);
                        }));
                    }
                }
                else
                {
                    serialPortReceive.Open();
                    compareCartonSameSize = true;
                }
            }
            else
            {
                serialPortReceive.Open();
                insertIncompleteCarton = true;
            }

            popInputSubPO.IsOpen = false;
            btnBarcodeComplete.IsEnabled = false;
            btnCartonNo.IsEnabled = false;
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

                    }
                    if (minActualWeight < actualWeight && actualWeight < maxActualWeight)
                    {
                        Dispatcher.Invoke(new Action(() =>
                        {
                            tblActualWeight.Text = string.Format("{0}", actualWeight);
                            tblActualWeight.Tag = actualWeight;
                            if (insertFirstCarton_Size_PO == true)
                            {
                                InsertCarton();
                                insertFirstCarton_Size_PO = false;
                                DefaultStatus();
                            }
                            if (insertIncompleteCarton == true)
                            {
                                InsertCarton();
                                insertIncompleteCarton = false;
                                IncompleteStatus();
                            }
                            if (compareCartonSameSize == true)
                            {
                                CompareWeight();
                                compareCartonSameSize = false;
                                DefaultStatus();
                            }
                        }));
                        serialPortReceive.Close();
                    }
                }
            }
        }

        private void InsertCarton()
        {
            double actualWeightFirstCarton = 0;
            Double.TryParse(tblActualWeight.Tag.ToString(), out actualWeightFirstCarton);
            var modelInsert = new StoringModel()
            {
                ProductNo = productNo,
                Barcode = barcode,
                SizeNo = sizeNo,
                CartonNo = cartonNo,
                ActualWeight = actualWeightFirstCarton,
                GrossWeight = 0,
                IsComplete = completeCarton,
                DifferencePercent = 0,
                IsPass = true,
                IssuesId = 0,
                WorkerId = account.UserName,
            };
            StoringController.Insert(modelInsert);
            if (completeCarton == true)
                DefaultStatus();
            else
                IncompleteStatus();

            // Highlight
            tblResult.Dispatcher.Invoke(new Action(() =>
            {
                tblResult.Foreground = Brushes.White;
                tblResult.Text = string.Format("{0} - Add", modelInsert.CartonNo);
            }));
            brResult.Dispatcher.Invoke(new Action(() =>
            {
                brResult.Background = Brushes.Green;
            }));

            StoringCurrent insertCurrent = new StoringCurrent();
            insertCurrent.StoringModel = modelInsert;
            storingCurrentList.Add(insertCurrent);
        }

        private void CompareWeight()
        {
            var currentCarton = new StoringModel()
            {
                ProductNo = productNo,
                Barcode = barcode,
                SizeNo = sizeNo,
                CartonNo = cartonNo,
                WorkerId = account.UserName,
                IsComplete = true,
            };
            // get gross weight of the carton has same size with this carton.
            tblGrossWeight.Text = string.Format("{0}", firstCarton_Size_PO.ActualWeight);
            tblGrossWeight.Tag = string.Format("{0}", firstCarton_Size_PO.ActualWeight);

            double grossWeight = 0;
            Double.TryParse(tblGrossWeight.Tag.ToString(), out grossWeight);
            double actualWeight = 0;
            Double.TryParse(tblActualWeight.Tag.ToString(), out actualWeight);

            if (grossWeight <= 0)
            {
                grossWeight = actualWeight;
            }

            currentCarton.GrossWeight = grossWeight;
            currentCarton.ActualWeight = actualWeight;
            double percentDifference = actualWeight / grossWeight;
            tblDifferencePercent.Dispatcher.Invoke(new Action(() =>
                tblDifferencePercent.Text = string.Format("{0}", Math.Round(100 * (percentDifference - 1), 2))
                ));
            currentCarton.DifferencePercent = Math.Round(100 * (percentDifference - 1), 2);

            // if carton is OK
            if (percentDifference >= LIMITED_MIN && percentDifference <= LIMITED_MAX)
            {
                tblResult.Dispatcher.Invoke(new Action(() =>
                {
                    tblResult.Foreground = Brushes.White;
                    tblResult.Text = string.Format("{0} - Pass", currentCarton.CartonNo);
                }));
                brResult.Dispatcher.Invoke(new Action(() =>
                {
                    brResult.Background = Brushes.Green;
                }));
                currentCarton.IsPass = true;
                currentCarton.IssuesId = 0;

                StoringController.Insert(currentCarton);

                StoringCurrent storingCurrentModel = new StoringCurrent();
                storingCurrentModel.StoringModel = currentCarton;
                storingCurrentList.Add(storingCurrentModel);
            }
            // If current carton has problem (the weight is lower or higher than firstcarton)
            else
            {
                // Show highlight.
                if (percentDifference < LIMITED_MIN)
                {
                    tblResult.Dispatcher.Invoke(new Action(() =>
                    {
                        tblResult.Foreground = Brushes.Black;
                        tblResult.Text = string.Format("{0} - Low", currentCarton.CartonNo);
                    }));
                    brResult.Dispatcher.Invoke(new Action(() =>
                    {
                        brResult.Background = Brushes.Yellow;
                    }));
                }
                else
                {
                    tblResult.Dispatcher.Invoke(new Action(() =>
                    {
                        tblResult.Foreground = Brushes.White;
                        tblResult.Text = string.Format("{0} - Hi", currentCarton.CartonNo);
                    }));
                    brResult.Dispatcher.Invoke(new Action(() =>
                    {
                        brResult.Background = Brushes.Red;
                    }));
                }
                // tranfer this model to check problem, after that insert to db
                currentCarton.IsPass = false;
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    CheckIssuesWindow window = new CheckIssuesWindow(null, currentCarton, IssuesType.Issues.IssuesCompareWeight, factory);
                    window.ShowDialog();

                    StoringModel storingRecieve = window.storingCurrent;

                    StoringCurrent storingCurrent = new StoringCurrent();
                    storingCurrent.StoringModel = storingRecieve;

                    storingCurrentList.Add(storingCurrent);
                }));
            }
            DefaultStatus();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DefaultStatus();
            }
            if (e.Key == Key.Home)
            {
                DefaultStatus();
            }
            if (e.Key == Key.End)
            {
                IncompleteStatus();
            }
        }

        #region visible
        bool completeCarton = true;
        private void btnComplete_Click(object sender, RoutedEventArgs e)
        {
            tblProductNo.Text = "";
            tblSizeItemQuantityCartonNo.Text = "";
            DefaultStatus();
        }
        private void btnIncomplete_Click(object sender, RoutedEventArgs e)
        {
            tblProductNo.Text = "";
            tblSizeItemQuantityCartonNo.Text = "";
            IncompleteStatus();
        }

        private void DefaultStatus()
        {
            completeCarton = true;
            brComplete.Background = (System.Windows.Media.LinearGradientBrush)FindResource("CompleteBackground");

            //brComplete.Background = Brushes.LightGreen;
            tblProductNo.Foreground = Brushes.Red;
            tblSizeItemQuantityCartonNo.Foreground = Brushes.Red;


            btnBarcodeComplete.IsEnabled = true;
            btnBarcodeComplete.IsDefault = true;

            txtBarCodeComplete.IsEnabled = true;
            txtBarCodeComplete.Focus();
            txtBarCodeComplete.SelectAll();

            txtProductNo.IsEnabled = false;
            btnProductNo.IsEnabled = false;

            txtCartonNo.IsEnabled = false;
            btnCartonNo.IsEnabled = false;

            popInputSubPO.IsOpen = false;
        }
        private void IncompleteStatus()
        {
            completeCarton = false;
            brComplete.Background = (System.Windows.Media.LinearGradientBrush)FindResource("IncompleteBackground");

            //brComplete.Background = Brushes.Tomato;
            tblProductNo.Foreground = Brushes.Blue;
            tblSizeItemQuantityCartonNo.Foreground = Brushes.Blue;

            btnBarcodeComplete.IsEnabled = true;
            btnBarcodeComplete.IsDefault = true;

            txtBarCodeComplete.IsEnabled = true;
            txtBarCodeComplete.Focus();
            txtBarCodeComplete.SelectAll();

            txtProductNo.IsEnabled = false;
            btnProductNo.IsEnabled = false;

            txtCartonNo.IsEnabled = false;
            btnCartonNo.IsEnabled = false;

            popInputSubPO.IsOpen = false;
        }

        private void InputPOStatus()
        {
            popInputSubPO.IsOpen = true;

            txtProductNo.IsEnabled = true;
            txtProductNo.Focus();
            txtProductNo.SelectAll();

            btnProductNo.IsEnabled = true;
            btnProductNo.IsDefault = true;

            btnBarcodeComplete.IsEnabled = false;
            btnBarcodeComplete.IsDefault = false;
        }

        private void ClearValue()
        {
            productNo = "";
            cartonNo = 0;
            sizeNo = "";

            tblGrossWeight.Text = "0";
            tblActualWeight.Text = "0";
            tblDifferencePercent.Text = "0";
            brResult.Background = Brushes.Transparent;
            tblResult.Foreground = Brushes.Black;
            tblResult.Text = "...";
            tblSizeItemQuantityCartonNo.Text = "";
            tblProductNo.Text = "";
        }

        private void HighLightTextblock(TextBox textbox, Button button)
        {
            if (completeCarton == true)
            {
                textbox.BorderBrush = Brushes.Red;
                textbox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
            if (completeCarton == false)
            {
                textbox.BorderBrush = Brushes.Blue;
                textbox.BorderThickness = new Thickness(2, 2, 2, 2);
            }
            textbox.IsEnabled = true;
            textbox.Focus();
            textbox.SelectAll();
            button.IsEnabled = true;
            button.IsDefault = true;
        }

        private void NoneHighLightTextblock(TextBox textbox)
        {
            textbox.IsEnabled = true;
            textbox.BorderBrush = Brushes.Black;
            textbox.BorderThickness = new Thickness(1, 1, 1, 1);
        }

        private void txtCartonNo_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnCartonNo.IsEnabled = true;
            btnCartonNo.IsDefault = true;
            txtCartonNo.Focus();
            txtCartonNo.SelectAll();
        }

        private void txtCartonNo_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnCartonNo.IsEnabled = false;
        }

        private void txtBarCodeComplete_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnBarcodeComplete.IsEnabled = true;
            btnBarcodeComplete.IsDefault = true;
            txtBarCodeComplete.Focus();
            txtBarCodeComplete.SelectAll();
        }

        private void txtBarCodeComplete_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnBarcodeComplete.IsEnabled = false;
        }

        private void txtProductNo_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnProductNo.IsEnabled = false;
        }

        private void txtProductNo_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnProductNo.IsEnabled = true;
            btnProductNo.IsDefault = true;

            txtProductNo.Focus();
            txtProductNo.SelectAll();
        }

        #endregion

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Đóng Chương Trình?", "Storing - System", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                serialPortReceive.Close();
                ComPortHelper.WriteToPort(portWrite, "DIO[0]:VALUE=0\r\n");
                ComPortHelper.WriteToPort(portWrite, "DIO[3]:VALUE=0\r\n");
            }
            else
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Convert 20Character Barcode to 10Character SerialNumber
        /// </summary>
        /// <param name="barcode">Barcode</param>
        /// <returns>SerialNo</returns>
        private int ConvertBarcodeToSerial(string barcode)
        {
            int serial = 0;
            Int32.TryParse(barcode.Substring(FIRST_SERIAL, SERIAL_LENGHT).ToString(), out serial);
            return serial;
        }

        private string GetSerialDigit(string barcode)
        {
            string result = "";
            return result = barcode.Substring(FIRST_SERIAL, SERIAL_LENGHT).ToString();
        }

        /// <summary>
        /// How many carton in this PO
        /// </summary>
        /// <param name="productNo">ProductNo</param>
        /// <returns>Quantity Per PO</returns>
        private int GetMaxCarton(string productNo)
        {
            var cartonNumberingDetailList = CartonNumberingDetailController.Select(productNo);
            if (cartonNumberingDetailList.Count() > 0)
            {
                return cartonNumberingDetailList.Select(s => s.CartonNo).Max();
            }
            else
            {
                return 0;
            }
        }

        class StoringCurrent
        {
            public StoringModel StoringModel { get; set; }
        }

        class StoringTemp
        {
            public string ProductNo { get; set; }
            public string SizeNo { get; set; }
            public int CartonNo { get; set; }
            public int SerialNo { get; set; }
        }

        private void smtpClient_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            flagSending = false;
        }
    }
}
