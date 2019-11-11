﻿using System;
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
using System.Windows.Shapes;
using WVA_Connect_CDI.Models.Manage;

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
            foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
            {
                if (result.RowColor == "Red")
                    result.IsSelected = true;
            }

            LearnedProductsDataGrid.Items.Refresh();
        }

        private void YellowButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
            {
                if (result.RowColor == "Yellow")
                    result.IsSelected = true;
            }

            LearnedProductsDataGrid.Items.Refresh();
        }

        private void GreenButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (MatchedProductResult result in LearnedProductsDataGrid.Items.Cast<MatchedProductResult>())
            {
                if (result.RowColor == "Green")
                    result.IsSelected = true;
            }

            LearnedProductsDataGrid.Items.Refresh();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }

    }
}
