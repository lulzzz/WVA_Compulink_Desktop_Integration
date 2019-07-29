using WVA_Connect_CDI.Errors;
using Squirrel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Updates
{
    class Updater
    {
        public static async Task CheckForUpdates()
        {
            try
            {
                using (var mgr = UpdateManager.GitHubUpdateManager("https://github.com/WVATeam/WVA_Connect_CDI").Result)
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
