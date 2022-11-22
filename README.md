# NetCoreTraining01
 Dotnet Core Training 01
 
 ## How to run the project

Run command for build project
```Powershell
dotnet build
```
Go to folder contain file `docker-compose`

1. Using docker-compose
```Powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans
```
2. Build project Contact.API in Docker
```Powershell
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --remove-orphans --build
```
3. Clean up containers and networks created by docker-compose
```Powershell
docker-compose down
```
Using docker run (Go to folder outside Contact.API)

1. docker build -f PROJECT_DIRECTORY/Dockerfile -t IMAGE_NAME .
```Powershell
docker build -f Contact.API/Dockerfile -t netcoretraining .
```

2. docker run -d -p 8888:80 --name CONTAINER_NAME IMAGE_NAME
```Powershell
docker run -d --name myapp -p 8888:80 --network example-app netcoretraining
```
3. Stop container myapp
```Powershell
docker stop myapp
docker rm myapp
```
docker network create example-app
docker run --name dotnetcoretraining -d -p 3306:3306 -e MYSQL_ROOT_PASSWORD=Passw0rd! -v mysql:/var/lib/mysql --network example-app mysql:8.0.29
## Application URLs - LOCAL Environment (Docker Container):
- Contact API:
+ Get list Contacts (HttpGet): http://localhost:6001/api/contacts
+ Get Contact by Id (HttpGet): http://localhost:6001/api/contacts/{id}
+ Create Contact (HttpPost): http://localhost:6001/api/contacts
+ Update Contact (HttpPut): http://localhost:6001/api/contacts

