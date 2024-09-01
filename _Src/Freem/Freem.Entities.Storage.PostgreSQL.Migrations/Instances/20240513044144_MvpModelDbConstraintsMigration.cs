using System.Reflection;
using System.Text;
using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts;
using Freem.Storage.Migrations.Scripts.Models;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class MvpModelDbConstraintsMigration : Migration
    {
	    private const string ScriptFileName = "Freem.Entities.Storage.PostgreSQL.Migrations.Instances.Raw.20240513044144_MvpModelDbConstraintsMigration.psql";
	    
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        var script = ScriptReader.Read(ScriptFileName, ScriptPart.Declarations);
	        migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        var script = ScriptReader.Read(ScriptFileName, ScriptPart.Droppings);
	        migrationBuilder.Sql(script);
        }
    }
}
