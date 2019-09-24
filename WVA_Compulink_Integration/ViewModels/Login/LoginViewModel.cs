using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Models.Users;
using WVA_Connect_CDI.Security;
using WVA_Connect_CDI.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.MatchFinder;

namespace WVA_Connect_CDI.ViewModels.Login
{
    public class LoginViewModel
    {
        public void DelTimePassChangedFile()
        {
            try
            {
                if (File.Exists(AppPath.PrevTimePassChangeFile))
                {
                    DateTime passChangedDate = Convert.ToDateTime(File.ReadAllText(AppPath.PrevTimePassChangeFile));
                    DateTime deleteTime = DateTime.Now.AddDays(-1);

                    if (passChangedDate <= deleteTime)
                        File.Delete(AppPath.PrevTimePassChangeFile);
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex.ToString());
            }
        }

        public User LoginUser(string username, string password)
        {
            try
            {
                User user = new User()
                {
                    UserName = username,
                    Password = Crypto.ConvertToHash(password) ?? "",
                };

                string dsn = File.ReadAllText(AppPath.IpConfigFile).Trim();
                string endpoint = $"http://{dsn}/api/User/login";
                string loginResponse = API.Post(endpoint, user);
                User userLoginResponse = JsonConvert.DeserializeObject<User>(loginResponse);

                return userLoginResponse;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        public UserSettings GetUserSettings()
        {
            try
            {
                if (!Directory.Exists(AppPath.DataDir))
                    Directory.CreateDirectory(AppPath.DataDir);

                // If no settings file exists, create one with the default settings
                // If it does exist, check for missing properties and carry over currently saved settings
                if (!File.Exists(AppPath.UserSettingsFile))
                {
                    var defaultSetting = new UserSettings()
                    {
                        DeleteBlankCompulinkOrders = false,
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

                    File.Create(AppPath.UserSettingsFile).Close();
                    File.WriteAllText(AppPath.UserSettingsFile, defaultSettings);
                }
                else
                {
                    UpdateSettingsFile();
                }

                return JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText($@"{AppPath.UserSettingsFile}"));
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        // Update any properties that may be missing from the settings file
        private void UpdateSettingsFile()
        {
            try
            {
                // Get current users settings
                string settingsFileText = File.ReadAllText($@"{AppPath.UserSettingsFile}");
                UserSettings currentSettings = JsonConvert.DeserializeObject<UserSettings>(settingsFileText);
                var updateSettings = new UserSettings
                {
                    DeleteBlankCompulinkOrders = currentSettings.DeleteBlankCompulinkOrders,
                    AutoFillLearnedProducts = currentSettings.AutoFillLearnedProducts,
                    ProductMatcher = currentSettings.ProductMatcher
                };

                // If property doesn't exist in file, rewrite the file using their saved user settings
                if (!settingsFileText.Contains("DeleteBlankCompulinkOrders"))
                    OverWriteUserSettingsFile(updateSettings);
                else if (!settingsFileText.Contains("AutoFillLearnedProducts"))
                    OverWriteUserSettingsFile(updateSettings);
                else if (!settingsFileText.Contains("ProductMatcher"))
                    OverWriteUserSettingsFile(updateSettings);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        private void OverWriteUserSettingsFile(UserSettings settings)
        {
            File.WriteAllText(AppPath.UserSettingsFile, JsonConvert.SerializeObject(settings));
        }

        public bool PasswordChangedRecently()
        {
            if (File.Exists(AppPath.PrevTimePassChangeFile))
            {
                string fileText = File.ReadAllText(AppPath.PrevTimePassChangeFile);

                if (fileText == null || fileText.Trim() == "")
                    return false;
                else
                {
                    var timeChanged = Convert.ToDateTime(fileText);
                    var timeNow = DateTime.Now.AddMinutes(-15);

                    return timeNow > timeChanged ? false : true;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
