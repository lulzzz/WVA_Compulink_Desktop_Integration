using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Models.Responses;
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

namespace WVA_Connect_CDI.ViewModels.Login
{
    public class ChangePasswordViewModel
    {
        // Gets the dsn from the DSN file
        public string GetDSN()
        {
            try
            {
                return File.ReadAllText(AppPath.IpConfigFile).Trim();
            }
            catch (FileNotFoundException)
            {
                if (!Directory.Exists(AppPath.IpConfigDir))
                    Directory.CreateDirectory(AppPath.IpConfigDir);

                if (!File.Exists(AppPath.IpConfigFile))
                    File.Create(AppPath.IpConfigFile);

                return "";
            }
        }

        // Checks if password has: at least 8 characters, a capital letter, a number
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

        // Requests a password change from the server
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

        // Creates a file noting when the user changed their password
        public void LogTimePassChanged()
        {
            if (!Directory.Exists(AppPath.TempDir))
                Directory.CreateDirectory(AppPath.TempDir);

            string timeNow = DateTime.Now.ToString();
            File.WriteAllText(AppPath.PrevTimePassChangeFile, timeNow);
        }
    }
}
