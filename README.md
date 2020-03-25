
![Build status](https://www.codeship.io/projects/70ffae40-510f-0138-2e7b-26b6454f2988/status?branch=master)
# TrueLayer
shakespeare's pokemon for TrueLayer

## Instruction to Run the service
### Running in Docker 

```docker-compose up -d```

it should start the service at http://localhost:6080/pokemon/pikachu

### Running locally 
```
cd src 
dotnet run -p PokemonService/PokemonService.csproj 
```
it should start the service at http://localhost:6080/pokemon/pikachu

## Instruction to Run Tests

```docker-compose up -d```
wait for all containers to start
``` 
cd src
dotnet run
```

