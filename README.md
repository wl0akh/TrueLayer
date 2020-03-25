# TrueLayer
shakespeare's pokemon for TrueLayer
## Instruction to Run the service
### Running in Docker 

```docker-compose up -d```

it should start the service at http://localhost:6080/pokemon/pikachu

### Running locallu 
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

