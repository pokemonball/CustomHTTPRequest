# CustomHTTPRequest
[![Nuget](https://img.shields.io/nuget/v/CustomHTTPRequest)](https://www.nuget.org/packages/CustomHTTPRequest/) ![](https://img.shields.io/nuget/dt/CustomHTTPRequest) ![GitHub repo size](https://img.shields.io/github/repo-size/pix3lize/CustomHTTPRequest) [![GitHub issues](https://img.shields.io/github/issues/pix3lize/CustomHTTPRequest)](https://github.com/pix3lize/CustomHTTPRequest/issues)

HTTP Request wrapper library to create `REST API request`, `Web Request to upload file`, and `Web Request with basic authentication`. CustomHTTPRequest come with the functionality to save cookies into cookies container.

### Usage
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
```