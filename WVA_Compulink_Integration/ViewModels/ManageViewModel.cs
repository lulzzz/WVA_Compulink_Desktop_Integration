using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.ProductMatcher.Data;
using WVA_Connect_CDI.ProductMatcher.Models;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.Utility.Files;

namespace WVA_Connect_CDI.ViewModels
{
    public class ManageViewModel
    {
        public List<LearnedProduct> LearnedProducts = new List<LearnedProduct>();

        public ManageViewModel()
        {
            SetLearnedProducts();
        }

        //
        // CREATE
        //

        // Creates a new learned product in the database
        public bool CreateLearnedProduct(LearnedProduct product)
        {
            return Database.CreateLearnedProduct(product);
        }

        // Checks if a learned product exists for a given compulink product string
        public bool CompulinkProductExists(string compulinkProduct)
        {
            return Database.CompulinkProductExists(compulinkProduct);
        }

        //
        // READ
        //

        // Updates the 'LearnedProducts' list in memory
        public void SetLearnedProducts()
        {
            LearnedProducts = GetLearnedProducts();
        }

        // Gets all learned products from the database
        public List<LearnedProduct> GetLearnedProducts()
        {
            return Database.GetLearnedProducts();
        }
       
        // Converts string array lines from a csv to a learned product list 
        private List<LearnedProduct> GetLearnedProducts(string[] csvLines)
        {
            var learnedProducts = new List<LearnedProduct>();

            foreach (string line in csvLines)
            {
                string[] lineItems = line.Split(',');

                var product = new LearnedProduct()
                {
                    CompulinkProduct = lineItems[0].Trim(),
                    WvaProduct = lineItems[1].Trim(),
                    NumPicks = 10
                };

                // Product change enabled is true by default. User does not have to include it
                try
                {
                    product.ChangeEnabled = Convert.ToBoolean(lineItems[2].Trim());
                }
                catch
                {
                    product.ChangeEnabled = true;
                }

                learnedProducts.Add(product);
            }

            return learnedProducts;
        }

        //
        // UPDATE
        //



        //
        // DELETE
        //

        // Removes a learned product from the database
        public bool DeleteLearnedProduct(List<LearnedProduct> products)
        {
            return Database.DeleteLearnedProduct(products);
        }

        //
        // Import Learned Products
        //

        // Imports a csv or txt file to inject product matches into the database
        public void ImportLearnedProducts()
        {
            string file = GetCsvPath();

            if (file == null || file.Trim() == "")
            {
                return;
            }
            else if (!CsvInCorrectFormat(File.ReadAllLines(file)))
            {
                throw new FileFormatException();
            }
            else if (!File.Exists(file))
            {
                throw new FileNotFoundException($"Could not find file {file}.");
            }
            else
            {
                var learnedProducts = GetLearnedProducts(File.ReadAllLines(file));

                foreach (LearnedProduct product in learnedProducts)
                {
                    bool created = false;
                    string importFile = AppPath.DesktopDir + "\\Product Import Log.txt";
                    string message = "";

                    if (!Database.CompulinkProductExists(product.CompulinkProduct))
                    {
                        created = CreateLearnedProduct(product);
                        message = created ? $"SUCCESS! Compulink product '{product.CompulinkProduct}' added!" : $"Failed to add line: {product.ChangeEnabled}, {product.WvaProduct}, {product.ChangeEnabled}";

                        string location = GetType().FullName + "." + "." + nameof(GetLearnedProducts);
                        string actionMessage = $"<Importing product: {product.CompulinkProduct}, {product.WvaProduct}, {product.ChangeEnabled}> <created={created}, message={message}>";
                        ActionLogger.Log(location, actionMessage);
                    }
                    else
                        message = $"FAIL! Compulink product '{product.CompulinkProduct}' already exists.";
                    
                    File.AppendAllText(importFile, message + "\n");
                }
            }
        }

        // Opens a file dialog for user to select a file to import
        private string GetCsvPath()
        {
            OpenFileDialog choofdlog = new OpenFileDialog();
            choofdlog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            choofdlog.ShowDialog();

            return choofdlog.FileName;
        }

        // Checks csv to be sure it is safe to import 
        private bool CsvInCorrectFormat(string[] csvLines)
        {
            foreach (string line in csvLines)
            {
                string[] lineItems = line.Split(',');

                // Make sure there are 2 || 3 items
                if (lineItems.Count() < 2 || lineItems.Count() > 3)
                    return false;

                // Check first item
                if (lineItems[0].Trim() == "")
                    return false;

                // Check second item
                if (lineItems[1].Trim() == "")
                    return false;
            }

            return true;
        }
        
    }
}
