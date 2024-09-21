FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AlbinRonnkvist.HybridSearch.Api/AlbinRonnkvist.HybridSearch.Api.csproj", "AlbinRonnkvist.HybridSearch.Api/"]
COPY ["AlbinRonnkvist.HybridSearch.Core/AlbinRonnkvist.HybridSearch.Core.csproj", "AlbinRonnkvist.HybridSearch.Core/"]
COPY ["AlbinRonnkvist.HybridSearch.Embedding/AlbinRonnkvist.HybridSearch.Embedding.csproj", "AlbinRonnkvist.HybridSearch.Embedding/"]
RUN dotnet restore "AlbinRonnkvist.HybridSearch.Api/AlbinRonnkvist.HybridSearch.Api.csproj"

COPY . .
WORKDIR "/src/AlbinRonnkvist.HybridSearch.Api"
RUN dotnet build "AlbinRonnkvist.HybridSearch.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AlbinRonnkvist.HybridSearch.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlbinRonnkvist.HybridSearch.Api.dll"]