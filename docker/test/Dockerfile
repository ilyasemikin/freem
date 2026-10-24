FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ./source ./
COPY ./docker/test/configs/Freem.Domain.Storage.PostgreSQL.IntegrationTests.configuration.json ./Freem.Domain.Storage.PostgreSQL.IntegrationTests/configuration.json
COPY ./docker/test/configs/Freem.Domain.UseCases.IntegrationTests.configuration.json ./Freem.Domain.UseCases.IntegrationTests/configuration.json
COPY ./docker/test/configs/Freem.Api.Public.IntegrationTests.configuration.json ./Freem.Api.Public.IntegrationTests/configuration.json

RUN ls ./packages/*.nupkg >/dev/null 2>&1 || (echo "There are no .nupkg dependencies in the folder 'packages'"; exit 1)

RUN dotnet restore Freem.slnx
RUN dotnet build Freem.slnx --no-restore
ENTRYPOINT ["dotnet", "test", "Freem.slnx", "--no-build", "--results-directory", "/summary"]
