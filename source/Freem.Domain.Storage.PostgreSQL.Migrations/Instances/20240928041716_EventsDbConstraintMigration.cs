using Freem.Domain.Storage.PostgreSQL.Migrations.Scripts.Extensions;
using Freem.Domain.Storage.PostgreSQL.Migrations.Scripts.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Domain.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class EventsDbConstraintMigration : Migration
    {
        public const string EventsTriggersScriptFileName = "Freem.Domain.Storage.PostgreSQL.Migrations.Instances.Raw._20240928041716_EventsDbConstraintMigration.events_triggers.psql";
        
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(EventsTriggersScriptFileName, ScriptPart.Declarations);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RunScript(EventsTriggersScriptFileName, ScriptPart.Droppings);
        }
    }
}
