FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
EXPOSE 80
COPY ["AlbinRonnkvist.HybridSearch.WebApp/AlbinRonnkvist.HybridSearch.WebApp.csproj", "AlbinRonnkvist.HybridSearch.WebApp/"]
COPY ["AlbinRonnkvist.HybridSearch.Core/AlbinRonnkvist.HybridSearch.Core.csproj", "AlbinRonnkvist.HybridSearch.Core/"]
RUN dotnet restore "AlbinRonnkvist.HybridSearch.WebApp/AlbinRonnkvist.HybridSearch.WebApp.csproj"

COPY . .
WORKDIR "/src/AlbinRonnkvist.HybridSearch.WebApp"
RUN dotnet build "AlbinRonnkvist.HybridSearch.WebApp.csproj" -c Release -o /app/build
RUN dotnet publish "AlbinRonnkvist.HybridSearch.WebApp.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=build /app/publish/wwwroot .
ENTRYPOINT ["nginx", "-g", "daemon off;"]