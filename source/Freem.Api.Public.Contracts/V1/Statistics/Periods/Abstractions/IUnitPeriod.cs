using Freem.Time;

namespace Freem.Api.Public.Contracts.V1.Statistics.Periods.Abstractions;

/// <summary>A calendar unit (day, week, month or year) to aggregate statistics over.</summary>
public interface IUnitPeriod
{
    /// <summary>Discriminator: which kind of unit the period is.</summary>
    DateUnit Unit { get; }
}