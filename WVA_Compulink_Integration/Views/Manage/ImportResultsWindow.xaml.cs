using Microsoft.Win32;
using Newtonsoft.Json;
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
using System.Windows.Shapes;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Models.Manage;
using WVA_Connect_CDI.ProductMatcher.Models;
using WVA_Connect_CDI.ViewModels.Manage;

namespace WVA_Connect_CDI.Views.Manage
{
    public partial class ImportResultsWindow : Window
    {
        //
        // Constructors
        //

        public ImportResultsWindow()
        {
            InitializeComponent();
        }

        public ImportResultsWindow(List<MatchedProductResult> matchedProductResults)
        {
            InitializeComponent();
            SetUpGrid(matchedProductResults);
        }

        //
        // Initial Setup
        //

        private void SetUpGrid(List<MatchedProductResult> matchedProductResults)
        {
            foreach (MatchedProductResult result in matchedProductResults)
            {
                LearnedProductsDataGrid.Items.Add(result);
            }
        }

        //
        // UI Events
        //

        private void RedButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
                    if (result.RowColor == "Red")
                        result.IsSelected = true;

                LearnedProductsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void YellowButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
                    if (result.RowColor == "Yellow")
                        result.IsSelected = true;

                LearnedProductsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
                    if (result.RowColor == "Green")
                        result.IsSelected = true;

                LearnedProductsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void SelectAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
                    result.IsSelected = true;

                LearnedProductsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void UnselectAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
                    result.IsSelected = false;

                LearnedProductsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void AddSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var manageViewModel = new ManageViewModel();
            var productMatches = new List<MatchedProductResult>();

            try
            {
                productMatches.AddRange(LearnedProductsDataGrid.Items.Cast<MatchedProductResult>());

                int index = 0;
                foreach (MatchedProductResult result in productMatches)
                {
                    if (result.IsSelected == true)
                    {
                        var learnedProduct = new LearnedProduct()
                        {
                            CompulinkProduct = result.CompulinkProduct,
                            WvaProduct = result.WvaProduct,
                            ChangeEnabled = true,
                            NumPicks = 10
                        };

                        if (!manageViewModel.CompulinkProductExists(result.CompulinkProduct)) manageViewModel.CreateLearnedProduct(learnedProduct);
                        LearnedProductsDataGrid.Items.RemoveAt(index);
                        LearnedProductsDataGrid.Items.Refresh();
                        index--;
                    }

                    index++;
                }

                MessageBox.Show("Product matches added!", "");
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void DeleteSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                for (int i = 0; i < LearnedProductsDataGrid.Items.Count; i++)
                    if (((MatchedProductResult)LearnedProductsDataGrid.Items[i]).IsSelected)
                        LearnedProductsDataGrid.Items.RemoveAt(i);

                LearnedProductsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void AddAllButton_Click(object sender, RoutedEventArgs e)
        {
            var manageViewModel = new ManageViewModel();
            var productMatches = new List<MatchedProductResult>();

            try
            {
                productMatches.AddRange(LearnedProductsDataGrid.Items.Cast<MatchedProductResult>());

                foreach (MatchedProductResult result in productMatches)
                {
                    var learnedProduct = new LearnedProduct()
                    {
                        CompulinkProduct = result.CompulinkProduct,
                        WvaProduct = result.WvaProduct,
                        ChangeEnabled = true,
                        NumPicks = 10
                    };

                    if (!manageViewModel.CompulinkProductExists(result.CompulinkProduct)) manageViewModel.CreateLearnedProduct(learnedProduct);
                    LearnedProductsDataGrid.Items.RemoveAt(0);
                    LearnedProductsDataGrid.Items.Refresh();
                }

                MessageBox.Show("Product matches added!", "");
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void DeleteAllButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LearnedProductsDataGrid.Items.Clear();
                LearnedProductsDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void DatagridComboBox_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                ComboBox cb = sender as ComboBox;
                int selectedRowIndex = LearnedProductsDataGrid.SelectedIndex;

                if (selectedRowIndex >= 0 && selectedRowIndex < LearnedProductsDataGrid.Items.Count)
                {
                    string selectedProduct = cb.SelectedValue.ToString();
                    int selectedDropDownIndex = cb.SelectedIndex;

                    // Get selected row in datagrid
                    var selectedRowItem = ((MatchedProductResult)LearnedProductsDataGrid.Items[selectedRowIndex]);

                    // Update WvaProduct
                    selectedRowItem.WvaProduct = selectedProduct;

                    // Update MatchScore
                    selectedRowItem.MatchScore = selectedRowItem.MatchedProducts[selectedDropDownIndex].MatchScore;

                   LearnedProductsDataGrid.Items.Refresh();
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void SaveResultsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "JSON File|*.json";
                saveFileDialog.Title = "Save Import Data";
                saveFileDialog.FileName = "SavedResults.json";
                saveFileDialog.ShowDialog();

                string path = saveFileDialog.FileName;

                if (!string.IsNullOrWhiteSpace(path))
                {
                    var listMatchedProductResults = LearnedProductsDataGrid.Items.Cast<MatchedProductResult>();
                    string jsonProducts = JsonConvert.SerializeObject(listMatchedProductResults);
                    File.WriteAllText(path, jsonProducts);
                }
                else
                {
                    MessageBox.Show("File name was not valid!", "");
                }
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void LoadSavedResultsButton_Click(object sender, RoutedEventArgs e)
        {
            // as for location of saved file


            // read from file at specified location 


            // convert string json data to object


            // add json data items to datagrid 
            
        }
    }
}
