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
        public string GetApiKey()
        {
            try
            {
                return File.ReadAllText(Paths.ApiKeyFile).Trim();
            }
            catch
            {
                if (!Directory.Exists(Paths.ApiKeyDir))
                    Directory.CreateDirectory(Paths.ApiKeyDir);

                if (!File.Exists(Paths.ApiKeyFile))
                    File.Create(Paths.ApiKeyFile);

                return "";
            }
        }

        public string GetDSN()
        {
            try
            {
                return File.ReadAllText(Paths.IpConfigFile).Trim();
            }
            catch
            {
                if (!Directory.Exists(Paths.IpConfigDir))
                    Directory.CreateDirectory(Paths.IpConfigDir);

                if (!File.Exists(Paths.IpConfigFile))
                    File.Create(Paths.IpConfigFile);

                return "";
            }
        }

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

        public Response ResetEmail(string DSN, string userEmail, string emailCode, string apiKey)
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
