# Hybrid Search

Hybrid Search implemented in C# .NET & Elasticsearch by Roberto Piran Amedi and Albin Rönnkvist

# Getting Started

## Elasticsearch local setup

- Windows: follow the guide https://www.elastic.co/guide/en/elasticsearch/reference/current/run-elasticsearch-locally.html
- Linux/Mac: Run the `DevSetup/SetupElasticsearch.sh` script.
  - cd into `DevSetup`
  - Run: `./SetupElasticsearch.sh`

## Setup credentials

Run the `SetupSecrets.sh` script in the `Devsetup` folder to add all secrets. Or follow the sections below for manual setup.

### Open AI credentials

Add your Open AI credentials to _secrets.json_ in the `AlbinRonnkvist.HybridSearch.Job.Initializer` and `AlbinRonnkvist.HybridSearch.Api` projects:

- cd into the project
- Init user secrets with the same `<UserSecretsId>` defined in the project's `.csproj`-file:
    ```bash
    dotnet user-secrets init --id <UserSecretsId>
    ```
- Add the following secrets with your own values:
    ```bash
    dotnet user-secrets set "OpenAiEmbeddingApiOptions:AccessToken" "your-access-token"
    dotnet user-secrets set "OpenAiEmbeddingApiOptions:OrganizationId" "your-organization-id"
    dotnet user-secrets set "OpenAiEmbeddingApiOptions:ProjectId" "your-project-id"
    ```
- Verify:
    ```bash
    dotnet user-secrets list
    ```

# Build and Test

## Manual tests

### Elasticsearch

1. Run the initializer (This will create a new _Product_ index in Elasticsearch and load data from _products-example.json_): 
    - (Optional) Enable `ProductIndexOptions:GenerateEmbeddings` for testing semantic serch
    - cd into the `AlbinRonnkvist.HybridSearch.Jobs.Initializer` project
    - Run: `dotnet run`. 
2. Experiment in [Kibana](http://localhost:5601/app/dev_tools#/console):
    - Execute a regular alias search: 
        ```bash
        GET /sa-product/_search
        ```
    - Execute a A k-nearest neighbor (kNN) search ([read more](https://www.elastic.co/guide/en/elasticsearch/reference/current/knn-search.html)):
        ```bash
        POST /sa-product/_search
        {
            "size": 10,
            "knn": {
                "field": "titleEmbedding", 
                "query_vector": [], // Insert an embedding between the square brackets
                "k": 10,
                "num_candidates": 100
            }
        }
        ```

# Contribute

To do:
- API:
    - CRUD operations on Elasticsearch documents
    - Searches with Vector, Keyword and Hybrid alternatives
- UI for showing diff between searches?