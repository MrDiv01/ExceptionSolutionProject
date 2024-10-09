using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExceptionSolutionProject.Models
{
    [Table("Folders")]
    public class Folder
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string FolderName { get; set; }
        public string? FolderDescription {  get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
