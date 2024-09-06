#!/bin/bash

# Script to set up Elasticsearch and Kibana with Docker on Linux without authentication

ELASTIC_VERSION="8.15.1"
KIBANA_VERSION="8.15.1"
ELASTIC_CONTAINER_NAME="elasticsearch"
KIBANA_CONTAINER_NAME="kibana"
ELASTIC_NETWORK="elastic"

ELASTIC_PASSWORD="changeme"  # Default password (not used for authentication)
KIBANA_PASSWORD="changeme"   # Password used internally by Kibana

echo "Creating Docker network..."
docker network create $ELASTIC_NETWORK || echo "Docker network '$ELASTIC_NETWORK' already exists."

echo "Starting Elasticsearch container..."
docker run -p 127.0.0.1:9200:9200 -d --name $ELASTIC_CONTAINER_NAME --network $ELASTIC_NETWORK \
  -e "discovery.type=single-node" \
  -e "xpack.security.enabled=false" \
  -e "xpack.security.http.ssl.enabled=false" \
  -e "xpack.license.self_generated.type=trial" \
  docker.elastic.co/elasticsearch/elasticsearch:$ELASTIC_VERSION

echo "Starting Kibana container..."
docker run -p 127.0.0.1:5601:5601 -d --name $KIBANA_CONTAINER_NAME --network $ELASTIC_NETWORK \
  -e "ELASTICSEARCH_URL=http://elasticsearch:9200" \
  -e "ELASTICSEARCH_HOSTS=http://elasticsearch:9200" \
  -e "ELASTICSEARCH_USERNAME=kibana_system" \
  -e "ELASTICSEARCH_PASSWORD=$KIBANA_PASSWORD" \
  -e "xpack.security.enabled=false" \
  -e "xpack.license.self_generated.type=trial" \
  docker.elastic.co/kibana/kibana:$KIBANA_VERSION

echo "Elasticsearch and Kibana containers are running..."
docker ps | grep -E "$ELASTIC_CONTAINER_NAME|$KIBANA_CONTAINER_NAME"

echo
echo "Elasticsearch available at http://localhost:9200 and Kibana at http://localhost:5601 (it might take some time to be ready)"
echo "Experiment in Kibana at: http://localhost:5601/app/dev_tools#/console"
echo
echo "Cleanup everything with: 'docker system prune'"

