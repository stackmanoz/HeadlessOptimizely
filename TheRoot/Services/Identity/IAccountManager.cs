using EPiServer.Cms.UI.AspNetIdentity;

namespace IDM.Application.Services.Identity
{
    public interface IAccountManager
    {
        ApplicationUserManager<ApplicationUser> UserManager { get; set; }
        ApplicationSignInManager<ApplicationUser> SignInManager { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }
        bool IsAuthenticated { get; }
    }
}
