using Microsoft.AspNetCore.Mvc;

namespace SF.Auth.WebUI.Components
{
    public class SupportedBanner : ViewComponent
    {
        #region Public Methods

        public IViewComponentResult Invoke()
        {
            return View();
        }

        #endregion Public Methods
    }
}