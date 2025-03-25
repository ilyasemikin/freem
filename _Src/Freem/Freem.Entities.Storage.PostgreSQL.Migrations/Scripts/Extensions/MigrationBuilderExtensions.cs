using Freem.Storage.Migrations.Scripts.Models;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Extensions;

internal static class MigrationBuilderExtensions
{
    public static void RunScript(this MigrationBuilder builder, string scriptName, ScriptPart part)
    {
        var script = ScriptReader.Read(scriptName, part);
        builder.Sql(script);
    }
}