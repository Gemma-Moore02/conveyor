public sealed class QueueOptions
{
    public string ConnectionString {get; set; } = ""; 
    public TimeSpan PollInterval {get; set; } = TimeSpan.FromSeconds(5); 
    public int MaxAttempts {get; set;} = 5; 
    public TimeSpan LockTimeout {get; set;} = TimeSpan.FromMinutes(10); 
}