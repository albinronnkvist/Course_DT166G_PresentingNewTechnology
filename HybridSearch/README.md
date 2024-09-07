# Hybrid Search

Hybrid Search implemented in C# .NET & Elasticsearch by Roberto Piran Amedi and Albin Rönnkvist

# Getting Started

## Local Elasticsearch setup

- Windows: follow the guide https://www.elastic.co/guide/en/elasticsearch/reference/current/run-elasticsearch-locally.html
- Linux: Run the `DevSetup/SetupElasticsearch.sh` script.

## Open AI setup

Add your Open AI credentials to _secrets.json_ in the `Jobs.Initializer` project:

```bash
"OpenAiEmbeddingApiOptions": {
    "AccessToken": "your-access-token",
    "OrganizationId": "your-organization-id",
    "ProjectId": "your-project-id"
}
```

# Build and Test

## Manual tests

### Elasticsearch

1. Uncomment the relevant section in `ProductsInitializer`.
2. Run the initializer (This will Create a new _Product_ index in Elasticsearch with correct mappings): 
    - cd into the `Jobs.Initializer` project
    - Run: `dotnet run`. 
3. Experiment in Kibana: open http://localhost:5601/app/dev_tools#/console

### Generate an embedding

1. Uncomment the relevant section in `ProductsInitializer`.
2. Run the `ProductsInitializer`:
    - cd into the `Jobs.Initializer` project
    - Run: `dotnet run`. 


# Contribute

To do:
- ETL process for loading Elasticsearch with documents (preferably a large data set)
  - A JSON/CSV file with texts to extract
- Store text with respective embedding in a file so we decrease usage of Open AI.
- API:
    - CRUD operations on Elasticsearch documents
    - Searches with Vector, Keyword and Hybrid alternatives
- UI for showing diff between searches?
- Host Elasticsearch remotely for ease of development?