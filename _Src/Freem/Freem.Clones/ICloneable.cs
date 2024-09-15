namespace Freem.Clones;

public interface ICloneable<out TSelf> : ICloneable
    where TSelf : notnull
{
    new TSelf Clone();

    object ICloneable.Clone()
    {
        return Clone();
    }
}
