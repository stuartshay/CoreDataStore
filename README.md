# CoreDataStore

[![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/coredatastore.svg)](https://hub.docker.com/r/stuartshay/coredatastore/) [![MyGet](https://img.shields.io/myget/coredatastore/v/CoreDataStore.Data.Postgre.svg)](https://www.myget.org/feed/Packages/coredatastore)

[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=584273597838fd75ab485e34b353101e0eeebdea&metric=alert_status)](http://sonar.navigatorglass.com:9000/dashboard?id=584273597838fd75ab485e34b353101e0eeebdea) [![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=584273597838fd75ab485e34b353101e0eeebdea&metric=reliability_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=584273597838fd75ab485e34b353101e0eeebdea) [![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=584273597838fd75ab485e34b353101e0eeebdea&metric=security_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=584273597838fd75ab485e34b353101e0eeebdea) [![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=584273597838fd75ab485e34b353101e0eeebdea&metric=sqale_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=584273597838fd75ab485e34b353101e0eeebdea)

[![Dependency Status](https://dependencyci.com/github/stuartshay/CoreDataStore/badge)](https://dependencyci.com/github/stuartshay/CoreDataStore) [![Code Climate](https://codeclimate.com/github/stuartshay/CoreDataStore/badges/gpa.svg)](https://codeclimate.com/github/stuartshay/CoreDataStore)

 Build | Status  
------------ | -------------
Docker Base  | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=CoreDataStore-QA/CoreDataStore-base)](https://jenkins.navigatorglass.com/job/CoreDataStore-QA/job/CoreDataStore-base/)
Docker API Local | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=CoreDataStore-QA/CoreDataStore-api-local)](https://jenkins.navigatorglass.com/job/CoreDataStore-QA/job/CoreDataStore-api-local/)
Linux Docker WebAPI | [![CircleCI](https://circleci.com/gh/stuartshay/CoreDataStore.svg?style=shield)](https://circleci.com/gh/stuartshay/CoreDataStore)
Linux Docker Web | [![Build Status](https://travis-ci.org/stuartshay/CoreDataStore.svg?branch=master)](https://travis-ci.org/stuartshay/CoreDataStore)
Windows Docker | [![Build status](https://ci.appveyor.com/api/projects/status/4j2ebt69uw0e0wmg/branch/master?svg=true)](https://ci.appveyor.com/project/StuartShay/coredatastore/branch/master)
Jenkins SonarQube | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=CoreDataStore/CoreDataStore-sonarqube)](https://jenkins.navigatorglass.com/job/CoreDataStore/job/CoreDataStore-sonarqube/)

#### Demo

https://coredatastore.com         

![](assets/web-coredatastore.png)


New York City Landmarks Reference Data     

- [Landmarks Preservation Commission - NYC.gov](http://www1.nyc.gov/site/lpc/index.page)
- [NYC OpenData](http://opendata.cityofnewyork.us/)   

### Prerequisites:
```
Node v6.10.2
NET Core 2.1
VS Code 1.19.1 or VS 2017 15.8.0
```

### Build & Run

```
cd CoreDataStore
setup-env.bat

dotnet restore

cd src\CoreDataStore.Web

npm install

npm run clean
npm run build

dotnet run

http://localhost:5000/

```

[Angular/NodeJS Web Client](https://github.com/stuartshay/CoreDataStore/tree/master/src/CoreDataStore.Web)

## Docker   

[Docker Commands](docker/README.md)      

## Postgres Db

Landmarks Reference Database    

```bash
docker pull stuartshay/coredatastore-postgres:staging 
docker run --rm --name postgresdb -p 5432:5432  stuartshay/coredatastore-postgres:staging 
```

### SonarQube Testing

Windows

```powershell
 .\build.ps1 -target sonar
```

Linux 

```bash
./build.sh --target=sonar
```

### Myget Package Deployment

Windows

```powershell
  $env:mygetApiKey = "adab4634-8ddb-4789-ae92-6461295ac69c"
  .\build.ps1 -target=push-myget
```

Linux
 
```bash
 export mygetApiKey="adab4634-8ddb-4789-ae92-6461295ac69c"
./build.sh --target=push-myget
```

### DocFX

#### Prerequisites:

```
choco install docfx
```

#### Build and Serve Website

```
docfx docfx/docfx.json
docfx docfx/docfx.json --serve
```

```
http://localhost:8080
```

#### Deployment 
```powershell
 .\build.ps1 -target Generate-Docs
```

