#!/bin/bash

# Define the path to the .env file
envFilePath=".env"

# Check if the .env file exists
if [ ! -f "$envFilePath" ]; then
    echo "Error: .env file not found!"
    exit 1
fi

# Initialize variables for database connection parameters
dbUsername=""
dbPassword=""
dbDatabase=""
dbPort=""

# Read the .env file and extract the relevant variables
while IFS='=' read -r key value; do
    # Remove any surrounding quotes from the value
    value=$(echo "$value" | tr -d '"')
    
    case $key in
        DB_USERNAME) dbUsername="$value" ;;
        DB_PASSWORD) dbPassword="$value" ;;
        DB_DATABASE) dbDatabase="$value" ;;
        DB_PORT)     dbPort="$value" ;;
    esac
done < "$envFilePath"

# Construct the connection string, Host nya sengaja localhost btw
connectionString="Host=localhost;Port=$dbPort;Database=$dbDatabase;Username=$dbUsername;Password=$dbPassword;"

# Check if the connection string was constructed correctly
if [ -z "$connectionString" ]; then
    echo "Error: Failed to construct the connection string!"
    exit 1
fi

# Run the EF database update command
cd ./DuitKuAPI/src/DuitKu && dotnet ef database update --connection "$connectionString" --verbose
