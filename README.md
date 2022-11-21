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

## Application URLs - LOCAL Environment (Docker Container):
- Contact API:
+ Get list (HttpGet): http://localhost:5001/api/contacts
+ Get by Id (HttpGet): http://localhost:5001/api/contacts/{id}
+ Create (HttpPost): http://localhost:5001/api/contacts
+ Update (HttpPut): http://localhost:5001/api/contacts

