# CoreDataStore

[![Build status](https://ci.appveyor.com/api/projects/status/4j2ebt69uw0e0wmg/branch/master?svg=true)](https://ci.appveyor.com/project/StuartShay/coredatastore/branch/master)
[![CircleCI](https://circleci.com/gh/stuartshay/CoreDataStore.svg?style=svg)](https://circleci.com/gh/stuartshay/CoreDataStore)
[![Code Climate](https://codeclimate.com/github/stuartshay/CoreDataStore/badges/gpa.svg)](https://codeclimate.com/github/stuartshay/CoreDataStore)

#### Demo
http://informationcart.eastus2.cloudapp.azure.com    
http://stuartshay.github.io/CoreDataStore/

## Prerequisites:

### .NET Core 
.NET Core SDK 1.1  - November 16, 2016   
https://www.microsoft.com/net/core  

###Visual Studio  

Visual Studio 2015 Update 3     
Visual Studio 2015 Tooling Preview 2

### Web Application

````
nodejs version: 6.x    
npm3
ruby 
gem install compass
```

## Build Web Application

### Clone

```bash
git clone https://github.com/stuartshay/CoreDataStore.git
```

Step 1: .NET Core Restore
```bash
cd  CoreDataStore/
dotnet restore
```

Step 2: Build Web Site


```bash
cd src/CoreDataStore.Web/
npm install
npm run clean
npm run build

dotnet run
```

#### Development Mode

Windows   
```bash
set NG_ENVIRONMENT=Dev
```

Linux   
```bash
export NG_ENVIRONMENT Dev
```


### Live reload Typescript ClientSide (not use dotnet watch)
> npm start

Deploy clientside
> npm run build


##Website

TODO: Enhance cmd - Use 1 win/bash Script (run.bat/run.sh)    

| Environment   | Database Provider     | Port  | Windows cmd  | Linux/Mac cmd
|---------------| ----------------------|:-----:|--------------|--------------
| Development   | SQLite                | 5000  | dev.cmd      | ./dev.sh   
| Staging       | PostgreSQL            | 5001  | stage.cmd    | ./stage.sh
| Production    | SQL Server            | 5002  | prod.cmd     | TODO


```bash
dotnet restore
dotnet build

dotnet run
```
##Docker   

[Docker Commands](docker/README.md)      

##Web Container

[Docker Hub](https://hub.docker.com/r/stuartshay/coredatastore/ )

### Run from Docker Hub
```
docker pull stuartshay/coredatastore
docker run --rm --name coredatastore  -p 5000:5000  stuartshay/coredatastore
```


##Postgres Db

```bash
docker pull stuartshay/coredatastore-postgres:stable
docker run --rm --name postgresdb -p 5432:5432  stuartshay/coredatastore-postgres:stable  
```

##Entity Framework

```bash
cd /test/CoreDataStore.Data.Postgre.Test

dotnet ef migrations add <MigrationName>
dotnet ef database update
```
