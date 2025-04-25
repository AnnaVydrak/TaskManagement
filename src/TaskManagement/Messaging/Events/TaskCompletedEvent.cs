namespace TaskManagement.Messaging.Events;

public class TaskCompletedEvent
{
    public int TaskId { get; set; }
    public string Name { get; set; } = default!;
    public DateTime CompletedAt { get; set; }
}