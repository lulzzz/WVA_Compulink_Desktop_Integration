using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.ProductMatcher;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.ProductMatcher.Models;
using WVA_Connect_CDI.ProductMatcher.Data;

namespace WVA_Connect_CDI.ProductPredictions
{
    class ProductPrediction
    {
        public static double MatchScore { get; set; }

        public static List<MatchedProduct> GetPredictionMatches(Prescription prescription, double matchScore, bool limitReturnedResults = false)
        {
            // Check for nulls
            if (prescription == null || prescription.Product.Trim() == "")
                throw new Exception("string 'compulinkProduct' cannot be null or blank.");

            if (WvaProducts.ListProducts == null || WvaProducts.ListProducts?.Count < 1)
                throw new Exception("List of WVA products cannot be null or empty.");        

            // Set the match score
            MatchScore = matchScore;

            // Trim whitespace off of product name
            prescription.Product = prescription.Product.Trim();

            // If 'product' is a stored wva match product, leave method. There is no reason to find any matches for it.
            MatchedProduct matchProduct = WvaProductExists(prescription.Product);

            if (matchProduct != null)
            {
                return new List<MatchedProduct>() { matchProduct };
            }
            else
            {
                //Get wva products that are similarly matched with the compulink product
                var listMatches = GetMatches(prescription, limitReturnedResults);

                return listMatches;
            }
        }

        public static bool ProductChangeEnabled(string compulinkProduct)
        {
            var product = Database.GetLearnedProduct(compulinkProduct);

            if (product == null)
                return true;
            else if (product.ChangeEnabled)
                return true;
            else
                return false;
        }

        public static void LearnProduct(string compulinkProduct, string wvaProduct)
        {
            // Check for nulls
            if (compulinkProduct == null || compulinkProduct.Trim() == "")
                return;

            if (wvaProduct == null || wvaProduct.Trim() == "")
                throw new Exception("List of WVA products cannot be null or empty.");

            // Clean up any whitespace around the product
            compulinkProduct = compulinkProduct.Trim();

            // Increase number of times this product has been picked or create a new object if it has not been used already
            if (Database.ProductMatchExists(compulinkProduct: compulinkProduct, wvaProduct: wvaProduct))
            {
                Database.IncrementNumPicks(compulinkProduct);
            }
            else if (Database.CompulinkProductExists(compulinkProduct) && Database.ReturnWvaProductFor(compulinkProduct) != wvaProduct)
            {
                int numPicks = Database.GetNumPicks(compulinkProduct);

                if (numPicks > 1)
                    Database.DecrementNumPicks(compulinkProduct);
                else
                    Database.UpdateCompulinkProductMatch(compulinkProduct, wvaProduct);
            }
            else
            {
                Database.CreateCompulinkProduct(compulinkProduct, wvaProduct);
            }
        }

        public static MatchedProduct WvaProductExists(string product)
        {
            Product wvaProduct = WvaProducts.ListProducts.Where(x => x.Description == product).FirstOrDefault();

            if (wvaProduct != null)
                return new MatchedProduct(wvaProduct?.Description, 100) { ProductCode = wvaProduct.ProductKey };
            else
                return null;
        }

        // Get a list of wva product matches for a given compulink product
        private static List<MatchedProduct> GetMatches(Prescription prescription, bool limitReturnedResults)
        {
            var listMatches = new List<MatchedProduct>();

            // If overrideNumPicks is true, the method will not limit the list based on numPicks
            int numPicks;
            if (limitReturnedResults)
                numPicks = 0;
            else
                numPicks = Database.GetNumPicks(prescription.Product);

            // If 10 or more numPicks only show suggested product (confidence: extremely confident)
            if (numPicks >= 7)
            {
                MatchedProduct matchProduct;
                matchProduct = WvaProductExists(Database.ReturnWvaProductFor(prescription.Product));

                listMatches.Add(matchProduct);
                return listMatches;
            }
            // If 5 numPicks show suggested product and 3 matches (high confidence)
            else if (numPicks >= 5)
            {
                listMatches = FilterList(3, prescription, new MatchedProduct(productName: Database.ReturnWvaProductFor(prescription.Product), matchScore: 100));
                return listMatches;
            }
            // If 3 numPicks show suggested product and 10 matches (medium confidence)
            else if (numPicks >= 3)
            {
                listMatches = FilterList(10, prescription, new MatchedProduct(productName: Database.ReturnWvaProductFor(prescription.Product), matchScore: 100));
                return listMatches;
            }
            // If 1 numPicks show suggested product and all matches (low confidence)
            else if (numPicks >= 1)
            {
                listMatches = FilterList(999, prescription, new MatchedProduct(productName: Database.ReturnWvaProductFor(prescription.Product), matchScore: 100));
                return listMatches;
            }
            // If 0 numPicks show all matches (no confidence)
            else
            {
                listMatches = DescriptionMatcher.FindMatches(prescription, MatchScore);
                return listMatches;
            }
        }

        private static List<MatchedProduct> FilterList(int countLimit, Prescription prescription, MatchedProduct suggestedProduct = null)
        {
            var listMatches = new List<MatchedProduct>();

            if (suggestedProduct != null)
            {
                string wvaProd = Database.ReturnWvaProductFor(prescription.Product);
                listMatches.Add(new MatchedProduct(productName: wvaProd, matchScore: 100));

                // Set the product key
                listMatches[0].ProductCode = WvaProducts.ListProducts.Where(x => x.Description == wvaProd).Select(x => x.ProductKey).First();
            }

            if (WvaProducts.ListProducts != null)
                listMatches.AddRange(DescriptionMatcher.FindMatches(prescription, MatchScore));

            // Remove match product in list that is the same as the suggested product
            if (listMatches.Count > 1)
                listMatches = listMatches.GroupBy(x => x.ProductName).Select(x => x.First()).ToList();

            // Get only top x matches
            for (int i = listMatches.Count; i > countLimit; i--)
                listMatches.RemoveAt(i - 1);

            return listMatches;
        }
    }
}
