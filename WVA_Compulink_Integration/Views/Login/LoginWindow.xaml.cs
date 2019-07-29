using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Users;
using WVA_Connect_CDI.ViewModels.Login;
using WVA_Connect_CDI.Views.Registration;

namespace WVA_Connect_CDI.Views.Login
{
    public partial class LoginWindow : Window
    {
        LoginViewModel loginViewModel;

        public LoginWindow()
        {
            CloseExistingInstancesOfApp();
            InitializeComponent();
            loginViewModel = new LoginViewModel();
            SetUp();
        }

        private void SetUp()
        {
            UsernameTextBox.Focus();
            loginViewModel.DelTimePassChangedFile();
        }

        private void CloseExistingInstancesOfApp()
        {
            try
            {
                if (Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location)).Count() > 1)
                {
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void Login()
        {
            try
            {
                // Make NotifyLabel visible if necessary.
                NotifyLabel.Visibility = Visibility.Visible;

                // Verify user's credentials through the api and return verifiedUser object. 
                User loginUserResponse = loginViewModel.LoginUser(UsernameTextBox.Text, PasswordTextBox.Password);

                if (loginUserResponse == null)
                    throw new Exception("Null response from endpoint while logging in user!");

                // Check login credentials                 
                if (loginUserResponse.Status == "ERROR" || loginUserResponse.Status == "FAIL")
                {
                    NotifyLabel.Visibility = Visibility.Visible;
                    NotifyLabel.Text = $"{loginUserResponse.Message}";
                    return;
                }
                else if (loginUserResponse.Status == "OK")
                {
                    // Set user data in memory to response items                   
                    UserData.Data = loginUserResponse;
                    UserData.Data.Settings = loginViewModel.GetUserSettings();

                    // Let user continue into application
                    new MainWindow().Show();
                    Close();
                }
                else
                    throw new Exception("Server was unable to provide a sufficient response.");
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                NotifyLabel.Visibility = Visibility.Visible;
                NotifyLabel.Text = "An error has occurred. If the problem persists, please contact IT.";
            }
        }

        private void BackToLogin()
        {
            new RegistrationWindow().Show();
            Close();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ForgotPasswordLink_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (loginViewModel.PasswordChangedRecently())
                {
                    new MessageWindow("You have changed your password recently. Try again shortly.").Show();
                    return;
                }

                ForgotPasswordWindow forgotPasswordWindow = new ForgotPasswordWindow();

                // If window is open, bring to front, else open it
                if (forgotPasswordWindow.IsActive)
                    forgotPasswordWindow.Topmost = true;
                else
                    forgotPasswordWindow.Show();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void CreateAccountLink_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                BackToLogin();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            NotifyLabel.Visibility = Visibility.Hidden;
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            NotifyLabel.Visibility = Visibility.Hidden;
        }

        private void UsernameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }

        private void PasswordTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Login();
        }

    }
}
