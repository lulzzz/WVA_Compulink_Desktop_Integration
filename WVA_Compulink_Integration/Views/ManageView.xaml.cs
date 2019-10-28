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
                MessageBox.Show("An error has occurred. Product not added","");
            }
        }

        private void ImportButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
