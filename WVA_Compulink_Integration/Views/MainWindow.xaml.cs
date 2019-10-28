using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.Utility.Files;
using WVA_Connect_CDI.ViewModels;
using WVA_Connect_CDI.Views.Login;

namespace WVA_Connect_CDI.Views
{
    public partial class MainWindow : Window
    {
        MainViewModel mainViewModel;
        private readonly BackgroundWorker CheckInactivityTimerWorker = new BackgroundWorker();

        [DllImport("user32.dll")]
        public static extern Boolean GetLastInputInfo(ref tagLASTINPUTINFO plii);

        public struct tagLASTINPUTINFO
        {
            public uint cbSize;
            public Int32 dwTime;
        }

        public MainWindow()
        {
            mainViewModel = new MainViewModel();

            InitializeComponent();
            mainViewModel.SetupDatabase();
            mainViewModel.CheckForProductPredictionUpdates();
            SetupRoleBasedFeatures();
            SetUpApp();
            Task.Run(() => StartInactivityTimer());
        }

        //
        // Check for user inacvitity 
        //

        private async void StartInactivityTimer()
        {
            bool shouldCheckForInactivity = true;

            while (shouldCheckForInactivity)
            {
                shouldCheckForInactivity = CheckUserInactivity();
                Thread.Sleep(5000);
            }
        }

        private bool CheckUserInactivity()
        {
            tagLASTINPUTINFO LastInput = new tagLASTINPUTINFO();
            Int32 IdleTime;

            LastInput.cbSize = (uint)Marshal.SizeOf(LastInput);
            LastInput.dwTime = 0;

            if (GetLastInputInfo(ref LastInput))
            {
                IdleTime = System.Environment.TickCount - LastInput.dwTime;
                Trace.WriteLine($"*** IdleTime: {IdleTime}ms ***");

                if (IdleTime > 300000)
                {
                    ForceLogOff();
                    return false;
                }
            }

            return true;
        }

        private void ForceLogOff()
        {
            Application.Current.Dispatcher.Invoke((Action)delegate {
                new LoginWindow().Show();
                this.Close();
            });
        }

        //
        // View Set Up
        //

        private void SetUpApp()
        {
            try
            {
                 string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

                 // Set app version at bottom of view
                 AppVersionLabel.Content = $"Version: {version}";

                 // Move Product Prediction database from AppData to Public docs in case user has an older version of the app
                 if (ShouldMoveProductPredictionDatabase(version))  MoveProductPredictionDatabase();

                // Set the main data context to the Compulink orders view if their account number is set
                if (mainViewModel.AccountNumAvailable())
                {
                    TryLoadOrderView();
                }
                else
                {
                    new MessageWindow("You must set your account number in the settings tab before continuing.").Show();
                    MainContentControl.DataContext = new SettingsViewModel();
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private bool ShouldMoveProductPredictionDatabase(string version)
        {
            try
            {
                return Convert.ToInt32(version.Replace(".", "")) < 1212 ? true : false;
            }
            catch
            {
                return false;
            }
        }

        private void MoveProductPredictionDatabase()
        {
            try
            {
                if (!File.Exists(AppPath.ProductDatabaseFile))
                {
                    Directory.CreateDirectory(AppPath.PublicDataDir);
                    string oldFileLocation = AppPath.AppDataLocal + @"\" + AppPath.AppName + @"\Data\ProductPrediction.sqlite";
                    File.Move(oldFileLocation, AppPath.ProductDatabaseFile);
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void SetupRoleBasedFeatures()
        {
            // SuperUser
            if (UserData.Data.RoleId == 3)
            {
               
            }
            // IT Admin
            else if (UserData.Data.RoleId == 2)
            {
                Environment.Exit(0); // We do not allow IT Admins to log in to this app
            }
            // Manager
            else if (UserData.Data.RoleId == 1)
            {

            }
            else
            {
                RemoveManageButtonFromNav();
            }
        }

        private void RemoveManageButtonFromNav()
        {
            NavButtonGroupStackPanel.Children.Remove(ManageButton);
        }

        //
        // Load Views
        //

        private async void TryLoadOrderView()
        {
            LoadingWindow loadingWindow = new LoadingWindow();

            try
            {
                // Spawn a loading window and change cursor to waiting cursor               
                loadingWindow.Show();
                Mouse.OverrideCursor = Cursors.Wait;

                // Load product list into memory for match algorithm. If products to not load, Notify user.              
                bool ProductsLoaded = await Task.Run(() => mainViewModel.LoadProductsAsync());

                if (!ProductsLoaded)
                {
                    // If products list did not load correctly, this error window will pop up and we leave the method, not opening the OrdersViewModel
                    new MessageWindow("WVA products not loaded! Please see error log for more details.").Show();
                    return;
                }

                MainContentControl.DataContext = new OrdersViewModel();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
            finally
            {
                // Close loading window and change cursor back to default arrow cursor
                loadingWindow.Close();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void LoadManageView()
        {
            MainContentControl.DataContext = new ManageViewModel();
        }

        //
        // UI Events
        //

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            SetUpApp();
        }

        private void ManageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadManageView();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainContentControl.DataContext = new SettingsView();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            string location = GetType().FullName + nameof(Window_Closing);
            string actionMessage = "<Exiting>";
            ActionLogger.Log(location, actionMessage);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Send data collection of user activity for statistics and diagnosing user problems
            mainViewModel.ReportActionLogData();

            string location = GetType().FullName + nameof(Window_Loaded);
            string actionMessage = "<App_Launch>";
            ActionLogger.Log(location, actionMessage);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            string location = GetType().FullName + nameof(Window_Closed);
            string actionMessage = " <App_Exit>\n";
            ActionLogger.Log(location, actionMessage);
        }

    }
}
