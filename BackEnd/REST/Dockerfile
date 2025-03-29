# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine3.20 AS build
WORKDIR /src

# 1. Kopeeri projekti failid ja restore
COPY *.csproj .
RUN dotnet restore "REST.csproj"

# 2. Kopeeri kogu kood
COPY . .

# 3. Avalda rakendus /publish kausta
RUN dotnet publish "REST.csproj" -c Release -o /publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine3.20
WORKDIR /app

# 4. Kopeeri ÕIGEST kataloogist (kasuta /publish)
COPY --from=build /publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "REST.dll"]