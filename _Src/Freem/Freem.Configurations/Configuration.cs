using Microsoft.Extensions.Configuration;

namespace Freem.Configurations;

public static class Configuration
{
    public static T ReadFromJsonFile<T>(string path)
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile(path);

        var configuration = builder.Build();

        var result = configuration.Get<T>();
        return result is not null
            ? result
            : throw new InvalidOperationException($"Can't convert content of file \"{path}\" to type \"{nameof(T)}\"");
    }
}