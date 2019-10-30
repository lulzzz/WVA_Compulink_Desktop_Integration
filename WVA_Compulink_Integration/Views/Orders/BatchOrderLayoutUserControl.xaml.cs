using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.MatchFinder;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Orders;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Models.ProductParameters.Derived;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.Models.Validations;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions.Models;
using WVA_Connect_CDI.Utility.UI_Tools;
using WVA_Connect_CDI.ViewModels.Orders;

namespace WVA_Connect_CDI.Views.Orders
{
    public partial class BatchOrderLayoutUserControl : UserControl
    {
        private int SelectedRow { get; set; }
        private int SelectedColumn { get; set; }
        public BatchOrderCreationUserControlViewModel viewModel = new BatchOrderCreationUserControlViewModel();
        OrderCreationViewModel orderCreationViewModel = new OrderCreationViewModel();

        public BatchOrderLayoutUserControl(string orderName, List<Prescription> prescriptions)
        {
            InitializeComponent();
            SetTitle(orderName, prescriptions);
            SetUpDatagrid(prescriptions);
        }

        private void SetTitle(string orderName, List<Prescription> prescriptions)
        {
            OrderNameLabel.Content = orderName;
            AcctNumLabel.Content = prescriptions[0].AccountNumber;
            OrderTypeLabel.Content = prescriptions[0].IsShipToPat ? "(Ship to Patient)" : "(Ship to Office)";
        }

        private void SetUpDatagrid(List<Prescription> prescriptions)
        {
            OrdersDataGrid.ItemsSource = prescriptions;
            viewModel.Prescriptions = prescriptions;
        }

        public void Verify()
        {
            try
            {
                var validationWrapper = GetValidationWrapper(OrdersDataGrid.Items);
                var validationResponse = orderCreationViewModel.ValidateOrder(validationWrapper);

                if (validationResponse.Status == "FAIL" && validationResponse.Message != "Invalid")
                    throw new Exception($"An error has occurred while validating products. ErrorMessage: {validationResponse.Message}");

                var validatedProducts = validationResponse.Data.Products;
                UpdateGridItems(validatedProducts);
            }
            catch (Exception x)
            {
                Error.Log(x.ToString());
            }
        }

        private void UpdateGridItems(List<ValidationDetail> detail)
        {
            for (int i = 0; i < detail.Count(); i++)
            {
                // if product is null or invalid, keep old value, else change it to the new value from prods
                var prescription = new Prescription()
                {
                    // These properties don't need to be validated
                    ProductImagePath = viewModel.Prescriptions[i].ProductImagePath,
                    IsChecked = viewModel.Prescriptions[i].IsChecked,
                    FirstName = viewModel.Prescriptions[i].FirstName,
                    LastName = viewModel.Prescriptions[i].LastName,
                    Patient = viewModel.Prescriptions[i].Patient,
                    Eye = viewModel.Prescriptions[i].Eye,
                    Quantity = viewModel.Prescriptions[i].Quantity,
                    Date = viewModel.Prescriptions[i].Date,
                    _CustomerID = viewModel.Prescriptions[i]._CustomerID,
                    IsShipToPat = viewModel.Prescriptions[i].IsShipToPat,
                    LensRx = viewModel.Prescriptions[i].LensRx,

                    // If prods[i].Property.Value == null change field to old value, else change to new value
                    CanBeValidated = detail[i].CanBeValidated,
                    Product = detail[i].Description ?? viewModel.Prescriptions[i].Product,
                    ProductCode = Validator.CheckIfValid(detail[i]._ProductKey) ? detail[i]._ProductKey?.Value : viewModel.Prescriptions[i].ProductCode,

                    // NOTE: To help explain ternary statements below for cell colors
                    // If property is null or is blank and errorMessage is null then cell color = White 
                    // If property isValid then cell color = Green
                    // If property not isValid then cell color = Red
                    BaseCurveValidItems = detail[i]._BaseCurve.ValidItems,
                    BaseCurve = Validator.CheckIfValid(detail[i]._BaseCurve) ? detail[i]._BaseCurve.Value : viewModel.Prescriptions[i].BaseCurve,
                    BaseCurveCellColor = AssignCellColor(prodValue: detail[i]._BaseCurve?.Value?.Trim(), isValid: detail[i]._BaseCurve.IsValid, errorMessage: detail[i]._BaseCurve.ErrorMessage, canBeValidated: detail[i].CanBeValidated),

                    DiameterValidItems = detail[i]._Diameter.ValidItems,
                    Diameter = Validator.CheckIfValid(detail[i]._Diameter) ? detail[i]._Diameter.Value : viewModel.Prescriptions[i].Diameter,
                    DiameterCellColor = AssignCellColor(prodValue: detail[i]._Diameter?.Value?.Trim(), isValid: detail[i]._Diameter.IsValid, errorMessage: detail[i]._Diameter.ErrorMessage, canBeValidated: detail[i].CanBeValidated),

                    SphereValidItems = detail[i]._Sphere.ValidItems,
                    Sphere = Validator.CheckIfValid(detail[i]._Sphere) ? detail[i]._Sphere.Value : viewModel.Prescriptions[i].Sphere,
                    SphereCellColor = AssignCellColor(prodValue: detail[i]._Sphere?.Value?.Trim(), isValid: detail[i]._Sphere.IsValid, errorMessage: detail[i]._Sphere.ErrorMessage, canBeValidated: detail[i].CanBeValidated),

                    CylinderValidItems = detail[i]._Cylinder.ValidItems,
                    Cylinder = Validator.CheckIfValid(detail[i]._Cylinder) ? detail[i]._Cylinder.Value : viewModel.Prescriptions[i].Cylinder,
                    CylinderCellColor = AssignCellColor(prodValue: detail[i]._Cylinder?.Value?.Trim(), isValid: detail[i]._Cylinder.IsValid, errorMessage: detail[i]._Cylinder.ErrorMessage, canBeValidated: detail[i].CanBeValidated),

                    AxisValidItems = detail[i]._Axis.ValidItems,
                    Axis = Validator.CheckIfValid(detail[i]._Axis) ? detail[i]._Axis.Value : viewModel.Prescriptions[i].Axis,
                    AxisCellColor = AssignCellColor(prodValue: detail[i]._Axis?.Value?.Trim(), isValid: detail[i]._Axis.IsValid, errorMessage: detail[i]._Axis.ErrorMessage, canBeValidated: detail[i].CanBeValidated),

                    AddValidItems = detail[i]._Add.ValidItems,
                    Add = Validator.CheckIfValid(detail[i]._Add) ? detail[i]._Add.Value : viewModel.Prescriptions[i].Add,
                    AddCellColor = AssignCellColor(prodValue: detail[i]._Add?.Value?.Trim(), isValid: detail[i]._Add.IsValid, errorMessage: detail[i]._Add.ErrorMessage, canBeValidated: detail[i].CanBeValidated),

                    ColorValidItems = detail[i]._Color.ValidItems,
                    Color = Validator.CheckIfValid(detail[i]._Color) ? detail[i]._Color.Value : viewModel.Prescriptions[i].Color,
                    ColorCellColor = AssignCellColor(prodValue: detail[i]._Color?.Value?.Trim(), isValid: detail[i]._Color.IsValid, errorMessage: detail[i]._Color.ErrorMessage, canBeValidated: detail[i].CanBeValidated),

                    MultifocalValidItems = detail[i]._Multifocal.ValidItems,
                    Multifocal = Validator.CheckIfValid(detail[i]._Multifocal) ? detail[i]._Multifocal.Value : viewModel.Prescriptions[i].Multifocal,
                    MultifocalCellColor = AssignCellColor(prodValue: detail[i]._Multifocal?.Value?.Trim(), isValid: detail[i]._Multifocal.IsValid, errorMessage: detail[i]._Multifocal.ErrorMessage, canBeValidated: detail[i].CanBeValidated),
                };

                viewModel.Prescriptions[i] = prescription;
            }

            OrdersDataGrid.Items.Refresh();
        }

        private string AssignCellColor(string prodValue, bool isValid, string errorMessage, bool canBeValidated)
        {
            if (prodValue == null && errorMessage == null || prodValue == "" && errorMessage == null)
                return "White";
            else if (!canBeValidated)
                return "Yellow";
            else if (isValid)
                return "Green";
            else
                return "Red";
        }

        private List<Prescription> GetGridPrescriptions()
        {
            var prescriptions = new List<Prescription>();

            foreach (Prescription prescription in OrdersDataGrid.Items as IList)
                prescriptions.Add(prescription);

            return prescriptions;
        }

        private static ValidationWrapper GetValidationWrapper(ItemCollection itemCollection)
        {
            IList rows = itemCollection;
            var listValidations = new List<ValidationDetail>();

            for (int i = 0; i < itemCollection.Count; i++)
            {
                var prescription = (Prescription)rows[i];

                listValidations.Add(new ValidationDetail()
                {
                    _PatientName = prescription.Patient,
                    Eye = prescription.Eye,
                    Quantity = prescription.Quantity.ToString(),
                    Description = prescription.Product,
                    Vendor = "",
                    Price = "",
                    _ID = new ID() { Value = prescription.ID },
                    _CustomerID = new CustomerID() { Value = UserData.Data.Account },
                    _SKU = new SKU() { Value = "" },
                    _ProductKey = new ProductKey() { Value = prescription.ProductCode },
                    _UPC = new UPC() { Value = "" },
                    _BaseCurve = new BaseCurve() { Value = prescription.BaseCurve },
                    _Diameter = new Diameter() { Value = prescription.Diameter },
                    _Sphere = new Sphere() { Value = prescription.Sphere },
                    _Cylinder = new Cylinder() { Value = prescription.Cylinder },
                    _Axis = new Axis() { Value = prescription.Axis },
                    _Add = new Add() { Value = prescription.Add },
                    _Color = new Models.ProductParameters.Derived.Color() { Value = prescription.Color },
                    _Multifocal = new Multifocal() { Value = prescription.Multifocal },
                });
            }

            var validationWrapper = new ValidationWrapper()
            {
                Request = new ProductValidation()
                {
                    Key = UserData.Data.ApiKey,
                    ProductsToValidate = new List<ItemDetail>(),

                }
            };

            foreach (ValidationDetail validationDetail in listValidations)
            {
                validationWrapper.Request.ProductsToValidate.Add(new ValidationDetail(validationDetail));
            }

            return validationWrapper;
        }

        public void AutofillProductNames()
        {
            for (int i = 0; i < OrdersDataGrid.Items.Count; i++)
            {
                var o = (Prescription)OrdersDataGrid.Items[i];
                var prodName = o.Product.Trim();

                if (Database.GetNumPicks(prodName) >= 5)
                {
                    string wvaProduct = Database.ReturnWvaProductFor(prodName);

                    if (!string.IsNullOrEmpty(wvaProduct))
                    {
                        o.Product = wvaProduct;
                        o.ProductImagePath = @"/Resources/CheckMarkCircle.png";
                    }
                }
            }
        }

        public void AutofillParameters()
        {
            for (int i = 0; i < OrdersDataGrid.Items.Count; i++)
                AutoFillParameterCells(i);
        }

        // =======================================================================================================================
        // ================================== Match Algorithm ====================================================================
        // =======================================================================================================================

        public void FindProductMatches(double matchScore = 30, bool overrideNumPicks = false)
        {
            double MatchScore;
            if (overrideNumPicks)
                MatchScore = matchScore;
            else
                MatchScore = Convert.ToDouble(BatchOrderCreationViewModel.MatchScoreTargetValue);

            // Reset list of matched products 
            viewModel.Matches.Clear();

            // Get product names in DataGrid
            List<Prescription> compulinkProducts = new List<Prescription>();
            for (int i = 0; i < OrdersDataGrid.Items.Count; i++)
            {
                Prescription prescription = (Prescription)OrdersDataGrid.Items[i];
                compulinkProducts.Add(prescription);
            }

            // Loop through product names list and pass each one into matcher algorithm 
            int index = 0;
            foreach (Prescription prescription in compulinkProducts)
            {
                try
                {
                    List<Product> wvaProducts = WvaProducts.ListProducts;

                    if (wvaProducts == null || wvaProducts.Count == 0)
                        throw new Exception("List<WVA_Products> is null or empty!");

                    // Run match finder for product and return results based on numPicks (number of times same product has been chosen)
                    List<MatchedProduct> matchProducts = ProductPrediction.GetPredictionMatches(prescription, MatchScore, overrideNumPicks);

                    if (matchProducts?.Count > 0)
                    {
                        viewModel.Matches.Add(matchProducts);
                        viewModel.Prescriptions[index].ProductCode = matchProducts[0].ProductCode;
                    }
                    else
                        viewModel.Matches.Add(new List<MatchedProduct> { new MatchedProduct("No Matches Found", 0) });
                }
                catch { }
                finally
                {
                    index++;
                }
            }
        }

        // Purpose of this overload is so product can be changed before it's commited from cell edit event
        public void FindProductMatches(string product, int rowToUpdate, double matchScore = 30, bool overrideNumPicks = false)
        {
            double MatchScore;
            if (overrideNumPicks)
                MatchScore = matchScore;
            else
                MatchScore = Convert.ToDouble(BatchOrderCreationViewModel.MatchScoreTargetValue);

            // Reset list of matched products 
            viewModel.Matches.Clear();

            // Get product names in DataGrid
            List<Prescription> compulinkProducts = new List<Prescription>();
            for (int i = 0; i < OrdersDataGrid.Items.Count; i++)
            {
                Prescription prescription = (Prescription)OrdersDataGrid.Items[i];
                compulinkProducts.Add(prescription);
            }

            // Update prescription object with new product
            compulinkProducts[rowToUpdate].Product = product;

            // Loop through product names list and pass each one into matcher algorithm 
            int index = 0;
            foreach (Prescription prescription in compulinkProducts)
            {
                try
                {
                    List<Product> wvaProducts = WvaProducts.ListProducts;

                    if (wvaProducts == null || wvaProducts.Count == 0)
                        throw new Exception("List<WVA_Products> is null or empty!");

                    // Run match finder for product and return results based on numPicks (number of times same product has been chosen)
                    List<MatchedProduct> matchProducts = ProductPrediction.GetPredictionMatches(prescription, MatchScore, overrideNumPicks);

                    if (matchProducts?.Count > 0)
                    {
                        viewModel.Matches.Add(matchProducts);
                        viewModel.Prescriptions[index].ProductCode = matchProducts[0].ProductCode;
                    }
                    else
                        viewModel.Matches.Add(new List<MatchedProduct> { new MatchedProduct("No Matches Found", 0) });
                }
                catch { }
                finally
                {
                    index++;
                }
            }
        }

        public void SetMenuItems()
        {
            try
            {
                // If products list isn't loaded or an error occurs in DescriptionMatcher and 
                // ListMatchedProducts' can't be loaded then leave so no errors will occur
                if (viewModel.Matches == null || viewModel.Matches.Count == 0)
                    return;

                // Reset the products ContextMenu
                WVA_OrdersContextMenu.Items.Clear();

                // Sets 'ClickedIndex' to the index of selected cell
                //int index = OrdersDataGrid.SelectedIndex;
                int index = OrdersDataGrid.Items.IndexOf(OrdersDataGrid.CurrentItem);
                if (index > -1)
                {
                    //SelectedRow = OrdersDataGrid.SelectedIndex;
                    SelectedRow = OrdersDataGrid.Items.IndexOf(OrdersDataGrid.CurrentItem);
                    SelectedColumn = OrdersDataGrid.CurrentColumn.DisplayIndex;
                }
                else
                    return;

                int ValidItemsCount;

                try
                {
                    // Load the context menu with relevant items at a specific column
                    // 0-2 are skipped because they are not editable parameters
                    switch (SelectedColumn)
                    {
                        case 3: // If column is a 'Product'
                            foreach (MatchedProduct match in viewModel.Matches[SelectedRow])
                            {
                                MenuItem menuItem = new MenuItem() { Header = match.ProductName };
                                menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                WVA_OrdersContextMenu.Items.Add(menuItem);
                            }

                            // Add 'More Matches' item to the end of the list 
                            MenuItem moreMatchesMenuItem = new MenuItem() { Header = "-- More Matches --" };
                            moreMatchesMenuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                            WVA_OrdersContextMenu.Items.Add(moreMatchesMenuItem);
                            break;
                        case 5: // If column is a 'BaseCurve'
                            if (viewModel.Prescriptions[SelectedRow].BaseCurveValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].BaseCurveValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].BaseCurveValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                        case 6: // If column is a 'Diameter'
                            if (viewModel.Prescriptions[SelectedRow].DiameterValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].DiameterValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].DiameterValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                        case 7: // If column is a 'Sphere'
                            if (viewModel.Prescriptions[SelectedRow].SphereValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].SphereValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].SphereValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                        case 8: // If column is a 'Cylinder'
                            if (viewModel.Prescriptions[SelectedRow].CylinderValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].CylinderValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].CylinderValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                        case 9: // If column is a 'Axis'
                            if (viewModel.Prescriptions[SelectedRow].AxisValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].AxisValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].AxisValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                        case 10: // If column is a 'Add'
                            if (viewModel.Prescriptions[SelectedRow].AddValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].AddValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].AddValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                        case 11: // If column is a 'Color'
                            if (viewModel.Prescriptions[SelectedRow].ColorValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].ColorValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].ColorValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                        case 12: // If column is a 'Multifocal'
                            if (viewModel.Prescriptions[SelectedRow].MultifocalValidItems != null)
                                ValidItemsCount = viewModel.Prescriptions[SelectedRow].MultifocalValidItems.Count;
                            else
                            {
                                SetNotAvailableMenuItem();
                                return;
                            }

                            if (ValidItemsCount > 0)
                            {
                                for (int i = 0; i < ValidItemsCount; i++)
                                {
                                    MenuItem menuItem = new MenuItem() { Header = viewModel.Prescriptions[SelectedRow].MultifocalValidItems[i] };
                                    menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
                                    WVA_OrdersContextMenu.Items.Add(menuItem);
                                }
                            }
                            else
                                throw new Exception("No Valid Items");
                            break;
                    }
                }
                catch
                {
                    SetNotAvailableMenuItem();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void AutoFillParameterCells(int row)
        {
            if (viewModel.Prescriptions.Count > 0)
            {
                var prescriptions = GetGridPrescriptions();
                FindProductMatches();
                SetMenuItems();
                Verify();

                if (viewModel.Prescriptions[row]?.BaseCurveValidItems?.Count == 1 && (viewModel.Prescriptions[row].BaseCurve == null || viewModel.Prescriptions[row].BaseCurve.Trim() == ""))
                {
                    OrdersDataGrid.GetCell(row, 5).Content = viewModel.Prescriptions[row]?.BaseCurveValidItems[0];
                    viewModel.Prescriptions[row].BaseCurve = viewModel.Prescriptions[row]?.BaseCurveValidItems[0];
                }

                if (viewModel.Prescriptions[row]?.DiameterValidItems?.Count == 1)
                {
                    OrdersDataGrid.GetCell(row, 6).Content = viewModel.Prescriptions[row].DiameterValidItems[0];
                    viewModel.Prescriptions[row].Diameter = viewModel.Prescriptions[row].DiameterValidItems[0];
                }

                if (viewModel.Prescriptions[row]?.SphereValidItems?.Count == 1)
                {
                    OrdersDataGrid.GetCell(row, 7).Content = viewModel.Prescriptions[row].SphereValidItems[0];
                    viewModel.Prescriptions[row].Sphere = viewModel.Prescriptions[row].SphereValidItems[0];
                }

                if (viewModel.Prescriptions[row]?.CylinderValidItems?.Count == 1)
                {
                    OrdersDataGrid.GetCell(row, 8).Content = viewModel.Prescriptions[row].CylinderValidItems[0];
                    viewModel.Prescriptions[row].Cylinder = viewModel.Prescriptions[row].CylinderValidItems[0];
                }

                if (viewModel.Prescriptions[row]?.AxisValidItems?.Count == 1)
                {
                    OrdersDataGrid.GetCell(row, 9).Content = viewModel.Prescriptions[row].AxisValidItems[0];
                    viewModel.Prescriptions[row].Axis = viewModel.Prescriptions[row].AxisValidItems[0];
                }

                if (viewModel.Prescriptions[row]?.AddValidItems?.Count == 1)
                {
                    OrdersDataGrid.GetCell(row, 10).Content = viewModel.Prescriptions[row].AddValidItems[0];
                    viewModel.Prescriptions[row].Add = viewModel.Prescriptions[row].AddValidItems[0];
                }

                if (viewModel.Prescriptions[row]?.ColorValidItems?.Count == 1)
                {
                    OrdersDataGrid.GetCell(row, 11).Content = viewModel.Prescriptions[row].ColorValidItems[0];
                    viewModel.Prescriptions[row].Color = viewModel.Prescriptions[row].ColorValidItems[0];
                }

                if (viewModel.Prescriptions[row]?.MultifocalValidItems?.Count == 1)
                {
                    OrdersDataGrid.GetCell(row, 12).Content = viewModel.Prescriptions[row].MultifocalValidItems[0];
                    viewModel.Prescriptions[row].Multifocal = viewModel.Prescriptions[row].MultifocalValidItems[0];
                }
            }
        }

        private void SetNotAvailableMenuItem()
        {
            MenuItem menuItem = new MenuItem() { Header = "Not Available" };
            menuItem.Click += new RoutedEventHandler(WVA_OrdersContextMenu_Click);
            WVA_OrdersContextMenu.Items.Add(menuItem);
        }

        private void WVA_OrdersContextMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var menuItem = sender as MenuItem;
                string selectedItem = menuItem.Header.ToString();

                // Drop out of function and populate context menu with a lower match tolerance
                if (selectedItem.Contains("More Matches"))
                {
                    // Run match FindMatch again slghtly lower tollerances
                    BatchOrderCreationViewModel.MatchScoreTargetValue -= 10;

                    FindProductMatches(Convert.ToInt16(BatchOrderCreationViewModel.MatchScoreTargetValue), true);
                    SetMenuItems();
                    WVA_OrdersContextMenu.IsOpen = true;
                    return;
                }

                // Get selected row and column
                int row = OrdersDataGrid.Items.IndexOf(OrdersDataGrid.CurrentItem);
                int column = OrdersDataGrid.CurrentColumn.DisplayIndex;

                // Only want to change 'Products' column 
                if (selectedItem != "No Matches Found" && selectedItem != "Not Available")
                    AutoFillParameterCells(row);

                OrdersDataGrid.GetCell(row, column).Content = selectedItem;

                if (column <= 4)
                {
                    string compulinkProduct = (OrdersDataGrid.CurrentItem as Prescription).Product;
                    viewModel.Prescriptions[row].Product = selectedItem;
                    viewModel.Prescriptions[row].ProductImagePath = @"/Resources/CheckMarkCircle.png";

                    // Only learn product if product change is enabled for this compulink product 
                    if (ProductPrediction.ProductChangeEnabled(compulinkProduct)) ProductPrediction.LearnProduct(compulinkProduct, selectedItem);
                }
                if (column == 5)
                    viewModel.Prescriptions[row].BaseCurve = selectedItem;
                if (column == 6)
                    viewModel.Prescriptions[row].Diameter = selectedItem;
                if (column == 7)
                    viewModel.Prescriptions[row].Sphere = selectedItem;
                if (column == 8)
                    viewModel.Prescriptions[row].Cylinder = selectedItem;
                if (column == 9)
                    viewModel.Prescriptions[row].Axis = selectedItem;
                if (column == 10)
                    viewModel.Prescriptions[row].Add = selectedItem;
                if (column == 11)
                    viewModel.Prescriptions[row].Color = selectedItem;
                if (column == 12)
                    viewModel.Prescriptions[row].Multifocal = selectedItem;

                AutoFillParameterCells(row);
                Verify();

            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        private void OrdersDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                SetMenuItems();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void OrdersDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            try
            {
                if (e.EditAction == DataGridEditAction.Commit)
                {
                    // Only change text in product name cells
                    if (SelectedColumn == 3)
                    {
                        FindProductMatches((e.EditingElement as TextBox).Text.ToString(), e.Row.GetIndex());
                        SetMenuItems();
                    }
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

    }
}
