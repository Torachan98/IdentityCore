#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./IdentityCore/IdentityCore.csproj", "IdentityCore/"]
COPY ["./IdentityCore.AutoMapper/IdentityCore.AutoMapper.csproj", "IdentityCore.AutoMapper/"]
COPY ["./IdentityCore.EFs/IdentityCore.EFs.csproj", "IdentityCore.EFs/"]
COPY ["./IdentityCore.Repository/IdentityCore.Repository.csproj", "IdentityCore.Repository/"]
COPY ["./IdentityCore.Services/IdentityCore.Services.csproj", "IdentityCore.Services/"]
COPY ["./IdentityCore.Business/IdentityCore.Business.csproj", "IdentityCore.Business/"]
RUN dotnet restore "./IdentityCore/IdentityCore.csproj"
COPY . .

WORKDIR "/src/IdentityCore"
RUN dotnet build "./IdentityCore.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IdentityCore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IdentityCore.dll"]