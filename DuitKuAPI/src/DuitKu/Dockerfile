FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Prepare and install deps 🤖
COPY *.csproj ./
RUN dotnet restore

# Publish and Build 👌
COPY . .
RUN dotnet publish -c Release -o out

# Run it 🚀
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

ENTRYPOINT ["dotnet", "DuitKu.dll"]
