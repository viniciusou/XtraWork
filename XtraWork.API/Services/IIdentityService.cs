namespace XtraWork.API.Services
{
    public interface IIdentityService
    {
         Task<AuthenticationResult> RegisterAsync(string email, string password);
    }
}