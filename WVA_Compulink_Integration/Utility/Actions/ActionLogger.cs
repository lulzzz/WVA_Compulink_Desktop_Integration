using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Responses;
using WVA_Connect_CDI.Utility.Files;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Utility.Actions
{
    class ActionLogger
    {
        // 
        // LOGGING ACTIONS
        //

        // Accepts a function name (location of the action) and an action message to report
        public static void Log(string actionLocation, string actionMessage = null)
        {
            try
            {
                CreateLogFile();
                string file = GetLogFileName();

                string contents = actionMessage == null ? GetFileContents(actionLocation) : GetFileContents(actionLocation, actionMessage);

                WriteToLogFile(file, contents);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        // Creates a new action logger file with todays date if it does not exist 
        private static void CreateLogFile()
        {
            string file = GetLogFileName();

            if (!Directory.Exists(AppPath.TempDir))
                Directory.CreateDirectory(AppPath.TempDir);

            if (!File.Exists(file))
                File.Create(file).Close(); ;
        }

        // Returns a file name based on todays data
        private static string GetLogFileName()
        {
            return $"{AppPath.TempDir}CDI_Action_Log_{DateTime.Today.ToString("MM-dd-yy")}.txt";
        }

        // Returns a string containing data specific to this machine instance and app insallation
        private static string GetFileContents(string actionLocation)
        {
            try
            {
                string time = DateTime.Now.ToString("hh:mm:ss");
                return $"ApiKey={UserData.Data?.ApiKey} => MachName={Environment.MachineName} => EnvUserName={Environment.UserName} => AppUserName={UserData.Data?.UserName} => ActNum={UserData.Data?.Account} => {time} => {actionLocation}";
            }
            catch
            {
                return "";
            }
        }

        private static string GetFileContents(string actionLocation, string actionMessage)
        {
            return GetFileContents(actionLocation) + $" => {actionMessage}";
        }

        // Creates a log file if it doesn't exist, and appends the contents to it
        private static void WriteToLogFile(string file, string contents)
        {
            try
            {
                if (!File.Exists(file))
                    File.Create(file);

                var stream = File.AppendText(file);
                stream.WriteLine(contents);
                stream.Close();
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        // 
        // GETTING ACTION DATA 
        // 

        // Returns all action logger file data that is not labeled with todays date
        public static List<ActionData> GetDataNotToday()
        {
            try
            {
                List<ActionData> listActionData = new List<ActionData>();

                var files = Directory.EnumerateFiles(AppPath.TempDir, "CDI_Action_Log*").Where(x => !x.Contains(DateTime.Today.ToString("MM-dd-yy")));

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        string content = File.ReadAllText(file);
                        listActionData.Add(new ActionData(file, content));
                    }
                }

                return listActionData;
            }
            catch
            {
                return null;
            }
        }

        // Returns all action logger file data 
        public static List<ActionData> GetAllData()
        {
            try
            {
                List<ActionData> listActionData = new List<ActionData>();

                var files = Directory.EnumerateFiles(AppPath.TempDir, "CDI_Action_Log*");

                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        string content = File.ReadAllText(file);
                        listActionData.Add(new ActionData(file, content));
                    }
                }

                return listActionData;
            }
            catch
            {
                return null;
            }
        }

        // 
        //  SENDING ACTION DATA 
        // 

        // Sends all data to WVA and reports it to everyone on the email list
        public static bool ReportData(string data)
        {
            try
            {
                var dataMessage = new JsonError()
                {
                    ActNum = UserData.Data.Account,
                    Error = data.ToString(),
                    Application = Assembly.GetCallingAssembly().GetName().Name,
                    AppVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString()
                };

                string strResponse = API.Post(AppPath.WisVisErrors, dataMessage);
                var response = JsonConvert.DeserializeObject<Response>(strResponse);

                return response.Status == "SUCCESS" ? true : false;
            }
            catch
            {
                return false;
            }
        }

        // Gathers all action logger file data and reports it to WVA
        public static void ReportAllDataNow()
        {
            string data = "";
            var listActionData = GetAllData();

            if (listActionData == null) return;

            foreach (ActionData d in listActionData)
                data += d.Content;

            // Don't report if there is not data (user clicks button multiple times)
            if (data.Trim() != "")
                ReportData(data);
        }
    }
}
