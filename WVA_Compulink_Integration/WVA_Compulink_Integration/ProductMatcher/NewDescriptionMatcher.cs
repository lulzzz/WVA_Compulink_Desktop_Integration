using WVA_Compulink_Desktop_Integration.Errors;
using WVA_Compulink_Desktop_Integration.Memory;
using WVA_Compulink_Desktop_Integration.Models.Prescriptions;
using WVA_Compulink_Desktop_Integration.Models.Products;
using WVA_Compulink_Desktop_Integration.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WVA_Compulink_Desktop_Integration.MatchFinder
{
    class NewDescriptionMatcher
    {
        // Pulls from user settings config file loaded after login 
        private static int CharSeqMaxScore = UserData.Data.Settings.ProductMatcher.CharSequenceMaxScore;
        private static int WordMaxScore = UserData.Data.Settings.ProductMatcher.SameWordMaxScore;
        private static int SkuTypeMaxScore = UserData.Data.Settings.ProductMatcher.SkuTypeMaxScore;
        private static int QuantityMaxScore = UserData.Data.Settings.ProductMatcher.QuantityMaxScore; 

        public static List<MatchProduct> FindMatch(Prescription prescription, List<Product> listProducts, double minimumScore)
        {
            try
            {
                // Check for nulls
                if (prescription == null || prescription.Product.Trim() == "")
                    throw new NullReferenceException("'matchString' cannot be null or blank.");

                if (listProducts == null || listProducts?.Count < 1)
                    throw new NullReferenceException("'listProducts' cannot be null or empty");

                return RunMatchFinder(prescription, listProducts, minimumScore);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }


        // Will return a list of possible matches. The lowest index will have the highest match rating
        private static List<MatchProduct> RunMatchFinder(Prescription prescription, List<Product> listProducts, double minimumScore)
        {
            // Check for nulls
            if (prescription == null || prescription?.Product.Trim() == "")
                return new List<MatchProduct>();

            if (listProducts == null || listProducts?.Count < 1)
                return new List<MatchProduct>();

            var ListMatchProducts = new List<MatchProduct>();

           

            foreach (Product product in listProducts)
            {
                var matcherObject = BuildMatcherObject(prescription, product.Description);

                if (matcherObject == null)
                    continue;

                // If the product score meets the minimum score defined by the caller, add it to list of possible matches
                if (matcherObject?.TotalScore > minimumScore)
                {
                    if (!ListMatchProducts.Exists(x => x.Name == product.Description))
                        ListMatchProducts.Add(new MatchProduct(product.Description, matcherObject.TotalScore) { ProductKey = product.ProductKey });
                }
            }

            // This just puts the matched items in a neat order so that the highest match is at the lowest index, i.e. the 100% match is at index[0] and the 98% match is at index[1]
            return ListMatchProducts.OrderBy(o => o.MatchScore).Reverse().ToList();
        }

        // ----------------------------------------------------------------------------------------------------
        //              Build a Matcher Object 
        // ----------------------------------------------------------------------------------------------------

        static MatcherProduct BuildMatcherObject(Prescription compulinkPrescription, string wisVisProduct)
        {
            // Null check 
            if (compulinkPrescription == null || string.IsNullOrWhiteSpace(wisVisProduct))
                return null;

            string p = compulinkPrescription.Product + compulinkPrescription.Type ?? "";

            // Clean up the WVA product
            wisVisProduct = SanitizeProductName(wisVisProduct);

            var MatcherProduct = new MatcherProduct()
            {
                MatchProduct = SanitizeProductName(p),
                SKU = GetSkuType(p),
            };

            MatcherProduct = SetQuantity(MatcherProduct);
            MatcherProduct.MatchProduct = RemoveQuantityFromProductName(MatcherProduct.MatchProduct);
            MatcherProduct.CharacterSequenceMatchScore = GetCharacterSequenceMatchScore(MatcherProduct.MatchProduct, wisVisProduct);
            MatcherProduct.WordMatchScore = GetWordMatchScore(MatcherProduct.MatchProduct, wisVisProduct);
            MatcherProduct.SkuTypeMatchScore = GetSkuTypeMatchScore(GetSkuType(compulinkPrescription), GetSkuType(wisVisProduct));
            MatcherProduct.QuantityMatchScore = GetQuantityMatchScore(MatcherProduct.Quantity, wisVisProduct);
            MatcherProduct = AverageOutCharSequenceToWordMatchScores(MatcherProduct);

            return MatcherProduct;
        }

        // ----------------------------------------------------------------------------------------------------
        //              Product String Manipulation
        // ----------------------------------------------------------------------------------------------------

        static string SanitizeProductName(string productName)
        {
            productName = RemoveGarbage(productName);
            productName = AdjustProductName(productName);
            productName = AdjustSKUType(productName);
            productName = AdjustQuantity(productName);
            productName = BoostDullProducts(productName);

            return productName;
        }

        static string RemoveGarbage(string productName)
        {
            // Null check
            if (productName == null)
                return productName;

            productName = productName.ToLower();
            productName = productName.Trim();

            Regex regex0 = new Regex("trial", RegexOptions.IgnoreCase);
            MatchCollection matches0 = regex0.Matches(productName);
            if (matches0.Count > 1)
                productName = ReplaceLastOccurrence(productName, "trials", "");

            Regex regex1 = new Regex("trial", RegexOptions.IgnoreCase);
            MatchCollection matches1 = regex1.Matches(productName);
            if (matches1.Count > 1)
                productName = ReplaceLastOccurrence(productName, "trial", "");

            productName = productName.Replace("........", "")
                                           .Replace("*", "")
                                           .Replace("-", "")
                                           .Replace("?", "")
                                           .Replace("+", "")
                                           .Replace(",", "");

            productName = productName.Replace("1d ", "1 day ")
                                           .Replace("1 d ", "1 day ")
                                           .Replace("1 da ", "1 day ")
                                           .Replace("1day ", "1 day ")
                                           .Replace("pk", "")
                                           .Replace("pak", "")
                                           .Replace("pack", "")
                                           .Replace(" trail", " trial")
                                           .Replace("final", "")
                                           .Replace(" fina", "")
                                           .Replace(" fin", "")
                                           .Replace(" fin", "")
                                           .Replace(" fi", "");

            return productName;
        }

        static string AdjustProductName(string originalString)
        {
            // Null check
            if (originalString == null)
                return originalString;

            originalString = originalString.Replace("av2 ", "acuvue 2")
                                           .Replace("hydra ", "hydraglyde ")
                                           .Replace("pres  ", "presbyopia ")
                                           .Replace("frlk ", "freshlook ")
                                           .Replace("freq ", "frequency ")
                                           .Replace("prclr ", "proclear ")
                                           .Replace("pv2 ", "purevision 2 ")
                                           .Replace("b and l ", "b&l")
                                           .Replace("bandl ", "b&l")
                                           .Replace("air op ", "air optix ")
                                           .Replace("air opt ", "air optix ")
                                           .Replace("avo ", "acuvue oasys")
                                           .Replace("sl ", "soflens")
                                           .Replace("slmf", "soflens multifocal")
                                           .Replace("nt&day ", "night & day ")
                                           .Replace("n&d ", "night & day ")
                                           .Replace("n & d ", "night & day ")
                                           .Replace("aqcomfplusdali", "aqua comfort plus dailies ")
                                           .Replace("aqcmplus", "aqua comfort plus dailies ")
                                           .Replace("Aq Comf Dailies", "aqua comfort plus dailies ");

            if (!originalString.Contains("uv35"))
                originalString = originalString.Replace("av ", "acuvue ");

            return originalString;
        }

        static string AdjustSKUType(string originalString)
        {
            // Null check
            if (originalString == null)
                return originalString;

            originalString += " ";
            originalString = originalString.Replace("astig ", " astigmatism ")
                                           .Replace("asti ", " astigmatism ")
                                           .Replace("astg", " astigmatism ")
                                           .Replace("astgma", " astigmatism ")
                                           .Replace("toric", " astigmatism ")
                                           .Replace("tor ", " astigmatism ")
                                           .Replace("trc ", " astigmatism ")
                                           .Replace("tori ", " astigmatism ")
                                           .Replace(" mf", " multifocal ")
                                           .Replace("multif ", " multifocal ")
                                           .Replace("multi ", " multifocal ")
                                           .Replace("presby ", "multifocal ")
                                           .Replace("pres ", "multifocal ")
                                           .Replace("presbyopia", "multifocal ")
                                           .Replace("asph ", " aspheric ")
                                           .Replace("clr ", " color ");

            if (!originalString.Contains("comf") && !originalString.Contains("cmf"))
                originalString = originalString.Replace("mf", " multifocal ");

            return originalString.Trim();
        }

        static string AdjustQuantity(string originalString)
        {
            // Null check
            if (originalString == null)
                return originalString;

            originalString = originalString.Replace("ninety", "90")
                                           .Replace("thirty", "30")
                                           .Replace("twentyfour", "24")
                                           .Replace("twelve", "12")
                                           .Replace("six", "6");

            return originalString;
        }

        static string BoostDullProducts(string originalString)
        {
            // Null check
            if (originalString == null)
                return originalString;

            if (originalString.Contains("vitality") && !originalString.Contains("avaira"))
                originalString = originalString.Insert(0, "avaira ");
            if (originalString.Contains("define") && !originalString.Contains("lacreon"))
                originalString = originalString.Insert(0, "w/ lacreon ");
            if (originalString.Contains("biofinity t") && !originalString.Contains("tor"))
                originalString = originalString.Replace("biofinity t", "biofinity astigmatism");

            return originalString;
        }

        static MatcherProduct SetQuantity(MatcherProduct matcherProduct)
        {
            // Null check
            if (matcherProduct == null)
                return null;

            // Sets the matcherProduct's quantity using the product name 
            if (matcherProduct.MatchProduct.Contains("90"))
                matcherProduct.Quantity = "90";
            else if (matcherProduct.MatchProduct.Contains("30"))
                matcherProduct.Quantity = "30";
            else if (matcherProduct.MatchProduct.Contains("24"))
                matcherProduct.Quantity = "24";
            else if (matcherProduct.MatchProduct.Contains("12"))
                matcherProduct.Quantity = "12";
            else if (matcherProduct.MatchProduct.Contains("6") && !matcherProduct.MatchProduct.Contains("trial"))
                matcherProduct.Quantity = "6";
            else if (matcherProduct.MatchProduct.Contains("trial"))
                matcherProduct.Quantity = "trial";

            return matcherProduct;
        }

        static string RemoveQuantityFromProductName(string productName)
        {
            // Null check
            if (productName == null)
                return productName;

            // Removes any quantity identifiers from the product name 

            return productName.Replace("90", "")
                              .Replace("30", "")
                              .Replace("24", "")
                              .Replace("12", "")
                              .Replace("6", "")
                              .Replace("trial", "");
        }

        static string ReplaceLastOccurrence(string source, string find, string replace)
        {
            // Check for null input
            if (source == null || find == null || replace == null)
                return source;

            int place = source.LastIndexOf(find);

            if (place == -1)
                return source;

            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        // ----------------------------------------------------------------------------------------------------
        //              Core Match Functions
        // ----------------------------------------------------------------------------------------------------

        static double GetCharacterSequenceMatchScore(string compulinkProductName, string wisVisProductName)
        {
            var a_charList = compulinkProductName.ToCharArray().ToList();
            a_charList.RemoveAll(x => x.Equals(' '));

            var b_charList = wisVisProductName.ToCharArray().ToList();
            b_charList.RemoveAll(x => x.Equals(' '));

            double score = 0;

            // Check character sequence from a_list to b_list
            foreach (char letter in a_charList)
            {
                int cutIndex = 0;

                for (int i = 0; i < b_charList.Count; i++)
                {
                    if (b_charList[i] == letter)
                    {
                        score++;
                        cutIndex = i;
                        break;
                    }
                }

                b_charList.RemoveRange(0, cutIndex + 1);

                if (b_charList.Count < 1)
                    break;
            }

            return CharSeqMaxScore * (score / a_charList.Count);
        }

        static double GetWordMatchScore(string compulinkProductName, string wvaProductName)
        {
            List<string> a_Words = compulinkProductName.Split(' ').ToList();
            List<string> b_Words = wvaProductName.Split(' ').ToList();

            // Remove blanks to improve match score
            a_Words.RemoveAll(x => x.Equals(""));
            b_Words.RemoveAll(x => x.Equals(""));

            List<string> sharedWords = a_Words.Intersect(b_Words).ToList();

            return (WordMaxScore * ((double)sharedWords.Count() / a_Words.Count()));
        }

        static double GetSkuTypeMatchScore(string compulinkProductSKU, string wvaProductSKU)
        {
            if (compulinkProductSKU == wvaProductSKU)
                return SkuTypeMaxScore;
            else
                return 0;
        }

        static double GetQuantityMatchScore(string prodToMatch, string wisVisProduct)
        {
            // Null check
            if (prodToMatch == null || wisVisProduct == null)
                return 0;

            if (prodToMatch.Contains("90") && wisVisProduct.Contains("90"))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("30") && wisVisProduct.Contains("30"))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("24") && wisVisProduct.Contains("24"))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("12") && wisVisProduct.Contains("12"))
                return QuantityMaxScore;
            else if ((prodToMatch.Contains("6") && !prodToMatch.Contains("trial")) && (wisVisProduct.Contains("6") && !wisVisProduct.Contains("trial")))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("trial") && wisVisProduct.Contains("trial"))
                return QuantityMaxScore;
            else
                return 0;
        }

        // ----------------------------------------------------------------------------------------------------
        //              Helper Functions
        // ----------------------------------------------------------------------------------------------------

        static string GetSkuType(Prescription p)
        {
            // These params are required by all products. If one is null, something is wrong
            if (string.IsNullOrWhiteSpace(p.BaseCurve) || string.IsNullOrWhiteSpace(p.Sphere))
                return null;

            // Sphere
            else if (!string.IsNullOrWhiteSpace(p.BaseCurve) && !string.IsNullOrWhiteSpace(p.Sphere) && string.IsNullOrWhiteSpace(p.Cylinder) && string.IsNullOrWhiteSpace(p.Axis) && string.IsNullOrWhiteSpace(p.Color) && string.IsNullOrWhiteSpace(p.Multifocal) && string.IsNullOrWhiteSpace(p.Add))
                return "sphere";

            // Astigmatism
            else if (!string.IsNullOrWhiteSpace(p.Cylinder) && !string.IsNullOrWhiteSpace(p.Axis) && string.IsNullOrWhiteSpace(p.Color) && string.IsNullOrWhiteSpace(p.Multifocal) && string.IsNullOrWhiteSpace(p.Add))
                return "astigmatism";

            // Multifocal
            else if (string.IsNullOrWhiteSpace(p.Cylinder) && string.IsNullOrWhiteSpace(p.Axis) && string.IsNullOrWhiteSpace(p.Color) && !string.IsNullOrWhiteSpace(p.Multifocal) && !string.IsNullOrWhiteSpace(p.Add))
                return "multifocal";

            // Astigmatism Multifocal                                                                                                  
            else if (!string.IsNullOrWhiteSpace(p.Cylinder) && !string.IsNullOrWhiteSpace(p.Axis) && string.IsNullOrWhiteSpace(p.Color) && !string.IsNullOrWhiteSpace(p.Multifocal) && !string.IsNullOrWhiteSpace(p.Add))
                return "astigmatism multifocal";

            // Color                                                                                                                   
            else if (string.IsNullOrWhiteSpace(p.Cylinder) && string.IsNullOrWhiteSpace(p.Axis) && !string.IsNullOrWhiteSpace(p.Color) && string.IsNullOrWhiteSpace(p.Multifocal) && string.IsNullOrWhiteSpace(p.Add))
                return "color";

            // Astigmatism Color                                                                                                       
            else if (!string.IsNullOrWhiteSpace(p.Cylinder) && !string.IsNullOrWhiteSpace(p.Axis) && !string.IsNullOrWhiteSpace(p.Color) && string.IsNullOrWhiteSpace(p.Multifocal) && string.IsNullOrWhiteSpace(p.Add))
                return "astigmatism color";

            // Multifocal Color                                                                                                        
            else if (string.IsNullOrWhiteSpace(p.Cylinder) && string.IsNullOrWhiteSpace(p.Axis) && !string.IsNullOrWhiteSpace(p.Color) && !string.IsNullOrWhiteSpace(p.Multifocal) && !string.IsNullOrWhiteSpace(p.Add))
                return "multifocal color";

            // Multifocal Astigmatism Color
            else if (!string.IsNullOrWhiteSpace(p.Cylinder) && !string.IsNullOrWhiteSpace(p.Axis) && !string.IsNullOrWhiteSpace(p.Color) && !string.IsNullOrWhiteSpace(p.Multifocal) && !string.IsNullOrWhiteSpace(p.Add))
                return "multifocal astigmatism color";

            else
                return null;
        }

        static string GetSkuType(string productName)
        {
            // Null check
            if (productName == null)
                return null;

            if (productName.Contains("sphere"))
                return "sphere";
            else if (productName.Contains("astigmatism"))
                return "astigmatism";
            else if (productName.Contains("multifocal"))
                return "multifocal";
            else if (productName.Contains("presbyopia"))
                return "presbyopia";
            else if (productName.Contains("aspheric"))
                return "aspheric";
            else if (productName.Contains("color") || productName.Contains("colors"))
                return "color";
            else if (productName.Contains("multifocal") && productName.Contains("astigmatism"))
                return "multifocal astigmatism";
            else if (productName.Contains("multifocal") && productName.Contains("color"))
                return "multifocal color";
            else if (productName.Contains("astigmatism") && productName.Contains("color"))
                return "astigmatism color";
            else
                return null;
        }

        static string GetProductKey(string productName, List<Product> listProducts)
        {
            if (listProducts == null || listProducts?.Count > 1 || productName == null || productName.Trim() == "")
                return null;

            foreach (Product prod in listProducts)
            {
                if (productName == prod.Description)
                    return prod.ProductKey;
            }

            return null;
        }

        static MatcherProduct AverageOutCharSequenceToWordMatchScores(MatcherProduct matcherProduct)
        {
            // The purpose of this is to balance out character sequence and word match score.
            // Gives products with no spaces in them the ability to have a high score instead of needing to be spelled and spaced out correctly 

            if (matcherProduct.WordMatchScore >= 20)
                matcherProduct.WordMatchScore += 15;
            if (matcherProduct.WordMatchScore >= 25)
                matcherProduct.WordMatchScore += 20;
            if (matcherProduct.WordMatchScore >= 30)
                matcherProduct.WordMatchScore += 25;
            if (matcherProduct.WordMatchScore >= 35)
                matcherProduct.WordMatchScore += 30;

            return matcherProduct;
        }
    }
}
