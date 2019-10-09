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
    class Updater
    {
        // This is called asychronously in IpConfigWindow, the first view that spawns for the application
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

        public static bool UpdatesAvailable()
        {
            using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/WVA_Compulink_Desktop_Integration").Result)
            {
                var updateInfo = mgr.CheckForUpdate().Result;

                if (updateInfo.ReleasesToApply.Any())
                    return true;
                else
                    return true;
            }
        }


    }
}
