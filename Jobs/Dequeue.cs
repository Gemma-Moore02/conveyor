using conveyor.Models; 
using Dapper; 
using Microsoft.Data.SqlClient; 
using Microsoft.Extensions.Options; 

public interface IJobQueue
{
    Task<QueueItem?> TryDequeueAsync(string workerId, CancellationToken ct); 
    // Task CompleteAsync(long JobId, string workerId, CancellationToken ct); 
    // Task FailAsync(Job job, string workerId, string error, CancellationToken ct); 
}

public sealed class SqlJobQueue : IJobQueue
{
    private readonly QueueOptions _options; 
 
    public SqlJobQueue(IOptions<QueueOptions> options) => _options = options.Value; 

    private SqlConnection Open () => new SqlConnection(_options.ConnectionString); 

    private const string  _dequeueSql = """                
                WITH next AS (
                    SELECT TOP (1) *
                    FROM dbo.Jobs WITH (UPDLOCK, READPAST, ROWLOCK)
                    WHERE Status = 'Queued' AND RunAfter <= SYSUTCDATETIME()
                    ORDER BY Priority DESC, Id
                )
                UPDATE next
                SET Status = 'Processing', 
                    LockedBy = @WorkerId,
                    LockedAt = SYSUTCDATETIME(), 
                    Attempts = Attempts + 1
                OUTPUT inserted.Id, inserted.JobType, inserted.Payload
               
                """; 
    public async Task<QueueItem?> TryDequeueAsync(string workerId, CancellationToken ct)
    {
        using (var connection = Open())
        {
            await connection.OpenAsync(ct);            
                
            return await connection.QuerySingleOrDefaultAsync<QueueItem>(_dequeueSql, new {WorkerId = workerId});               
        }
    }


}