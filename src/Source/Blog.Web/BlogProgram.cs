using Blog.Web.Components;
using Blog.Web.Components.Account;
using Blog.Web.Data;
using Core.Framework;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Blog.Web;

public class BlogProgram
{
    public static void Main(string[] args)
    {
        var web = new MicroApp(args);

        web.RegisterApiDefaults();
        web.RegisterCors();

        web.Register(
            builder =>
            {
                builder.Services.AddMigration<ApplicationDbContext>();
                builder.AddNpgsqlDbContext<ApplicationDbContext>("cs-blog", _ => { }, optionsBuilder => optionsBuilder.EnableSensitiveDataLogging());
            },
            app => { });

        web.Register(
            builder =>
            {
                builder.Services
                       .AddRazorComponents()
                       .AddInteractiveServerComponents();

                builder.Services.AddCascadingAuthenticationState();
                builder.Services.AddScoped<IdentityUserAccessor>();
                builder.Services.AddScoped<IdentityRedirectManager>();
                builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

                builder.Services
                       .AddAuthentication(
                            options =>
                            {
                                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                            })
                       .AddIdentityCookies();

                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

                builder.Services
                       .AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                       .AddEntityFrameworkStores<ApplicationDbContext>()
                       .AddSignInManager()
                       .AddDefaultTokenProviders();

                builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
            },
            app =>
            {
                if (app.Environment.IsDevelopment())
                {
                    app.UseMigrationsEndPoint();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();

                app.UseStaticFiles();
                app.UseAntiforgery();

                app.MapRazorComponents<App>()
                   .AddInteractiveServerRenderMode();

                // Add additional endpoints required by the Identity /Account Razor components.
                app.MapAdditionalIdentityEndpoints();
            });

        web.Run();
    }
}
