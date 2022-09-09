using ChallengeOne.Data;
using ChallengeOne.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChallengeOne.Controllers
{
    public class SubscriberController : Controller
    {

        private readonly DatabaseContext _database;

        public SubscriberController(DatabaseContext database)
        {
            _database = database;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Subscribe a new user
        /// </summary>
        /// <param name="id">ID of the user to subscribe</param>
        /// <returns></returns>
        public async Task<IActionResult> SubscribeUser(int id)
        {
            try
            {
                var suscription = new Subscription
                {
                    IdUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)),
                    IdSubscribedUser = id
                };
                await _database.Subscriptions.AddAsync(suscription);
                await _database.SaveChangesAsync();
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return RedirectToAction("Index", "User");
        }
    }
}
