using System.ComponentModel.DataAnnotations;

namespace MyLandingPage.Models
{
    public class ContactFormModel
    {
        [Required(ErrorMessage = "Укажите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите email")]
        [EmailAddress(ErrorMessage = "Неверный формат email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Укажите сообщение")]
        public string Message { get; set; }
    }
}
