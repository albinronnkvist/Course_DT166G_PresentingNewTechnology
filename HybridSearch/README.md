# Hybrid Search

Hybrid Search implemented in C# .NET & Elasticsearch by Roberto Piran Amedi and Albin Rönnkvist

# Getting Started

## Local Elasticsearch setup

- Windows: follow the guide https://www.elastic.co/guide/en/elasticsearch/reference/current/run-elasticsearch-locally.html
- Linux/Mac: Run the `DevSetup/SetupElasticsearch.sh` script.
  - cd into `DevSetup`
  - Run: `./SetupElasticsearch.sh`

## Open AI setup

Add your Open AI credentials to _secrets.json_ in the `AlbinRonnkvist.HybridSearch.Job.Initializer` project:

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

1. Run the initializer (This will create a new _Product_ index in Elasticsearch and load data from _products-example.json_): 
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