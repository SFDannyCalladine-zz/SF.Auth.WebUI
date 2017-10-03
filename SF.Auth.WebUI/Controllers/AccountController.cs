using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SF.Auth.DataTransferObjects;
using SF.Auth.Services.Interfaces;
using SF.Auth.Services.Request;
using SF.Auth.WebUI.Models.Account;
using SF.Common.ServiceModels.Response;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SF.Auth.WebUI.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthService _authService;

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

            // issue authentication cookie with subject ID and username
            var userClaims = GenerateUserClaims(response.Entity);

            var claims = new ClaimsPrincipal(new ClaimsIdentity(userClaims));

            await HttpContext.SignInAsync(claims);

            // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
            if (_interaction.IsValidReturnUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }

            return Redirect("~/");
        }

        private IList<Claim> GenerateUserClaims(UserDto user)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim("userid", user.UserId.ToString()));
            claims.Add(new Claim(JwtClaimTypes.Subject, user.UserGuid.ToString()));
            claims.Add(new Claim(JwtClaimTypes.Email, user.Email));
            claims.Add(new Claim(JwtClaimTypes.Name, user.Name));

            return claims;
        }
    }
}