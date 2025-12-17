FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Organization.API/Organization.API.csproj", "Organization.API/"]
COPY ["Organization.Application/Organization.Application.csproj", "Organization.Application/"]
COPY ["Organization.Domain/Organization.Domain.csproj", "Organization.Domain/"]
COPY ["Organization.Infrastructure/Organization.Infrastructure.csproj", "Organization.Infrastructure/"]
COPY ["../Shared.Middleware/Shared.Middleware/Shared.Middleware.csproj", "../Shared.Middleware/Shared.Middleware/"]
RUN dotnet restore "Organization.API/Organization.API.csproj"
COPY . .
WORKDIR "/src/Organization.API"
RUN dotnet build "./Organization.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Organization.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Organization.API.dll"]
