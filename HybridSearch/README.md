# Hybrid Search

Hybrid Search implemented in C# .NET & Elasticsearch by Roberto Piran Amedi and Albin Rönnkvist

# Getting Started

## Local Elasticsearch setup

- Windows: follow the guide https://www.elastic.co/guide/en/elasticsearch/reference/current/run-elasticsearch-locally.html
- Linux: Run the `DevSetup/SetupElasticsearch.sh` script.

# Build and Test

## Manual tests

1. Run the `ProductsInitializer`: cd into the `Jobs.Initializer` project and run: `dotnet run`. 
    - This will Create a new _Product_ index in Elasticsearch with correct mappings.
2. Experiment in Kibana: open http://localhost:5601/app/dev_tools#/console

# Contribute

To do:
- Service for generating text embeddings
- ETL process for loading Elasticsearch with documents (preferably a large data set)
- API:
    - CRUD operations on Elasticsearch documents
    - Searches with Vector, Keyword and Hybrid alternatives
- UI for showing diff between searches?
- Host Elasticsearch remotely for ease of development?