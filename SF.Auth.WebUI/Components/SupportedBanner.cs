using Microsoft.AspNetCore.Mvc;

namespace SF.Auth.WebUI.Components
{
    public class SupportedBanner : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}