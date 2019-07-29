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

    class SqliteDataAccess
    {
        
        public static string GetWvaProduct(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<Product>($"SELECT WvaProduct FROM products WHERE CompulinkProduct = '{compulinkProduct}'").FirstOrDefault();
                return product?.WvaProduct;
            }
        }

        public static string GetCompulinkProduct(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<Product>($"SELECT CompulinkProduct FROM products WHERE CompulinkProduct = '{compulinkProduct}'").FirstOrDefault();
                return product?.CompulinkProduct;
            }
        }

        public static string GetCompulinkProductFromMatch(string compulinkProduct, string wvaProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<Product>($"SELECT CompulinkProduct FROM products WHERE CompulinkProduct = '{compulinkProduct}' AND WvaProduct = '{wvaProduct}'").FirstOrDefault();
                return product?.CompulinkProduct;
            }
            
        }

        public static int GetNumPicks(string compulinkProduct)
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                var product = cnn.Query<Product>($"SELECT NumPicks FROM products WHERE Compulinkproduct = '{compulinkProduct}'").FirstOrDefault();
                return product != null ? product.NumPicks : 0;
            }
        }

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

        public static void DeleteProductsTable()
        {
            using (IDbConnection cnn = new SQLiteConnection(GetDbConnectionString()))
            {
                cnn.Execute("DELETE FROM Products;");
            }
        }

        private static string GetDbConnectionString(string id = "Default")
        {
            return $@"Data Source={Paths.ProductDatabaseFile};Version=3;";
        }

    }
}
