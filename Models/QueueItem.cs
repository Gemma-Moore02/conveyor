public sealed record QueueItem
{
    public long Id {get; init;}
    public string JobType {get; init;} = string.Empty; 
    public string Payload {get; init;} = string.Empty; 
}