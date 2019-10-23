using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.MatchFinder;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Orders.Out;
using WVA_Connect_CDI.Models.Patients;
using WVA_Connect_CDI.Models.Prescriptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.Models.Products;

namespace WVA_Connect_CDI.ViewModels.Orders
{
    class BatchOrderCreationViewModel : OrderCreationViewModel
    {
        public static int MatchScoreTargetValue { get; set; }
        public static List<string> OrderNames = new List<string>();
        public static List<List<Prescription>> BatchPrescriptions = new List<List<Prescription>>();
        public static List<List<MatchProduct>> BatchPrescriptionMatches = new List<List<MatchProduct>>();

        public BatchOrderCreationViewModel()
        {

        }

        public BatchOrderCreationViewModel(string orderName, List<Prescription> listPrescriptions)
        {
            // Get split out orders and add to 2D batch order list
            var splitPrescriptions = SplitPrescriptions(listPrescriptions);
            BatchPrescriptions.AddRange(splitPrescriptions);

            // Get list of order names for the split orders
            var batchOrderNames = GetOrderNames(orderName, BatchPrescriptions.Count);
            OrderNames.AddRange(batchOrderNames);
        }

        private List<List<Prescription>> SplitPrescriptions(List<Prescription> prescriptions)
        {
            if (prescriptions == null || prescriptions.Count < 1)
                return null;

            var splitOrders = new List<List<Prescription>>();

            var stp_Prescriptions = prescriptions.Where(x => x.IsShipToPat).ToList();
            var sto_Prescriptions = prescriptions.Where(x => !x.IsShipToPat).ToList();

            // Set adresses
            foreach (Prescription p in stp_Prescriptions)
            {
                p.Address = GetPatientAddress(p._CustomerID.Value);
            }

            // Split out STPs by address
            while (stp_Prescriptions.Count > 0)
            {
                string address = stp_Prescriptions[0].Address;
                string actNum = stp_Prescriptions[0].AccountNumber; 
                splitOrders.Add(stp_Prescriptions.Where(x => x.Address == address && x.AccountNumber == actNum).ToList());
                stp_Prescriptions.RemoveAll(x => x.Address == address && x.AccountNumber == actNum);
            }

            // Split out STOs by account number
            while (sto_Prescriptions.Count > 0)
            {
                string acctNum = sto_Prescriptions[0].AccountNumber;
                splitOrders.Add(sto_Prescriptions.Where(x => x.AccountNumber == acctNum).ToList());
                sto_Prescriptions.RemoveAll(x => x.AccountNumber == acctNum);
            }

            return splitOrders;
        }

        private List<string> GetOrderNames(string orderName, int numSplitPrescriptions)
        {
            var orderNames = new List<string>();

            for (int i = 0; i < numSplitPrescriptions; i++)
            {
                orderNames.Add(orderName + " (" + (i + 1) + ")");
            }

            return orderNames;
        }

        private string GetPatientAddress(string patientID)
        {
            string dsn = UserData.Data?.DSN;

            if (patientID == null || string.IsNullOrWhiteSpace(patientID))
                throw new Exception("patientID cannot be null or blank");

            string endpoint = $"http://{dsn}/api/patient/" + $"{patientID}";
            string strResponse = API.Get(endpoint, out string httpStatus);
            var patient = JsonConvert.DeserializeObject<Patient>(strResponse);

            return patient.Street;
        }

    }
}
