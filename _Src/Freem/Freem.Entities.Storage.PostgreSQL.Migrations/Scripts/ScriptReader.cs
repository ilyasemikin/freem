using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Factories;
using Freem.Reflection;
using Freem.Storage.Migrations.Scripts.Models;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts;

internal static class ScriptReader
{
    public static string Read(string scriptName, ScriptPart part)
    {
        string script;
        using (var stream = ResourceLoader.Load(typeof(ScriptReader), scriptName))
        {
            var reader = new StreamReader(stream);
            script = reader.ReadToEnd();
        }
        
        var extractor = ScriptExtractorFactory.Create();
        return extractor.Extract(script, part);
    }
}