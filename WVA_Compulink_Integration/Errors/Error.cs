using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Utility.Actions;

namespace WVA_Connect_CDI.Errors
{
    public class Error
    {
        // Attempts to report an error. If it fails, it will log the error on this machine. 
        // This function also reports all action data to better determine what happened leading up to the exception
        public static void ReportOrLog(Exception e)
        {
            try
            {
                var error = GetErrorObject(e);

                if (!Report(error))
                    Log(error.Error);
            }
            finally
            {
                ActionLogger.ReportAllDataNow();
            }
        }

        // Creates and error log directory and file if it doesn't exist, then writes to the file 
        public static void Log(string exceptionMessage)
        {
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                if (!Directory.Exists(AppPath.ErrorLogDir))
                    Directory.CreateDirectory(AppPath.ErrorLogDir);

                if (!File.Exists(AppPath.ErrorLogDir + $@"\Error_{time}.txt"))
                {
                    var file = File.Create(AppPath.ErrorLogDir + $@"\Error_{time}.txt");
                    file.Close();
                }

                using (var writer = new StreamWriter(AppPath.ErrorLogDir + $@"\Error_{time}.txt", true))
                {
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                    writer.WriteLine("");
                    writer.WriteLine($"(ERROR.TIME_ENCOUNTERED: {time})");
                    writer.WriteLine($"(ERROR.MESSAGE: {exceptionMessage})");
                    writer.WriteLine("");
                    writer.WriteLine("-----------------------------------------------------------------------------------");
                    writer.Close();
                }
            }
            catch { }
        }

        // Builds an error object using a given exception object
        private static JsonError GetErrorObject(Exception e)
        {
            return new JsonError()
            {
                ActNum = UserData.Data?.Account,
                Error = e.ToString(),
                Application = Assembly.GetCallingAssembly().GetName().Name,
                AppVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString(),
                UserName = Environment.UserName,
                MachineName = Environment.MachineName
            };
        }

        // Attempts to report error the error to WVA. Returns false if it fails
        private static bool Report(JsonError error)
        {
            try
            {
                string dsn = UserData.Data?.DSN;

                if (dsn == null || dsn.Trim() == "")
                    return false;

                string endpoint = $"http://{dsn}/api/error/";
                string strResponse = API.Post(endpoint, error);
                bool messageSent = JsonConvert.DeserializeObject<bool>(strResponse);

                return messageSent;
            }
            catch
            {
                return false;
            }
        }

        
    }
}
