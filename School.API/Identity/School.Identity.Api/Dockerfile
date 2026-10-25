# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["School.API/Identity/School.Identity.Api/School.Identity.Api.csproj", "School.API/Identity/School.Identity.Api/"]
COPY ["School.API/Identity/School.Identity.Business/School.Identity.Business.csproj", "School.API/Identity/School.Identity.Business/"]
COPY ["School.API/Identity/School.Identity.Data/School.Identity.Data.csproj", "School.API/Identity/School.Identity.Data/"]
COPY ["Common/School.Entity/School.Entity.csproj", "Common/School.Entity/"]
COPY ["Common/School.ViewModel/School.ViewModel.csproj", "Common/School.ViewModel/"]
COPY ["Common/School.Infrastructure/School.Infrastructure.csproj", "Common/School.Infrastructure/"]
RUN dotnet restore "./School.API/Identity/School.Identity.Api/School.Identity.Api.csproj"
COPY . .
WORKDIR "/src/School.API/Identity/School.Identity.Api"
RUN dotnet build "./School.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./School.Identity.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "School.Identity.Api.dll"]