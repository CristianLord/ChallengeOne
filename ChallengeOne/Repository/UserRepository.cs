using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChallengeOne.Repository
{
    /// <inheritdoc/>
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _database;

        public UserRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            IEnumerable<User> users = await _database.Users.ToListAsync();
            return users;
        }

        public async Task<List<UserViewModel>> GetAllWithoutSubscriptions(int idUser)
        {
            try
            {
                var list = await (from user in _database.Users
                                  where
                            !(from s in _database.Subscriptions
                              where s.IdUser == idUser
                              select s.IdSubscribedUser)
                              .Contains(user.Id) && user.Id != idUser
                                  select new UserViewModel
                                  {
                                      Id = user.Id,
                                      Name = user.FirstName + " " + user.LastName
                                  }).ToListAsync();
                return list;
            }
            catch
            {
                return null;
            }
        }

        public async Task<UserViewModel> GetViewUser(int idUser)
        {
            try
            {
                var user = await (from u in _database.Users
                                  where u.Id == idUser
                                  select new UserViewModel
                                  {
                                      Id = u.Id,
                                      Name = u.FirstName + " " + u.LastName,
                                      Email = u.Email
                                  }).FirstOrDefaultAsync();

                return user;
            }
            catch
            {
                return null;
            }
        }


        public async Task<User> GetUser(int idUser)
        {
            var user = await _database.Users.Where(u => u.Id == idUser).FirstOrDefaultAsync();
            return user;
        }

    }
}
