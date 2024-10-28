using CustomAuth.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CustomAuth.Controllers
{
    public class ExternalAuthController : Controller
    {
        private readonly AppDbContext _context;
        [HttpGet]
        public IActionResult Google()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleCallbackAsync()
        {
            var result = await HttpContext.AuthenticateAsync("Google");
            if (result.Succeeded)
            {
                var claims = result.Principal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                // Create a new user account if it doesn't exist
                var user = _context.UserAccounts.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    user = new UserAccount { Email = email, FirstName = name, LastName = "" };
                    _context.UserAccounts.Add(user);
                    _context.SaveChanges();
                }

                // Sign in the user
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Securepage");
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Microsoft()
        {
            return Challenge(new AuthenticationProperties { RedirectUri = "/" }, "Microsoft");
        }

        [HttpGet]
        public async Task<IActionResult> MicrosoftCallback()
        {
            var result = await HttpContext.AuthenticateAsync("Microsoft");
            if (result.Succeeded)
            {
                var claims = result.Principal.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                // Create a new user account if it doesn't exist
                var user = _context.UserAccounts.FirstOrDefault(u => u.Email == email);
                if (user == null)
                {
                    user = new UserAccount { Email = email, FirstName = name, LastName = "" };
                    _context.UserAccounts.Add(user);
                    _context.SaveChanges();
                }

                // Sign in the user
                var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Securepage");
            }
            return RedirectToAction("Index");
        }
    }
}