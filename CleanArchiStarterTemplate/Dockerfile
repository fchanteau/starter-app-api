FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Starter.WebApi/Starter.WebApi.csproj"
RUN dotnet build "Starter.WebApi/Starter.WebApi.csproj" --no-restore -c Release

FROM build AS publish
WORKDIR /src
RUN dotnet publish "Starter.WebApi/Starter.WebApi.csproj" --no-build -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Starter.WebApi.dll"]