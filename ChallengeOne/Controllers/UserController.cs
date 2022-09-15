using ChallengeOne.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChallengeOne.Controllers
{
    /// <summary>
    /// This is user view controller
    /// </summary>
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Shows all users in database except current one and their subscriptions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            try
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (idUser == 0)
                    return NotFound();

                var list = await _userRepository.GetAllWithoutSubscriptions(idUser);
                if (list == null)
                    return NotFound();

                return View(list);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }
        
        /// <summary>
        /// Shows current user's profile
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Profile()
        {
            try
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                if (idUser == 0)
                    return NotFound();

                var user = await _userRepository.GetViewUser(idUser);

                if (user == null)
                    return NotFound();

                return View(user);
            }
            catch(Exception e)
            {
                return NotFound(e);
            }
        }
    }
}
