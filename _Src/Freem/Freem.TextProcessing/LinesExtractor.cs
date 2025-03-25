namespace Freem.TextProcessing;

public static class LinesExtractor
{
    public static IEnumerable<string> GetLinesBetween(IEnumerable<string> lines, string start, string end)
    {
        var inBlock = false;
        foreach (var line in lines)
        {
            if (line.Contains(start))
                inBlock = true;
            else if (line.Contains(end))
                inBlock = false;
            else if (inBlock)
                yield return line;
        }
    }

    public static string GetLinesBetween(string input, string start, string end)
    {
        var lines = input.Split(Environment.NewLine);
        var result = GetLinesBetween(lines, start, end);
        return string.Join(Environment.NewLine, result);
    }
}