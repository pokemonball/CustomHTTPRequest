# CustomHTTPRequest
[![Nuget](https://img.shields.io/nuget/v/CustomHTTPRequest)](https://www.nuget.org/packages/CustomHTTPRequest/) ![](https://img.shields.io/nuget/dt/CustomHTTPRequest) ![GitHub repo size](https://img.shields.io/github/repo-size/pix3lize/CustomHTTPRequest) [![GitHub issues](https://img.shields.io/github/issues/pix3lize/CustomHTTPRequest)](https://github.com/pix3lize/CustomHTTPRequest/issues)

HTTP Request wrapper library to create `REST API request`, `Web Request to upload file`, and `Web Request with basic authentication`. CustomHTTPRequest come with the functionality to save cookies into cookies container.

### Usage

#### HTTP GET request 
Please find the sample code :
```csharp 
CustomHTTPRequest CRequest = new CustomHTTPRequest(); 
CustomWebResponse CResponse = new CustomWebResponse();
	
CResponse = CRequest.HTTPCustomRequest("https://www.google.com");
	
//GET  https://www.google.com 200 OK
Console.WriteLine(CResponse.Method + "  " + 
    CResponse.UrlRequest + " " + 
    CResponse.StatusCode + " " +
    CResponse.Status);
	
//Time taken : 00:00:00:05
Console.WriteLine("Time taken : " +CResponse.TimeTaken); 

//Result:
//GET  https://www.google.com 200 OK
//Time taken : 00:00:00:12
```
#### HTTP POST with cookies container 
* Login to a website 
* Save the authentication cookies 
* Download the file
```csharp 
CustomHTTPRequest CRequest = new CustomHTTPRequest();
CustomWebResponse CResponse = new CustomWebResponse();

CookieContainer CContainer = new CookieContainer();

CResponse = CRequest.HTTPCustomRequest("https://docushare.xerox.com/doug/dsweb/ApplyLogin", "username=username@email.com&password=password&domain=DocuShare&Login=Login", ref CContainer);

//GET  https://www.google.com 200 OK
Console.WriteLine(CResponse.Method + "  " +
    CResponse.UrlRequest + " " +
    CResponse.StatusCode + " " +
    CResponse.Status);

//Time taken : 00:00:00:05
Console.WriteLine("Time taken : " + CResponse.TimeTaken);
Console.WriteLine("Response : " + CResponse.Response);

CRequest.ReturnStream = true;
CResponse = CRequest.HTTPCustomRequest(@"https://docushare.xerox.com/doug/dsweb/Get/Document-121470", CContainer);

//To restore response as stream
CResponse.ResponseStream.Position = 0;

//dotnet core use : please use encoding.GetEncoding("windows-1254") 
StreamWriter sw = new StreamWriter(@"download2.pdf", false, Encoding.Default); 

//dotnet core use : please use encoding.GetEncoding("windows-1254") 
StreamReader sr = new StreamReader(CResponse.ResponseStream, Encoding.Default); 
sw.Write(sr.ReadToEnd());
sw.Close();
sr.Close();

//Result:
//File will be downloaded to download2.pdf
```
#### REST API
Send REST API request with JWT token
```csharp
//REST API Sample 
RESTRequest RRequest = new RESTRequest();
CResponse = new CustomWebResponse();

CResponse = RRequest.RESTGETBearer(@"https://api.sandbox.finance.tech/v1/profiles","{{JWT token}}");

```
Check verified key