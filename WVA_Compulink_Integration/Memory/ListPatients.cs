using WVA_Connect_CDI.Models.Patients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.Memory
{
    class ListPatients
    {
        public static List<Patient> OrigListPatients = new List<Patient>() { };
        public static List<Patient> CopyListPatients = new List<Patient>() { };
    }
}
