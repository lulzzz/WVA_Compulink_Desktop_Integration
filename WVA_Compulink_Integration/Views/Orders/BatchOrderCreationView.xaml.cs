using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Orders.Out;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Models.Responses;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.ViewModels.Orders;

namespace WVA_Connect_CDI.Views.Orders
{
    public partial class BatchOrderCreationView : UserControl
    {
        OrderCreationViewModel orderCreationViewModel = new OrderCreationViewModel();

        public BatchOrderCreationView()
        {
            InitializeComponent();
            SetUpView();
            SetMatchSliderValue();
            AddPrescriptionsToMenu();
            VerifyAllChildrenTables();
        }

        private void SetUpView()
        {
            OrderedByTextBox.Text = UserData.Data.UserName;
        }

        private void AddPrescriptionsToMenu()
        {
            for (int i = 0; i < BatchOrderCreationViewModel.BatchPrescriptions.Count; i++)
                BatchScrollViewerStackPanel.Children.Add(new BatchOrderLayoutUserControl(BatchOrderCreationViewModel.OrderNames[i], BatchOrderCreationViewModel.BatchPrescriptions[i]));
        }

        private void VerifyAllChildrenTables()
        {
            foreach (BatchOrderLayoutUserControl c in BatchScrollViewerStackPanel.Children)
            {
                c.FindProductMatches();
                c.SetMenuItems();
                c.Verify();
            }
        }

        private List<OutOrderWrapper> GetAllOrders(bool selectOnlyChecked = false)
        {
            var listOrders = new List<OutOrderWrapper>();

            // Iterate over all child datagrids
            foreach (BatchOrderLayoutUserControl childGrid in BatchScrollViewerStackPanel.Children)
            {
                // Skip over non-checked items if selectOnlyChecked is true
                if (selectOnlyChecked)
                    if (childGrid.OrderSelectCheckBox.IsChecked == false)
                        continue;

                // Build the order object 
                var order = new Order()
                {
                    OrderName = childGrid.OrderNameLabel.Content.ToString(),
                    OrderedBy = OrderedByTextBox.Text,
                    ShippingMethod = orderCreationViewModel.GetShippingTypeID(ShippingTypeComboBox.Text),
                    ShipToPatient = childGrid.OrderTypeLabel.Content.ToString().ToLower().Contains("patient") ? "Y" : "N",
                };

                for (int e = 0; e < childGrid.OrdersDataGrid.Items.Count; e++)
                {
                    IList rows = childGrid.OrdersDataGrid.Items;
                    var prescription = (Prescription)rows[e];

                    order.CustomerID = childGrid.AcctNumLabel.Content.ToString();

                    if (order.ShipToPatient == "Y")
                    {
                        var patient = CompulinkOrdersViewModel.GetPatientInfo(prescription._CustomerID.Value);
                        order.Name1 = patient.FullName ?? "";
                        order.StreetAddr1 = patient.Street ?? "";
                        order.State = patient.State ?? "";
                        order.City = patient.City ?? "";
                        order.Zip = patient.Zip ?? "";
                        order.Phone = patient.Phone ?? "";
                        order.DoB = patient.DoB ?? "";
                    }

                    order.Items = new List<Item>();
                    order.Items.Add(new Item()
                    {
                        ID = Guid.NewGuid().ToString().Replace("-", ""),
                        FirstName = prescription.FirstName,
                        LastName = prescription.LastName,
                        PatientID = prescription._CustomerID?.Value,
                        Eye = prescription.Eye,
                        Quantity = prescription.Quantity == null || prescription.Quantity.Trim() == "" || prescription.Quantity == "0" ? "1" : prescription.Quantity,
                        ItemRetailPrice = prescription.Price,
                        ProductDetail = new OrderDetail()
                        {
                            Name = prescription.Product,
                            ProductReviewed = prescription.ProductImagePath == null || prescription.ProductImagePath == "" ? false : true,
                            SKU = prescription.SKU,
                            ProductKey = prescription.ProductCode,
                            UPC = prescription.UPC,
                            BaseCurve = prescription.BaseCurve,
                            Diameter = prescription.Diameter,
                            Sphere = prescription.Sphere,
                            Cylinder = prescription.Cylinder,
                            Axis = prescription.Axis,
                            Add = prescription.Add,
                            Color = prescription.Color,
                            Multifocal = prescription.Multifocal,
                            LensRx = prescription.LensRx
                        }
                    });
                }

                listOrders.Add(new OutOrderWrapper()
                {
                    OutOrder = new OutOrder()
                    {
                        ApiKey = UserData.Data.ApiKey,
                        PatientOrder = order
                    }
                });
            }
            return listOrders;
        }

        // ----------------------------------------------------------------------------------------
        // Product Match Finder Slider 
        // ----------------------------------------------------------------------------------------

        private void MinScoreAdjustSlider_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            SetMatchSliderValue();
        }

        private void SetMatchSliderValue()
        {
            BatchOrderCreationViewModel.MatchScoreTargetValue = (int)MinScoreAdjustSlider.Value;
            MatchPercentLabel.Content = $"Match Percent Value: {((int)MinScoreAdjustSlider.Value).ToString()}%";
        }

        private void SetMatchSliderValue(int value)
        {
            MinScoreAdjustSlider.Value = value;
            BatchOrderCreationViewModel.MatchScoreTargetValue = (int)MinScoreAdjustSlider.Value;
            MatchPercentLabel.Content = $"Match Percent Value: {((int)MinScoreAdjustSlider.Value).ToString()}%";
        }


        // ----------------------------------------------------------------------------------------
        // Single Order 
        // ----------------------------------------------------------------------------------------

        private void SubmitSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(SubmitSelectedButton_Click);
            string actionMessage = "<Submit_Selected_Order_Start> ";

            var responses = new List<OrderResponse>();
            var orders = GetAllOrders(selectOnlyChecked: true);

            foreach (OutOrderWrapper order in orders)
            {
                actionMessage += $"<Order.ID={order.OutOrder.PatientOrder.ID}> <Order.Name={order.OutOrder.PatientOrder.OrderName}>";
                var response = orderCreationViewModel.CreateOrder(order);
                responses.Add(response);
                actionMessage += response?.Data?.Wva_order_id != null ? $" <Response.OrderID={response.Data.Wva_order_id}>" : $" <Response.OrderID=null> <Response.Message={response.Message}>";
            }

            int successCt = 0;
            foreach (OrderResponse response in responses)
                if (response.Status == "SUCCESS")
                    successCt++;

            if (successCt == orders.Count)
                new MessageWindow($"\t\t {orders.Count} Orders Submitted!").Show();
            else
                MessageBox.Show("One or more orders encountered a problem and were not created. Please contact WVA Technical support if the problem persists.", "", MessageBoxButton.OK);

            actionMessage += "<Submit_Selected_Order_End> ";
            ActionLogger.Log(location, actionMessage);
        }

        private void DeleteSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(DeleteSelectedButton_Click);
            string actionMessage = "<Delete_Selected_Order_Start> ";

            // Safely remove a control from the parent tree
            var controls = new List<BatchOrderLayoutUserControl>();

            foreach (BatchOrderLayoutUserControl c in BatchScrollViewerStackPanel.Children)
                if (c.OrderSelectCheckBox.IsChecked == true)
                    controls.Add(c);

            for (int i = 0; i < controls.Count; i++)
            {
                BatchScrollViewerStackPanel.Children.Remove(controls[i]);
                actionMessage += $"<Order.Name={controls[i].OrderNameLabel}> ";
            }

            actionMessage += "<Delete_Selected_Order_End> ";
            ActionLogger.Log(location, actionMessage);
        }

        // ----------------------------------------------------------------------------------------
        // All Orders 
        // ----------------------------------------------------------------------------------------

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(SelectAllButton_Click);
            string actionMessage = "<User Checked All Orders> ";

            foreach (BatchOrderLayoutUserControl c in BatchScrollViewerStackPanel.Children)
                if (c.OrderSelectCheckBox.IsChecked == false)
                    c.OrderSelectCheckBox.IsChecked = true;

            ActionLogger.Log(location, actionMessage);
        }

        private void DeselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(DeselectAllButton_Click);
            string actionMessage = "<User Unchecked All Orders> ";

            foreach (BatchOrderLayoutUserControl c in BatchScrollViewerStackPanel.Children)
                if (c.OrderSelectCheckBox.IsChecked == true)
                    c.OrderSelectCheckBox.IsChecked = false;

            ActionLogger.Log(location, actionMessage);
        }

        private void SubmitAllOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string location = GetType().FullName + "." + nameof(SubmitAllOrderButton_Click);
                string actionMessage = $"<Submit_All_Orders_Start> ";

                var orders = GetAllOrders();

                var orderResponses = new List<OrderResponse>();

                foreach (OutOrderWrapper order in orders)
                {
                    actionMessage += $"<Order.ID={order.OutOrder.PatientOrder.ID}> <Order.Name={order.OutOrder.PatientOrder.OrderName}> ";
                    var response = orderCreationViewModel.CreateOrder(order);
                    orderResponses.Add(response);
                    actionMessage += response?.Data?.Wva_order_id != null ? $" <Response.OrderID={response.Data.Wva_order_id}>" : $" <Response.OrderID=null> <Response.Message={response.Message}>";
                }

                // Make sure all responses are good
                int successCt = 0;
                foreach (OrderResponse response in orderResponses)
                    if (response.Status == "SUCCESS")
                        successCt++;

                if (successCt == orders.Count)
                {
                    // Change view to WVA Orders view
                    foreach (Window window in Application.Current.Windows)
                        if (window.GetType() == typeof(MainWindow))
                            (window as MainWindow).MainContentControl.DataContext = new OrdersView(null, null, "WVAOrders");
                }
                else
                {
                    MessageBox.Show("One or more orders encountered a problem and were not created. Please contact WVA Technical support if the problem persists.", "", MessageBoxButton.OK);
                }

                actionMessage += "<Submit_All_Orders_End>";
                ActionLogger.Log(location, actionMessage);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void DeleteAllOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Delete this entire WVA batch order? (You can still add these compulink prescriptions to a WVA order later)", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    // Track delete order actions
                    string location = GetType().FullName + "." + nameof(DeleteAllOrderButton_Click);
                    string actionMessage = "<Delete_Order_Start> ";

                    foreach (string orderName in BatchOrderCreationViewModel.OrderNames)
                    {
                        actionMessage += $"<Deleting_Order_Where_Name={orderName ?? "null"}>";
                    }

                    BatchOrderCreationViewModel.OrderNames.Clear();
                    BatchOrderCreationViewModel.BatchPrescriptions.Clear();
                    BatchOrderCreationViewModel.OrderNames.Clear();

                    // Change view to Compulink Orders view
                    foreach (Window window in Application.Current.Windows)
                        if (window.GetType() == typeof(MainWindow))
                            (window as MainWindow).MainContentControl.DataContext = new OrdersView(null, null, "CompulinkOrders");

                    actionMessage += "<Delete_Order_End>";
                    ActionLogger.Log(location, actionMessage);
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void SaveOrderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Track save order actions
                string location = GetType().FullName + "." + nameof(SaveOrderButton_Click);
                string actionMessage = "<Save_Order_Start> ";

                // Get orders 
                var orders = GetAllOrders();
                actionMessage += $"<Orders.Count={orders.Count}> ";

                var orderResponses = new List<OrderResponse>();

                foreach (OutOrderWrapper order in orders)
                {
                    actionMessage += $"<Order.ID={order.OutOrder.PatientOrder.ID}> <Order.Name={order.OutOrder.PatientOrder.OrderName}>";
                    var response = orderCreationViewModel.SaveOrder(order);
                    orderResponses.Add(response);
                    actionMessage += response?.Data?.Wva_order_id != null ? $" <Response.OrderID={response.Data.Wva_order_id}>" : $" <Response.OrderID=null> <Response.Message={response.Message}>";
                }

                bool showSaveError = false;
                foreach (OrderResponse response in orderResponses)
                {
                    if (response.Status != "SUCCESS")
                        showSaveError = true;
                }

                if (showSaveError)
                    throw new Exception("Bad response detected while saving order.");
                else
                    new MessageWindow("\t\tOrders Saved!").Show();

                actionMessage += "<Save_Order_End>";
                ActionLogger.Log(location, actionMessage);
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                MessageBox.Show("One or more orders were not saved. Please contact IT if the problem persists.", "Warning", MessageBoxButton.OK);
            }
        }

        private void VerifyOrderButton_Click(object sender, RoutedEventArgs e)
        {
            SetMatchSliderValue(60);
            VerifyAllChildrenTables();
        }

        // ----------------------------------------------------------------------------------------
        // Field Events 
        // ----------------------------------------------------------------------------------------

        private void OrderedByTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(UserControl_Unloaded);
            string actionMessage = "<Leaving_Batch_Order_Creation>";

            ActionLogger.Log(location, actionMessage);

            BatchOrderCreationViewModel.OrderNames?.Clear();
            BatchOrderCreationViewModel.BatchPrescriptions?.Clear();
            BatchOrderCreationViewModel.BatchPrescriptionMatches?.Clear();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + nameof(UserControl_Loaded);
            string actionMessage = "<Entering_Batch_Order_Creation>";

            foreach (string orderName in BatchOrderCreationViewModel.OrderNames)
                actionMessage += $"<Order.Name={orderName}>";

            ActionLogger.Log(location, actionMessage);
        }

        private void MinScoreAdjustSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MatchPercentLabel.Content = $"Match Percent: {Convert.ToInt16(MinScoreAdjustSlider.Value)}%";
        }
    }
}
