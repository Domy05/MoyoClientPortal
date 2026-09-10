# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["backend/ClientPortal.Api.csproj", "backend/"]
RUN dotnet restore "backend/ClientPortal.Api.csproj"
COPY . .
WORKDIR "/src/backend"
RUN dotnet publish "ClientPortal.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Run
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ClientPortal.Api.dll"]
