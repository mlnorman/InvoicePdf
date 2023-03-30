using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityServer.Configuration;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public class CustomProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // get the subject from the token, which is the user id
            var subId = context.Subject.GetSubjectId();
            
            // find the in memory user form Config.cs and add the claims to the access token.
            // I am adding all claims, but all I really needed was the role
            var user = Config.TestUsers.FirstOrDefault(x => x.SubjectId == subId);

            context.IssuedClaims.AddRange(user.Claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subId = context.Subject.GetSubjectId();
            var user = Config.TestUsers.FirstOrDefault(x => x.SubjectId == subId);

            context.IsActive = user != null;
        }
    }
}
