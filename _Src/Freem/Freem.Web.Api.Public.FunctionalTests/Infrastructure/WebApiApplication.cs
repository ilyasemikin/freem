using Microsoft.AspNetCore.Mvc.Testing;
using Xunit.Abstractions;

namespace Freem.Web.Api.Public.FunctionalTests.Infrastructure;

public class WebApiApplication : WebApplicationFactory<Program>
{
    private readonly IConfiguration _configuration;
    private readonly Func<ITestOutputHelper?> _factory;

    public WebApiApplication(IConfiguration configuration, Func<ITestOutputHelper?> factory)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(factory);
        
        _configuration = configuration;
        _factory = factory;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureHostConfiguration(cb =>
        {
            cb.AddConfiguration(_configuration);
        });

        var logger = new Logger(_factory);
        var provider = new LoggerProvider(logger);

        builder.ConfigureServices(services =>
        {
            services.AddLogging(logging => logging.AddProvider(provider));
        });
        
        return base.CreateHost(builder);
    }
}

internal class LoggerProvider : ILoggerProvider
{
    private readonly Logger _logger;

    public LoggerProvider(Logger logger)
    {
        _logger = logger;
    }

    public void Dispose()
    {
        // nothing
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _logger;
    }
}

internal class Logger : ILogger
{
    private readonly Func<ITestOutputHelper?> _factory;

    public Logger(Func<ITestOutputHelper?> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        
        _factory = factory;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return null!;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        var output = _factory();
        
        if (exception is not null)
            output?.WriteLine(exception.Message);
        else
            output?.WriteLine(formatter(state, exception));
    }
}