using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Informe o Email!")]
        [EmailAddress(ErrorMessage = "Email Inválido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe a senha!")]
        public string Password { get; set; }
    }
}
