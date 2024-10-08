namespace ExpHub.Models
{
    public class User:BaseEntity
    {
        public string UserId {  get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Folder> Folders { get; set; } = new List<Folder>();
    }

}
