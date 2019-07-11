using WVA_Compulink_Desktop_Integration.WebTools;
using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Memory;
using WVA_Compulink_Desktop_Integration.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.ViewModels
{
    class SettingsViewModel
    {
        public List<string> GetAvailableAccounts()
        {
            try
            {
                string endpoint = $"http://{UserData.Data?.DSN}/api/user/get-acts";
                string response = API.Get(endpoint, out string httpStatus);
                return JsonConvert.DeserializeObject<List<string>>(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void UpdateUserSettings(bool deleteBlankCompulinkOrders)
        {
            try
            {
                // Updates user settings in memory
                UserData.Data.Settings.DeleteBlankCompulinkOrders = deleteBlankCompulinkOrders;

                // Updates user settings in settings file
                string userSettings = JsonConvert.SerializeObject(UserData.Data?.Settings);

                if (!Directory.Exists(Paths.DataDir))
                    Directory.CreateDirectory(Paths.DataDir);

                if (!File.Exists(Paths.UserSettingsFile))
                    File.Create(Paths.UserSettingsFile).Close();

                File.WriteAllText(Paths.UserSettingsFile, userSettings);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

    }
}
