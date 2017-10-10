using System;
using System.ComponentModel.DataAnnotations;

namespace SF.Auth.WebUI.Models.Account
{
    public class PasswordResetViewModel
    {
        [Compare("NewPassword", ErrorMessage = "Confirm New Password must be the same as New Password")]
        public string ConfirmNewPassword { get; set; }

        public string Key { get; set; }

        [Required(ErrorMessage = "Please enter a New Password")]
        //[IsValidPassword(ErrorMessage = "Please enter a valid Password")]
        public string NewPassword { get; set; }

        public Guid UserGuid { get; set; }

        public PasswordResetViewModel()
        {
        }

        public PasswordResetViewModel(
            string key,
            Guid userGuid)
        {
            Key = key;
            UserGuid = UserGuid;
        }
    }
}