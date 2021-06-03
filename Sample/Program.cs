using System;
using CustomHTTPRequestNS;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            CustomHTTPRequest CRequest = new CustomHTTPRequest();
            CustomWebResponse CResponse = new CustomWebResponse();

            CResponse = CRequest.HTTPCustomRequest("https://www.google.com");

            //GET  https://www.google.com 200 OK
            Console.WriteLine(CResponse.Method + "  " +
                CResponse.UrlRequest + " " +
                CResponse.StatusCode + " " +
                CResponse.Status);

            //Time taken : 00:00:00:05
            Console.WriteLine("Time taken : " + CResponse.TimeTaken);

            //REST API Sample 
            RESTRequest RRequest = new RESTRequest();
            CResponse = new CustomWebResponse();

            CResponse = RRequest.RESTGETBearer(@"https://api.sandbox.transferwise.tech/v1/profiles","");

            //GET  https://www.google.com 200 OK
            Console.WriteLine(CResponse.Method + "  " +
                CResponse.UrlRequest + " " +
                CResponse.StatusCode + " " +
                CResponse.Status);

            Console.WriteLine(CResponse.Response);

            //Time taken : 00:00:00:05
            Console.WriteLine("Time taken : " + CResponse.TimeTaken);
        }
    }
}
