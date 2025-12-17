FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FileService.API/FileService.API.csproj", "FileService.API/"]
COPY ["../Shared.Middleware/Shared.Middleware/Shared.Middleware.csproj", "../Shared.Middleware/Shared.Middleware/"]
COPY ["FileService.Application/FileService.Application.csproj", "FileService.Application/"]
COPY ["FileService.Domain/FileService.Domain.csproj", "FileService.Domain/"]
COPY ["FileService.Infrastructure/FileService.Infrastructure.csproj", "FileService.Infrastructure/"]
RUN dotnet restore "FileService.API/FileService.API.csproj"
COPY . .
WORKDIR "/src/FileService.API"
RUN dotnet build "./FileService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FileService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FileService.API.dll"]
