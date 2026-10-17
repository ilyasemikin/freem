The tests read connection settings from the committed "configuration.json" in this folder, which contains
empty placeholders. Put your local values there and hide the change from git:

```shell
git update-index --skip-worktree configuration.json
```

```JSON
{
	"ConnectionString": "Your PostgreSQL connection string"
}
```
