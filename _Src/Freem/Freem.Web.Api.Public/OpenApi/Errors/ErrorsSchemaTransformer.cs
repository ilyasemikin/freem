using System.Text.Json.Nodes;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;
using Utilities.Errors;

namespace Freem.Web.Api.Public.OpenApi.Errors;

/// <summary>
/// Describes <see cref="Error"/> parts as they are serialized by Utilities.Errors.Serialization.Json: the generator
/// infers them from the CLR shape (objects, a list of name/value pairs) instead of the converters' output.
/// </summary>
internal sealed class ErrorsSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        if (type == typeof(ErrorType))
        {
            ApplyString(schema, "Error type `<scope>:<code>`, the code may contain `:`.", "freem:entity_not_found");
        }
        else if (type == typeof(ErrorMessage))
        {
            ApplyString(schema, "Human-readable description of the error.", "Entity \"tag\" with id = [...] not found");
        }
        else if (type == typeof(ErrorProperties))
        {
            var nullable = schema.Type?.HasFlag(JsonSchemaType.Null) == true;
            schema.Type = nullable
                ? JsonSchemaType.Object | JsonSchemaType.Null
                : JsonSchemaType.Object;
            schema.Items = null;
            schema.AdditionalProperties = new OpenApiSchema
            {
                Type = JsonSchemaType.String
            };
            schema.Description = "Error properties: name to value, e.g. the identifier of the missing entity.";
        }

        return Task.CompletedTask;
    }

    private static void ApplyString(OpenApiSchema schema, string description, string example)
    {
        schema.Type = JsonSchemaType.String;
        schema.Properties?.Clear();
        schema.Required?.Clear();
        schema.Description = description;
        var value = JsonValue.Create(example);
        schema.Examples = [value];
    }
}
