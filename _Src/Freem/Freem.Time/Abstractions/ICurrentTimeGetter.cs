namespace Freem.Time.Abstractions;

public interface ICurrentTimeGetter
{
    DateTimeOffset Get();
}