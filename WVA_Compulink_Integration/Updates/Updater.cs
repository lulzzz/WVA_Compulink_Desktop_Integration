using WVA_Compulink_Desktop_Integration.Errors;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Updates
{
    class Updater
    {
        public static async Task CheckForUpdates()
        {
            try
            {
                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/Compulink_WVA_Compulink_Desktop_Integration").Result)
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
    }
}
