using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

/// <summary>A calendar unit (day, month or year) to aggregate statistics over.</summary>
public interface IUnitPeriod
{
    /// <summary>Discriminator: which kind of unit the period is.</summary>
    DateUnit Unit { get; }
}