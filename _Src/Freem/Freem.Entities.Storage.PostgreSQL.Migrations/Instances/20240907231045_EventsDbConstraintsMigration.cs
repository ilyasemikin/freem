using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Extensions;
using Freem.Storage.Migrations.Scripts.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class EventsDbConstraintsMigration : Migration
    {
        public const string TriggersScriptFileName = "Freem.Entities.Storage.PostgreSQL.Migrations.Instances.Raw._20240907231045_EventsDbConstraintsMigration.triggers.psql";
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(TriggersScriptFileName, ScriptPart.Declarations);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(TriggersScriptFileName, ScriptPart.Droppings);
        }
    }
}
