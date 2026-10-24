FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG GIT_COMMIT

WORKDIR /source

COPY ./ ./
RUN dotnet restore

WORKDIR /source/Freem.Api.Public
RUN dotnet publish -c release -o /app --no-restore -p:SourceRevisionId=$GIT_COMMIT

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./

RUN apt-get update && apt-get install -y --no-install-recommends curl && rm -rf /var/lib/apt/lists/*

COPY --from=build /source/Freem.Api.Public/appsettings.json ./
ENTRYPOINT ["dotnet", "Freem.Api.Public.dll"]