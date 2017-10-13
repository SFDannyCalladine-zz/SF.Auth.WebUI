using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SF.Auth.Services.Interfaces;
using SF.Auth.WebUI.Models;
using SF.Common.ServiceModels.Response;

namespace SF.Auth.WebUI.Components
{
    public class HelpMenu : ViewComponent
    {
        #region Private Fields

        private readonly IHelpService _helpService;

        #endregion Private Fields

        #region Public Constructors

        public HelpMenu(IHelpService helpService)
        {
            _helpService = helpService;
        }

        #endregion Public Constructors

        #region Public Methods

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

        #endregion Public Methods
    }
}