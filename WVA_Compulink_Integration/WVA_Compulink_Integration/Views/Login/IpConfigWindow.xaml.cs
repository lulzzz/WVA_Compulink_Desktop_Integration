using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.ViewModels.Login;

namespace WVA_Compulink_Desktop_Integration.Views.Login
{
    public partial class IpConfigWindow : Window
    {
        IpConfigViewModel ipConfigViewModel;

        public IpConfigWindow()
        {
            InitializeComponent();
            ipConfigViewModel = new IpConfigViewModel();
            IpConfigTextBox.Focus();
            CheckFields();
        }

        private void CheckFields()
        {
            try
            {
                string ipConfig = ipConfigViewModel.GetIpConfig();
                string apiKey = ipConfigViewModel.GetApiKey();
                string actNum = ipConfigViewModel.GetActNum();

                // Updates user settings to not show other account numbers if they don't want them to show up
                bool blockExternalLocations = string.IsNullOrWhiteSpace(ActNumTextBox.Text.ToString()) && actNum == "" ? false : true;
                ipConfigViewModel.UpdateSettingsAllowSingleAcct(blockExternalLocations);

                // Open login window if DSN and Api key has been set
                if (ipConfig.Trim() != "" && apiKey.Trim() != "")
                {
                    LoginWindow loginWindow = new LoginWindow();
                    loginWindow.Show();
                    Close();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void IpConfigTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    ipConfigViewModel.WriteToFiles(IpConfigTextBox.Text.Trim(), ApiKeyTextBox.Text.Trim(), ActNumTextBox.Text.Trim());
                    CheckFields();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void ApiKeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    ipConfigViewModel.WriteToFiles(IpConfigTextBox.Text.Trim(), ApiKeyTextBox.Text.Trim(), ActNumTextBox.Text.Trim());
                    CheckFields();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }
        private void ActNumTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    ipConfigViewModel.WriteToFiles(IpConfigTextBox.Text.Trim(), ApiKeyTextBox.Text.Trim(), ActNumTextBox.Text.Trim());
                    CheckFields();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            ipConfigViewModel.WriteToFiles(IpConfigTextBox.Text.Trim(), ApiKeyTextBox.Text.Trim(), ActNumTextBox.Text.Trim());
            CheckFields();
        }

    }
}
