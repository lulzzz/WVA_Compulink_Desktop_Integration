using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Utility.Files
{
    class AppPath
    {
        /* ---------------------------------------- ROOT PATHS --------------------------------------------------------- */

        public static readonly string AppDataLocal              = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string PublicDocs                = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
        public static readonly string AppName                   = Assembly.GetCallingAssembly().GetName().Name.ToString();
        public static readonly string DesktopDir                = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);


        /* ---------------------------------------- APP DATA --------------------------------------------------------- */

        // DIRECTORIES 
        public static readonly string TempDir                   = $@"{AppDataLocal}\{AppName}\Temp\";
        public static readonly string UserDataDir               = $@"{AppDataLocal}\{AppName}\Data\";
        public static readonly string ErrorLogDir               = $@"{AppDataLocal}\{AppName}\ErrorLog\";
        public static readonly string ActNumDir                 = $@"{AppDataLocal}\{AppName}\ActNum\";
        public static readonly string AppBasePath               = $@"{AppDataLocal}\{AppName}\";

        // FILES
        public static readonly string ActNumFile                = $@"{AppDataLocal}\{AppName}\ActNum\ActNum.txt";
        public static readonly string UserSettingsFile          = $@"{AppDataLocal}\{AppName}\Data\Settings.json";
        public static readonly string PrevTimePassChangeFile    = $@"{AppDataLocal}\{AppName}\Temp\PrevTimePassChange.txt";
        public static readonly string AppExecFile               = $@"{AppDataLocal}\{AppName}\{AppName}.exe";
        public static readonly string AvailableActsFile         = $@"{AppDataLocal}\{AppName}\ActNum\AvailableActs.txt";


        /* ---------------------------------------- PUBLIC DOCS --------------------------------------------------------- */

        // DIRECTORIES 
        public static readonly string ApiKeyDir                 = $@"{PublicDocs}\{AppName}\ApiKey\";
        public static readonly string IpConfigDir               = $@"{PublicDocs}\{AppName}\IpConfig\";
        public static readonly string PublicDataDir             = $@"{PublicDocs}\{AppName}\Data\";

        // FILES
        public static readonly string ApiKeyFile                = $@"{PublicDocs}\{AppName}\ApiKey\ApiKey.txt";
        public static readonly string IpConfigFile              = $@"{PublicDocs}\{AppName}\IpConfig\IpConfig.txt";
        public static readonly string ProductDatabaseFile       = $@"{PublicDataDir}ProductPrediction.sqlite";


        /* -------------------------------------------- WEB PATHS --------------------------------------------------------- */

        public static readonly string WisVisBase                = "https://orders.wisvis.com";
        public static readonly string WisVisOrders              = $@"{WisVisBase}/orders";
        public static readonly string WisVisOrderStatus         = $@"https://orders.wisvis.com/order_status";
        public static readonly string WisVisErrors              = $@"https://ws2.wisvis.com/aws/scanner/error_handler.rb";
        public static readonly string WisVisValidations         = $@"https://orders.wisvis.com/validations";
        public static readonly string WisVisCdiRepo             = $@"https://github.com/WVATeam/WVA_Compulink_Desktop_Integration";
    }
}
