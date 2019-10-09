using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Utility.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Models.Users;
using Newtonsoft.Json;

namespace WVA_Connect_CDI.ViewModels.Login
{
    public class IpConfigViewModel
    {
        public string GetIpConfig()
        {
            try
            {
                return File.ReadAllText(AppPath.IpConfigFile);
            }
            catch 
            {
                Directory.CreateDirectory(AppPath.IpConfigDir);
                File.Create(AppPath.IpConfigFile).Close();

                return "";
            }
        }

        public string GetApiKey()
        {
            try
            {
                return File.ReadAllText(AppPath.ApiKeyFile);
            }
            catch
            {
                Directory.CreateDirectory(AppPath.ApiKeyDir);
                File.Create(AppPath.ApiKeyFile).Close();

                return "";
            }
        }

        public string GetActNum()
        {
            try
            {
                return File.ReadAllText(AppPath.ActNumFile);
            }
            catch 
            {
                Directory.CreateDirectory(AppPath.ActNumDir);
                File.Create(AppPath.ActNumFile).Close();

                return "";
            }
        }

        public void BlockExternalLocations(bool blockExternalLocations)
        {
            try
            {
                var settings = GetUserSettings();

                string writeSettings = JsonConvert.SerializeObject(settings);
                File.WriteAllText(AppPath.UserSettingsFile, writeSettings);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            } 
        }

        public UserSettings GetUserSettings()
        {
            var defaultSetting = new UserSettings()
            {
                DeleteBlankCompulinkOrders = false,
                AutoUpdate = true,
                AutoFillLearnedProducts = true,
                ProductMatcher = new Models.Users.ProductMatcher()
                {
                    CharSequenceMaxScore = 40,
                    SameWordMaxScore = 50,
                    SkuTypeMaxScore = 5,
                    QuantityMaxScore = 5
                }
            };

            string defaultSettings = JsonConvert.SerializeObject(defaultSetting);

            if (!Directory.Exists(AppPath.DataDir))
                Directory.CreateDirectory(AppPath.DataDir);

            // If there is no user settings file, create a new one using default parameters
            if (!File.Exists(AppPath.UserSettingsFile))
            {
                File.Create(AppPath.UserSettingsFile).Close();
                File.WriteAllText(AppPath.UserSettingsFile, defaultSettings);
            }

            return JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText($@"{AppPath.UserSettingsFile}"));
        }

        public void WriteToFiles(string ipConfig, string apiKey, string actNum)
        {
            try
            {
                // Write to ipConfig file
                if (!File.Exists(AppPath.IpConfigFile))
                {
                    Directory.CreateDirectory(AppPath.IpConfigDir);
                    File.Create(AppPath.IpConfigFile).Close();
                }

                File.WriteAllText(AppPath.IpConfigFile, ipConfig);

                // Write to apiKey file
                if (!File.Exists(AppPath.ApiKeyFile))
                {
                    Directory.CreateDirectory(AppPath.ApiKeyDir);
                    File.Create(AppPath.ApiKeyFile).Close();
                }

                File.WriteAllText(AppPath.ApiKeyFile, apiKey);

                // Write to actNum file
                if (!File.Exists(AppPath.ActNumFile))
                {
                    Directory.CreateDirectory(AppPath.ActNumDir);
                    File.Create(AppPath.ActNumFile).Close();
                }

                File.WriteAllText(AppPath.ActNumFile, actNum);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }
    }
}
