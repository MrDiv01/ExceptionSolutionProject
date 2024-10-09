using ExceptionSolutionProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExceptionSolutionProject.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string UserId {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Folder> Folders { get; set; } = new List<Folder>();
    }

}
