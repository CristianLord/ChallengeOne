using ChallengeOne.Data;
using ChallengeOne.Models;
using ChallengeOne.Repository;
using ChallengeOne.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChallengeOne.Controllers
{
    /// <summary>
    /// This is subscribe view controller
    /// </summary>
    [Authorize]
    public class SubscriberController : Controller
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriberController(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        /// <summary>
        /// Shows current user's subscriptions
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Subscriptions()
        {
            try
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (idUser == 0)
                    return NotFound();

                var list = await _subscriptionRepository.GetAllSubscriptions(idUser);
                if (list == null)
                    return NotFound();

                return View(list);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
        /// Unsubscribed the current user to other
        /// </summary>
        /// <param name="id">ID of the user to unsubcribe</param>
        /// <returns></returns>
        public async Task<IActionResult> Unsubscription(int id)
        {
            try
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (idUser == 0)
                    return NotFound();

                if (!(await _subscriptionRepository.Unsubscription(id, idUser)))
                    return NotFound();

                return RedirectToAction(nameof(Subscriptions));
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
        /// Subscribe the current user to a new user
        /// </summary>
        /// <param name="id">ID of the user to subscribe</param>
        /// <returns></returns>
        public async Task<IActionResult> SubscribeUser(int id)
        {
            try
            {
                var idUser = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
                if (idUser == 0 || id == 0)
                    return NotFound();

                if (!(await _subscriptionRepository.SubscribeTo(idUser, id)))
                    return NotFound();

                return RedirectToAction("Index", "User");
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
