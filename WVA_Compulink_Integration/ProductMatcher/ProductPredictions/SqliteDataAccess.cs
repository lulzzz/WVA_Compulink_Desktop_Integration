using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.ProductMatcher.ProductPredictions.Models;
using WVA_Connect_CDI.Utility.Files;

namespace WVA_Connect_CDI.ProductMatcher.ProductPredictions
{
    //  
    // NOTE: This class ONLY handles direct SQL queries. It is not intended to handle any other logic - that is taken care of in the Database class.
    //

    public class SqliteDataAccess
    {

        //
        // TABLE MODIFICATION
        //
            
        public static void AddChangeEnabledColumn()
        {
            try
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute("ALTER TABLE products ADD COLUMN ChangeEnabled INT");
                }
            }
            catch
            {
                // Ignore error because column exists
            }
        }

        public static bool ProductTableExists()
        {
            var cnn = new SQLiteConnection(GetDbConnectionString());

            cnn.Open();

            string query = "SELECT name " +
                            "FROM sqlite_master " +
                            "WHERE type='table' " +
                            "AND name='products'";

            SQLiteCommand command = new SQLiteCommand(query, cnn);
            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    return reader[0].ToString() == "products" ? true : false;
                }
            }

            return false;
        }

        //
        // CREATE
        //

        public static void CreateCompulinkProduct(string compulinkProduct, string wvaProduct, int numPicks = 1)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute("INSERT OR IGNORE into products (" +
                                        "CompulinkProduct, " +
                                        "WvaProduct, " +
                                        "NumPicks) " +
                                        "values (" +
                                            $"'{compulinkProduct}', " +
                                            $"'{wvaProduct}', " +
                                            $"'{numPicks}'" +
                                            ")");
            }
        }

        public static void CreateProduct(LearnedProduct product)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute("INSERT OR IGNORE into products (" +
                                        "CompulinkProduct, " +
                                        "WvaProduct, " +
                                        "NumPicks, " +
                                        "ChangeEnabled) " +
                                        "values (" +
                                            $"'{product.CompulinkProduct}', " +
                                            $"'{product.WvaProduct}', " +
                                            $"'{product.NumPicks}'," +
                                            $"'{(product.ChangeEnabled ? 1 : 0)}'" +
                                            ")");
            }
        }

        public static void CreateProductsTable()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute("CREATE TABLE products (" +
                                    "Id                     INTEGER PRIMARY KEY AUTOINCREMENT, " +
                                    "CompulinkProduct       TEXT, " +
                                    "WvaProduct             TEXT, " +
                                    "NumPicks               INT); ");
            }
        }

        //
        // READ
        // 

        public static LearnedProduct GetLearnedProduct(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<LearnedProduct>($"SELECT * FROM products WHERE CompulinkProduct = '{compulinkProduct}'").FirstOrDefault();
                return product;
            }
        }

        public static List<LearnedProduct> GetLearnedProducts()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var products = cnn.Query<LearnedProduct>($"SELECT * FROM products").ToList();
                return products;
            }
        }

        public static string GetWvaProduct(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<LearnedProduct>($"SELECT WvaProduct FROM products WHERE CompulinkProduct = '{compulinkProduct}'").FirstOrDefault();
                return product?.WvaProduct;
            }
        }

        public static string GetCompulinkProduct(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<LearnedProduct>($"SELECT CompulinkProduct FROM products WHERE CompulinkProduct = '{compulinkProduct}'").FirstOrDefault();
                return product?.CompulinkProduct;
            }
        }

        public static string GetCompulinkProductFromMatch(string compulinkProduct, string wvaProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<LearnedProduct>($"SELECT CompulinkProduct FROM products WHERE CompulinkProduct = '{compulinkProduct}' AND WvaProduct = '{wvaProduct}'").FirstOrDefault();
                return product?.CompulinkProduct;
            }
        }

        public static int GetNumPicks(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<LearnedProduct>($"SELECT NumPicks FROM products WHERE Compulinkproduct = '{compulinkProduct}'").FirstOrDefault();
                return product != null ? product.NumPicks : 0;
            }
        }

        //
        // UPDATE
        //

        public static void IncrementNumPicks(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE products SET NumPicks = '{GetNumPicks(compulinkProduct) + 1}' WHERE CompulinkProduct = '{compulinkProduct}'");
            }
        }

        public static void DecrementNumPicks(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE products SET NumPicks = '{GetNumPicks(compulinkProduct) - 1}' WHERE CompulinkProduct = '{compulinkProduct}'");
            }
        }

        public static void UpdateCompulinkProductMatch(string compulinkProduct, string wvaProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE products SET WvaProduct = '{wvaProduct}' WHERE CompulinkProduct = '{compulinkProduct}'");
            }
        }

        public static void UpdateChangeEnabled(string compulinkProduct, bool changeEnabled)
        {
            int intChangeEnabled = changeEnabled ? 1 : 0;

            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"UPDATE products SET ChangeEnabled = '{intChangeEnabled}' WHERE CompulinkProduct = '{compulinkProduct}'");
            }
        }

        // 
        // DESTROY
        //

        public static void DeleteLearnedProduct(LearnedProduct product)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute($"DELETE FROM products WHERE CompulinkProduct = '{product.CompulinkProduct}'");
            }
        }

        public static void DeleteLearnedProduct(List<LearnedProduct> products)
        {
            foreach (LearnedProduct product in products)
            {
                using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
                {
                    cnn.Execute($"DELETE FROM products WHERE CompulinkProduct = '{product.CompulinkProduct}'");
                }
            }
        }

        //
        // HELPER METHODS
        //

        private static string GetDbConnectionString()
        {
            return $@"Data Source={AppPath.ProductDatabaseFile};Version=3;";
        }

    }
}
