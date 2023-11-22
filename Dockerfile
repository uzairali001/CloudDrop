#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CloudDrop.Api/CloudDrop.Api.csproj", "CloudDrop.Api/"]
COPY ["CloudDrop.Api.Core/CloudDrop.Api.Core.csproj", "CloudDrop.Api.Core/"]
COPY ["CloudDrop.Shared/CloudDrop.Shared.csproj", "CloudDrop.Shared/"]
RUN dotnet restore "CloudDrop.Api/CloudDrop.Api.csproj"

COPY ./CloudDrop.Api ./CloudDrop.Api
COPY ./CloudDrop.Api.Core ./CloudDrop.Api.Core
COPY ./CloudDrop.Shared ./CloudDrop.Shared

WORKDIR "/src/CloudDrop.Api"
RUN dotnet build "CloudDrop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "CloudDrop.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudDrop.Api.dll"]