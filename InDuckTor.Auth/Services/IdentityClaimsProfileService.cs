using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4;
using System.Security.Claims;
using IdentityServer4.Extensions;

namespace InDuckTor.Auth.Services
{
    public class IdentityClaimsProfileService : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var claims = new List<Claim>();
            claims.Add(new Claim("login", "login"));
            claims.Add(new Claim("account_type", "client"));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            //var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = true;
        }
    }
}
