using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;
using WVA_Compulink_Desktop_Integration.Utility.Files;
using WVA_Compulink_Desktop_Integration.ViewModels;

namespace WVA_Compulink_Desktop_Integration.Views.Login
{
    /// <summary>
    /// Interaction logic for AccountSetupWindow.xaml
    /// </summary>
    public partial class AccountSetupWindow : Window
    {
        public AccountSetupWindow()
        {
            InitializeComponent();
            SetUpAvailableActsFile();
            if (AvailableAccountsSet())
            {
                GoToLogin();
            }
            else
            {
                SetUpAvailableAccounts();
                Show();
            }
        }

        private void SetUpAvailableActsFile()
        {
            if (!Directory.Exists(Paths.ActNumDir))
                Directory.CreateDirectory(Paths.ActNumDir);

            if (!File.Exists(Paths.AvailableActsFile))
                File.Create(Paths.AvailableActsFile).Close();
        }

        private void SetUpAvailableAccounts()
        {
            string dsn = File.ReadAllText(Paths.IpConfigFile).Trim();
            List<string> availableAccounts = new SettingsViewModel().GetAllAvailableAccounts(dsn);

            foreach (string account in availableAccounts)
            {
                var item = new AvailableAccount()
                {
                    IsChecked = false,
                    AccountNumber = account
                };

                AvailableActsTable.Items.Add(item);
            }
        }

        private void SetSelectedAccounts()
        {
            foreach (AvailableAccount row in AvailableActsTable.Items)
                if (row.IsChecked)
                    File.AppendAllText(Paths.AvailableActsFile, row.AccountNumber + "\n");
        }

        private bool AvailableAccountsSet()
        {
            var accounts = File.ReadAllLines(Paths.AvailableActsFile).ToList();
                accounts.RemoveAll(x => string.IsNullOrWhiteSpace(x));

            return accounts.Count > 0 ? true : false;
        }

        private void GoToLogin()
        {
            new LoginWindow().Show();
            Close();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(Paths.AvailableActsFile, ""); // Reset the file since SetSelectedAccounts appends to any existing text
            SetSelectedAccounts();

            if (AvailableAccountsSet())
                GoToLogin();
            else
                MessageBox.Show("You must select at least one account number", "", MessageBoxButton.OK);
        }

        private void AvailableActsTable_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }

    public class AvailableAccount
    {
        public string AccountNumber { get; set; }
        public bool IsChecked { get; set; }
    }


}
