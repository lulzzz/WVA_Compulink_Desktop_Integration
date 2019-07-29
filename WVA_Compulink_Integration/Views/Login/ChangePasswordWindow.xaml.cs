using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Models.Responses;
using WVA_Connect_CDI.ViewModels.Login;

namespace WVA_Connect_CDI.Views.Login
{
    public partial class ChangePasswordWindow : Window
    {
        private string DSN { get; set; }
        private string UserName { get; set; }
        ChangePasswordViewModel changePasswordViewModel;

        public ChangePasswordWindow(string userName)
        {
            InitializeComponent();
            changePasswordViewModel = new ChangePasswordViewModel();
            UserName = userName;
            DSN = changePasswordViewModel.GetDSN();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SubmitCodeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Make sure passwords match
                if (PasswordTextBox.Password != PasswordConfTextBox.Password)
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Text = "Passwords must match!";
                    return;
                }

                // Make sure password is complex
                if (!changePasswordViewModel.IsComplexPassword(PasswordTextBox.Password))
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Text = "Password must be a minimum of 8 characters, have one capital letter, and contain at least one number.";
                    Height = 350;
                    return;
                }

                // Make request to change password
                Response response = changePasswordViewModel.ChangePassword(DSN, UserName, PasswordTextBox.Password);

                if (response == null)
                    throw new Exception("Null response from endpoint while changing password!");

                if (response?.Status == "SUCCESS")
                {
                    changePasswordViewModel.LogTimePassChanged();
                    new MessageWindow("\t\tPassword updated!").Show();
                    Close();
                }
                else
                    throw new Exception($"Bad response from endpoint. Status: {response?.Status} -- Message: {response?.Message}.");
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                MessageLabel.Visibility = Visibility.Visible;
                MessageLabel.Text = "An error has occurred. Password not updated. If this problem persists, please contact IT.";
                Height = 350;
            }
        }

    }
}
