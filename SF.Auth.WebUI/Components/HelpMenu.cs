using Microsoft.AspNetCore.Mvc;
using SF.Auth.Services.Interfaces;
using SF.Auth.WebUI.Models;
using SF.Common.ServiceModels.Response;
using System.Collections.Generic;
using System.Linq;

namespace SF.Auth.WebUI.Components
{
    public class HelpMenu : ViewComponent
    {
        private readonly IHelpService _helpService;

        public HelpMenu(IHelpService helpService)
        {
            _helpService = helpService;
        }

        public IViewComponentResult Invoke()
        {
            var response = _helpService.GetAllLinks();

            if (response.Code != ResponseCode.Success)
            {
                return View(new List<HelpLinkViewModel>());
            }

            var model = response.Entity
                .Select(x =>
                    new HelpLinkViewModel(
                        x.Url,
                        x.LinkText,
                        x.Order)
                        )
                .ToList();

            return View(model);
        }
    }
}