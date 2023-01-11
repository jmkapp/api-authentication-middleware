using AuthenticationApi.Model;

namespace AuthenticationApi.Services
{
    public interface IUserService
    {
        Task<User> Get(string userName);
        Task<bool> Add(string userName, string password);
        Task <bool> Delete(string userName);
        Task<bool> VerifyPassword(string userName, string password);
    }
}
