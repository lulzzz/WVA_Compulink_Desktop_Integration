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
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions.Models;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.ViewModels;

namespace WVA_Connect_CDI.Views
{
    /// <summary>
    /// Interaction logic for ManageView.xaml
    /// </summary>
    public partial class ManageView : UserControl
    {
        ManageViewModel manageViewModel;

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
                WvaProduct = WvaProductTextBox.Text,
                ChangeEnabled = (bool)IsEditableCheckBox.IsChecked,
                NumPicks = 10
            };
        }

        private void RefreshGrid()
        {
            LearnedProductsDataGrid.ItemsSource = manageViewModel.GetLearnedProducts();
            LearnedProductsDataGrid.Items.Refresh();
        }

        private void ResetCreateLearnedProductElements()
        {
            CompulinkProductTextBox.Text = "";
            WvaProductTextBox.Text = "";
            IsEditableCheckBox.IsChecked = true;
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

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {
            // Action Logging
            string location = GetType().FullName + "." + "." + nameof(ImportButton_Click);
            string actionMessage = $"<Import_Products_Start>";
            ActionLogger.Log(location, actionMessage);

            try
            {
                manageViewModel.ImportLearnedProducts();
                RefreshGrid();
                MessageBox.Show("Products imported! Check your desktop to see import results.", "");
            }
            catch (FileFormatException ex)
            {
                MessageBox.Show("The selected CSV file was not in the correct format. Format is as follows: CompulinkProduct, WvaProduct, true/false ","File Format Error");
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "");
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
            finally
            {
                actionMessage = $"<Import_Products_End>";
                ActionLogger.Log(location, actionMessage);
            }
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
                string compulinkProduct = ((LearnedProduct)LearnedProductsDataGrid.SelectedItem).CompulinkProduct;
                Database.UpdateChangeEnabled(compulinkProduct, true);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (LearnedProductsDataGrid.SelectedItem != null)
            {
                string compulinkProduct = ((LearnedProduct)LearnedProductsDataGrid.SelectedItem).CompulinkProduct;
                Database.UpdateChangeEnabled(compulinkProduct, false);
            }
        }

        private void CheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (LearnedProduct product in LearnedProductsDataGrid.Items.Cast<LearnedProduct>().ToList())
                {
                    Database.UpdateChangeEnabled(product.CompulinkProduct, true);
                }

                RefreshGrid();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void UnCheckAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (LearnedProduct product in LearnedProductsDataGrid.Items.Cast<LearnedProduct>().ToList())
                {
                    Database.UpdateChangeEnabled(product.CompulinkProduct, false);
                }

                RefreshGrid();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        //
        // Loaded and Unloaded Events
        //

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
    }
}
