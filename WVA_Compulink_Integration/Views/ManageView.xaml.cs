using System;
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
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions.Models;
using WVA_Connect_CDI.ViewModels;

namespace WVA_Connect_CDI.Views
{
    /// <summary>
    /// Interaction logic for ManageView.xaml
    /// </summary>
    public partial class ManageView : UserControl
    {
        ManageViewModel manageViewModel = new ManageViewModel();

        public ManageView()
        {
            InitializeComponent();
            RefreshGrid();
            SetUpContextMenu();
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
                WvaProduct = WvaProductTextBox.Text,
                ChangeEnabled = (bool)IsEditableCheckBox.IsChecked,
                NumPicks = 0
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
            var learnedProduct = GetCreatedLearnedProduct();
 
            bool productCreated = manageViewModel.CreateLearnedProduct(learnedProduct);

            if (productCreated)
            {
                ResetCreateLearnedProductElements();
                RefreshGrid();
            }
            else
            {
                MessageBox.Show("An error has occurred. Product not added.","");
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Datagrid context menu
        private void WvaProductsContextMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Confirm with user that they want to delete the selected products 
                var result = MessageBox.Show("Are you sure you want to delete the selected products? They will not be able to be recovered.", "Attention!", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var products = LearnedProductsDataGrid.SelectedItems.Cast<LearnedProduct>().ToList();

                    Database.DeleteLearnedProduct(products);
                    RefreshGrid();
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
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
    }
}
