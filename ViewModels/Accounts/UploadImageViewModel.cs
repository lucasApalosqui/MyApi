using System.ComponentModel.DataAnnotations;

namespace BlogAspNet.ViewModels.Accounts
{
    public class UploadImageViewModel
    {
        [Required(ErrorMessage = "Imagem é necessária")]
        public string Base64Image { get; set; }
    }
}
