using Microsoft.Extensions.Configuration;

namespace SchoolManagement.Web.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool Login(string username, string password)
        {
            var adminUsername = _configuration["AdminCredentials:Username"];
            var adminPassword = _configuration["AdminCredentials:Password"];

            return username == adminUsername && password == adminPassword;
        }
    }
}