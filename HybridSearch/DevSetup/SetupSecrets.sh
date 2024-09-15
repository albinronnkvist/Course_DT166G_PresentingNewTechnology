#!/bin/bash

# Prompt the user for OpenAI credentials
echo "OpenAI credentials"
read -p "Enter your OpenAI Access Token: " openai_access_token
read -p "Enter your OpenAI Organization ID: " openai_organization_id
read -p "Enter your OpenAI Project ID: " openai_project_id

# Define the user secrets ID for each project
job_initializer_secrets_id="1dc85df5-c665-4c8a-a32e-c5aa99f0ffca"
api_secrets_id="e19c5e46-7837-48d5-bd7a-82181e3398ff"

# Function to set secrets for a project
set_secrets() {
    project_dir=$1
    secrets_id=$2

    echo "Navigating to $project_dir"
    cd $project_dir || { echo "Directory $project_dir not found!"; exit 1; }

    echo "Initializing user secrets (if not already initialized)..."
    dotnet user-secrets init --id $secrets_id

    echo "Setting OpenAI secrets..."
    dotnet user-secrets set "OpenAiEmbeddingApiOptions:AccessToken" "$openai_access_token"
    dotnet user-secrets set "OpenAiEmbeddingApiOptions:OrganizationId" "$openai_organization_id"
    dotnet user-secrets set "OpenAiEmbeddingApiOptions:ProjectId" "$openai_project_id"

    echo "Secrets set successfully for $project_dir"
}

# Set secrets for the Initializer project
set_secrets "../AlbinRonnkvist.HybridSearch.Job.Initializer" "$job_initializer_secrets_id"

# Set secrets for the API project
set_secrets "../AlbinRonnkvist.HybridSearch.Api" "$api_secrets_id"

echo "All secrets set successfully!"
