using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Utility.Files
{
    class Paths
    {
        /* ---------------------------------------- ROOT PATHS --------------------------------------------------------- */

        public static readonly string AppDataLocal              = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string PublicDocs               = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        private static readonly string AppName                  = Assembly.GetCallingAssembly().GetName().Name.ToString();


        /* ---------------------------------------- APP DATA --------------------------------------------------------- */

        // DIRECTORIES 
        public static readonly string TempDir = $@"{AppDataLocal}\{AppName}\Temp\";
        public static readonly string DataDir = $@"{AppDataLocal}\{AppName}\Data\";
        public static readonly string ErrorLogDir = $@"{AppDataLocal}\{AppName}\ErrorLog\";
        public static readonly string ActNumDir = $@"{AppDataLocal}\{AppName}\ActNum\";


        // FILES
        public static readonly string ActNumFile = $@"{AppDataLocal}\{AppName}\ActNum\ActNum.txt";
        public static readonly string ProductDatabaseFile = $@"{AppDataLocal}\{AppName}\Data\ProductPrediction.sqlite";
        public static readonly string UserSettingsFile = $@"{AppDataLocal}\{AppName}\Data\Settings.json";
        public static readonly string ProductCrossRefFile = $@"{AppDataLocal}\{AppName}\Data\ProductCrossRef.json";
        public static readonly string PrevTimePassChangeFile = $@"{AppDataLocal}\{AppName}\Temp\PrevTimePassChange.txt";
        public static readonly string AppExecFile = $@"{AppDataLocal}\{AppName}\{AppName}.exe";


        /* ---------------------------------------- PUBLIC DOCS --------------------------------------------------------- */

        // DIRECTORIES 
        public static readonly string ApiKeyDir                 = $@"{PublicDocs}\{AppName}\ApiKey\";
        public static readonly string IpConfigDir               = $@"{PublicDocs}\{AppName}\IpConfig\";

        // FILES
        public static readonly string ApiKeyFile                = $@"{PublicDocs}\{AppName}\ApiKey\ApiKey.txt";
        public static readonly string IpConfigFile              = $@"{PublicDocs}\{AppName}\IpConfig\IpConfig.txt";


        /* -------------------------------------------- WEB PATHS --------------------------------------------------------- */

        public static readonly string WisVisBase                = "https://orders.wisvis.com";
        public static readonly string WisVisOrders              = $@"{WisVisBase}/orders";
        public static readonly string WisVisOrderStatus         = $@"https://orders.wisvis.com/order_status";
        public static readonly string WisVisErrors              = $@"https://ws2.wisvis.com/aws/scanner/error_handler.rb";
    }
}
