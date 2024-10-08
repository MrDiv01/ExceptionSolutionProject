using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpHub.Models
{
    public class Folder:BaseEntity
    {
        public string FolderName { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
