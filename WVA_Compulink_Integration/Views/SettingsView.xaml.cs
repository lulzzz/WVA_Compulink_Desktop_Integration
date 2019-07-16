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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Memory;
using WVA_Compulink_Desktop_Integration.Utility.Actions;
using WVA_Compulink_Desktop_Integration.Utility.Files;
using WVA_Compulink_Desktop_Integration.ViewModels;

namespace WVA_Compulink_Desktop_Integration.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        SettingsViewModel settingsViewModel = new SettingsViewModel();

        public SettingsView()
        {
            InitializeComponent();
            SetUpUI();
            CheckActNumberVisibility();
            SetUpWvaAccountNumber();
        }

        private void SetUpUI()
        {
            try
            {
                DeleteBlankCompulinkOrdersCheckBox.IsChecked = UserData.Data?.Settings?.DeleteBlankCompulinkOrders ?? false;

                // Subscribe to AccountTextBox event delegate
                IsVisibleChanged += new DependencyPropertyChangedEventHandler(AvailableActsComboBox_IsVisibleChanged);
            }
            catch
            {
                DeleteBlankCompulinkOrdersCheckBox.IsChecked = false;
            }
        }
        
        private void CheckActNumberVisibility()
        {
            if (UserData.Data?.Settings?.BlockExternalLocations ?? false)
            {
                ActNumLabel.Visibility = Visibility.Hidden;
                AvailableActsComboBox.Visibility = Visibility.Hidden;
            }
        }

        private void SetUpWvaAccountNumber()
        {
            try
            {
                // Populate AvailableActComboBox with user's accounts
                List<string> availableActs = settingsViewModel.GetAvailableAccounts();

                // Check for nulls 
                if (availableActs == null || availableActs.ToString().Trim() == "" || availableActs?.Count < 1)
                    return;

                // Add accounts to combo box
                foreach (string account in availableActs)
                    AvailableActsComboBox.Items.Add(account);

                // Pull account number from file if its there
                string actNum = File.ReadAllText(Paths.ActNumFile).Trim();

                // Select their account number if it's been set already in the drop down
                for (int i = 0; i < availableActs.Count; i++)
                {
                    if (availableActs[i] == actNum)
                        AvailableActsComboBox.SelectedIndex = i;
                }
            }
            catch (FileNotFoundException)
            {
                if (!Directory.Exists(Paths.ActNumDir))
                    Directory.CreateDirectory(Paths.ActNumDir);

                if (!File.Exists(Paths.ActNumFile))
                    File.Create(Paths.ActNumFile);

                SetUpWvaAccountNumber();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        // Allow SearchTextBox to get focus
        void AvailableActsComboBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            try
            {
                if ((bool)e.NewValue == true)
                {
                    Dispatcher.BeginInvoke(
                    DispatcherPriority.ContextIdle,
                    new Action(delegate ()
                    {
                        AvailableActsComboBox.Focus();
                    }));
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void AvailableActsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (!File.Exists(Paths.ActNumFile))
                {
                    Directory.CreateDirectory(Paths.ActNumDir);
                    var actNumFile = File.Create(Paths.ActNumFile);
                    actNumFile.Close();
                }

                File.WriteAllText(Paths.ActNumFile, (sender as ComboBox).SelectedItem as string);
            }
            catch (Exception x)
            {
                NotifyLabel.Visibility = Visibility.Visible;
                Error.ReportOrLog(x);
            }
        }

        private void DeleteBlankCompulinkOrdersCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            settingsViewModel.UpdateUserSettings(deleteBlankCompulinkOrders: true);
        }

        private void DeleteBlankCompulinkOrdersCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            settingsViewModel.UpdateUserSettings(deleteBlankCompulinkOrders: false);
        }

        private void SendDebugDataButton_Click(object sender, RoutedEventArgs e)
        {
            ActionLogger.ReportAllDataNow();
        }
    }
}
