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