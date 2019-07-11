using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.Errors
{
    class JsonError
    {
        public string ActNum { get; set; }
        public string Error { get; set; }
        public string Application { get; set; }
        public string AppVersion { get; set; }
        public string UserName { get; set; }
        public string MachineName { get; set; }
    }
}
