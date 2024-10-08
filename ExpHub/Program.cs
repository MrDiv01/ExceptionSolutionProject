using ExpHub.DTOs;
using ExpHub.Services;

namespace ExpHub
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using (var httpClient = new HttpClient())
            {
                var loginService = new LoginService(httpClient);

                Console.WriteLine("Mail Daxil et");
                string mail = Console.ReadLine();
                Console.WriteLine("Parol Daxil et");
                string password = Console.ReadLine();

                var loginDto = new LoginDto
                {
                    Email = mail,
                    Password = password
                };

                // LoginCheck metodunu çağır
                bool isLoginSuccessful = await loginService.LoginCheck(loginDto);

                if (isLoginSuccessful)
                {
                    Console.WriteLine("Login successful!");

                    #region Started File Uploading
                    // Kullanıcıdan Exp Hub klasör yolunu al
                    Console.WriteLine("Lütfen Exp Hub klasör yolunu girin:");
                    string userDirectoryPath = Console.ReadLine();

                    // Proje dizininde Uploads klasörünün yolunu ayarla (Debug dışına al)
                    string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string uploadsFolder = Path.Combine(projectDirectory, "Uploads");

                    // Uploads klasörü yoksa oluştur
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Exp Hub dizini var mı kontrol et
                    if (Directory.Exists(userDirectoryPath))
                    {
                        // Kopyalama işlemi başlat
                        try
                        {
                            CopyDirectory(userDirectoryPath, uploadsFolder);
                            Console.WriteLine("Tüm dosya ve klasörler başarıyla kopyalandı.");
                        }
                        catch (UnauthorizedAccessException ex)
                        {
                            Console.WriteLine($"Erişim hatası: {ex.Message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Bir hata oluştu: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz dosya yolu girdiniz.");
                    }
                    #endregion
                }
                else
                {
                    Console.WriteLine("Login failed!");
                }
            }
        }

        // Klasörleri ve dosyaları kopyalayacak rekurzif fonksiyon
        static void CopyDirectory(string sourceDir, string destinationDir)
        {
            // Kopyalanacak dizini oluştur
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            // Tüm dosyaları kopyala
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                // Kopyalamak istemediğimiz dosyaları kontrol et
                if (file.Contains(@".git\"))
                {
                    continue; // Bu dosyayı atla
                }

                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Copy(file, destFile, true);
                Console.WriteLine($"{fileName} dosyası kopyalandı.");
            }

            // Alt klasörleri kopyala (rekursif)
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                // Kopyalamak istemediğimiz dizinleri kontrol et
                if (subDir.Contains(@".git\"))
                {
                    continue; // Bu dizini atla
                }

                string subDirName = Path.GetFileName(subDir);
                string destSubDir = Path.Combine(destinationDir, subDirName);
                CopyDirectory(subDir, destSubDir); // Rekursif olarak alt klasörleri kopyala
            }
        }
    }
}
