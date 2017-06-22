## Docker 

Build Instructions for Docker Images      

### Web Assets 

```
cd CoreDataStore.Web
npm i
npm run clean  
npm run build
```

### Linux

#### Publish Build Artifacts 

```
cd  CoreDataStore/
dotnet restore

dotnet publish src/CoreDataStore.Web/CoreDataStore.Web.csproj \
-c Release -f netcoreapp2.0 -r debian.8-x64


docker build -f docker/runtime.dockerfile -t coredatastore-runtime  .
```

#### Build in Container
```
cd  CoreDataStore/
docker build -f docker/aspnetcore.dockerfile -t coredatastore_web  .
```

#### Run
```
docker run -it --rm --name coredatastore_web -p 5000:5000 coredatastore_web
```


### Windows - Nano Server 

````
cd  CoreDataStore/
dotnet restore

dotnet publish src/CoreDataStore.Web/CoreDataStore.Web.csproj \
-c Release -f netcoreapp1.1 -r win10-x64

docker build -f docker/runtime-nanoserver.dockerfile -t coredatastore-runtime  .
````

#### Run
````
docker run -it --rm --name coredatastore_web:nanoserver -p 5000:5000 coredatastore_web:nanoserver

docker inspect -f '{{range .NetworkSettings.Networks}}{{.IPAddress}}{{end}}' <CONTAINERID> 
````

### Docker Compose

##### Local 

````
 docker-compose --file docker-compose-local.yml  up
````

##### Development - (Lattest Tag)
````
 docker-compose --file docker-compose-development.yml  up
````

##### Staging - (Stable Tag)
````
 docker-compose --file docker-compose-staging.yml  up
````

## Usefull Commands 

### Images 

```bash 
    docker images                     #Get List of Images
    docker rmi  -f <IMAGEID>          #Remove Image 
```

### Containers 

```bash 
    docker ps -a                    #All Containers
    docker ps                       #Running Containers 
   
    docker stop <CONTAINERID>       #Stop Running Container
    docker stop $(docker ps -a -q)
   
    docker rm  -f <CONTAINERID>     #Remove Container
    docker rm $(docker ps -a -q)
```

### Tag & Publish

```bash 
docker tag <imageid> coredatastore_web:stable
docker tag coredatastore_web:stable  stuartshay/coredatastore:stable

docker push stuartshay/coredatastore:stable
```

### Run 

```bash 
   docker pull stuartshay/coredatastore
   docker run --rm --name <containername> -p 5000:5000 stuartshay/coredatastore
```

### Volumes 

```bash 
   docker volume ls
   docker volume rm <VOLUMENAME>
```

## Examine Containers

```bash 
# Get Created DateTime  
docker inspect -f '{{ .Created }}' stuartshay/coredatastore
docker logs <CONATINERID>
```

```bash 
# Examine Filesystem (bash shell)
docker run -i -t --entrypoint /bin/bash <IMAGEID>  
```
