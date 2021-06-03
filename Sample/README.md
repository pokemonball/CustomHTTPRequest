### CustomHTTPRequest Sample script 🍜
#### How to add project to solution 
✅Please refer to the code below : 
Create new solution into a folder
```sh
dotnet new solution
```
Add existing project into solution 
```sh
dotnet sln .\CustomHTTPRequest.sln add .\CustomHTTPRequestNS.csproj
```
Create a new console solution
```sh
dotnet new console  
```
Add new console to newly created solution 
```sh
dotnet sln .\CustomHTTPRequest.sln add .\Sample\Sample.csproj
```
Add CustomHTTPRequest reference to console application 
```sh
dotnet add .\Sample.csproj reference ..\CustomHTTPRequestNS.csproj
```
#### 🏃🏻‍♀️Run the console application to test the code
```sh
dotnet run
```
Copyright 2021 - modified this file to test merge 2