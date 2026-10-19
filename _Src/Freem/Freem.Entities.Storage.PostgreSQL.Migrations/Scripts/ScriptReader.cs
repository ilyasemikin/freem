using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Factories;
using Freem.Entities.Storage.PostgreSQL.Migrations.Scripts.Models;

namespace Freem.Entities.Storage.PostgreSQL.Migrations.Scripts;

internal static class ScriptReader
{
    public static string Read(string scriptName, ScriptPart part)
    {
        var assembly = typeof(ScriptReader).Assembly;

        string script;
        using (var stream = assembly.GetManifestResourceStream(scriptName))
        {
            if (stream is null)
                throw new InvalidOperationException($"Resource \"{scriptName}\" not found");

            var reader = new StreamReader(stream);
            script = reader.ReadToEnd();
        }
        
        var extractor = ScriptExtractorFactory.Create();
        return extractor.Extract(script, part);
    }
}