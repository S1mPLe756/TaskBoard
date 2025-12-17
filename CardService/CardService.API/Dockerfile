FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CardService.API/CardService.API.csproj", "CardService.API/"]
COPY ["../Shared.Middleware/Shared.Middleware/Shared.Middleware.csproj", "../Shared.Middleware/Shared.Middleware/"]
COPY ["CardService.Application/CardService.Application.csproj", "CardService.Application/"]
COPY ["CardService.Domain/CardService.Domain.csproj", "CardService.Domain/"]
COPY ["CardService.Infrastructure/CardService.Infrastructure.csproj", "CardService.Infrastructure/"]
RUN dotnet restore "CardService.API/CardService.API.csproj"
COPY . .
WORKDIR "/src/CardService.API"
RUN dotnet build "./CardService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CardService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CardService.API.dll"]
