using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;

using SaoVietStoring.Models;
using SaoVietStoring.Controllers;

namespace SaoVietStoring.Views
{
    /// <summary>
    /// Interaction logic for ImportPORepackingWindow.xaml
    /// </summary>
    public partial class ImportPORepackingWindow : Window
    {
        List<PORepackingModel> poRepackingInsertList;
        List<PORepackingModel> poRepackingLoadList;
        List<PORepackingModel> poRepackingReLoadList;
        BackgroundWorker bwLoad;

        ControlIssuesAccountModel controlAccount;
        public ImportPORepackingWindow()
        {
            poRepackingInsertList = new List<PORepackingModel>();
            poRepackingLoadList = new List<PORepackingModel>();
            poRepackingReLoadList = new List<PORepackingModel>();
            controlAccount = new ControlIssuesAccountModel();

            bwLoad = new BackgroundWorker();
            bwLoad.DoWork += new DoWorkEventHandler(bwLoad_DoWork);
            bwLoad.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bwLoad_RunWorkerCompleted);
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (bwLoad.IsBusy == true)
            {
                return;
            }
            this.Cursor = Cursors.Wait;
            bwLoad.RunWorkerAsync();
        }

        private void bwLoad_DoWork(object sender, DoWorkEventArgs e)
        {
            poRepackingLoadList = PORepackingController.GetAll();
        }
        private void bwLoad_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgPORepacking.ItemsSource = poRepackingLoadList;
            this.Cursor = null;
        }

        int passWord = 0;
        int modeClearOrSave = 0;
        PORepackingModel currentPO = new PORepackingModel();
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            poRepackingReLoadList = dgPORepacking.Items.OfType<PORepackingModel>().ToList();
            currentPO = dgPORepacking.CurrentItem as PORepackingModel;
            stkControlAccount.Visibility = Visibility.Visible;
            txtPassword.Clear();
            txtPassword.Focus();

            modeClearOrSave = 1;
        }

        private void ReLoad()
        {
            dgPORepacking.ItemsSource = null;
            dgPORepacking.ItemsSource = poRepackingReLoadList;
            stkControlAccount.Visibility = Visibility.Collapsed;
        }

        private void btnAddPORepacking_Click(object sender, RoutedEventArgs e)
        {
            poRepackingReLoadList = dgPORepacking.Items.OfType<PORepackingModel>().ToList();

            string productNo = "";
            productNo = txtPORepacking.Text.ToUpper().Trim();
            if (productNo == "")
            {
                txtPORepacking.Focus();
                return;
            }

            var productNoExist = poRepackingReLoadList.Where(w => w.ProductNo == productNo).ToList();
            if (productNoExist.Count > 0)
            {
                MessageBox.Show(string.Format("PO: {0} exist !", productNo), this.Title, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            PORepackingModel newPO = new PORepackingModel();
            newPO.ProductNo = productNo;
            newPO.CreatedTime = DateTime.Now;

            poRepackingReLoadList.Add(newPO);

            ReLoad();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            stkControlAccount.Visibility = Visibility.Visible;
            txtPassword.Clear();
            txtPassword.Focus();

            modeClearOrSave = 2;
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            Int32.TryParse(txtPassword.Password.ToString(), out passWord);
            controlAccount = ControlIssuesAccountController.FindSecurityCode(passWord);
            if (controlAccount == null)
            {
                MessageBox.Show(string.Format("Wrong Security Code !"), "Infor", MessageBoxButton.OK, MessageBoxImage.Error);
                txtPassword.Focus();
                txtPassword.SelectAll();
                return;
            }

            if (modeClearOrSave == 1)
            {
                MessageBoxResult result = MessageBox.Show("Confirm Clear!", "Infor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                poRepackingReLoadList.RemoveAll(r => r.ProductNo == currentPO.ProductNo);
                bool update = PORepackingController.Update(currentPO.ProductNo);

                ReLoad();
            }

            if (modeClearOrSave == 2)
            {

                poRepackingInsertList = dgPORepacking.Items.OfType<PORepackingModel>().ToList();
                if (poRepackingInsertList.Count == 0)
                {
                    MessageBox.Show("PO Repacking List Is Empty !", "Infor", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                MessageBoxResult result = MessageBox.Show("Confirm Save?", "Infor", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    return;
                }

                var poDBList = poRepackingLoadList.Select(s => s.ProductNo).ToList();
                poRepackingInsertList.RemoveAll(r => poDBList.Contains(r.ProductNo));

                foreach (var poRepacking in poRepackingInsertList)
                {
                    PORepackingController.Insert(poRepacking);
                    dgPORepacking.Dispatcher.Invoke((Action)(() =>
                    {
                        dgPORepacking.SelectedItem = poRepacking;
                        dgPORepacking.ScrollIntoView(poRepacking);
                    }));
                }
                MessageBox.Show(string.Format("{0} PO Imported !", poRepackingInsertList.Count), "Result", MessageBoxButton.OK, MessageBoxImage.Information);

                ReLoad();
            }
        }
    }
}
