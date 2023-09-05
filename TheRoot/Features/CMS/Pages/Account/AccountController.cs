using System.Net;
using EPiServer.Cms.UI.AspNetIdentity;
using IDM.Application.Services.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using IDM.Shared.Models.Account;

namespace IDM.Application.Features.CMS.Pages.Account
{
    [Microsoft.AspNetCore.Mvc.Route("api/[Controller]")]
    public class AccountController : ApiController
    {
        private readonly IAccountManager _accountManager;

        public AccountController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(string email, string password)
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
                UserName = email,
                PasswordHash = passwordHash,
                NormalizedEmail = email,
                NormalizedUserName = email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await _accountManager.UserManager.CreateAsync(applicationUser);

            return await Task.FromResult(new JsonResult(null));
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route(nameof(GetUserByEmail))]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var createdUser = _accountManager.UserManager.FindByEmailAsync(email);
            return await Task.FromResult(new JsonResult(createdUser));
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(Login))]
        public async Task<IActionResult> Login(AccountModel model)
        {
            var user = _accountManager.UserManager.FindByEmailAsync(model.Email).GetAwaiter().GetResult();
            if (user is null)
            {
                return new JsonResult(null);
            }

            await _accountManager.SignInManager.SignOutAsync();

            var signInResult =
                _accountManager.SignInManager.PasswordSignInAsync(user.UserName, model.Password, model.IsPersistent, false).GetAwaiter().GetResult();

            if (!signInResult.Succeeded)
            {
                return new JsonResult(new
                {
                    StatusCode = 405,
                    Message = @"Login Password does not match"
                });
            }

            var identity = await _accountManager.SignInManager.GenerateUserIdentityAsync(user);
            await _accountManager.SignInManager.SignInWithClaimsAsync(user, true, identity.Claims);

            user.LastLoginDate = DateTime.UtcNow;
            _accountManager.UserManager.UpdateAsync(user).GetAwaiter().GetResult();

            //todo:replace redirect url
            return new JsonResult(new
            {
                StatusCode = 200,
                Data = new
                {
                    UserId = user.Id,
                    Email = user.Email
                },
                RedirectUrl = "/",
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Microsoft.AspNetCore.Mvc.Route(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            if (_accountManager.IsAuthenticated)
            {
                await _accountManager.SignInManager.SignOutAsync();
            }

            return new JsonResult(new
            {
                StatusCode = 200,
                Message = @"Logout Successful",
                RedirectUrl = $"https://google.com",
            });
        }

        [Microsoft.AspNetCore.Mvc.HttpGet]
        [Microsoft.AspNetCore.Mvc.Route(nameof(Render))]
        public IActionResult Render()
        {
            var html = System.IO.File.ReadAllText(@"klarna.html");
            return new ContentResult
            {
                Content = html,
                ContentType = "text/html",
                StatusCode = 200
            };
        }
    }
}
