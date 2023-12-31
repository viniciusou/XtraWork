using System.ComponentModel.DataAnnotations;

namespace XtraWork.API.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}