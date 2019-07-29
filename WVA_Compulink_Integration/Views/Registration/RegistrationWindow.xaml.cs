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
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.ViewModels.Registration;
using WVA_Connect_CDI.Views.Login;

namespace WVA_Connect_CDI.Views.Registration
{
    public partial class RegistrationWindow : Window
    {
        RegistrationViewModel regViewModel;

        public RegistrationWindow()
        {
            try
            {
                InitializeComponent();
                regViewModel = new RegistrationViewModel();
                SetUp();
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
            }
        }

        private void SetUp()
        {
            EmailTextBox.Focus();
            ChangeToDefault();
        }

        private void Register()
        {
            try
            {
                // Pre API reponse check
                string password = PasswordTextBox.Password.ToString();
                string confirmPassword = ConfirmPasswordTextBox.Password.ToString();
                string email = EmailTextBox.Text;
                string username = UserNameTextBox.Text;

                if (!regViewModel.IsValidEmail(email))
                {
                    MessageSetup("Email is not valid!");
                    return;
                }
                // Check if password and confirm password match 
                else if (password != confirmPassword)
                {
                    MessageSetup("Passwords must match!");
                    return;
                }
                // Check for blank fields 
                else if (email.Trim() == "" || username.Trim() == "" || password.Trim() == "" || confirmPassword.Trim() == "")
                {
                    MessageSetup("Field cannot be blank!");
                    return;
                }
                // Make sure password is complex enough 
                else if (!regViewModel.IsComplexPassword(password))
                {
                    MessageSetup("Password must be a minimum of 8 characters, have one capital letter, and contain at least one number.");
                    return;
                }

                var registerReponse = regViewModel.RegisterUser(username, password, email);

                // Check if email exists
                if (registerReponse.Message == "Email already exists")
                    MessageSetup("Email is already in use!");
                if (registerReponse.Message == "UserName already exists")
                    MessageSetup("Username is already in use!");
                else if (registerReponse.Status == "ERROR")
                    throw new Exception($"Server responded with the following error while registering user: {registerReponse.Message}");
                else if (registerReponse.Status == "OK")
                {
                    UserData.Data = registerReponse;
                    new MainWindow().Show();
                    Close();
                }
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
                MessageSetup("An error has occurred. If the problem persists, please contact IT.");
            }
        }

        private void ChangeToDefault()
        {
            NotifyLabel.Visibility = Visibility.Hidden;
            Height = 360;
        }

        private void MessageSetup(string notifyMessage)
        {
            NotifyLabel.Visibility = Visibility.Visible;
            NotifyLabel.Text = notifyMessage;
            Height = 420;
        }

        private void BackToLogin()
        {
            new LoginWindow().Show();
            Close();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Register();
        }

        private void BackToLoginLink_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BackToLogin();
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
            }
        }

        private void ActNumTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeToDefault();
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeToDefault();
        }

        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeToDefault();
        }

        private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeToDefault();
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ChangeToDefault();
        }

        private void ConfirmPasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ChangeToDefault();
        }

        private void ConfirmPasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Register();
                }
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
            }
        }

        private void PasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Register();
                }
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
            }
        }

        private void UserNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Register();
                }
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
            }
        }

        private void EmailTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Register();
                }
            }
            catch (Exception x)
            {
                Errors.Error.ReportOrLog(x);
            }
        }

    }
}
