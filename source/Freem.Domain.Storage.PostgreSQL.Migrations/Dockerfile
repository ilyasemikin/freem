FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . .

RUN ls ./packages/*.nupkg >/dev/null 2>&1 || (echo "There are no .nupkg dependencies in the folder 'packages'"; exit 1)

WORKDIR /src/Freem.Domain.Storage.PostgreSQL.Migrations
RUN chmod +x ./entrypoint.sh
RUN dotnet build

ENTRYPOINT ["./entrypoint.sh"]
