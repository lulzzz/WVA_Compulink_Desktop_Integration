using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.Users;

namespace WVA_Connect_CDI.ViewModels
{
    public class SettingsViewModel
    {
        public List<string> GetAvailableAccounts()
        {
            try
            {
                return File.ReadAllLines(AppPath.AvailableActsFile).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> GetAllAvailableAccounts(string dsn)
        {
            try
            {
                string endpoint = $"http://{dsn}/api/user/get-acts";
                string response = API.Get(endpoint, out string httpStatus);
                return JsonConvert.DeserializeObject<List<string>>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private void SetAvailableAccounts(List<string> accounts)
        {
            try
            {
                File.WriteAllLines(AppPath.AvailableActsFile, accounts);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public void AddAvailableAccount(string accountToAdd)
        {
            if (accountToAdd == null || accountToAdd.Trim() == "")
                return;

            // Get list of available accounts from file
            var accounts = GetAvailableAccounts();

            // Add 'accountToAdd' if it doesn't exist in that file
            if (!accounts.Contains(accountToAdd))
                accounts.Add(accountToAdd);

            SetAvailableAccounts(accounts);
        }

        public void UpdateUserSettings(UserSettings userSettings)
        {
            try
            {
                // Updates user settings in memory
                UserData.Data.Settings.DeleteBlankCompulinkOrders = userSettings.DeleteBlankCompulinkOrders;
                UserData.Data.Settings.AutoFillLearnedProducts = userSettings.AutoFillLearnedProducts;
                UserData.Data.Settings.AutoUpdate = userSettings.AutoUpdate;

                // Updates user settings in settings file
                string strUserSettings = JsonConvert.SerializeObject(UserData.Data?.Settings);

                if (!Directory.Exists(AppPath.UserDataDir))
                    Directory.CreateDirectory(AppPath.UserDataDir);

                if (!File.Exists(AppPath.UserSettingsFile))
                    File.Create(AppPath.UserSettingsFile).Close();

                File.WriteAllText(AppPath.UserSettingsFile, strUserSettings);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public string GetSavedAccountNumber()
        {
            try
            {
                return File.ReadAllText(AppPath.ActNumFile);
            }
            catch
            {
                return "";
            }
        }

    }
}
