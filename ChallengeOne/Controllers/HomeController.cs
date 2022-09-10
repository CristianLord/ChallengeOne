using ChallengeOne.Data;
using ChallengeOne.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace ChallengeOne.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _database;

        public HomeController(DatabaseContext database)
        {
            _database = database;
        }

        /// <summary>
        /// Shows all journals of their subscriptions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index() 
        {
            var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var list = await (from j in _database.Journals
                       where
                       (from s in _database.Subscriptions
                        where s.IdUser == idUser
                        select s.IdSubscribedUser)
                        .Contains(j.IdUser) && j.IdUser != idUser
                       select j).Include("User").OrderByDescending(m => m.UploadDate).ToListAsync();

            return View(list);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}