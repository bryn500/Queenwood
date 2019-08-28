# Get aspnetcore runtime and expose port 80
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Restore packages, copy files, run build
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Queenwood/Queenwood.csproj Queenwood/
RUN dotnet restore Queenwood/Queenwood.csproj
COPY . .
WORKDIR /src/Queenwood
RUN dotnet build Queenwood.csproj -c Release -o /app

# Publish site to /app
FROM build AS publish
RUN dotnet publish Queenwood.csproj -c Release -o /app

# Copy published files into image and then run 
FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Queenwood.dll"]
