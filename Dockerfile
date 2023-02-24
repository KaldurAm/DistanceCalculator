FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["/src/CTeleport.DistanceCalculator.Api/CTeleport.DistanceCalculator.Api.csproj", "CTeleport.DistanceCalculator.Api/"]
COPY ["/src/CTeleport.DistanceCalculator.Core/CTeleport.DistanceCalculator.Core.csproj", "CTeleport.DistanceCalculator.Core/"]
COPY ["/src/CTeleport.DistanceCalculator.Infrastructure/CTeleport.DistanceCalculator.Infrastructure.csproj", "CTeleport.DistanceCalculator.Infrastructure/"]
RUN dotnet restore "/src/CTeleport.DistanceCalculator.Api/CTeleport.DistanceCalculator.Api.csproj"
COPY . .
WORKDIR "src/CTeleport.DistanceCalculator.Api"
RUN dotnet build "CTeleport.DistanceCalculator.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CTeleport.DistanceCalculator.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CTeleport.DistanceCalculator.Api.dll"]
