using EPiServer.Cms.Shell;
using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using Headless.Services;
using Mediachase.Commerce.Anonymous;
using Microsoft.EntityFrameworkCore;

namespace Headless;

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

        //todo:role to cdapi
        services.AddContentDeliveryApi()
            .WithFriendlyUrl();

        services.AddCommerceApi<ApplicationUser>(string.Empty);

        services.AddSingleton<DbContext, ApplicationDbContext<ApplicationUser>>();
        services.AddDbContext<ApplicationDbContext<ApplicationUser>>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("EPiServerDB"))
        );

        services.InitializeServices();

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

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapContent();
        });
    }
}
