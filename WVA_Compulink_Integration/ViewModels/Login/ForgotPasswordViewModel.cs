using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Models.Responses;
using WVA_Connect_CDI.Models.Users;
using WVA_Connect_CDI.Models.Validations.Emails;
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
    public class ForgotPasswordViewModel
    {
        // Gets the apiKey from the ApiKey file
        public string GetApiKey()
        {
            try
            {
                return File.ReadAllText(AppPath.ApiKeyFile).Trim();
            }
            catch
            {
                if (!Directory.Exists(AppPath.ApiKeyDir))
                    Directory.CreateDirectory(AppPath.ApiKeyDir);

                if (!File.Exists(AppPath.ApiKeyFile))
                    File.Create(AppPath.ApiKeyFile);

                return "";
            }
        }

        // Gets the dsn from the DSN file
        public string GetDSN()
        {
            try
            {
                return File.ReadAllText(AppPath.IpConfigFile).Trim();
            }
            catch
            {
                if (!Directory.Exists(AppPath.IpConfigDir))
                    Directory.CreateDirectory(AppPath.IpConfigDir);

                if (!File.Exists(AppPath.IpConfigFile))
                    File.Create(AppPath.IpConfigFile);

                return "";
            }
        }

        // Gets the user's email from the server
        public User GetUserEmail(string username, string DSN)
        {
            try
            {
                string getEmailEndpoint = $"http://{DSN}/api/User/GetEmail";
                User user = new User()
                {
                    UserName = username
                };

                string strEmail = API.Post(getEmailEndpoint, user);

                if (strEmail == null || strEmail.ToString().Trim() == "")
                    throw new Exception("Null or blank response from endpoint.");

                return JsonConvert.DeserializeObject<User>(strEmail);
            }
            catch
            {
                return null;
            }
        }

        // // Checks to see if user entered in the right password reset code 
        public Response ResetEmailCheck(string DSN, string userEmail, string emailCode, string apiKey)
        {
            try
            {
                string endpoint = $"http://{DSN}/api/user/reset-email-check";
                EmailValidationCode emailValidation = new EmailValidationCode()
                {
                    Email = userEmail,
                    EmailCode = emailCode,
                    ApiKey = apiKey
                };

                string strResponse = API.Post(endpoint, emailValidation);
                return JsonConvert.DeserializeObject<Response>(strResponse);
            }
            catch
            {
                return null;
            }
        }

        // Attempts to send an email to the user with a code to reset their password
        public Response SendEmail(string DSN, string userEmail, string apiKey)
        {
            try
            {
                string endpoint = $"http://{DSN}/api/user/reset-email";

                EmailValidationSend emailValidation = new EmailValidationSend()
                {
                    Email = userEmail,
                    ApiKey = apiKey
                };

                string strResponse = API.Post(endpoint, emailValidation);
                return JsonConvert.DeserializeObject<Response>(strResponse);
            }
            catch
            {
                return null;
            }
        }
    }
}
