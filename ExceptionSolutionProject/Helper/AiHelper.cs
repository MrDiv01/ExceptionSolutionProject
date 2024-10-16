using System.IdentityModel.Tokens.Jwt;

namespace ExceptionSolutionProject.Helper
{
    public static class AiHelper
    {
        public static string GetEmailFromJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // "email" claim'ini alıyoruz
            var email = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;

            return email;
        }
    }
}
