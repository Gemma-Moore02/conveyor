public sealed record QueueItem
{
    public string Id {get; init;}
    public string JobType {get; init;}
    public string Payload {get; init;}
}