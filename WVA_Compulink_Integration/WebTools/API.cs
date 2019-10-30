using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WVA_Connect_CDI.Errors;

namespace WVA_Connect_CDI.WebTools
{
    public class API
    {
        //
        // POST
        //

        static public string Post(string endpoint, object jsonObject)
        {
            try
            {
                // Convert the object to a json string
                string json = JsonConvert.SerializeObject(jsonObject);

                // Convert json string to a byte array
                var encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(json);

                // Build the request object
                var request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.ContentLength = byteArray.Length;
                    request.ContentType = @"application/json";
                    request.Method = "POST";
                    request.Timeout = 30000;

                // Open up a stream and write our data to it
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response object
                var webResponse = request.GetResponse();

                // Read data from the response object 
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }

        }

        static public string Post(string endpoint, string jsonString)
        {
            try
            {
               // Convert the object to a json string
                string json = JsonConvert.SerializeObject(jsonString);

                // Convert json string to a byte array
                var encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(json);

                // Build the request object
                var request = (HttpWebRequest)WebRequest.Create(endpoint);
                    request.ContentLength = byteArray.Length;
                    request.ContentType = @"application/json";
                    request.Method = "POST";

                // Open up a stream and write our data to it
                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                // Get the response object
                var webResponse = request.GetResponse();

                // Read data from the response object 
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream, Encoding.UTF8))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        //
        // GET
        //

        static public string Get(string endpoint, out string httpStatus)
        {
            try
            {
                string targetResponse = null;
                httpStatus = "";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.ContentType = @"application/json";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    httpStatus = response.StatusDescription;
                    return targetResponse = reader.ReadToEnd();
                }
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                httpStatus = null;
                return null;
            }
        }
    }
}
