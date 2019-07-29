using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Orders.Out;
using WVA_Connect_CDI.Models.OrderStatus.FromApi;
using WVA_Connect_CDI.Models.OrderStatus.ToApi;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.Utility.Files;
using WVA_Connect_CDI.ViewModels.Orders;
using WVA_Connect_CDI.WebTools;

namespace WVA_Connect_CDI.Views.Orders
{
    public partial class ViewOrderDetails : UserControl
    {
        public int SelectedRow { get; set; }

        public ViewOrderDetails()
        {
            InitializeComponent();
            SetUp();
        }

        private void SetUp()
        {
            try
            {
                var o = ViewOrderDetailsViewModel.SelectedOrder;

                // Update order status 
                o = UpdateOrderStatus(o);

                // Header
                OrderNameLabel.Content = o.OrderName;
                OrderedByLabel.Content = o.OrderedBy;
                AccountIDLabel.Content = o.CustomerID;
                OrderIDLabel.Content = $"WVA Order ID: {o.WvaStoreID}";

                // Sub-header (if value is not null or blank, add it to a stack panel column so the view scales smoothly)
                if (o.Name_1 != null && o.Name_1.Trim() != "")
                    StackPanelAddLeftChild($"Addressee: {o.Name_1}");

                if (o.StreetAddr_1 != null && o.StreetAddr_1.Trim() != "")
                    StackPanelAddLeftChild($"Address: {o.StreetAddr_1}");

                if (o.ShippingMethod != null && o.ShippingMethod.Trim() != "")
                    StackPanelAddLeftChild($"Ship Type: {o.ShippingMethod}");

                if (o.Phone != null && o.Phone.Trim() != "")
                    StackPanelAddLeftChild($"Phone: {o.Phone}");

                if (o.City != null && o.City.Trim() != "")
                    StackPanelAddRightChild($"City: {o.City}");

                if (o.State != null && o.State.Trim() != "")
                    StackPanelAddRightChild($"State: {o.State}");

                if (o.Zip != null && o.Zip.Trim() != "")
                    StackPanelAddRightChild($"Zip: {o.Zip}");

                if (o.StreetAddr_2 != null && o.StreetAddr_2.Trim() != "")
                    StackPanelAddRightChild($"Suite/Apt: {o.StreetAddr_2}");

                // Grid items
                foreach (Item item in ViewOrderDetailsViewModel.SelectedOrder.Items)
                {
                    ReviewOrderDataGrid.Items.Add(new Prescription()
                    {
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Eye = item.Eye,
                        Product = item.ProductDetail.Name,
                        Quantity = item.Quantity,
                        BaseCurve = item.ProductDetail.BaseCurve,
                        Diameter = item.ProductDetail.Diameter,
                        Sphere = item.ProductDetail.Sphere,
                        Cylinder = item.ProductDetail.Cylinder,
                        Axis = item.ProductDetail.Axis,
                        Add = item.ProductDetail.Add,
                        Color = item.ProductDetail.Color,
                        Multifocal = item.ProductDetail.Multifocal
                    });
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void StackPanelAddLeftChild(string content)
        {
            LeftInnerStackPanel.Children.Add(new Label()
            {
                Content = content,
                FontFamily = new FontFamily("Sitka Text"),
                FontSize = 16
            });
        }

        private void StackPanelAddRightChild(string content)
        {
            RightInnerStackPanel.Children.Add(new Label()
            {
                Content = content,
                FontFamily = new FontFamily("Sitka Text"),
                FontSize = 16
            });
        }

        private void SetUpGridContextMenuItems()
        {
            try
            {
                WVA_OrdersContextMenu.Items.Clear();

                if (SelectedRow < 0)
                    return;

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].DeletedFlag != null && ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].DeletedFlag.ToLower() == "y")
                    AddItemToGridContextMenu("-- DELETED ORDER! --");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].QuantityBackordered > 0)
                    AddItemToGridContextMenu($"Quantity backordered: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].QuantityBackordered}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].QuantityCancelled > 0)
                    AddItemToGridContextMenu($"Quantity cancelled: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].QuantityCancelled}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].QuantityShipped > 0)
                    AddItemToGridContextMenu($"Quantity shipped: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].QuantityShipped}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].Status != null && ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].Status.Trim() != "")
                    AddItemToGridContextMenu($"Status: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].Status}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ItemStatus != null && ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ItemStatus.Trim() != "")
                    AddItemToGridContextMenu($"Item Status: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ItemStatus}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ShippingDate != null && ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ShippingDate.Trim() != "")
                    AddItemToGridContextMenu($"Shipping Date: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ShippingDate}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ShippingCarrier != null && ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ShippingCarrier.Trim() != "")
                    AddItemToGridContextMenu($"Shipping Carrier: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].ShippingCarrier}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].TrackingUrl != null && ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].TrackingUrl.Trim() != "")
                    AddItemToGridContextMenu($"{ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].TrackingUrl}");

                if (ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].TrackingNumber != null && ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].TrackingNumber.Trim() != "")
                    AddItemToGridContextMenu($"Tracking Number: {ViewOrderDetailsViewModel.SelectedOrder.Items[SelectedRow].TrackingNumber}");
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void AddItemToGridContextMenu(string headerContent)
        {
            try
            {
                MenuItem item = new MenuItem() { Header = headerContent };
                item.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                WVA_OrdersContextMenu.Items.Add(item);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        // Update order status for each order in list
        private Order UpdateOrderStatus(Order order)
        {
            if (order.WvaStoreID == null || order.WvaStoreID.Trim() == "" || order.Status == "open")
                return order;

            RequestWrapper request = new RequestWrapper()
            {
                Request = new StatusRequest()
                {
                    ApiKey = UserData.Data.ApiKey,
                    CustomerId = UserData.Data.Account,
                    WvaStoreNumber = order.WvaStoreID
                }
            };

            string statusEndpoint = Paths.WisVisOrderStatus;
            string strStatusResponse = API.Post(statusEndpoint, request);

            if (strStatusResponse == null || strStatusResponse.Trim() == "")
                return order;

            var statusResponse = JsonConvert.DeserializeObject<StatusResponse>(strStatusResponse);

            order.Message = statusResponse.Message;
            order.ProcessedFlag = statusResponse.ProcessedFlag;

            if (statusResponse.Items == null || statusResponse.Items?.Count < 1)
                return order;

            for (int i = 0; i < statusResponse.Items.Count; i++)
            {
                order.Items[i].DeletedFlag = statusResponse.DeletedFlag;
                order.Items[i].QuantityBackordered = statusResponse.Items[i].QuantityBackordered;
                order.Items[i].QuantityCancelled = statusResponse.Items[i].QuantityCancelled;
                order.Items[i].QuantityShipped = statusResponse.Items[i].QuantityShipped;
                order.Items[i].Status = statusResponse.Items[i].Status;
                order.Items[i].ItemStatus = statusResponse.Items[i].ItemStatus;
                order.Items[i].ShippingDate = statusResponse.Items[i].ShippingDate;
                order.Items[i].ShippingCarrier = statusResponse.Items[i].ShippingCarrier;
                order.Items[i].TrackingUrl = statusResponse.Items[i].TrackingUrl;
                order.Items[i].TrackingNumber = statusResponse.Items[i].TrackingNumber;
            }

            return order;
        }

        private void WVA_OrdersContextMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem menuItem = sender as MenuItem;
                string selectedItem = menuItem.Header.ToString();

                if (selectedItem.Contains("https"))
                {
                    Process.Start(selectedItem);
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void ReviewOrderDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            SelectedRow = ReviewOrderDataGrid.Items.IndexOf(ReviewOrderDataGrid.CurrentItem);
            SetUpGridContextMenuItems();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + nameof(UserControl_Loaded);
            string actionMessage = $"<Order.ID={ViewOrderDetailsViewModel.SelectedOrder.ID}> <Order.Name={ViewOrderDetailsViewModel.SelectedOrder.OrderName}> <Order.WvaOrderID={ViewOrderDetailsViewModel.SelectedOrder.WvaStoreID}>";
            ActionLogger.Log(location, actionMessage);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + nameof(UserControl_Unloaded);
            string actionMessage = $"<Order.ID={ViewOrderDetailsViewModel.SelectedOrder.ID}> <Order.Name={ViewOrderDetailsViewModel.SelectedOrder.OrderName}> <Order.WvaOrderID={ViewOrderDetailsViewModel.SelectedOrder.WvaStoreID}>";
            ActionLogger.Log(location, actionMessage);
        }

    }
}
