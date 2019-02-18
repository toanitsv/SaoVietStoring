using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Reflection;

using SaoVietStoring.Views;
using SaoVietStoring.Helpers;

namespace SaoVietStoring
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static Mutex mutex;
        private App()
        {
            //System.Uri resourceLocater = new System.Uri("/SaoVietStoring;component/App.xaml", System.UriKind.Relative);
            //InitializeComponent();
        }

        [STAThread]
        private static void Main()
        {
            try
            {
                Mutex.OpenExisting("SaoVietStoring");
                MessageBox.Show("Application Running ....", "Sao Viet - Storing System", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            catch
            {
                App.mutex = new Mutex(true, "SaoVietStoring");
                App app = new App();
                app.Run(new MainWindow());
                mutex.ReleaseMutex();
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(Assembly.GetEntryAssembly().Location);
            // Get the connectionStrings section. 
            ConfigurationSection configSection = config.GetSection("connectionStrings");
            //Ensures that the section is not already protected.
            if (configSection.SectionInformation.IsProtected == false)
            {
                //Uses the Windows Data Protection API (DPAPI) to encrypt the configuration section using a machine-specific secret key.
                configSection.SectionInformation.ProtectSection(
                    "DataProtectionConfigurationProvider");
                config.Save();
            }
        }
    }
}
