namespace ExceptionSolutionProject.ApiResponse
{
    public class UserByIdResponse
    {
        public string Id {  get; set; }
        public string? FullName { get; set; } // Kullanıcının tam adı
        public string? PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
