using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SF.Auth.WebUI.Models.Account
{
    public class LoginViewModel
    {
        [Required]
        [DisplayName("Password")]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }

        [Required]
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