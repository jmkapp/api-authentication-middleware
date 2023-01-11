using AuthenticationApi.Repositories;
using AuthenticationApi.Model;
using Microsoft.AspNetCore.Identity;

namespace AuthenticationApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Get(string userName)
        {
            return await _userRepository.Get(userName);
        }

        public async Task<bool> Add(string userName, string password)
        {
            User newUser = new User()
            {
                UserName = userName
            };

            newUser.PasswordHash = new PasswordHasher<User>().HashPassword(newUser, password);

            return await _userRepository.Add(newUser);
        }

        public async Task<bool> Delete(string userName)
        {
            return await _userRepository.Delete(userName);
        }

        public async Task<bool> VerifyPassword(string userName, string password)
        {
            User user = await Get(userName);

            PasswordVerificationResult verifiedResult = new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, password);

            return verifiedResult == PasswordVerificationResult.Success;
        }
    }
}
