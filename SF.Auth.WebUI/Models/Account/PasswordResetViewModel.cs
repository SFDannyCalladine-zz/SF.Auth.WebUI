using System;
using System.ComponentModel.DataAnnotations;

namespace SF.Auth.WebUI.Models.Account
{
    public class PasswordResetViewModel
    {
        #region Public Properties

        [Compare("NewPassword", ErrorMessage = "Confirm New Password must be the same as New Password")]
        public string ConfirmNewPassword { get; set; }

        public Guid Key { get; set; }

        [Required(ErrorMessage = "Please enter a New Password")]
        //[IsValidPassword(ErrorMessage = "Please enter a valid Password")]
        public string NewPassword { get; set; }

        public Guid UserGuid { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public PasswordResetViewModel()
        {
        }

        public PasswordResetViewModel(
            Guid key,
            Guid userGuid)
        {
            Key = key;
            UserGuid = UserGuid;
        }

        #endregion Public Constructors
    }
}