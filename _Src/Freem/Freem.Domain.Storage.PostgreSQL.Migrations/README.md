### Usage:

```shell
dotnet ef {Migration command} -- {Connection string}
```

### Examples:

#### List migrations

```shell
dotnet ef migrations list -- "Host=127.0.0.1; Port=8080; UserID=user; Password=Password; Database=freem"
```

### Apply all migrations

```shell
dotnet ef database update -- "Host=127.0.0.1; Port=8080; UserID=user; Password=Password; Database=freem"
```

### Apply all migrations in Docker

Container image (build context is `_Src/Freem`) applies all migrations on start. Required environment variables:
`POSTGRES_HOST`, `POSTGRES_DB` (space-separated list of databases), `POSTGRES_USER`, `POSTGRES_PASSWORD`. Used by `docker/develop`, `docker/service`
and `docker/test`.
