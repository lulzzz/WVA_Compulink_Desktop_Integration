﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Models.Manage;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.ProductMatcher.Data;
using WVA_Connect_CDI.ProductMatcher.Models;
using WVA_Connect_CDI.ProductPredictions;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.ViewModels.Manage;

namespace WVA_Connect_CDI.Views.Manage
{
    public partial class ManageView : UserControl
    {
        ToolTip toolTip = new ToolTip();
        ManageViewModel manageViewModel;

        private bool SortCompulinkProductsAscending = true;
        private bool SortWvaProductsAscending = true;

        public ManageView()
        {
            manageViewModel = new ManageViewModel();
            
            InitializeComponent();
            RefreshGrid();
            SetUpContextMenu();
        }

        //
        // UI Support Methods
        //

        private bool BringResultsWindowToViewIfOpen()
        {
            bool resultsWindowOpen = false;

            try
            {
                foreach (Window window in Application.Current.Windows)
                {
                    // Bring Import Results Window to the front 
                    if (window.Name == "ImportCompulinkProductsResultsWindow")
                    {
                        window.Topmost = true;
                        window.Topmost = false;
                        window.Focus();
                        resultsWindowOpen = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }

            return resultsWindowOpen;
        }

        private void OpenImportResultsWindow(List<MatchedProductResult> listMatchProductResults)
        {
            // Create new instance of ImporResultsWindow with match results for the compulink product in the selected file
            new ImportResultsWindow(listMatchProductResults).Show();
        }

        private List<MatchedProductResult> GetMatchedProductResults(List<string> compulinkProducts, List<List<MatchedProduct>> listMatchedProducts)
        {
            var listMatchProductResults = new List<MatchedProductResult>();

            try
            {
                int count = 0;
                foreach (List<MatchedProduct> listMatches in listMatchedProducts)
                {
                    var suggestedMatches = new List<string>();

                    foreach (MatchedProduct match in listMatches)
                        suggestedMatches.Add(match.ProductName);

                    listMatchProductResults.Add(new MatchedProductResult()
                    {
                        CompulinkProduct = compulinkProducts[count],
                        WvaProduct = listMatches[0].ProductName,
                        MatchScore = listMatches[0].MatchScore,
                        RowColor = "Green",
                        MatchedProducts = listMatches,
                        SuggestedWvaProducts = suggestedMatches
                    });

                    count++;
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }

            return listMatchProductResults;
        }

        private void SetUpContextMenu()
        {
            MenuItem menuItem = new MenuItem() { Header = "Delete" };
            menuItem.Click += new RoutedEventHandler(WvaProductsContextMenu_Click);
            WvaProductsContextMenu.Items.Add(menuItem);
        }

        private LearnedProduct GetCreatedLearnedProduct()
        {
            return new LearnedProduct()
            {
                CompulinkProduct = CompulinkProductTextBox.Text,
                WvaProduct = WvaProductComboBox.Text,
                ChangeEnabled = (bool)IsEditableCheckBox.IsChecked,
                NumPicks = 10
            };
        }

        private async void RefreshGrid()
        {
            LearnedProductsDataGrid.ItemsSource = manageViewModel.GetLearnedProducts();
            LearnedProductsDataGrid.Items.Refresh();
        }

        private void ResetCreateLearnedProductElements()
        {
            CompulinkProductTextBox.Text = "";
            WvaProductComboBox.Text = "";
            IsEditableCheckBox.IsChecked = true;
        }

        private async Task UpdateChangeEnabledAsync(bool changeEnabled)
        {
            foreach (LearnedProduct product in LearnedProductsDataGrid.Items.Cast<LearnedProduct>().ToList())
                Database.UpdateChangeEnabled(product.CompulinkProduct, changeEnabled);
        }

        private void SortByCompulinkProduct()
        {
            var column = LearnedProductsDataGrid.Columns[0];
            LearnedProductsDataGrid.Items.SortDescriptions.Clear();

            // Toggle between sort ascending and descending 
            if (SortCompulinkProductsAscending)
            {
                LearnedProductsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                foreach (var col in LearnedProductsDataGrid.Columns)
                    col.SortDirection = null;

                column.SortDirection = ListSortDirection.Ascending;

                SortCompulinkProductsAscending = false;
            }
            else
            {
                LearnedProductsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Descending));

                foreach (var col in LearnedProductsDataGrid.Columns)
                    col.SortDirection = null;

                column.SortDirection = ListSortDirection.Descending;

                SortCompulinkProductsAscending = true;
            }

            LearnedProductsDataGrid.Items.Refresh();
        }

        private void SortByWvaProduct()
        {
            var column = LearnedProductsDataGrid.Columns[1];
            LearnedProductsDataGrid.Items.SortDescriptions.Clear();

            // Toggle between sort ascending and descending 
            if (SortCompulinkProductsAscending)
            {
                LearnedProductsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Ascending));

                foreach (var col in LearnedProductsDataGrid.Columns)
                    col.SortDirection = null;

                column.SortDirection = ListSortDirection.Ascending;

                SortCompulinkProductsAscending = false;
            }
            else
            {
                LearnedProductsDataGrid.Items.SortDescriptions.Add(new SortDescription(column.SortMemberPath, ListSortDirection.Descending));

                foreach (var col in LearnedProductsDataGrid.Columns)
                    col.SortDirection = null;

                column.SortDirection = ListSortDirection.Descending;

                SortCompulinkProductsAscending = true;
            }

            LearnedProductsDataGrid.Items.Refresh();
        }

        private void SortByChangeEnabled()
        {

        }

        //
        // UI Events
        //

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Action Logging
            string location = GetType().FullName + "." + "." + nameof(AddButton_Click);
            string actionMessage = $"<Create_LearnedProduct_Start>";
            ActionLogger.Log(location, actionMessage);

            try
            {
                // Get product from text fields
                var learnedProduct = GetCreatedLearnedProduct();

                // Make sure there is a compulink product
                if (learnedProduct.CompulinkProduct.Trim() == "")
                {
                    MessageBox.Show("Compulink Product cannot be blank!", "");
                    return;
                }

                // Make sure there is a wva product
                if (learnedProduct.WvaProduct.Trim() == "")
                {
                    MessageBox.Show("WVA Product cannot be blank!", "");
                    return;
                }

                // Make sure product doesn't exist
                if (manageViewModel.CompulinkProductExists(learnedProduct.CompulinkProduct))
                {
                    MessageBox.Show("Compulink product already exists!", "");
                    return;
                }

                // Create a new learned product
                bool productCreated = manageViewModel.CreateLearnedProduct(learnedProduct);

                if (productCreated)
                {
                    // Action Logging
                    actionMessage = $"<Adding product: Compulink={learnedProduct.CompulinkProduct}, Wva={learnedProduct.WvaProduct}, ChangeEnabled={learnedProduct.ChangeEnabled}>";
                    ActionLogger.Log(location, actionMessage);

                    // Update UI
                    ResetCreateLearnedProductElements();
                    RefreshGrid();
                }
                else
                {
                    MessageBox.Show("An error has occurred. Product not added.", "");
                }
            }
            finally
            {
                actionMessage = $"<Create_LearnedProduct_End>";
                ActionLogger.Log(location, actionMessage);
            }
        }

        private async void ImportMatchesButton_Click(object sender, RoutedEventArgs e)
        {
            // Action Logging
            string location = GetType().FullName + "." + "." + nameof(ImportMatchesButton_Click);
            string actionMessage = $"<Import_Matches_Start>";
            ActionLogger.Log(location, actionMessage);

            try
            {
                string file = manageViewModel.GetCsvPath();

                if (file == null || file.Trim() == "")
                {
                    return;
                }
                else if (!manageViewModel.CsvInLearnedProductFormat(File.ReadAllLines(file)))
                {
                    throw new FileFormatException();
                }
                else if (!File.Exists(file))
                {
                    throw new FileNotFoundException($"Could not find file {file}.");
                }
                else
                {
                    LoadingWindow loadingWindow = new LoadingWindow("Importing...");
                    loadingWindow.Show();
                    Cursor = Cursors.Wait;

                    bool imported = await Task.Run(() => manageViewModel.ImportLearnedProducts(file));

                    Cursor = Cursors.Arrow;
                    loadingWindow.Close();

                    if (imported)
                    {
                        RefreshGrid();
                        MessageBox.Show("Products imported! Check your desktop to see import results.", "");
                    }
                }

            }
            catch (FileFormatException)
            {
                MessageBox.Show("The selected CSV file was not in the correct format. Format is as follows: CompulinkProduct, WvaProduct, true/false ","File Format Error");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Could not find file!", "");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "");
                Error.ReportOrLog(ex);
            }
            finally
            {
                actionMessage = $"<Import_Matches_End>";
                ActionLogger.Log(location, actionMessage);
            }
        }

        private async void ImportCompulinkProductsButton_Click(object sender, RoutedEventArgs e)
        {
            // Action Logging
            string location = GetType().FullName + "." + "." + nameof(ImportCompulinkProductsButton_Click);
            string actionMessage = $"<Import_Compulink_Products_Start>";
            ActionLogger.Log(location, actionMessage);

            try
            {
                bool resultsWindowOpen = BringResultsWindowToViewIfOpen();

                if (!resultsWindowOpen)
                {
                    // Get the file to import 
                    string file = manageViewModel.GetCsvPath();
                    List<string> compulinkProducts = manageViewModel.GetCompulinkProductsFromCsv(file);

                    // Display a loading window while we are looking for matches 
                    var loadingWindow = new LoadingWindow();
                    loadingWindow.Show();

                    // Get a list of possible matches for the compulink products
                    var listMatchedProducts = await Task.Run(() => manageViewModel.ImportCompulinkProducts(compulinkProducts));

                    // Convert list of matches to a format that will be easy to display in results view 
                    var listMatchProductResults = GetMatchedProductResults(compulinkProducts, listMatchedProducts);

                    // Close loading window before loading new view
                    loadingWindow.Close();

                    // Open the results view 
                    OpenImportResultsWindow(listMatchProductResults);
                }
            }
            catch (InvalidOperationException)
            {
                return;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Could not find path to specified file.", "File Not Found");
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Could not get access to specified file.", "Unauthorized Access");
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }

            actionMessage = $"<Import_Compulink_Products_End>";
            ActionLogger.Log(location, actionMessage);
        }

        private void WvaProductsContextMenu_Click(object sender, RoutedEventArgs e)
        {
            // Action Logging
            string location = GetType().FullName + "." + "." + nameof(WvaProductsContextMenu_Click);
            string actionMessage = $"<Delete_Products_Start>";
            ActionLogger.Log(location, actionMessage);

            try
            {
                // Confirm with user that they want to delete the selected products 
                var result = MessageBox.Show("Are you sure you want to delete the selected products? They will not be able to be recovered.", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    // Get products from datagrid and cast them into a list
                    var products = LearnedProductsDataGrid.SelectedItems.Cast<LearnedProduct>().ToList();

                    manageViewModel.DeleteLearnedProduct(products);
                    RefreshGrid();

                    // Action Logging
                    foreach (LearnedProduct product in products)
                    {
                        actionMessage = $"<Deleting product: Compulink={product.CompulinkProduct}, WVA={product.WvaProduct}, NumPicks={product.NumPicks}, ChangeEnabled={product.ChangeEnabled}>";
                        ActionLogger.Log(location, actionMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
            finally
            {
                actionMessage = $"<Delete_Products_End>";
                ActionLogger.Log(location, actionMessage);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (LearnedProductsDataGrid.SelectedItem != null)
            {
                string compulinkProduct = ((LearnedProduct)LearnedProductsDataGrid.SelectedItem).CompulinkProduct ?? "";
                Database.UpdateChangeEnabled(compulinkProduct, true);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LearnedProductsDataGrid.SelectedItem != null)
            {
                string compulinkProduct = ((LearnedProduct)LearnedProductsDataGrid.SelectedItem).CompulinkProduct ?? "";
                Database.UpdateChangeEnabled(compulinkProduct, false);
            }
        }

        private async void CheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loadingWindow = new LoadingWindow("Updating...");
                loadingWindow.Show();
                Cursor = Cursors.Wait;

                await Task.Run(() => UpdateChangeEnabledAsync(true));
                RefreshGrid();

                Cursor = Cursors.Arrow;
                loadingWindow.Close();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private async void UnCheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var loadingWindow = new LoadingWindow("Updating...");
                loadingWindow.Show();
                Cursor = Cursors.Wait;

                await Task.Run(() => UpdateChangeEnabledAsync(false));
                RefreshGrid();

                Cursor = Cursors.Arrow;
                loadingWindow.Close();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + "." + nameof(UserControl_Loaded);
            string actionMessage = $"<ManageView_Enter>";
            ActionLogger.Log(location, actionMessage);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            string location = GetType().FullName + "." + "." + nameof(UserControl_Unloaded);
            string actionMessage = $"<ManageViewLeave>";
            ActionLogger.Log(location, actionMessage);
        }

        private void CompulinkProductTextBox_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                // Get the compulink product to match 
                string compulinkProduct = CompulinkProductTextBox.Text;

                if (compulinkProduct.Trim() != "")
                {
                    // Set combo box items
                    WvaProductComboBox.ItemsSource = manageViewModel.GetWvaDropDownMatches(compulinkProduct);
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void RefreshButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                // Open tool tip on mouseover, and change image color
                toolTip.Content = "Refresh Content";
                toolTip.IsOpen = true;
                RefreshImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-available-updates-48.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void RefreshButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                // Close tool tip on mouseleave, and change image color
                toolTip.IsOpen = false;
                RefreshImage.Source = new BitmapImage(new Uri(@"/Resources/icons8-available-updates-filled-48.png", UriKind.Relative));
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Spawn a loading window and change cursor to waiting cursor
                LoadingWindow loadingWindow = new LoadingWindow();
                loadingWindow.Show();
                Mouse.OverrideCursor = Cursors.Wait;

                await Task.Run(() => RefreshGrid());

                // Close loading window and change cursor back to default arrow cursor
                loadingWindow.Close();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void DataGridColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as DataGridColumnHeader).Content) 
            {
                case "Compulink Products":
                    SortByCompulinkProduct();
                    break;

                case "WVA Products":
                    SortByWvaProduct();
                    break;

                case "Changed Enabled":
                    SortByChangeEnabled();
                    break;

                default: break;
            }
        }
    }
}
