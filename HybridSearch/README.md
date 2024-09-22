# Hybrid Search

Hybrid Search implemented in C# .NET & Elasticsearch by Roberto Piran Amedi and Albin Rönnkvist

# Getting Started

## 1. Setup credentials

Run the `SetupSecrets.sh` script in the `Devsetup` folder to add all secrets

## 2. Run docker compose
- cd into the solutions root folder
- Run: `docker compose up --build`

## 3. Seed Elasticsearch

Run the initializer (This will create a new _Product_ index in Elasticsearch and load data from _products-example.json_): 
- (Optional) Enable `ProductIndexOptions:GenerateEmbeddings` for testing semantic search
- cd into the `AlbinRonnkvist.HybridSearch.Jobs.Initializer` project
- Run: `dotnet run`. 