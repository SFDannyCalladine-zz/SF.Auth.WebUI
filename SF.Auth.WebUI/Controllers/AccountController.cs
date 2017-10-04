using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SF.Auth.Services.Interfaces;
using SF.Auth.Services.Request;
using SF.Auth.WebUI.Models.Account;
using SF.Common.ServiceModels.Response;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SF.Auth.WebUI.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IIdentityServerInteractionService _interaction;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IAuthService authService)
            : base()
        {
            _interaction = interaction;
            _authService = authService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel(returnUrl);

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = _authService.ValidateUser(
                new ValidateUserRequest(
                    model.Username,
                    model.Password));

            if (response.Code != ResponseCode.Success)
            {
                ModelState.AddModelError("", "There has been an issue logging in.");
                return View(model);
            }

            AuthenticationProperties props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30)
            };

            var user = response.Entity;

            await HttpContext.SignInAsync(
                user.UserGuid.ToString(),
                user.Name,
                props,
                new Claim("userid", user.UserId.ToString()),
                new Claim(JwtClaimTypes.Email, user.Email));

            // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
            if (_interaction.IsValidReturnUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return Redirect("~/");
        }

        [HttpGet]
        public IActionResult Logout(string logoutId)
        {
            var model = new LogoutViewModel(logoutId);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutViewModel model)
        {
            var context = await _interaction.GetLogoutContextAsync(model.LogoutId);

            var loggedOutModel = new LoggedOutViewModel(
                model.LogoutId,
                context?.ClientId,
                context?.SignOutIFrameUrl,
                context?.PostLogoutRedirectUri);

            await HttpContext.SignOutAsync();

            return View("LoggedOut", loggedOutModel);
        }
    }
}