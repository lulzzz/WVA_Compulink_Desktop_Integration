using WVA_Connect_CDI.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Memory
{
    public class UserData
    {
        // Contains user login data that will be accessed throughout the application 
        public static User Data { get; set; }
    }
}
