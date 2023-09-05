using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.ContentApi.Cms;
using EPiServer.OpenIDConnect;
using EPiServer.Scheduler;
using EPiServer.Web.Routing;
using IDM.Application.Services;
using Klarna.Common.Configuration;
using Mediachase.Commerce.Anonymous;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace IDM.Application;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostingEnvironment;

    public Startup(IWebHostEnvironment webHostingEnvironment, IConfiguration configuration)
    {
        _webHostingEnvironment = webHostingEnvironment;
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        if (_webHostingEnvironment.IsDevelopment())
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", Path.Combine(_webHostingEnvironment.ContentRootPath, "App_Data"));

            services.Configure<SchedulerOptions>(options => options.Enabled = false);
        }

        services
            .AddCmsAspNetIdentity<ApplicationUser>()
            .AddCommerce()
            .AddFind()
            .AddAdminUserRegistration()
            .AddEmbeddedLocalization<Startup>();

        services.AddMvc()
            .AddNewtonsoftJson(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        services.AddSession();
        services.AddContentDeliveryApi(OpenIDConnectOptionsDefaults.AuthenticationScheme)
            .WithFriendlyUrl();

        services.AddCommerceApi<ApplicationUser>(OpenIDConnectOptionsDefaults.AuthenticationScheme);

        services.AddSingleton<DbContext, ApplicationDbContext<ApplicationUser>>();
        services.AddDbContext<ApplicationDbContext<ApplicationUser>>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("EPiServerDB"))
        );

        services.AddOpenIDConnect<ApplicationUser>(
            useDevelopmentCertificate: true,
            signingCertificate: null,
            encryptionCertificate: null,
            createSchema: true,
            options =>
            {
                //options.RequireHttps = !_webHostingEnvironment.IsDevelopment();
                var application = new OpenIDConnectApplication()
                {
                    ClientId = "postman-client",
                    ClientSecret = "postman",
                    Scopes =
                    {
                        ContentDeliveryApiOptionsDefaults.Scope,
                    }
                };

                // Using Postman for testing purpose.
                // The authorization code is sent to postman after successful authentication.
                application.RedirectUris.Add(new Uri("https://oauth.pstmn.io/v1/callback"));
                options.Applications.Add(application);
                options.AllowResourceOwnerPasswordFlow = true;
            });
        services.AddOpenIDConnectUI();
        services.AddKlarnaCheckout();
        services.Configure<CheckoutConfiguration>("EU", _configuration.GetSection("Klarna:Checkout:EU"));
        services.Configure<CheckoutConfiguration>("US", _configuration.GetSection("Klarna:Checkout:US"));
        services.Configure<CheckoutConfiguration>("DEFAULT", _configuration.GetSection("Klarna:Checkout:US"));
        services.InitializeServices(_configuration);

        services.AddCors(options =>
        {
            options.AddPolicy("AllowLocalhost",
                builder =>
                {
                    builder.WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseAnonymousId();

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("AllowLocalhost");

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSession();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapContent();
        });
    }
}
