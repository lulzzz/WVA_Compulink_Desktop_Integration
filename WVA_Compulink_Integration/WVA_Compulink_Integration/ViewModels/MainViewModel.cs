using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Memory;
using WVA_Compulink_Desktop_Integration.Models.Products.ProductOut;
using WVA_Compulink_Desktop_Integration.Updates;
using WVA_Compulink_Desktop_Integration.Utility.Actions;
using WVA_Compulink_Desktop_Integration.Utility.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Compulink_Desktop_Integration.Models.Products;

namespace WVA_Compulink_Desktop_Integration.ViewModels
{
    public class MainViewModel
    {
        public void Update()
        {
            // Start the updater on a new thread
            Task.Run(() => Updater.CheckForUpdates());
        }

        public void CheckForProductPredictionUpdates()
        {
            ProductCrossRefDict.CheckForUpdates();
        }

        public bool AccountNumAvailable()
        {
            try
            {
                SetUserData();
                if (UserData.Data?.Account != null && UserData.Data.Account.Trim() != "")
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private void SetUserData()
        {
            // Set account number, api key, DSN to Mem user data
            try
            {
                // Create ActNum file path if not already
                if (!Directory.Exists(Paths.ActNumDir))
                    Directory.CreateDirectory(Paths.ActNumDir);

                if (!File.Exists(Paths.ActNumFile))
                {
                    var file = File.Create(Paths.ActNumFile);
                    file.Close();
                }

                UserData.Data.Account = File.ReadAllText(Paths.ActNumFile).Trim();
                UserData.Data.ApiKey = File.ReadAllText(Paths.ApiKeyFile).Trim();
                UserData.Data.DSN = File.ReadAllText(Paths.IpConfigFile).Trim();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        public void ReportActionLogData()
        {
            List<ActionData> data = ActionLogger.GetDataNotToday();

            if (data == null)
                return;

            foreach (ActionData d in data)
            {
                bool dataReported = ActionLogger.ReportData($"FileName={d.FileName} \n{d.Content}");

                if (dataReported)
                    File.Delete(d.FileName);
            }
        }

        public async Task<bool> LoadProductsAsync()
        {
            try
            {
                string dsn = UserData.Data?.DSN ?? throw new NullReferenceException("DSN not set in MainWindow.LoadProducts.");
                string endpoint = $"http://{dsn}/api/product/";

                RequestOut request = new RequestOut()
                {
                    Request = new ProductOut()
                    {
                        Api_key = UserData.Data?.ApiKey ?? throw new NullReferenceException("ApiKey not set in MainWindow.LoadProducts."),
                        AccountID = UserData.Data?.Account ?? throw new NullReferenceException("Account ID not set in MainWindow.LoadProducts.")
                    }
                };

                WvaProducts.LoadProductList(request, endpoint);
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
