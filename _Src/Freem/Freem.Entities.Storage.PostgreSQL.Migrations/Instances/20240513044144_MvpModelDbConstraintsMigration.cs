using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Instances
{
    /// <inheritdoc />
    public partial class MvpModelDbConstraintsMigration : Migration
    {
	    private const string ScriptFileName =
		    "Freem.Entities.Storage.PostgreSQL.Migrations.Instances.Raw.20240513044144_MvpModelDbConstraintsMigration.psql";

	    private const string BeginDeclarationLexeme = "Begin declaration";
	    private const string EndDeclarationLexeme = "End declaration";

	    private const string BeginDroppingLexeme = "Begin dropping";
	    private const string EndDroppingLexeme = "End dropping";
	    
	    private readonly IReadOnlyList<string> _scriptLines;
	    
	    public MvpModelDbConstraintsMigration()
	    {
		    _scriptLines = ReadScriptLines();
	    }
	    
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        var script = GetUpScript();
	        migrationBuilder.Sql(script);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
	        var script = GetDownScript();
	        migrationBuilder.Sql(script);
        }

        private string GetUpScript()
        {
	        return GetScript(_scriptLines, BeginDeclarationLexeme, EndDeclarationLexeme);
        }

        private string GetDownScript()
        {
	        return GetScript(_scriptLines, BeginDroppingLexeme, EndDroppingLexeme);
        }

        private static IReadOnlyList<string> ReadScriptLines()
        {
	        var stream = Assembly
		        .GetAssembly(typeof(MvpModelDbConstraintsMigration))?
		        .GetManifestResourceStream(ScriptFileName);

	        if (stream is null)
		        throw new InvalidOperationException();

	        var reader = new StreamReader(stream, Encoding.UTF8);

	        return reader
		        .ReadToEnd()
		        .Split("\n");
        }
        
        private static string GetScript(IReadOnlyList<string> lines, string start, string end)
        {
	        var result = new List<string>();
	        
	        var inBlock = false;
	        
	        foreach (var line in lines)
	        {
		        if (line.Contains(start))
			        inBlock = true;
		        if (line.Contains(end))
			        inBlock = false;

		        if (inBlock)
			        result.Add(line);
	        }

	        return string.Join("\n", result);
        }
    }
}
