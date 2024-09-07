using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Extensions;
using Freem.Storage.Migrations.Scripts.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class MvpModelDbConstraintsMigration : Migration
    {
	    public const string ExceptionFunctionsScriptFileName = "Freem.Entities.Storage.PostgreSQL.Migrations.Instances.Raw._20240513044144_MvpModelDbConstraintsMigration.exception_functions.psql";
	    public const string TriggersScriptFileName = "Freem.Entities.Storage.PostgreSQL.Migrations.Instances.Raw._20240513044144_MvpModelDbConstraintsMigration.triggers.psql";
	    
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.RunScript(ExceptionFunctionsScriptFileName, ScriptPart.Declarations);
	        migrationBuilder.RunScript(TriggersScriptFileName, ScriptPart.Declarations);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.RunScript(TriggersScriptFileName, ScriptPart.Droppings);
	        migrationBuilder.RunScript(ExceptionFunctionsScriptFileName, ScriptPart.Droppings);
        }
    }
}
