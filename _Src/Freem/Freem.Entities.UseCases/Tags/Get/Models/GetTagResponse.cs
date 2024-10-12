using System.Diagnostics.CodeAnalysis;
using Freem.Entities.Tags;

namespace Freem.Entities.UseCases.Tags.Get.Models;

public sealed class GetTagResponse
{
    [MemberNotNullWhen(true, nameof(Tag))]
    public bool Founded { get; }
    
    public Tag? Tag { get; }

    public GetTagResponse(Tag? tag)
    {
        Founded = tag is not null;
        Tag = tag;
    }
}