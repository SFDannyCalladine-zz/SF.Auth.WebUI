using Microsoft.AspNetCore.Mvc;
using SF.Common.Security.Mvc;

namespace SF.Auth.WebUI.Controllers
{
    [SecurityHeaders]
    public class BaseController : Controller
    {
        public BaseController() { }
    }
}
