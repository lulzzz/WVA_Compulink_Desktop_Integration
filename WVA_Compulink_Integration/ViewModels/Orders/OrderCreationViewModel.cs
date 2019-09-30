using WVA_Connect_CDI.WebTools;
using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.MatchFinder;
using WVA_Connect_CDI.MatchFinder.ProductPredictions;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Orders.Out;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Models.Responses;
using WVA_Connect_CDI.Models.Validations;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WVA_Connect_CDI.Utility.UI_Tools;
using WVA_Connect_CDI.Utility.Files;

namespace WVA_Connect_CDI.ViewModels.Orders
{
    class OrderCreationViewModel
    {
        public List<List<MatchProduct>> ListMatchedProducts = new List<List<MatchProduct>>();

        // <note> These MUST remain static! They are accessed from other classes to properly load this view
        public static Order Order { get; set; }
        public static List<Prescription> Prescriptions { get; set; }
        public static string OrderName { get; set; }
        // </note>

        public OrderCreationViewModel()
        {
            
        }

        public OrderCreationViewModel(List<Prescription> listPrescriptions, string orderName)
        {
            try
            {
                Order = null;
                OrderName = orderName;

                // Deletes any orders with a blank product name if user has this setting enabled
                if (UserData.Data?.Settings != null && UserData.Data.Settings.DeleteBlankCompulinkOrders)
                {
                    listPrescriptions.RemoveAll(p => p.Product == null);
                    listPrescriptions.RemoveAll(p => p.Product.Trim() == "");
                }
                else
                {
                    for (int i = 0; i < listPrescriptions.Count; i++)
                        if (listPrescriptions[i].Product == null || listPrescriptions[i].Product.Trim() == "")
                            listPrescriptions[i] = listPrescriptions.Where(x => x.FirstName == listPrescriptions[i].FirstName && x.LastName == listPrescriptions[i].LastName).First();
                }

                Prescriptions = listPrescriptions;
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
            }
        }

        public OrderCreationViewModel(Order order, string orderName)
        {
            Order = order;
            OrderName = orderName;
        }

        public OrderCreationViewModel(Order order, List<Prescription> listPrescriptions, string orderName)
        {
            Order = order;
            Prescriptions = listPrescriptions;
            OrderName = orderName;
        }

        public Order GetOrder(string orderName)
        {
            string dsn = UserData.Data.DSN;
            string endpoint = $"http://{dsn}/api/order/exists/";
            string strOrder = API.Post(endpoint, orderName);
            var order = JsonConvert.DeserializeObject<Order>(strOrder);

            // Change shipping code into a readable string value
            if (order != null)
                order.ShippingMethod = ShippingTools.GetShippingString(order.ShippingMethod);

            return order;
        }

        public List<Order> GetOrders(string account)
        {
            string dsn = UserData.Data.DSN;
            string endpoint = $"http://{dsn}/api/order/get-orders/{account}";
            string strOrders = API.Get(endpoint, out string httpStatus);

            if (strOrders == null || strOrders == "")
                throw new NullReferenceException();

            var listOrders = JsonConvert.DeserializeObject<List<Order>>(strOrders);

            return listOrders;
        }

        public OrderResponse CreateOrder(OutOrderWrapper outOrderWrapper)
        {
            string dsn = UserData.Data.DSN;
            string endpoint = $"http://{dsn}/api/order/submit/";
            string strResponse = API.Post(endpoint, outOrderWrapper);
            OrderResponse response = JsonConvert.DeserializeObject<OrderResponse>(strResponse);

            return response;
        }

        public OrderResponse DeleteOrder(string orderName)
        {
            string dsn = UserData.Data.DSN;
            string endpoint = $"http://{dsn}/api/order/delete/";
            string strResponse = API.Post(endpoint, orderName);
            OrderResponse response = JsonConvert.DeserializeObject<OrderResponse>(strResponse);

            return response;
        }

        public OrderResponse SaveOrder(OutOrderWrapper outOrderWrapper)
        {
            if (outOrderWrapper?.OutOrder?.PatientOrder?.OrderName == null || outOrderWrapper?.OutOrder?.PatientOrder?.OrderName.Trim() == "")
                return null;

            string dsn = UserData.Data.DSN;
            string endpoint = $"http://{dsn}/api/order/save/";
            string strResponse = API.Post(endpoint, outOrderWrapper);
            OrderResponse response = JsonConvert.DeserializeObject<OrderResponse>(strResponse);

            return response;
        }

        public ValidationResponse ValidateOrder(ValidationWrapper validationWrapper)
        {
            string endpoint = AppPath.WisVisValidations;
            string strValidatedProducts = API.Post(endpoint, validationWrapper);
            return JsonConvert.DeserializeObject<ValidationResponse>(strValidatedProducts);
        }

        public bool OrderExists(string orderName)
        {
            // Make sure they do not add a batch order to an existing order
            var existingOrder = GetOrder(orderName);

            if (existingOrder == null)
                return false;
            else
                return true;
        }

        public void FindProductMatches(List<Prescription> prescriptions, double matchScore = 30, bool limitReturnedResults = false)
        {
            // Reset list of matched products 
            ListMatchedProducts.Clear();

            // Loop through product names list and pass each one into matcher algorithm 
            int index = 0;

            foreach (Prescription prescription in prescriptions)
            {
                try
                {
                    // Trim exta white space off of product name
                    prescription.Product = prescription.Product.Trim();

                    // Run match finder for product and return results based on numPicks (number of times same product has been chosen)
                    var matchProducts = ProductPrediction.GetPredictionMatches(prescription, matchScore, WvaProducts.ListProducts, limitReturnedResults);

                    if (matchProducts?.Count > 0)
                    {
                        ListMatchedProducts.Add(matchProducts);
                        Prescriptions[index].ProductCode = matchProducts[0].ProductKey;
                    }
                    else
                        ListMatchedProducts.Add(new List<MatchProduct> { new MatchProduct("No Matches Found", 0) });
                }
                finally
                {
                    index++;
                }
            }
        }

        // Purpose of this overload is so product can be changed before it's commited from cell edit event
        public void FindProductMatches(List<Prescription> prescriptions, string product, int rowToUpdate, double matchScore = 30, bool overrideNumPicks = false)
        {
            if (string.IsNullOrWhiteSpace(product))
                return;

            // Reset list of matched products 
            ListMatchedProducts.Clear();

            // Update prescription object with new product
            prescriptions[rowToUpdate].Product = product;

            // Loop through product names list and pass each one into matcher algorithm 
            int index = 0;

            foreach (Prescription prescription in prescriptions)
            {
                try
                {
                    // Trim exta white space off of product name
                    prescription.Product = prescription.Product.Trim();

                    // Run match finder for product and return results based on numPicks (number of times same product has been chosen)
                    var matchProducts = ProductPrediction.GetPredictionMatches(prescription, matchScore, WvaProducts.ListProducts, overrideNumPicks);

                    if (matchProducts?.Count > 0)
                    {
                        ListMatchedProducts.Add(matchProducts);
                        Prescriptions[index].ProductCode = matchProducts[0].ProductKey;
                    }
                    else
                        ListMatchedProducts.Add(new List<MatchProduct> { new MatchProduct("No Matches Found", 0) });
                }
                finally
                {
                    index++;
                }
            }
        }
    }
}
