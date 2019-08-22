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
        private bool ShouldRedirectToMainWindow { get; set; } = false;
        ChangePasswordViewModel changePasswordViewModel;

        // If view instantiated under this constructor, force this view to spawn a new 
        // instance of MainWindow after password has been successfully changed
        public ChangePasswordWindow(string userName, bool redirectToMainWindow = false)
        {
            ShouldRedirectToMainWindow = redirectToMainWindow;
            InitializeComponent();
            changePasswordViewModel = new ChangePasswordViewModel();
            UserName = userName;
            DSN = changePasswordViewModel.GetDSN();
        }

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
                    Height = 370;
                    return;
                }

                // Make sure password is complex
                if (!changePasswordViewModel.IsComplexPassword(PasswordTextBox.Password))
                {
                    MessageLabel.Visibility = Visibility.Visible;
                    MessageLabel.Text = "Password must be a minimum of 8 characters, have one capital letter, and contain at least one number.";
                    Height = 370;
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

                    // Opens MainWindow if this view has been notified to redirect to main window
                    if (ShouldRedirectToMainWindow)
                        new MainWindow().Show();

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
