using CustomAuth.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using System.Security.Claims;

namespace CustomAuth.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;


        public AccountController(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public IActionResult Index()
        {
            return View(_context.UserAccounts.ToList());
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Registration(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserAccount account = new UserAccount();
                account.Email = model.Email;
                account.FirstName = model.FirstName;
                account.LastName = model.LastName;
                account.Password = model.Password;
                account.Username = model.Username;

                try
                {
                    _context.UserAccounts.Add(account);
                    _context.SaveChanges();

                    ModelState.Clear();
                    ViewBag.Message = $"{account.FirstName} {account.LastName} Registered successfully. Please Login";
                    
                }
                catch (DbUpdateException ex)
                {
                    //_context.UserAccounts.Add(account);
                    ModelState.AddModelError("", "Please enter unique Email or Password. ");
                    return View(model);
                }
                return View();
            }
            return View(model);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(LoginVeiwModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccounts.Where(x => (x.Username == model.UsernameOrEmail || x.Email == model.UsernameOrEmail) && x.Password == model.Password).FirstOrDefault();
                if (user != null)
                {
                    //Success, Create a cookie
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.FirstName),
                        new Claim(ClaimTypes.Role, "User"),
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("Securepage");
                }
                else
                {
                    ModelState.AddModelError("", "Username/Email or Password is not correct");
                    return View(model);
                }
             }
            return View(model);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Securepage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }

        [HttpGet]
        public IActionResult ExternalLogin()
        {
            return RedirectToAction("Google", "ExternalAuth");
        }

        [HttpGet]
        public IActionResult MicrosoftLogin()
        {
            return RedirectToAction("Microsoft", "ExternalAuth");
        }

    }
}
