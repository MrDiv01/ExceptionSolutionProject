using System;
using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using ExpHub.DTOs;
using LoginRegisterAPI.DTOs;
using System.Data.SqlClient;

namespace ExpHub.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private string _connectionString = "Server=ACER;Database=LoginRegisterAppDB;Trusted_Connection=True;TrustServerCertificate=True";

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> LoginCheck(LoginDto loginDto)
        {
            _httpClient.BaseAddress = new Uri("http://localhost:5192/api/auth/");

            var response = await _httpClient.PostAsJsonAsync("login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(jsonResponse);

                var token = tokenResponse?.token?.result;

                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<UserDto> GetDataByEmail(string email)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                UserDto userDto = new UserDto();
                string query = "SELECT * FROM AspNetUsers WHERE Email = @Email";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Kullanıcı bilgilerini burada işleyin
                            Console.WriteLine($"Name: {reader["FullName"]}, Email: {reader["Email"]}");
                            userDto.UserId = reader["Id"].ToString();
                            userDto.UserName = reader["FullName"].ToString();
                            userDto.UserMail = reader["Email"].ToString();
                        }
                        return userDto;
                    }
                }
            }
        }
    }
}