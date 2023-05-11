using System.ComponentModel.DataAnnotations;

namespace TaskManager.ViewModels;

public class LogInViewModel
{
    [Required(ErrorMessage = "Email адрес обязателен")]
    [EmailAddress(ErrorMessage = "Пожалуйста, введите корректный адрес электронной почты")]
    [Display(Name = "Email адрес")]
    public string EmailAddress { get; set; }

    [Display(Name = "Пароль")]
    [Required(ErrorMessage = "Пароль обязателен")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
