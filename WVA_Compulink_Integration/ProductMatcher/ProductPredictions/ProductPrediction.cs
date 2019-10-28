using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WVA_Connect_CDI.MatchFinder.ProductPredictions
{
    class ProductPrediction
    {
        public static double MatchScore { get; set; }

        public static List<MatchProduct> GetPredictionMatches(Prescription prescription, double matchScore, List<Product> wvaProducts, bool limitReturnedResults = false)
        {
            // Check for nulls
            if (prescription == null || prescription.Product.Trim() == "")
                throw new Exception("string 'compulinkProduct' cannot be null or blank.");

            if (wvaProducts == null || wvaProducts?.Count < 1)
                throw new Exception("List of WVA products cannot be null or empty.");        

            // Set the match score
            MatchScore = matchScore;

            // Trim whitespace off of product name
            prescription.Product = prescription.Product.Trim();

            // If 'product' is a stored wva match product, leave method. There is no reason to find any matches for it.
            MatchProduct matchProduct = WvaProductExists(prescription.Product, wvaProducts);

            if (matchProduct != null)
            {
                return new List<MatchProduct>() { matchProduct };
            }
            else
            {
                //Get wva products that are similarly matched with the compulink product
                var listMatches = GetMatches(prescription, wvaProducts, limitReturnedResults);

                return listMatches;
            }
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

        public static MatchProduct WvaProductExists(string product, List<Product> wvaProducts)
        {
            Product wvaProduct = wvaProducts.Where(x => x.Description == product).FirstOrDefault();

            if (wvaProduct != null)
                return new MatchProduct(wvaProduct?.Description, 100) { ProductKey = wvaProduct.ProductKey };
            else
                return null;
        }

        // Get a list of wva product matches for a given compulink product
        private static List<MatchProduct> GetMatches(Prescription prescription, List<Product> wvaProducts, bool limitReturnedResults)
        {
            var listMatches = new List<MatchProduct>();

            // If overrideNumPicks is true, the method will not limit the list based on numPicks
            int numPicks;
            if (limitReturnedResults)
                numPicks = 0;
            else
                numPicks = Database.GetNumPicks(prescription.Product);

            // If 10 or more numPicks only show suggested product (confidence: extremely confident)
            if (numPicks >= 7)
            {
                MatchProduct matchProduct;
                matchProduct = WvaProductExists(Database.ReturnWvaProductFor(prescription.Product), wvaProducts);

                listMatches.Add(matchProduct);
                return listMatches;
            }
            // If 5 numPicks show suggested product and 4 matches (high confidence)
            else if (numPicks >= 5)
            {
                listMatches = FilterList(3, prescription, wvaProducts, new MatchProduct(name: Database.ReturnWvaProductFor(prescription.Product), matchScore: 100));
                return listMatches;
            }
            // If 3 numPicks show suggested product and 14 matches (medium confidence)
            else if (numPicks >= 3)
            {
                listMatches = FilterList(10, prescription, wvaProducts, new MatchProduct(name: Database.ReturnWvaProductFor(prescription.Product), matchScore: 100));
                return listMatches;
            }
            // If 1 numPicks show suggested product and all matches (low confidence)
            else if (numPicks >= 1)
            {
                listMatches = FilterList(999, prescription, wvaProducts, new MatchProduct(name: Database.ReturnWvaProductFor(prescription.Product), matchScore: 100));
                return listMatches;
            }
            // If 0 numPicks show all matches (no confidence)
            else
            {
                listMatches = DescriptionMatcher.FindMatch(prescription, wvaProducts, MatchScore);
                return listMatches;
            }
        }

        private static List<MatchProduct> FilterList(int countLimit, Prescription prescription, List<Product> wvaProducts = null, MatchProduct suggestedProduct = null)
        {
            var listMatches = new List<MatchProduct>();

            if (suggestedProduct != null)
            {
                string wvaProd = Database.ReturnWvaProductFor(prescription.Product);
                listMatches.Add(new MatchProduct(name: wvaProd, matchScore: 100));

                // Set the product key
                listMatches[0].ProductKey = wvaProducts.Where(x => x.Description == wvaProd).Select(x => x.ProductKey).First();
            }

            if (wvaProducts != null)
                listMatches.AddRange(DescriptionMatcher.FindMatch(prescription, wvaProducts, MatchScore));

            // Remove match product in list that is the same as the suggested product
            if (listMatches.Count > 1)
                listMatches = listMatches.GroupBy(x => x.Name).Select(x => x.First()).ToList();

            // Get only top x matches
            for (int i = listMatches.Count; i > countLimit; i--)
                listMatches.RemoveAt(i - 1);

            return listMatches;
        }
    }
}
