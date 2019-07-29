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

namespace WVA_Connect_CDI.Errors
{
    class Error
    {
        public static void ReportOrLog(Exception e)
        {
            JsonError error = new JsonError()
            {
                ActNum = UserData.Data?.Account,
                Error = e.ToString(),
                Application = Assembly.GetCallingAssembly().GetName().Name,
                AppVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString(),
                UserName = Environment.UserName,
                MachineName = Environment.MachineName
            };

            if (!ErrorReported(error))
                Log(error.Error);
        }

        private static bool ErrorReported(JsonError error)
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

        public static void Log(string exceptionMessage)
        {
            try
            {
                string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

                if (!Directory.Exists(Paths.ErrorLogDir))
                {
                    Directory.CreateDirectory(Paths.ErrorLogDir);
                }

                if (!File.Exists(Paths.ErrorLogDir + $@"\Error_{time}.txt"))
                {
                    var file = File.Create(Paths.ErrorLogDir + $@"\Error_{time}.txt");
                    file.Close();
                }

                using (StreamWriter writer = new StreamWriter((Paths.ErrorLogDir + $@"\Error_{time}.txt"), true))
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
    }
}
