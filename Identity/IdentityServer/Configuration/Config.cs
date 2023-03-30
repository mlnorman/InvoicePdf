using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Test;
using IdentityModel;
using static Duende.IdentityServer.Models.IdentityResources;
using System.Security.Claims;
using System.Text.Json;
using Duende.IdentityServer.Validation;

namespace IdentityServer.Configuration;

public static class Config
{
    //Place roles in id token.  Used for client UI 
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new OpenId(),
            new Profile(),
            new IdentityResource("roles", new List<string> { "role" })
        };

    // api scope to allow access token to work with the api
    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
            { new ApiScope(name: "InvoicePdfApi", displayName: "Invoice Pdf Api")};

    public static IEnumerable<Client> Clients =>
        new List<Client>() {
            // client for web app
            new Client()
            {
                ClientId = "InvoicePdfClient",
                AllowedGrantTypes = GrantTypes.Code,

                //going to leave this hard coded.  Normally would be in the database
                RedirectUris = { "https://localhost:7164/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7164/signout-callback-oidc" },

                AllowOfflineAccess = true,

                //set oidc scopes
                AllowedScopes = {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "InvoicePdfApi",
                    "roles"
                },

                // secrets should not be handled like this in production
                // should be using something like Azure Key Vault
                ClientSecrets = { new Secret("secret".Sha256()) },

            }
        };

    // Some test users. Used this instead of scaffolding identity or using custom stores
    public static List<TestUser> TestUsers => new List<TestUser>()
    {
        new TestUser() {
            SubjectId = "1",
            Username = "admin",
            Password = "admin",
            IsActive= true,
            Claims =
            {
                new Claim(JwtClaimTypes.Name, "Admin User"),
                new Claim(JwtClaimTypes.Email, "admin@test.com"),
                new Claim("role", "admin")
            }

        },
        new TestUser() {
            SubjectId = "2",
            Username = "readonly",
            Password = "readonly",
            IsActive= true,
            Claims =
            {
                new Claim(JwtClaimTypes.Name, "Read Only User"),
                new Claim(JwtClaimTypes.Email, "readonly@test.com"),
                new Claim("role", "readonly")
            }

        },
        new TestUser() {
            SubjectId = "3",
            Username = "noaccess",
            Password = "noaccess",
            IsActive= true,
            Claims =
            {
                new Claim(JwtClaimTypes.Name, "No Access User"),
                new Claim(JwtClaimTypes.Email, "noaccess@test.com"),
            }
        }
    };
}