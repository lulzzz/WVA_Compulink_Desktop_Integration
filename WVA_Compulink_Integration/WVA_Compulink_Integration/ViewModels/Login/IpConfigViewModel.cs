using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Utility.Files;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Compulink_Desktop_Integration.Models.Users;
using Newtonsoft.Json;

namespace WVA_Compulink_Desktop_Integration.ViewModels.Login
{
    public class IpConfigViewModel
    {
        public string GetIpConfig()
        {
            try
            {
                return File.ReadAllText(Paths.IpConfigFile);
            }
            catch 
            {
                Directory.CreateDirectory(Paths.IpConfigDir);
                File.Create(Paths.IpConfigFile).Close();

                return "";
            }
        }

        public string GetApiKey()
        {
            try
            {
                return File.ReadAllText(Paths.ApiKeyFile);
            }
            catch
            {
                Directory.CreateDirectory(Paths.ApiKeyDir);
                File.Create(Paths.ApiKeyFile).Close();

                return "";
            }
        }

        public string GetActNum()
        {
            try
            {
                return File.ReadAllText(Paths.ActNumFile);
            }
            catch 
            {
                Directory.CreateDirectory(Paths.ActNumDir);
                File.Create(Paths.ActNumFile).Close();

                return "";
            }
        }

        public void BlockExternalLocations(bool blockExternalLocations)
        {
            try
            {
                var settings = GetUserSettings();

                settings.BlockExternalLocations = blockExternalLocations;

                string writeSettings = JsonConvert.SerializeObject(settings);
                File.WriteAllText(Paths.UserSettingsFile, writeSettings);
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
                BlockExternalLocations = false,
                ProductMatcher = new Models.Users.ProductMatcher()
                {
                    CharSequenceMaxScore = 40,
                    SameWordMaxScore = 50,
                    SkuTypeMaxScore = 5,
                    QuantityMaxScore = 5
                }
            };

            string defaultSettings = JsonConvert.SerializeObject(defaultSetting);

            if (!Directory.Exists(Paths.DataDir))
                Directory.CreateDirectory(Paths.DataDir);

            // If there is no user settings file, create a new one using default parameters
            if (!File.Exists(Paths.UserSettingsFile))
            {
                File.Create(Paths.UserSettingsFile).Close();
                File.WriteAllText(Paths.UserSettingsFile, defaultSettings);
            }

            return JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText($@"{Paths.UserSettingsFile}"));
        }

        public void WriteToFiles(string ipConfig, string apiKey, string actNum)
        {
            try
            {
                // Write to ipConfig file
                if (!File.Exists(Paths.IpConfigFile))
                {
                    Directory.CreateDirectory(Paths.IpConfigDir);
                    File.Create(Paths.IpConfigFile).Close();
                }

                File.WriteAllText(Paths.IpConfigFile, ipConfig);

                // Write to apiKey file
                if (!File.Exists(Paths.ApiKeyFile))
                {
                    Directory.CreateDirectory(Paths.ApiKeyDir);
                    File.Create(Paths.ApiKeyFile).Close();
                }

                File.WriteAllText(Paths.ApiKeyFile, apiKey);

                // Write to actNum file
                if (!File.Exists(Paths.ActNumFile))
                {
                    Directory.CreateDirectory(Paths.ActNumDir);
                    File.Create(Paths.ActNumFile).Close();
                }

                File.WriteAllText(Paths.ActNumFile, actNum);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }
    }
}
