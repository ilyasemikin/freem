using Freem.Time.Models;

namespace Freem.Web.Api.Public.Contracts.DTO.Statistics.Periods.Abstractions;

public interface IUnitPeriod
{
    DateUnit Unit { get; }
}