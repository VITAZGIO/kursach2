FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ElectroWarehouse/ElectroWarehouse.csproj ElectroWarehouse/
RUN dotnet restore ElectroWarehouse/ElectroWarehouse.csproj

COPY . .
WORKDIR /src/ElectroWarehouse
RUN dotnet publish ElectroWarehouse.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080

ENTRYPOINT ["dotnet", "ElectroWarehouse.dll"]