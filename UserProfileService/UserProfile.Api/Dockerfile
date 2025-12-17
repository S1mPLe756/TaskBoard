FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserProfile.Api/UserProfile.Api.csproj", "UserProfile.Api/"]
COPY ["UserProfile.Domain/UserProfile.Domain.csproj", "UserProfile.Domain/"]
COPY ["UserProfile.Infrastructure/UserProfile.Infrastructure.csproj", "UserProfile.Infrastructure/"]
COPY ["UserProfile.Application/UserProfile.Application.csproj", "UserProfile.Application/"]
COPY ["../Shared.Middleware/Shared.Middleware/Shared.Middleware.csproj", "../Shared.Middleware/Shared.Middleware/"]
RUN dotnet restore "UserProfile.Api/UserProfile.Api.csproj"
COPY . .
WORKDIR "/src/UserProfile.Api"
RUN dotnet build "./UserProfile.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./UserProfile.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserProfile.Api.dll"]
