using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Orders.Out;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.ViewModels.Orders;
using WVA_Connect_CDI.WebTools;

namespace WVA_Connect_CDI.Views.Orders
{
    public partial class WVAOrders : UserControl
    {

        ToolTip toolTip = new ToolTip();
        List<Order> ListOrders { get; set; }

        public WVAOrders()
        {
            InitializeComponent();
            SetUp();
        }

        // Do any setup after loading the view elements
        private void SetUp()
        {
            IsVisibleChanged += new DependencyPropertyChangedEventHandler(LoginControl_IsVisibleChanged);  // Subscribe to LoginControl_IsVisibleChanged Event (This just sets the cursor on the search box)
            RefreshOrders();
        }

        private string GetShippingString(string shipID)
        {
            switch (shipID)
            {
                case "1":
                    return "Standard";
                case "D":
                    return "UPS Ground";
                case "J":
                    return "UPS 2nd Day Air";
                case "P":
                    return "UPS Next Day Air";
                default:
                    return shipID;
            }
        }

        private void SearchOrders(string searchString)
        {
            List<Order> tempList = new List<Order>();

            try
            {
                switch (SearchFilterComboBox.SelectedIndex)
                {
                    case 0:
                        tempList = ListOrders.Where(x => x.WvaStoreID.ToLower().Contains(searchString.ToLower())).ToList();
                        break;
                    case 1:
                        tempList = ListOrders.Where(x => x.OrderName.ToLower().Contains(searchString.ToLower())).ToList();
                        break;
                    case 2:
                        tempList = ListOrders.Where(x => x.CreatedDate.ToLower().Contains(searchString.ToLower())).ToList();
                        break;
                    case 3:
                        tempList = ListOrders.Where(x => x.ShipToPatient.ToLower().Contains(searchString.ToLower())).ToList();
                        break;
                    case 4:
                        tempList = ListOrders.Where(x => x.PoNumber.ToLower().Contains(searchString.ToLower())).ToList();
                        break;
                    case 5:
                        tempList = ListOrders.Where(x => x.OrderedBy.ToLower().Contains(searchString.ToLower())).ToList();
                        break;
                    case 6:
                        tempList = ListOrders.Where(x => x.Status.ToLower().Contains(searchString.ToLower())).ToList();
                        break;
                }

                WvaOrdersDataGrid.Items.Clear();
                foreach (Order order in tempList)
                {
                    WvaOrdersDataGrid.Items.Add(order);
                }
            }
            catch { }
        }

        //  Return this account's orders from the server 
        private List<Order> GetWVAOrders()
        {
            try
            {
                string dsn = UserData.Data.DSN;
                string endpoint = $"http://{dsn}/api/order/get-orders/" + $"{UserData.Data.Account}";
                string strOrders = API.Get(endpoint, out string httpStatus);

                if (strOrders == null || strOrders == "")
                    throw new NullReferenceException();

                return JsonConvert.DeserializeObject<List<Order>>(strOrders);
            }
            catch
            {
                return null;
            }
        }

        // Removes all orders from list that are older than 8 days
        private List<Order> RemoveOldOrders(List<Order> orders)
        {
            var returnList = new List<Order>();

            foreach (Order order in orders)
            {
                if (order.CreatedDate != null && order.CreatedDate.Trim() != "")
                {
                    DateTime cutoffDate = DateTime.Now.AddDays(-8);
                    var cInfo = CultureInfo.CreateSpecificCulture("en-US");
                    DateTime orderCreatedDate = DateTime.ParseExact(order.CreatedDate, "yyyy-MM-dd-HH:mm:ss", cInfo);

                    // Don't return order if it is submitted and older than 8 days
                    if (orderCreatedDate < cutoffDate && order.Status == "submitted")
                        continue;
                }

                // Find number of items in the order and set the quantity
                int quantity = 0;
                foreach (var detail in order.Items)
                {
                    try { quantity += Convert.ToInt32(detail.Quantity); }
                    catch { continue; }
                }

                order.Quantity = quantity;
                order.ShippingMethod = GetShippingString(order.ShippingMethod);

                returnList.Add(order);
            }

            // Reverse order of the list so that more recent orders show up first 
            returnList.Reverse(0, returnList.Count);

            return returnList;
        }

        private List<Order> GetFilteredOrders()
        {
            return RemoveOldOrders(GetWVAOrders());
        }

        private List<Order> SetUpOrdersDataGrid()
        {
            try
            {
                ListOrders = GetFilteredOrders();

                if (ListOrders == null)
                    return null;

                return ListOrders;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        private async void RefreshOrders()
        {
            // Spawn a loading window and change cursor to waiting cursor
            LoadingWindow loadingWindow = new LoadingWindow();
            loadingWindow.Show();
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                List<Order> orders = await Task.Run(() => SetUpOrdersDataGrid());

                WvaOrdersDataGrid.Items.Clear();

                foreach (Order order in ListOrders)
                    WvaOrdersDataGrid.Items.Add(order);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
            finally
            {
                // Close loading window and change cursor back to default arrow cursor
                loadingWindow.Close();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private List<Order> GetSelectedOrders()
        {
            // Get selected order
            List<Order> listOrders = new List<Order>();
            IList rows = WvaOrdersDataGrid.SelectedItems;


            // Leave if no items selected
            if (rows.Count == 0)
                return null;

            for (int i = 0; i < rows.Count; i++)
                listOrders.Add((Order)rows[i]);

            return listOrders;
        }

        // Navigate to Views/Orders/OrderCreationView
        private void OpenEditOrder()
        {
            List<Order> order = GetSelectedOrders();

            // Leave method if they don't select an order or if order has already been submitted
            if (order == null || order[0].Status.ToLower() == "submitted")
                return;

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new OrdersView(new List<Prescription>(), order[0].OrderName, "OrderCreation");
            }
        }

        private void OpenViewOrderDetails(Order selectedOrder)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(MainWindow))
                    (window as MainWindow).MainContentControl.DataContext = new OrdersView(selectedOrder);
            }
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
            toolTip.Content = "Refresh Content";
            toolTip.IsOpen = true;
            RefreshImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-available-updates-48.png", UriKind.Relative));
        }

        private void RefreshButton_MouseLeave(object sender, MouseEventArgs e)
        {
            toolTip.IsOpen = false;
            RefreshImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-available-updates-filled-48.png", UriKind.Relative));
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshOrders();
        }

        // Edit Button
        private void EditOrderButton_Click(object sender, RoutedEventArgs e)
        {
            OpenEditOrder();
        }

        // Delete Button
        private void DeleteOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected orders in the datagrid
            List<Order> listOrders = GetSelectedOrders();

            // Leave method if they don't select an order
            if (listOrders == null)
                return;

            foreach (var order in listOrders)
            {
                string location = GetType().FullName + nameof(DeleteOrderButton_Click);
                string actionMessage = $"<Delete_Order_Start>";
                ActionLogger.Log(location, actionMessage);

                OrderCreationViewModel.DeleteOrder(order.OrderName);

                actionMessage = $"<Delete_Order_Start> <Order.Name={order.OrderName}>";
                ActionLogger.Log(location, actionMessage);
            }


            RefreshOrders();
        }

        private void WvaOrdersDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Order selectedOrder = (Order)WvaOrdersDataGrid.SelectedItems[0];

                if (selectedOrder.Status == "open")
                    OpenEditOrder();

                if (selectedOrder.Status == "submitted")
                    OpenViewOrderDetails(selectedOrder);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + nameof(UserControl_Loaded);
            ActionLogger.Log(location);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + nameof(UserControl_Unloaded);
            ActionLogger.Log(location);
        }

    }
}
