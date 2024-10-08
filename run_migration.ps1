# Define the path to the .env file
$envFilePath = ".\.env"

# Check if the .env file exists
if (-Not (Test-Path $envFilePath)) {
    Write-Host "Error: .env file not found!"
    exit 1
}

# Initialize variables for database connection parameters
$dbUsername = ""
$dbPassword = ""
$dbDatabase = ""
$dbPort = ""

# Read the .env file line by line
$envContent = Get-Content $envFilePath

# Populate variables with values from the .env file
foreach ($line in $envContent) {
    if ($line -match "^(?<key>[^=]+)=(?<value>.*)$") {
        $key = $matches['key']
        $value = $matches['value'].Trim('"')
        
        switch ($key) {
            "DB_USERNAME" { $dbUsername = $value }
            "DB_PASSWORD" { $dbPassword = $value }
            "DB_DATABASE" { $dbDatabase = $value }
            "DB_PORT"     { $dbPort = $value }
        }
    }
}

# Construct the connection string. Host nya sengaja localhost btw
$connectionString = "Host=localhost;Port=$dbPort;Database=$dbDatabase;Username=$dbUsername;Password=$dbPassword;"

# Check if the connection string was constructed correctly
if ([string]::IsNullOrWhiteSpace($connectionString)) {
    Write-Host "Error: Failed to construct the connection string!"
    exit 1
}

# Run the EF database update command
cd ./DuitKuAPI/src/DuitKu && dotnet ef database update --connection $connectionString --verbose
