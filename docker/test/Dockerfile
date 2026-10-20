FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ./_Src/Freem ./
COPY ./docker/test/configs/Freem.Entities.Storage.PostgreSQL.IntegrationTests.configuration.json ./Freem.Entities.Storage.PostgreSQL.IntegrationTests/configuration.json
COPY ./docker/test/configs/Freem.Entities.UseCases.IntegrationTests.configuration.json ./Freem.Entities.UseCases.IntegrationTests/configuration.json
COPY ./docker/test/configs/Freem.Locking.Redis.IntegrationTests.configuration.json ./Freem.Locking.Redis.IntegrationTests/configuration.json
COPY ./docker/test/configs/Freem.Web.Api.Public.IntegrationTests.configuration.json ./Freem.Web.Api.Public.IntegrationTests/configuration.json

RUN ls ./packages/*.nupkg >/dev/null 2>&1 || (echo "There are no .nupkg dependencies in the folder 'packages'"; exit 1)

RUN dotnet restore Freem.sln
RUN dotnet build Freem.sln --no-restore
ENTRYPOINT ["dotnet", "test", "Freem.sln", "--no-build", "--results-directory", "/summary"]
