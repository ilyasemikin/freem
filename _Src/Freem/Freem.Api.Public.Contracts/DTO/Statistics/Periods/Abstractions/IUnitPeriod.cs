using Freem.Time.Models;

namespace Freem.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

public interface IUnitPeriod
{
    DateUnit Unit { get; }
}