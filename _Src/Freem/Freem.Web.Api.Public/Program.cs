using Freem.Credentials.Password.DependencyInjection.Microsoft.Extensions;
using Freem.Crypto.Hashes.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.DependencyInjection;
using Freem.Entities.Events.Producer.Kafka.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Serialization.Json;
using Freem.Entities.Storage.PostgreSQL.DependencyInjection.Microsoft.Extensions;
using Freem.Entities.Tokens.JWT.DependencyInjection;
using Freem.Entities.UseCases.DependencyInjection.Microsoft.Extensions;
using Freem.Locking.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Time.DependencyInjection.Microsoft;
using Freem.Tokens.Blacklist.Redis.DependencyInjection.Microsoft.Extensions;
using Freem.Tokens.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Authentication.DependencyInjection.Microsoft;
using Freem.Web.Api.Public.Authentication.OpenApi;
using Freem.Web.Api.Public.Configuration.DependencyInjection.Microsoft.Extensions;
using Freem.Web.Api.Public.Configuration.Extensions;
using Freem.Web.Api.Public.ModelBinders.Providers;
using Freem.Web.Api.Public.OpenApi;
using Freem.Web.Api.Public.OpenApi.Headers;
using Freem.Web.Api.Public.Services.DependencyInjection.Microsoft.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseDefaultServiceProvider(options =>
{
    options.ValidateScopes = true;
    options.ValidateOnBuild = true;
});

builder.Services.AddConfiguration(builder.Configuration);

builder.Services.AddHttpLogging(o => {});

builder.Services
    .AddUtcCurrentTimeGetter()
    .AddHttpContextAccessor()
    .AddLogging();

builder.Services
    .AddCryptoHashes()
    .AddPasswordRawHahser()
    .AddGuidSaltGenerator()
    .AddStaticCurrentPasswordHashAlgorithmGetter(builder.Configuration.GetPasswordCredentialsHashAlgorithm());

builder.Services
    .AddPostgreSqlStorage(builder.Configuration.GetStorageConfiguration())
    .AddSimpleRedisDistributedLocks(builder.Configuration.GetRedisLockingConfiguration())
    .AddRedisTokensBlacklist(builder.Configuration.GetRedisTokensConfiguration())
    .AddKafkaEventProduction(builder.Configuration.GetKafkaProducerConfiguration());

builder.Services
    .AddUseCaseContextProvider()
    .AddIdentifiersGenerators()
    .AddEntitiesIdentifiersNameConverters()
    .AddEntitiesUseCases();

builder.Services
    .AddStaticSecurityKeyGetter(builder.Configuration.GetTokensSecretKey())
    .AddAccessTokens(builder.Configuration.GetAccessTokenSettings())
    .AddRefreshTokens(builder.Configuration.GetRefreshTokenSettings());

builder.Services.AddAuthentication()
    .AddJwtBearerAuthentication();

builder.Services
    .AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new QueryListModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        EntitiesJsonSerialization.Populate(options.JsonSerializerOptions);
    });

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<JwtBearerOpenApiDocumentTransformer>();
    options.AddOperationTransformer<JwtBearerOpenApiOperationTransformer>();

    options.AddDocumentTransformer<OpenApiDocumentTransformer>();
    options.AddOperationTransformer<HeadersOpenApiOperationTransformer>();
});

var application = builder.Build();

application
    .MapOpenApi()
    .CacheOutput();
application.MapScalarApiReference();

application.MapControllers();

application.Run();

public partial class Program {}