using AuthenticationApi.Model;
using AuthenticationApi.Services;
using AuthenticationApi.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{userName}")]
        public async Task<UserViewModel> Get(string userName)
        {
            User user =  await _userService.Get(userName);

            UserViewModel userViewModel = new UserViewModel()
            {
                UserName = user.UserName != null ? user.UserName : string.Empty,
                Password = user.PasswordHash != null ? user.PasswordHash : String.Empty
            };

            return userViewModel;
        }

        [HttpPost("Add")]
        public async Task<bool> Add(UserViewModel user)
        {
            try
            {
                return await _userService.Add(user.UserName, user.Password);
            }
            catch (Exception)
            {
                return false;
            }
        }

        [HttpDelete("{userName}")]
        public async Task<bool> Delete(string userName)
        {
            try
            {
                return await _userService.Delete(userName);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
