using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Extensions;
using Freem.Storage.Migrations.Scripts.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class RecreateDbTriggersMigration : Migration
    {
        public const string ExceptionFunctionsScriptFileName = "Freem.Entities.Storage.PostgreSQL.Migrations.Instances.Raw._20240910200644_RecreateDbTriggersMigration.exception_functions.psql";
        public const string EntitiesTriggersScriptFileName = "Freem.Entities.Storage.PostgreSQL.Migrations.Instances.Raw._20240910200644_RecreateDbTriggersMigration.entities_triggers.psql";
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(ExceptionFunctionsScriptFileName, ScriptPart.Declarations);
            migrationBuilder.RunScript(EntitiesTriggersScriptFileName, ScriptPart.Declarations);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(EntitiesTriggersScriptFileName, ScriptPart.Droppings);
            migrationBuilder.RunScript(ExceptionFunctionsScriptFileName, ScriptPart.Droppings);
        }
    }
}
