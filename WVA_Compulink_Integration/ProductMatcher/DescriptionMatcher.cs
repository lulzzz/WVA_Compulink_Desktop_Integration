using WVA_Connect_CDI.Errors;
using WVA_Connect_CDI.Memory;
using WVA_Connect_CDI.Models.Prescriptions;
using WVA_Connect_CDI.Models.Products;
using WVA_Connect_CDI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using WVA_Connect_CDI.ProductMatcher.Models;

namespace WVA_Connect_CDI.ProductMatcher
{
    class DescriptionMatcher
    {
        // Pulls from user settings config file loaded after login 
        private static int CharSeqMaxScore  = UserData.Data.Settings.ProductMatcher.CharSequenceMaxScore;
        private static int WordMaxScore     = UserData.Data.Settings.ProductMatcher.SameWordMaxScore;
        private static int SkuTypeMaxScore  = UserData.Data.Settings.ProductMatcher.SkuTypeMaxScore;
        private static int QuantityMaxScore = UserData.Data.Settings.ProductMatcher.QuantityMaxScore; 

        public static List<MatchedProduct> FindMatches(Prescription prescription, double minimumScore)
        {
            try
            {
                // Check for nulls
                if (prescription == null || prescription.Product.Trim() == "")
                    throw new NullReferenceException("'matchString' cannot be null or blank.");

                if (WvaProducts.ListProducts == null || WvaProducts.ListProducts?.Count < 1)
                    throw new NullReferenceException("'listProducts' cannot be null or empty");

                return GetMatches(prescription, minimumScore);
            }
            catch (Exception ex)
            {
                Error.ReportOrLog(ex);
                return null;
            }
        }

        // Returns a list of possible matches. The lowest index will have the highest match rating
        private static List<MatchedProduct> GetMatches(Prescription prescription, double minimumScore)
        {
            var listMatchProducts = new List<MatchedProduct>();

            foreach (Product product in WvaProducts.ListProducts)
            {
                try
                {
                    var matcherObject = BuildMatcherObject(prescription, product.Description);

                    // If the product score meets the minimum score defined by the caller, add it to list of possible matches
                    if (matcherObject?.MatchScore > minimumScore)
                    {
                        // Make sure this product doesn't exist in our list of matches already
                        if (!listMatchProducts.Exists(x => x.ProductName == product.Description))
                            listMatchProducts.Add(new MatchedProduct(product.Description, matcherObject.MatchScore) { ProductCode = product.ProductKey });
                    }
                }
                catch (Exception ex)
                {
                    Error.Log(ex.Message);
                }
            }

            // This puts the matched items in a neat order so that the highest match is at the lowest index, i.e. the 100% match is at index[0] and the 98% match is at index[1]
            return listMatchProducts.OrderBy(o => o.MatchScore).Reverse().ToList();
        }

        // ----------------------------------------------------------------------------------------------------
        //              Build a Matcher Object 
        // ----------------------------------------------------------------------------------------------------

        private static PreMatchedProduct BuildMatcherObject(Prescription compulinkPrescription, string wisVisProduct)
        {
            string p = compulinkPrescription.Product + compulinkPrescription.Type ?? "";

            // Clean up the WVA product
            wisVisProduct = SanitizeProductName(wisVisProduct);

            var MatcherProduct = new PreMatchedProduct()
            {
                CompulinkProduct = SanitizeProductName(p),
                SKU = GetSkuType(p),
            };

            MatcherProduct = SetQuantity(MatcherProduct);
            MatcherProduct.CompulinkProduct = RemoveQuantityFromProductName(MatcherProduct.CompulinkProduct);
            MatcherProduct.CharacterSequenceMatchScore = GetCharacterSequenceMatchScore(MatcherProduct.CompulinkProduct, wisVisProduct);
            MatcherProduct.WordMatchScore = GetWordMatchScore(MatcherProduct.CompulinkProduct, wisVisProduct);
            MatcherProduct.SkuTypeMatchScore = GetSkuTypeMatchScore(GetSkuType(compulinkPrescription), GetSkuType(wisVisProduct));
            MatcherProduct.QuantityMatchScore = GetQuantityMatchScore(MatcherProduct.Quantity, wisVisProduct);
            MatcherProduct = AverageOutWordMatchScore(MatcherProduct);

            return MatcherProduct;
        }

        private static string SanitizeProductName(string productName)
        {
            productName = RemoveGarbage(productName);
            productName = AdjustProductName(productName);
            productName = AdjustSKUType(productName);
            productName = AdjustQuantity(productName);
            productName = BoostDullProducts(productName);

            return productName;
        }

        private static string RemoveGarbage(string productName)
        {
            // Null check
            if (productName == null)
                return productName;

            productName = productName.ToLower();
            productName = productName.Trim();

            var regex0 = new Regex("trial", RegexOptions.IgnoreCase);
            MatchCollection matches0 = regex0.Matches(productName);
            if (matches0.Count > 1)
                productName = ReplaceLastOccurrence(productName, "trials", "");

            var regex1 = new Regex("trial", RegexOptions.IgnoreCase);
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

        private static string AdjustProductName(string originalString)
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

        private static string AdjustSKUType(string originalString)
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

        private static string AdjustQuantity(string originalString)
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

        private static string BoostDullProducts(string originalString)
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

        private static PreMatchedProduct SetQuantity(PreMatchedProduct matcherProduct)
        {
            // Null check
            if (matcherProduct == null)
                return null;

            // Sets the matcherProduct's quantity using the product name 
            if (matcherProduct.CompulinkProduct.Contains("90"))
                matcherProduct.Quantity = "90";
            else if (matcherProduct.CompulinkProduct.Contains("30"))
                matcherProduct.Quantity = "30";
            else if (matcherProduct.CompulinkProduct.Contains("24"))
                matcherProduct.Quantity = "24";
            else if (matcherProduct.CompulinkProduct.Contains("12"))
                matcherProduct.Quantity = "12";
            else if (matcherProduct.CompulinkProduct.Contains("6") && !matcherProduct.CompulinkProduct.Contains("trial"))
                matcherProduct.Quantity = "6";
            else if (matcherProduct.CompulinkProduct.Contains("trial"))
                matcherProduct.Quantity = "trial";

            return matcherProduct;
        }

        private static string RemoveQuantityFromProductName(string productName)
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

        private static string ReplaceLastOccurrence(string source, string find, string replace)
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
        //              Core Matching Functions
        // ----------------------------------------------------------------------------------------------------

        private static double GetCharacterSequenceMatchScore(string compulinkProductName, string wvaProductName)
        {
            var a_charList = compulinkProductName.ToCharArray().ToList();
            a_charList.RemoveAll(x => x.Equals(' '));

            var b_charList = wvaProductName.ToCharArray().ToList();
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

        private static double GetWordMatchScore(string compulinkProductName, string wvaProductName)
        {
            List<string> a_Words = compulinkProductName.Split(' ').ToList();
            List<string> b_Words = wvaProductName.Split(' ').ToList();

            // Remove blanks to improve match score
            a_Words.RemoveAll(x => x.Equals(""));
            b_Words.RemoveAll(x => x.Equals(""));

            List<string> sharedWords = a_Words.Intersect(b_Words).ToList();

            return (WordMaxScore * ((double)sharedWords.Count() / a_Words.Count()));
        }

        private static double GetSkuTypeMatchScore(string compulinkProductSKU, string wvaProductSKU)
        {
            if (compulinkProductSKU == wvaProductSKU)
                return SkuTypeMaxScore;
            else
                return 0;
        }

        private static double GetQuantityMatchScore(string prodToMatch, string wvaProduct)
        {
            // Null check
            if (prodToMatch == null || wvaProduct == null)
                return 0;

            if (prodToMatch.Contains("90") && wvaProduct.Contains("90"))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("30") && wvaProduct.Contains("30"))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("24") && wvaProduct.Contains("24"))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("12") && wvaProduct.Contains("12"))
                return QuantityMaxScore;
            else if ((prodToMatch.Contains("6") && !prodToMatch.Contains("trial")) && (wvaProduct.Contains("6") && !wvaProduct.Contains("trial")))
                return QuantityMaxScore;
            else if (prodToMatch.Contains("trial") && wvaProduct.Contains("trial"))
                return QuantityMaxScore;
            else
                return 0;
        }

        // ----------------------------------------------------------------------------------------------------
        //              Helper Functions
        // ----------------------------------------------------------------------------------------------------

        private static string GetSkuType(Prescription p)
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

        private static string GetSkuType(string productName)
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

        private static PreMatchedProduct AverageOutWordMatchScore(PreMatchedProduct matcherProduct)
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
