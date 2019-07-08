using WVA_Compulink_Desktop_Integration.WebTools;
using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Models.Users;
using WVA_Compulink_Desktop_Integration.Security;
using WVA_Compulink_Desktop_Integration.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Compulink_Desktop_Integration.MatchFinder;

namespace WVA_Compulink_Desktop_Integration.ViewModels.Login
{
    public class LoginViewModel
    {
        public void DelTimePassChangedFile()
        {
            try
            {
                if (File.Exists(Paths.PrevTimePassChangeFile))
                {
                    DateTime passChangedDate = Convert.ToDateTime(File.ReadAllText(Paths.PrevTimePassChangeFile));
                    DateTime deleteTime = DateTime.Now.AddDays(-1);

                    if (passChangedDate <= deleteTime)
                        File.Delete(Paths.PrevTimePassChangeFile);
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
                    Password = Crypto.ConvertToHash(password),
                };

                string dsn = File.ReadAllText(Paths.IpConfigFile).Trim();
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
                var defaultSetting = new UserSettings()
                {
                    DeleteBlankCompulinkOrders = false,
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

                if (!File.Exists(Paths.UserSettingsFile))
                {
                    File.Create(Paths.UserSettingsFile).Close();
                    File.WriteAllText(Paths.UserSettingsFile, defaultSettings);
                }

                return JsonConvert.DeserializeObject<UserSettings>(File.ReadAllText($@"{Paths.UserSettingsFile}"));
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        public bool PasswordChangedRecently()
        {
            if (File.Exists(Paths.PrevTimePassChangeFile))
            {
                string fileText = File.ReadAllText(Paths.PrevTimePassChangeFile);

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
