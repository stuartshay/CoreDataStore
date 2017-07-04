FROM microsoft/dotnet:2.0.0-preview1-sdk
MAINTAINER Stuart Shay

# Set environment variables
ENV NG_ENVIRONMENT="Dev"
ENV ASPNETCORE_URLS="http://*:5000"
ENV ASPNETCORE_ENVIRONMENT="Staging"
ENV CONNECTION_PostgreSQL="User ID=nyclandmarks;Password=nyclandmarks;Server=database;Port=5432;Database=nyclandmarks;Integrated Security=true;Pooling=true;"
ENV MAPSAPI="http://informationcart.eastus2.cloudapp.azure.com:82/api/"

# Open up port
EXPOSE 5000

RUN apt-get install curl -y
RUN curl -sL https://deb.nodesource.com/setup_6.x | bash -
RUN apt-get install -y nodejs
RUN npm install -g npm3


COPY src /app/src
COPY CoreDataStore.sln /app/CoreDataStore.sln
COPY NuGet.config /app/NuGet.config
WORKDIR /app

RUN dotnet restore

WORKDIR /app/src/CoreDataStore.Web
RUN npm install
RUN npm run build
RUN dotnet build

ENTRYPOINT ["dotnet", "run"]
