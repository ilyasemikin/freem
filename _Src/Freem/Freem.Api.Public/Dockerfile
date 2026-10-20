FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source

COPY ./ ./
RUN dotnet restore

WORKDIR /source/Freem.Api.Public
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app ./
COPY --from=build /source/Freem.Api.Public/appsettings.json ./
ENTRYPOINT ["dotnet", "Freem.Api.Public.dll"]