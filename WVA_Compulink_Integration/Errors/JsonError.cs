using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Errors
{
    class JsonError
    {
        public string ActNum { get; set; }      // WVA account number
        public string Error { get; set; }       // A ToString()'ed exception object or exception message
        public string Application { get; set; } // Assembly version of the application
        public string AppVersion { get; set; }  // Assembly version of the application
        public string UserName { get; set; }    // Environment user name
        public string MachineName { get; set; } // Environment machine name
    }
}
