using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Net.Security;

namespace CustomHTTPRequestNS
{
    /// <summary>
    /// 
    /// </summary>
    public class RESTRequest : CustomHTTPRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public RESTRequest()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            UserAgent = "Agent X";

            XDeveloper = "";
            this.Timeout = 120 * 1000;
            CharEncode = Encoding.Default;
        }

        /// <summary>
        /// Token Authentication POST request  
        /// </summary>
        /// <param name="url">Web URL / REST API URL</param>
        /// <param name="poststring">POST message string</param>
        /// <param name="Token">Your token / JWT</param>
        /// <returns></returns>
        public CustomWebResponse RESTPOSTBearer(string url, string poststring, string Token)
        {
            HttpWebResponse Hresponse = null;
            Stream tstream = null;
            StreamReader reader = null;
            Stopwatch StopWatch = new Stopwatch();
            HttpWebRequest Hrequest = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                StopWatch.Start();
                byte[] Data = CharEncode.GetBytes(poststring);

                CustomWebResponse CustWR;

                //Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                //Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "application/json";

                Hrequest.Headers.Add("Authorization", @"Bearer " + Token);
                Hrequest.ContentLength = Data.Length;

                using (Stream stream = Hrequest.GetRequestStream())
                {
                    stream.Write(Data, 0, Data.Length);
                    stream.Close();
                }

                Hrequest.AllowAutoRedirect = true;
                Hresponse = (HttpWebResponse)Hrequest.GetResponse();

                tstream = Hresponse.GetResponseStream();
                reader = new StreamReader(tstream, CharEncode);

                StopWatch.Stop();
                if (ReturnStream)
                {
                    CustWR = new CustomWebResponse(tstream, null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url, Hresponse.ContentType);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url, Hresponse.ContentType);
                }

                CustWR.Method = Hrequest.Method;
                CustWR.UrlRequest = url;
                return CustWR;
            }
            catch (WebException webex)
            {
                return WebExceptHandle(webex);
            }
            catch (Exception ex)
            {
                CustomWebResponse CustWR = new CustomWebResponse(ex.Message, null, "Custom error this error thrown by other exception on the function", 1000);
                CustWR.Method = Hrequest.Method;
                CustWR.UrlRequest = url;
                return CustWR;
            }
            finally
            {
                if (tstream != null) tstream.Close();
                if (reader != null) reader.Close();
                if (Hresponse != null) Hresponse.Close();
            }
        }


        /// <summary>
        /// Token Authentication GET request  
        /// </summary>
        /// <param name="url">Web URL / REST API URL</param>
        /// <param name="Token">Your token / JWT</param>
        /// <returns></returns>
        public CustomWebResponse RESTGETBearer(string url, string Token)
        {
            HttpWebResponse Hresponse = null;
            Stream tstream = null;
            StreamReader reader = null;
            Stopwatch StopWatch = new Stopwatch();
            HttpWebRequest Hrequest = (HttpWebRequest)WebRequest.Create(url);

            try
            {
                StopWatch.Start();

                CustomWebResponse CustWR;

                //Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "GET";
                //Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "application/json";

                Hrequest.Headers.Add("Authorization", @"Bearer " + Token);

                Hrequest.AllowAutoRedirect = true;
                Hresponse = (HttpWebResponse)Hrequest.GetResponse();

                tstream = Hresponse.GetResponseStream();
                reader = new StreamReader(tstream, CharEncode);

                StopWatch.Stop();
                if (ReturnStream)
                {
                    CustWR = new CustomWebResponse(tstream, null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url, Hresponse.ContentType);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url, Hresponse.ContentType);
                }

                CustWR.Method = Hrequest.Method;
                CustWR.UrlRequest = url;

                return CustWR;
            }
            catch (WebException webex)
            {
                return WebExceptHandle(webex);
            }
            catch (Exception ex)
            {
                CustomWebResponse CustWR = new CustomWebResponse(ex.Message, null, "Custom error this error thrown by other exception on the function", 1000);
                CustWR.Method = Hrequest.Method;
                CustWR.UrlRequest = url;
                return CustWR;
            }
            finally
            {
                if (tstream != null) tstream.Close();
                if (reader != null) reader.Close();
                if (Hresponse != null) Hresponse.Close();
            }
        }
    }
}
