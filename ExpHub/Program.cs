using ExpHub.Data;
using ExpHub.DTOs;
using ExpHub.Models;
using ExpHub.Services;
using Microsoft.EntityFrameworkCore;

namespace ExpHub
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            
            using (var httpClient = new HttpClient())
            {
                var loginService = new LoginService(httpClient);
                bool isLoginSuccessful = false;
                string mail = string.Empty;
                string password = string.Empty;

                // Kullanıcı başarılı bir giriş yapana kadar döngü
                while (!isLoginSuccessful)
                {
                    Console.WriteLine("Mail Daxil et");
                    mail = Console.ReadLine();
                    Console.WriteLine("Parol Daxil et");
                    password = Console.ReadLine();

                    var loginDto = new LoginDto
                    {
                        Email = mail,
                        Password = password
                    };

                    isLoginSuccessful = await loginService.LoginCheck(loginDto);

                    if (!isLoginSuccessful)
                    {
                        Console.WriteLine("Login failed! Lütfen bilgilerinizi kontrol edip tekrar deneyin.");
                    }
                }

                Console.WriteLine("Login successful!");
                UserDto user = await loginService.GetDataByEmail(mail);
                // Kullanıcıya işlem türünü sor
                Console.WriteLine("Dosya yüklemek mi yoksa klasörü kopyalamak mı istiyorsunuz? (Upload/Clone)");
                string action = Console.ReadLine()?.ToLower();

                if (action == "upload")
                {
                    #region File Upload
                    Console.WriteLine("Lütfen kopyalanacak klasör yolunu girin:");
                    string userDirectoryPath = Console.ReadLine();

                    string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string uploadsFolder = Path.Combine(projectDirectory, "Uploads");

                    // Kullanıcının emaili ile klasör yolu
                    string userFolder = Path.Combine(uploadsFolder, mail.ToLower());

                    // Email adında klasör varsa yeniden oluşturma, yoksa oluştur
                    if (!Directory.Exists(userFolder))
                    {
                        Directory.CreateDirectory(userFolder);
                        Console.WriteLine($"{mail.ToLower()} için klasör oluşturuldu.");
                    }
                    else
                    {
                        Console.WriteLine($"{mail.ToLower()} için klasör zaten mevcut.");
                    }

                    string customSubFolderName;

                    // Kullanıcıdan alt klasör adını al ve tekrar giriş iste
                    while (true)
                    {
                        Console.WriteLine("Lütfen klasör anahtarini girin klasöre bu anahtarla erisim saglanacak:");
                        customSubFolderName = Console.ReadLine();


                        string userSubFolder = Path.Combine(userFolder, customSubFolderName);
                        if (Directory.Exists(userSubFolder))
                        {
                            Console.WriteLine($"{customSubFolderName} klasörü zaten mevcut. Lütfen farklı bir ad girin.");
                        }
                        else
                        {
                            // Eğer alt klasör yoksa oluştur
                            Directory.CreateDirectory(userSubFolder);
                            Console.WriteLine($"{customSubFolderName} klasörü oluşturuldu.");
                            break; // Döngüyü kır
                        }
                    }



                    Console.WriteLine("Lutfen Folderin Descriptionun girin");
                   string description =  Console.ReadLine();

                    if (Directory.Exists(userDirectoryPath))
                    {
                        try
                        {
                            // userDirectoryPath'in son klasör adını al
                            string lastFolderName = Path.GetFileName(userDirectoryPath.TrimEnd(Path.DirectorySeparatorChar));

                            // Alt klasör içinde son klasör adıyla yeni klasör oluştur
                            string finalSubFolder = Path.Combine(userFolder, customSubFolderName, lastFolderName);
                            Directory.CreateDirectory(finalSubFolder);
                            Console.WriteLine($"{lastFolderName} klasörü oluşturuldu.");

                            // Dosyaları yeni oluşturulan klasöre kopyala
                            CopyDirectory(userDirectoryPath, finalSubFolder);
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

                        using (var context = new ExpApplicationDbContext())
                        {
                            var data = new User
                            {
                                UserId = user.UserId, // UserDto'dan UserId'yi al
                                Name = user.UserName,  // UserDto'dan UserName'i al
                                Email = user.UserMail,  // UserDto'dan UserMail'i al
                                DateTime = DateTime.Now
                            };
                            var dataCheck = await context.Users.FirstOrDefaultAsync(x => x.Email == data.Email);
                            if (dataCheck != null)
                            {
                                var folder = new Folder
                                {
                                    FolderName = customSubFolderName,
                                    UserId = dataCheck.Id ,// Kullanıcının ID'sini kullan
                                    DateTime = DateTime.Now,
                                    FolderDescription = description
                                };
                                await context.Folders.AddAsync(folder);
                                await context.SaveChangesAsync(); // Değişiklikleri kaydet
                            }
                            else
                            {
                                await context.Users.AddAsync(data);
                                await context.SaveChangesAsync(); // Değişiklikleri kaydet
                                var folder = new Folder
                                {
                                    FolderName = customSubFolderName,
                                    UserId = data.Id ,// Kullanıcının ID'sini kullan
                                     DateTime= DateTime.Now,
                                     FolderDescription = description
                                };
                                await context.Folders.AddAsync(folder);
                                await context.SaveChangesAsync();
                            }
                            // Klasörü ekle
                        }


                    }
                    else
                    {
                        Console.WriteLine("Geçersiz dosya yolu girdiniz.");
                    }
                    #endregion


                }
                else if (action == "clone")
                {
                    #region File Clone
                    Console.WriteLine("Kopyalanacak klasörün adını girin (örneğin, alt klasör):");
                    string subFolderName = Console.ReadLine();

                    string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string uploadsFolder = Path.Combine(projectDirectory, "Uploads");

                    // Kullanıcının emaili ile klasör yolu
                    string userFolder = Path.Combine(uploadsFolder, mail, subFolderName);

                    if (!Directory.Exists(userFolder))
                    {
                        Console.WriteLine($"{subFolderName} klasörü mevcut değil!");
                    }
                    else
                    {
                        // Kopyalama hedef yolunu al
                        Console.WriteLine("Kopyalamak istediğiniz hedef klasör yolunu girin:");
                        string targetDirectory = Console.ReadLine();

                        if (Directory.Exists(targetDirectory))
                        {
                            try
                            {
                                // Klasörü kopyala
                                CopyDirectory(userFolder, targetDirectory);
                                Console.WriteLine($"{subFolderName} klasörü başarıyla {targetDirectory} dizinine kopyalandı.");
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
                            Console.WriteLine("Geçersiz hedef klasör yolu girdiniz.");
                        }
                    }
                    #endregion
                }
                else
                {
                    Console.WriteLine("Geçersiz işlem seçimi!");
                }
            }
        }

        static void CopyDirectory(string sourceDir, string destinationDir)
        {
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

            foreach (string file in Directory.GetFiles(sourceDir))
            {
                if (file.Contains(@".git\"))
                {
                    continue;
                }

                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationDir, fileName);
                File.Copy(file, destFile, true);
                Console.WriteLine($"{fileName} dosyası kopyalandı.");
            }

            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                if (subDir.Contains(@".git\"))
                {
                    continue;
                }

                string subDirName = Path.GetFileName(subDir);
                string destSubDir = Path.Combine(destinationDir, subDirName);
                CopyDirectory(subDir, destSubDir);
            }
        }
    }
}
