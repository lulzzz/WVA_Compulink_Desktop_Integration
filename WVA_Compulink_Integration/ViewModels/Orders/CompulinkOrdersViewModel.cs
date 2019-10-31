using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Patients;
using WVA_Connect_CDI.Models.Prescriptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.ViewModels.Orders
{
    public class CompulinkOrdersViewModel
    {
        public static List<Prescription> ListPrescriptions = new List<Prescription>();

        //
        // Constructors
        // 

        public CompulinkOrdersViewModel()
        {

        }

        public CompulinkOrdersViewModel(List<Prescription> prescriptions)
        {
            if (prescriptions == null)
                return;

            ListPrescriptions.AddRange(prescriptions);
        }


        // Determines if prescription items should be turned into a batch order 
        public bool IsBatchOrder(List<Prescription> listPrescriptions)
        {
            bool isBatchOrder;

            if (AreMultipleSTPs(listPrescriptions))
            {
                if (AddressesMatch(listPrescriptions))
                    isBatchOrder = false;
                else
                    isBatchOrder = true;
            }
            else if (IsMixedTypeOrder(listPrescriptions))
            {
                isBatchOrder = true;
            }
            else
            {
                if (AccountNumsMatch(listPrescriptions))
                    isBatchOrder = false;
                else
                    isBatchOrder = true;
            }

            return isBatchOrder;
        }

        // Returns true if any account numbers match 
        private bool AccountNumsMatch(List<Prescription> prescriptions)
        {
            // Null check   
            if (prescriptions == null || prescriptions.Count < 1)
                return false;

            var listDiffAccounts = new List<List<Prescription>>();

            // Checks if there are any other account numbers in the list that do not match the first index 
            listDiffAccounts.Add(prescriptions.Where(x => x.AccountNumber == prescriptions[0].AccountNumber).ToList());

            if (listDiffAccounts[0].Count == prescriptions.Count)
                return true;
            else
                return false;
        }

        // Returns true if any addresses match 
        private bool AddressesMatch(List<Prescription> prescriptions)
        {
            // Null check   
            if (prescriptions == null || prescriptions.Count < 1)
                return false;

            // Get patient information 
            var patients = new List<Patient>();

            foreach (Prescription p in prescriptions.Where(x => x.IsShipToPat))
            {
                patients.Add(GetPatientInfo(p._CustomerID.Value));
            }

            var listDiffAddresses = new List<List<Patient>>();

            listDiffAddresses.Add(patients.Where(x => x.Street != patients[0].Street).ToList());

            if (listDiffAddresses[0].Count > 0)
                return false;
            else
                return true;
        }

        // Returns true if there are more than one ship to patients 
        private bool AreMultipleSTPs(List<Prescription> prescriptions)
        {
            string origPatientName = "";
            bool areMultiplePatients = false;

            foreach (Prescription p in prescriptions.Where(x => x.IsShipToPat))
            {
                if (origPatientName == "")
                {
                    origPatientName = p.FirstName + p.LastName;
                    continue;
                }

                if (p.FirstName + p.LastName != origPatientName)
                    return true;
            }

            return areMultiplePatients;
        }

        // Returns true if there are STP and STO prescriptions 
        private bool IsMixedTypeOrder(List<Prescription> prescriptions)
        {
            int numStps = prescriptions.Where(x => x.IsShipToPat).Count();
            int numStos = prescriptions.Where(x => !x.IsShipToPat).Count();

            if (numStps > 0 && numStos > 0)
                return true;
            else
                return false;
        }
        
        // Gets detailed patient information based on given patient id
        public static Patient GetPatientInfo(string patientID)
        {
            string dsn = UserData.Data?.DSN;

            if (patientID == null || string.IsNullOrWhiteSpace(patientID))
                throw new Exception("patientID cannot be null or blank");

            string endpoint = $"http://{dsn}/api/patient/" + $"{patientID}";
            string strResponse = API.Get(endpoint, out string httpStatus);
            return JsonConvert.DeserializeObject<Patient>(strResponse);
        }
    }
}
