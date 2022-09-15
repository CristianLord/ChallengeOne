using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using ChallengeOne.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChallengeOne.Repository
{
    /// <inheritdoc/>
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DatabaseContext _database;

        public SubscriptionRepository(DatabaseContext database)
        {
            _database = database;
        }

        public async Task<bool> SubscribeTo(int idUser, int idSubscribeTo)
        {
            try
            {
                var suscription = new Subscription
                {
                    IdUser = idUser,
                    IdSubscribedUser = idSubscribeTo
                };
                await _database.Subscriptions.AddAsync(suscription);
                return await _database.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<UserViewModel>> GetAllSubscriptions(int idUser)
        {
            var list = await (from u in _database.Users
                              join s in _database.Subscriptions
                              on u.Id equals s.IdSubscribedUser
                              where s.IdUser == idUser
                              select new UserViewModel
                              {
                                  Id = u.Id,
                                  Name = u.FirstName + " " + u.LastName
                              }).ToListAsync();
            return list;
        }

        public async Task<bool> Unsubscription(int idSusbcription, int idUser)
        {
            try
            {
                var subscription = await _database.Subscriptions.Where(s => s.IdUser == idUser && s.IdSubscribedUser == idSusbcription)
                .FirstOrDefaultAsync();
                if (subscription != null)
                {
                    _database.Subscriptions.Remove(subscription);
                    return await _database.SaveChangesAsync() > 0;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
