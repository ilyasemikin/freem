FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

COPY ./ ./
RUN dotnet restore

WORKDIR /source/Freem.Web.Api.Public
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app ./
COPY --from=build /source/Freem.Web.Api.Public.appsettings.json ./
ENTRYPOINT ["dotnet", "Freem.Web.Api.Public.dll"]