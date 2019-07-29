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
    class API
    {
        // POST
        static public string Post(string endpoint, object jsonObject)
        {
            try
            {
                string targetResponse = null;
                string json = JsonConvert.SerializeObject(jsonObject);

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(json);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.ContentLength = byteArray.Length;
                request.ContentType = @"application/json";
                request.Method = "POST";
                request.Timeout = 30000;

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse webResponse = request.GetResponse();
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    targetResponse = reader.ReadToEnd();
                    reader.Close();
                }

                return targetResponse;
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
                string targetResponse = null;
                string json = JsonConvert.SerializeObject(jsonString);

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] byteArray = encoding.GetBytes(json);

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endpoint);
                request.ContentLength = byteArray.Length;
                request.ContentType = @"application/json";
                request.Method = "POST";

                using (Stream dataStream = request.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                }

                WebResponse webResponse = request.GetResponse();
                using (Stream responseStream = webResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    targetResponse = reader.ReadToEnd();
                    reader.Close();
                }

                return targetResponse;
            }
            catch (Exception x)
            {
                Error.ReportOrLog(x);
                return null;
            }
        }

        // GET
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
