using Freem.Entities.UseCases.Plain.Implementations.Executors.Async;
using Freem.UseCases.Abstractions;

namespace Freem.Entities.UseCases.Plain.Implementations;

public class EntitiesPlainExecutors
{
    public ActivitiesPlainExecutor Activities { get; }
    public TagsPlainExecutor Tags { get; }
    
    public RecordsPlainExecutor Records { get; }
    public RunningRecordsPlainExecutor RunningRecords { get; }
    
    public UsersPasswordPlainExecutor UsersPassword { get; }
    public UsersTokensPlainExecutor UsersTokens { get; }
    public UsersSettingsPlainExecutor UsersSettings { get; }
    
    public EntitiesPlainExecutors(IUseCaseExecutor<UseCaseExecutionContext> executor)
    {
        ArgumentNullException.ThrowIfNull(executor);
        
        Activities = new ActivitiesPlainExecutor(executor);
        Tags = new TagsPlainExecutor(executor);
        
        Records = new RecordsPlainExecutor(executor);
        RunningRecords = new RunningRecordsPlainExecutor(executor);
        
        UsersPassword = new UsersPasswordPlainExecutor(executor);
        UsersTokens = new UsersTokensPlainExecutor(executor);
        UsersSettings = new UsersSettingsPlainExecutor(executor);
    }
}