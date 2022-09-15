using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChallengeOne.Repository
{
    /// <inheritdoc/>
    public class AccessRepository : IAccessRepository
    {
        private readonly DatabaseContext _database;

        public AccessRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<bool> EmailExist(string email)
        {
            return await _database.Users.Where(item => item.Email == email).FirstOrDefaultAsync() != null;
        }

        public async Task<User> LoginUser(Login login)
        {
            try
            {
                var user = await _database.Users.Where(item => item.Email == login.Email).FirstOrDefaultAsync();
                return user;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public async Task<bool> RegisterUser(User user)
        {
            try
            {
                //Create a new user
                await _database.Users.AddAsync(user);
                await _database.SaveChangesAsync();

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
        
        public async Task<bool> IsValid(Login login)
        {
            return await _database.Users.Where(
                item => item.Email == login.Email &&
                item.Password == login.Password).FirstOrDefaultAsync() != null;
        }
    }
}
