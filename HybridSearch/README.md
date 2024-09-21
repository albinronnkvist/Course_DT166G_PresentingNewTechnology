# Hybrid Search

Hybrid Search implemented in C# .NET & Elasticsearch by Roberto Piran Amedi and Albin Rönnkvist

# Getting Started

## Setup Elasticsearch

- Windows: follow the guide https://www.elastic.co/guide/en/elasticsearch/reference/current/run-elasticsearch-locally.html
- Linux/Mac: Run the `DevSetup/SetupElasticsearch.sh` script.
  - cd into `DevSetup`
  - Run: `./SetupElasticsearch.sh`

## Setup credentials

Run the `SetupSecrets.sh` script in the `Devsetup` folder to add all secrets

## Seed Elasticsearch

Run the initializer (This will create a new _Product_ index in Elasticsearch and load data from _products-example.json_): 
- (Optional) Enable `ProductIndexOptions:GenerateEmbeddings` for testing semantic serch
- cd into the `AlbinRonnkvist.HybridSearch.Jobs.Initializer` project
- Run: `dotnet run`. 

## Run apps
Run Api and WebApp: 
- cd into the solution root folder
- Run: `docker compose up --build`

# Cleanup

Apps:
```bash
docker compose down
docker rmi hybridsearch-api hybridsearch-webapp
```

Everything:
```bash
docker system prune
```