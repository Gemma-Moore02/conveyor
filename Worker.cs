using Microsoft.Extensions.Options;

namespace conveyor;

public class Worker(ILogger<Worker> logger, IJobQueue jobQueue, IOptions<QueueOptions> configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var workerId = $"{Environment.MachineName}{Environment.ProcessId}"; 
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var item = await jobQueue.TryDequeueAsync(workerId, stoppingToken);

                if (item is  null)
                {
                    await Task.Delay(configuration.Value.PollInterval, stoppingToken); 
                    continue;
                }
                else
                {
                    logger.LogInformation("Dequeued Job {JobId}, Type: {JobType}", item.Id, item.JobType);
                    await Task.Delay(3000, stoppingToken); 
                    var isComplete = await jobQueue.CompleteAsync(item.Id, workerId, stoppingToken); 
                    if(!isComplete){
                        logger.LogWarning("Job {JobId} failed to complete", item.Id); 
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Worker loop error"); 
                await Task.Delay(configuration.Value.PollInterval, stoppingToken); 
            }
        }
    }
}
