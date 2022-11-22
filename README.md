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

## Application URLs - LOCAL Environment (Docker Container):
- Contact API:
+ Get list Contacts (HttpGet): http://localhost:6001/api/contacts
+ Get Contact by Id (HttpGet): http://localhost:6001/api/contacts/{id}
+ Create Contact (HttpPost): http://localhost:6001/api/contacts
+ Update Contact (HttpPut): http://localhost:6001/api/contacts

