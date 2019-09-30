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
    class SettingsViewModel
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

        public void UpdateUserSettings(UserSettings userSettings)
        {
            try
            {
                // Updates user settings in memory
                UserData.Data.Settings.DeleteBlankCompulinkOrders = userSettings.DeleteBlankCompulinkOrders;
                UserData.Data.Settings.AutoFillLearnedProducts = userSettings.AutoFillLearnedProducts;

                // Updates user settings in settings file
                string strUserSettings = JsonConvert.SerializeObject(UserData.Data?.Settings);

                if (!Directory.Exists(AppPath.DataDir))
                    Directory.CreateDirectory(AppPath.DataDir);

                if (!File.Exists(AppPath.UserSettingsFile))
                    File.Create(AppPath.UserSettingsFile).Close();

                File.WriteAllText(AppPath.UserSettingsFile, strUserSettings);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

    }
}
