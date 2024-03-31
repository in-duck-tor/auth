using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InDuckTor.Auth.Pages.Account.Logout
{
    public class LoggedOut : PageModel
    {
        private readonly IIdentityServerInteractionService _interactionService;

        public LoggedOutViewModel View { get; set; } = default!;

        public LoggedOut(IIdentityServerInteractionService interactionService)
        {
            _interactionService = interactionService;
        }

        public async Task OnGet(string? logoutId)
        {
            var logout = await _interactionService.GetLogoutContextAsync(logoutId);

            View = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = LogoutOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = String.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout?.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl
            };
        }
    }
}
