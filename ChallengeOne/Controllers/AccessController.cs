using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using ChallengeOne.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using ChallengeOne.Repository.Interfaces;
using System.Security.Claims;
using ChallengeOne.Models;

namespace ChallengeOne.Controllers
{
    /// <summary>
    /// This is access view controller
    /// </summary>
    public class AccessController : Controller
    {
        private readonly IAccessRepository _accessRepository;

        public AccessController(IAccessRepository accessRepository)
        {
            _accessRepository = accessRepository;
        }

        public IActionResult Register() => View();

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="register">Registration data.</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!(await _accessRepository.EmailExist(register.Email)))
                    {
                        Models.User user = new()
                        {
                            FirstName = register.FirstName,
                            LastName = register.LastName,
                            Email = register.Email,
                            Password = register.Password
                        };

                        if (await _accessRepository.RegisterUser(user))
                        {
                            //Create a cookie

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                                new Claim(ClaimTypes.Name, user.FirstName)
                            };

                            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(claimsIdentity));

                            //Go to home page
                            return RedirectToAction("Index", "Home");
                        }
                            return View();
                    }

                    ModelState.AddModelError("", "Email already exists");
                }
                return View();
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /// <summary>
        /// Verify if there's a logged in user
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Log in a user
        /// </summary>
        /// <param name="loginModel">Login Data</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login loginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _accessRepository.IsValid(loginModel))
                    {
                        var user = await _accessRepository.LoginUser(loginModel);

                        if (user == null)
                            return NotFound();

                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.FirstName)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Password or Email incorrects.");
                    }
                }
                return View(loginModel);
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }
        
        /// <summary>
        /// Sign out a user
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction(nameof(Login));
            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
    }
}
