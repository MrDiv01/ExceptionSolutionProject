namespace ExceptionSolutionProject.Models
{
    public class AIChat:BaseEntity
    {
        public string UserMessage {  get; set; }
        public string ChatMessage { get; set; }
        public string? UserId {  get; set; }
    }
}
