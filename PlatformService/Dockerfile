FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

# Build phase  
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlatformService.csproj", "./"]
RUN dotnet restore "PlatformService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "PlatformService.csproj" -c Release -o /app/build

# Publish phase where code is compilled into IL and DLL
FROM build AS publish 
RUN dotnet publish "PlatformService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlatformService.dll"]
