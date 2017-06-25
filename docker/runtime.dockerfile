FROM microsoft/dotnet:2.0.0-preview1-runtime
MAINTAINER Stuart Shay

# Set environment variables
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Staging"

# Open up port
EXPOSE 5000

WORKDIR /app
COPY src/CoreDataStore.Web/bin/Release/netcoreapp2.0/debian.8-x64/publish .
COPY src/CoreDataStore.Web/bin/Release/netcoreapp2.0/debian.8-x64/CoreDataStore.Web.xml .
COPY src/CoreDataStore.Web/wwwroot ./wwwroot

ENTRYPOINT ["dotnet", "CoreDataStore.Web.dll"]
