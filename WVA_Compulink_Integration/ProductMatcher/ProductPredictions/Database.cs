using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Utility.Files;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions.Models;

namespace WVA_Connect_CDI.MatchFinder.ProductPredictions
{
    public class Database
    {
        public static void SetUpDatabase()
        {
            try
            {
                // Create the path to the product database file if it doesn't exists already
                if (!Directory.Exists($"{AppPath.DataDir}"))
                    Directory.CreateDirectory($"{AppPath.DataDir}");

                // Create product database file if it's not created already and set up table
                if (!File.Exists($"{AppPath.ProductDatabaseFile}"))
                {
                    SQLiteConnection.CreateFile($"{AppPath.ProductDatabaseFile}");
                    SqliteDataAccess.CreateProductsTable();
                }

                // Check if products table exists 
                if (!SqliteDataAccess.ProductTableExists())
                    SqliteDataAccess.CreateProductsTable();

                // Create the ChangeEnabled column if it doesn't exist
                SqliteDataAccess.AddChangeEnabledColumn();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        //
        // CREATE
        //

        public static bool CreateCompulinkProduct(string compulinkProduct, string wvaProduct)
        {
            try
            {
                SqliteDataAccess.CreateCompulinkProduct(compulinkProduct, wvaProduct);
                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        public static bool CreateLearnedProduct(LearnedProduct product)
        {
            try
            {
                SqliteDataAccess.CreateProduct(product);
                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        //
        // READ
        //

        public static List<LearnedProduct> GetLearnedProducts()
        {
            try
            {
                return SqliteDataAccess.GetLearnedProducts();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return new List<LearnedProduct>();
            }
        }

        public static int GetNumPicks(string compulinkProduct)
        {
            try
            {
                return SqliteDataAccess.GetNumPicks(compulinkProduct);
            }
            catch
            {
                return 0;
            }
        }

        public static bool CompulinkProductExists(string compulinkProduct)
        {
            try
            {
                // Update order status to 'submitted'
                string product = SqliteDataAccess.GetCompulinkProduct(compulinkProduct);

                return product == null ? false : true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        public static bool ProductMatchExists(string compulinkProduct, string wvaProduct)
        {
            try
            {
                string readCompProd = SqliteDataAccess.GetCompulinkProductFromMatch(compulinkProduct, wvaProduct);

                return readCompProd != null ? true : false;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        public static string ReturnWvaProductFor(string compulinkProduct)
        {
            try
            {
                return SqliteDataAccess.GetWvaProduct(compulinkProduct);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        //
        // UPDATE
        //

        public static void UpdateCompulinkProductMatch(string compulinkProduct, string wvaProduct)
        {
            try
            {
                SqliteDataAccess.UpdateCompulinkProductMatch(compulinkProduct, wvaProduct);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public static void IncrementNumPicks(string compulinkProduct)
        {
            try
            {
                SqliteDataAccess.IncrementNumPicks(compulinkProduct);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public static void DecrementNumPicks(string compulinkProduct)
        {
            try
            {
                SqliteDataAccess.DecrementNumPicks(compulinkProduct);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        //
        // DESTROY  
        //

        public static bool DeleteLearnedProduct(LearnedProduct product)
        {
            try
            {
                SqliteDataAccess.DeleteLearnedProduct(product);
                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        public static bool DeleteLearnedProduct(List<LearnedProduct> products)
        {
            try
            {
                SqliteDataAccess.DeleteLearnedProduct(products);
                return true;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }


    }
}
