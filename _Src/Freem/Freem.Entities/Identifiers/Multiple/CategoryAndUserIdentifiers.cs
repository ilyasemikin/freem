namespace Freem.Entities.Identifiers.Multiple;

public sealed class CategoryAndUserIdentifiers
{
    public CategoryIdentifier CategoryId { get; }
    public UserIdentifier UserId { get; }

    public CategoryAndUserIdentifiers(CategoryIdentifier categoryId, UserIdentifier userId)
    {
        ArgumentNullException.ThrowIfNull(categoryId);
        ArgumentNullException.ThrowIfNull(userId);
        
        CategoryId = categoryId;
        UserId = userId;
    }
}