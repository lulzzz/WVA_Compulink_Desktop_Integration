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
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Models.Responses;
using WVA_Connect_CDI.Models.Users;
using WVA_Connect_CDI.ViewModels.Login;

namespace WVA_Connect_CDI.Views.Login
{
    public partial class ForgotPasswordWindow : Window
    {
        private string UserEmail { get; set; }
        private string API_Key { get; set; }
        private string DSN { get; set; }
        ForgotPasswordViewModel forgotPasswordViewModel;

        public ForgotPasswordWindow()
        {
            InitializeComponent();
            forgotPasswordViewModel = new ForgotPasswordViewModel();
            API_Key = forgotPasswordViewModel.GetApiKey();
            DSN = forgotPasswordViewModel.GetDSN();
        }

        // Brings window to front without overlapping any following windows opened by user
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Topmost = true;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            Topmost = false;
            MessageLabel.Visibility = Visibility.Hidden;
        }

        private void ShowError()
        {
            MessageLabel.Visibility = Visibility.Visible;
            MessageLabel.Content = "Error sending email!";
        }

        private void SendEmailButton_Click(object sender, RoutedEventArgs e)
        {
            UserEmail = "";

            try
            {
                // Make sure username is set
                if (UserNameTextBox.Text.Trim() == "")
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Content = "Enter username!";
                    return;
                }

                User userResponse = forgotPasswordViewModel.GetUserEmail(UserNameTextBox.Text, DSN);

                if (userResponse?.Email != null || userResponse?.Email?.Trim() != "")
                    UserEmail = userResponse?.Email;
                else
                    throw new Exception($"Unable to get email for user: {UserNameTextBox.Text}.");

                if (UserEmail == null || UserEmail == "")
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Content = "Bad response from server!";
                    return;
                }

                Response response = forgotPasswordViewModel.SendEmail(DSN, UserEmail, API_Key);

                if (response == null)
                    throw new Exception("Null response from endpoint while sending email!");

                if (response?.Status == "SUCCESS")
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Content = "Code sent to email!";
                }
                else
                    throw new Exception($"Bad response from email reset endpoint. Status: {response.Status} -- Message: {response.Message}");
            }
            catch (Exception x)
            {
                ShowError();
                Error.ReportOrLog(x);
            }
        }

        private void SubmitCodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Make sure username is set
                if (UserNameTextBox.Text.Trim() == "")
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Content = "Enter username!";
                    return;
                }

                Response response = forgotPasswordViewModel.ResetEmailCheck(DSN, UserEmail, CodeTextBox.Text.Trim(), API_Key);

                if (response == null)
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Content = "An error has occurred!";
                }
                else if (response.Status == "SUCCESS")
                {
                    new ChangePasswordWindow(UserNameTextBox.Text.Trim()).Show();
                    Close();
                }
                else
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Content = "Invalid Code!";
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void CodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageLabel.Visibility = Visibility.Hidden;
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            MessageLabel.Visibility = Visibility.Hidden;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
