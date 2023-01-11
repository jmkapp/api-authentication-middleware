﻿using AuthenticationApi.Model;

namespace AuthenticationApi.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(string userName);
        Task<bool> Add(User newUser);
        Task<bool> Delete(string userName);
    }
}
