using WVA_Connect_CDI.WebTools;
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

namespace WVA_Connect_CDI.ViewModels.Registration
{
    public class RegistrationViewModel
    {
        // Does a rought check to see if string is in an email format
        public bool IsValidEmail(string email)
        {
            try
            {
                // Check if string is in an email-like format
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Checks that string has: at least 8 characters, a capital letter, a number
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
                Errors.Error.ReportOrLog(ex);
                return false;
            }
        }

        // Creats a new user in the server database
        public User RegisterUser(string username, string password, string email)
        {
            // Post API response check
            User user = new User()
            {
                UserName = username,
                Password = Crypto.ConvertToHash(password),
                Email = email,
                ApiKey = ""
            };

            string dsn = File.ReadAllText(AppPath.IpConfigFile).Trim();
            string endpoint = $"http://{dsn}/api/User/register";
            string registerResponse = API.Post(endpoint, user);
            User userRegisterResponse = JsonConvert.DeserializeObject<User>(registerResponse);

            return userRegisterResponse;
        }

    }
}
