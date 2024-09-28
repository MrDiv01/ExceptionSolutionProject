namespace ExceptionSolutionProject.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatingDate { get; set; }
        public DateTime UpdateingDate { get; set; }
    }
}