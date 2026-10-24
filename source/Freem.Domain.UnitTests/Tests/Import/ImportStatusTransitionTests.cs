using Freem.Domain.Import;

namespace Freem.Domain.UnitTests.Tests.Import;

public sealed class ImportStatusTransitionTests
{
    private static readonly ImportDocumentStatus[] DocumentStatuses =
    [
        ImportDocumentStatus.Created,
        ImportDocumentStatus.Parsing,
        ImportDocumentStatus.Parsed,
        ImportDocumentStatus.Failed
    ];

    [Fact]
    public void DocumentStatus_CanChangeTo_ShouldAllowOnlyExpectedTransitions()
    {
        var allowed = new HashSet<(string, string)>
        {
            ("Created", "Parsing"),
            ("Parsing", "Parsed"),
            ("Parsing", "Failed")
        };

        foreach (var current in DocumentStatuses)
        {
            foreach (var next in DocumentStatuses)
            {
                var expected = allowed.Contains((current.ToString(), next.ToString()));

                Assert.Equal(expected, current.CanChangeTo(next));
            }
        }
    }

    [Fact]
    public void CanChangeTo_ShouldThrow_WhenNextIsNull()
    {
        var exception = Record.Exception(() => ImportDocumentStatus.Created.CanChangeTo(null!));

        Assert.IsType<ArgumentNullException>(exception);
    }
}
