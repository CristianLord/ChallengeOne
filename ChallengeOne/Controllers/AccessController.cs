using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ChallengeOne.Data;
using ChallengeOne.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace ChallengeOne.Controllers
{
    public class AccessController : Controller
    {
        private readonly DatabaseContext _database;

        public AccessController(DatabaseContext context)
        {
            _database = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(Register register)
        {
            if (ModelState.IsValid)
            {
                if(!EmailExist(register.Email))
                {
                    try
                    {
                        Models.User user = new()
                        {
                            FirstName = register.FirstName,
                            LastName = register.LastName,
                            Email = register.Email,
                            Password = register.Password
                        };

                        //Create a new user
                        await _database.Users.AddAsync(user);
                        await _database.SaveChangesAsync();

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
                    catch (Exception e)
                    {
                        ViewBag.Error = e.Message;
                    }
                }

                ModelState.AddModelError("", "Email already exists");
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login loginModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (IsValid(loginModel))
                    {
                        var user = _database.Users.Where(item => item.Email == loginModel.Email).First();

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
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }

            }
            return View(loginModel);
        }

        /// <summary>
        /// Check if a user exist
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        private bool IsValid(Login loginModel)
        {
            return _database.Users.Where(
                item => item.Email == loginModel.Email &&
                item.Password == loginModel.Password).FirstOrDefault() != null;
        }

        /// <summary>
        /// Check if a user´s email exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool EmailExist(string email)
        {
            return _database.Users.Where(item => item.Email == email).FirstOrDefault() != null;
        }

        //Sign out
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }
    }
}
