using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Security.Claims;
using IdentityServer4.Extensions;

namespace InDuckTor.Auth.Services
{
    public class IdentityClaimsProfileService : IProfileService
    {
        private readonly IUserHttpClient _userHttpClient;

        public IdentityClaimsProfileService(IUserHttpClient userHttpClient)
        {
            _userHttpClient = userHttpClient;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            if (sub is null)
            {
                return;
            }

            var user = await _userHttpClient.GetUserShortInfo(sub);

            if (user is null) { 
                return; 
            }

            var claims = new List<Claim>();
            claims.Add(new Claim("account_type", "client"));
            claims.Add(new Claim("login", user.Login));

            user.Roles.ToList().ForEach(r => {
                claims.Add(new Claim("roles", r));
            });

            user.Permissions.ToList().ForEach(p => {
                claims.Add(new Claim("permissions", p));
            });

            context.IssuedClaims = claims;
           
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userHttpClient.GetUserShortInfo(sub);
            context.IsActive = user != null ? user.IsActive : false ;
        }
    }
}
