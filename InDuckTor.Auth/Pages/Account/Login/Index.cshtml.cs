using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using InDuckTor.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InDuckTor.Auth.Pages.Account.Login
{
    public class Index : PageModel
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly ICredentialsService _credentialsService;

        public ViewModel View { get; set; } = default!;

        [BindProperty]
        public InputModel Input { get; set; } = default!;

        public Index(
            IIdentityServerInteractionService interaction,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events, ICredentialsService credentialsService)
        {
            _interaction = interaction;
            _schemeProvider = schemeProvider;
            _events = events;
            _credentialsService = credentialsService;
        }

        public async Task<IActionResult> OnGet(string? returnUrl)
        {
            await BuildModelAsync(returnUrl);

            if (View.IsExternalLoginOnly)
            {
                return RedirectToPage("/ExternalLogin/Challenge", new { scheme = View.ExternalLoginScheme, returnUrl });
            }

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

            if (ModelState.IsValid)
            {
                var userId = await _credentialsService.CheckCredentials(new Models.CredentialsDto(Input.Login, Input.Password));

                if (userId != null)
                {
                    await _events.RaiseAsync(new UserLoginSuccessEvent(Input.Login, userId.ToString(), Input.Login, clientId: context?.Client.ClientId));

                    var isuser = new IdentityServerUser(userId.ToString())
                    {
                        DisplayName = Input.Login
                    };

                    await HttpContext.SignInAsync(isuser);

                    if (context != null)
                    {
                        ArgumentNullException.ThrowIfNull(Input.ReturnUrl, nameof(Input.ReturnUrl));


                        //if (context.IsNativeClient())
                        //{
                        //    // The client is native, so this change in how to
                        //    // return the response is for better UX for the end user.
                        //    return this.LoadingPage(Input.ReturnUrl);
                        //}

                        return Redirect(Input.ReturnUrl ?? "~/");
                    }

                    if (Url.IsLocalUrl(Input.ReturnUrl))
                    {
                        return Redirect(Input.ReturnUrl);
                    }
                    else if (string.IsNullOrEmpty(Input.ReturnUrl))
                    {
                        return Redirect("~/");
                    }
                    else
                    {
                        throw new ArgumentException("invalid return URL");
                    }
                }

                // Check is Active

                await _events.RaiseAsync(new UserLoginFailureEvent(Input.Login, LoginOptions.InvalidCredentialsErrorMessage, clientId: context?.Client.ClientId));
                ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
            }

            await BuildModelAsync(Input.ReturnUrl);
            return Page();
        }

        private async Task BuildModelAsync(string? returnUrl)
        {

            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            Input = new InputModel
            {
                ReturnUrl = returnUrl,
            };


            if (context?.IdP != null && await _schemeProvider.GetSchemeAsync(context.IdP) != null)
            {
                var local = context.IdP == IdentityServer4.IdentityServerConstants.LocalIdentityProvider;

                View = new ViewModel
                {
                    EnableLocalLogin = local,
                };

                Input.Login = context.LoginHint;

                if (!local)
                {
                    View.ExternalProviders = new[] { new ViewModel.ExternalProvider(authenticationScheme: context.IdP) };
                }

                return;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null)
                .Select(x => new ViewModel.ExternalProvider
                (
                    authenticationScheme: x.Name,
                    displayName: x.DisplayName ?? x.Name
                )).ToList();


            var allowLocal = true;
            var client = context?.Client;
            if (client != null)
            {
                allowLocal = client.EnableLocalLogin;
                if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Count != 0)
                {
                    providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                }
            }

            View = new ViewModel
            {
                AllowRememberLogin = LoginOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && LoginOptions.AllowLocalLogin,
                ExternalProviders = providers.ToArray()
            };
        }
    }
}
