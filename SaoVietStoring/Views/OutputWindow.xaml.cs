using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
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
    /// Interaction logic for OutputWindow.xaml
    /// </summary>
    public partial class OutputWindow : Window
    {
        AccountModel account;
        DispatcherTimer dispatcherTimer;

        string portReceive;
        SerialPort serialPortReceive;
        string portWrite;
        ElectricScaleProfile electricScaleProfile;
        double LIMITED_MIN = 0.95, LIMITED_MAX = 1.05;

        BackgroundWorker bwLoad;

        List<StoringModel> storingList;
        List<OutputingModel> outputingList;
        List<OutputingCurrent> outputingCurrentList;
        string factory;

        public OutputWindow(AccountModel _account, ElectricScaleProfile _electricScaleProfile, string factory)
        {
            this.account = _account;
            this.electricScaleProfile = _electricScaleProfile;
            this.factory = factory;

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();

            storingList = new List<StoringModel>();
            outputingList = new List<OutputingModel>();
            outputingCurrentList = new List<OutputingCurrent>();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

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

            InitializeComponent();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            tblStatusTime.Text = string.Format("{0:dd/MM/yyyy HH:mm:ss}", DateTime.Now);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = string.Format("{0} {1} - Version: {2} / User: {3} / Location: {4}", factory, this.Title, AssemblyHelper.Version(), account.FullName, electricScaleProfile.Location);

            serialPortReceive = new SerialPort();
            serialPortReceive.BaudRate = electricScaleProfile.BaudRate;
            serialPortReceive.DataReceived += new SerialDataReceivedEventHandler(serialPortReceive_DataReceived);

            portReceive = AppSettingsHelper.ReadSetting("ReceivePort");
            portWrite = AppSettingsHelper.ReadSetting("WritePort");

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
            storingList = StoringController.SelectIsNotLoad();
            outputingList = OutputingController.SelectIsNotLoad();

            if (outputingList.Count > 0)
            {
                foreach (var outputInDB in outputingList)
                {
                    OutputingCurrent current = new OutputingCurrent();
                    current.Outputing = outputInDB;
                    outputingCurrentList.Add(current);
                }
            }
            else
            {
                OutputingModel outputingDefault = new OutputingModel()
                {
                    ProductNo = "Default",
                    Barcode = "Default",
                    SizeNo = "Default",
                    CartonNo = 0,
                    GrossWeight = 0,
                    ActualWeight = 0,
                    DifferencePercent = 0,
                    IsPass = false,
                    WorkerId = "Default",
                    IssuesId = 0
                };

                OutputingCurrent currentDefault = new OutputingCurrent();
                currentDefault.Outputing = outputingDefault;
                outputingCurrentList.Add(currentDefault);
            }
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            txtBarCodeComplete.Focus();
            btnBarcodeComplete.IsEnabled = true;
            btnBarcodeComplete.IsDefault = true;
        }

        string barcode = "";
        double minActualWeight = 0;
        double maxActualWeight = 0;
        StoringModel storingPerBarcode = new StoringModel();
        OutputingModel outputingPerBarcode = new OutputingModel();
        private void btnBarcodeComplete_Click(object sender, RoutedEventArgs e)
        {
            ClearValue();
            barcode = txtBarCodeComplete.Text.Trim();
            storingPerBarcode = storingList.Where(w => w.Barcode == barcode).FirstOrDefault();
            if (storingPerBarcode == null)
            {
                MessageBox.Show("Thùng chưa được cân ở INPUT !", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                HighLightTextblock(txtBarCodeComplete, btnBarcodeComplete);
                return;
            }

            outputingPerBarcode = outputingCurrentList.Where(w => w.Outputing.Barcode == barcode && w.Outputing.IsPass == true).Select(s => s.Outputing).FirstOrDefault();
            if (outputingPerBarcode != null)
            {
                MessageBox.Show("Thùng đã được cân !", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                HighLightTextblock(txtBarCodeComplete, btnBarcodeComplete);
                return;
            }
            NoneHighLightTextblock(txtBarCodeComplete);

            tblProductNo.Text = storingPerBarcode.ProductNo;
            tblSizeItemQuantityCartonNo.Text = string.Format("Size: {0}\nCartonNo: {1}", storingPerBarcode.SizeNo, storingPerBarcode.CartonNo);

            serialPortReceive.Close();
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

            serialPortReceive.Open();
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
                        }));

                        serialPortReceive.Close();
                        CompareWeight();
                    }
                }
            }
        }

        OutputingModel outputingInsert = new OutputingModel();
        private void CompareWeight()
        {
            Dispatcher.Invoke(new Action(() =>
            {
                tblGrossWeight.Text = string.Format("{0}", storingPerBarcode.ActualWeight);
                tblGrossWeight.Tag = string.Format("{0}", storingPerBarcode.ActualWeight);
            }));

            double grossWeight = 0;
            double actualWeight = 0;
            Dispatcher.Invoke(new Action(() =>
            {
                Double.TryParse(tblGrossWeight.Tag.ToString(), out grossWeight);
                Double.TryParse(tblActualWeight.Tag.ToString(), out actualWeight);
            }));

            double percentDiffence = actualWeight / grossWeight;
            tblDifferencePercent.Dispatcher.Invoke(new Action(() => tblDifferencePercent.Text = string.Format("{0}", Math.Round(100 * (percentDiffence - 1), 2))));

            outputingInsert.ProductNo = storingPerBarcode.ProductNo;
            outputingInsert.Barcode = storingPerBarcode.Barcode;
            outputingInsert.CartonNo = storingPerBarcode.CartonNo;
            outputingInsert.SizeNo = storingPerBarcode.SizeNo;
            outputingInsert.GrossWeight = grossWeight;
            outputingInsert.ActualWeight = actualWeight;
            outputingInsert.WorkerId = account.UserName;


            outputingInsert.DifferencePercent = Math.Round(100 * (percentDiffence - 1), 2);
            // PASS
            if (percentDiffence >= LIMITED_MIN && percentDiffence <= LIMITED_MAX)
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    tblResult.Foreground = Brushes.White;
                    tblResult.Text = string.Format("{0} - Pass", outputingInsert.CartonNo);
                    brResult.Background = Brushes.Green;
                }));
                outputingInsert.IsPass = true;
                outputingInsert.IssuesId = 0;

                if (OutputingController.Insert(outputingInsert) == false)
                {
                    MessageBox.Show("Insert Error!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                OutputingCurrent outputCurrentInsert = new OutputingCurrent();
                outputCurrentInsert.Outputing = outputingInsert;
                outputingCurrentList.Add(outputCurrentInsert);
            }
            else
            {
                outputingInsert.IsPass = false;
                //LOW
                if (percentDiffence < LIMITED_MIN)
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        tblResult.Foreground = Brushes.Black;
                        tblResult.Text = string.Format("{0} - Low", outputingInsert.CartonNo);
                        brResult.Background = Brushes.Yellow;
                    }));
                }
                else
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        tblResult.Foreground = Brushes.White;
                        tblResult.Text = string.Format("{0} - Hi", outputingInsert.CartonNo);
                        brResult.Background = Brushes.Red;
                    }));
                }
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    CheckIssuesWindow window = new CheckIssuesWindow(outputingInsert, null, IssuesType.Issues.IssuesCompareWeight, factory);
                    window.ShowDialog();

                    OutputingModel outputRecieve = window.outputingCurrent;
                    OutputingCurrent outputCurrentRecieve = new OutputingCurrent();
                    outputCurrentRecieve.Outputing = outputRecieve;
                    outputingCurrentList.Add(outputCurrentRecieve);
                }));
            }
            Dispatcher.Invoke(new Action(() =>
            {
                txtBarCodeComplete.Focus();
                txtBarCodeComplete.SelectAll();
                btnBarcodeComplete.IsEnabled = true;
                btnBarcodeComplete.IsDefault = true;
            }));
        }

        private void BarcodeFocus()
        {
            txtBarCodeComplete.Focus();
            txtBarCodeComplete.SelectAll();

            btnBarcodeComplete.IsEnabled = true;
            btnBarcodeComplete.IsDefault = true;
        }

        private void HighLightTextblock(TextBox textbox, Button button)
        {
            textbox.BorderBrush = Brushes.Blue;
            textbox.BorderThickness = new Thickness(2, 2, 2, 2);
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

        private void ClearValue()
        {
            tblGrossWeight.Text = "0";
            tblActualWeight.Text = "0";
            tblDifferencePercent.Text = "0";
            brResult.Background = Brushes.Transparent;
            tblResult.Foreground = Brushes.Black;
            tblResult.Text = "...";
            tblSizeItemQuantityCartonNo.Text = "";
            tblProductNo.Text = "";
        }

        private void txtBarCodeComplete_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            btnBarcodeComplete.IsEnabled = true;
            btnBarcodeComplete.IsDefault = true;
            txtBarCodeComplete.Focus();
            txtBarCodeComplete.SelectAll();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                btnBarcodeComplete.IsEnabled = true;
                btnBarcodeComplete.IsDefault = true;
                txtBarCodeComplete.Focus();
                txtBarCodeComplete.SelectAll();
            }
        }

        private void txtBarCodeComplete_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
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

        class OutputingCurrent
        {
            public OutputingModel Outputing { get; set; }
        }
    }
}
