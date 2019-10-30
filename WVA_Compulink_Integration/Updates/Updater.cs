using WVA_Connect_CDI.Errors;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Utility.Files;

namespace WVA_Connect_CDI.Updates
{
    public class Updater
    {
        // This is called asychronously when the app is launched
        // Checks Github for any new releases. If there are any, it installs them
        public static async Task CheckForUpdates()
        {
            try
            {
                using (var mgr = UpdateManager.GitHubUpdateManager(AppPath.WisVisCdiRepo).Result)
                {
                    var updateInfo = mgr.CheckForUpdate().Result;
                    if (updateInfo.ReleasesToApply.Any())
                    {
                        await mgr.UpdateApp();
                    }
                }
            }
            catch (Exception e)
            {
                Error.Log(e.Message);
            }
        }

        // Checks Github for any new releases. Does not install anything if an update is available
        public static bool UpdatesAvailable()
        {
            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/WVA_Compulink_Desktop_Integration").Result)
            {
                var updateInfo = mgr.CheckForUpdate().Result;

                if (updateInfo.ReleasesToApply.Any())
                    return true;
                else
                    return false;
            }
        }
    }
}
