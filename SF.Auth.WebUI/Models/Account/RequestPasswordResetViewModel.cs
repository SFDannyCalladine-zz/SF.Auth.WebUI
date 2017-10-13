using System.ComponentModel.DataAnnotations;

namespace SF.Auth.WebUI.Models.Account
{
    public class RequestPasswordResetViewModel
    {
        #region Public Properties

        [Required(ErrorMessage = "Please enter your Email Address")]
        public string EmailAddress { get; set; }

        #endregion Public Properties
    }
}