using EPiServer.Cms.UI.AspNetIdentity;
namespace IDM.Application.Services.Identity;

public class AccountManager : IAccountManager
{
    public AccountManager(ApplicationUserManager<ApplicationUser> userManager,
        ApplicationSignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        UserManager = userManager;
        SignInManager = signInManager;
        HttpContextAccessor = httpContextAccessor;
    }

    public ApplicationUserManager<ApplicationUser> UserManager { get; set; }
    public ApplicationSignInManager<ApplicationUser> SignInManager { get; set; }
    public IHttpContextAccessor HttpContextAccessor { get; set; }
    public bool IsAuthenticated => HttpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true };
}