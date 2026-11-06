namespace conveyor.Models;

public sealed class Job
{
    public long Id { get; set; }
    public string JobType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public string Status { get; set; } = "Queued";
    public int Priority { get; set; }
    public int Attempts { get; set; }
    public DateTime RunAfter { get; set; } = DateTime.UtcNow;
    public string? LockedBy { get; set; }
    public DateTime? LockedAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
