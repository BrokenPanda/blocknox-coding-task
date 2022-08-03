FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["meta-exchange-api.csproj", "."]
RUN dotnet restore "./meta-exchange-api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "meta-exchange-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "meta-exchange-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "meta-exchange-api.dll"]