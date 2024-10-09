using ExceptionSolutionProject.Models;

public class ChatMessage : BaseEntity
{
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
}
