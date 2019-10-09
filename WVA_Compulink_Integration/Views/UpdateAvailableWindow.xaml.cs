using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WVA_Connect_CDI.Updates;
using WVA_Connect_CDI.Views.Login;

namespace WVA_Connect_CDI.Views
{
    /// <summary>
    /// Interaction logic for UpdateAvailableWindow.xaml
    /// </summary>
    public partial class UpdateAvailableWindow : Window
    {
        public UpdateAvailableWindow()
        {
            InitializeComponent();
        }

        private void GoToLogin()
        {
            new LoginWindow().Show();
            Close();
        }

        private async void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            YesButton.IsEnabled = false;

            // Hide this window from view so the loading window doesn't over lap it 
            Hide();

            // Open a loading window
            LoadingWindow loadingWindow = new LoadingWindow("Installing Update...");
            loadingWindow.Show();

            // Install updates async
            Task.Run(() => Updater.CheckForUpdates());

            // Wait 5 seconds for update to install files
            await Task.Run(() => Thread.Sleep(5000));

            // Close loading window 
            loadingWindow.Close();

            YesButton.IsEnabled = true;
            Cursor = Cursors.Arrow;

            // Open login window and close this window 
            GoToLogin();
        }

        private void NotNowButton_Click(object sender, RoutedEventArgs e)
        {
            GoToLogin();
        }
    }
}
