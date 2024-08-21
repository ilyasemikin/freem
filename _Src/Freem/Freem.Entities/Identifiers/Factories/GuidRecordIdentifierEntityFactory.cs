namespace Freem.Entities.Identifiers.Factories;

public sealed class GuidRecordIdentifierEntityFactory : BaseGuidIdentifierEntityFactory<RecordIdentifier>
{
    public GuidRecordIdentifierEntityFactory() 
        : base(value => new RecordIdentifier(value))
    {
    }
}