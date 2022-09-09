
using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Security.Claims;

namespace ChallengeOne.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly DatabaseContext _database;

        public UserController(DatabaseContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Shows all users in database except current one and their subscriptions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var list = await (from user in _database.Users where 
                        !(from s in _database.Subscriptions 
                          where s.IdUser == idUser select s.IdSubscribedUser)
                          .Contains(user.Id) && user.Id != idUser
                         select new UserViewModel
                         {
                             Id = user.Id,
                             Name = user.FirstName + " " + user.LastName
                         }).ToListAsync();

            return View(list);
        }

        /// <summary>
        /// Shows current user's subscriptions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Subscription()
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var list = await (from u in _database.Users
                       join s in _database.Subscriptions
                       on u.Id equals s.IdSubscribedUser
                       where s.IdUser == idUser
                       select new UserViewModel
                       {
                           Id = u.Id,
                           Name = u.FirstName + " " + u.LastName
                       }).ToListAsync();
            return View(list);
        }



        /// <summary>
        /// Unsubscribed the current user to other
        /// </summary>
        /// <param name="id">ID of the user to unsubcribe</param>
        /// <returns></returns>
        public async Task<IActionResult> Unsubscription(int id)
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var subscription = await _database.Subscriptions.Where(s => s.IdUser == idUser && s.IdSubscribedUser == id).FirstOrDefaultAsync();
            if (subscription != null)
            {
                _database.Subscriptions.Remove(subscription);
                await _database.SaveChangesAsync();
            }
                
            return RedirectToAction("Subscription");
        }


        /// <summary>
        /// Shows current user's profile
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Profile()
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var user = await (from u in _database.Users
                              where u.Id == idUser
                              select new UserViewModel
                              {
                                  Id = u.Id,
                                  Name = u.FirstName + " " + u.LastName,
                                  Email = u.Email
                              }).FirstAsync();
            return View(user);
        }
    }
}
