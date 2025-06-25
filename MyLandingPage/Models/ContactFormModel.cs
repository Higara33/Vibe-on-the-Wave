using System.ComponentModel.DataAnnotations;

namespace MyLandingPage.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Укажите имя")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите email")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Укажите сообщение")]
        public string Message { get; set; } = string.Empty;
    }
}
