namespace ExceptionSolutionProject.Models
{
    public class ScheduledMessage
    {
        public int Id { get; set; } // Benzersiz tanımlayıcı
        public string UserEmail { get; set; } // Kullanıcının e-posta adresi
        public string Message { get; set; } // Gönderilecek mesaj
        public DateTime ScheduledTime { get; set; } // Planlanan gönderim zamanı
        public bool IsSent { get; set; } // Mesajın gönderilip gönderilmediğini belirtir
    }
}
