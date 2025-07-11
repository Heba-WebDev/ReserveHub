FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["ReserveHub.sln", "./"]
COPY ["ReserveHub.API/ReserveHub.API.csproj", "ReserveHub.API/"]
COPY ["ReserveHub.Application/ReserveHub.Application.csproj", "ReserveHub.Application/"]
COPY ["ReserveHub.Domain/ReserveHub.Domain.csproj", "ReserveHub.Domain/"]
COPY ["ReserveHub.Infrastructure/ReserveHub.Infrastructure.csproj", "ReserveHub.Infrastructure/"]

RUN dotnet restore "ReserveHub.sln"

COPY . .

# Build and publish
WORKDIR "/src/ReserveHub.API"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ReserveHub.API.dll"]
