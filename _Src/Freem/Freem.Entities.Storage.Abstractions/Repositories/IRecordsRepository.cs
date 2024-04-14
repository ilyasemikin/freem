using Freem.Entities.Storage.Abstractions.Base;
using Freem.Entities.Storage.Abstractions.Models.Filters;
using Freem.Entities.Storage.Abstractions.Models.Sorting;

namespace Freem.Entities.Storage.Abstractions.Repositories;

public interface IRecordsRepository :
    IBaseWriteRepository<Record>,
    IBaseSearchByIdRepository<Record>,
    IBaseSearchByFilterRepository<Record, RecordFilter, RecordSortField>
{
}
