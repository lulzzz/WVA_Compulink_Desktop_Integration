using Newtonsoft.Json;
using System;
using System.Collections;
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
using System.Windows.Threading;
using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.ViewModels;
using WVA_Connect_CDI.ViewModels.Orders;
using WVA_Connect_CDI.Models.ProductParameters.Derived;

namespace WVA_Connect_CDI.Views.Orders
{
    public partial class CompulinkOrders : UserControl
    {
        ToolTip toolTip = new ToolTip();
        List<Prescription> ListPrescriptions = new List<Prescription>();

        public CompulinkOrders()
        {
            InitializeComponent();
            SetUp();
        }

        private void SetUp()
        {
            //OrdersDataGrid.ItemsSource = Memory.Orders.CompulinkOrders;
            IsVisibleChanged += new DependencyPropertyChangedEventHandler(LoginControl_IsVisibleChanged);
            SetUpOrdersDataGrid();
            SetDefaultOrderName();
            GetWvaOrders();
        }

        private void SetDefaultOrderName()
        {
            // Autofill new order display
            WvaOrdersComboBox.Text = RemoveUnsafeChars($"WVA Order ({UserData.Data.UserName})-{DateTime.Now.ToString("MM/dd/yy--hh:mm:ss")}");
        }

        // Set up items in OrdersDataGrid 
        private async void SetUpOrdersDataGrid()
        {
            try
            {
                Memory.Orders.CompulinkOrders.Clear();
                ListPrescriptions.Clear();

                var prescriptionWrapper = await GetCompulinkOrders();

                if (prescriptionWrapper == null)
                    throw new NullReferenceException("Response returned null from endpoint while getting Compulink orders.");

                var products = prescriptionWrapper?.Request?.Products;

                foreach (Prescription prescription in products)
                {
                    var p = prescription;

                    // Clean up Compulink data
                    p.AccountNumber = UserData.Data.Account;
                    p._CustomerID.Value = prescription?._CustomerID?.Value ?? null;
                    p.BaseCurve = prescription?.BaseCurve?.Trim()?.Replace(" ", "");
                    p.Diameter = prescription?.Diameter?.Trim()?.Replace(" ", "");
                    p.Add = prescription?.Add?.Trim()?.Replace(" ", "");
                    p.Axis = prescription?.Axis?.Trim()?.Replace(" ", "");
                    p.Multifocal = prescription?.Multifocal?.Trim()?.Replace(" ", "");
                    p.Sphere = prescription?.Sphere?.Trim()?.Replace(" ", "");
                    p.Cylinder = prescription?.Cylinder?.Trim()?.Replace(" ", "");

                    Memory.Orders.CompulinkOrders.Add(p);
                }

                ListPrescriptions.AddRange(Memory.Orders.CompulinkOrders);

                // Set available accounts based on user settings
                var accounts = new List<string>();

                accounts = new SettingsViewModel().GetAvailableAccounts();

                SelAcctNumber.ItemsSource = accounts;

                // Add updated rows to grid
                OrdersDataGrid.ItemsSource = ListPrescriptions;
                OrdersDataGrid.Items.Refresh();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        // Get wva orders for this user 
        private void GetWvaOrders()
        {
            try
            {
                // Get this account's open wva orders
                string dsn = UserData.Data.DSN;
                string endpoint = $"http://{dsn}/api/order/get-names/" + UserData.Data?.Account;
                string strNames = API.Get(endpoint, out string httpStatus);

                if (strNames == null || strNames.ToString().Trim() == "")
                    throw new NullReferenceException("Response null ");

                // Break if bad response
                if (httpStatus != "OK")
                    throw new Exception($"Http status: {httpStatus.ToString()} from server while getting order names!");

                Dictionary<string, string> dictOrderNames = JsonConvert.DeserializeObject<Dictionary<string, string>>(strNames);

                // Put account's open orders in the drop down
                if (dictOrderNames?.Count > 0)
                {
                    foreach (string orderName in dictOrderNames?.Values)
                        WvaOrdersComboBox.Items.Add(orderName);
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        // Asyncronously get Compulink Orders from server
        private async Task<PrescriptionWrapper> GetCompulinkOrders()
        {
            try
            {
                string dsn = UserData.Data.DSN;
                string actNum = UserData.Data.Account;
                string endpoint = $"http://{dsn}/api/openorder/{actNum}";
                string strPrescriptions = API.Get(endpoint, out string httpStatus);

                if (strPrescriptions == null || strPrescriptions.Trim() == "")
                    throw new NullReferenceException("Response from open orders is null or empty.");

                // Break if bad response
                if (httpStatus != "OK")
                    throw new Exception($"Http status: {httpStatus.ToString()} from server while getting order names!");

                var prescriptionWrapper = JsonConvert.DeserializeObject<PrescriptionWrapper>(strPrescriptions);

                // Remove any unsafe sql characters from compulink data
                for (int i = 0; i < prescriptionWrapper.Request.Products.Count; i++)
                {
                    var prescription = prescriptionWrapper.Request.Products[i];

                    if (prescription.IsTrial)
                        prescription.Product += " TRIAL ";

                    prescription._CustomerID.Value = RemoveUnsafeChars(prescription._CustomerID.Value);
                    prescription.Patient = RemoveUnsafeChars(prescription.Patient);
                    prescription.Add = RemoveUnsafeChars(prescription.Add);
                    prescription.BaseCurve = RemoveUnsafeChars(prescription.BaseCurve);
                    prescription.Color = RemoveUnsafeChars(prescription.Color);
                    prescription.Cylinder = RemoveUnsafeChars(prescription.Cylinder);
                    prescription.Axis = RemoveUnsafeChars(prescription.Axis);
                    prescription.Diameter = RemoveUnsafeChars(prescription.Diameter);
                    prescription.Eye = RemoveUnsafeChars(prescription.Eye);
                    prescription.FirstName = RemoveUnsafeChars(prescription.FirstName);
                    prescription.LastName = RemoveUnsafeChars(prescription.LastName);
                    prescription.Multifocal = RemoveUnsafeChars(prescription.Multifocal);
                    prescription.Sphere = RemoveUnsafeChars(prescription.Sphere);
                }

                return prescriptionWrapper;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        private async Task<PrescriptionWrapper> GetTestCompulinkOrders()
        {
            try
            {
                var wrapper = new PrescriptionWrapper()
                {
                    Request = new PrescriptionRequest()
                    {
                        Products = new List<Prescription>()
                        {
                            new Prescription()
                            {
                                _CustomerID = new CustomerID() { Value = "75532" },
                                FirstName = "Evan",
                                LastName = "Taylor",
                                Eye = "R",
                                Product = "Air optix sphere",
                                Quantity = "1",
                                BaseCurve = "8.6"
                            },
                            new Prescription()
                            {
                                _CustomerID = new CustomerID() { Value = "75532" },
                                FirstName = "Evan",
                                LastName = "Taylor",
                                Eye = "L",
                                Product = "Air optix sphere",
                                Quantity = "1",
                                BaseCurve = "8.6"
                            }
                        }
                    }   
                };

                return wrapper;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        // Filters out orders in table by whatever patient they search for in the searc box
        private void SearchOrders(string searchString)
        {
            try
            {
                ListPrescriptions.Clear();

                List<Prescription> tempList = Memory.Orders.CompulinkOrders.Where(x => x.Patient.ToLower().Replace(",", "").StartsWith(searchString.ToLower().Replace(",", ""))).ToList();

                foreach (Prescription prescription in tempList)
                    ListPrescriptions.Add(prescription);

                OrdersDataGrid.Items.Refresh();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private string RemoveUnsafeChars(string originalString)
        {
            return originalString.Replace("<", "")
                                 .Replace(">", "")
                                 .Replace("'", "")
                                 .Replace("\"", "")
                                 .Replace("|", "")
                                 .Replace(";", "")
                                 .Replace("\\", "")
                                 .Replace("~", "")
                                 .Replace("{", "")
                                 .Replace("}", "")
                                 .Replace("[", "")
                                 .Replace("]", "")
                                 .Replace("%", "")
                                 .Replace("*", "")
                                 .Replace("=", "")
                                 .Replace("^", "")
                                 .Replace("$", "")
                                 .Replace("+", "")
                                 .Replace("?", "")
                                 .Replace("&", "");
        }

        private void LoadView(OrdersView view)
        {
            // Jumps back to parent windows and changes user control
            foreach (Window window in Application.Current.Windows)
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = view;
        }

        private List<Prescription> GetSelectedPrescriptions()
        {
            // Get order name, selected row in table, and patient object attatched to row
            IList rows = OrdersDataGrid.Items;
            var prescriptions = new List<Prescription>();

            // Get products checked in the grid's first columns
            foreach (Prescription prescription in rows)
                if (prescription.IsChecked)
                    prescriptions.Add(prescription);

            return prescriptions;
        }

        // Allow SearchTextBox to get focus
        void LoginControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue == true)
            {
                Dispatcher.BeginInvoke(
                DispatcherPriority.ContextIdle,
                new Action(delegate ()
                {
                    SearchTextBox.Focus();
                }));
            }
        }

        // Search Text Box 
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchOrders(SearchTextBox.Text);
        }

        // Refresh Button
        private void RefreshButton_MouseEnter(object sender, MouseEventArgs e)
        {
            // Open tool tip on mouseover, and change image color
            toolTip.Content = "Refresh Content";
            toolTip.IsOpen = true;
            RefreshImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-available-updates-48.png", UriKind.Relative));
        }

        private void RefreshButton_MouseLeave(object sender, MouseEventArgs e)
        {
            // Close tool tip on mouseleave, and change image color
            toolTip.IsOpen = false;
            RefreshImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-available-updates-filled-48.png", UriKind.Relative));
        }

        // Get data from compulink and refresh table data
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            // Spawn a loading window and change cursor to waiting cursor
            LoadingWindow loadingWindow = new LoadingWindow();
            loadingWindow.Show();
            Mouse.OverrideCursor = Cursors.Wait;

            SearchTextBox.Text = "";
            SetUpOrdersDataGrid();
            SetDefaultOrderName();

            // Close loading window and change cursor back to default arrow cursor
            loadingWindow.Close();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        // Create Order Button
        private void AddToOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var listPrescriptions = GetSelectedPrescriptions();

                // Make sure at least one item is selected for order creation 
                if (listPrescriptions.Count < 1)
                {
                    MessageBox.Show("You must select at least one Compulink order. \nClick checkbox in the (+) column to add it to the order.", "Compulink Integration", MessageBoxButton.OK);
                }
                else
                {
                    // Determine if order is a batch order and should be split out 
                    bool isBatchOrder = new CompulinkOrdersViewModel().IsBatchOrder(listPrescriptions); ;

                    if (isBatchOrder && OrderCreationViewModel.OrderExists(WvaOrdersComboBox.Text))
                    {
                        MessageBox.Show("Cannot add a batch order to an existing WVA order!", "Warning", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                        return;
                    }

                    // Load a different order creation view based on the items the user adds to the order
                    if (isBatchOrder)
                        LoadView(new OrdersView(listPrescriptions, RemoveUnsafeChars(WvaOrdersComboBox.Text), "BatchOrderCreation"));  // Order items, order name, action to run in orders view
                    else
                        LoadView(new OrdersView(listPrescriptions, RemoveUnsafeChars(WvaOrdersComboBox.Text), "OrderCreation"));
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
            finally
            {
                string location = GetType().FullName + "." + nameof(AddToOrderButton_Click);
                string actionMessage = $"<OrderName=<{WvaOrdersComboBox.Text}>";
                ActionLogger.Log(location, actionMessage);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(UserControl_Loaded);
            ActionLogger.Log(location);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(UserControl_Unloaded);
            ActionLogger.Log(location);
        }

    }
}
