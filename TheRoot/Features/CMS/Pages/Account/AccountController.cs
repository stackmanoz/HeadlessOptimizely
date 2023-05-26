using System.Globalization;
using System.Web.Http;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Framework.Localization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Headless.Features.CMS.Pages.Account
{
    [Microsoft.AspNetCore.Mvc.Route("api/[Controller]")]
    public class AccountController : ApiController
    {
        private readonly ApplicationUserManager<ApplicationUser> _userManager;
        private readonly ApplicationSignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(ApplicationUserManager<ApplicationUser> userManager,
            ApplicationSignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(string email, string userName, string password)
        {
            IPasswordHasher<PasswordHasherOptions> hasher = new PasswordHasher<PasswordHasherOptions>();

            var passwordHash = hasher.HashPassword(new PasswordHasherOptions
            {
                CompatibilityMode = PasswordHasherCompatibilityMode.IdentityV3,
            }, password);

            var applicationUser = new ApplicationUser
            {
                Email = email,
                EmailConfirmed = true,
                LockoutEnabled = true,
                IsApproved = true,
                UserName = userName,
                PasswordHash = passwordHash,
                NormalizedEmail = email,
                NormalizedUserName = userName,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await _userManager.CreateAsync(applicationUser);

            return await Task.FromResult(new JsonResult(null));
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route(nameof(GetUserByEmail))]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var createdUser = _userManager.FindByEmailAsync(email);
            return await Task.FromResult(new JsonResult(createdUser));
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(Login))]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = _userManager.FindByEmailAsync(email).GetAwaiter().GetResult();
            if (user is null)
            {
                return new JsonResult(null);
            }

            await _signInManager.SignOutAsync();

            var signInResult =
                _signInManager.PasswordSignInAsync(user.UserName, password, true, false).GetAwaiter().GetResult();

            if (!signInResult.Succeeded)
            {
                return new JsonResult(new
                {
                    StatusCode = 200,
                    Message = @"Login Password does not match",
                    RedirectUrl = $"no login succeed",
                });
            }

            var identity = await _signInManager.GenerateUserIdentityAsync(user);
            await _signInManager.SignInWithClaimsAsync(user, true, identity.Claims);

            user.LastLoginDate = DateTime.UtcNow;
            _userManager.UpdateAsync(user).GetAwaiter().GetResult();

            //todo:replace redirect url
            return new JsonResult(new
            {
                StatusCode = 200,
                Message = @"Login Successful",
                RedirectUrl = $"https://google.com",
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            if (_httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true })
            {
                await _signInManager.SignOutAsync();
            }

            return new JsonResult(new
            {
                StatusCode = 200,
                Message = @"Logout Successful",
                RedirectUrl = $"https://google.com",
            });
        }
    }
}
