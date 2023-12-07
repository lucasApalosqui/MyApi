using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O email é obrigatório!")]
        [EmailAddress( ErrorMessage = "Email iválido")]
        public string Email { get; set; }
    }
}
