using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SF.Auth.WebUI.Models.Account
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter your Email Address")]
        [DisplayName("Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        [Required(ErrorMessage = "Please enter your Password")]
        [DisplayName("Username")]
        public string Username { get; set; }

        public LoginViewModel()
        {
        }

        public LoginViewModel(string returnUrl)
        {
            ReturnUrl = returnUrl;
        }
    }
}