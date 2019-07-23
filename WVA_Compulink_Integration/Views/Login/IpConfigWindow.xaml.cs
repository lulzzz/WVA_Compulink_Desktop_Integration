﻿using System;
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
using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Utility.Files;
using WVA_Compulink_Desktop_Integration.ViewModels;
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
            DeleteOldApp();
        }
    
        private void CheckFields()
        {
            try
            {
                string ipConfig = ipConfigViewModel.GetIpConfig();
                string apiKey = ipConfigViewModel.GetApiKey();
                string actNum = ipConfigViewModel.GetActNum();

                // Updates user settings to not show other account numbers if they don't want them to show up
                if (!string.IsNullOrWhiteSpace(ActNumTextBox.Text.ToString()))
                {
                    ipConfigViewModel.BlockExternalLocations(true);
                }

                // Open login window if DSN and Api key has been set
                if (ipConfig.Trim() != "" && apiKey.Trim() != "")
                {
                    new AccountSetupWindow();
                    Close();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void DeleteOldApp()
        {
            try
            {
                // Delete shortcut
                if (File.Exists(Paths.DesktopDir + "\\FentonEC_Desktop.lnk"))
                    File.Delete(Paths.DesktopDir + "\\FentonEC_Desktop.lnk");

                // Delete all remains of the disgusting FentonEC desktop integration
                if (Directory.Exists(Paths.AppDataLocal + @"\FentonEC_Desktop"))
                {
                    foreach (FileInfo file in new DirectoryInfo(Paths.AppDataLocal + @"\FentonEC_Desktop").GetFiles())
                        file.Delete();

                    foreach (DirectoryInfo dir in new DirectoryInfo(Paths.AppDataLocal + @"\FentonEC_Desktop").GetDirectories())
                        dir.Delete(true);

                    if (Directory.Exists(Paths.AppDataLocal + @"\FentonEC_Desktop"))
                        Directory.Delete(Paths.AppDataLocal + @"\FentonEC_Desktop");
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
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
