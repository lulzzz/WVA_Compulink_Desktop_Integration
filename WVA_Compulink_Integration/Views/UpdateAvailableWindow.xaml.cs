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

        private async void WaitForUpdate()
        {
            var task = new Task(() => WaitSeconds(5));
            task.Wait();
        }

        private async void WaitSeconds(int seconds)
        {
            for (int i = 0; i < seconds; i++)
                Thread.Sleep(1000);
        }


        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            YesButton.IsEnabled = false;

            // Start running loading window in the background
            Task.Run(() => Updater.CheckForUpdates());

            var loadingWindow = new LoadingWindow();
            loadingWindow.Show();

            WaitForUpdate();

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
