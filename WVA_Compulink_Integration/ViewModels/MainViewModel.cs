using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Products.ProductOut;
using WVA_Connect_CDI.Updates;
using WVA_Connect_CDI.Utility.Actions;
using WVA_Connect_CDI.Utility.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WVA_Connect_CDI.ProductMatcher.Data;

namespace WVA_Connect_CDI.ViewModels
{
    public class MainViewModel
    {
        public void SetupDatabase()
        {
            Database.SetUpDatabase();
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
                if (!Directory.Exists(AppPath.ActNumDir))
                    Directory.CreateDirectory(AppPath.ActNumDir);

                if (!File.Exists(AppPath.ActNumFile))
                {
                    var file = File.Create(AppPath.ActNumFile);
                    file.Close();
                }

                UserData.Data.Account = File.ReadAllText(AppPath.ActNumFile).Trim();
                UserData.Data.ApiKey = File.ReadAllText(AppPath.ApiKeyFile).Trim();
                UserData.Data.DSN = File.ReadAllText(AppPath.IpConfigFile).Trim();
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
            }
        }

        public void ReportActionLogData()
        {
            List<ActionData> data = ActionLogger.GetDataNotToday();

            if (data == null || data.Count <= 0)
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
