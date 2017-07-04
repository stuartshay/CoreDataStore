FROM microsoft/dotnet:2-runtime-nanoserver
MAINTAINER Stuart Shay

# Set environment variables
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Staging"

# Open up port
EXPOSE 5000

WORKDIR /app
COPY src/CoreDataStore.Web/bin/Release/netcoreapp2.0/win10-x64/publish .
COPY src/CoreDataStore.Web/bin/Release/netcoreapp2.0/win10-x64/CoreDataStore.Web.xml .
COPY src/CoreDataStore.Web/wwwroot ./wwwroot

ENTRYPOINT ["dotnet", "CoreDataStore.Web.dll"]
