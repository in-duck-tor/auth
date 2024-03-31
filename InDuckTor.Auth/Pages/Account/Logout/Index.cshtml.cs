using IdentityModel;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InDuckTor.Auth.Pages.Account.Logout
{
    public class Index : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IEventService _events;

        [BindProperty]
        public string? LogoutId { get; set; }

        public Index(IIdentityServerInteractionService interaction, IEventService events)
        {
            _interaction = interaction;
            _events = events;
        }

        public async Task<IActionResult> OnGet(string? logoutId)
        {
            LogoutId = logoutId;

            return await OnPost();
        }

        public async Task<IActionResult> OnPost()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                LogoutId ??= await _interaction.CreateLogoutContextAsync();
                var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

                await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
                //Telemetry.Metrics.UserLogout(idp);

                //// if it's a local login we can ignore this workflow
                //if (idp != null && idp != Duende.IdentityServer.IdentityServerConstants.LocalIdentityProvider)
                //{
                //    // we need to see if the provider supports external logout
                //    if (await HttpContext.GetSchemeSupportsSignOutAsync(idp))
                //    {
                //        var url = Url.Page("/Account/Logout/Loggedout", new { logoutId = LogoutId });
                //        return SignOut(new AuthenticationProperties { RedirectUri = url }, idp);
                //    }
                //}
            }

            var context = await _interaction.GetLogoutContextAsync(LogoutId);
            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            return Redirect(context.PostLogoutRedirectUri);
        }
    }
}
