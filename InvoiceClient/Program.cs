using InvoiceClient.Infrastructure;
using InvoiceClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace InvoiceClient
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // locking down Invoices page and leaving index to anyone
            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizePage("/Invoices", "lockdown");
                options.Conventions.AllowAnonymousToPage("/");
            });

            //services
            builder.Services.AddScoped<IInvoiceService, InvoiceService>();

            // configure sql server
            builder.Services.AddDbContext<InvoiceContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging().EnableDetailedErrors();
            });
            
            
            // clear out default claim mappings
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            // oidc configuration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = builder.Configuration.GetValue<string>("Oidc:Authority");

                    options.ClientId = builder.Configuration.GetValue<string>("Oidc:InvoicePdfClient");

                    // secrets should not be handled like this in production
                    // should be using something like Azure Key Vault
                    options.ClientSecret = builder.Configuration.GetValue<string>("Oidc:Secret");

                    options.ResponseType = builder.Configuration.GetValue<string>("Oidc:ResponseType"); ;

                    

                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");

                    // scope for our api
                    options.Scope.Add("InvoicePdfApi");

                    // for refresh token, but will probably ignore refreshing for this.
                    options.Scope.Add("offline_access");

                    // add roles scope and map token roles to claims
                    options.Scope.Add("roles");
                    options.ClaimActions.MapUniqueJsonKey("role", "role");

                    options.GetClaimsFromUserInfoEndpoint = true;

                    // mapping identity server name/role claims to microsofts..
                    options.TokenValidationParameters.NameClaimType = "name";
                    options.TokenValidationParameters.RoleClaimType = "role";

                    // saves id, access and refresh tokens in the cookie
                    options.SaveTokens = true;
                });

            // set policy to lock down the Invoices page
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("lockdown",
                    policy => policy.RequireRole(new string[] { "admin", "readonly" })) ;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}