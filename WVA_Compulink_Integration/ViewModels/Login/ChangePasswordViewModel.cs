using WVA_Compulink_Desktop_Integration.WebTools;
using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Models.Responses;
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

namespace WVA_Compulink_Desktop_Integration.ViewModels.Login
{
    public class ChangePasswordViewModel
    {
        public string GetDSN()
        {
            try
            {
                return File.ReadAllText(Paths.IpConfigFile).Trim();
            }
            catch (FileNotFoundException)
            {
                if (!Directory.Exists(Paths.IpConfigDir))
                    Directory.CreateDirectory(Paths.IpConfigDir);

                if (!File.Exists(Paths.IpConfigFile))
                    File.Create(Paths.IpConfigFile);

                return "";
            }
        }

        public bool IsComplexPassword(string password)
        {
            try
            {
                // Password must be at least 8 characters
                if (password == null || password.Length < 8)
                    return false;

                bool hasCapitalLetter = false;
                bool hasNumber = false;

                foreach (char letter in password)
                {
                    // Check password for capital letters
                    if (char.IsUpper(letter) && char.IsLetter(letter))
                        hasCapitalLetter = true;

                    // Check password for numbers
                    if (char.IsNumber(letter))
                        hasNumber = true;
                }

                if (hasCapitalLetter && hasNumber)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return false;
            }
        }

        public Response ChangePassword(string DSN, string username, string password)
        {
            string endpoint = $"http://{DSN}/api/user/changePass";

            User changePassUser = new User()
            {
                UserName = username,
                Password = Crypto.ConvertToHash(password)
            };

            string strResponse = API.Post(endpoint, changePassUser);

            if (strResponse == null || strResponse.ToString().Trim() == "")
                throw new NullReferenceException("Response from endpoint was null or empty");
            else
                return JsonConvert.DeserializeObject<Response>(strResponse);
        }

        public void LogTimePassChanged()
        {
            if (!Directory.Exists(Paths.TempDir))
                Directory.CreateDirectory(Paths.TempDir);

            string timeNow = DateTime.Now.ToString();
            File.WriteAllText(Paths.PrevTimePassChangeFile, timeNow);
        }
    }
}
