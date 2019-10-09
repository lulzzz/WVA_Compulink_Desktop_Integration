﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.Utility.Files;
using WVA_Connect_CDI.ViewModels;

namespace WVA_Connect_CDI.Views
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
            SetUpWvaAccountNumber();
            SetUpUI();
        }

        private void SetUpUI()
        {
            try
            {
                // Set check box values equal to current user settings 
                DeleteBlankCompulinkOrdersCheckBox.IsChecked = UserData.Data?.Settings?.DeleteBlankCompulinkOrders ?? false;
                AutoFillProductNamesCheckBox.IsChecked = UserData.Data?.Settings?.AutoFillLearnedProducts ?? true;
                AutoUpdateCheckBox.IsChecked = UserData.Data?.Settings?.AutoUpdate ?? true;

                // Subscribe to AccountTextBox event delegate
                IsVisibleChanged += new DependencyPropertyChangedEventHandler(AvailableActsComboBox_IsVisibleChanged);

                // If user role is a manager or above, 
                if (UserData.Data?.RoleId > 1)
                {
                    LoadUpdateAct();
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void LoadUpdateAct()
        {
            UpdateActNumStackPanel.Visibility = Visibility.Visible;
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
                {
                    if (!AvailableActsComboBox.Items.Contains(account))
                        AvailableActsComboBox.Items.Add(account);
                }

                // Pull account number from file if its there
                string actNum = File.ReadAllText(AppPath.ActNumFile).Trim();

                // Select their account number if it's been set already in the drop down
                for (int i = 0; i < availableActs.Count; i++)
                {
                    if (availableActs[i] == actNum)
                        AvailableActsComboBox.SelectedIndex = i;
                }
            }
            catch (FileNotFoundException)
            {
                if (!Directory.Exists(AppPath.ActNumDir))
                    Directory.CreateDirectory(AppPath.ActNumDir);

                if (!File.Exists(AppPath.ActNumFile))
                    File.Create(AppPath.ActNumFile);

                SetUpWvaAccountNumber();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        // Allow SearchTextBox to get focus
        private void AvailableActsComboBox_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
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
                if (!File.Exists(AppPath.ActNumFile))
                {
                    Directory.CreateDirectory(AppPath.ActNumDir);
                    var actNumFile = File.Create(AppPath.ActNumFile);
                    actNumFile.Close();
                }

                File.WriteAllText(AppPath.ActNumFile, (string)AvailableActsComboBox.SelectedValue);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void DeleteBlankCompulinkOrdersCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Get current user settings
            var userSettings = UserData.Data.Settings;

            // Update the settings object to have DeleteBlankCompulinkOrders set to 'true'
            userSettings.DeleteBlankCompulinkOrders = true;

            // Update the user settings in memory and in the save file
            settingsViewModel.UpdateUserSettings(userSettings);
        }

        private void DeleteBlankCompulinkOrdersCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Get current user settings
            var userSettings = UserData.Data.Settings;

            // Update the settings object to have DeleteBlankCompulinkOrders set to 'false'
            userSettings.DeleteBlankCompulinkOrders = false;

            // Update the user settings in memory and in the save file
            settingsViewModel.UpdateUserSettings(userSettings);
        }

        private void AutoFillProductNamesCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Get current user settings
            var userSettings = UserData.Data.Settings;

            // Update the settings object to have AutoFillLearnedProducts set to 'true'
            userSettings.AutoFillLearnedProducts = true;

            // Update the user settings in memory and in the save file
            settingsViewModel.UpdateUserSettings(userSettings);
        }

        private void AutoFillProductNamesCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Get current user settings
            var userSettings = UserData.Data.Settings;

            // Update the settings object to have AutoFillLearnedProducts set to 'false'
            userSettings.AutoFillLearnedProducts = false;

            // Update the user settings in memory and in the save file
            settingsViewModel.UpdateUserSettings(userSettings);
        }

        private void AutoUpdateCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            // Get current user settings
            var userSettings = UserData.Data.Settings;

            // Update the settings object to have AutoUpdate set to 'true'
            userSettings.AutoUpdate = true;

            // Update the user settings in memory and in the save file
            settingsViewModel.UpdateUserSettings(userSettings);
        }

        private void AutoUpdateCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            // Get current user settings
            var userSettings = UserData.Data.Settings;

            // Update the settings object to have AutoUpdate set to 'false'
            userSettings.AutoUpdate = false;

            // Update the user settings in memory and in the save file
            settingsViewModel.UpdateUserSettings(userSettings);
        }

        private void UpdateActBtn_Click(object sender, RoutedEventArgs e)
        {
            settingsViewModel.AddAvailableAccount(UpdateActTextBox.Text);
            UpdateActTextBox.Text = "";
            SetUpWvaAccountNumber();
        }

       
    }
}
