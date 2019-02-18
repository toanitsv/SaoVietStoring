using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using BarcodeLib;
using SaoVietStoring.Controllers;
using SaoVietStoring.Models;
using drawing = System.Drawing;

namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for PrintBarcodeWindow.xaml
    /// </summary>
    public partial class PrintBarcodeWindow : Window
    {
        BackgroundWorker bwLoad;
        BackgroundWorker bwPrintBarcode;
        List<CartonNumberingDetailModel> cartonNumberingDetailList;
        List<CartonNumberingDetailModel> cartonNumberingDetailPrintList;

        //drawing.Image image;
        drawing.Image image1;
        drawing.Image image2;
        int position1X = 0, position1Y = 0, position2X = 0, position2Y = 0;
        public PrintBarcodeWindow()
        {
            bwLoad = new BackgroundWorker();
            bwLoad.DoWork +=new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);

            bwPrintBarcode = new BackgroundWorker();
            bwPrintBarcode.DoWork +=new DoWorkEventHandler(bwPrintBarcode_DoWork);
            bwPrintBarcode.RunWorkerCompleted +=new RunWorkerCompletedEventHandler(bwPrintBarcode_RunWorkerCompleted);

            cartonNumberingDetailList = new List<CartonNumberingDetailModel>();
            cartonNumberingDetailPrintList = new List<CartonNumberingDetailModel>();

            InitializeComponent();
        }

        string productNo = "";
        private void btnSeach_Click(object sender, RoutedEventArgs e)
        {
            productNo = txtProductNo.Text.ToString();
            txtProductNo.Text = productNo.ToUpper();
            if (bwLoad.IsBusy == false)
            {
                stkMain.Children.Clear();
                txtSize.Clear();
                txtCartonNoFrom.Clear();
                txtCartonNoTo.Clear();
                txtStatus.Text = "";

                imageBarcode1.Source = null;
                imageBarcode2.Source = null;

                this.Cursor = Cursors.Wait;
                btnSeach.IsEnabled = false;
                bwLoad.RunWorkerAsync();
            }
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            cartonNumberingDetailList = CartonNumberingDetailController.Select(productNo).OrderBy(o => o.CartonNo).ToList();
        }

        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            if (cartonNumberingDetailList.Count == 0)
            {
                MessageBox.Show(String.Format("Not Found! Please Upload PO: {0} to LoadingSystem !", productNo), this.Title, MessageBoxButton.OK, MessageBoxImage.Error);
                btnSeach.IsEnabled = true;
                btnSeach.IsDefault = true;
                txtProductNo.Focus();
                txtProductNo.SelectAll();
                return;
            }

            var sizeNoList = cartonNumberingDetailList.Select(s => s.SizeNo).Distinct().ToList();
            foreach (var sizeNo in sizeNoList)
            {
                StackPanel stkSizeNo = new StackPanel();
                stkSizeNo.Margin = new Thickness(0, 0, 20, 0);
                stkSizeNo.Orientation = Orientation.Vertical;
                TextBlock tblSize = new TextBlock();
                tblSize.Text = sizeNo;
                tblSize.TextAlignment = TextAlignment.Center;
                tblSize.FontSize = 16;
                tblSize.FontWeight = FontWeights.Bold;
                tblSize.Margin = new Thickness(0, 0, 0, 10);
                stkSizeNo.Children.Add(tblSize);

                var cartonNoList = cartonNumberingDetailList.Where(w => w.SizeNo == sizeNo).Select(s => s.CartonNo).ToList();
                foreach (var cartonNo in cartonNoList)
                {
                    Button btnCartonNo = new Button();
                    Style style = (Style)FindResource("MyButton");
                    btnCartonNo.Style = style;
                    btnCartonNo.Content = cartonNo.ToString();
                    btnCartonNo.FontSize = 14;
                    btnCartonNo.MinWidth = 50;
                    btnCartonNo.Margin = new Thickness(0, 0, 0, 5);
                    btnCartonNo.Tag = cartonNo;
                    btnCartonNo.Click +=new RoutedEventHandler(btnCartonNo_Click);
                    stkSizeNo.Children.Add(btnCartonNo);
                }
                stkMain.Children.Add(stkSizeNo);
            }

            btnSeach.IsEnabled = true;
            btnSeach.IsDefault = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtProductNo.Focus();

            Int32.TryParse(txtWidth.Text.ToString(), out WIDTH);
            Int32.TryParse(txtHeight.Text.ToString(), out HEIGHT);

        }

        private void btnCartonNo_Click(object sender, RoutedEventArgs e)
        {
            imageBarcode1.Source = null;
            imageBarcode2.Source = null;
            txtStatus.Text = "";

            Int32.TryParse(txtWidth.Text.ToString(), out WIDTH);
            Int32.TryParse(txtHeight.Text.ToString(), out HEIGHT);

            Button btnCartonNoClicked = (Button)sender as Button;
            int cartonNoClicked = (int)btnCartonNoClicked.Tag;
            var cartonNumberingDetailClicked = cartonNumberingDetailList.Where(w => w.CartonNo == cartonNoClicked).ToList();

            cartonNumberingDetailPrintList = new List<CartonNumberingDetailModel>();
            cartonNumberingDetailPrintList = cartonNumberingDetailClicked.ToList();

            if (bwPrintBarcode.IsBusy == true || cartonNumberingDetailClicked.Count == 0 ||
                MessageBox.Show(String.Format("Confirm Print:  {0},{1}?",
                    cartonNumberingDetailClicked.FirstOrDefault().ProductNo,
                    cartonNumberingDetailClicked.FirstOrDefault().CartonNo),
                    this.Title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                return;
            }

            this.Cursor = Cursors.Wait;
            bwPrintBarcode.RunWorkerAsync();
        }

        int cartonNoFrom = 0, cartonNoTo = 0;
        string sizeNo = "";
        int WIDTH = 0, HEIGHT = 0;
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            txtStatus.Text = "";
            Int32.TryParse(txtCartonNoFrom.Text.ToString(), out cartonNoFrom);
            Int32.TryParse(txtCartonNoTo.Text.ToString(), out cartonNoTo);
            sizeNo = txtSize.Text.ToString();

            Int32.TryParse(txtWidth.Text.ToString(), out WIDTH);
            Int32.TryParse(txtHeight.Text.ToString(), out HEIGHT);

            if (bwPrintBarcode.IsBusy == false)
            {
                if (radBySizeClicked == true)
                {
                    cartonNumberingDetailPrintList = cartonNumberingDetailList.Where(w => w.SizeNo == sizeNo).ToList();
                }

                if (radCartonRangeClicked == true)
                {
                    cartonNumberingDetailPrintList = cartonNumberingDetailList.Where(w => w.CartonNo >= cartonNoFrom && w.CartonNo <= cartonNoTo).ToList();
                }
                if (radAllClicked == true)
                {
                    cartonNumberingDetailPrintList = cartonNumberingDetailList.ToList();
                }

                if (cartonNumberingDetailPrintList.Count == 0)
                {
                    MessageBox.Show("Nothing to print !", this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                btnPrint.IsEnabled = false;
                this.Cursor = Cursors.Wait;
                bwPrintBarcode.RunWorkerAsync();
            }
        }

        bool printLocation2;
        private void bwPrintBarcode_DoWork(object sender, DoWorkEventArgs e)
        {
            var productNoAndCartonNoList = cartonNumberingDetailPrintList.Select(s => new { ProductNo = s.ProductNo, CartonNo = s.CartonNo }).ToList();
            for (int i = 0; i < productNoAndCartonNoList.Count; i +=quantityLabelPrint)
            {
                Thread.Sleep(789);
                printLocation2 = false;
                Dispatcher.Invoke(new Action(() =>
                {
                    imageBarcode1.Source = null;
                    imageBarcode2.Source = null;
                    string barcode1 = "", barcode2 = "";

                    // Location 1
                    // Created Image Barcode 1
                    barcode1 = string.Format("{0},{1}", productNoAndCartonNoList[i].ProductNo.ToUpper().ToString(), productNoAndCartonNoList[i].CartonNo);

                    var barcodeLib1 = new BarcodeLib.Barcode()
                    {
                        Alignment = AlignmentPositions.CENTER,
                        IncludeLabel = true,
                        Width = WIDTH,
                        Height = HEIGHT
                    };
                    
                    try
                    {
                        image1 = barcodeLib1.Encode(TYPE.CODE128, barcode1);
                    }
                    catch (Exception ex)
                    {
                        throw new ArgumentException(string.Format("Generator {0} fail !", barcode1));
                    }

                    // View image 1
                    MemoryStream memoryStream1 = new MemoryStream();
                    image1.Save(memoryStream1, drawing.Imaging.ImageFormat.Png);
                    memoryStream1.Position = 0;
                    BitmapImage itmapImage1 = new BitmapImage();
                    itmapImage1.BeginInit();
                    itmapImage1.StreamSource = memoryStream1;
                    itmapImage1.EndInit();

                    imageBarcode1.Source = itmapImage1;

                    // Location 2
                    // Created Image Barcode 2
                    if (quantityLabelPrint == 2)
                    {
                        if (i + 1 < productNoAndCartonNoList.Count)
                        {
                            printLocation2 = true;
                            barcode2 = string.Format("{0},{1}", productNoAndCartonNoList[i + 1].ProductNo.ToUpper().ToString(), productNoAndCartonNoList[i + 1].CartonNo);
                            BarcodeLib.Barcode barcodeLib2 = new BarcodeLib.Barcode();
                            barcodeLib2.Alignment = AlignmentPositions.CENTER;
                            barcodeLib2.IncludeLabel = true;
                            barcodeLib2.Width = WIDTH;
                            barcodeLib2.Height = HEIGHT;
                            try
                            {
                                image2 = barcodeLib2.Encode(TYPE.CODE128, barcode2);
                            }
                            catch (Exception ex)
                            {
                                throw new ArgumentException(string.Format("Generator {0} fail !", barcode2));
                            }

                            // View image 2
                            MemoryStream memoryStream2 = new MemoryStream();
                            image2.Save(memoryStream2, drawing.Imaging.ImageFormat.Png);
                            memoryStream2.Position = 0;
                            BitmapImage itmapImage2 = new BitmapImage();
                            itmapImage2.BeginInit();
                            itmapImage2.StreamSource = memoryStream2;
                            itmapImage2.EndInit();

                            imageBarcode2.Source = itmapImage2;
                        }
                    }

                    // Print 2 label per onetime.
                    drawing.Printing.PrintDocument printDocument = new drawing.Printing.PrintDocument();
                    printDocument.DocumentName = string.Format("Barcode Print {0}\n{1}", barcode1, barcode2);
                    printDocument.PrintPage += new drawing.Printing.PrintPageEventHandler(printDocument_PrintPage);
                    printDocument.EndPrint += new drawing.Printing.PrintEventHandler(printDocument_EndPrint);
                    printDocument.Print();

                    txtStatus.Text = string.Format("Sent {0} labels to printer !", productNoAndCartonNoList.Count());
                }));
            }
        }

        private void bwPrintBarcode_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = null;
            btnPrint.IsEnabled = true;
            txtStatus.Text = "Finished !";
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            string location1 = txtLocation1.Text.ToString();
            string location2 = txtLocation2.Text.ToString();
            string[] location1Arr = location1.Split(',');
            string[] location2Arr = location2.Split(',');

            Int32.TryParse(location1Arr[0].ToString(), out position1X);
            Int32.TryParse(location1Arr[1].ToString(), out position1Y);

            Int32.TryParse(location2Arr[0].ToString(), out position2X);
            Int32.TryParse(location2Arr[1].ToString(), out position2Y);

            if (image1 != null)
            {
                drawing.Point location1Point = new drawing.Point(position1X, position1Y);
                e.Graphics.DrawImage(image1, location1Point);
            }
            if (image2 != null && printLocation2 == true && quantityLabelPrint == 2)
            {
                drawing.Point location2Point = new drawing.Point(position2X, position2Y);
                e.Graphics.DrawImage(image2, location2Point);
            }
        }

        private void printDocument_EndPrint(object sender, drawing.Printing.PrintEventArgs e)
        {
        }

        bool radAllClicked = false, radBySizeClicked = false, radCartonRangeClicked = false;
        private void radAll_Checked(object sender, RoutedEventArgs e)
        {
            txtCartonNoFrom.Clear();
            txtCartonNoTo.Clear();
            txtSize.Clear();

            radAllClicked = true;
            radBySizeClicked = false;
            radCartonRangeClicked = false;
        }

        private void radBySize_Checked(object sender, RoutedEventArgs e)
        {
            txtCartonNoFrom.Clear();
            txtCartonNoTo.Clear();
            txtSize.Focus();

            radAllClicked = false;
            radBySizeClicked = true;
            radCartonRangeClicked = false;
        }

        private void radByCartonRange_Checked(object sender, RoutedEventArgs e)
        {
            txtSize.Clear();
            txtCartonNoFrom.Focus();

            radAllClicked = false;
            radBySizeClicked = false;
            radCartonRangeClicked = true;
        }

        int quantityLabelPrint = 1;
        private void ckQuantityLabelPrint_Checked(object sender, RoutedEventArgs e)
        {
            quantityLabelPrint = 2;
        }

        private void ckQuantityLabelPrint_Unchecked(object sender, RoutedEventArgs e)
        {
            quantityLabelPrint = 1;
        }

    }
}
