using Microsoft.Extensions.Options;

namespace conveyor;

public class Worker(ILogger<Worker> logger, IJobQueue jobQueue, IOptions<QueueOptions> configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var item = await jobQueue.TryDequeueAsync("3", stoppingToken);

                if (item is  null)
                {
                    await Task.Delay(configuration.Value.PollInterval, stoppingToken); 
                    continue;
                }
                else
                {
                    logger.LogInformation("Dequeued Job {JobId}, Type: {JobType}", item.Id, item.JobType);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to dequeue a job"); 
                await Task.Delay(configuration.Value.PollInterval, stoppingToken); 
            }
        }
    }
}
