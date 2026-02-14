FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["nuget.config", "."]
COPY [".editorconfig", "."]
COPY ["Directory.Packages.props", "."]

RUN dotnet nuget update source github \
    --store-password-in-clear-text \
    --configfile nuget.config

COPY ["DirectoryService/", "DirectoryService/"]

RUN dotnet restore "DirectoryService/src/DirectoryService.API/DirectoryService.API.csproj"

RUN dotnet tool install --global dotnet-ef
ENV PATH="/root/.dotnet/tools:${PATH}"

WORKDIR "/src/DirectoryService/src"

RUN dotnet ef migrations bundle \
    --project "DirectoryService.Infrastructure/DirectoryService.Infrastructure.csproj" \
    --startup-project "DirectoryService.API/DirectoryService.API.csproj" \
    --output /app/efbundle \
    --configuration Release \
    --verbose \
    -- --configfile /src/nuget.config

RUN dotnet publish "DirectoryService.API/DirectoryService.API.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .
COPY --from=build /app/efbundle .
RUN chmod +x efbundle
    
EXPOSE 5048
ENTRYPOINT ["dotnet", "DirectoryService.API.dll"]