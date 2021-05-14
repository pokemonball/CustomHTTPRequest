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
    /// This is a custom made HTTPRequest in this class you can specify the Timeout and Character encoding 
    /// In addition you also have the liberty to change the return stream by set <br />
    /// CustomHTTPRequest.ReturnStream = true <br />
    /// Copyright by H Tjipto 2014
    /// </summary> 
    public class CustomHTTPRequest
    {
        /// <summary>
        /// Get and set Timeout value in second
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Set CustomHTTPRequest.ReturnStream = true to return the request into stream response.
        /// You will be able to retrieve the response CustomWebResponse.ResponseStream
        /// </summary>
        public bool ReturnStream { get; set; }

        /// <summary>
        /// Set User Agent for the request
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Character encoding for this request
        /// </summary>
        public Encoding CharEncode { get; set; }

        /// <summary>
        /// Time taken to process the request
        /// </summary>
        public string TimeTaken { get; set; }

        /// <summary>
        /// Check content type
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// XDeveloper for GV
        /// </summary>
        public string XDeveloper { get; set; }

        /// <summary>
        /// CustomHTTPRequest object intialization 
        /// </summary>
        public CustomHTTPRequest()
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            UserAgent = "Agent X";

            XDeveloper = "";
            this.Timeout = 120 * 1000;
            CharEncode = Encoding.Default;
        }

        /// <summary>
        /// Basic authentication POST request web request with cookies container 
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="poststring">Post string variable</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="cookiescontain">Cookies contains - .NET C# cookies value container</param>
        /// <returns>CustomWebResponse</returns>
        public CustomWebResponse HTTPCustomRequestBasic(string url, string poststring,
            string username, string password, CookieContainer cookiescontain)
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

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "application/x-www-form-urlencoded";
                Hrequest.ContentLength = Data.Length;
                Hrequest.CookieContainer = cookiescontain;
                if (XDeveloper != "")
                {
                    Hrequest.Headers.Add("X_Developer", XDeveloper);
                }

                NetworkCredential myNetworkCredential = new NetworkCredential(username, password);
                CredentialCache myCredentialCache = new CredentialCache();
                myCredentialCache.Add(new Uri(url), "Basic", myNetworkCredential);

                Hrequest.Credentials = myCredentialCache;
                Hrequest.PreAuthenticate = true;

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
                    CustWR = new CustomWebResponse(tstream, cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Basic authentication POST request web request with cookies container 
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="poststring">Post string variable</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <param name="SOAPActionText">SOAP Action string</param>
        /// <returns>CustomWebResponse</returns>
        public CustomWebResponse HTTPCustomRequestBasicSOAP(string url, string poststring,
            string username, string password, string SOAPActionText)
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
                Hrequest.ContentType = "text/xml; charset=UTF-8";

                NetworkCredential myNetworkCredential = new NetworkCredential(username, password);
                CredentialCache myCredentialCache = new CredentialCache();
                myCredentialCache.Add(new Uri(url), "basic", myNetworkCredential);

                Hrequest.Credentials = myCredentialCache;
                Hrequest.PreAuthenticate = true;

                Hrequest.Headers.Add("SOAPAction", @"""" + SOAPActionText + @"""");
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
        /// Token Authentication POST request  
        /// </summary>
        /// <param name="url">Web URL / REST API URL</param>
        /// <param name="poststring">POST message string</param>
        /// <param name="Token">Your token / JWT</param>
        /// <returns></returns>
        public CustomWebResponse HTTPCustomRequestBearer(string url, string poststring, string Token)
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
        public CustomWebResponse HTTPCustomRequestBearer(string url, string Token)
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



        /// <summary>
        /// Basic authentication GET request web request with cookies container 
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="username">username for basic authentication</param>
        /// <param name="password">password for basic authentication</param>
        /// <param name="cookiescontain">Cookies contains - .NET C# cookies value container</param>
        /// <returns></returns>
        public CustomWebResponse HTTPCustomRequestBasic(string url, string username,
            string password, CookieContainer cookiescontain)
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

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "GET";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "";
                Hrequest.ContentLength = 0;
                Hrequest.CookieContainer = cookiescontain;

                NetworkCredential myNetworkCredential = new NetworkCredential(username, password);
                CredentialCache myCredentialCache = new CredentialCache();
                myCredentialCache.Add(new Uri(url), "Basic", myNetworkCredential);

                Hrequest.Credentials = myCredentialCache;
                Hrequest.PreAuthenticate = true;

                Hrequest.AllowAutoRedirect = true;
                Hresponse = (HttpWebResponse)Hrequest.GetResponse();


                tstream = Hresponse.GetResponseStream();
                reader = new StreamReader(tstream, CharEncode);

                StopWatch.Stop();
                if (ReturnStream)
                {
                    CustWR = new CustomWebResponse(tstream, cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP POST request with cookie container to simulate the session on web client
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="poststring">Post variable request</param>
        /// <param name="cookiescontain">Cookies contains - .NET C# cookies value container</param>
        /// <returns>CustomWebResponse</returns>
        public CustomWebResponse HTTPCustomRequest(string url, string poststring, CookieContainer cookiescontain)
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

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "application/x-www-form-urlencoded";
                Hrequest.ContentLength = Data.Length;
                Hrequest.CookieContainer = cookiescontain;

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
                    CustWR = new CustomWebResponse(tstream, cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP POST request
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="poststring">Post variable request</param>
        /// <returns>CustomWebResponse</returns>
        public CustomWebResponse HTTPCustomRequest(string url, string poststring)
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

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "application/x-www-form-urlencoded";
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
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), null, Hresponse.StatusDescription,
                        (int)Hresponse.StatusCode, StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP GET request 
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <returns>CustomWebResponse</returns>
        public CustomWebResponse HTTPCustomRequest(string url)
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

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "GET";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "";
                Hrequest.ContentLength = 0;

                Hrequest.AllowAutoRedirect = true;
                Hresponse = (HttpWebResponse)Hrequest.GetResponse();

                tstream = Hresponse.GetResponseStream();
                reader = new StreamReader(tstream, CharEncode);

                StopWatch.Stop();
                if (ReturnStream)
                {
                    CustWR = new CustomWebResponse(tstream, null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP POST request with cookie container to simulate the session on web client
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="poststring">Post variable request</param>
        /// <param name="cookiescontain">Cookies contains - .NET C# cookies value container as reference</param>
        /// <returns>CustomWebResponse</returns>
        public CustomWebResponse HTTPCustomRequest(string url, string poststring, ref CookieContainer cookiescontain)
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

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "application/x-www-form-urlencoded";
                Hrequest.ContentLength = Data.Length;
                Hrequest.CookieContainer = cookiescontain;

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
                    CustWR = new CustomWebResponse(tstream, cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP GET request  with cookie container to simulate the session on web client
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="cookiescontain">Cookies contains - .NET C# cookies value container</param>
        /// <returns>CustomWebResponse</returns>
        public CustomWebResponse HTTPCustomRequest(string url, CookieContainer cookiescontain)
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

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "GET";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "";
                Hrequest.ContentLength = 0;
                Hrequest.CookieContainer = cookiescontain;

                Hrequest.AllowAutoRedirect = true;
                Hresponse = (HttpWebResponse)Hrequest.GetResponse();


                tstream = Hresponse.GetResponseStream();
                reader = new StreamReader(tstream, CharEncode);

                StopWatch.Stop();
                if (ReturnStream)
                {
                    CustWR = new CustomWebResponse(tstream, cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), cookiescontain, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP POST request with attachment - content type is multipart/form-data; boundary=
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="poststring">Post variable request</param>
        /// <param name="fileforminput">File input form name *please refer to HTML for dummy*</param>
        /// <param name="filepath">File path for attachment</param>
        /// <returns></returns>
        public CustomWebResponse HTTPCustomRequest(string url, List<PostMessage> poststring, string fileforminput, string filepath)
        {
            HttpWebResponse Hresponse = null;
            Stream tstream = null;
            StreamReader reader = null;
            Stopwatch StopWatch = new Stopwatch();
            HttpWebRequest Hrequest = (HttpWebRequest)WebRequest.Create(url);

            string Boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            try
            {
                StopWatch.Start();

                CustomWebResponse CustWR;

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = "multipart/form-data; boundary=" + Boundary;
                //Hrequest.ContentLength = Data.Length;
                using (Stream stream = Hrequest.GetRequestStream())
                {
                    byte[] Data;
                    string FormItem;
                    FormItem = "--" + Boundary;
                    Data = CharEncode.GetBytes(FormItem);
                    stream.Write(Data, 0, Data.Length);
                    int i = 0;
                    foreach (var item in poststring)
                    {
                        FormItem = string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", item.Key, item.Value);
                        Data = CharEncode.GetBytes(FormItem);
                        stream.Write(Data, 0, Data.Length);
                        if (i < (poststring.Count - 1))
                        {
                            FormItem = "\r\n--" + Boundary;
                            Data = CharEncode.GetBytes(FormItem);
                            stream.Write(Data, 0, Data.Length);

                            if (item.Key == "store_type")
                            {
                                FileInfo Fi = new FileInfo(filepath);

                                FormItem = string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n",
                                   fileforminput, Fi.Name, ContentTypeClass.GetMimeType(Fi.Extension));
                                Data = CharEncode.GetBytes(FormItem);
                                stream.Write(Data, 0, Data.Length);

                                //MemoryStream MS = new MemoryStream();
                                FileStream FS = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                                byte[] BufferSR = new byte[1024];
                                int ByteRead = 0;

                                while ((ByteRead = FS.Read(BufferSR, 0, BufferSR.Length)) != 0)
                                {
                                    stream.Write(BufferSR, 0, ByteRead);
                                }

                                FormItem = "\r\n--" + Boundary;
                                Data = CharEncode.GetBytes(FormItem);
                                stream.Write(Data, 0, Data.Length);
                            }
                        }
                        i++;
                    }

                    FormItem = "\r\n--" + Boundary + "--\r\n";
                    Data = CharEncode.GetBytes(FormItem);
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
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), null, Hresponse.StatusDescription,
                        (int)Hresponse.StatusCode, StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP POST request for XCP with filepath as an input for attachement - content type is multipart/form-data; boundary=
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="identifier">Exclusion Identifier string from create context to push something to machine</param>
        /// <param name="filepath">File path for attachment</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public CustomWebResponse HTTPXCPCustomRequest(string url, string identifier, string filepath, string username, string password)
        {
            HttpWebResponse Hresponse = null;
            Stream tstream = null;
            StreamReader reader = null;
            Stopwatch StopWatch = new Stopwatch();
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest Hrequest = (HttpWebRequest)WebRequest.Create(url);

            string Boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            try
            {
                StopWatch.Start();

                CustomWebResponse CustWR;

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = @"multipart/related; type=""text/xml""; boundary=""" + Boundary + @"""; start=""<2>""";
                Hrequest.Headers.Add("SOAPAction", @"""http://www.fujixerox.co.jp/2011/01/ssm/management/xcp#AddXCPPlugin""");
                Hrequest.Credentials = (ICredentials)new NetworkCredential(username, password);

                //Hrequest.ContentLength = Data.Length;
                using (Stream stream = Hrequest.GetRequestStream())
                {
                    byte[] Data;
                    string FormItem;
                    FormItem = "--" + Boundary;
                    Data = CharEncode.GetBytes(FormItem);
                    stream.Write(Data, 0, Data.Length);

                    FileInfo Fi = new FileInfo(filepath);

                    string message1 =
@"
Content-Type: text/xml; charset=UTF-8
Content-Transfer-Encoding: binary
Content-Id: <2>

<?xml version='1.0' encoding='utf-8' ?><SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header><excls:ExclusiveContext xmlns:excls=""http://www.fujixerox.co.jp/2007/07/ssm/management/exclusion""
><Identifier>::identifier::</Identifier><Expire>120</Expire></excls:ExclusiveContext></SOAP-ENV:Header><SOAP-ENV:Body>
<ns1:AddXCPPlugin xmlns:ns1=""http://www.fujixerox.co.jp/2011/01/ssm/management/xcp"">
<PluginFile href=""cid:1""
   xsi:type=""ns2:DataHandler""
   xmlns:ns2=""http://xml.apache.org/xml-soap""/>
</ns1:AddXCPPlugin>
</SOAP-ENV:Body>
</SOAP-ENV:Envelope>
";

                    message1 = message1.Replace("::identifier::", identifier);

                    Data = CharEncode.GetBytes(message1);
                    stream.Write(Data, 0, Data.Length);

                    FormItem = "--" + Boundary;
                    Data = CharEncode.GetBytes(FormItem);
                    stream.Write(Data, 0, Data.Length);


                    string message2 =
@"
Content-Id: <1>
Content-Type: application/octet-stream
Content-Transfer-Encoding: binary

";

                    Data = CharEncode.GetBytes(message2);
                    stream.Write(Data, 0, Data.Length);

                    FileStream FS = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    byte[] BufferSR = new byte[1024];
                    int ByteRead = 0;

                    while ((ByteRead = FS.Read(BufferSR, 0, BufferSR.Length)) != 0)
                    {
                        stream.Write(BufferSR, 0, ByteRead);
                    }

                    FormItem = "\r\n--" + Boundary + "--\r\n";
                    Data = CharEncode.GetBytes(FormItem);
                    stream.Write(Data, 0, Data.Length);
                    stream.Close();
                }

                Hrequest.AllowAutoRedirect = true;
                ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)((senderx, certificate, chain, sslPolicyErrors) => true);
                Hresponse = (HttpWebResponse)Hrequest.GetResponse();

                tstream = Hresponse.GetResponseStream();
                reader = new StreamReader(tstream, CharEncode);

                StopWatch.Stop();
                if (ReturnStream)
                {
                    CustWR = new CustomWebResponse(tstream, null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), null, Hresponse.StatusDescription,
                        (int)Hresponse.StatusCode, StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// Custom HTTP POST with ONLY attachment - content type is multipart/form-data; boundary=
        /// </summary>
        /// <param name="url">Web URL</param>
        /// <param name="foldername">Custom service name</param>
        /// <param name="identifier">Exclusion Identifier string from create context to push something to machine</param>
        /// <param name="filepath">File path for attachment</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        /// <returns></returns>
        public CustomWebResponse HTTPCSVCustomRequest(string url, string foldername, string identifier, string filepath, string username, string password)
        {
            HttpWebResponse Hresponse = null;
            Stream tstream = null;
            StreamReader reader = null;
            Stopwatch StopWatch = new Stopwatch();
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest Hrequest = (HttpWebRequest)WebRequest.Create(url);

            string Boundary = "----WebKitFormBoundary" + DateTime.Now.Ticks.ToString("x");
            try
            {
                StopWatch.Start();

                CustomWebResponse CustWR;

                Hrequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                Hrequest.Timeout = this.Timeout;
                Hrequest.Method = "POST";
                Hrequest.UserAgent = UserAgent;
                Hrequest.ContentType = @"multipart/related; type=""text/xml""; boundary=""" + Boundary + @"""; start=""<2>""";
                Hrequest.Headers.Add("SOAPAction", @"""http://www.fujixerox.co.jp/2007/07/ssm/management/csv#StoreCsvFiles""");
                Hrequest.Credentials = (ICredentials)new NetworkCredential(username, password);

                //Hrequest.ContentLength = Data.Length;
                using (Stream stream = Hrequest.GetRequestStream())
                {
                    byte[] Data;
                    string FormItem;
                    FormItem = "--" + Boundary;
                    Data = CharEncode.GetBytes(FormItem);
                    stream.Write(Data, 0, Data.Length);

                    FileInfo Fi = new FileInfo(filepath);

                    string message1 =
@"
Content-Type: text/xml; charset=UTF-8
Content-Transfer-Encoding: 8bit
Content-Id: <2>

<?xml version='1.0' encoding='utf-8' ?><SOAP-ENV:Envelope xmlns:SOAP-ENV=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""><SOAP-ENV:Header><excls:ExclusiveContext xmlns:excls=""http://www.fujixerox.co.jp/2007/07/ssm/management/exclusion""
><Identifier>::identifier::</Identifier><Expire>120</Expire></excls:ExclusiveContext></SOAP-ENV:Header><SOAP-ENV:Body>
<StoreCsvFiles xmlns=""http://www.fujixerox.co.jp/2007/07/ssm/management/csv"">
      <FolderName>::foldername::</FolderName>
      <Override>true</Override>
      <File href=""cid:messageid.0000@host.domain"" />
 </StoreCsvFiles>
</SOAP-ENV:Body>
</SOAP-ENV:Envelope>
";

                    message1 = message1.Replace("::identifier::", identifier);
                    message1 = message1.Replace("::foldername::", foldername);
                    Data = CharEncode.GetBytes(message1);
                    stream.Write(Data, 0, Data.Length);

                    FormItem = "--" + Boundary;
                    Data = CharEncode.GetBytes(FormItem);
                    stream.Write(Data, 0, Data.Length);


                    string message2 =
@"
Content-Id: <messageid.0000@host.domain>
Content-Type: application/octet-stream
Content-Transfer-Encoding: binary

";

                    Data = CharEncode.GetBytes(message2);
                    stream.Write(Data, 0, Data.Length);

                    FileStream FS = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                    byte[] BufferSR = new byte[1024];
                    int ByteRead = 0;

                    while ((ByteRead = FS.Read(BufferSR, 0, BufferSR.Length)) != 0)
                    {
                        stream.Write(BufferSR, 0, ByteRead);
                    }

                    FormItem = "\r\n--" + Boundary + "--\r\n";
                    Data = CharEncode.GetBytes(FormItem);
                    stream.Write(Data, 0, Data.Length);
                    stream.Close();
                }

                Hrequest.AllowAutoRedirect = true;
                ServicePointManager.ServerCertificateValidationCallback = (RemoteCertificateValidationCallback)((senderx, certificate, chain, sslPolicyErrors) => true);
                Hresponse = (HttpWebResponse)Hrequest.GetResponse();

                tstream = Hresponse.GetResponseStream();
                reader = new StreamReader(tstream, CharEncode);

                StopWatch.Stop();
                if (ReturnStream)
                {
                    CustWR = new CustomWebResponse(tstream, null, Hresponse.StatusDescription, (int)Hresponse.StatusCode,
                        StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
                }
                else
                {
                    CustWR = new CustomWebResponse(reader.ReadToEnd(), null, Hresponse.StatusDescription,
                        (int)Hresponse.StatusCode, StopWatch.Elapsed.ToString(@"hh\:mm\:ss\:ff"), Hrequest.Method + " " + url);
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
        /// To catch web exception and able to retrieve the web error status and HTTP transaction 
        /// </summary>
        /// <param name="Webex"></param>
        /// <returns></returns>
        public CustomWebResponse WebExceptHandle(WebException Webex)
        {
            CustomWebResponse CustWR;
            if (Webex.Response != null)
            {
                Stream tsstream = Webex.Response.GetResponseStream();
                HttpWebResponse HTTPWebRes = (HttpWebResponse)Webex.Response;
                StreamReader reader = new StreamReader(tsstream);
                CustWR = new CustomWebResponse(reader.ReadToEnd(), null, HTTPWebRes.StatusDescription, (int)HTTPWebRes.StatusCode);
            }
            else
            {
                CustWR = new CustomWebResponse(Webex.Message, null, "Custom error this error thrown by other exception on the function", 1000);
            }

            return CustWR;
        }
    }

    /// <summary>
    /// This is custom made web response you will have the web status time taken to make the request 
    /// plus cookies container which is usefull for session web application parsing <br />
    /// Copyright by H Tjipto 2014
    /// </summary>
    public class CustomWebResponse
    {
        /// <summary>
        /// HTTP response as string 
        /// </summary>
        public string Response { get; set; }

        /// <summary>
        /// HTTP request URL
        /// </summary>
        public string UrlRequest { get; set; }

        /// <summary>
        /// HTTP request method
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// HTTP request CookieContainer this variable will be use to make a session request 
        /// </summary>
        public CookieContainer CookiesContain { get; set; }

        /// <summary>
        /// HTTP response status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// HTTP response content Type 
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// HTTP response status code
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Time taken when sending the HTTP request and getting HTTP response 
        /// </summary>
        public string TimeTaken { get; set; }

        /// <summary>
        /// Encoding type
        /// </summary>
        public string AcceptEncoding { get; set; }

        /// <summary>
        /// Memory stream to handle the stream response 
        /// </summary>
        public MemoryStream ResponseStream = new MemoryStream();

        /// <summary>
        /// Initialise CustomWebResponse
        /// </summary>
        public CustomWebResponse()
        {
            this.TimeTaken = "{notimetakenlog}";
            //Object initiate
        }

        /// <summary>
        /// This CustomWebResponse only intitate this following variable below
        /// </summary>
        /// <param name="Response">HTTP response as string</param>
        /// <param name="CookContain">HTTP request CookieContainer this variable will be use to make a session request </param>
        /// <param name="Status">HTTP response status</param>
        /// <param name="StatusCode">HTTP response status code</param>
        public CustomWebResponse(string Response, CookieContainer CookContain, string Status, int StatusCode)
        {
            this.Response = Response;
            this.CookiesContain = CookContain;
            this.Status = Status;
            this.StatusCode = StatusCode;
        }

        /// <summary>
        /// This CustomWebResponse only intitate this following variable below
        /// </summary>
        /// <param name="PResponseStream">HTTP Response stream</param>
        /// <param name="CookContain">HTTP request CookieContainer this variable will be use to make a session request</param>
        /// <param name="Status">HTTP response status</param>
        /// <param name="StatusCode">HTTP response status code</param>
        public CustomWebResponse(Stream PResponseStream, CookieContainer CookContain, string Status, int StatusCode)
        {
            PResponseStream.CopyTo(ResponseStream);
            this.CookiesContain = CookContain;
            this.Status = Status;
            this.StatusCode = StatusCode;
        }

        /// <summary>
        /// This CustomWebResponse only intitate this following variable below
        /// </summary>
        /// <param name="Response">HTTP response as string</param>
        /// <param name="CookContain">HTTP request CookieContainer this variable will be use to make a session request</param>
        /// <param name="Status">HTTP response status</param>
        /// <param name="StatusCode">HTTP response status code</param>
        /// <param name="TimeTaken">Time taken when sending the HTTP request and getting HTTP response </param>
        /// <param name="url">HTTP request URL</param>
        public CustomWebResponse(string Response, CookieContainer CookContain, string Status, int StatusCode,
            string TimeTaken, string url)
        {
            this.Response = Response;
            this.CookiesContain = CookContain;
            this.Status = Status;
            this.StatusCode = StatusCode;
            this.TimeTaken = TimeTaken;
        }

        /// <summary>
        /// This CustomWebResponse only intitate this following variable below
        /// </summary>
        /// <param name="PResponseStream">HTTP Response stream</param>
        /// <param name="CookContain">HTTP request CookieContainer this variable will be use to make a session request</param>
        /// <param name="Status">HTTP response status</param>
        /// <param name="StatusCode">HTTP response status code</param>
        /// <param name="TimeTaken">Time taken when sending the HTTP request and getting HTTP response </param>
        /// <param name="url">HTTP request URL</param>
        public CustomWebResponse(Stream PResponseStream, CookieContainer CookContain, string Status,
            int StatusCode, string TimeTaken, string url)
        {
            PResponseStream.CopyTo(ResponseStream);
            this.CookiesContain = CookContain;
            this.Status = Status;
            this.StatusCode = StatusCode;
            this.TimeTaken = TimeTaken;
        }

        /// <summary>
        /// This CustomWebResponse only intitate this following variable below
        /// </summary>
        /// <param name="Response">HTTP response as string</param>
        /// <param name="CookContain">HTTP request CookieContainer this variable will be use to make a session request</param>
        /// <param name="Status">HTTP response status</param>
        /// <param name="StatusCode">HTTP response status code</param>
        /// <param name="TimeTaken">Time taken when sending the HTTP request and getting HTTP response </param>
        /// <param name="url">HTTP request URL</param>
        /// <param name="contenttype">Content type</param>
        public CustomWebResponse(string Response, CookieContainer CookContain, string Status, int StatusCode,
            string TimeTaken, string url, string contenttype)
        {
            this.Response = Response;
            this.CookiesContain = CookContain;
            this.Status = Status;
            this.StatusCode = StatusCode;
            this.TimeTaken = TimeTaken;
            this.ContentType = contenttype;
            this.UrlRequest = url;
        }

        /// <summary>
        /// This CustomWebResponse only intitate this following variable below
        /// </summary>
        /// <param name="PResponseStream">HTTP Response stream</param>
        /// <param name="CookContain">HTTP request CookieContainer this variable will be use to make a session request</param>
        /// <param name="Status">HTTP response status</param>
        /// <param name="StatusCode">HTTP response status code</param>
        /// <param name="TimeTaken">Time taken when sending the HTTP request and getting HTTP response </param>
        /// <param name="url">HTTP request URL</param>
        /// <param name="contenttype">Content type</param>
        public CustomWebResponse(Stream PResponseStream, CookieContainer CookContain, string Status,
            int StatusCode, string TimeTaken, string url, string contenttype)
        {
            PResponseStream.CopyTo(ResponseStream);
            this.CookiesContain = CookContain;
            this.Status = Status;
            this.StatusCode = StatusCode;
            this.TimeTaken = TimeTaken;
            this.ContentType = contenttype;
            this.UrlRequest = url;
        }
    }

    /// <summary>
    /// PostMessage class is an object to send post variable to server.
    /// This class is created so we could send post variable in more structured way
    /// </summary>
    public class PostMessage
    {
        /// <summary>
        /// Post message key 
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// Post message value
        /// </summary>        
        public string Value { get; set; }

        /// <summary>
        /// Initialise object with key and value POST message
        /// </summary>
        /// <param name="key">Post message key </param>
        /// <param name="value">Post message value</param>
        public PostMessage(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    /// <summary>
    /// Log the HTTP transaction 
    /// </summary>
    public class HTTPLog
    {

    }
}
