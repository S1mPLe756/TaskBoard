FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BoardService.API/BoardService.API.csproj", "BoardService.API/"]
COPY ["BoardService.Infrastructure/BoardService.Infrastructure.csproj", "BoardService.Infrastructure/"]
COPY ["BoardService.Domain/BoardService.Domain.csproj", "BoardService.Domain/"]
COPY ["BoardService.Application/BoardService.Application.csproj", "BoardService.Application/"]
COPY ["../Shared.Middleware/Shared.Middleware/Shared.Middleware.csproj", "../Shared.Middleware/Shared.Middleware/"]
RUN dotnet restore "BoardService.API/BoardService.API.csproj"
COPY . .
WORKDIR "/src/BoardService.API"
RUN dotnet build "./BoardService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BoardService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BoardService.API.dll"]
