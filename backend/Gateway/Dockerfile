FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["nuget.config", "."]
COPY [".editorconfig", "."]
COPY ["Directory.Packages.props", "."]

RUN dotnet nuget update source github \
    --store-password-in-clear-text \
    --configfile nuget.config

COPY ["Gateway/", "Gateway/"]

RUN dotnet restore "Gateway/Gateway.Api/Gateway.Api.csproj"

WORKDIR "/src/Gateway"

RUN dotnet publish "Gateway.Api/Gateway.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
    
EXPOSE 80
ENTRYPOINT ["dotnet", "Gateway.Api.dll"]