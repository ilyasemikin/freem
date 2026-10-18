using Freem.Entities.UseCases.Plain.Implementations.Executors.Sync;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations;

public class EntitiesPlainSyncExecutors
{
    public ActivitiesPlainSyncExecutor Activities { get; }
    public TagsPlainSyncExecutor Tags { get; }
    
    public RecordsPlainSyncExecutor Records { get; }
    public RunningRecordsPlainSyncExecutor RunningRecords { get; }
    
    public UsersPlainSyncExecutor Users { get; }
    public UsersSettingsPlainSyncExecutor UsersSettings { get; }
    
    public EntitiesPlainSyncExecutors(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        Activities = new ActivitiesPlainSyncExecutor(executor);
        Tags = new TagsPlainSyncExecutor(executor);
        
        Records = new RecordsPlainSyncExecutor(executor);
        RunningRecords = new RunningRecordsPlainSyncExecutor(executor);
        
        Users = new UsersPlainSyncExecutor(executor);
        UsersSettings = new UsersSettingsPlainSyncExecutor(executor);
    }
}