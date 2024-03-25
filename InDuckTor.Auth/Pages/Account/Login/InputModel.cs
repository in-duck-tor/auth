using System.ComponentModel.DataAnnotations;

namespace InDuckTor.Auth.Pages.Account.Login
{
    public class InputModel
    {
        [Required]
        public string Login { get; set; }       
        [Required]
        public string Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
